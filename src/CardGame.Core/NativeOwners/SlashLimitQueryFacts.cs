#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct SlashLimitQueryFacts(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheMarketEntries> getCoreGameEngineShanheMarketRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanMobileShiEntries> getCharacterWeiYanMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangChongMobileShiEntries> getCharacterHuangChongMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiMobileShiEntries> getCharacterZhongHuiMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSequentialDeckKindGrowthEntries> getCoreGameEngineSequentialDeckKindGrowthRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuangGaiMouMobileEntries> getCharacterHuangGaiMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYangFengOlEntries> getCharacterYangFengOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanYanBoundaryMobileEntries> getCharacterYanYanBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuaXiongMouMobileEntries> getCharacterHuaXiongMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouMobileEntries> getCharacterLuMengMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries> getCharacterYuanTanYuanShangOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedPoliciesEntries> getCoreGameEngineAdvancedPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangTongFriendEntries> getCharacterPangTongFriendRuntime, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries> getCoreGameEngineAdvancedWeaponsRuntime, global::System.Func<global::CardGame.Core.MatchModeRuntime> getMatchModeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries> getCoreGameEngineModeRulesRuntime, CharacterState player) : IMatchSlashLimitQuery
{
    public string ModeId => matchDefinitionState.ReferenceToModeDefinition.Id;
    public int Seat => player.Seat;
    public int? AssignedDefault => getCoreGameEngineTurnDefaultStatsRuntime().GetCurrentTurnDefaultStat(player.Seat, SkillRuleQuery.SlashLimit);
    public IEnumerable<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.SlashLimit);
    public int MarketBonus => getCoreGameEngineShanheMarketRuntime().ShanheMarketRuleBonus(player.Seat, SkillRuleQuery.SlashLimit);
    public int SharedRankBonus => getCoreGameEngineShanheSchoolTeamRuntime().ShanheSharedRankRuleBonus(player.Seat, SkillRuleQuery.SlashLimit);
    public int GangshuBonus => getCharacterCaoXiOlRuntime().CaoXiGangshuBonus(player, SkillRuleQuery.SlashLimit);
    public int ShunshiBonus => getCharacterDuFuRenOlRuntime().DuFuRenPhaseBonus(player.Seat, SkillRuleQuery.SlashLimit);
    public int HengyeGrowth => getCharacterLuMengMouRuntime().GetHengyeGrowth(player);
    public int AdditiveTurnBonus => getCoreGameEngineCardUseModulesRuntime().GetAdditiveTurnRuleModifier(player.Seat, SkillRuleQuery.SlashLimit);
    public bool TracksFengjiRoundLedger => getCharacterChenDengOlRuntime().TracksFengjiRoundLedger;
    public int FengjiRoundAdjustment => getCharacterChenDengOlRuntime().GetFengjiRoundAdjustment(player.Seat, SkillRuleQuery.SlashLimit);
    public int KuangguSlashBonus => getCharacterWeiYanMobileShiRuntime().MobileShiWeiYanPhaseSlashBonus(player.Seat);
    public int CujunSlashBonus => getCharacterHuangChongMobileShiRuntime().ShiHcCujunSlashBonus(player.Seat);
    public int XiezhiOverflowCount => getCharacterZhongHuiMobileShiRuntime().ShiZhHuiOverflowCount(player.Seat);
    public bool HasZhuangshiQuota => getCharacterWeiYanMobileShiRuntime().HasMobileShiZhuangshiQuota(player.Seat);
    public int SequentialDeckShortfallBonus => getCoreGameEngineSequentialDeckKindGrowthRuntime().SequentialDeckKindGrowthBonus(player.Seat);
    public bool HasZhajiangPreUsePrivilege => getCharacterHuangGaiMouMobileRuntime().HasMouZhaJiangPreUsePrivilege(player);
    public int QujiaSlashBonus => getCharacterYangFengOlRuntime().QujiaPermanentSlashBonus(player.Seat);
    public int JuzhanSlashBonus => getCharacterYanYanBoundaryMobileRuntime().BoundaryYanYanSlashBonus(player.Seat);
    public bool HasYangweiBonus => getCharacterHuaXiongMouMobileRuntime().HasMouHuaXiongWei(player.Seat);
    public int DuojingSlashBonus => getCharacterLuMengMouMobileRuntime().MouDuojingPhaseSlashBonus(player.Seat);
    public int NeifaSlashBonus => getCharacterYuanTanYuanShangOlRuntime().NeifaSlashLimitBonus(player.Seat);
    public bool HasNextUnlimitedCard => getCoreGameEngineAdvancedPoliciesRuntime().HasNextUnlimitedCard(player);
    public bool HasFriendQihuiAllowance => getCoreGameEngineAdvancedPoliciesRuntime().HasFriendQihuiAllowance(player);
    public bool HasMouWujingAllowance => getCharacterZhugeLiangMouRuntime().HasMouWujingAllowance(player);
    public bool HasFriendYangmingQuota => getCharacterPangTongFriendRuntime().HasFriendYangmingQuota(player);
    public bool HasUnlimitedTurnBonus => getCoreGameEngineCardUseModulesRuntime().HasUnlimitedTurnRuleModifier(player.Seat, SkillRuleQuery.SlashLimit);

    public EquipmentDefinition EquipmentDefinition(CardKind kind) => getEquipmentContentRegistry().Get(kind);
    public IReadOnlyList<Card> EquipmentCards(EquipmentSlot? excludedEquipmentSlot) => getCoreGameEngineAdvancedWeaponsRuntime().GetEquipmentRuleCards(player, excludedEquipmentSlot);
    public int EquipmentSlashLimitBonus(CardKind kind, int printedBonus) => getMatchModeRuntime().EquipmentSlashLimitBonus(kind, printedBonus);
    public RuleQueryEvaluation Project(RuleQueryEvaluation evaluation) => getCoreGameEngineModeRulesRuntime().ApplyModeNumericRules(player, evaluation, SkillRuleQuery.SlashLimit);
}
