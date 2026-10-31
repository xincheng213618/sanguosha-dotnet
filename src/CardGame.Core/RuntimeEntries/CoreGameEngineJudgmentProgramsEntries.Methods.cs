#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentProgramsEntries
{
    internal void AdvanceProgramJudgmentCandidate(ProgramJudgmentTriggerWindowFrame frame) => _resolve().AdvanceProgramJudgmentCandidate(frame);
    internal bool CanRunProgramJudgmentTrigger(ProgramJudgmentTriggerCandidate candidate, SkillProgramTrigger trigger, JudgmentFinalizedContext judgment) => _resolve().CanRunProgramJudgmentTrigger(candidate, trigger, judgment);
    internal bool CanStartFinalJudgmentEffects(CharacterState owner, SkillProgramTrigger trigger) => _resolve().CanStartFinalJudgmentEffects(owner, trigger);
    internal void ContinueProgramJudgmentWindowCore() => _resolve().ContinueProgramJudgmentWindowCore();
    internal ProgramSkillWindowContext CreateFinalizedJudgmentProgramContext(ProgramJudgmentTriggerWindowFrame frame, ProgramJudgmentTriggerCandidate candidate) => _resolve().CreateFinalizedJudgmentProgramContext(frame, candidate);
    internal void ExposeProgramJudgmentPrompt(ProgramJudgmentTriggerWindowFrame frame, ProgramJudgmentTriggerCandidate candidate) => _resolve().ExposeProgramJudgmentPrompt(frame, candidate);
    internal bool IsAiProgramJudgmentPending() => _resolve().IsAiProgramJudgmentPending();
    internal bool MatchesFinalJudgment(SkillProgramTrigger trigger, CharacterState owner, JudgmentFrame pending, Card judgmentCard, Suit effectiveSuit) => _resolve().MatchesFinalJudgment(trigger, owner, pending, judgmentCard, effectiveSuit);
    internal void ResolvePendingAiProgramJudgment() => _resolve().ResolvePendingAiProgramJudgment();
    internal void ResolveProgramJudgmentChoice(PromptChoice selected) => _resolve().ResolveProgramJudgmentChoice(selected);
    internal void ShortCircuitProgramJudgmentWindow(ProgramJudgmentTriggerWindowFrame frame, JudgmentFrame pending) => _resolve().ShortCircuitProgramJudgmentWindow(frame, pending);
    internal CommandResult SubmitProgramJudgmentTriggerAnswer(int actorSeat, PromptId prompt, ChoiceId choice) => _resolve().SubmitProgramJudgmentTriggerAnswer(actorSeat, prompt, choice);
    internal bool TryBeginProgramJudgmentWindow(JudgmentFrame pending, Card judgmentCard, Suit effectiveSuit, bool succeeded) => _resolve().TryBeginProgramJudgmentWindow(pending, judgmentCard, effectiveSuit, succeeded);
}
