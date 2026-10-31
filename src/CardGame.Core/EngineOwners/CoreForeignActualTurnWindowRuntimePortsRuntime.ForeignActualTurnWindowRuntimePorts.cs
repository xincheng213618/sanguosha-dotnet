#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreForeignActualTurnWindowRuntimePortsRuntime
{
    Winner global::CardGame.Core.IForeignActualTurnWindowFacts.Winner => _coreMatchOutcomeStatusWinnerAccess._winner;

    EngineStatus global::CardGame.Core.IForeignActualTurnWindowFacts.Status => _coreMatchOutcomeStatusWinnerAccess._status;

    SkillProgramTriggerFacts global::CardGame.Core.IForeignActualTurnWindowFacts.CaptureTriggerFacts(int ownerSeat) => _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
    IReadOnlyList<ProgramTriggerCandidate> global::CardGame.Core.IForeignActualTurnWindowFacts.CollectCandidates(int ownerSeat, SkillProgramTriggerFacts facts) => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[ownerSeat], SkillProgramTriggerWindow.OtherActualTurnStarted, facts);
    int global::CardGame.Core.IForeignActualTurnWindowFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    void global::CardGame.Core.IForeignActualTurnWindowReturns.CompleteGame() => _kernelCoreMainOperations.CompleteGame();
    void global::CardGame.Core.IForeignActualTurnWindowReturns.EndTurn() => _coreActualTurnsOperations.EndTurn();
    void global::CardGame.Core.IForeignActualTurnWindowReturns.AfterForeignActualContests(int ownerSeat) => _coreActualTurnsOperations.ContinueTurnAfterForeignActualContests(_coreMatchParticipantsPlayersAccess._players[ownerSeat]);
}
