#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreForeignActualTurnWindowRuntimePortsRuntime
{
    Winner IForeignActualTurnWindowFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus IForeignActualTurnWindowFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    SkillProgramTriggerFacts IForeignActualTurnWindowFacts.CaptureTriggerFacts(int ownerSeat) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
    IReadOnlyList<ProgramTriggerCandidate> IForeignActualTurnWindowFacts.CollectCandidates(int ownerSeat, SkillProgramTriggerFacts facts) => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramTriggerWindow.OtherActualTurnStarted, facts);
    int IForeignActualTurnWindowFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    void IForeignActualTurnWindowReturns.CompleteGame() => _kernelCoreMainOperations.CompleteGame();
    void IForeignActualTurnWindowReturns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void IForeignActualTurnWindowReturns.AfterForeignActualContests(int ownerSeat) => _coreActualTurnsOperations.ContinueTurnAfterForeignActualContests(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
}
