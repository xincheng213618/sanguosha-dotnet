#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTriggeredCardConversionsRuntime
{
    internal bool CanUseTriggeredSlashMaterial(int ownerSeat, int targetSeat, Card card, CardLocation location, CardConversionSource source) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && ownerSeat != targetSeat && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && location.OwnerSeat == ownerSeat && location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _cardZonesOperations._cardZones.GetLocation(card.Id) == location && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(ownerSeat, [card.Id]) && (location.Zone != CardZoneKind.Hand || !_coreCardUseModulesOperations.IsTurnHandCardRestricted(_coreMatchParticipantsPlayersAccess._players[ownerSeat], card)) && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(ownerSeat, SkillProgramCardTargetRestriction.SelfOnly) && _coreNativeTargetLegalityOperations.CanUseSlashTarget(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], card, source, CardKind.Slash, physicalCardIds: [card.Id], excludedEquipmentId: location.Zone == CardZoneKind.Equipment ? card.Id : null) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], false, CardKind.Slash, card, excludedEquipmentId: location.Zone == CardZoneKind.Equipment ? card.Id : null, physicalCardIds: [card.Id]);
    internal bool MatchesTriggeredSlashMaterial(ProgramSkillFrame frame, string bind, int ownerSeat, Card card, CardLocation location)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        if (!plan.Instructions.Any(effect => effect.Op == SkillProgramEffectOp.UseBoundCardsAsSlash && effect.SourceBind == bind))
            return true;
        var selection = plan.Instructions[0];
        return ownerSeat == frame.OwnerSeat && frame.TriggerId is not null && frame.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding, TargetSeat: { } target } && TriggeredSlashSelectionAccepts(_coreMatchParticipantsPlayersAccess._players[ownerSeat], selection, card, location) && CanUseTriggeredSlashMaterial(ownerSeat, target, card, location, new(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), ownerSeat, frame.SkillInstanceId));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AfterInitialPayments, 50, SkillProgramEffectOp.UseBoundCardsAsSlash)]
    internal bool CanOfferTriggeredSlashConversion(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var consumer = trigger.Effects.FirstOrDefault(effect => effect.Op == SkillProgramEffectOp.UseBoundCardsAsSlash);
        if (consumer is null)
            return true;
        if (context is not { Window: SkillProgramTriggerWindow.TurnEnding, TargetSeat: { } target })
            return false;
        var selection = trigger.Effects.Single(effect => effect.Op == SkillProgramEffectOp.SelectOwnedCards && effect.ResultBind == consumer.SourceBind);
        var owner = _coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat];
        var source = new CardConversionSource(candidate.SkillId, candidate.BindingId, owner.Seat, candidate.SkillInstanceId);
        return selection.Zones.Any(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, owner.Seat)).Any(card => TriggeredSlashSelectionAccepts(owner, selection, card, new(zone, owner.Seat)) && CanUseTriggeredSlashMaterial(owner.Seat, target, card, new(zone, owner.Seat), source)));
    }

    internal bool TriggeredSlashSelectionAccepts(CharacterState owner, SkillProgramEffect selection, Card card, CardLocation location) => selection.Zones.Contains(location.Zone) && (selection.Suits.Count == 0 || selection.Suits.Contains(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card))) && (selection.CardKinds.Count == 0 || ProgramCardKindMatch.Contains(selection.CardKinds, card.Kind));
    internal SkillProgramStepOutcome UseTriggeredBoundCardsAsSlash(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.TriggerId is null || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding, TargetSeat: { } target } || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id)
            throw new InvalidOperationException("A triggered physical Slash requires its owning ending-phase program.");
        var binding = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
        if (binding.CardIds is not [var cardId] || binding.SourceLocations is not [var location])
            throw new InvalidOperationException("A triggered physical Slash requires exactly one owned bound entity.");
        var card = _coreCardAttackStateOperations.GetAttackCard(cardId);
        if (!_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame) || !MatchesTriggeredSlashMaterial(frame, effect.SourceBind!, frame.OwnerSeat, card, location))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "转化的实体牌、技能或目标已失效，未支付实体牌。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var source = new CardConversionSource(frame.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[target], card, CardKind.Slash, frame.OwnerSeat, physicalCards: [card], countsTowardSlashLimit: false, conversionSource: source, programSkillCardUseFrameId: frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    SkillProgramStepOutcome ITriggeredCardConversionHost.UseBoundCardsAsSlash(ProgramSkillFrame frame, SkillProgramEffect effect) => UseTriggeredBoundCardsAsSlash(frame, effect);
}
