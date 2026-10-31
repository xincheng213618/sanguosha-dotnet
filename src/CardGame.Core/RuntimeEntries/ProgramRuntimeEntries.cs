using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CardGame.Core;

// A shared, typed entry stores only its module resolver. It is not a rule host.
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class ProgramRuntimeEntryAttribute(Type owner) : Attribute
{
    internal Type Owner { get; } = owner;
}

internal sealed record ProgramRuntimeEntryRegistration(
    Type Type, Type Owner, ConstructorInfo Constructor, Func<Func<Type, object>, object> ResolverFactory)
{
    internal static ProgramRuntimeEntryRegistration For(Type type)
    {
        var owner = type.GetCustomAttribute<ProgramRuntimeEntryAttribute>()?.Owner;
        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (owner is null || !type.IsSealed || type.IsAbstract || type.ContainsGenericParameters ||
            !ProgramRuntimeModuleCatalog.IsDeclared(owner) || owner.IsAbstract ||
            constructors is not [var constructor] || !constructor.GetParameters().Select(parameter => parameter.ParameterType)
                .SequenceEqual([typeof(Func<>).MakeGenericType(owner)]))
            throw new InvalidOperationException($"Runtime entry '{type.FullName}' requires one resolver for its declared module.");

        var resolverFactory = typeof(ProgramRuntimeEntryRegistration)
            .GetMethod(nameof(CreateResolver), BindingFlags.Static | BindingFlags.NonPublic)!
            .MakeGenericMethod(owner).CreateDelegate<Func<Func<Type, object>, object>>();
        return new(type, owner, constructor, resolverFactory);
    }

    internal object Create(Func<Type, object> resolve)
    {
        try { return Constructor.Invoke([ResolverFactory(resolve)]); }
        catch (TargetInvocationException failure) when (failure.InnerException is { } cause)
        { ExceptionDispatchInfo.Capture(cause).Throw(); throw; }
    }

    private static object CreateResolver<TOwner>(Func<Type, object> resolve) where TOwner : class =>
        (Func<TOwner>)(() => (TOwner)resolve(typeof(TOwner)));
}
