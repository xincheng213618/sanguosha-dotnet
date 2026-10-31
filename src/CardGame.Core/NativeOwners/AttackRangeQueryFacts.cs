#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct AttackRangeQueryFacts(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedLifecycleEntries> getCoreGameEngineAdvancedLifecycleRuntime, global::System.Func<global::CardGame.Core.ProgramDistanceRules> getProgramDistanceRules, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXiahouBaMobileShiEntries> getCharacterXiahouBaMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangRenMobileShiEntries> getCharacterZhangRenMobileShiRuntime, global::System.Func<global::CardGame.Core.TurnCardUseEffectStore> getTurnCardUseEffectStore, CharacterState player) : IMatchAttackRangeQuery
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
