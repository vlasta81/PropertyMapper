using PropertyMapper.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertyMapper.Masking
{
    /// <summary>
    /// Per-request field visibility mask that suppresses specific <typeparamref name="T"/> properties
    /// after mapping or excludes them from expression-tree projections.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Register as a <c>Scoped</c> service so that each HTTP request receives an instance
    /// populated with the current user's field-level permissions.
    /// </para>
    /// <para>
    /// <b>In-memory mapping:</b>
    /// Pass to <see cref="PropMap.MapThenApplyMask{TIn,TOut}(TIn, IFieldMask{TOut})"/>.
    /// The singleton compiled delegate maps all fields; the mask zeroes out excluded ones
    /// on the returned instance. Sensitive data is produced and then cleared in-process.
    /// </para>
    /// <para>
    /// <b>IQueryable / EF Core:</b>
    /// Pass to <see cref="PropMap.GetProjectionExpression{TIn,TOut}(IFieldMask{TOut})"/> or
    /// <see cref="PropMap.Project{TIn,TOut}(System.Linq.IQueryable{TIn}, IFieldMask{TOut})"/>.
    /// Excluded fields are omitted from the generated SQL <c>SELECT</c> clause so sensitive
    /// data is never loaded from the database.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The target DTO type whose fields this mask controls.</typeparam>
    public interface IFieldMask<T>
    {
        /// <summary>
        /// Names of <typeparamref name="T"/> properties that should be suppressed after mapping
        /// or excluded from expression-tree projections.
        /// </summary>
        IReadOnlyCollection<string> ExcludedFields { get; }
    }

    /// <summary>
    /// Default <see cref="IFieldMask{T}"/> implementation backed by compile-time expression trees.
    /// Setter-clearing actions are compiled once on construction and reused for every
    /// subsequent <see cref="Apply"/> call, making the hot path allocation-free.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Works only with <b>writable</b> (non-<c>init</c>) properties on mutable class DTOs.
    /// For records or <c>init</c>-only DTOs, use the
    /// <see cref="PropMap.GetProjectionExpression{TIn,TOut}(IFieldMask{TOut})"/> overload instead,
    /// which excludes fields at the query level so they are never populated.
    /// </para>
    /// <para>Non-existent or read-only property names are silently ignored during construction.</para>
    /// </remarks>
    /// <typeparam name="T">The target DTO type.</typeparam>
    public sealed class FieldMask<T> : IFieldMask<T>
    {
        /// <summary>Names of excluded properties; returned by <see cref="ExcludedFields"/>.</summary>
        private readonly IReadOnlyCollection<string> _excludedFields;
        /// <summary>Pre-compiled clearing delegates, one per valid writable excluded property.</summary>
        private readonly Action<T>[] _clearers;

        /// <inheritdoc/>
        public IReadOnlyCollection<string> ExcludedFields => _excludedFields;

        /// <summary>
        /// Initializes a new <see cref="FieldMask{T}"/> that will zero out the specified properties.
        /// Clearing actions are compiled to expression-tree delegates immediately.
        /// </summary>
        /// <param name="excludedPropertyNames">
        /// Names of <typeparamref name="T"/> properties to zero out after mapping.
        /// Non-existent or non-writable names are silently ignored.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="excludedPropertyNames"/> is <see langword="null"/>.
        /// </exception>
        public FieldMask(params string[] excludedPropertyNames)
        {
            ArgumentNullException.ThrowIfNull(excludedPropertyNames);
            _excludedFields = (string[])excludedPropertyNames.Clone();
            _clearers = BuildClearers(excludedPropertyNames);
        }

        /// <summary>
        /// Sets each excluded property on <paramref name="instance"/> to its <see langword="default"/> value.
        /// Uses the pre-compiled delegate array — no reflection overhead on the hot path.
        /// </summary>
        /// <param name="instance">The mapped target instance to modify in-place.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="instance"/> is <see langword="null"/>.</exception>
        public void Apply(T instance)
        {
            ArgumentNullException.ThrowIfNull(instance);
            foreach (Action<T> clearer in _clearers)
                clearer(instance);
        }

        /// <summary>
        /// Compiles a clearing <see cref="Action{T}"/> for each writable property whose name
        /// appears in <paramref name="names"/>.
        /// Non-existent and read-only / <c>init</c>-only property names are silently skipped.
        /// </summary>
        /// <param name="names">Property names to compile clearing actions for.</param>
        /// <returns>
        /// An array of compiled clearing delegates (one per valid writable property);
        /// empty when no valid names are found.
        /// </returns>
        private static Action<T>[] BuildClearers(string[] names)
        {
            List<Action<T>> clearers = new(names.Length);
            foreach (string name in names)
            {
                // Skip non-existent or init-only / read-only properties.
                if (!TypeMetadata<T>.PropertyMap.TryGetValue(name, out PropertyInfo? prop) || !prop.CanWrite)
                    continue;

                // Compile: (T instance) => instance.<Name> = default(<PropType>)
                ParameterExpression param = Expression.Parameter(typeof(T), "instance");
                BinaryExpression assign = Expression.Assign(Expression.Property(param, prop), Expression.Default(prop.PropertyType));
                clearers.Add(Expression.Lambda<Action<T>>(assign, param).Compile());
            }
            return [.. clearers];
        }
    }
}
