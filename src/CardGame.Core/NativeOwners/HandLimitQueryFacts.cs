#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct HandLimitQueryFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries> getCoreNativeSkillContextQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouMobileEntries> getCharacterZhouYuMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuYuShengOlEntries> getCharacterLuYuShengOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoTengOlEntries> getCharacterCaoTengOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries> getCoreGameEngineOriginalHandEntitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterGuoYuanMobileShiEntries> getCharacterGuoYuanMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhongHuiMobileShiEntries> getCharacterZhongHuiMobileShiRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouMobileEntries> getCharacterLuMengMouMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries> getCoreGameEngineProgramPersistentCombatPoliciesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineHandComparisonEntries> getCoreGameEngineHandComparisonRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJiangWeiAnniversaryGodEntries> getCharacterJiangWeiAnniversaryGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries> getCharacterZhangFeiAnniversaryGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuLiAnniversaryEntries> getCharacterLiuLiAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterYanXiangAnniversaryEntries> getCharacterYanXiangAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuCheErAnniversaryEntries> getCharacterHuCheErAnniversaryRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineActorHandLimitPenaltiesEntries> getCoreGameEngineActorHandLimitPenaltiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterXuRongOlEntries> getCharacterXuRongOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSuitHandLimitEntries> getCoreGameEngineEquipmentSuitHandLimitRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangYiOlEntries> getCharacterZhangYiOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhuZhiBoundaryMobileEntries> getCharacterZhuZhiBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLiuShanBoundaryMobileEntries> getCharacterLiuShanBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries> getCharacterHanSongAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheTerrainPoliciesEntries> getCoreGameEngineShanheTerrainPoliciesRuntime, CharacterState player) : IMatchHandLimitQuery
{
    public int Seat => player.Seat;
    public bool HasClassicBuqu => getCoreNativeSkillContextQueriesRuntime().HasProgramSkill(player, "classic:buqu");
    public bool HasBoundaryBuqu => getCoreNativeSkillContextQueriesRuntime().HasProgramSkill(player, "boundary:buqu-current");
    public int BuquWoundCount => getCorePhysicalOwnedCardQueriesRuntime().GetBuquWounds(player).Count;
    public int? AssignedDefault => getCoreGameEngineTurnDefaultStatsRuntime().GetCurrentTurnDefaultStat(player.Seat, SkillRuleQuery.HandLimit);
    public int? HappyQiaoHandLimitBase => player.HappyQiaoHealth?.HandLimitBase;
    public int Hp => player.Hp;
    public IEnumerable<RuleQueryContribution> Contributions => getCoreGameEngineRuleQueriesRuntime().CollectNumericRuleContributions(player, SkillRuleQuery.HandLimit);
    public int YingziBonus => getCharacterZhouYuMouMobileRuntime().MouZyMobileHandLimit(player.Seat);
    public int RunweiBonus => getCharacterLuYuShengOlRuntime().LuYuShengRunweiHandLimitBonus(player.Seat);
    public int YongzuBonus => getCharacterCaoTengOlRuntime().CaoTengYongzuHandLimitBonus(player.Seat);
    public int ShunshiBonus => getCharacterDuFuRenOlRuntime().DuFuRenPhaseBonus(player.Seat, SkillRuleQuery.HandLimit);
    public int OriginalHandBonus => getCoreGameEngineOriginalHandEntitiesRuntime().GetOriginalHandHandLimitBonus(player.Seat);
    public int XiugengBonus => getCharacterGuoYuanMobileShiRuntime().ShiGyHandLimitBonus(player.Seat);
    public int XiezhiOverflowCount => getCharacterZhongHuiMobileShiRuntime().ShiZhHuiOverflowCount(player.Seat);
    public int KejiArmorBonus => getCharacterLuMengMouMobileRuntime().MouLuMengArmorHandLimit(player);
    public IEnumerable<RuleQueryContribution> ProgramDamageContributions => getCoreGameEngineProgramPersistentCombatPoliciesRuntime().ProgramDamageHandLimitContributions(player);
    public IEnumerable<RuleQueryContribution> PersistentContributions => getCoreGameEngineHandComparisonRuntime().PersistentHandLimitContributions(player);
    public IEnumerable<RuleQueryContribution> PingxiangContributions => getCharacterJiangWeiAnniversaryGodRuntime().ShenJiangWeiPingxiangHandLimitContributions(player);
    public IEnumerable<RuleQueryContribution> ShenZfContributions => getCharacterZhangFeiAnniversaryGodRuntime().ShenZfHandLimitContributions(player);
    public IEnumerable<RuleQueryContribution> LiuLiContributions => getCharacterLiuLiAnniversaryRuntime().AnniversaryLiuLiHandLimitContributions(player);
    public IEnumerable<RuleQueryContribution> YanXiangContributions => getCharacterYanXiangAnniversaryRuntime().AnniversaryYanXiangHandLimitContributions(player);
    public IEnumerable<RuleQueryContribution> HuCheErContributions => getCharacterHuCheErAnniversaryRuntime().AnniversaryHuCheErHandLimitContributions(player);
    public bool HasLiushiEffect => getContentRegistry().ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ApplyLiushiEffect);
    public int LiushiMarkerCount => player.Markers.GetValueOrDefault(PlayerMarkerKind.LiuShi);
    public IEnumerable<RuleQueryContribution> ActorPenaltyContributions => getCoreGameEngineActorHandLimitPenaltiesRuntime().ActorHandLimitPenaltyContributions(player);
    public int XionghuoPenalty => getCharacterXuRongOlRuntime().XuRongHandLimitPenalty(player);
    public int AdditiveTurnBonus => getCoreGameEngineCardUseModulesRuntime().GetAdditiveTurnRuleModifier(player.Seat, SkillRuleQuery.HandLimit);
    public IEnumerable<(IndexedSkillProgramInstance Source, SkillProgramCardPolicy Policy)> FactionPolicies => getCoreGameEngineProgramCardPoliciesRuntime().CardPolicies(player, SkillProgramCardPolicyKind.FactionHandLimitBonus);

    public int LivingOtherFactionCount(string? factionId)
    {
        var gameMatchParticipantsState = matchParticipantsState;
        var gameGetCoreMatchSetupDefinitionsRuntime = getCoreMatchSetupDefinitionsRuntime;
        var owner = player;
        return gameMatchParticipantsState.ReferenceToPlayers.Count(other => other.IsAlive && other.Seat != owner.Seat && string.Equals(gameGetCoreMatchSetupDefinitionsRuntime().GetEffectiveFactionId(other), factionId, StringComparison.Ordinal));
    }

    public int HengyeGrowth => getCharacterLuMengMouRuntime().GetHengyeGrowth(player);

    public RuleQueryEvaluation ApplyEquipmentSuit(RuleQueryEvaluation evaluation) => getCoreGameEngineEquipmentSuitHandLimitRuntime().ApplyEquipmentSuitHandLimit(player, evaluation);
    public RuleQueryEvaluation ApplyZhangYiKangrui(RuleQueryEvaluation evaluation) => getCharacterZhangYiOlRuntime().ApplyZhangYiKangruiHandLimit(player, evaluation);
    public RuleQueryEvaluation ApplyBoundaryZhuZhi(RuleQueryEvaluation evaluation) => getCharacterZhuZhiBoundaryMobileRuntime().ApplyBoundaryZhuZhiHandLimit(player, evaluation);
    public RuleQueryEvaluation ApplyBoundaryLiuShan(RuleQueryEvaluation evaluation) => getCharacterLiuShanBoundaryMobileRuntime().ApplyBoundaryLiuShanHandLimit(player, evaluation);
    public RuleQueryEvaluation ApplyHanSongYinbi(RuleQueryEvaluation evaluation) => getCharacterHanSongAnniversaryRuntime().ApplyAnniversaryHanSongHandLimit(player, evaluation);
    public RuleQueryEvaluation ApplyMinimumPolicy(RuleQueryEvaluation evaluation) => getCoreGameEngineShanheTerrainPoliciesRuntime().ApplyMinimumHandLimitPolicy(player, evaluation);
}
