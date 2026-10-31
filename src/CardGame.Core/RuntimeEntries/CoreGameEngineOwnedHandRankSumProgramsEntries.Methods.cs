#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedHandRankSumProgramsEntries
{
    internal SkillProgramStepOutcome BeginOwnedHandRankSum(ProgramSkillFrame frame, int exactRankSum, string resultBind) => _resolve().BeginOwnedHandRankSum(frame, exactRankSum, resultBind);
    internal bool CanStartOwnedHandRankSum(CharacterState owner, ProgramExecutionPlan plan) => _resolve().CanStartOwnedHandRankSum(owner, plan);
    internal IReadOnlyList<PromptChoice> OwnedHandRankSumChoices(ProgramSkillFrame frame) => _resolve().OwnedHandRankSumChoices(frame);
    internal void PublishOwnedHandRankSum(ProgramSkillFrame frame) => _resolve().PublishOwnedHandRankSum(frame);
    internal void ResolveOwnedHandRankSumChoice(PromptChoice selected) => _resolve().ResolveOwnedHandRankSumChoice(selected);
    internal PromptChoice SelectAiOwnedHandRankSum(PendingDecision decision, ProgramSkillFrame frame) => _resolve().SelectAiOwnedHandRankSum(decision, frame);
}
