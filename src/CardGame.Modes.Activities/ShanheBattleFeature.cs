using CardGame.Core;
using CardGame.Core.Campaigns;

namespace CardGame.Modes.Activities;

internal sealed class ShanheBattleFeature :
    IShanheBattleFeature, IMatchModeNumericRules, IMatchModeLifecycle, IMatchModeInitialDeal, IMatchModeDamageRuleProvider
{
    private readonly ShanheBattleSetup setup;
    private readonly IStandardBattleHost host;
    int IMatchModeNumericRules.Order => 20;
    int IMatchModeLifecycle.Order => 10;
    int IMatchModeInitialDeal.Order => 10;
    public IReadOnlyList<ModeDamageRuleRegistration> DamageRules { get; }
    internal ShanheBattleFeature(ShanheBattleSetup setup, IStandardBattleHost host)
    {
        this.setup = setup; this.host = host;
        DamageRules = Array.AsReadOnly<ModeDamageRuleRegistration>([ModeDamageRuleRegistration.Additive(280, damage =>
        {
            if (damage.IsSourceLess || damage.IsChainPropagation || damage.SourceSeat <= setup.AllyRoster.Count ||
                damage.SourceSeat > setup.AllyRoster.Count + setup.EnemyRoster.Count) return 0;
            return NumericMember(damage.SourceSeat).ExtraDamage;
        })]);
    }

    private ShanheCombatantDefinition NumericMember(int seat)
    {
        if (setup.BossTransition is { Combatant: { } stage } transition &&
            seat == setup.AllyRoster.Count + 1 + transition.EnemyIndex && host.Player(seat).GeneralId == transition.GeneralId)
            return stage;
        return seat == 0 ? new ShanheCombatantDefinition(setup.PlayerGeneralId,
            setup.MaxHpBonus, setup.Hp, setup.InitialHandBonus, setup.DrawBonus, setup.SlashBonus) :
            seat <= setup.AllyRoster.Count ? setup.AllyRoster[seat - 1] : setup.EnemyRoster[seat - setup.AllyRoster.Count - 1];
    }

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        var member = NumericMember(seat);
        var bonus = query switch
        {
            SkillRuleQuery.InitialHandSize => member.InitialHandBonus,
            SkillRuleQuery.DrawCount => member.DrawBonus,
            SkillRuleQuery.SlashLimit => member.SlashBonus,
            _ => 0
        };
        if (bonus != 0) yield return new FiniteRuleQueryContribution($"campaign:shanhe:{query}", SkillRuleOperation.Add, bonus);
    }

    public void OnMatchStart(ModeMatchStartPhase phase)
    {
        if (phase == ModeMatchStartPhase.BeforeRunning && setup.StartingResourcesExpireOnUse)
            host.PublishShanheStartingResourceOrigins();
    }

    public int? InitialHandSizeOverride(int seat) => seat == 0 ? null :
        seat <= setup.AllyRoster.Count ? setup.AllyRoster[seat - 1].StartingHand?.Count :
            setup.EnemyRoster[seat - setup.AllyRoster.Count - 1].StartingHand?.Count;

    public bool TryDealInitialCard(int seat, int cardIndex)
    {
        if (seat == 0) return false;
        var hand = seat <= setup.AllyRoster.Count ? setup.AllyRoster[seat - 1].StartingHand :
            setup.EnemyRoster[seat - setup.AllyRoster.Count - 1].StartingHand;
        if (hand is null || cardIndex >= hand.Count) return false;
        host.PlaceStartingCard(hand[cardIndex], seat, equipment: false);
        return true;
    }

    public void AfterInitialDeal()
    {
        foreach (var card in setup.StartingCards) host.PlaceStartingCard(card, 0, equipment: false);
        foreach (var card in setup.StartingEquipment)
            host.PlaceStartingCard(card, 0, equipment: !setup.StartingResourcesExpireOnUse);
        for (var seat = 1; seat < host.Players.Count; seat++)
        {
            var member = seat <= setup.AllyRoster.Count ? setup.AllyRoster[seat - 1] :
                setup.EnemyRoster[seat - setup.AllyRoster.Count - 1];
            foreach (var card in member.StartingCards ?? []) host.PlaceStartingCard(card, seat, equipment: false);
        }
        if (host.Started) host.PublishShanheStartingResourceOrigins();
    }
}
