namespace CardGame.Core;

internal interface IForeignActualTurnWindowFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    EngineStatus Status { get; }
    SkillProgramTriggerFacts CaptureTriggerFacts(int ownerSeat);
    IReadOnlyList<ProgramTriggerCandidate> CollectCandidates(int ownerSeat, SkillProgramTriggerFacts facts);
    int RelativeTableOrder(int from, int target);
}

// Only the final native turn transition remains with its existing owner.
internal interface IForeignActualTurnWindowReturns : IProgramRuntimeOperations
{
    void CompleteGame();
    void EndTurn();
    void AfterForeignActualContests(int ownerSeat);
}

public sealed partial class GameEngine
{
}
