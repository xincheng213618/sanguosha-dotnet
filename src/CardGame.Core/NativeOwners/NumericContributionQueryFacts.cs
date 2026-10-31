#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct NumericContributionQueryFacts(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreActualRoundProgressState actualRoundProgressState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEnginePublicFactionCountsEntries> getCoreGameEnginePublicFactionCountsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTeamActivityProgramsEntries> getCoreTeamActivityProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawPhaseDrawHandLimitEntries> getCoreGameEngineDrawPhaseDrawHandLimitRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries> getCoreGameEngineCurrentRoundActualUsesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheQianlongEntries> getCoreGameEngineShanheQianlongRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLuBuMobileGodEntries> getCharacterLuBuMobileGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangSkillsBEntries> getCoreGameEngineWushuangSkillsBRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNumericRuleAurasEntries> getCoreGameEngineNumericRuleAurasRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangLiaoHappyGodEntries> getCharacterZhangLiaoHappyGodRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangXunAnniversaryEntries> getCharacterZhangXunAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHaoMengAnniversaryEntries> getCharacterHaoMengAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHanSongAnniversaryEntries> getCharacterHanSongAnniversaryRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhangBoundaryMobileEntries> getCharacterCaoZhangBoundaryMobileRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterCaoZhenMobileEntries> getCharacterCaoZhenMobileRuntime, global::System.Func<global::CardGame.Core.MatchModeRuntime> getMatchModeRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSchoolsEntries> getCoreGameEngineShanheSchoolsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheSteadyHandEntries> getCoreGameEngineShanheSteadyHandRuntime, global::System.Func<global::CardGame.Core.ContentRegistry> getContentRegistry, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries> getCoreGameEngineLordSkillProjectionsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries> getCoreNativeSkillContextQueriesRuntime, global::System.Func<global::CardGame.Core.CardZoneStore> getCardZoneStore, CharacterState player, SkillRuleQuery ruleQuery) : IProgramNumericContributionQuery
{
    public IReadOnlyList<IndexedSkillProgramModifier> Modifiers => getCoreGameEngineNationalSkillsRuntime().GetSkillBindingShard(player).GetNumericModifiers(ruleQuery);
    public int RoundNumber => actualRoundProgressState.ReferenceToRoundNumber;
    public int LivingFactionCount => getCoreGameEngineRuleQueriesRuntime().GetLivingFactionCount();
    public bool SupportsMultiplePublicPiles => getCoreGameEnginePublicPileSourcesRuntime().SupportsMultiplePublicPiles;
    public int PublicLivingFactionCount => getCoreGameEnginePublicFactionCountsRuntime().GetPublicLivingFactionCount();

    public int PublicTeamEnemyCount
    {
        get
        {
            var gameMatchParticipantsState = matchParticipantsState;
            var owner = player;
            return gameMatchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive && p.TeamRevealed && owner.TeamRevealed && p.TeamId is not null && owner.TeamId is not null && p.TeamId != owner.TeamId);
        }
    }

    public int OtherPublicTeamAllyCount
    {
        get
        {
            var gameMatchParticipantsState = matchParticipantsState;
            var gameGetCoreTeamActivityProgramsRuntime = getCoreTeamActivityProgramsRuntime;
            var owner = player;
            return gameMatchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive && gameGetCoreTeamActivityProgramsRuntime().OtherRevealedPublicTeamAlly(owner, p));
        }
    }

    public int OtherLivingPlayerCount
    {
        get
        {
            var gameMatchParticipantsState = matchParticipantsState;
            var owner = player;
            return gameMatchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive && p.Seat != owner.Seat);
        }
    }

    public int LivingFemaleCount => matchParticipantsState.ReferenceToPlayers.Count(p => p.IsAlive && p.Gender == GeneralGender.Female);
    public int CurrentTurnUsedHandSuitCount => getCoreGameEngineCurrentTurnUseKindsRuntime().CurrentTurnUseKinds(player.Seat).HandSuits;
    public int CurrentTurnDrawPhaseDrawCount => getCoreGameEngineDrawPhaseDrawHandLimitRuntime().CurrentTurnDrawPhaseDrawCount(player.Seat);
    public int CurrentRoundActualUseCount => getCoreGameEngineCurrentRoundActualUsesRuntime().CurrentRoundActualUseCount(player.Seat);
    public int EmptyCampaignSkillSlotCount => getCoreGameEngineShanheQianlongRuntime().ShanheEmptyCampaignSkillSlotCount(player.Seat);
    public IEnumerable<RuleQueryContribution> StrategicContributions => getCoreGameEngineStrategicProgramsRuntime().CollectStrategicRuleContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> WuqianContributions => getCharacterLuBuMobileGodRuntime().MobileWuqianRuleContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> WushuangContributions => getCoreGameEngineWushuangSkillsBRuntime().WushuangSkillsBNumericContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> OtherPublicTeamContributions => getCoreGameEngineNumericRuleAurasRuntime().CollectOtherPublicTeamNumericRuleContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> MouSimaSlashContributions => getCharacterSimaYiMouRuntime().MouSmSlashContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> HappyZhaoLanContributions => getCharacterZhangLiaoHappyGodRuntime().HappyZlNumericContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> ZhangXunSlashContributions => getCharacterZhangXunAnniversaryRuntime().AnniversaryZhangXunSlashContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> HaoMengSlashContributions => getCharacterHaoMengAnniversaryRuntime().AnniversaryHaoMengSlashContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> HanSongContributions => getCharacterHanSongAnniversaryRuntime().AnniversaryHanSongNumericContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> CaoZhangPhaseContributions => getCharacterCaoZhangBoundaryMobileRuntime().BoundaryCaoZhangPhaseContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> CaoZhenSidiContributions => getCharacterCaoZhenMobileRuntime().MobileCaoZhenSidiContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> ModeContributions => getMatchModeRuntime().NumericContributions(player.Seat, ruleQuery);

    public bool SchoolQualified(string skillId) => getCoreGameEngineShanheSchoolsRuntime().IsShanheSchoolSkillQualified(player.Seat, skillId);
    public bool SteadyHandQualified(string skillId) => getCoreGameEngineShanheSteadyHandRuntime().IsShanheSteadyHandModifierQualified(player.Seat, skillId);
    public bool IsLordSkill(string skillId) => getContentRegistry().GetSkill(skillId).Tags.HasFlag(SkillTag.Lord);
    public bool HasLordRoleQualification(string skillId, string instanceId) => getCoreGameEngineLordSkillProjectionsRuntime().HasSkillRoleQualification(player, skillId, instanceId, Role.Lord);
    public PlayerSkillContext CreateOwnerContext() => getCoreNativeSkillContextQueriesRuntime().CreateSkillContext(player);
    public int OwnedZoneCount(CardZoneKind zone) => getCardZoneStore().Count(new CardLocation(zone, player.Seat));
    public int PublicPileCount(string skillId, string instanceId) => getCoreGameEnginePublicPileSourcesRuntime().PublicPileProgramCount(player.Seat, skillId, instanceId);
    public int OwnerMarkerCount(PlayerMarkerKind marker) => player.Markers.GetValueOrDefault(marker);
}
