#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct AttackRangeQueryFacts(EngineStates.CoreMatchDefinitionState matchDefinitionState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, Func<RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, Func<ProgramDistanceRules> getProgramDistanceRules, Func<RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CharacterXiahouBaMobileShiEntries> getCharacterXiahouBaMobileShiRuntime, Func<RuntimeEntries.CharacterZhangRenMobileShiEntries> getCharacterZhangRenMobileShiRuntime, Func<TurnCardUseEffectStore> getTurnCardUseEffectStore, CharacterState player) : IMatchAttackRangeQuery
{
    public string ModeId => matchDefinitionState.ReferenceToModeDefinition.Id;
    public int Seat => player.Seat;
    public int TurnNumber => actualTurnProgressState.ReferenceToTurnNumber;
    public IReadOnlyList<Card> EffectiveEquipment => getCharacterHuCheErMouRuntime().KongwuEffectiveEquipment(player);

    public EquipmentDefinition EquipmentDefinition(CardKind kind) => getEquipmentContentRegistry().Get(kind);
    public int WeaponAttackRange(Card weapon) => getCoreGameEngineAdvancedLifecycleRuntime().GetWeaponAttackRange(player, weapon);
    public void AddBaseTerms(int? excludedEquipmentId, EquipmentSlot? excludedEquipmentSlot, List<RuleQueryBaseTerm> terms) => getProgramDistanceRules().AddAttackRangeBaseTerms(new(player, excludedEquipmentId, excludedEquipmentSlot), terms);
    public int? AssignedRange => getCoreGameEngineTurnDefaultStatsRuntime().GetCurrentTurnDefaultStat(player.Seat, SkillRuleQuery.AttackRange);
    public IEnumerable<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.AttackRange);
    public int LibbingRangeBonus => getCharacterXiahouBaMobileShiRuntime().ShiXbLibbingRangeBonus(player.Seat);
    public int YinXianRangeBonus => getCharacterZhangRenMobileShiRuntime().ShiZrYinXianRangeBonus(player);
    public bool FuanBaseZeroed => getCharacterZhangRenMobileShiRuntime().ShiZrFuanBaseZeroed(player);

    public void AddContributions(List<RuleQueryContribution> contributions) => getProgramDistanceRules().AddAttackRangeContributions(player, contributions);
    public IReadOnlyList<TurnRuleModifier> TurnModifiers => getTurnCardUseEffectStore().GetRuleModifiers(actualTurnProgressState.ReferenceToTurnNumber, actualTurnProgressState.ReferenceToCurrentSeat, player.Seat, SkillRuleQuery.AttackRange);

    public RuleQueryEvaluation Project(RuleQueryEvaluation evaluation) => getProgramDistanceRules().ProjectAttackRange(player, evaluation);
}
