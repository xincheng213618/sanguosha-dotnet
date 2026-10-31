#nullable enable
namespace CardGame.Core.NativeOwners;
internal readonly struct NumericContributionQueryFacts(Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreActualRoundProgressState actualRoundProgressState, Func<RuntimeEntries.CoreGameEngineRuleQueriesEntries> getCoreGameEngineRuleQueriesRuntime, Func<RuntimeEntries.CoreGameEnginePublicPileSourcesEntries> getCoreGameEnginePublicPileSourcesRuntime, Func<RuntimeEntries.CoreGameEnginePublicFactionCountsEntries> getCoreGameEnginePublicFactionCountsRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreTeamActivityProgramsEntries> getCoreTeamActivityProgramsRuntime, Func<RuntimeEntries.CoreGameEngineCurrentTurnUseKindsEntries> getCoreGameEngineCurrentTurnUseKindsRuntime, Func<RuntimeEntries.CoreGameEngineDrawPhaseDrawHandLimitEntries> getCoreGameEngineDrawPhaseDrawHandLimitRuntime, Func<RuntimeEntries.CoreGameEngineCurrentRoundActualUsesEntries> getCoreGameEngineCurrentRoundActualUsesRuntime, Func<RuntimeEntries.CoreGameEngineShanheQianlongEntries> getCoreGameEngineShanheQianlongRuntime, Func<RuntimeEntries.CoreGameEngineStrategicProgramsEntries> getCoreGameEngineStrategicProgramsRuntime, Func<RuntimeEntries.CharacterLuBuMobileGodEntries> getCharacterLuBuMobileGodRuntime, Func<RuntimeEntries.CoreGameEngineWushuangSkillsBEntries> getCoreGameEngineWushuangSkillsBRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineNumericRuleAurasRuntime>> getCoreGameEngineNumericRuleAurasRuntime, Func<RuntimeEntries.CharacterSimaYiMouEntries> getCharacterSimaYiMouRuntime, Func<RuntimeEntries.CharacterZhangLiaoHappyGodEntries> getCharacterZhangLiaoHappyGodRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterZhangXunAnniversaryRuntime>> getCharacterZhangXunAnniversaryRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHaoMengAnniversaryRuntime>> getCharacterHaoMengAnniversaryRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterHanSongAnniversaryRuntime>> getCharacterHanSongAnniversaryRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoZhangBoundaryMobileRuntime>> getCharacterCaoZhangBoundaryMobileRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterCaoZhenMobileRuntime>> getCharacterCaoZhenMobileRuntime, Func<MatchModeRuntime> getMatchModeRuntime, Func<RuntimeEntries.CoreGameEngineShanheSchoolsEntries> getCoreGameEngineShanheSchoolsRuntime, Func<RuntimeEntries.CoreGameEngineShanheSteadyHandEntries> getCoreGameEngineShanheSteadyHandRuntime, Func<ContentRegistry> getContentRegistry, Func<RuntimeEntries.CoreGameEngineLordSkillProjectionsEntries> getCoreGameEngineLordSkillProjectionsRuntime, Func<RuntimeEntries.CoreNativeSkillContextQueriesEntries> getCoreNativeSkillContextQueriesRuntime, Func<CardZoneStore> getCardZoneStore, CharacterState player, SkillRuleQuery ruleQuery) : IProgramNumericContributionQuery
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
    public IEnumerable<RuleQueryContribution> OtherPublicTeamContributions => getCoreGameEngineNumericRuleAurasRuntime().Value.CollectOtherPublicTeamNumericRuleContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> MouSimaSlashContributions => getCharacterSimaYiMouRuntime().MouSmSlashContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> HappyZhaoLanContributions => getCharacterZhangLiaoHappyGodRuntime().HappyZlNumericContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> ZhangXunSlashContributions => getCharacterZhangXunAnniversaryRuntime().Value.AnniversaryZhangXunSlashContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> HaoMengSlashContributions => getCharacterHaoMengAnniversaryRuntime().Value.AnniversaryHaoMengSlashContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> HanSongContributions => getCharacterHanSongAnniversaryRuntime().Value.AnniversaryHanSongNumericContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> CaoZhangPhaseContributions => getCharacterCaoZhangBoundaryMobileRuntime().Value.BoundaryCaoZhangPhaseContributions(player, ruleQuery);
    public IEnumerable<RuleQueryContribution> CaoZhenSidiContributions => getCharacterCaoZhenMobileRuntime().Value.MobileCaoZhenSidiContributions(player, ruleQuery);
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
