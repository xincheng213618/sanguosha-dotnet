using System.Reflection;

namespace CardGame.Core;
// Transitional access to legacy core adapters. Independent rule modules only
// receive the dedicated contracts resolved by Capabilities.
internal interface IMatchRuntimeAdapterFactory
{
    object CreateAdapter(ProgramCapabilityCatalog.Registration registration);
    Delegate BindCoreHandler(MethodInfo method, Type delegateType);
    object CoreHandlerTarget(MethodInfo method);
}

/// <summary>
/// Owns the executable instances and handler bindings of one match. Catalogs
/// contain immutable definitions; constructing a match only wires factories.
/// Resolution frames remain the sole owners of pending rule state.
/// </summary>
internal sealed class MatchRuntime
{
    private readonly ProgramRuntimeModuleRegistry _modules;
    private readonly IMatchRuntimeAdapterFactory _adapters;
    internal ProgramCapabilityRegistry Capabilities { get; }

    internal MatchRuntime(ProgramRuntimeModuleCatalog modules, ProgramCapabilityCatalog capabilities, IMatchRuntimeAdapterFactory adapters, IEnumerable<ProgramNativeCapabilityBinding>? nativeBindings = null, ResolutionScheduler? resolutionScheduler = null)
    {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(capabilities);
        ArgumentNullException.ThrowIfNull(adapters);
        _adapters = adapters;
        _modules = new(modules, ResolveCapability);
        var bindings = (nativeBindings ?? []).ToList();
        if (resolutionScheduler is not null)
        {
            var routing = new NativeFrameRoutingProvider(_modules);
            var frames = new ProgramRuntimeFrameServices(resolutionScheduler, routing);
            bindings.Add(new(typeof(IProgramRuntimeFrames), frames));
            bindings.Add(new(typeof(INativeFrameRoutingFacts), resolutionScheduler));
        }

        Capabilities = new(capabilities, registration => registration.IsRuntimeModule ? _modules.Get(registration.Implementation) : registration.DeferredModule is not null ? registration.CreateDeferredAdapter(_modules.Get) : _adapters.CreateAdapter(registration), global::CardGame.Core.RuntimeAccess.ClosedCapabilityComposition.Append(bindings, modules, () => (global::CardGame.Core.IHandCardViewQueries)this.ResolveCapability(typeof(global::CardGame.Core.IHandCardViewQueries)), () => (global::CardGame.Core.INativeGivenCardQueries)this.ResolveCapability(typeof(global::CardGame.Core.INativeGivenCardQueries)), () => (global::CardGame.Core.IOwnedCardMoveQueries)this.ResolveCapability(typeof(global::CardGame.Core.IOwnedCardMoveQueries)), () => (global::CardGame.Core.IProgramOperationDraws)this.ResolveCapability(typeof(global::CardGame.Core.IProgramOperationDraws)), () => (global::CardGame.Core.IProgramPhaseFrameQueries)this.ResolveCapability(typeof(global::CardGame.Core.IProgramPhaseFrameQueries)), () => (global::CardGame.Core.ISelectionParticipantQueries)this.ResolveCapability(typeof(global::CardGame.Core.ISelectionParticipantQueries))));
    }

    private object ResolveCapability(Type contract) => Capabilities.Get(contract);
    internal object GetModule(Type type) => _modules.Get(type);
    internal Delegate BindHandler(MethodInfo method, Type delegateType) => _modules.Bind(method, delegateType, _adapters.BindCoreHandler);
    internal object? HandlerTarget(MethodInfo method) => method.IsStatic ? null : method.DeclaringType == typeof(GameEngine) ? _adapters.CoreHandlerTarget(method) : _modules.Get(method.DeclaringType!);
}
