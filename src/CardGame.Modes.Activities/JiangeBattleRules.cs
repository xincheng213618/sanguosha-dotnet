using CardGame.Core;

namespace CardGame.Modes.Activities;

internal sealed class JiangeBattleRules(GameOptions options, ContentModeDefinition mode, IStandardBattleHost host) :
    IJiangeBattleFeature, IMatchModeNumericRules, IMatchModeDistanceRules
{
    int IMatchModeNumericRules.Order => 70;
    int IMatchModeDistanceRules.Order => 10;
    private int Offset(int seat) => (seat - Math.Max(0, options.HumanSeat) + options.PlayerCount) % options.PlayerCount;
    private static bool SameTeam(StandardBattlePlayerFacts first, StandardBattlePlayerFacts second) =>
        first.TeamId is not null && first.TeamId == second.TeamId;

    public ModeDamageCapPlan? AlliedDamageCap(int targetSeat)
    {
        var target = host.Player(targetSeat);
        var owner = host.Players.FirstOrDefault(player => SameTeam(player, target) &&
            host.HasEffectiveSkill(player.Seat, "jiange:chiying"));
        return owner is null ? null : new(owner.Seat, "jiange:chiying", "allied-damage-cap", 1);
    }

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        var activity = mode.TeamActivity!;
        var regular = Offset(seat) is 0 or 5;
        var tier = regular ? activity.PlayerTier : Offset(seat) is 2 or 3
            ? activity.SpiritTierMinimum + (int)(((uint)options.Seed + (uint)Offset(seat)) %
                (uint)(activity.SpiritTierMaximum - activity.SpiritTierMinimum + 1)) : activity.MachineTier;
        var value = query switch
        {
            SkillRuleQuery.InitialHandSize => regular ? (tier >= 3 ? 1 : 0) : (tier >= 2 ? 1 : 0) + (tier >= 4 ? 1 : 0) + (tier >= 5 ? 1 : 0),
            SkillRuleQuery.DrawCount => regular ? (tier >= 4 ? 1 : 0) : (tier >= 2 ? 1 : 0) + (tier >= 4 ? 1 : 0),
            SkillRuleQuery.SlashLimit => (tier >= 3 ? 1 : 0) + (!regular && tier >= 5 ? 1 : 0),
            _ => 0
        };
        if (value != 0) yield return new FiniteRuleQueryContribution($"activity:{mode.Id}:tier-{query}", SkillRuleOperation.Add, value);
        if (query == SkillRuleQuery.DrawCount && host.IsWounded(seat))
        {
            var player = host.Player(seat);
            foreach (var owner in host.Players.Where(owner => !SameTeam(owner, player) && host.HasEffectiveSkill(owner.Seat, "jiange:jueji")))
                yield return new FiniteRuleQueryContribution($"jiange:jueji:{owner.Seat}", SkillRuleOperation.Add, -1);
        }
    }

    public IEnumerable<RuleQueryBaseTerm> BaseDistanceTerms(int sourceSeat, int targetSeat)
    {
        var source = host.Player(sourceSeat);
        var target = host.Player(targetSeat);
        var amount = SameTeam(source, target) ? 0 : -host.Players.Count(player => player.Seat != sourceSeat &&
            SameTeam(player, source) && host.HasEffectiveSkill(player.Seat, "jiange:jingfan"));
        yield return new("jiange:jingfan:allied-distance", amount);
    }
}
