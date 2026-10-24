using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IStandardBattleHost CreateStandardBattleHost() => new StandardBattleHost(this);

    private sealed class StandardBattleHost(GameEngine engine) : IStandardBattleHost
    {
        public int InitialHandSize => engine._initialHandSize;
        public bool Started => engine._started;
        public IReadOnlyList<StandardBattlePlayerFacts> Players => Array.AsReadOnly(engine._players.Select(player => Player(player.Seat)).ToArray());
        public StandardBattlePlayerFacts Player(int seat)
        {
            var player = engine._players[seat];
            return new(player.Seat, player.General.Id, player.General.Name, player.IsAlive,
                player.GeneralSelected, player.Hp, player.MaxHp, player.TeamId);
        }
        public bool IsWounded(int seat) => engine.IsRuleWounded(engine._players[seat]);
        public bool HasEffectiveSkill(int seat, string skillId) => engine.HasActivitySkill(engine._players[seat], skillId);
        public ShenwuBattleDraftFacts? ShenwuDraft() => engine._shenwuDraft is { } draft ? new(draft.SharedGods, draft.LeaderId) : null;
        public void GrantSkill(int seat, SkillGrant grant)
        {
            engine._players[seat].SkillGrants.Grant(grant);
            engine._skillBindingIndex.Invalidate(seat);
        }
        public void PublishShanheStartingResourceOrigins() => engine.PublishShanheStartingResourceOrigins();
        public void PlaceStartingCard(ShanheStartingCard card, int seat, bool equipment) =>
            engine.PlaceShanhePhysicalCard(card, seat, equipment);
    }
}
