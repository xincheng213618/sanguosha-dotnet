#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMiHengLeRuntime
{
    internal bool ValidSiruiInvoice(ResolutionFrame frame, SiruiUseReceipt r, bool requirePayment)
    {
        if (r.OwnerFrameId != frame.Id || r.Source.OwnerSeat != r.Phase.ActorSeat || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.Phase.ActualTurnOwnerSeat != r.Source.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || !IsSiruiSource(r.Source) || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program!.GameplayHash != r.GameplayHash || r.From.OwnerSeat != r.Source.OwnerSeat || r.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || SiruiNameLength(r.PrintedKind) != SiruiNameLength(_corePlayCardHistoryOperations.ViewAsRule(r.Source)!.OutputKind) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiPhaseUseIssuedEvent>().Count(e => e.OwnerFrameId == frame.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Phase == r.Phase && e.NativeKind == r.NativeKind && e.OutputKind == r.OutputKind && e.CardId == r.CardId && e.PrintedKind == r.PrintedKind && e.From == r.From && e.SequenceBefore == r.SequenceBefore) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiPhaseUseIssuedEvent>().Count(e => e.Source.OwnerSeat == r.Source.OwnerSeat && e.Phase.Token == r.Phase.Token) != 1)
            return false;
        if (r.NativeKind == SiruiNativeUseKind.CardUse)
        {
            if (frame is not CardUseFrame { Action: { Type: CardActionType.Use } action } use || action.ActionId != r.ActionId || action.ActorSeat != r.Source.OwnerSeat || action.ProviderSeat != r.Source.OwnerSeat || !action.ConversionChain.Contains(r.Source) || action.PhysicalCards is not [var cost] || cost.CardId != r.CardId || cost.From != r.From || cost.CardKind != r.PrintedKind || use.PhysicalCardIds is not [var id] || id != r.CardId || action.EffectiveKind != r.OutputKind && !(CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && CoreNativeCardClassificationsRuntime.IsSlashCard(r.OutputKind)))
                return false;
        }
        else if (r.NativeKind == SiruiNativeUseKind.Dodge && frame is not CardUseFrame || r.NativeKind == SiruiNativeUseKind.Nullification && frame is not NullificationWindowFrame)
            return false;
        if (r.PaymentSequence == 0)
            return !requirePayment;
        var receipt = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiMaterialPaidEvent>().SingleOrDefault(e => e.OwnerFrameId == frame.Id);
        return receipt is not null && receipt.CardId == r.CardId && receipt.From == r.From && receipt.MovementSequence == r.PaymentSequence && r.PaymentSequence > r.SequenceBefore && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == r.PaymentSequence && m.CardId == r.CardId && m.From == r.From && m.To == receipt.To && m.Reason == receipt.Reason) == 1;
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SlashCountExemption, 50)]
    internal bool HasIssuedSiruiUse(long useId, int actorSeat) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Sirui: { NativeKind: SiruiNativeUseKind.CardUse } r } use && r.Source.OwnerSeat == actorSeat && ValidSiruiInvoice(use, r, requirePayment: false);
    internal bool IsSiruiOutsideGeneralWeaponMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Sirui: { NativeKind: SiruiNativeUseKind.CardUse, PrintedKind: CardKind.GeneralWeapon, PaymentSequence: > 0 } r } use && r.CardId == cardId && r.From.Zone == CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiMaterialPaidEvent>().Any(e => e.OwnerFrameId == useId && e.CardId == cardId && e.From == r.From && e.To == CardLocation.OutsideGame && e.MovementSequence == r.PaymentSequence && e.Reason == CardMoveReasons.Use) && ValidSiruiInvoice(use, r, requirePayment: true);
    internal int? SiruiResponseRank(CardConversionSource? source, IReadOnlyList<CardActionCost> costs)
    {
        if (!IsSiruiSource(source) || costs is not [var cost])
            return null;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Sirui is { } r && r.Source == source && r.CardId == cost.CardId && r.From == cost.From && r.NativeKind != SiruiNativeUseKind.CardUse && ValidSiruiInvoice(f, r, requirePayment: true));
        return parent is null ? null : _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cost.CardId)).Single(c => c.Id == cost.CardId).Rank;
    }

    internal bool IsSiruiOutsideResponseMaterial(long parentId, int cardId) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == parentId)is { Sirui: { NativeKind: SiruiNativeUseKind.Nullification, PrintedKind: CardKind.GeneralWeapon, PaymentSequence: > 0 } r } parent && r.CardId == cardId && r.From.Zone == CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiMaterialPaidEvent>().Any(e => e.OwnerFrameId == parentId && e.CardId == cardId && e.From == r.From && e.To == CardLocation.OutsideGame && e.MovementSequence == r.PaymentSequence && e.Reason == CardMoveReasons.Nullification) && ValidSiruiInvoice(parent, r, requirePayment: true);
    internal bool TryGetSiruiDestroyedResponseCost(CardActionContext action, int cardId, out CardMovementRecord movement)
    {
        movement = null !;
        if (action is not { Type: CardActionType.Response, EffectiveKind: CardKind.Nullification } || action.ActorSeat != action.ProviderSeat || action.RequesterSeat is not null || action.PhysicalCards is not [var cost] || cost.CardId != cardId)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Sirui is { } r && r.ActionId == action.ActionId && r.Source.OwnerSeat == action.ActorSeat && r.CardId == cardId && action.ConversionChain.Contains(r.Source) && cost.From == r.From && cost.CardKind == r.PrintedKind && IsSiruiOutsideResponseMaterial(f.Id, cardId));
        if (parent?.Sirui is not { } receipt)
            return false;
        movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.Sequence == receipt.PaymentSequence);
        return true;
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 28212)]
    internal void AssertSiruiUses()
    {
        foreach (var frame in _resolutionStackOperations._resolutionStack.Where(f => f.Sirui is not null))
        {
            var r = frame.Sirui!;
            if (!ValidSiruiInvoice(frame, r, requirePayment: false) || r.ActionId != 0 && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<SiruiNativeActionBoundEvent>().Count(e => e.OwnerFrameId == frame.Id && e.ActionId == r.ActionId && e.ActorSeat == r.Source.OwnerSeat && e.ProviderSeat == r.Source.OwnerSeat && e.OutputKind == r.OutputKind && e.ActionType == (r.NativeKind == SiruiNativeUseKind.CardUse ? CardActionType.Use : CardActionType.Response)) != 1)
                throw new InvalidOperationException("Sirui lost its exact original native owner, phase, source, material or action invoice.");
        }
    }

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 10)]
    internal bool DistancePhysicalSlash10HasIssuedSiruiUse(ProgramSlashDistanceQuery q) => q.ExistingUseFrameId is { } siruiUseId && HasIssuedSiruiUse(siruiUseId, q.Actor.Seat);
}
