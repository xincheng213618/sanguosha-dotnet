#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceDingxiEntries
{
    internal void AnswerSourceDingxi(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerSourceDingxi(selected);
    internal void CaptureSourceDingxiUseDiscards(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureSourceDingxiUseDiscards(batch);
    internal void ContinueSourceDingxi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueSourceDingxi(f);
    internal int[]? SelectSourceDingxiDiscards(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().SelectSourceDingxiDiscards(batch, candidate, features);
    internal global::CardGame.Core.PromptChoice SourceDingxiAiChoice(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().SourceDingxiAiChoice(p, f);
    internal int[] SourceDingxiCandidates(int owner, global::CardGame.Core.ProgramSkillWindowContext? context) => _resolve().SourceDingxiCandidates(owner, context);
    internal int SourceDingxiLeft(int owner) => _resolve().SourceDingxiLeft(owner);
    internal global::CardGame.Core.GameEngine.SourceDingxiUsePlan? SourceDingxiPlan(int owner, int id, int target) => _resolve().SourceDingxiPlan(owner, id, target);
    internal global::CardGame.Core.SkillProgramStepOutcome StartSourceDingxi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().StartSourceDingxi(supplied);
}
