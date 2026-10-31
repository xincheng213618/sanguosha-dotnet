#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct SlashLimitQueryFacts(EngineStates.CoreMatchDefinitionState matchDefinitionState, Func<RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CoreGameEngineShanheMarketEntries> getCoreGameEngineShanheMarketRuntime, Func<RuntimeEntries.CoreGameEngineShanheSchoolTeamEntries> getCoreGameEngineShanheSchoolTeamRuntime, Func<RuntimeEntries.CharacterCaoXiOlEntries> getCharacterCaoXiOlRuntime, Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CharacterChenDengOlEntries> getCharacterChenDengOlRuntime, Func<RuntimeEntries.CharacterWeiYanMobileShiEntries> getCharacterWeiYanMobileShiRuntime, Func<RuntimeEntries.CharacterHuangChongMobileShiEntries> getCharacterHuangChongMobileShiRuntime, Func<RuntimeEntries.CharacterZhongHuiMobileShiEntries> getCharacterZhongHuiMobileShiRuntime, Func<RuntimeEntries.CoreGameEngineSequentialDeckKindGrowthEntries> getCoreGameEngineSequentialDeckKindGrowthRuntime, Func<RuntimeEntries.CharacterHuangGaiMouMobileEntries> getCharacterHuangGaiMouMobileRuntime, Func<RuntimeEntries.CharacterYangFengOlEntries> getCharacterYangFengOlRuntime, Func<RuntimeEntries.CharacterYanYanBoundaryMobileEntries> getCharacterYanYanBoundaryMobileRuntime, Func<RuntimeEntries.CharacterHuaXiongMouMobileEntries> getCharacterHuaXiongMouMobileRuntime, Func<RuntimeEntries.CharacterLuMengMouMobileEntries> getCharacterLuMengMouMobileRuntime, Func<RuntimeEntries.CharacterYuanTanYuanShangOlEntries> getCharacterYuanTanYuanShangOlRuntime, Func<RuntimeEntries.CoreGameEngineAdvancedPoliciesEntries> getCoreGameEngineAdvancedPoliciesRuntime, Func<RuntimeEntries.CharacterZhugeLiangMouEntries> getCharacterZhugeLiangMouRuntime, Func<RuntimeEntries.CharacterPangTongFriendEntries> getCharacterPangTongFriendRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry, Func<RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries> getCoreGameEngineAdvancedWeaponsRuntime, Func<MatchModeRuntime> getMatchModeRuntime, Func<RuntimeEntries.CoreGameEngineModeRulesEntries> getCoreGameEngineModeRulesRuntime, CharacterState player) : IMatchSlashLimitQuery
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
