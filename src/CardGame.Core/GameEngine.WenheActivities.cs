namespace CardGame.Core;

public sealed partial class GameEngine
{
    private int? _winnerSeat;
    private WenheRuleKind? CurrentWenheRule => WenheRules?.CurrentRule;
    private WenheRoundRuleSnapshot? GetWenheRuleSnapshot() => WenheRules?.RoundRuleSnapshot;
    private int? WenheSurvivalRank(int seat) => WenheRules?.SurvivalRank(seat);
    private bool WenheChampionMarkUsed(int seat) => WenheRules?.ChampionMarkUsed(seat) ?? false;
    private int WenheGoalRank(int seat) => WenheRules?.GoalRank(seat) ?? 1;
    private WenheAspirationSnapshot? GetWenheAspirationSnapshot(int viewer) => WenheRules?.Aspiration(viewer);
    private bool IsWenhePlayerSkillVisible(CharacterState player, GeneralSkillDefinition skill, int viewer, bool revealAll) =>
        WenheRules is not IMatchModeSkillVisibility visibility || visibility.IsSkillVisible(player.Seat, skill.ContentId, viewer, revealAll);
    private IEnumerable<RuleQueryContribution> WenheActivityRuleContributions(CharacterState player, SkillRuleQuery query) =>
        WenheRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
    private void RecordWenheDeathRank(long deathId, int seat) =>
        (WenheRules as IMatchModeDeathRanking)?.RecordDeathRank(deathId, seat);
    private bool TryCompleteWenheSurvival() => WenheRules is IMatchModeLifecycle lifecycle &&
        lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterCommand);

}
