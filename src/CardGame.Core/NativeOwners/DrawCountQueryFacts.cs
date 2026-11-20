#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct DrawCountQueryFacts(EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CoreGameEngineShanheMarketEntries> getCoreGameEngineShanheMarketRuntime, Func<RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CoreGameEngineFieldEquipmentEntries> getFieldEquipment, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreProgramShanheLuanxinRuntime>> getCoreProgramShanheLuanxinRuntime, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineModeRulesRuntime>> getCoreGameEngineModeRulesRuntime, CharacterState player) : IMatchDrawCountQuery
{
    public string ModeId => matchDefinitionState.ReferenceToModeDefinition.Id;
    public int Seat => player.Seat;
    public int? AssignedDefault => getCoreGameEngineTurnDefaultStatsRuntime().GetCurrentTurnDefaultStat(player.Seat, SkillRuleQuery.DrawCount);
    public IEnumerable<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.DrawCount);
    public int MarketBonus => getCoreGameEngineShanheMarketRuntime().ShanheMarketRuleBonus(player.Seat, SkillRuleQuery.DrawCount);
    public int SharedRankBonus => getCoreGameEngineShanheSchoolTeamRuntime().ShanheSharedRankRuleBonus(player.Seat, SkillRuleQuery.DrawCount);
    public int HengyeGrowth => getCharacterLuMengMouRuntime().GetHengyeGrowth(player);
    public int AdditiveTurnBonus => getCoreGameEngineCardUseModulesRuntime().GetAdditiveTurnRuleModifier(player.Seat, SkillRuleQuery.DrawCount);
    public int DefaultDrawCount => matchDefinitionState.ReferenceToDrawPerTurn;
    public IReadOnlyList<Card> EffectiveEquipment => getFieldEquipment().GetEquipmentWithPrintedEffects(player);
    public int LuanxinNextDrawBonus => getCoreProgramShanheLuanxinRuntime().Value.ShanheLuanxinNextDrawBonus(player);
    public int AlternatingNextDrawBonus => getCoreGameEngineAlternatingBenefitsRuntime().AlternatingNextDrawPhaseBonus(player);

    public EquipmentDefinition EquipmentDefinition(CardKind kind) => getEquipmentContentRegistry().Get(kind);
    public RuleQueryEvaluation Project(RuleQueryEvaluation evaluation) => getCoreGameEngineModeRulesRuntime().Value.ApplyModeNumericRules(player, evaluation, SkillRuleQuery.DrawCount);
}
