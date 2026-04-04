using System.Linq.Expressions;
using PropertyMapper.Configuration;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Builds <see cref="Expression{TDelegate}"/> projection trees from <see cref="MappingPlan"/> instances.
    /// The resulting expressions are compatible with <see cref="System.Linq.IQueryable{T}"/> and ORM
    /// frameworks such as EF Core that translate expression trees to SQL.
    /// </summary>
    internal static class ExpressionProjectionBuilder
    {
        /// <summary>
        /// Builds an <see cref="Expression{TDelegate}"/> that projects a <typeparamref name="TIn"/> to a new
        /// <typeparamref name="TOut"/> by copying all property bindings from <paramref name="plan"/>, then
        /// inlining any expression-based custom bindings from <paramref name="customBindings"/>.
        /// Fields listed in <paramref name="maskedFields"/> are omitted from the generated expression.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="plan">Filtered mapping plan (already has ignored properties removed).</param>
        /// <param name="customBindings">Optional expression-based custom bindings registered via
        /// <see cref="TypePairConfiguration{TIn,TOut}.MapFromExpression{TProp}"/>.</param>
        /// <param name="maskedFields">Optional per-request set of target property names to exclude from
        /// the generated expression (e.g. fields the current user is not authorised to read).</param>
        /// <returns>A <c>src =&gt; new TOut { P1 = src.P1, … }</c> expression tree.</returns>
        public static Expression<Func<TIn, TOut>> Build<TIn, TOut>(MappingPlan plan, IReadOnlyList<TypePairConfiguration<TIn, TOut>.ExpressionBinding>? customBindings = null, IReadOnlyCollection<string>? maskedFields = null) where TOut : new()
        {
            ParameterExpression srcParam = Expression.Parameter(typeof(TIn), "src");
            List<MemberBinding> memberBindings = BuildMemberBindings(plan, srcParam, maskedFields);

            if (customBindings is not null)
            {
                foreach (TypePairConfiguration<TIn, TOut>.ExpressionBinding custom in customBindings)
                {
                    // Skip custom bindings that are also masked.
                    if (maskedFields is not null && maskedFields.Contains(custom.Target.Name))
                        continue;

                    // Inline the custom lambda body by substituting its parameter with srcParam.
                    Expression valueExpr = ParameterReplacer.Replace(custom.Mapping.Body, custom.Mapping.Parameters[0], srcParam);
                    memberBindings.Add(Expression.Bind(custom.Target, valueExpr));
                }
            }

            MemberInitExpression memberInit = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindings);
            return Expression.Lambda<Func<TIn, TOut>>(memberInit, srcParam);
        }

        /// <summary>
        /// Iterates all bindings in <paramref name="plan"/> and converts each to a
        /// <see cref="MemberAssignment"/> expression, skipping bindings whose conversion kind
        /// cannot be represented as a member-init expression
        /// and bindings whose target property name appears in <paramref name="maskedFields"/>.
        /// </summary>
        /// <param name="plan">The mapping plan whose bindings to convert.</param>
        /// <param name="sourceRoot">The expression representing the source object (parameter or property access).</param>
        /// <param name="maskedFields">Optional set of target property names to exclude from the result.</param>
        /// <returns>A list of <see cref="MemberBinding"/> instances ready for use in a <see cref="System.Linq.Expressions.MemberInitExpression"/>.</returns>
        private static List<MemberBinding> BuildMemberBindings(MappingPlan plan, Expression sourceRoot, IReadOnlyCollection<string>? maskedFields = null)
        {
            ReadOnlySpan<PropertyBinding> bindings = plan.Bindings;
            List<MemberBinding> result = new(bindings.Length);

            for (int i = 0; i < bindings.Length; i++)
            {
                if (maskedFields is not null && maskedFields.Contains(bindings[i].Target.Name))
                    continue;

                MemberAssignment? assignment = BuildMemberAssignment(bindings[i], sourceRoot);
                if (assignment is not null)
                    result.Add(assignment);
            }

            return result;
        }

        /// <summary>
        /// Builds a single <see cref="MemberAssignment"/> for the given <paramref name="binding"/>,
        /// selecting the correct expression form based on <see cref="PropertyBinding.Conversion"/>.
        /// Returns <see langword="null"/> when the conversion kind cannot be represented as a member-init expression.
        /// </summary>
        /// <param name="binding">The property binding to translate into an expression assignment.</param>
        /// <param name="sourceRoot">The expression representing the source object.</param>
        /// <returns>A <see cref="MemberAssignment"/>, or <see langword="null"/> when the binding is not expressible.</returns>
        private static MemberAssignment? BuildMemberAssignment(PropertyBinding binding, Expression sourceRoot)
        {
            MemberExpression sourceProp = Expression.Property(sourceRoot, binding.Source);

            Expression? valueExpr = binding.Conversion switch
            {
                ConversionKind.Direct or ConversionKind.NullableToNullable => sourceProp,

                ConversionKind.NullableToValue => Expression.Coalesce(sourceProp, Expression.Default(binding.Target.PropertyType)), // src.Prop ?? default(T) — translates to SQL COALESCE.

                ConversionKind.ValueToNullable => Expression.Convert(sourceProp, binding.Target.PropertyType),

                ConversionKind.Operator => OperatorDiscovery.BuildConvertExpression(sourceProp, binding.Source.PropertyType, binding.Target.PropertyType),

                ConversionKind.Nested when binding.NestedPlan is not null => BuildNestedExpression(binding.NestedPlan, sourceProp),

                _ => null  // ConversionKind.None or unrecognised: no expression to emit.
            };

            return valueExpr is not null ? Expression.Bind(binding.Target, valueExpr) : null;
        }

        /// <summary>
        /// Recursively builds a nested <c>new TNestedOut { … }</c> member-init expression.
        /// For reference-type navigation properties a null-guard is emitted so the expression
        /// translates correctly to a LEFT JOIN in SQL.
        /// </summary>
        private static Expression BuildNestedExpression(MappingPlan nestedPlan, Expression sourceNavProp)
        {
            List<MemberBinding> nestedBindings = BuildMemberBindings(nestedPlan, sourceNavProp);
            MemberInitExpression memberInit = Expression.MemberInit(Expression.New(nestedPlan.TargetType), nestedBindings);

            // Wrap in a null-conditional guard only when both nav-prop and target are reference types.
            if (!nestedPlan.SourceType.IsValueType && !nestedPlan.TargetType.IsValueType)
            {
                return Expression.Condition(Expression.Equal(sourceNavProp, Expression.Constant(null, sourceNavProp.Type)), Expression.Constant(null, nestedPlan.TargetType), memberInit);
            }

            return memberInit;
        }

        /// <summary>Replaces one <see cref="ParameterExpression"/> with another expression throughout a tree.</summary>
        private sealed class ParameterReplacer : ExpressionVisitor
        {
            /// <summary>The parameter node to find and replace throughout the expression tree.</summary>
            private readonly ParameterExpression _old;
            /// <summary>The expression that replaces every occurrence of <see cref="_old"/>.</summary>
            private readonly Expression _new;

            /// <summary>
            /// Initializes a new <see cref="ParameterReplacer"/> that substitutes <paramref name="old"/> with <paramref name="new"/>.
            /// </summary>
            /// <param name="old">The parameter expression to replace.</param>
            /// <param name="new">The expression to substitute in its place.</param>
            private ParameterReplacer(ParameterExpression old, Expression @new) => (_old, _new) = (old, @new);

            /// <summary>
            /// Substitutes all occurrences of <paramref name="old"/> in <paramref name="body"/> with <paramref name="new"/>.
            /// </summary>
            /// <param name="body">The expression tree to rewrite.</param>
            /// <param name="old">The parameter node to replace.</param>
            /// <param name="new">The replacement expression.</param>
            /// <returns>A rewritten expression tree with all occurrences of <paramref name="old"/> replaced.</returns>
            public static Expression Replace(Expression body, ParameterExpression old, Expression @new) => new ParameterReplacer(old, @new).Visit(body)!;

            /// <inheritdoc/>
            protected override Expression VisitParameter(ParameterExpression node) => node == _old ? _new : base.VisitParameter(node);
        }
    }

}
