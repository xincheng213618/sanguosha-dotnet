#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRepeatedJudgmentEntries
{
    internal SkillProgramStepOutcome BeginProgramRepeatedJudgment(ProgramSkillFrame frame, string reason, string resultBind, IReadOnlyList<Suit> successSuits) => _resolve().BeginProgramRepeatedJudgment(frame, reason, resultBind, successSuits);
    internal void ContinueProgramRepeatedJudgmentAfterMovement(long frameId) => _resolve().ContinueProgramRepeatedJudgmentAfterMovement(frameId);
    internal bool IsAiProgramRepeatJudgmentPending() => _resolve().IsAiProgramRepeatJudgmentPending();
    internal void ResolvePendingAiProgramRepeatJudgment() => _resolve().ResolvePendingAiProgramRepeatJudgment();
    internal void ResolveProgramRepeatJudgmentChoice(PromptChoice choice) => _resolve().ResolveProgramRepeatJudgmentChoice(choice);
    internal void ResumeProgramRepeatedJudgment(JudgmentFrame pending) => _resolve().ResumeProgramRepeatedJudgment(pending);
    internal CommandResult SubmitProgramRepeatJudgmentAnswer(PromptChoice choice) => _resolve().SubmitProgramRepeatJudgmentAnswer(choice);
}
