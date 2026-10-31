#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNoDamageSkillDisableRuntime
{
    internal void IssueNoDamageSkillDisablePolicy(long frameId)
    {
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(frameId)!;
        if (use.Action is not { Type: CardActionType.Use } action)
            return;
        var eligible = action.ConversionChain.Where(source => _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program?.ViewAs.SingleOrDefault(rule => rule.Id == source.BindingId)?.DisableSkillUntilTurnEndIfNoDamage == true).ToArray();
        if (eligible.Length == 0)
            return;
        if (eligible is not [var source] || source.OwnerSeat != action.ActorSeat || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[source.OwnerSeat], source.SkillId, source.SkillInstanceId) || use.CardKind != CardKind.UnexpectedAssault || action.PhysicalCards is not [{ From.Zone: CardZoneKind.Hand }])
            throw new InvalidOperationException("A no-damage disable policy requires its exact accepted hand conversion.");
        var policy = new NoDamageSkillDisablePolicy(use.Id, action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { NoDamageSkillDisablePolicy = policy });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NoDamageSkillDisablePolicyIssuedEvent(policy));
    }

    internal void SettleNoDamageSkillDisable(CardUseFrame use)
    {
        if (use.NoDamageSkillDisablePolicy is not { } policy)
            return;
        AssertNoDamageSkillDisablePolicy(use);
        if (use.CausedDamage)
            return;
        if (policy.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || policy.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("A no-damage conversion cannot settle in another actual turn.");
        var source = policy.Source;
        var issued = new CurrentTurnOwnSkillSuppression(_coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, use.Id, 0, new(source.SkillId, source.BindingId, source.OwnerSeat, source.SkillInstanceId), source.SkillId, use.Id, policy.ActionId);
        var existing = _coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.SingleOrDefault(s => s.ParentFrameId == use.Id && s.EffectIndex == 0);
        if (existing is not null)
        {
            if (existing != issued)
                throw new InvalidOperationException("A completed conversion disable changed identity.");
            return;
        }

        _coreGameEngineAlcoholSlashSuppressionsCurrentTurnOwnSkillSuppressionsAccess._currentTurnOwnSkillSuppressions.Add(issued);
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CurrentTurnOwnSkillSuppressionIssuedEvent(issued));
    }

    internal void AssertNoDamageSkillDisablePolicy(CardUseFrame use)
    {
        if (use.NoDamageSkillDisablePolicy is not { } policy)
            return;
        if (use.CardKind != CardKind.UnexpectedAssault || use.Action is not { Type: CardActionType.Use } action || policy.UseFrameId != use.Id || policy.ActionId != action.ActionId || !action.ConversionChain.Contains(policy.Source) || policy.GameplayHash != _contentRegistryOperations._contentRegistry.GetSkill(policy.Source.SkillId).Program?.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(policy.Source.SkillId).Program?.ViewAs.SingleOrDefault(r => r.Id == policy.Source.BindingId)?.DisableSkillUntilTurnEndIfNoDamage != true || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NoDamageSkillDisablePolicyIssuedEvent>().Count(e => e.Policy == policy) != 1)
            throw new InvalidOperationException("A no-damage disable policy lost its original accepted use.");
    }
}
