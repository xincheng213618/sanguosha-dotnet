#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDuanJiongOlRuntime
{
    internal bool IsSaoguPaidDestination(ProgramSkillFrame frame, CardMovementRecord movement) => movement.To == CardLocation.DiscardPile || _coreWushuangShopCardsOperations.IsWushuangShopDestroyedMaterial(movement) && _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangShopEquipmentDestroyedEvent>().Count(e => e.CardId == movement.CardId && e.MovementSequence == movement.Sequence && e.ParentFrameId == frame.Id) == 1;
    internal bool ValidSaogu(ProgramSkillFrame f)
    {
        if (f.DuanJiongSaogu is not { } r || !HasDuanJiongSaoguCapability || f.SkillId != "ol:saogu" || r.Source != DuanJiongSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != f.InstructionIndex || f.InstructionIndex != 1 || r.Phase.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.Phase.ActorSeat != f.OwnerSeat || r.Phase.ActualTurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActorSeat) || !Enum.IsDefined(r.Stage) || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveSaoguConversionSweep || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Phase == r.Phase && e.Ending == r.Ending && e.Yang == r.Yang) != 1)
            return false;
        if (r.Ending)
        {
            if (f.TriggerId != "delegate-current" || f.ActivationId != f.TriggerId || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Ending || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == context.ParentFrameId)is not TurnEndingBoundaryFrame parent || parent.OwnerSeat != f.OwnerSeat || parent.TurnNumber != r.Phase.TurnNumber || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } || context.SourceSeat != parent.OwnerSeat || context.TargetSeat != parent.OwnerSeat || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate))
                return false;
        }
        else if (f.ActivationId != "sweep" || f.TriggerId is not null || f.WindowContext is not null || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.ActorSeat != f.OwnerSeat || r.DelegateFee is not null)
            return false;
        if (r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.DrawCount is < 0 or > 1 || r.RequiredCount is < 0 or > 2 || r.ForbiddenSuitMask is < 0 or > 15 || r.Candidates.Select(c => c.CardId).Distinct().Count() != r.Candidates.Count || r.Selected.Select(c => c.CardId).Distinct().Count() != r.Selected.Count || r.Payments.Select(c => c.CardId).Distinct().Count() != r.Payments.Count || r.Payments.Count > 2 || r.UsedCardIds.Distinct().Count() != r.UsedCardIds.Count || r.UsedCardIds.Any(id => !r.Payments.Any(c => c.CardId == id && CoreNativeCardClassificationsRuntime.IsSlashCard(c.Kind))) || r.Selected.Any(c => c.CardId <= 0 || c.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            return false;
        var entities = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguEntityPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        foreach (var e in entities)
        {
            var card = e.DelegateFee ? r.DelegateFee : r.Payments.SingleOrDefault(c => c.CardId == e.CardId);
            if (card is null || card.CardId != e.CardId || card.From != e.From || card.Kind != e.Kind || card.Suit != e.Suit || e.ActorSeat != (e.DelegateFee ? f.OwnerSeat : r.ActorSeat) || e.From.OwnerSeat != e.ActorSeat || e.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || entities.Count(p => p.DelegateFee == e.DelegateFee && p.CardId == e.CardId) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && IsSaoguPaidDestination(f, m) && m.Reason.Value == SaoguDiscardReason) != 1)
                return false;
        }

        if (r.DelegateFee is not null && entities.Count(e => e.DelegateFee) != 1 || r.DelegateFee is null && entities.Any(e => e.DelegateFee) || entities.Count(e => !e.DelegateFee) != r.Payments.Count || r.Ending && r.Stage != DuanJiongSaoguStage.DelegateChoice && (r.DelegateFee is null || r.ActorSeat == f.OwnerSeat))
            return false;
        var branch = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguBranchPaidEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (branch.Length > 1 || branch.Any(e => e.ActorSeat != r.ActorSeat || e.Yang != r.Yang || e.Converted == r.Ending || e.ActualCount != (r.Yang ? r.Payments.Count : r.DrawCount)))
            return false;
        if (r.Stage == DuanJiongSaoguStage.DiscardChoice && (r.Selected.Count >= r.RequiredCount || r.Selected.Any(c => !r.Candidates.Contains(c)) || r.Payments.Count != 0 || branch.Length != 0))
            return false;
        if (r.Stage is DuanJiongSaoguStage.DiscardChildren or DuanJiongSaoguStage.DrawChildren or DuanJiongSaoguStage.UseChoice or DuanJiongSaoguStage.NativeUse or DuanJiongSaoguStage.NativeChildren)
            if (!r.Paid || branch.Length != 1 || r.Yang && r.Payments.Count != r.RequiredCount || !r.Yang && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(r.ActorSeat) && m.Reason.Value == SaoguDrawReason) != r.DrawCount)
                return false;
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguNativeIssuedEvent>().Where(e => e.Origin.ProgramFrameId == f.Id).ToArray();
        var returned = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DuanJiongSaoguNativeReturnedEvent>().Where(e => e.Origin.ProgramFrameId == f.Id).ToArray();
        if (issued.Select(e => e.Origin.CardId).Distinct().Count() != issued.Length || issued.Any(e => !r.UsedCardIds.Contains(e.Origin.CardId) || e.Origin.ActorSeat != r.ActorSeat || !CoreNativeCardClassificationsRuntime.IsSlashCard(e.Origin.Kind)) || returned.Any(e => issued.Count(i => i.Origin == e.Origin) != 1) || returned.Select(e => e.Origin).Distinct().Count() != returned.Length)
            return false;
        if (r.Stage == DuanJiongSaoguStage.NativeUse)
            return r.SelectedUseCardId is { } id && r.ReservedUseId is> 0 && r.UsedCardIds.Contains(id) && (r.NativeUse is null ? issued.Length == r.UsedCardIds.Count - 1 : r.NativeUse.CardId == id && r.NativeUse.CardUseFrameId == r.ReservedUseId && issued.Count(e => e.Origin == r.NativeUse) == 1) && returned.Length == r.UsedCardIds.Count - 1;
        return issued.Length == r.UsedCardIds.Count && returned.Length == issued.Length;
    }
}
