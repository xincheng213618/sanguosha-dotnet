#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineWenheActivitiesEntries
{
    internal global::CardGame.Core.WenheAspirationSnapshot? GetWenheAspirationSnapshot(int viewer) => _resolve().GetWenheAspirationSnapshot(viewer);
    internal global::CardGame.Core.WenheRoundRuleSnapshot? GetWenheRuleSnapshot() => _resolve().GetWenheRuleSnapshot();
    internal bool IsWenhePlayerSkillVisible(global::CardGame.Core.CharacterState player, global::CardGame.Core.GeneralSkillDefinition skill, int viewer, bool revealAll) => _resolve().IsWenhePlayerSkillVisible(player, skill, viewer, revealAll);
    internal void RecordWenheDeathRank(long deathId, int seat) => _resolve().RecordWenheDeathRank(deathId, seat);
    internal bool TryCompleteWenheSurvival() => _resolve().TryCompleteWenheSurvival();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> WenheActivityRuleContributions(global::CardGame.Core.CharacterState player, global::CardGame.Core.SkillRuleQuery query) => _resolve().WenheActivityRuleContributions(player, query);
    internal bool WenheChampionMarkUsed(int seat) => _resolve().WenheChampionMarkUsed(seat);
    internal int WenheGoalRank(int seat) => _resolve().WenheGoalRank(seat);
    internal int? WenheSurvivalRank(int seat) => _resolve().WenheSurvivalRank(seat);
    internal global::CardGame.Core.WenheRuleKind? CurrentWenheRule { get => _resolve().CurrentWenheRule; }
}
