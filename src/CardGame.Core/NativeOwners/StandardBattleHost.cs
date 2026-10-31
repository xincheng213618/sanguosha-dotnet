#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class StandardBattleHost(EngineStates.CoreMatchDefinitionState matchDefinitionState, EngineStates.CoreActualTurnProgressState actualTurnProgressState, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, Func<RuntimeEntries.CoreGameEngineTeamActivitiesEntries> getCoreGameEngineTeamActivitiesRuntime, Func<RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, EngineStates.CoreGameEngineShenwuActivitiesState gameEngineShenwuActivitiesState, Func<MatchSkillBindingIndex> getMatchSkillBindingIndex, Func<RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries> getCoreGameEngineShanheStartingResourcesRuntime, Func<RuntimeEntries.CoreGameEngineShanheCampaignsEntries> getCoreGameEngineShanheCampaignsRuntime) : IStandardBattleHost
{
    public int InitialHandSize => matchDefinitionState.ReferenceToInitialHandSize;
    public bool Started => actualTurnProgressState.ReferenceToStarted;
    public IReadOnlyList<StandardBattlePlayerFacts> Players => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => Player(player.Seat)).ToArray());

    public StandardBattlePlayerFacts Player(int seat)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        return new(player.Seat, player.General.Id, player.General.Name, player.IsAlive, player.GeneralSelected, player.Hp, player.MaxHp, player.TeamId);
    }

    public bool IsWounded(int seat) => getCoreGameEngineTreatedWoundedRuntime().IsRuleWounded(matchParticipantsState.ReferenceToPlayers[seat]);
    public bool HasEffectiveSkill(int seat, string skillId) => getCoreGameEngineTeamActivitiesRuntime().HasActivitySkill(matchParticipantsState.ReferenceToPlayers[seat], skillId);
    public CardConversionSource? ActiveShanheDongdangSource(int seat)
    {
        var player = matchParticipantsState.ReferenceToPlayers[seat];
        const string skill = ShanheSourceDongdangRules.SkillId;
        if (!getCoreGameEngineTeamActivitiesRuntime().HasActivitySkill(player, skill))
            return null;
        var instance = getCoreGameEngineNationalSkillsRuntime().GetRuntimeSkillInstanceId(player, skill);
        if (!getCoreGameEngineNationalSkillsRuntime().HasRuntimeSkillInstance(player, skill, instance))
            throw new InvalidOperationException("Source Dongdang lost its effective entry instance.");
        return new(skill, ShanheSourceDongdangRules.BindingId, seat, instance);
    }

    public ShenwuBattleDraftFacts? ShenwuDraft() => gameEngineShenwuActivitiesState.ReferenceToShenwuDraft is { } draft ? new(draft.SharedGods, draft.LeaderId) : null;
    public void GrantSkill(int seat, SkillGrant grant)
    {
        matchParticipantsState.ReferenceToPlayers[seat].SkillGrants.Grant(grant);
        getMatchSkillBindingIndex().Invalidate(seat);
    }

    public void PublishShanheStartingResourceOrigins() => getCoreGameEngineShanheStartingResourcesRuntime().PublishShanheStartingResourceOrigins();
    public void PlaceStartingCard(ShanheStartingCard card, int seat, bool equipment) => getCoreGameEngineShanheCampaignsRuntime().PlaceShanhePhysicalCard(card, seat, equipment);
}
