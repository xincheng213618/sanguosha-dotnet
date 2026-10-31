#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceDingxiEntries
{
    internal void AnswerSourceDingxi(PromptChoice selected) => _resolve().AnswerSourceDingxi(selected);
    internal void CaptureSourceDingxiUseDiscards(CardMovementBatchContext batch) => _resolve().CaptureSourceDingxiUseDiscards(batch);
    internal void ContinueSourceDingxi(ProgramSkillFrame f) => _resolve().ContinueSourceDingxi(f);
    internal int[]? SelectSourceDingxiDiscards(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _resolve().SelectSourceDingxiDiscards(batch, candidate, features);
    internal PromptChoice SourceDingxiAiChoice(PendingDecision p, ProgramSkillFrame f) => _resolve().SourceDingxiAiChoice(p, f);
    internal int[] SourceDingxiCandidates(int owner, ProgramSkillWindowContext? context) => _resolve().SourceDingxiCandidates(owner, context);
    internal int SourceDingxiLeft(int owner) => _resolve().SourceDingxiLeft(owner);
    internal GameEngine.SourceDingxiUsePlan? SourceDingxiPlan(int owner, int id, int target) => _resolve().SourceDingxiPlan(owner, id, target);
    internal SkillProgramStepOutcome StartSourceDingxi(ProgramSkillFrame supplied) => _resolve().StartSourceDingxi(supplied);
}
