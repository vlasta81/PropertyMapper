using PropertyMapper.Masking;
using System.Diagnostics.CodeAnalysis;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Null-Safe Mapping

        /// <summary>
        /// Maps <paramref name="source"/> to a new <typeparamref name="TOut"/> instance, or returns
        /// <see langword="null"/> when <paramref name="source"/> is <see langword="null"/>.
        /// Never throws <see cref="ArgumentNullException"/>.
        /// </summary>
        /// <typeparam name="TIn">Source type (must be a reference type).</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source object to map, or <see langword="null"/>.</param>
        /// <returns>A mapped <typeparamref name="TOut"/> instance, or <see langword="null"/> when <paramref name="source"/> is <see langword="null"/>.</returns>
        public TOut? MapOrDefault<TIn, TOut>(TIn? source) where TIn : class where TOut : class, new() => source is null ? null : Map<TIn, TOut>(source);

        /// <summary>
        /// Try-map pattern: maps <paramref name="source"/> into <paramref name="result"/> and returns
        /// <see langword="true"/>, or sets <paramref name="result"/> to <see langword="null"/> and returns
        /// <see langword="false"/> when <paramref name="source"/> is <see langword="null"/>.
        /// </summary>
        public bool TryMap<TIn, TOut>(TIn? source, [NotNullWhen(true)] out TOut? result) where TIn : class where TOut : class, new()
        {
            if (source is null) { result = null; return false; }
            result = Map<TIn, TOut>(source);
            return true;
        }

        /// <summary>
        /// Maps <paramref name="source"/>, or returns the pre-built <paramref name="fallback"/> instance
        /// when <paramref name="source"/> is <see langword="null"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="fallback"/> is null.</exception>
        public TOut MapOrElse<TIn, TOut>(TIn? source, TOut fallback) where TIn : class where TOut : class, new()
        {
            ArgumentNullException.ThrowIfNull(fallback);
            return source is null ? fallback : Map<TIn, TOut>(source);
        }

        /// <summary>
        /// Maps <paramref name="source"/>, or invokes <paramref name="fallbackFactory"/> and returns its result
        /// when <paramref name="source"/> is <see langword="null"/>.
        /// The factory is only called when needed (lazy evaluation).
        /// </summary>
        /// <exception cref="ArgumentNullException">When <paramref name="fallbackFactory"/> is null.</exception>
        public TOut MapOrElse<TIn, TOut>(TIn? source, Func<TOut> fallbackFactory) where TIn : class where TOut : class, new()
        {
            ArgumentNullException.ThrowIfNull(fallbackFactory);
            return source is null ? fallbackFactory() : Map<TIn, TOut>(source);
        }

        #endregion

        #region Field-Masked Mapping

        /// <summary>
        /// Maps <paramref name="source"/> to a new <typeparamref name="TOut"/> using the compiled
        /// singleton delegate, then applies <paramref name="mask"/> to zero out fields the current
        /// caller is not authorised to see.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is the <em>post-map filter</em> pattern: the singleton compiled delegate always
        /// maps all fields at maximum performance; a per-request <see cref="IFieldMask{TOut}"/>
        /// zeroes sensitive fields <em>after</em> construction. The mapper itself remains stateless
        /// and thread-safe — the per-request sensitivity is confined entirely to the mask instance.
        /// </para>
        /// <para>
        /// Register <see cref="FieldMask{T}"/> as a <c>Scoped</c> service so each HTTP request
        /// receives a mask populated with the current user's field-level permissions.
        /// </para>
        /// <para>
        /// For <see cref="System.Linq.IQueryable{T}"/> / EF Core scenarios where sensitive fields
        /// must not be loaded from the database at all, use
        /// <see cref="Project{TIn,TOut}(System.Linq.IQueryable{TIn}, IFieldMask{TOut})"/> instead.
        /// </para>
        /// </remarks>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source object to map.</param>
        /// <param name="mask">Per-request field visibility mask applied after mapping.</param>
        /// <returns>A new <typeparamref name="TOut"/> with excluded fields set to their default value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="mask"/> is <see langword="null"/>.</exception>
        public TOut MapThenApplyMask<TIn, TOut>(TIn source, IFieldMask<TOut> mask) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(mask);

            TOut result = Map<TIn, TOut>(source);

            if (mask is FieldMask<TOut> fieldMask)
                fieldMask.Apply(result);
            else
                ApplyMaskSlow(result, mask.ExcludedFields);

            return result;
        }

        /// <summary>
        /// Slow-path mask application via reflection for custom <see cref="IFieldMask{T}"/> implementations.
        /// Used only when the mask is not the built-in <see cref="FieldMask{T}"/> type.
        /// </summary>
        /// <param name="instance">The mapped target instance to reset excluded fields on.</param>
        /// <param name="excludedFields">Names of properties to set to their <see langword="default"/> value.</param>
        private static void ApplyMaskSlow<TOut>(TOut instance, IReadOnlyCollection<string> excludedFields)
        {
            foreach (string name in excludedFields)
            {
                System.Reflection.PropertyInfo? prop = typeof(TOut).GetProperty(name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (prop?.CanWrite != true)
                    continue;

                object? defaultValue = prop.PropertyType.IsValueType ? Activator.CreateInstance(prop.PropertyType) : null;
                prop.SetValue(instance, defaultValue);
            }
        }

        #endregion
    }
}
