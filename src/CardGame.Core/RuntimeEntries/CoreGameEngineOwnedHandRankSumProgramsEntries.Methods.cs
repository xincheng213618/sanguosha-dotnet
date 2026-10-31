#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOwnedHandRankSumProgramsEntries
{
    internal void AssertOwnedHandRankSumSelection(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().AssertOwnedHandRankSumSelection(frame, plan);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginOwnedHandRankSum(global::CardGame.Core.ProgramSkillFrame frame, int exactRankSum, string resultBind) => _resolve().BeginOwnedHandRankSum(frame, exactRankSum, resultBind);
    internal bool CanStartOwnedHandRankSum(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramExecutionPlan plan) => _resolve().CanStartOwnedHandRankSum(owner, plan);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> OwnedHandRankSumChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().OwnedHandRankSumChoices(frame);
    internal void PublishOwnedHandRankSum(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PublishOwnedHandRankSum(frame);
    internal void ResolveOwnedHandRankSumChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveOwnedHandRankSumChoice(selected);
    internal global::CardGame.Core.PromptChoice SelectAiOwnedHandRankSum(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiOwnedHandRankSum(decision, frame);
}
