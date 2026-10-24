using CardGame.Core;

namespace CardGame.Modes.Activities;

internal sealed class BossBattleNumericRules(GameOptions options, ContentModeDefinition mode,
    IStandardBattleHost host, IModeEncounterHost encounter) : IMatchModeNumericRules
{
    public int Order => 60;

    public IEnumerable<RuleQueryContribution> NumericContributions(int seat, SkillRuleQuery query)
    {
        var activity = mode.BossActivity!;
        var bossSeat = (Math.Max(0, options.HumanSeat) + 3) % options.PlayerCount;
        var stage = encounter.History<BossActivityStageStartedEvent>().Select(fact => fact.StageIndex).DefaultIfEmpty(0).Last();
        var tier = activity.PlayerTier;
        var amount = seat == bossSeat
            ? query == SkillRuleQuery.InitialHandSize ? activity.Stages[stage].InitialHandSize - host.InitialHandSize : 0
            : query switch
            {
                SkillRuleQuery.InitialHandSize => (tier >= 3 ? 1 : 0) +
                    (mode.HulaoActivity is not null && stage == 0 ?
                        3 + (seat - Math.Max(0, options.HumanSeat) + options.PlayerCount) % options.PlayerCount - host.InitialHandSize : 0),
                SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
                SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
                _ => 0
            };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"activity:{mode.Id}:tier-{query}", SkillRuleOperation.Add, amount);
    }
}
