#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct DrawCountQueryFacts(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMarketEntries> getCoreGameEngineShanheMarketRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries> getCharacterHuCheErMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiYanMobileEntries> getCharacterLiYanMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreProgramShanheLuanxinEntries> getCoreProgramShanheLuanxinRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries> getCoreGameEngineAlternatingBenefitsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChengGongYingOlEntries> getCharacterChengGongYingOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries> getCharacterHuCheErAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGaoGanOlEntries> getCharacterGaoGanOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuXiOlEntries> getCharacterDuXiOlRuntime, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries> getCoreGameEngineModeRulesRuntime, CharacterState player) : IMatchDrawCountQuery
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
