using System.Globalization;
using System.Reflection.Emit;
using System.Text;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Builds <see cref="Action{TIn, TOut}"/> delegates for in-place (merge) mapping.
    /// The emitted method writes matching properties from <c>TIn</c> directly onto an
    /// existing <c>TOut</c> instance — no <c>Newobj</c> or <c>Initobj</c> is emitted.
    /// </summary>
    /// <remarks>
    /// IL signature: <c>void(TIn source, TOut target)</c>
    /// <list type="bullet">
    ///   <item><description><c>arg_0</c> — source; loaded with <c>Ldarga_S</c> for value types, <c>Ldarg_0</c> for classes.</description></item>
    ///   <item><description><c>arg_1</c> — existing target reference; always loaded with <c>Ldarg_1</c>.</description></item>
    /// </list>
    /// Mirrors <see cref="UnifiedDelegateBuilder"/> but omits object construction and returns an
    /// <see cref="Action{TIn, TOut}"/> instead of a <see cref="Func{TIn, TOut}"/>.
    /// </remarks>
    internal sealed class InPlaceDelegateBuilder
    {
        /// <summary>Monotonically increasing counter used to generate unique dynamic method names.</summary>
        private static int _methodCounter;
        /// <summary>Pre-compiled format template for dynamic method names (avoids repeated string interpolation allocations).</summary>
        private static readonly CompositeFormat s_methodNameFormat = CompositeFormat.Parse("MapInto_{0}_to_{1}_{2}");

        /// <summary>
        /// Builds a compiled <see cref="Action{TIn, TOut}"/> delegate that copies matching properties
        /// from <typeparamref name="TIn"/> onto an existing <typeparamref name="TOut"/> instance.
        /// No new object is allocated; the caller supplies the target.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type; must be a reference type (class).</typeparam>
        /// <param name="plan">The resolved mapping plan describing which properties to copy.</param>
        /// <returns>A compiled, reusable in-place mapping delegate.</returns>
        public Action<TIn, TOut> BuildDelegate<TIn, TOut>(MappingPlan plan) where TOut : class
        {
            string methodName = string.Format(
                CultureInfo.InvariantCulture,
                s_methodNameFormat,
                plan.SourceType.Name,
                plan.TargetType.Name,
                Interlocked.Increment(ref _methodCounter));

            // void(TIn source, TOut target) — arg_0 = source, arg_1 = existing target reference
            DynamicMethod dm = new DynamicMethod(
                methodName,
                typeof(void),
                [typeof(TIn), typeof(TOut)],
                typeof(TOut),
                skipVisibility: true);

            ILGenerator il = dm.GetILGenerator();

            ReadOnlySpan<PropertyBinding> bindings = plan.Bindings;
            for (int i = 0; i < bindings.Length; i++)
            {
                EmitPropertyMapping<TIn>(il, bindings[i]);
            }

            il.Emit(OpCodes.Ret);

            return (Action<TIn, TOut>)dm.CreateDelegate(typeof(Action<TIn, TOut>));
        }

        /// <summary>
        /// Emits the IL instructions that load the existing target reference (<c>arg_1</c>), read a
        /// single source property, apply any necessary conversion, and call the target property setter.
        /// </summary>
        /// <typeparam name="TIn">Source type used by the dynamic method parameter.</typeparam>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="binding">The property binding to emit.</param>
        private void EmitPropertyMapping<TIn>(ILGenerator il, PropertyBinding binding)
        {
            // Push target reference (arg_1) — always a class, loaded by value
            il.Emit(OpCodes.Ldarg_1);

            // Declare a local variable only when the conversion path requires it
            bool needsLocal = binding.Conversion is ConversionKind.NullableToValue or ConversionKind.NullableToNullable or ConversionKind.Nested;
            LocalBuilder? srcLocal = needsLocal ? il.DeclareLocal(binding.Source.PropertyType) : null;

            // Push source value from arg_0
            if (typeof(TIn).IsValueType)
                il.Emit(OpCodes.Ldarga_S, 0);   // address for value-type source
            else
                il.Emit(OpCodes.Ldarg_0);        // reference for class source

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

            // Target is always a reference type — use Callvirt for virtual setters
            OpCode setterOpCode = binding.Target.SetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;
            il.EmitCall(setterOpCode, binding.Target.SetMethod!, null);
        }
    }

}

