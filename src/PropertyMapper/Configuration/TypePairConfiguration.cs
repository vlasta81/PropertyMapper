using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using PropertyMapper.Core;

namespace PropertyMapper.Configuration
{
    /// <summary>
    /// Fluent per-property configuration for a <typeparamref name="TIn"/>→<typeparamref name="TOut"/> mapping pair.
    /// Use <see cref="PropMap.Configure{TIn,TOut}()"/> to obtain an instance.
    /// </summary>
    /// <typeparam name="TIn">Source type.</typeparam>
    /// <typeparam name="TOut">Target type.</typeparam>
    public sealed class TypePairConfiguration<TIn, TOut> : ITypePairConfiguration
    {
        /// <summary>
        /// Describes an expression-based custom binding registered via
        /// <see cref="MapFromExpression{TProp}"/>.
        /// Stored as a <see cref="LambdaExpression"/> so it can be inlined into an expression tree
        /// by <see cref="ExpressionProjectionBuilder"/>.
        /// </summary>
        internal sealed record ExpressionBinding(System.Reflection.PropertyInfo Target, LambdaExpression Mapping);

        /// <summary>Describes a context-aware setter registered via <see cref="MapFromWithContext{TCtx,TProp}"/>.</summary>
        internal sealed record ContextSetter(Type ContextType, Delegate Setter);

        /// <summary>Names of target properties excluded from IL-generated mapping via <see cref="Ignore{TProp}"/>.</summary>
        private readonly HashSet<string> _ignoredProperties = new(StringComparer.Ordinal);
        /// <summary>Custom setter actions registered via <see cref="MapFrom{TProp}(Expression{Func{TOut,TProp}},Func{TIn,TProp})"/> or
        /// <see cref="MapFromExpression{TProp}"/>; applied after the base IL mapping completes.</summary>
        private readonly List<Action<TIn, TOut>> _customSetters = [];
        /// <summary>Expression-based bindings registered via <see cref="MapFromExpression{TProp}"/>;
        /// consumed by <see cref="ExpressionProjectionBuilder"/> to inline custom logic into projection trees.</summary>
        private readonly List<ExpressionBinding> _expressionBindings = [];
        /// <summary>Post-map hooks registered via <see cref="AfterMap"/>; invoked after all property copies and custom setters.</summary>
        private readonly List<Action<TIn, TOut>> _afterMapActions = [];
        /// <summary>Context-aware setters registered via <see cref="MapFromWithContext{TCtx,TProp}"/>; invoked per-call with a caller-supplied context.</summary>
        private readonly List<ContextSetter> _contextSetters = [];
        /// <summary>When <see langword="true"/>, <see cref="PropMapBuilder.Build"/> auto-registers a reverse mapping.</summary>
        private bool _reverseMap;
        /// <summary>Lazily populated cache of compiled context delegates keyed by context type. Access is protected by <see cref="PropMap._compileLock"/>.</summary>
        private Dictionary<Type, object?>? _contextDelegateCache;

        /// <summary>
        /// Excludes the specified target property from the IL-generated mapping.
        /// The property on <typeparamref name="TOut"/> will retain its default value after mapping.
        /// </summary>
        /// <typeparam name="TProp">Property type.</typeparam>
        /// <param name="property">Expression selecting the target property to ignore (e.g. <c>x =&gt; x.Name</c>).</param>
        /// <returns>This instance for method chaining.</returns>
        /// <exception cref="ArgumentException">When the expression does not resolve to a simple property access.</exception>
        public TypePairConfiguration<TIn, TOut> Ignore<TProp>(Expression<Func<TOut, TProp>> property)
        {
            ArgumentNullException.ThrowIfNull(property);
            _ignoredProperties.Add(GetPropertyName(property));
            return this;
        }

        /// <summary>
        /// Maps a target property using a custom value factory, bypassing the default name-match logic.
        /// The property is first excluded from IL emission (as if <see cref="Ignore{TProp}"/> were called),
        /// then the compiled setter is invoked after the base mapping completes.
        /// </summary>
        /// <typeparam name="TProp">Property type.</typeparam>
        /// <param name="target">Expression selecting the target property (e.g. <c>x =&gt; x.FullName</c>).</param>
        /// <param name="mapping">Factory that derives the value from a <typeparamref name="TIn"/> source instance.</param>
        /// <returns>This instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="mapping"/> is null.</exception>
        /// <exception cref="ArgumentException">When the expression does not resolve to a simple property access.</exception>
        public TypePairConfiguration<TIn, TOut> MapFrom<TProp>(Expression<Func<TOut, TProp>> target, Func<TIn, TProp> mapping)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(mapping);
            ClosureInspector.GuardAgainstClosureCapture(mapping, nameof(MapFrom));

            string propName = GetPropertyName(target);

            // Compile a typed setter via Expression.Assign to avoid per-call reflection overhead.
            ParameterExpression tgtParam = Expression.Parameter(typeof(TOut), "tgt");
            ParameterExpression valParam = Expression.Parameter(typeof(TProp), "val");
            BinaryExpression assign = Expression.Assign(Expression.Property(tgtParam, propName), valParam);
            Action<TOut, TProp> setter = Expression.Lambda<Action<TOut, TProp>>(assign, tgtParam, valParam).Compile();

            // Exclude from IL-generated mapping; apply custom setter post-construction instead.
            _ignoredProperties.Add(propName);
            _customSetters.Add((src, tgt) => setter(tgt, mapping(src)));
            return this;
        }

        /// <summary>
        /// Maps a target property using a custom expression-based factory, suitable for both regular mapping
        /// and <see cref="System.Linq.IQueryable{T}"/> projection (e.g. EF Core).
        /// Unlike <see cref="MapFrom{TProp}(Expression{Func{TOut,TProp}}, Func{TIn,TProp})"/>,
        /// the factory is expressed as an <see cref="Expression{TDelegate}"/> so that LINQ providers
        /// can translate it server-side.
        /// </summary>
        /// <typeparam name="TProp">Property type.</typeparam>
        /// <param name="target">Expression selecting the target property (e.g. <c>x =&gt; x.TotalPrice</c>).</param>
        /// <param name="mapping">Expression factory that derives the value from a <typeparamref name="TIn"/> source instance.</param>
        /// <returns>This instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="mapping"/> is null.</exception>
        /// <exception cref="ArgumentException">When the expression does not resolve to a simple property access,
        /// or the target property is not found on <typeparamref name="TOut"/>.</exception>
        public TypePairConfiguration<TIn, TOut> MapFromExpression<TProp>(Expression<Func<TOut, TProp>> target, Expression<Func<TIn, TProp>> mapping)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(mapping);

            string propName = GetPropertyName(target);
            System.Reflection.PropertyInfo targetProp = typeof(TOut).GetProperty(propName) ?? throw new ArgumentException($"Property '{propName}' not found on type '{typeof(TOut).Name}'.", nameof(target));

            // Compile a typed setter via Expression.Assign for non-IQueryable use.
            ParameterExpression tgtParam = Expression.Parameter(typeof(TOut), "tgt");
            ParameterExpression valParam = Expression.Parameter(typeof(TProp), "val");
            BinaryExpression assign = Expression.Assign(Expression.Property(tgtParam, propName), valParam);
            Action<TOut, TProp> setter = Expression.Lambda<Action<TOut, TProp>>(assign, tgtParam, valParam).Compile();
            Func<TIn, TProp> compiled = mapping.Compile();

            // Exclude from IL-generated mapping; apply compiled setter post-construction.
            _ignoredProperties.Add(propName);
            _customSetters.Add((src, tgt) => setter(tgt, compiled(src)));

            // Store the raw expression so ExpressionProjectionBuilder can inline it.
            _expressionBindings.Add(new ExpressionBinding(targetProp, mapping));
            return this;
        }

        /// <summary>Gets whether any custom setters or post-map hooks have been registered.</summary>
        internal bool HasCustomMappings => _customSetters.Count > 0 || _afterMapActions.Count > 0;

        /// <summary>Gets the expression-based custom bindings for use in projection expression trees.</summary>
        internal IReadOnlyList<ExpressionBinding> ExpressionBindings => _expressionBindings;

        /// <summary>Gets whether <see cref="ReverseMap"/> was called on this configuration.</summary>
        internal bool IsReverseMapEnabled => _reverseMap;

        /// <summary>Gets the ignored property names for use when building a reverse mapping.</summary>
        internal IReadOnlySet<string> IgnoredProperties => _ignoredProperties;

        /// <summary>Gets the context-aware setter descriptors registered via <see cref="MapFromWithContext{TCtx,TProp}"/>.</summary>
        internal IReadOnlyList<ContextSetter> ContextSetters => _contextSetters;

        /// <summary>
        /// Registers a post-map hook that is invoked after all property copies and custom
        /// <see cref="MapFrom{TProp}(Expression{Func{TOut, TProp}}, Func{TIn, TProp})"/> setters
        /// have completed. Use it to apply cross-property logic or validation on the finished target.
        /// </summary>
        /// <param name="action">Action receiving the source and the fully populated target.</param>
        /// <returns>This instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="action"/> is <see langword="null"/>.</exception>
        public TypePairConfiguration<TIn, TOut> AfterMap(Action<TIn, TOut> action)
        {
            ArgumentNullException.ThrowIfNull(action);
            ClosureInspector.GuardAgainstClosureCapture(action, nameof(AfterMap));
            _afterMapActions.Add(action);
            return this;
        }

        /// <summary>
        /// Instructs <see cref="PropMapBuilder.Build"/> to automatically register a
        /// <typeparamref name="TOut"/>→<typeparamref name="TIn"/> mapping alongside this
        /// <typeparamref name="TIn"/>→<typeparamref name="TOut"/> registration.
        /// Only name-matched properties and <see cref="Ignore{TProp}"/> exclusions are mirrored;
        /// <see cref="MapFrom{TProp}(Expression{Func{TOut,TProp}}, Func{TIn,TProp})"/> factories
        /// cannot be automatically inverted.
        /// </summary>
        /// <returns>This instance for method chaining.</returns>
        public TypePairConfiguration<TIn, TOut> ReverseMap()
        {
            _reverseMap = true;
            return this;
        }

        /// <summary>
        /// Maps a target property using a context-aware factory.
        /// Unlike <see cref="MapFrom{TProp}(Expression{Func{TOut,TProp}}, Func{TIn,TProp})"/>,
        /// the factory receives both the source object and a per-call context value of type
        /// <typeparamref name="TCtx"/> supplied to
        /// <see cref="PropMap.MapWithContext{TIn,TOut,TCtx}(TIn,TCtx)"/>.
        /// The context is <b>not</b> compiled into the IL delegate — it is passed on each call,
        /// making this pattern safe for per-request state (exchange rates, tenant settings, etc.).
        /// </summary>
        /// <typeparam name="TCtx">Context type supplied per call.</typeparam>
        /// <typeparam name="TProp">Property type.</typeparam>
        /// <param name="target">Expression selecting the target property.</param>
        /// <param name="mapping">Factory receiving source and context, returning the property value.</param>
        /// <returns>This instance for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="mapping"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">When the expression does not resolve to a simple property access.</exception>
        public TypePairConfiguration<TIn, TOut> MapFromWithContext<TCtx, TProp>(Expression<Func<TOut, TProp>> target, Func<TIn, TCtx, TProp> mapping)
        {
            ArgumentNullException.ThrowIfNull(target);
            ArgumentNullException.ThrowIfNull(mapping);
            ClosureInspector.GuardAgainstSingletonContext<TCtx>();

            string propName = GetPropertyName(target);

            ParameterExpression tgtParam = Expression.Parameter(typeof(TOut), "tgt");
            ParameterExpression valParam = Expression.Parameter(typeof(TProp), "val");
            BinaryExpression assign = Expression.Assign(Expression.Property(tgtParam, propName), valParam);
            Action<TOut, TProp> setter = Expression.Lambda<Action<TOut, TProp>>(assign, tgtParam, valParam).Compile();

            // Exclude from base IL mapping; the value will be supplied per-call via context.
            _ignoredProperties.Add(propName);

            Action<TIn, TOut, TCtx> contextSetter = (src, tgt, ctx) => setter(tgt, mapping(src, ctx));
            _contextSetters.Add(new ContextSetter(typeof(TCtx), contextSetter));
            return this;
        }

        /// <summary>
        /// Returns a new <see cref="MappingPlan"/> with all ignored properties removed from the binding list.
        /// Idempotent: returns the original plan unchanged when no bindings are affected.
        /// </summary>
        internal MappingPlan ApplyToPlan(MappingPlan plan)
        {
            if (_ignoredProperties.Count == 0)
                return plan;

            ReadOnlySpan<PropertyBinding> bindings = plan.Bindings;

            // Single-pass: collect kept bindings directly.
            List<PropertyBinding> filtered = new(bindings.Length);
            for (int i = 0; i < bindings.Length; i++)
            {
                if (!_ignoredProperties.Contains(bindings[i].Target.Name))
                    filtered.Add(bindings[i]);
            }

            // Nothing filtered — return same plan (idempotent).
            if (filtered.Count == bindings.Length)
                return plan;

            return new MappingPlan(plan.SourceType, plan.TargetType, CollectionsMarshal.AsSpan(filtered));
        }

        /// <summary>
        /// Returns a new delegate that first invokes <paramref name="inner"/> to create the target instance,
        /// then applies all registered custom setters, then all <see cref="AfterMap"/> hooks, in registration order.
        /// </summary>
        internal Func<TIn, TOut> WrapWithCustomMappings(Func<TIn, TOut> inner)
        {
            // Snapshot to array so the closure cannot observe future mutations to _customSetters.
            Action<TIn, TOut>[] setters = [.. _customSetters];
            Action<TIn, TOut>[] afterActions = [.. _afterMapActions];
            return src =>
            {
                TOut result = inner(src);
                for (int i = 0; i < setters.Length; i++)
                    setters[i](src, result);
                for (int i = 0; i < afterActions.Length; i++)
                    afterActions[i](src, result);
                return result;
            };
        }

        /// <summary>
        /// Returns a new in-place delegate that first invokes <paramref name="inner"/> to copy matching properties,
        /// then applies all registered custom setters, then all <see cref="AfterMap"/> hooks, in registration order.
        /// </summary>
        internal Action<TIn, TOut> WrapIntoWithCustomMappings(Action<TIn, TOut> inner)
        {
            Action<TIn, TOut>[] setters = [.. _customSetters];
            Action<TIn, TOut>[] afterActions = [.. _afterMapActions];
            return (src, tgt) =>
            {
                inner(src, tgt);
                for (int i = 0; i < setters.Length; i++)
                    setters[i](src, tgt);
                for (int i = 0; i < afterActions.Length; i++)
                    afterActions[i](src, tgt);
            };
        }

        /// <summary>
        /// Returns (and caches) a delegate that applies all context-aware setters registered via
        /// <see cref="MapFromWithContext{TCtx,TProp}"/> for context type <typeparamref name="TCtx"/>.
        /// Returns <see langword="null"/> when no matching setters are registered.
        /// The result is cached on first call; subsequent calls for the same <typeparamref name="TCtx"/> are allocation-free.
        /// </summary>
        internal Action<TIn, TOut, TCtx>? BuildContextDelegate<TCtx>()
        {
            Type ctxType = typeof(TCtx);
            if (_contextDelegateCache is not null && _contextDelegateCache.TryGetValue(ctxType, out object? cached))
                return cached as Action<TIn, TOut, TCtx>;

            List<Action<TIn, TOut, TCtx>> matched = [];
            foreach (ContextSetter cs in _contextSetters)
            {
                if (cs.ContextType == typeof(TCtx))
                    matched.Add((Action<TIn, TOut, TCtx>)cs.Setter);
            }

            Action<TIn, TOut, TCtx>? result = null;
            if (matched.Count > 0)
            {
                Action<TIn, TOut, TCtx>[] snapshot = [.. matched];
                result = (src, tgt, ctx) =>
                {
                    for (int i = 0; i < snapshot.Length; i++)
                        snapshot[i](src, tgt, ctx);
                };
            }

            _contextDelegateCache ??= new Dictionary<Type, object?>();
            _contextDelegateCache[ctxType] = result;
            return result;
        }

        /// <summary>
        /// Extracts the property name from a simple member-access lambda expression.
        /// </summary>
        /// <typeparam name="TProp">The type of the property being accessed.</typeparam>
        /// <param name="expression">A lambda in the form <c>x =&gt; x.PropertyName</c>, optionally wrapped in a <see cref="UnaryExpression"/> (e.g. for value-type boxing).</param>
        /// <returns>The unqualified name of the accessed property.</returns>
        /// <exception cref="ArgumentException">Thrown when the lambda body is not a simple property access.</exception>
        private static string GetPropertyName<TProp>(Expression<Func<TOut, TProp>> expression) =>
            expression.Body switch
            {
                MemberExpression m => m.Member.Name,
                UnaryExpression { Operand: MemberExpression m } => m.Member.Name,
                _ => throw new ArgumentException(
                    $"Expression must be a simple property access (e.g. x => x.Name), got: {expression.Body.NodeType}",
                    nameof(expression))
            };

        /// <summary>Adds <paramref name="name"/> directly to the ignored-property set; used by <see cref="ITypePairConfiguration.CreateReverseEntry"/>.</summary>
        internal void AddIgnoredProperty(string name) => _ignoredProperties.Add(name);

        bool ITypePairConfiguration.IsReverseMapEnabled => _reverseMap;

        (TypePairKey, object) ITypePairConfiguration.CreateReverseEntry()
        {
            TypePairKey reverseKey = new TypePairKey(typeof(TOut), typeof(TIn));
            TypePairConfiguration<TOut, TIn> reverseCfg = new();
            foreach (string name in _ignoredProperties)
                reverseCfg.AddIgnoredProperty(name);
            return (reverseKey, reverseCfg);
        }
    }

    /// <summary>
    /// Non-generic interface allowing <see cref="PropMapBuilder"/> to inspect and build
    /// reverse mappings without reflection.
    /// </summary>
    internal interface ITypePairConfiguration
    {
        /// <summary>Gets whether <see cref="TypePairConfiguration{TIn,TOut}.ReverseMap"/> was called on this configuration.</summary>
        bool IsReverseMapEnabled { get; }

        /// <summary>
        /// Creates the <see cref="TypePairKey"/> and configuration object for the reverse
        /// (<c>TOut</c>→<c>TIn</c>) direction, mirroring ignored-property names
        /// from the forward configuration.
        /// </summary>
        /// <returns>
        /// A tuple of the reverse-direction <see cref="TypePairKey"/> and its
        /// untyped <see cref="TypePairConfiguration{TOut,TIn}"/> instance (boxed as <see cref="object"/>).
        /// </returns>
        (TypePairKey ReverseKey, object ReverseCfg) CreateReverseEntry();
    }

}
