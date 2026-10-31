#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramCapability(typeof(ActualPhaseEndRuntime))]
internal sealed class CoreActualPhaseEndsActualPhaseEndAdapter(Func<ActualPhaseEndRuntime> targetFactory) : ICoreActualPhaseEndsActualPhaseEndOperations
{
    private readonly Func<ActualPhaseEndRuntime> _targetFactory = targetFactory;
    public ActualPhaseEndRuntime Get() => this._targetFactory();
}
