#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct DrawCountQueryFacts(EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CoreGameEngineShanheMarketEntries> getCoreGameEngineShanheMarketRuntime, Func<RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime, Func<RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime, Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime, Func<RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, Func<RuntimeEntries.CharacterLiYanMobileEntries> getCharacterLiYanMobileRuntime, Func<RuntimeEntries.CoreProgramShanheLuanxinEntries> getCoreProgramShanheLuanxinRuntime, Func<RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, Func<RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime, Func<RuntimeEntries.CharacterHuCheErAnniversaryEntries> getCharacterHuCheErAnniversaryRuntime, Func<RuntimeEntries.CharacterGaoGanOlEntries> getCharacterGaoGanOlRuntime, Func<RuntimeEntries.CharacterDuXiOlEntries> getCharacterDuXiOlRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.CoreGameEngineModeRulesEntries> getCoreGameEngineModeRulesRuntime, CharacterState player) : IMatchDrawCountQuery
{
    public string ModeId => matchDefinitionState.ReferenceToModeDefinition.Id;
    public int Seat => player.Seat;
    public int? AssignedDefault => getCoreGameEngineTurnDefaultStatsRuntime().GetCurrentTurnDefaultStat(player.Seat, SkillRuleQuery.DrawCount);
    public IEnumerable<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.DrawCount);
    public int MarketBonus => getCoreGameEngineShanheMarketRuntime().ShanheMarketRuleBonus(player.Seat, SkillRuleQuery.DrawCount);
    public int SharedRankBonus => getCoreGameEngineShanheSchoolTeamRuntime().ShanheSharedRankRuleBonus(player.Seat, SkillRuleQuery.DrawCount);
    public int GangshuBonus => getCharacterCaoXiOlRuntime().CaoXiGangshuBonus(player, SkillRuleQuery.DrawCount);
    public int ShunshiBonus => getCharacterDuFuRenOlRuntime().DuFuRenPhaseBonus(player.Seat, SkillRuleQuery.DrawCount);
    public int HengyeGrowth => getCharacterLuMengMouRuntime().GetHengyeGrowth(player);
    public int AdditiveTurnBonus => getCoreGameEngineCardUseModulesRuntime().GetAdditiveTurnRuleModifier(player.Seat, SkillRuleQuery.DrawCount);
    public bool TracksFengjiRoundLedger => getCharacterChenDengOlRuntime().TracksFengjiRoundLedger;
    public int FengjiRoundAdjustment => getCharacterChenDengOlRuntime().GetFengjiRoundAdjustment(player.Seat, SkillRuleQuery.DrawCount);
    public int DefaultDrawCount => matchDefinitionState.ReferenceToDrawPerTurn;
    public IReadOnlyList<Card> EffectiveEquipment => getCharacterHuCheErMouRuntime().KongwuEffectiveEquipment(player);
    public int DuliangNextDrawBonus => getCharacterLiYanMobileRuntime().MobileLiYanNextDrawBonus(player);
    public int LuanxinNextDrawBonus => getCoreProgramShanheLuanxinRuntime().ShanheLuanxinNextDrawBonus(player);
    public int AlternatingNextDrawBonus => getCoreGameEngineAlternatingBenefitsRuntime().AlternatingNextDrawPhaseBonus(player);
    public int KuangxiangDrawPenalty => getCharacterChengGongYingOlRuntime().ChengGongYingDrawPenalty(player.Seat);
    public int KongwuDrawPenalty => getCharacterHuCheErMouRuntime().MouHuCheErDrawPenalty(player);
    public int FuzhongDrawBonus => getCharacterHuCheErAnniversaryRuntime().AnniversaryHuCheErDrawBonus(player);
    public int JuguanDrawAdjustment => getCharacterGaoGanOlRuntime().JuguanActualDrawAdjustment(player);
    public int QuxiDrawAdjustment => getCharacterDuXiOlRuntime().DuXiDrawAdjustment(player);

    public EquipmentDefinition EquipmentDefinition(CardKind kind) => getEquipmentContentRegistry().Get(kind);
    public RuleQueryEvaluation Project(RuleQueryEvaluation evaluation) => getCoreGameEngineModeRulesRuntime().ApplyModeNumericRules(player, evaluation, SkillRuleQuery.DrawCount);
}
