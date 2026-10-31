#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentOutcomeDamageEdgesEntries
{
    internal global::CardGame.Core.ProgramSkillFrame? BeforeDamageJudgmentOutcomeRoot(long windowId) => _resolve().BeforeDamageJudgmentOutcomeRoot(windowId);
    internal bool HasExactBeforeDamageJudgmentSubtree(long windowId) => _resolve().HasExactBeforeDamageJudgmentSubtree(windowId);
    internal bool HasExactJudgmentOutcomeSubtree(long judgmentId) => _resolve().HasExactJudgmentOutcomeSubtree(judgmentId);
    internal bool IsExactJudgmentOutcomeFinalizedSubtree(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame window, global::CardGame.Core.JudgmentFrame? judgment, int index) => _resolve().IsExactJudgmentOutcomeFinalizedSubtree(window, judgment, index);
    internal bool IsJudgmentOutcomeProgramDying() => _resolve().IsJudgmentOutcomeProgramDying();
    internal bool JudgmentOutcomeDamageEdge(int index, global::CardGame.Core.ProgramSkillFrame root) => _resolve().JudgmentOutcomeDamageEdge(index, root);
}
