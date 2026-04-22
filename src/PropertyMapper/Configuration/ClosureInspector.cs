using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace PropertyMapper.Configuration
{
    /// <summary>
    /// Guards mapping configuration lambdas against accidental capture of request-scoped or
    /// singleton DI services that must not be compiled into process-wide delegates.
    /// </summary>
    internal static class ClosureInspector
    {
        /// <summary>
        /// Detects compiler-generated closures that capture request-scoped services, preventing them
        /// from being compiled into the process-wide singleton mapping delegate.
        /// </summary>
        /// <param name="del">Delegate to inspect.</param>
        /// <param name="callerName">Name of the calling API method, used in the exception message.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a captured field's type is identified as a known request-scoped service type.
        /// </exception>
        internal static void GuardAgainstClosureCapture(Delegate del, string callerName)
        {
            if (del.Target is null) return; // static method — no closure, always safe

            Type closureType = del.Target.GetType();
            if (closureType.GetCustomAttribute<CompilerGeneratedAttribute>() is null) return; // explicit instance method, not a lambda closure

            foreach (FieldInfo field in closureType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (IsSuspectedScopedService(field.FieldType))
                    throw new InvalidOperationException(
                        $"{callerName} lambda captures '{field.FieldType.Name}', which appears to be a " +
                        "request-scoped or transient service. Mapping delegates are compiled into a " +
                        "process-wide singleton — captured scoped instances become stale after the " +
                        "first request and leak data across requests. " +
                        "Pass per-request dependencies via MapFromWithContext<TCtx> instead.");
            }
        }

        /// <summary>
        /// Returns <see langword="true"/> when <paramref name="contextType"/> is or implements
        /// <see cref="IServiceProvider"/> or <see cref="IServiceScopeFactory"/> — types that must
        /// never be used as a mapping context because they expose the root DI container.
        /// </summary>
        /// <param name="contextType">The CLR type to test.</param>
        internal static bool IsForbiddenContextType(Type contextType) =>
            typeof(IServiceProvider).IsAssignableFrom(contextType) ||
            typeof(IServiceScopeFactory).IsAssignableFrom(contextType);

        /// <summary>
        /// Guards against <typeparamref name="TCtx"/> being <see cref="IServiceProvider"/> or
        /// <see cref="IServiceScopeFactory"/>, which are root-container singletons and must never
        /// flow through the per-call context parameter.
        /// </summary>
        /// <typeparam name="TCtx">The context type to validate; must not be or implement <see cref="IServiceProvider"/> or <see cref="IServiceScopeFactory"/>.</typeparam>
        /// <exception cref="InvalidOperationException">Thrown when <typeparamref name="TCtx"/> is or implements a forbidden DI container type.</exception>
        internal static void GuardAgainstSingletonContext<TCtx>()
        {
            if (IsForbiddenContextType(typeof(TCtx)))
                throw new InvalidOperationException(
                    $"'{typeof(TCtx).Name}' must not be used as a MapFromWithContext context type. " +
                    "IServiceProvider and IServiceScopeFactory are root-container singletons: " +
                    "using them as per-call context defeats DI scoping and risks resolving stale scoped services. " +
                    "Resolve the required values before calling MapWithContext and pass a plain value object " +
                    "(e.g. an exchange-rate record or tenant identifier DTO) as TCtx instead.");
        }

        /// <summary>
        /// Returns <see langword="true"/> when <paramref name="t"/> matches a well-known type that must
        /// not be captured inside a singleton mapping delegate.
        /// </summary>
        private static bool IsSuspectedScopedService(Type t)
        {
            if (t.IsValueType || t.IsPrimitive || t == typeof(string)) return false;

            // Definitive: DI root container types reachable without extra package references.
            if (typeof(IServiceProvider).IsAssignableFrom(t)) return true;

            // Name-based heuristics for common ASP.NET Core / EF Core types.
            // We cannot take a hard reference to those assemblies from this library.
            string name = t.Name;
            return name is "HttpContext" or "IHttpContextAccessor" or "ClaimsPrincipal" or "HttpRequest" or "HttpResponse" || name.EndsWith("DbContext", StringComparison.Ordinal);
        }
    }
}
