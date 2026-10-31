#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ForeignTurnContestRuntime))]
internal sealed class CoreForeignContestAidObserversForeignTurnContestAdapter(Func<ForeignTurnContestRuntime> targetFactory) : ICoreForeignContestAidObserversForeignTurnContestOperations
{
    private readonly Func<ForeignTurnContestRuntime> _targetFactory = targetFactory;
    public ForeignTurnContestRuntime Get() => this._targetFactory();
}
