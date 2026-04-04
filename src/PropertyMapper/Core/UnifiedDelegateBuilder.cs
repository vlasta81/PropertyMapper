using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Unified delegate builder for both reference types (class, record class) and value types (struct, record struct).
    /// Automatically selects appropriate IL emission based on type characteristics.
    /// </summary>
    /// <remarks>
    /// .NET 10 optimizations applied:
    /// <list type="bullet">
    /// <item><see cref="System.Text.CompositeFormat"/> for dynamic method-name generation — reusable parsed template avoids per-call string interpolation allocations.</item>
    /// <item>Single unified builder eliminates the prior class/struct code duplication.</item>
    /// </list>
    /// </remarks>
    internal sealed class UnifiedDelegateBuilder : IDelegateBuilder
    {
        /// <summary>Monotonically increasing counter used to generate unique dynamic method names.</summary>
        private static int _methodCounter;
        /// <summary>Pre-compiled format template for dynamic method names (avoids repeated string interpolation allocations).</summary>
        private static readonly CompositeFormat s_methodNameFormat = CompositeFormat.Parse("Map_{0}_to_{1}_{2}");

        /// <summary>
        /// Builds a compiled <see cref="Func{TIn, TOut}"/> delegate that copies matching properties
        /// from <typeparamref name="TIn"/> to a newly created <typeparamref name="TOut"/> instance.
        /// Emits a <see cref="System.Reflection.Emit.DynamicMethod"/> that uses <c>Initobj</c> for
        /// value-type targets and <c>Newobj</c> for class targets.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type; must have a parameterless constructor.</typeparam>
        /// <param name="plan">The resolved mapping plan describing which properties to copy.</param>
        /// <returns>A compiled, reusable mapping delegate.</returns>
        /// <exception cref="MissingMethodException">Thrown when <typeparamref name="TOut"/> has no public parameterless constructor.</exception>
        public Func<TIn, TOut> BuildDelegate<TIn, TOut>(MappingPlan plan) where TOut : new()
        {
            string methodName = string.Format(
                CultureInfo.InvariantCulture,
                s_methodNameFormat,
                plan.SourceType.Name,
                plan.TargetType.Name,
                Interlocked.Increment(ref _methodCounter));

            DynamicMethod dm = new DynamicMethod(
                methodName,
                typeof(TOut),
                [typeof(TIn)],
                typeof(TOut),
                skipVisibility: true);

            ILGenerator il = dm.GetILGenerator();
            LocalBuilder result = il.DeclareLocal(typeof(TOut));

            bool isValueType = typeof(TOut).IsValueType;

            // Initialize target instance
            if (isValueType)
            {
                // Struct: use Initobj (faster, no allocation)
                il.Emit(OpCodes.Ldloca, result);
                il.Emit(OpCodes.Initobj, typeof(TOut));
            }
            else
            {
                // Class: use Newobj
                ConstructorInfo? ctor = typeof(TOut).GetConstructor(Type.EmptyTypes);
                if (ctor is null)
                    throw new MissingMethodException(typeof(TOut).FullName, ".ctor");
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Stloc, result);
            }

            // Map properties
            ReadOnlySpan<PropertyBinding> bindings = plan.Bindings;
            for (int i = 0; i < bindings.Length; i++)
            {
                EmitPropertyMapping<TIn>(il, bindings[i], result, isValueType);
            }

            il.Emit(OpCodes.Ldloc, result);
            il.Emit(OpCodes.Ret);

            return (Func<TIn, TOut>)dm.CreateDelegate(typeof(Func<TIn, TOut>));
        }

        /// <summary>
        /// Emits the IL instructions that load the target instance address (or value), read a single
        /// source property, apply any necessary conversion, and call the target property setter.
        /// </summary>
        /// <typeparam name="TIn">Source type used by the dynamic method parameter.</typeparam>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="binding">The property binding to emit.</param>
        /// <param name="result">Local variable that holds the partially-constructed target object.</param>
        /// <param name="isTargetValueType">
        /// <see langword="true"/> when <c>TOut</c> is a value type; changes whether the target local
        /// is loaded by address (<c>Ldloca</c>) or by value (<c>Ldloc</c>).
        /// </param>
        private static void EmitPropertyMapping<TIn>(ILGenerator il, PropertyBinding binding, LocalBuilder result, bool isTargetValueType)
        {
            // Load target instance (address for struct, value for class)
            if (isTargetValueType)
            {
                il.Emit(OpCodes.Ldloca, result); // Struct needs address
            }
            else
            {
                il.Emit(OpCodes.Ldloc, result); // Class needs value
            }

            // Optimization: only declare local if conversion needs it
            bool needsLocal = binding.Conversion is ConversionKind.NullableToValue or ConversionKind.NullableToNullable or ConversionKind.Nested;
            LocalBuilder? srcLocal = needsLocal ? il.DeclareLocal(binding.Source.PropertyType) : null;

            // Load source value
            if (typeof(TIn).IsValueType)
            {
                il.Emit(OpCodes.Ldarga_S, 0); // Address for value-type parameter
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0); // Value for reference-type parameter
            }

            il.EmitCall(
                binding.Source.GetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
                binding.Source.GetMethod,
                null);

            if (srcLocal is not null)
            {
                il.Emit(OpCodes.Stloc, srcLocal);
                il.Emit(OpCodes.Ldloc, srcLocal);
            }

            ConversionEmitter.EmitConversion(il, binding, srcLocal);

            // Call setter (always Call for struct, Callvirt or Call for class)
            OpCode setterOpCode = isTargetValueType ? OpCodes.Call : (binding.Target.SetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call);

            il.EmitCall(setterOpCode, binding.Target.SetMethod!, null);
        }
    }

}
