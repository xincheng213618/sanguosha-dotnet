#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentProgramsEntries
{
    internal void AdvanceProgramJudgmentCandidate(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame) => _resolve().AdvanceProgramJudgmentCandidate(frame);
    internal void AssertProgramJudgmentWindowState() => _resolve().AssertProgramJudgmentWindowState();
    internal bool CanRunProgramJudgmentTrigger(global::CardGame.Core.ProgramJudgmentTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.JudgmentFinalizedContext judgment) => _resolve().CanRunProgramJudgmentTrigger(candidate, trigger, judgment);
    internal bool CanStartFinalJudgmentEffects(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().CanStartFinalJudgmentEffects(owner, trigger);
    internal void ContinueProgramJudgmentWindowCore() => _resolve().ContinueProgramJudgmentWindowCore();
    internal global::CardGame.Core.ProgramSkillWindowContext CreateFinalizedJudgmentProgramContext(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.ProgramJudgmentTriggerCandidate candidate) => _resolve().CreateFinalizedJudgmentProgramContext(frame, candidate);
    internal void ExposeProgramJudgmentPrompt(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.ProgramJudgmentTriggerCandidate candidate) => _resolve().ExposeProgramJudgmentPrompt(frame, candidate);
    internal bool IsAiProgramJudgmentPending() => _resolve().IsAiProgramJudgmentPending();
    internal bool IsProgramJudgmentPromptValid(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame) => _resolve().IsProgramJudgmentPromptValid(frame);
    internal bool MatchesFinalJudgment(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.CharacterState owner, global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, global::CardGame.Core.Suit effectiveSuit) => _resolve().MatchesFinalJudgment(trigger, owner, pending, judgmentCard, effectiveSuit);
    internal void ResolvePendingAiProgramJudgment() => _resolve().ResolvePendingAiProgramJudgment();
    internal void ResolveProgramJudgmentChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramJudgmentChoice(selected);
    internal void ShortCircuitProgramJudgmentWindow(global::CardGame.Core.ProgramJudgmentTriggerWindowFrame frame, global::CardGame.Core.JudgmentFrame pending) => _resolve().ShortCircuitProgramJudgmentWindow(frame, pending);
    internal global::CardGame.Core.CommandResult SubmitProgramJudgmentTriggerAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => _resolve().SubmitProgramJudgmentTriggerAnswer(actorSeat, prompt, choice);
    internal bool TryBeginProgramJudgmentWindow(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.Card judgmentCard, global::CardGame.Core.Suit effectiveSuit, bool succeeded) => _resolve().TryBeginProgramJudgmentWindow(pending, judgmentCard, effectiveSuit, succeeded);
}
