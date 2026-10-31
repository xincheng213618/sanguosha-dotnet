#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal bool ValidMouZuifengHistory(long frameId, out MouZuifengStartedEvent started, out MouZuifengCostPaidEvent paid)
    {
        started = null !;
        paid = null !;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengStartedEvent>().Where(e => e.FrameId == frameId).ToArray()is not [var s] || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengCostPaidEvent>().Where(e => e.FrameId == frameId).ToArray()is not [var p] || s.Source.SkillId != MouZuifengSkill || s.Source.BindingId != MouZuifengBinding || s.Source != p.Source || s.GameplayHash != p.GameplayHash || s.Phase != p.Phase || s.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || s.Phase.ActorSeat != s.Source.OwnerSeat || s.Phase.TurnNumber != s.ActualTurn || s.Phase.ActualTurnOwnerSeat != s.TurnOwner || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(s.Phase) || _contentRegistryOperations._contentRegistry.GetSkill(s.Source.SkillId).Program?.GameplayHash != s.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == frameId && e.SkillId == s.Source.SkillId && e.ActivationId == s.Source.BindingId && e.OwnerSeat == s.Source.OwnerSeat) != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(p.TargetSeat) || p.CardId <= 0 || p.BatchId <= 0 || p.From != CardLocation.Hand(s.Source.OwnerSeat) || p.To == p.From || p.EffectiveSuit is not (Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond) || p.Before < 0 || p.Sequence <= p.Before || p.After < p.Sequence || p.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || p.MaxHpAtPayment <= 0 || p.UseOrdinal <= 0 || p.UseOrdinal > p.MaxHpAtPayment || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengCostChosenEvent>().Count(e => e == new MouZuifengCostChosenEvent(frameId, p.CardId, p.PrintedKind, p.EffectiveSuit)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == p.Sequence && m.CardId == p.CardId && m.CardKind == p.PrintedKind && m.From == p.From && m.To == p.To && m.Reason.Value == MouZuifengDiscardReason) != 1)
            return false;
        var payments = _coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengCostPaidEvent>().Where(e => e.Source.OwnerSeat == s.Source.OwnerSeat && e.Phase == s.Phase).ToArray();
        if (payments.Count(e => e.Sequence <= p.Sequence) != p.UseOrdinal || payments.Select(e => e.Sequence).Distinct().Count() != payments.Length)
            return false;
        started = s;
        paid = p;
        return true;
    }

    internal bool ValidMouZuifengStarted(ProgramSkillFrame f) => f.MouZuifeng is { } r && f.SkillId == MouZuifengSkill && f.ActivationId == MouZuifengBinding && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && f.InstructionIndex == 1 && r.InstructionIndex == f.InstructionIndex && r.Source == MouZuifengSource(f) && r.GameplayHash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == r.GameplayHash && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && r.Phase.ActorSeat == f.OwnerSeat && r.Phase.TurnNumber == r.ActualTurn && r.Phase.ActualTurnOwnerSeat == r.TurnOwner && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZuifengStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var s] && s == new MouZuifengStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId && e.OwnerSeat == f.OwnerSeat) == 1;
    internal bool IsPaidMouZuifeng(ProgramSkillFrame f) => f.MouZuifeng is { Paid: true } r && ValidMouZuifengStarted(f) && ValidMouZuifengHistory(f.Id, out var s, out var p) && s.Source == r.Source && p.TargetSeat == r.TargetSeat && p.CardId == r.CostId && p.PrintedKind == r.CostKind && p.EffectiveSuit == r.CostSuit && p.BatchId == r.CostBatchId && p.Sequence == r.CostSequence;
    internal bool MouZuifengCostBatch(ProgramSkillFrame f, CardMovementBatchContext b) => f.MouZuifeng is { Stage: MouZuifengStage.CostChildren } r && IsPaidMouZuifeng(f) && b.Id == r.CostBatchId && b.ParentFrameId == f.Id && b.OriginOwnerSeat == f.OwnerSeat && b.OriginSkillId == f.SkillId && b.OriginSkillInstanceId == f.SkillInstanceId && b.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence == r.CostSequence && m.CardId == r.CostId && m.CardKind == r.CostKind && m.From == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouZuifengDiscardReason;
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 430)]
    internal bool IsMouZuifengMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.MouZuifeng is { Stage: MouZuifengStage.CostChildren or MouZuifengStage.NativeChildren } && IsPaidMouZuifeng(f) && effect?.Op == SkillProgramEffectOp.ResolveMouZhouYuZuifeng && f.PendingMovementContinuation == pending && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
}
