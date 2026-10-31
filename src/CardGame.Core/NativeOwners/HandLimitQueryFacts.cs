#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct HandLimitQueryFacts(Func<RuntimeEntries.CoreNativeSkillContextQueriesEntries> getCoreNativeSkillContextQueriesRuntime, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries> getCoreGameEngineTurnDefaultStatsRuntime, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CharacterZhouYuMouMobileEntries> getCharacterZhouYuMouMobileRuntime, Func<RuntimeEntries.CharacterLuYuShengOlEntries> getCharacterLuYuShengOlRuntime, Func<RuntimeEntries.CharacterCaoTengOlEntries> getCharacterCaoTengOlRuntime, Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, Func<RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries> getCoreGameEngineOriginalHandEntitiesRuntime, Func<RuntimeEntries.CharacterGuoYuanMobileShiEntries> getCharacterGuoYuanMobileShiRuntime, Func<RuntimeEntries.CharacterZhongHuiMobileShiEntries> getCharacterZhongHuiMobileShiRuntime, Func<RuntimeEntries.CharacterLuMengMouMobileEntries> getCharacterLuMengMouMobileRuntime, Func<RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries> getCoreGameEngineProgramPersistentCombatPoliciesRuntime, Func<RuntimeEntries.CoreGameEngineHandComparisonEntries> getCoreGameEngineHandComparisonRuntime, Func<RuntimeEntries.CharacterJiangWeiAnniversaryGodEntries> getCharacterJiangWeiAnniversaryGodRuntime, Func<RuntimeEntries.CharacterZhangFeiAnniversaryGodEntries> getCharacterZhangFeiAnniversaryGodRuntime, Func<RuntimeEntries.CharacterLiuLiAnniversaryEntries> getCharacterLiuLiAnniversaryRuntime, Func<RuntimeEntries.CharacterYanXiangAnniversaryEntries> getCharacterYanXiangAnniversaryRuntime, Func<RuntimeEntries.CharacterHuCheErAnniversaryEntries> getCharacterHuCheErAnniversaryRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineActorHandLimitPenaltiesEntries> getCoreGameEngineActorHandLimitPenaltiesRuntime, Func<RuntimeEntries.CharacterXuRongOlEntries> getCharacterXuRongOlRuntime, Func<RuntimeEntries.CoreGameEngineCardUseModulesEntries> getCoreGameEngineCardUseModulesRuntime, Func<RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries> getCoreGameEngineProgramCardPoliciesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreMatchSetupDefinitionsEntries> getCoreMatchSetupDefinitionsRuntime, Func<RuntimeEntries.CharacterLuMengMouEntries> getCharacterLuMengMouRuntime, Func<RuntimeEntries.CoreGameEngineEquipmentSuitHandLimitEntries> getCoreGameEngineEquipmentSuitHandLimitRuntime, Func<RuntimeEntries.CharacterZhangYiOlEntries> getCharacterZhangYiOlRuntime, Func<RuntimeEntries.CharacterZhuZhiBoundaryMobileEntries> getCharacterZhuZhiBoundaryMobileRuntime, Func<RuntimeEntries.CharacterLiuShanBoundaryMobileEntries> getCharacterLiuShanBoundaryMobileRuntime, Func<RuntimeEntries.CharacterHanSongAnniversaryEntries> getCharacterHanSongAnniversaryRuntime, Func<RuntimeEntries.CoreGameEngineShanheTerrainPoliciesEntries> getCoreGameEngineShanheTerrainPoliciesRuntime, CharacterState player) : IMatchHandLimitQuery
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
