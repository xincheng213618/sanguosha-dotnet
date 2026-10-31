#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuFuRenOlRuntime
{
    internal bool TryReturnDuFuRenPaidPreparation(ProgramLifecycleTriggerWindowFrame frame)
    {
        if (!HasDuFuRenShunshi || frame.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || frame.Continuation != ProgramLifecycleContinuation.NormalTurnStart || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(frame.OwnerSeat) || _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || frame.CandidateIndex != frame.Candidates.Count || _resolutionStackOperations._resolutionStack.Count != 0)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenStartedEvent>().Where(start => start.WindowId == frame.Id && start.Stage == DuFuRenStage.GiftChoice && start.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && start.TurnOwner == frame.OwnerSeat && start.Source.OwnerSeat == frame.OwnerSeat && start.DamageFrameId is null && start.BatchId is null && frame.Candidates.Any(c => c.OwnerSeat == start.Source.OwnerSeat && c.SkillId == start.Source.SkillId && c.BindingId == start.Source.BindingId && c.SkillInstanceId == start.Source.SkillInstanceId && c.GameplayHash == start.GameplayHash && _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.GiveForNextActualPhases }]) && _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenCompletedEvent>().Count(e => e.FrameId == start.FrameId && e.Applied) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<DuFuRenGiftIssuedEvent>().Count(g => g.FrameId == start.FrameId && g.Source == start.Source && g.GameplayHash == start.GameplayHash && g.Turn == start.Turn && g.TurnOwner == start.TurnOwner && g.From.OwnerSeat == frame.OwnerSeat && g.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == g.MovementSequence && m.CardId == g.CardId && m.From == g.From && m.To == CardLocation.Hand(g.RecipientSeat) && m.Reason.Value == DuFuRenGiftReason) == 1) == 1).ToArray();
        if (paid.Length == 0)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<TurnEndedEvent>().Any(e => e.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && e.ActorSeat == frame.OwnerSeat))
            throw new InvalidOperationException("A paid Shunshi preparation cannot finalize an already ended actual turn twice.");
        // The paid native subtree has fully returned and this exact preparation
        // window has been popped. No Draw or Ending is invented for the dead
        // actor. The existing finalizer owns expiry, deferred end work and the
        // next living actor when the game itself has not ended.
        _coreActualDiscardRecoveryPhasesOperations.ClearActualDiscardRecoveryPhase();
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            _coreActualTurnsOperations.FinalizeEndTurn(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        return true;
    }
}
