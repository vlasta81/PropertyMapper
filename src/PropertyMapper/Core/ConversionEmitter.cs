using System.Reflection;
using System.Reflection.Emit;

namespace PropertyMapper.Core
{

    /// <summary>
    /// Handles IL emission for type conversions between a source property value and its target property type.
    /// Supports direct assignment, nullable wrapping/unwrapping, user-defined operator conversions, and nested object mapping.
    /// </summary>
    internal static class ConversionEmitter
    {
        /// <summary>
        /// Emits the IL instructions required to convert the value currently on the evaluation stack
        /// from the source property type to the target property type described by <paramref name="binding"/>.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method being built.</param>
        /// <param name="binding">The property binding that describes the source, target, and conversion kind.</param>
        /// <param name="srcLocal">
        /// A local variable that already holds the source property value.
        /// Required for <see cref="ConversionKind.NullableToValue"/>, <see cref="ConversionKind.NullableToNullable"/>,
        /// and <see cref="ConversionKind.Nested"/>; may be <see langword="null"/> for other kinds.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a required nested plan or source local is missing.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown for unrecognized <see cref="ConversionKind"/> values.</exception>
        public static void EmitConversion(ILGenerator il, PropertyBinding binding, LocalBuilder? srcLocal)
        {
            switch (binding.Conversion)
            {
                case ConversionKind.Direct:
                    return;

                case ConversionKind.ValueToNullable:
                    EmitValueToNullable(il, binding);
                    return;

                case ConversionKind.NullableToValue:
                    EmitNullableToValue(il, binding.Source.PropertyType, binding.Target.PropertyType);
                    return;

                case ConversionKind.NullableToNullable:
                    EmitNullableToNullable(il, binding.Source.PropertyType, binding.Target.PropertyType);
                    return;

                case ConversionKind.Operator:
                    EmitOperatorConversion(il, binding);
                    return;

                case ConversionKind.Nested:
                    if (binding.NestedPlan is null)
                        throw new InvalidOperationException("Nested plan is null");
                    if (srcLocal is null)
                        throw new InvalidOperationException("Source local required for nested conversion");
                    EmitNested(il, binding, srcLocal);
                    return;

                default:
                    throw new ArgumentOutOfRangeException(nameof(binding.Conversion), binding.Conversion, "Unsupported conversion kind.");
            }
        }

        /// <summary>
        /// Emits IL that wraps a non-nullable value-type value in its <c>Nullable&lt;T&gt;</c> counterpart
        /// by calling <c>new Nullable&lt;T&gt;(value)</c>.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="binding">The binding whose target property type is the nullable wrapper.</param>
        private static void EmitValueToNullable(ILGenerator il, PropertyBinding binding)
        {
            ConstructorInfo? ctor = binding.Target.PropertyType.GetConstructor([binding.Source.PropertyType]);
            if (ctor is null)
                throw new InvalidOperationException($"Cannot create nullable wrapper for {binding.Target.PropertyType}");
            il.Emit(OpCodes.Newobj, ctor);
        }

        /// <summary>
        /// Emits IL that calls the user-defined <c>op_Implicit</c> or <c>op_Explicit</c> conversion operator
        /// found on the source or target type.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="binding">The binding whose source and target types define the operator.</param>
        /// <exception cref="InvalidOperationException">Thrown when no compatible operator is found at emit time.</exception>
        private static void EmitOperatorConversion(ILGenerator il, PropertyBinding binding)
        {
            MethodInfo? op = OperatorDiscovery.Find(binding.Source.PropertyType, binding.Target.PropertyType);
            if (op is null)
                throw new InvalidOperationException("Conversion operator not found");
            il.EmitCall(OpCodes.Call, op, null);
        }

        /// <summary>
        /// Emits IL for mapping a nested complex object: creates a new target instance, maps its properties
        /// recursively using the binding's <see cref="PropertyBinding.NestedPlan"/>, and handles
        /// <see langword="null"/> source references by storing <see langword="null"/> in the target.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="binding">The nested binding whose <see cref="PropertyBinding.NestedPlan"/> drives recursive mapping.</param>
        /// <param name="srcLocal">Local variable that holds the source property value.</param>
        private static void EmitNested(ILGenerator il, PropertyBinding binding, LocalBuilder srcLocal)
        {
            Type sourceType = binding.Source.PropertyType;
            Type targetType = binding.Target.PropertyType;
            LocalBuilder resultLocal = il.DeclareLocal(targetType);

            // Pop the srcValue that the caller pushed; we access it via srcLocal below.
            il.Emit(OpCodes.Pop);

            if (!sourceType.IsValueType)
            {
                Label isNullLabel = il.DefineLabel();
                Label endLabel = il.DefineLabel();

                il.Emit(OpCodes.Ldloc, srcLocal);
                il.Emit(OpCodes.Brfalse, isNullLabel);

                EmitCreateAndMapNested(il, binding.NestedPlan!, srcLocal, resultLocal);
                il.Emit(OpCodes.Br, endLabel);

                il.MarkLabel(isNullLabel);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Stloc, resultLocal);

                il.MarkLabel(endLabel);
            }
            else
            {
                EmitCreateAndMapNested(il, binding.NestedPlan!, srcLocal, resultLocal);
            }

            il.Emit(OpCodes.Ldloc, resultLocal);
        }

        /// <summary>
        /// Emits IL that instantiates a new target object (via <c>Initobj</c> for value types or <c>Newobj</c> for classes)
        /// and copies all properties described by <paramref name="plan"/> from the object held in <paramref name="srcLocal"/>.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="plan">The mapping plan for the nested type pair.</param>
        /// <param name="srcLocal">Local variable that holds the source nested object.</param>
        /// <param name="dstLocal">Local variable that will receive the newly constructed target object.</param>
        private static void EmitCreateAndMapNested(ILGenerator il, MappingPlan plan, LocalBuilder srcLocal, LocalBuilder dstLocal)
        {
            if (plan.TargetType.IsValueType)
            {
                // Struct: use Initobj
                il.Emit(OpCodes.Ldloca, dstLocal);
                il.Emit(OpCodes.Initobj, plan.TargetType);
            }
            else
            {
                // Class: use Newobj
                ConstructorInfo? ctor = plan.TargetType.GetConstructor(Type.EmptyTypes);
                if (ctor is null)
                    throw new MissingMethodException(plan.TargetType.FullName, ".ctor");
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Stloc, dstLocal);
            }

            // .NET 10 optimization: Bindings as ReadOnlySpan
            ReadOnlySpan<PropertyBinding> bindings = plan.Bindings;
            for (int i = 0; i < bindings.Length; i++)
            {
                PropertyBinding binding = bindings[i];

                if (plan.TargetType.IsValueType)
                    il.Emit(OpCodes.Ldloca, dstLocal);
                else
                    il.Emit(OpCodes.Ldloc, dstLocal);

                LocalBuilder valueLocal = il.DeclareLocal(binding.Source.PropertyType);
                il.Emit(OpCodes.Ldloc, srcLocal);
                il.EmitCall(
                    binding.Source.GetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call,
                    binding.Source.GetMethod,
                    null);
                il.Emit(OpCodes.Stloc, valueLocal);
                il.Emit(OpCodes.Ldloc, valueLocal);

                EmitConversion(il, binding, valueLocal);

                il.EmitCall(
                    plan.TargetType.IsValueType ? OpCodes.Call :
                        (binding.Target.SetMethod!.IsVirtual ? OpCodes.Callvirt : OpCodes.Call),
                    binding.Target.SetMethod!,
                    null);
            }
        }

        /// <summary>
        /// Emits IL that unwraps a <c>Nullable&lt;T&gt;</c> source value: yields the underlying value when
        /// <c>HasValue</c> is <see langword="true"/>, or the default value of <paramref name="target"/> otherwise.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="source">The nullable source type (e.g. <c>int?</c>).</param>
        /// <param name="target">The non-nullable target type (e.g. <c>int</c>).</param>
        private static void EmitNullableToValue(ILGenerator il, Type source, Type target)
        {
            MethodInfo hasValueGetter = NullableInfo.GetHasValueGetter(source);
            MethodInfo valueGetter = NullableInfo.GetValueGetter(source);

            LocalBuilder nullableLocal = il.DeclareLocal(source);
            il.Emit(OpCodes.Stloc, nullableLocal);

            Label hasValueLabel = il.DefineLabel();
            Label endLabel = il.DefineLabel();

            il.Emit(OpCodes.Ldloca, nullableLocal); // Fixed: use Ldloca not Ldloca_S
            il.EmitCall(OpCodes.Call, hasValueGetter, null);
            il.Emit(OpCodes.Brtrue, hasValueLabel);

            EmitDefault(il, target);
            il.Emit(OpCodes.Br, endLabel);

            il.MarkLabel(hasValueLabel);
            il.Emit(OpCodes.Ldloca, nullableLocal);
            il.EmitCall(OpCodes.Call, valueGetter, null);

            il.MarkLabel(endLabel);
        }

        /// <summary>
        /// Emits IL that converts a <c>Nullable&lt;TSource&gt;</c> source to a <c>Nullable&lt;TTarget&gt;</c> target,
        /// preserving <see langword="null"/> and applying any necessary primitive widening/narrowing to the underlying value.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="source">The nullable source type.</param>
        /// <param name="target">The nullable target type.</param>
        private static void EmitNullableToNullable(ILGenerator il, Type source, Type target)
        {
            MethodInfo hasValueGetter = NullableInfo.GetHasValueGetter(source);
            MethodInfo valueGetter = NullableInfo.GetValueGetter(source);
            Type? sourceUnderlying = Nullable.GetUnderlyingType(source);
            Type? targetUnderlying = Nullable.GetUnderlyingType(target);

            if (sourceUnderlying is null || targetUnderlying is null)
                throw new InvalidOperationException($"Cannot convert {source} to {target}");

            LocalBuilder nullableLocal = il.DeclareLocal(source);
            il.Emit(OpCodes.Stloc, nullableLocal);

            Label hasValueLabel = il.DefineLabel();
            Label endLabel = il.DefineLabel();

            il.Emit(OpCodes.Ldloca, nullableLocal);
            il.EmitCall(OpCodes.Call, hasValueGetter, null);
            il.Emit(OpCodes.Brtrue, hasValueLabel);

            EmitDefault(il, target);
            il.Emit(OpCodes.Br, endLabel);

            il.MarkLabel(hasValueLabel);
            il.Emit(OpCodes.Ldloca, nullableLocal);
            il.EmitCall(OpCodes.Call, valueGetter, null);

            EmitPrimitiveConversionIfNeeded(il, sourceUnderlying, targetUnderlying);

            ConstructorInfo? ctor = target.GetConstructor([targetUnderlying]);
            if (ctor is null)
                throw new InvalidOperationException($"Ctor not found for {target}");
            il.Emit(OpCodes.Newobj, ctor);

            il.MarkLabel(endLabel);
        }

        /// <summary>
        /// Emits a primitive numeric widening or narrowing IL opcode when
        /// <paramref name="source"/> and <paramref name="target"/> differ in type.
        /// Is a no-op when both types are identical.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="source">The underlying source numeric type.</param>
        /// <param name="target">The underlying target numeric type.</param>
        /// <exception cref="NotSupportedException">
        /// Thrown when no conversion opcode covers the given type combination.
        /// </exception>
        private static void EmitPrimitiveConversionIfNeeded(ILGenerator il, Type source, Type target)
        {
            if (source == target) return;

            if (target == typeof(long))
                il.Emit(OpCodes.Conv_I8);
            else if (target == typeof(int))
                il.Emit(OpCodes.Conv_I4);
            else if (target == typeof(short))
                il.Emit(OpCodes.Conv_I2);
            else if (target == typeof(byte))
                il.Emit(OpCodes.Conv_U1);
            else if (target == typeof(float))
                il.Emit(OpCodes.Conv_R4);
            else if (target == typeof(double))
                il.Emit(OpCodes.Conv_R8);
            else
                throw new NotSupportedException($"Conversion {source} → {target}");
        }

        /// <summary>
        /// Emits IL that pushes the default value of <paramref name="type"/> onto the evaluation stack.
        /// Value types are zeroed via <c>Initobj</c>; reference types push <see langword="null"/>.
        /// </summary>
        /// <param name="il">The <see cref="ILGenerator"/> for the dynamic method.</param>
        /// <param name="type">The type whose default value should be emitted.</param>
        private static void EmitDefault(ILGenerator il, Type type)
        {
            if (type.IsValueType)
            {
                LocalBuilder tmp = il.DeclareLocal(type);
                il.Emit(OpCodes.Ldloca, tmp);
                il.Emit(OpCodes.Initobj, type);
                il.Emit(OpCodes.Ldloc, tmp);
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
            }
        }

        }


}

