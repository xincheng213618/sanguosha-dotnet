using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CardGame.Core;

/// <summary>Registers one private, independently constructed program capability.</summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
internal sealed class ProgramCapabilityAttribute(Type? deferredModule = null) : Attribute
{
    internal Type? DeferredModule { get; } = deferredModule;
}

/// <summary>
/// Immutable capability metadata. Constructors and contracts never capture a
/// game; only the match runtime's registries retain executable instances.
/// </summary>
internal sealed class ProgramCapabilityCatalog
{
    private static readonly Lazy<ProgramCapabilityCatalog> BuiltIn = new(() =>
        new(typeof(GameEngine).Assembly.GetTypes()), true);
    internal static ProgramCapabilityCatalog Default => BuiltIn.Value;

    internal sealed record Registration(
        Type Implementation,
        IReadOnlyList<Type> Contracts,
        ConstructorInfo Constructor,
        bool IsRuntimeModule,
        Type? DeferredModule = null)
    {
        internal object CreateAdapter(GameEngine engine)
        {
            if (IsRuntimeModule) throw new InvalidOperationException("Independent modules are created by the match runtime.");
            return DeferredModule is null ? Construct([engine]) : CreateDeferredAdapter(engine.GetRuntimeModuleInstance);
        }

        internal object CreateDeferredAdapter(Func<Type, object> resolveModule)
        {
            if (DeferredModule is null) throw new InvalidOperationException("This capability has no deferred module target.");
            var factory = typeof(Registration).GetMethod(nameof(TargetFactory), BindingFlags.Static | BindingFlags.NonPublic)!
                .MakeGenericMethod(DeferredModule).Invoke(null, [resolveModule])!;
            return Construct([factory]);
        }

        private static Func<T> TargetFactory<T>(Func<Type, object> resolveModule) where T : class =>
            () => (T)resolveModule(typeof(T));

        private object Construct(object[] arguments)
        {
            try { return Constructor.Invoke(arguments); }
            catch (TargetInvocationException failure) when (failure.InnerException is { } cause)
            { ExceptionDispatchInfo.Capture(cause).Throw(); throw; }
        }
    }

    private readonly IReadOnlyDictionary<Type, Registration> _byContract;
    internal IReadOnlyList<Registration> Registrations { get; }

    internal ProgramCapabilityCatalog(IEnumerable<Type> types)
    {
        ArgumentNullException.ThrowIfNull(types);
        var declaredTypes = types.ToArray();
        if (declaredTypes.Any(type => type is null))
            throw new InvalidOperationException("A program capability type cannot be null.");
        var registrations = new List<Registration>();
        var byContract = new Dictionary<Type, Registration>();
        foreach (var type in declaredTypes.Where(type => type.IsDefined(typeof(ProgramCapabilityAttribute), false))
                     .OrderBy(type => type.FullName, StringComparer.Ordinal))
        {
            if (!type.IsClass || !type.IsSealed || type.IsAbstract || type.IsGenericType || type.ContainsGenericParameters)
                throw Invalid(type, "must be a sealed, non-generic class");
            var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var independent = ProgramRuntimeModuleCatalog.IsDeclared(type);
            var deferredModule = type.GetCustomAttribute<ProgramCapabilityAttribute>()!.DeferredModule;
            if (independent)
            {
                if (deferredModule is not null) throw Invalid(type, "a runtime module cannot also be a deferred adapter");
                _ = new ProgramRuntimeModuleCatalog([type]);
            }
            else if (deferredModule is not null)
            {
                _ = new ProgramRuntimeModuleCatalog([deferredModule]).Get(deferredModule);
                if (deferredModule.IsAbstract || constructors is not [var deferredConstructor] ||
                    !deferredConstructor.GetParameters().Select(parameter => parameter.ParameterType)
                        .SequenceEqual([typeof(Func<>).MakeGenericType(deferredModule)]))
                    throw Invalid(type, "a deferred adapter requires one factory for its declared independent module");
            }
            else if (constructors is not [var legacyConstructor] ||
                !legacyConstructor.GetParameters().Select(parameter => parameter.ParameterType).SequenceEqual([typeof(GameEngine)]))
                throw Invalid(type, "requires exactly one constructor accepting only GameEngine, or an explicit independent runtime module");
            var contracts = type.GetInterfaces().Where(contract => contract != typeof(IProgramSelectionQuery) &&
                    contract != typeof(IProgramRuntimeOperations))
                .OrderBy(contract => contract.FullName, StringComparer.Ordinal).ToArray();
            if (contracts.Length == 0 || contracts.Any(contract => contract.ContainsGenericParameters ||
                    contract == typeof(ISkillProgramExecutionHost) || contract == typeof(ISkillProgramEffectHost)))
                throw Invalid(type, "requires dedicated capability interfaces, separate from the shared program host");
            var registration = new Registration(type, Array.AsReadOnly(contracts), constructors[0], independent, deferredModule);
            foreach (var contract in contracts)
                if (!byContract.TryAdd(contract, registration))
                    throw new InvalidOperationException($"Program capability '{contract.FullName}' is registered by both " +
                        $"'{byContract[contract].Implementation.FullName}' and '{type.FullName}'.");
            registrations.Add(registration);
        }
        if (registrations.Count == 0)
            throw new InvalidOperationException("No program capabilities were registered.");
        Registrations = Array.AsReadOnly(registrations.ToArray());
        _byContract = new ReadOnlyDictionary<Type, Registration>(byContract);
    }

    internal bool TryResolve(Type contract, [NotNullWhen(true)] out Registration? registration)
    {
        ArgumentNullException.ThrowIfNull(contract);
        if (!contract.IsInterface)
            throw new ArgumentException("A program capability contract must be an interface.", nameof(contract));
        return _byContract.TryGetValue(contract, out registration);
    }

    private static InvalidOperationException Invalid(Type type, string reason) =>
        new($"Program capability '{type.FullName}' {reason}.");
}

// Explicit per-match bindings carry existing native owners, never factories
// that reach back into the engine or another copy of their mutable state.
internal sealed record ProgramNativeCapabilityBinding(Type Contract, object Instance);

/// <summary>Resolves dedicated adapters and actual native owners for exactly one game.</summary>
internal sealed class ProgramCapabilityRegistry(ProgramCapabilityCatalog catalog,
    Func<ProgramCapabilityCatalog.Registration, object> create,
    IEnumerable<ProgramNativeCapabilityBinding>? nativeBindings = null)
{
    private readonly IReadOnlyDictionary<Type, object> _nativeBindings = ValidateNativeBindings(catalog, nativeBindings);
    private readonly Dictionary<Type, object> _instances = new();
    private readonly HashSet<Type> _constructing = new();

    // Compatibility for consumers that build an isolated adapter catalog. The
    // production match supplies its own factory and owns both registries.
    internal ProgramCapabilityRegistry(GameEngine engine,ProgramCapabilityCatalog catalog)
        : this(catalog, registration => registration.IsRuntimeModule
            ? engine.GetRuntimeModuleInstance(registration.Implementation)
            : registration.CreateAdapter(engine)) { }

    internal T Get<T>() where T : class => TryGet<T>(out var capability)
        ? capability
        : throw new NotSupportedException($"No program capability is registered for '{typeof(T).FullName}'.");

    internal object Get(Type contract) => TryGet(contract, out var capability) ? capability :
        throw new NotSupportedException($"No program capability is registered for '{contract.FullName}'.");

    internal bool TryGet<T>([NotNullWhen(true)] out T? capability) where T : class
    {
        var found = TryGet(typeof(T), out var instance);
        capability = (T?)instance;
        return found;
    }

    private static IReadOnlyDictionary<Type, object> ValidateNativeBindings(ProgramCapabilityCatalog catalog,
        IEnumerable<ProgramNativeCapabilityBinding>? supplied)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        var bindings = new Dictionary<Type, object>();
        foreach (var binding in supplied ?? [])
        {
            if (binding is null)
                throw new InvalidOperationException("A native capability binding cannot be null.");
            var contract = binding.Contract;
            if (contract is null || !contract.IsInterface || contract.ContainsGenericParameters ||
                contract == typeof(IProgramSelectionQuery) || contract == typeof(IProgramRuntimeOperations) ||
                typeof(ISkillProgramExecutionHost).IsAssignableFrom(contract) ||
                typeof(ISkillProgramEffectHost).IsAssignableFrom(contract) ||
                !typeof(IProgramSelectionQuery).IsAssignableFrom(contract) &&
                !typeof(IProgramRuntimeOperations).IsAssignableFrom(contract))
                throw new InvalidOperationException("A native capability requires a dedicated query or runtime-operation interface.");
            if (binding.Instance is null or GameEngine || !contract.IsInstanceOfType(binding.Instance))
                throw new InvalidOperationException($"Native capability '{contract.FullName}' requires its matching state owner, never GameEngine.");
            if (catalog.TryResolve(contract, out _) || !bindings.TryAdd(contract, binding.Instance))
                throw new InvalidOperationException($"Native capability '{contract.FullName}' conflicts with another capability registration.");
        }
        return new ReadOnlyDictionary<Type, object>(bindings);
    }

    private bool TryGet(Type contract, [NotNullWhen(true)] out object? capability)
    {
        _ = catalog.TryResolve(contract, out var registration);
        if (_nativeBindings.TryGetValue(contract, out capability)) return true;
        if (registration is null)
        {
            capability = null;
            return false;
        }
        if (!_instances.TryGetValue(registration.Implementation, out var instance))
        {
            if (!_constructing.Add(registration.Implementation))
                throw new InvalidOperationException($"Cyclic program capability dependency '{registration.Implementation.FullName}'.");
            try
            {
                instance = create(registration);
                if (!registration.Implementation.IsInstanceOfType(instance))
                    throw new InvalidOperationException($"Capability factory returned an invalid '{registration.Implementation.FullName}' adapter.");
                _instances.Add(registration.Implementation, instance);
            }
            finally { _constructing.Remove(registration.Implementation); }
        }
        capability = instance;
        return true;
    }
}

public sealed partial class GameEngine
{
    // Legacy nested adapters and explicit independent modules share one contract
    // directory; independent hook/capability entry points share one match instance.
    private ProgramCapabilityRegistry? _programCapabilities;
    private ProgramCapabilityRegistry ProgramCapabilities =>_programCapabilities ??= MatchRuntime.Capabilities;
}
