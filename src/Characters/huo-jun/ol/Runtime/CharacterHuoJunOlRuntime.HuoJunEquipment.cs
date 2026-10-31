#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuoJunOlRuntime
{
    internal void IssueHuoJunRandomEquipment(ProgramSkillFrame f)
    {
        var r = f.HuoJunFenrui!;
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (r.RestoredSlot is not { } slot || r.Stage != HuoJunFenruiStage.RestoreChildren)
            throw new InvalidOperationException("Fenrui equipment must follow its one actually restored native slot.");
        var candidates = actor.IsAlive && actor.Hp > 0 ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && !c.IsGeneralWeapon && _coreEquipmentContentOperations.EquipmentRules.Get(c.Kind).Slot == slot && actor.EquipmentSlotCapacity(slot) > 0 && !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, c.Kind, CardActionType.Use) && !_coreHandComparisonOperations.IsSelfTargetForbiddenAction(actor, c.Kind, [actor.Seat]) && !_coreResponseEntityExchangeOperations.IsResponseEntityRestricted(actor.Seat, c.Id) && !_coreProgramPhaseColorRestrictionsOperations.IsPlayPhasePhysicalCardRestricted(actor, c)).OrderBy(c => c.Id).ToArray() : [];
        if (candidates.Length == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuoJunFenrui = r with { Stage = HuoJunFenruiStage.EquipmentChildren } });
            ContinueHuoJunFenrui(f.Id);
            return;
        }

        var card = candidates[_randomOperations._random.Next(candidates.Length)];
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HuoJunFenrui = r with { Stage = HuoJunFenruiStage.NativeEquipment, EquipmentCardId = card.Id, EquipmentKind = card.Kind } });
        var useId = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, [], conversionSource: r.Source);
        var actionId = _coreCardUseLifecycleOperations.LifecycleCardUse(useId)?.Action?.ActionId ?? throw new InvalidOperationException("Fenrui random equipment requires an actual native accepted use action.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { HuoJunFenrui = f.HuoJunFenrui!with { NativeUseId = useId, NativeActionId = actionId } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoJunFenruiEquipmentIssuedEvent(f.Id, r.Source, slot, card.Id, card.Kind, useId, actionId));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Processing, CardMoveReasons.EquipmentUse);
        if (!_coreFinalizedCardTargetsOperations.TryBeginEquipmentTargetPrograms(actor, card, useId))
            _coreNativeEquipmentUseOperations.CompleteEquipmentUse(actor, card, useId);
    }

    internal bool IsHuoJunFenruiNativeEquipment(ResolutionFrame ride, ResolutionFrame beneath)
    {
        if (ride is not CardUseFrame use || beneath is not ProgramSkillFrame { HuoJunFenrui: { Stage: HuoJunFenruiStage.NativeEquipment } r } f || r.Source != HuoJunSource(f) || r.NativeUseId != use.Id || r.NativeActionId != use.Action?.ActionId || r.EquipmentCardId != use.CardId || r.EquipmentKind != use.CardKind || r.RestoredSlot is not { } slot || _coreEquipmentContentOperations.EquipmentRules.Get(use.CardKind).Slot != slot || use.Action is not { Type: CardActionType.Use } action || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.EffectiveKind != use.CardKind || action.PhysicalCards is not [var cost] || cost.CardId != use.CardId || cost.CardKind != use.CardKind || cost.From != CardLocation.DrawPile || !action.ConversionChain.Contains(r.Source))
            return false;
        return _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuoJunFenruiEquipmentIssuedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Slot == slot && e.CardId == use.CardId && e.CardKind == use.CardKind && e.NativeUseId == use.Id && e.NativeActionId == action.ActionId) == 1;
    }

    [NativeUseValidation(26210)]
    internal void ValidateHuoJunEquipmentReturn(CardUseFrame use)
    {
        var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.HuoJunFenrui?.NativeUseId == use.Id);
        if (parent is null)
            return;
        if (use.Step != ResolutionFrameStep.Completed || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != use.Id || !IsHuoJunFenruiNativeEquipment(use, parent))
            throw new InvalidOperationException("Fenrui must finish the exact paid physical random equipment through its original native use.");
    }

    [NativeUseReturn(26210)]
    internal bool ReturnHuoJunEquipment(CardUseFrame use)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HuoJunFenrui: { Stage: HuoJunFenruiStage.NativeEquipment } r } f || r.NativeUseId != use.Id)
            return false;
        if (_resolutionStackOperations._resolutionStack.Any(frame => frame.Id == use.Id) || !IsHuoJunFenruiNativeEquipment(use, f) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == r.EquipmentCardId) != 1 || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HuoJunFenruiEquipmentReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Fenrui random equipment may return only after the one actual native use finishes.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HuoJunFenruiEquipmentReturnedEvent(f.Id, use.Id, r.NativeActionId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HuoJunFenrui = r with { Stage = HuoJunFenruiStage.EquipmentChildren } });
        ContinueHuoJunFenrui(f.Id);
        return true;
    }
}
