using System.Linq.Expressions;
using PropertyMapper.Configuration;
using PropertyMapper.Core;
using PropertyMapper.Masking;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Projection

        /// <summary>
        /// Returns a cached <see cref="Expression{TDelegate}"/> that projects a <typeparamref name="TIn"/>
        /// to a new <typeparamref name="TOut"/> by copying all matched properties as a
        /// <see cref="System.Linq.Expressions.MemberInitExpression"/>.
        /// The expression can be passed directly to
        /// <see cref="System.Linq.Queryable.Select{TSource,TResult}(System.Linq.IQueryable{TSource}, Expression{Func{TSource,TResult}})"/>
        /// or used with ORM frameworks such as EF Core that translate expression trees to SQL.
        /// Any <see cref="Configure{TIn,TOut}()"/> rules (<see cref="TypePairConfiguration{TIn,TOut}.Ignore{TProp}"/>,
        /// <see cref="TypePairConfiguration{TIn,TOut}.MapFromExpression{TProp}"/>) are applied to the expression.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <returns>The cached <c>src =&gt; new TOut { … }</c> expression.</returns>
        public Expression<Func<TIn, TOut>> GetProjectionExpression<TIn, TOut>() where TOut : new()
        {
            lock (_compileLock)
            {
                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

                if (_projectionCache.TryGetValue(key, out object? cached))
                    return (Expression<Func<TIn, TOut>>)cached;

                (MappingPlan plan, TypePairConfiguration<TIn, TOut>? config) = GetOrBuildPlan<TIn, TOut>(key);

                Expression<Func<TIn, TOut>> expr = ExpressionProjectionBuilder.Build(plan, config?.ExpressionBindings);

                _projectionCache[key] = expr;
                return expr;
            }
        }

        /// <summary>
        /// Projects an <see cref="System.Linq.IQueryable{T}"/> using the compiled expression tree for
        /// <typeparamref name="TIn"/>→<typeparamref name="TOut"/>.
        /// The expression is cached after the first call so subsequent calls are fast.
        /// Compatible with EF Core and any LINQ provider that translates expression trees.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">The queryable to project.</param>
        /// <returns>A new <see cref="System.Linq.IQueryable{T}"/> with the projection applied.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
        public IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            return source.Select(GetProjectionExpression<TIn, TOut>());
        }

        /// <summary>
        /// Returns a per-request (uncached) <see cref="Expression{TDelegate}"/> that projects
        /// <typeparamref name="TIn"/> to <typeparamref name="TOut"/>, omitting every field
        /// listed in <paramref name="mask"/> from the <c>MemberInitExpression</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Unlike the parameterless <see cref="GetProjectionExpression{TIn,TOut}()"/> overload,
        /// this result is <b>not cached</b> because the excluded field set may differ per request
        /// (e.g. per user role). The underlying <see cref="Core.MappingPlan"/> is still reused
        /// from the internal cache.
        /// </para>
        /// <para>
        /// When passed to an ORM such as EF Core, masked fields are excluded from the SQL
        /// <c>SELECT</c> clause so sensitive data is never loaded from the database.
        /// </para>
        /// </remarks>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="mask">Per-request field mask; its <see cref="IFieldMask{T}.ExcludedFields"/> are
        /// omitted from the generated expression tree.</param>
        /// <returns>An uncached <c>src =&gt; new TOut { … }</c> expression with masked fields omitted.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="mask"/> is <see langword="null"/>.</exception>
        public Expression<Func<TIn, TOut>> GetProjectionExpression<TIn, TOut>(IFieldMask<TOut> mask) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(mask);
            lock (_compileLock)
            {
                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

                (MappingPlan plan, TypePairConfiguration<TIn, TOut>? config) = GetOrBuildPlan<TIn, TOut>(key);

                // Not cached — the excluded field set varies per request.
                return ExpressionProjectionBuilder.Build(plan, config?.ExpressionBindings, mask.ExcludedFields);
            }
        }

        /// <summary>
        /// Projects an <see cref="System.Linq.IQueryable{T}"/> excluding fields specified by
        /// <paramref name="mask"/> from the generated SQL <c>SELECT</c> clause.
        /// Suitable for per-request field-level authorisation with EF Core or any LINQ provider
        /// that translates expression trees.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">The queryable to project.</param>
        /// <param name="mask">Per-request field mask applied to the SQL SELECT.</param>
        /// <returns>A new <see cref="System.Linq.IQueryable{T}"/> with masked fields excluded from the query.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="mask"/> is <see langword="null"/>.</exception>
        public IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source, IFieldMask<TOut> mask) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(mask);
            return source.Select(GetProjectionExpression<TIn, TOut>(mask));
        }

        #endregion
    }
}
