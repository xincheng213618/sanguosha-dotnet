#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuLeRuntime
{
    internal bool ValidPingluStarted(ProgramSkillFrame frame)
    {
        if (frame.Pinglu is not { } r || r.Source != PingluSource(frame) || r.GameplayHash != frame.GameplayHash || frame.SkillId != "le:pinglu" || frame.ActivationId != "all-in-range-random-hand" || frame.TriggerId is not null || frame.WindowContext is not null || frame.InstructionIndex != 1 || r.InstructionIndex != frame.InstructionIndex || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats.Count != 0 || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != r.GameplayHash || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != frame.OwnerSeat || phase.TurnNumber != r.ActualTurn || phase.ActualTurnOwnerSeat != r.TurnOwner || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<PingluStartedEvent>().Where(e => e.FrameId == frame.Id).ToArray()is [var started] && started == new PingluStartedEvent(frame.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner, r.Targets.Count, r.SequenceBefore) && // Active programs publish ProgramSkillStarted, while lifecycle
 // triggers publish ProgramBindingStarted. The signed Pinglu fact
        // above retains this activation's exact source instance and hash.
        _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.ActivationId == frame.ActivationId && e.OwnerSeat == frame.OwnerSeat) == 1;
    }

    internal bool ValidPingluPaid(ProgramSkillFrame frame)
    {
        if (!ValidPingluStarted(frame) || frame.Pinglu is not { ActualCount: >= 0, BatchId: > 0 } r || r.Before != r.SequenceBefore || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        var selected = r.Targets.Where(s => s.CardId is not null).ToArray();
        var entries = _coreProgramEventHistoryOperations.ProgramEventHistory<PingluEntityObtainedEvent>().Where(e => e.FrameId == frame.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length == 0 || entries.Length != selected.Length || r.ActualCount != entries.Count(e => e.To == CardLocation.Hand(frame.OwnerSeat)) || _coreProgramEventHistoryOperations.ProgramEventHistory<PingluObtainPaidEvent>().Where(e => e.FrameId == frame.Id).ToArray()is not [var paid] || paid != new PingluObtainPaidEvent(frame.Id, r.BatchId, selected.Length, r.ActualCount, r.Before, r.After))
            return false;
        for (var i = 0; i < entries.Length; i++)
        {
            var entry = entries[i];
            var sample = selected[i];
            if (entry.Index != i || entry.TargetSeat != sample.TargetSeat || entry.CardId != sample.CardId || entry.PrintedKind != sample.PrintedKind || entry.From != sample.From || entry.From != CardLocation.Hand(sample.TargetSeat) || entry.To == entry.From || entry.BatchId != r.BatchId || entry.Sequence <= r.Before || entry.Sequence > r.After || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == entry.CardId && m.CardKind == entry.PrintedKind && m.From == entry.From && m.To == entry.To && m.Sequence == entry.Sequence && m.Reason.Value == PingluObtainReason) != 1)
                return false;
        }

        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == PingluObtainReason) == entries.Length;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 640)]
    internal bool IsPingluMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => frame.Pinglu is { Stage: PingluStage.ObtainChildren } && effect?.Op == SkillProgramEffectOp.ResolvePingluInRangeRandomHands && frame.PendingMovementContinuation == pending && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && ValidPingluPaid(frame);
}
