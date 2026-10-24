namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int BossActivitySeat => (Math.Max(0, _options.HumanSeat) + 3) % _playerCount;
    private int BossActivityStageIndex => CompleteProgramEventHistory().OfType<BossActivityStageStartedEvent>()
        .Select(e => e.StageIndex).DefaultIfEmpty(0).Last();
    private IEnumerable<RuleQueryContribution> BossActivityRuleContributions(CharacterState player, SkillRuleQuery query)
    {
        if (_modeDefinition.BossActivity is not { } a) yield break;
        if (_modeDefinition.HellActivity is not null)
        { foreach (var contribution in HellActivityRuleContributions(player, query)) yield return contribution; yield break; }
        var tier = a.PlayerTier;
        var amount = player.Seat == BossActivitySeat
            ? query == SkillRuleQuery.InitialHandSize ? a.Stages[BossActivityStageIndex].InitialHandSize - _initialHandSize : 0
            : query switch
            {
                SkillRuleQuery.InitialHandSize => (tier >= 3 ? 1 : 0) +
                    (_modeDefinition.HulaoActivity is not null && BossActivityStageIndex == 0 ?
                        3 + (player.Seat - Math.Max(0, _options.HumanSeat) + _playerCount) % _playerCount - _initialHandSize : 0),
                SkillRuleQuery.DrawCount => tier >= 4 ? 1 : 0,
                SkillRuleQuery.SlashLimit => tier >= 3 ? 1 : 0,
                _ => 0
            };
        if (amount != 0) yield return new FiniteRuleQueryContribution($"activity:{_modeDefinition.Id}:tier-{query}", SkillRuleOperation.Add, amount);
    }
}
