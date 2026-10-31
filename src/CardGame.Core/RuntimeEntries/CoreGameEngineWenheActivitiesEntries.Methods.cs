#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheActivitiesEntries
{
    internal WenheAspirationSnapshot? GetWenheAspirationSnapshot(int viewer) => _resolve().GetWenheAspirationSnapshot(viewer);
    internal WenheRoundRuleSnapshot? GetWenheRuleSnapshot() => _resolve().GetWenheRuleSnapshot();
    internal bool IsWenhePlayerSkillVisible(CharacterState player, GeneralSkillDefinition skill, int viewer, bool revealAll) => _resolve().IsWenhePlayerSkillVisible(player, skill, viewer, revealAll);
    internal void RecordWenheDeathRank(long deathId, int seat) => _resolve().RecordWenheDeathRank(deathId, seat);
    internal bool TryCompleteWenheSurvival() => _resolve().TryCompleteWenheSurvival();
    internal IEnumerable<RuleQueryContribution> WenheActivityRuleContributions(CharacterState player, SkillRuleQuery query) => _resolve().WenheActivityRuleContributions(player, query);
    internal bool WenheChampionMarkUsed(int seat) => _resolve().WenheChampionMarkUsed(seat);
    internal int WenheGoalRank(int seat) => _resolve().WenheGoalRank(seat);
    internal int? WenheSurvivalRank(int seat) => _resolve().WenheSurvivalRank(seat);
    internal WenheRuleKind? CurrentWenheRule { get => _resolve().CurrentWenheRule; }
}
