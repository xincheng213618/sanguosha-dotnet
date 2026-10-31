#nullable enable
using CardGame.Core.Campaigns;

namespace CardGame.Core.NativeOwners;
internal sealed class StandardBattleHost(global::CardGame.Core.EngineStates.CoreMatchDefinitionState matchDefinitionState, global::CardGame.Core.EngineStates.CoreActualTurnProgressState actualTurnProgressState, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries> getCoreGameEngineTreatedWoundedRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineTeamActivitiesEntries> getCoreGameEngineTeamActivitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries> getCoreGameEngineNationalSkillsRuntime, global::CardGame.Core.EngineStates.CoreGameEngineShenwuActivitiesState gameEngineShenwuActivitiesState, global::System.Func<global::CardGame.Core.MatchSkillBindingIndex> getMatchSkillBindingIndex, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries> getCoreGameEngineShanheStartingResourcesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheCampaignsEntries> getCoreGameEngineShanheCampaignsRuntime) : IStandardBattleHost
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
