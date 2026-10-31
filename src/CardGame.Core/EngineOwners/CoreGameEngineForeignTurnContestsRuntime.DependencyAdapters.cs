#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ForeignActualTurnWindowRuntime))]
internal sealed class CoreForeignTurnContestsForeignActualTurnWindowAdapter(Func<ForeignActualTurnWindowRuntime> targetFactory) : ICoreForeignTurnContestsForeignActualTurnWindowOperations
{
    private readonly Func<ForeignActualTurnWindowRuntime> _targetFactory = targetFactory;
    public ForeignActualTurnWindowRuntime Get() => this._targetFactory();
}

[Core.ProgramCapability(typeof(ForeignTurnContestRuntime))]
internal sealed class CoreForeignTurnContestsForeignTurnContestAdapter(Func<ForeignTurnContestRuntime> targetFactory) : ICoreForeignTurnContestsForeignTurnContestOperations
{
    private readonly Func<ForeignTurnContestRuntime> _targetFactory = targetFactory;
    public ForeignTurnContestRuntime Get() => this._targetFactory();
}
