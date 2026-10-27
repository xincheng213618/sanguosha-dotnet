using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private IStandardBattleHost CreateStandardBattleHost()
    {
        if (_options.ShanheBattle is not null && _contentRegistry.Skills.TryGetValue(ShanheSourceDongdangRules.SkillId, out var definition))
            ShanheSourceDongdangRules.ValidateDefinition(definition);
        return new StandardBattleHost(this);
    }

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
        public CardConversionSource? ActiveShanheDongdangSource(int seat)
        {
            var player = engine._players[seat];
            const string skill = ShanheSourceDongdangRules.SkillId;
            if (!engine.HasActivitySkill(player, skill)) return null;
            var instance = engine.GetRuntimeSkillInstanceId(player, skill);
            if (!engine.HasRuntimeSkillInstance(player, skill, instance))
                throw new InvalidOperationException("Source Dongdang lost its effective entry instance.");
            return new(skill, ShanheSourceDongdangRules.BindingId, seat, instance);
        }
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
