#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ProgramTriggerRuntime))]
internal sealed class CoreTurnDiscardSuitHistoryProgramTriggerAdapter(Func<ProgramTriggerRuntime> targetFactory) : ICoreTurnDiscardSuitHistoryProgramTriggerOperations
{
    private readonly Func<ProgramTriggerRuntime> _targetFactory = targetFactory;
    public ProgramTriggerRuntime Get() => this._targetFactory();
}
