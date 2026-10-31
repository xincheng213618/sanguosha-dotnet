#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineJudgmentReplacementProgramsEntries
{
    internal void BeginProgramJudgmentReplacementChoice(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.JudgmentTriggerCandidate candidate) => _resolve().BeginProgramJudgmentReplacementChoice(pending, candidate);
    internal void CommitProgramJudgmentReplacement(global::CardGame.Core.JudgmentFrame pending, global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card oldJudgmentCard, global::CardGame.Core.Card replacement, global::CardGame.Core.Suit committedSuit, global::CardGame.Core.SkillProgramOldJudgmentCardDestination oldDestination, global::CardGame.Core.CardLocation? exactRiceLocation = default) => _resolve().CommitProgramJudgmentReplacement(pending, owner, oldJudgmentCard, replacement, committedSuit, oldDestination, exactRiceLocation);
    internal void CompleteProgramJudgmentReplacementBinding(global::CardGame.Core.ProgramSkillFrame frame, bool completed) => _resolve().CompleteProgramJudgmentReplacementBinding(frame, completed);
    internal (global::CardGame.Core.SkillProgram Program, global::CardGame.Core.SkillProgramTrigger Trigger) GetProgramJudgmentReplacement(global::CardGame.Core.JudgmentTriggerCandidate candidate) => _resolve().GetProgramJudgmentReplacement(candidate);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetProgramJudgmentReplacementCards(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int judgmentSubjectSeat, string judgmentReason, bool ignoreCurrentColor = false) => _resolve().GetProgramJudgmentReplacementCards(owner, trigger, judgmentSubjectSeat, judgmentReason, ignoreCurrentColor);
    internal bool IsAiProgramJudgmentReplacementPending() => _resolve().IsAiProgramJudgmentReplacementPending();
    internal bool MatchesProgramJudgmentReplacement(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTrigger trigger, int targetSeat, string reason) => _resolve().MatchesProgramJudgmentReplacement(owner, trigger, targetSeat, reason);
    internal void ReplaceProgramJudgment(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ReplaceProgramJudgment(frame, effect);
    internal void ResolvePendingAiProgramJudgmentReplacement() => _resolve().ResolvePendingAiProgramJudgmentReplacement();
    internal void ResolveProgramJudgmentReplacementChoice(global::CardGame.Core.PromptChoice selected) => _resolve().ResolveProgramJudgmentReplacementChoice(selected);
    internal global::CardGame.Core.CommandResult SubmitProgramJudgmentReplacementAnswer(int actorSeat, global::CardGame.Core.PromptId prompt, global::CardGame.Core.ChoiceId choice) => _resolve().SubmitProgramJudgmentReplacementAnswer(actorSeat, prompt, choice);
}
