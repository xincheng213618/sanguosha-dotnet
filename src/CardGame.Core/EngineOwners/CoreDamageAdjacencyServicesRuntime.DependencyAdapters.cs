#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(CoreProgramRuntimeHooksRuntime))]
internal sealed class CoreDamageAdjacencyServicesCoreProgramRuntimeHooksAdapter(Func<CoreProgramRuntimeHooksRuntime> targetFactory) : ICoreDamageAdjacencyServicesCoreProgramRuntimeHooksOperations
{
    private readonly Func<CoreProgramRuntimeHooksRuntime> _targetFactory = targetFactory;
    public ProgramRuntimeHooks RuntimeHooks { get => this._targetFactory().RuntimeHooks; }
}

[Core.ProgramCapability(typeof(DamageAdjacencyRuntime))]
internal sealed class CoreDamageAdjacencyServicesDamageAdjacencyAdapter(Func<DamageAdjacencyRuntime> targetFactory) : ICoreDamageAdjacencyServicesDamageAdjacencyOperations
{
    private readonly Func<DamageAdjacencyRuntime> _targetFactory = targetFactory;
    public DamageAdjacencyRuntime Get() => this._targetFactory();
}
