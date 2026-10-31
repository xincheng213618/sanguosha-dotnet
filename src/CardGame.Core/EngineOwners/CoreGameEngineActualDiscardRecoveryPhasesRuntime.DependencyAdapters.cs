#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ActualDiscardRecoveryPhaseRuntime))]
internal sealed class CoreActualDiscardRecoveryPhasesActualDiscardRecoveryPhaseAdapter(Func<ActualDiscardRecoveryPhaseRuntime> targetFactory) : ICoreActualDiscardRecoveryPhasesActualDiscardRecoveryPhaseOperations
{
    private readonly Func<ActualDiscardRecoveryPhaseRuntime> _targetFactory = targetFactory;
    public ActualDiscardRecoveryPhaseRuntime Get() => this._targetFactory();
}
