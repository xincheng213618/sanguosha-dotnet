#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRepeatedJudgmentEntries
{
    internal global::CardGame.Core.SkillProgramStepOutcome BeginProgramRepeatedJudgment(global::CardGame.Core.ProgramSkillFrame frame, string reason, string resultBind, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Suit> successSuits) => _resolve().BeginProgramRepeatedJudgment(frame, reason, resultBind, successSuits);
    internal void ContinueProgramRepeatedJudgmentAfterMovement(long frameId) => _resolve().ContinueProgramRepeatedJudgmentAfterMovement(frameId);
    internal bool IsAiProgramRepeatJudgmentPending() => _resolve().IsAiProgramRepeatJudgmentPending();
    internal void ResolvePendingAiProgramRepeatJudgment() => _resolve().ResolvePendingAiProgramRepeatJudgment();
    internal void ResolveProgramRepeatJudgmentChoice(global::CardGame.Core.PromptChoice choice) => _resolve().ResolveProgramRepeatJudgmentChoice(choice);
    internal void ResumeProgramRepeatedJudgment(global::CardGame.Core.JudgmentFrame pending) => _resolve().ResumeProgramRepeatedJudgment(pending);
    internal global::CardGame.Core.CommandResult SubmitProgramRepeatJudgmentAnswer(global::CardGame.Core.PromptChoice choice) => _resolve().SubmitProgramRepeatJudgmentAnswer(choice);
}
