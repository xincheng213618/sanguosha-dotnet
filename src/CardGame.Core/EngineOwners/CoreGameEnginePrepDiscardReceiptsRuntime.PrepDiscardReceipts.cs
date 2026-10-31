#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrepDiscardReceiptsRuntime
{
    internal bool ValidPrepDiscardPayment(ProgramSkillFrame f, PrepDiscardPayment p, bool ownerCost)
    {
        var d = f.PrepDiscard!;
        if (p.PayerSeat != (ownerCost ? f.OwnerSeat : d.TargetSeat) || p.CardIds.Count <= 0 || p.CardIds.Count != p.From.Count || p.CardIds.Distinct().Count() != p.CardIds.Count || p.ActualCount != p.CardIds.Count || p.NonEquipmentCount < 0 || p.NonEquipmentCount > p.ActualCount || p.SequenceBefore < 0 || p.SequenceAfter <= p.SequenceBefore || p.SequenceAfter > _corePrepDiscardPaymentsOperations.PrepDiscardSequence || p.From.Any(l => l.OwnerSeat != p.PayerSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            return false;
        var reason = ownerCost ? "skill-program.preparation-discard.owner.ResolvePrepDiscardOrEnding" : "skill-program.preparation-discard.target.ResolvePrepDiscardOrEnding";
        var records = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > p.SequenceBefore && m.Sequence <= p.SequenceAfter).ToArray();
        var primary = records.Where(m => p.CardIds.Contains(m.CardId)).ToArray();
        if (primary.Length != p.CardIds.Count || p.CardIds.Where((id, index) => primary.Count(m => m.CardId == id && m.From == p.From[index] && m.To == CardLocation.DiscardPile && m.Reason.Value == reason) != 1).Any() || primary.Count(m => !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(m.CardKind)) != p.NonEquipmentCount)
            return false;
        // A real WoodenOx removal may discard its private stored entities as a
        // separate native cleanup; those are not extra selected costs or N.
        if (records.Except(primary).Any(m => m.From != CardLocation.Grain(p.PayerSeat) || m.To != CardLocation.DiscardPile || m.Reason != CardMoveReasons.WoodenOxGrainDiscard || !primary.Any(paid => paid.CardKind == CardKind.WoodenOx && paid.From == CardLocation.Equipment(p.PayerSeat))))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == d.Source && e.GameplayHash == d.GameplayHash && e.ActualTurnNumber == d.ActualTurnNumber && e.ActualTurnOwnerSeat == d.ActualTurnOwnerSeat && e.PayerSeat == p.PayerSeat && e.OwnerCost == ownerCost && e.SequenceBefore == p.SequenceBefore && e.SequenceAfter == p.SequenceAfter && e.ActualCount == p.ActualCount && e.NonEquipmentCount == p.NonEquipmentCount) == 1;
    }

    internal bool ValidPrepDiscard(ProgramSkillFrame f)
    {
        if (f.PrepDiscard is not { } d || !_corePrepDiscardPaymentsOperations.MatchesActualPrepDiscard(f) || f.InstructionIndex != 1 || d.InstructionIndex != 0 || d.Source != _corePrepDiscardPaymentsOperations.PrepDiscardSource(f) || d.GameplayHash != f.GameplayHash || d.ActualTurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || d.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || d.ActualTurnOwnerSeat != f.OwnerSeat || d.PrepWindowId != f.WindowContext!.ParentFrameId || !Enum.IsDefined(d.Stage) || d.RequiredCount < 0 || d.SelectedCardIds.Count != d.SelectedFrom.Count || d.SelectedCardIds.Distinct().Count() != d.SelectedCardIds.Count || d.SelectedCardIds.Count > d.RequiredCount || d.SelectedFrom.Any(l => l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || l.OwnerSeat != (d.Stage == PrepDiscardStage.SelectingOwnerCards ? f.OwnerSeat : d.TargetSeat)))
            return false;
        if (d.TargetSeat is { } target)
        {
            if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || d.RequestedCount != Math.Max(1, d.FrozenHandCount - d.FrozenHp) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrepDiscardTargetFrozenEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == d.Source && e.GameplayHash == d.GameplayHash && e.ActualTurnNumber == d.ActualTurnNumber && e.ActualTurnOwnerSeat == d.ActualTurnOwnerSeat && e.PrepWindowId == d.PrepWindowId && e.TargetSeat == target && e.HandCount == d.FrozenHandCount && e.Hp == d.FrozenHp && e.RequestedCount == d.RequestedCount && e.RequiredCount > 0 && e.RequiredCount <= e.RequestedCount && (d.TargetPayment is null ? e.RequiredCount == d.RequiredCount : e.RequiredCount == d.TargetPayment.ActualCount)) != 1)
                return false;
        }
        else if (d.Stage is not (PrepDiscardStage.ChoosingTarget or PrepDiscardStage.Complete) || d.TargetPayment is not null || d.OwnerPayment is not null || d.RequestedCount != 0 || d.RequiredCount != 0 || d.SelectedCardIds.Count != 0)
            return false;
        if (d.TargetPayment is { } targetPayment && !ValidPrepDiscardPayment(f, targetPayment, false) || d.OwnerPayment is { } ownerPayment && (!ValidPrepDiscardPayment(f, ownerPayment, true) || d.TargetPayment is null || ownerPayment.ActualCount != d.TargetPayment.NonEquipmentCount || d.Deferred != false))
            return false;
        if ((d.Stage is PrepDiscardStage.TargetChildren or PrepDiscardStage.ChoosingBenefit or PrepDiscardStage.SelectingOwnerCards or PrepDiscardStage.OwnerChildren or PrepDiscardStage.ImmediateDrawAttempt or PrepDiscardStage.ImmediateDrawChildren) && d.TargetPayment is null || d.Stage == PrepDiscardStage.OwnerChildren && d.OwnerPayment is null || d.Stage != PrepDiscardStage.Complete && !f.ReexecuteParticipantInstruction)
            return false;
        if (!_corePrepDiscardImmediateDrawOperations.ValidPrepDiscardImmediateDraw(f))
            return false;
        var payment = d.Stage == PrepDiscardStage.TargetChildren ? d.TargetPayment : d.Stage == PrepDiscardStage.OwnerChildren ? d.OwnerPayment : null;
        return f.PendingMovementContinuation is not { } pending || pending.BeforeCount == 0 && pending.CoverageResultBind is null && (payment is not null && pending.SubjectSeat == payment.PayerSeat || d.Stage == PrepDiscardStage.ImmediateDrawChildren && d.ImmediateDraw?.TargetSeat == pending.SubjectSeat);
    }
}
