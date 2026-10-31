#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenheActivitiesRuntime
{
    internal WenheRuleKind? CurrentWenheRule => _coreActivityBattleRulesOperations.WenheRules?.CurrentRule;

    internal WenheRoundRuleSnapshot? GetWenheRuleSnapshot() => _coreActivityBattleRulesOperations.WenheRules?.RoundRuleSnapshot;
    internal int? WenheSurvivalRank(int seat) => _coreActivityBattleRulesOperations.WenheRules?.SurvivalRank(seat);
    internal bool WenheChampionMarkUsed(int seat) => _coreActivityBattleRulesOperations.WenheRules?.ChampionMarkUsed(seat) ?? false;
    internal int WenheGoalRank(int seat) => _coreActivityBattleRulesOperations.WenheRules?.GoalRank(seat) ?? 1;
    internal WenheAspirationSnapshot? GetWenheAspirationSnapshot(int viewer) => _coreActivityBattleRulesOperations.WenheRules?.Aspiration(viewer);
    internal bool IsWenhePlayerSkillVisible(CharacterState player, GeneralSkillDefinition skill, int viewer, bool revealAll) => _coreActivityBattleRulesOperations.WenheRules is not IMatchModeSkillVisibility visibility || visibility.IsSkillVisible(player.Seat, skill.ContentId, viewer, revealAll);
    internal IEnumerable<RuleQueryContribution> WenheActivityRuleContributions(CharacterState player, SkillRuleQuery query) => _coreActivityBattleRulesOperations.WenheRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
    internal void RecordWenheDeathRank(long deathId, int seat) => (_coreActivityBattleRulesOperations.WenheRules as IMatchModeDeathRanking)?.RecordDeathRank(deathId, seat);
    internal bool TryCompleteWenheSurvival() => _coreActivityBattleRulesOperations.WenheRules is IMatchModeLifecycle lifecycle && lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterCommand);
}
