#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiaoQiaoLeRuntime
{
    internal bool HasPaidWeiwanSelection(ProgramSkillFrame f)
    {
        if (f.Weiwan is not { CostSequence: > 0 } r || f.SkillId != "le:weiwan" || f.ActivationId != "discard-qin-obtain-suits" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || r.Source != WeiwanSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || !f.SelectedCardIds.SequenceEqual([r.CardId]) || !f.SelectedTargetSeats.SequenceEqual([r.TargetSeat]) || r.CostFrom.OwnerSeat != f.OwnerSeat || r.CostFrom.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || r.CostSequence <= r.SequenceBefore || r.CostSequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !HasQiqinEntityMark(r.CardId) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var start] || start != new WeiwanStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.ActualTurn, r.TurnOwner, r.CardId, r.PrintedKind, r.CostFrom, r.CostSuit, r.TargetSeat, r.SequenceBefore) || _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanCostPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid != new WeiwanCostPaidEvent(f.Id, r.CostBatchId, r.CardId, r.PrintedKind, r.CostFrom, r.CostTo, r.CostSequence))
            return false;
        return _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == r.CostSequence && m.CardId == r.CardId && m.CardKind == r.PrintedKind && m.From == r.CostFrom && m.To == r.CostTo && m.Reason.Value == WeiwanCostReason) == 1;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 220)]
    internal bool IsWeiwanMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Weiwan is { } r && HasPaidWeiwanSelection(f) && effect?.Op == SkillProgramEffectOp.ResolveWeiwanQinDiscardAndSuitObtain && pending == f.PendingMovementContinuation && pending.BeforeCount == 0 && pending.CoverageResultBind is null && pending.SubjectSeat == (r.Stage is WeiwanStage.CostChildren or WeiwanStage.ObtainChildren ? f.OwnerSeat : r.TargetSeat);
    internal bool WeiwanPrimaryMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.Weiwan!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        return r.Stage == WeiwanStage.CostChildren ? m.Sequence == r.CostSequence && m.CardId == r.CardId && m.From == r.CostFrom && m.To == r.CostTo && m.Reason.Value == WeiwanCostReason : r.Stage == WeiwanStage.ObtainChildren && _coreProgramEventHistoryOperations.ProgramEventHistory<WeiwanEntityObtainedEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.MovementSequence == m.Sequence && e.From == m.From && e.To == m.To && m.Reason.Value == WeiwanObtainReason);
    }

    internal bool WeiwanOxSpill(ProgramSkillFrame f, CardMovementBatchContext batch)
    {
        var r = f.Weiwan!;
        var originalBatch = r.Stage == WeiwanStage.CostChildren ? r.CostBatchId : r.Stage == WeiwanStage.ObtainChildren ? r.ObtainBatchId : 0;
        if (originalBatch <= 0 || batch.ParentBatchId != originalBatch || batch.Movements.Count == 0)
            return false;
        var oxOwners = r.Stage == WeiwanStage.CostChildren ? r.PrintedKind == CardKind.WoodenOx && r.CostFrom.Zone == CardZoneKind.Equipment ? new[]
        {
            f.OwnerSeat
        }

        : [] : r.Samples.Where(s => s.PrintedKind == CardKind.WoodenOx && s.From is { Zone: CardZoneKind.Equipment }).Select(s => s.From!.Value.OwnerSeat!.Value).ToArray();
        return batch.Movements.All(m => m.From is { Zone: CardZoneKind.WoodenOxGrain, OwnerSeat: { } owner } && oxOwners.Contains(owner) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
    }
}
