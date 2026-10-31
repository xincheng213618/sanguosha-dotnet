#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNonBasicSlashProfileRuntime
{
    internal void IssueNonBasicSlashPolicy(long useId, CardActionContext? action)
    {
        if (action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Slash, PhysicalCards: [var cost] } || action.ConversionChain.SingleOrDefault(s => _corePlayCardHistoryOperations.ViewAsRule(s)?.NonBasicUnrespondableSlash == true)is not { } source)
            return;
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(useId) ?? throw new InvalidOperationException("A non-basic Slash lost its real native Use.");
        if (source.OwnerSeat != action.ActorSeat || action.ProviderSeat != action.ActorSeat || cost.From.OwnerSeat != source.OwnerSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || CardUseCategoryCatalog.Get(cost.CardKind) == CardUseCategories.Basic || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) || _cardZonesOperations._cardZones.GetLocation(cost.CardId) != cost.From || use.NonBasicSlashPolicy is not null)
            throw new InvalidOperationException("A non-basic Slash policy requires its exact active conversion and one actual owned material.");
        var issued = new IssuedCardNoResponse(action.ActionId, useId, source, _coreActualTurnProgressTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        var policy = new ConvertedNonBasicSlashPolicy(useId, action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, cost);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { NonBasicSlashPolicy = policy });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ConvertedNonBasicSlashIssuedEvent(useId, action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, cost.CardId, cost.CardKind, cost.From));
        if (use.IssuedNoResponse is null)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IssuedCardNoResponseEvent(issued));
    }

    internal bool HasIssuedNonBasicSlashDistance(long? useId)
    {
        if (useId is not { } id || _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { Action: { } current, NonBasicSlashPolicy: { } policy } use)
            return false;
        if (current.Type != CardActionType.Use || use.CardKind != current.EffectiveKind)
            return false;
        var action = current;
        if (use.CurrentSlashFirePolicy is { Converted: true } fire)
        {
            if (!_coreCurrentSlashFireOffersOperations.IsCurrentSlashFireChangedUse(use))
                return false;
            _coreCurrentSlashFireOffersOperations.AssertCurrentSlashFirePolicy(use);
            action = fire.OriginalAction;
        }

        return policy.CardUseFrameId == id && action.ActionId == policy.ActionId && action.EffectiveKind == CardKind.Slash && action.PhysicalCards is [var cost] && cost == policy.Material && cost.From.OwnerSeat == policy.Source.OwnerSeat && cost.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && CardUseCategoryCatalog.Get(cost.CardKind) != CardUseCategories.Basic && action.ConversionChain.Contains(policy.Source) && _corePlayCardHistoryOperations.ViewAsRule(policy.Source)?.NonBasicUnrespondableSlash == true && _contentRegistryOperations._contentRegistry.GetSkill(policy.Source.SkillId).Program?.GameplayHash == policy.GameplayHash && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ConvertedNonBasicSlashIssuedEvent>().Count(e => e.CardUseFrameId == id && e.ActionId == policy.ActionId && e.Source == policy.Source && e.GameplayHash == policy.GameplayHash && e.MaterialId == cost.CardId && e.MaterialKind == cost.CardKind && e.From == cost.From) == 1;
    }

    internal bool IgnoresNonBasicSlashDistance(CharacterState owner, CardConversionSource source) => source.OwnerSeat == owner.Seat && _coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, source.SkillId, source.SkillInstanceId) && _corePlayCardHistoryOperations.ViewAsRule(source)?.NonBasicUnrespondableSlash == true;
    [ProgramDistanceRule(ProgramDistanceStage.ConvertedSlashExemption, 20)]
    internal bool IgnoresNonBasicSlashDistanceQuery(ProgramSlashConversionDistanceQuery q) => IgnoresNonBasicSlashDistance(q.Owner, q.Source);
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 80)]
    internal bool DistancePhysicalSlash80HasIssuedNonBasicSlashDistance(ProgramSlashDistanceQuery q) => HasIssuedNonBasicSlashDistance(q.ExistingUseFrameId);
}
