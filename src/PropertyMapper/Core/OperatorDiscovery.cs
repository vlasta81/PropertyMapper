using System.Linq.Expressions;
using System.Reflection;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Centralised discovery of user-defined implicit/explicit conversion operators.
    /// Previously the same logic was duplicated in <c>DefaultPlanBuilder</c>,
    /// <c>ConversionEmitter</c>, and <c>ExpressionProjectionBuilder</c>.
    /// </summary>
    internal static class OperatorDiscovery
    {
        /// <summary>
        /// Searches both <paramref name="source"/> and <paramref name="target"/> for a compatible
        /// <c>op_Implicit</c> or <c>op_Explicit</c> conversion operator.
        /// </summary>
        /// <param name="source">The source property type.</param>
        /// <param name="target">The target property type.</param>
        /// <returns>The <see cref="MethodInfo"/> of the operator, or <see langword="null"/> when none is found.</returns>
        internal static MethodInfo? Find(Type source, Type target) => FindOn(source, source, target) ?? FindOn(target, source, target);

        /// <summary>
        /// Builds an <see cref="Expression"/> that applies the user-defined conversion operator from
        /// <paramref name="sourceType"/> to <paramref name="targetType"/>, or falls back to a plain cast
        /// when no operator is found.
        /// </summary>
        /// <param name="sourceExpr">The expression whose value is to be converted.</param>
        /// <param name="sourceType">CLR type of the source expression.</param>
        /// <param name="targetType">Desired CLR type after conversion.</param>
        /// <returns>
        /// <see cref="Expression.Convert(Expression,Type,MethodInfo)"/> when an operator is found;
        /// otherwise a plain <see cref="Expression.Convert(Expression,Type)"/> cast.
        /// </returns>
        internal static Expression BuildConvertExpression(Expression sourceExpr, Type sourceType, Type targetType)
        {
            MethodInfo? method = Find(sourceType, targetType);
            return method is not null ? Expression.Convert(sourceExpr, targetType, method) : Expression.Convert(sourceExpr, targetType);
        }

        /// <summary>
        /// Searches <paramref name="declaringType"/> for a public static <c>op_Implicit</c> or <c>op_Explicit</c>
        /// method that converts from <paramref name="source"/> to <paramref name="target"/>.
        /// </summary>
        /// <param name="declaringType">The type whose static methods are inspected.</param>
        /// <param name="source">Required parameter type of the operator.</param>
        /// <param name="target">Required return type of the operator.</param>
        /// <returns>The matching <see cref="MethodInfo"/>, or <see langword="null"/> when not found.</returns>
        private static MethodInfo? FindOn(Type declaringType, Type source, Type target)
        {
            ReadOnlySpan<string> opNames = ["op_Implicit", "op_Explicit"];
            foreach (string opName in opNames)
            {
                foreach (MemberInfo m in declaringType.GetMember(opName, MemberTypes.Method, BindingFlags.Public | BindingFlags.Static))
                {
                    MethodInfo method = (MethodInfo)m;
                    if (method.ReturnType != target) continue;
                    ParameterInfo[] p = method.GetParameters();
                    if (p.Length == 1 && p[0].ParameterType == source) return method;
                }
            }
            return null;
        }
    }
}
