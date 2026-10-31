#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoYunOlGod2016Runtime
{
    internal static int HpScaledCardCount(CharacterState owner) => owner.HasHpValue ? Math.Max(1, owner.Hp) : int.MaxValue;
    internal static int EffectiveProgramViewAsInputCount(CharacterState owner, SkillProgramViewAs rule) => rule.InputCountExpression switch
    {
        null => rule.InputCount,
        SkillProgramCardCountExpression.OwnerHpAtLeastOne => HpScaledCardCount(owner),
        _ => throw new InvalidOperationException("Unsupported native conversion card-count expression.")};
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 40650)]
    internal bool HpScaledViewAsCountForbidden(ProgramViewAsLegalityQuery q) => _corePlayCardHistoryOperations.ViewAsRule(q.Source)is { InputCountExpression: not null } r && (q.Source.OwnerSeat != q.Owner.Seat || q.CardIds.Count != EffectiveProgramViewAsInputCount(q.Owner, r));
    internal void IssueHpScaledProgramSelection(ProgramSkillFrame f, SkillProgramActivation a)
    {
        if (a.CardCountExpression != SkillProgramCardCountExpression.OwnerHpAtLeastOne)
            return;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (f.SelectedCardIds.Count != HpScaledCardCount(owner))
            throw new InvalidOperationException("A HP-scaled conversion must revalidate its live material count before creating its original program frame.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HpScaledProgramSelectionEvent(f.Id, new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, owner.Hp, f.SelectedCardIds.Count));
    }
    internal CardActionContext CaptureHpScaledCardUseAction(CardActionContext action)
    {
        foreach (var source in action.ConversionChain)
        {
            if (_corePlayCardHistoryOperations.ViewAsRule(source)is not { InputCountExpression: not null } r)
                continue;
            var owner = _coreMatchParticipantsPlayersAccess._players[action.ProviderSeat];
            if (source.OwnerSeat != owner.Seat || action.PhysicalCards.Count != EffectiveProgramViewAsInputCount(owner, r))
                throw new InvalidOperationException("A HP-scaled native use must pay its current HP-defined material count.");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HpScaledCardUsePaidEvent(action.ActionId, source, _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash, owner.Hp, action.PhysicalCards.Count));
        }

        return action;
    }

    internal bool HasPaidHpScaledViewAsAction(CardActionContext action) => action.ConversionChain is [var source] && _corePlayCardHistoryOperations.ViewAsRule(source)is { InputCountExpression: SkillProgramCardCountExpression.OwnerHpAtLeastOne } && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HpScaledCardUsePaidEvent>().Where(e => e.ActionId == action.ActionId).ToArray()is [var e] && e.Source == source && e.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(source.SkillId).Program!.GameplayHash && e.MaterialCount == action.PhysicalCards.Count && e.MaterialCount == Math.Max(1, e.OwnerHp) && source.OwnerSeat == action.ProviderSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardActionAcceptedEvent>().Any(accepted => accepted.Action.ActionId == action.ActionId && accepted.Action.ActorSeat == action.ActorSeat && accepted.Action.ProviderSeat == action.ProviderSeat && accepted.Action.EffectiveKind == action.EffectiveKind && accepted.Action.ConversionChain.SequenceEqual(action.ConversionChain) && accepted.Action.PhysicalCards.SequenceEqual(action.PhysicalCards));
}
