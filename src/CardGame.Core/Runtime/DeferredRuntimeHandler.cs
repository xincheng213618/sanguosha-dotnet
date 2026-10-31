using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace CardGame.Core;

// One compiled wrapper factory per delegate signature; these factories never
// capture a match. Each wrapper caches its actual handler after first use.
internal sealed class DeferredRuntimeHandler(Func<Delegate> create)
{
    private static readonly ConcurrentDictionary<Type, Func<DeferredRuntimeHandler, Delegate>> Factories = new();
    private Delegate? _handler;
    public Delegate Resolve() => _handler ??= create();

    internal static Delegate Create(MethodInfo method, Type delegateType, Func<Delegate> create)
    {
        if (!typeof(Delegate).IsAssignableFrom(delegateType))
            throw new ArgumentException("A runtime handler requires a delegate type.", nameof(delegateType));
        var invoke = delegateType.GetMethod("Invoke") ??
            throw new ArgumentException("A runtime handler requires a delegate type.", nameof(delegateType));
        if (method.ReturnType != invoke.ReturnType &&
            (method.ReturnType == typeof(void) || invoke.ReturnType == typeof(void) ||
             method.ReturnType.IsValueType || invoke.ReturnType.IsValueType ||
             !invoke.ReturnType.IsAssignableFrom(method.ReturnType)))
            throw new ArgumentException("The runtime handler return type does not match its delegate.", nameof(delegateType));
        var parameters = invoke.GetParameters().Select(parameter => Expression.Parameter(parameter.ParameterType)).ToArray();
        // Validate the binding at registration time without constructing its
        // receiver. Factories and rules still run only at the original call site.
        var call = Expression.Call(Expression.Default(method.DeclaringType!), method, parameters);
        _ = Expression.Lambda(delegateType, call, parameters);
        return Factories.GetOrAdd(delegateType, CompileFactory)(new(create));
    }

    private static Func<DeferredRuntimeHandler, Delegate> CompileFactory(Type delegateType)
    {
        var receiver = Expression.Parameter(typeof(DeferredRuntimeHandler));
        var parameters = delegateType.GetMethod("Invoke")!.GetParameters()
            .Select(parameter => Expression.Parameter(parameter.ParameterType)).ToArray();
        var resolve = Expression.Call(receiver, nameof(Resolve), Type.EmptyTypes);
        var invoke = Expression.Invoke(Expression.Convert(resolve, delegateType), parameters);
        var handler = Expression.Lambda(delegateType, invoke, parameters);
        return Expression.Lambda<Func<DeferredRuntimeHandler, Delegate>>(
            Expression.Convert(handler, typeof(Delegate)), receiver).Compile();
    }
}
