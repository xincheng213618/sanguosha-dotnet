#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineWenheActivitiesRuntime
{
    internal WenheRuleKind? CurrentWenheRule => _coreActivityBattleRulesOperations.Value.WenheRules?.CurrentRule;

    internal WenheRoundRuleSnapshot? GetWenheRuleSnapshot() => _coreActivityBattleRulesOperations.Value.WenheRules?.RoundRuleSnapshot;
    internal int? WenheSurvivalRank(int seat) => _coreActivityBattleRulesOperations.Value.WenheRules?.SurvivalRank(seat);
    internal bool WenheChampionMarkUsed(int seat) => _coreActivityBattleRulesOperations.Value.WenheRules?.ChampionMarkUsed(seat) ?? false;
    internal int WenheGoalRank(int seat) => _coreActivityBattleRulesOperations.Value.WenheRules?.GoalRank(seat) ?? 1;
    internal WenheAspirationSnapshot? GetWenheAspirationSnapshot(int viewer) => _coreActivityBattleRulesOperations.Value.WenheRules?.Aspiration(viewer);
    internal bool IsWenhePlayerSkillVisible(CharacterState player, GeneralSkillDefinition skill, int viewer, bool revealAll) => _coreActivityBattleRulesOperations.Value.WenheRules is not IMatchModeSkillVisibility visibility || visibility.IsSkillVisible(player.Seat, skill.ContentId, viewer, revealAll);
    internal IEnumerable<RuleQueryContribution> WenheActivityRuleContributions(CharacterState player, SkillRuleQuery query) => _coreActivityBattleRulesOperations.Value.WenheRules is IMatchModeNumericRules numeric ? numeric.NumericContributions(player.Seat, query) : [];
    internal void RecordWenheDeathRank(long deathId, int seat) => (_coreActivityBattleRulesOperations.Value.WenheRules as IMatchModeDeathRanking)?.RecordDeathRank(deathId, seat);
    internal bool TryCompleteWenheSurvival() => _coreActivityBattleRulesOperations.Value.WenheRules is IMatchModeLifecycle lifecycle && lifecycle.TryCompleteMatch(ModeCompletionBoundary.AfterCommand);
}
