#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentReplacementProgramsEntries
{
    internal void BeginProgramJudgmentReplacementChoice(JudgmentFrame pending, JudgmentTriggerCandidate candidate) => _resolve().BeginProgramJudgmentReplacementChoice(pending, candidate);
    internal void CommitProgramJudgmentReplacement(JudgmentFrame pending, CharacterState owner, Card oldJudgmentCard, Card replacement, Suit committedSuit, SkillProgramOldJudgmentCardDestination oldDestination, CardLocation? exactRiceLocation = default) => _resolve().CommitProgramJudgmentReplacement(pending, owner, oldJudgmentCard, replacement, committedSuit, oldDestination, exactRiceLocation);
    internal void CompleteProgramJudgmentReplacementBinding(ProgramSkillFrame frame, bool completed) => _resolve().CompleteProgramJudgmentReplacementBinding(frame, completed);
    internal (SkillProgram Program, SkillProgramTrigger Trigger) GetProgramJudgmentReplacement(JudgmentTriggerCandidate candidate) => _resolve().GetProgramJudgmentReplacement(candidate);
    internal IReadOnlyList<Card> GetProgramJudgmentReplacementCards(CharacterState owner, SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false) => _resolve().GetProgramJudgmentReplacementCards(owner, trigger, judgmentSubjectSeat, judgmentReason, ignoreCurrentColor);
    internal bool IsAiProgramJudgmentReplacementPending() => _resolve().IsAiProgramJudgmentReplacementPending();
    internal bool MatchesProgramJudgmentReplacement(CharacterState owner, SkillProgramTrigger trigger, int targetSeat, string reason) => _resolve().MatchesProgramJudgmentReplacement(owner, trigger, targetSeat, reason);
    internal void ReplaceProgramJudgment(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ReplaceProgramJudgment(frame, effect);
    internal void ResolvePendingAiProgramJudgmentReplacement() => _resolve().ResolvePendingAiProgramJudgmentReplacement();
    internal void ResolveProgramJudgmentReplacementChoice(PromptChoice selected) => _resolve().ResolveProgramJudgmentReplacementChoice(selected);
    internal CommandResult SubmitProgramJudgmentReplacementAnswer(int actorSeat, PromptId prompt, ChoiceId choice) => _resolve().SubmitProgramJudgmentReplacementAnswer(actorSeat, prompt, choice);
}
