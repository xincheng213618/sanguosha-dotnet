#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanSongAnniversaryEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.RuleQueryContribution> AnniversaryHanSongNumericContributions(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillRuleQuery query) => _resolve().AnniversaryHanSongNumericContributions(owner, query);
    internal void AnswerHanSong(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerHanSong(choice);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> AppendAnniversaryHanSongDrawEndedCandidates(global::CardGame.Core.CharacterState phaseOwner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => _resolve().AppendAnniversaryHanSongDrawEndedCandidates(phaseOwner, existing);
    internal global::CardGame.Core.RuleQueryEvaluation ApplyAnniversaryHanSongHandLimit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().ApplyAnniversaryHanSongHandLimit(owner, ordinary);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryHanSong(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginAnniversaryHanSong(supplied);
    internal bool CanOfferAnniversaryHanSong(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().CanOfferAnniversaryHanSong(candidate, trigger, c);
    internal void ContinueHanSong(long id) => _resolve().ContinueHanSong(id);
    internal bool ContinuePaidHanSong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidHanSong(f);
    internal bool ExactHanSongParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ExactHanSongParent(f);
    internal void FinishHanSong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHanSong(f);
    internal global::CardGame.Core.PromptChoice HanSongAi(global::CardGame.Core.PendingDecision p) => _resolve().HanSongAi(p);
    internal bool HanSongCardDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().HanSongCardDistance(q);
    internal bool HanSongCardDistanceLimit(global::CardGame.Core.CharacterState owner) => _resolve().HanSongCardDistanceLimit(owner);
    internal global::CardGame.Core.PromptChoice HanSongChoice(global::CardGame.Core.ProgramSkillFrame f, string verb, string suffix, string label, global::System.Collections.Generic.IReadOnlyList<int>? cards = default, global::System.Collections.Generic.IReadOnlyDictionary<string, string>? extra = default) => _resolve().HanSongChoice(f, verb, suffix, label, cards, extra);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> HanSongChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HanSongChoices(f);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.Card Card, global::CardGame.Core.CardLocation From, int Slot, bool Hidden)> HanSongDiscardOptions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HanSongDiscardOptions(f);
    internal bool HanSongMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().HanSongMovement(f, e, p);
    internal bool HanSongSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().HanSongSlashDistance(q);
    internal bool HasAnniversaryHanSongUniqueHand(global::CardGame.Core.CharacterState owner) => _resolve().HasAnniversaryHanSongUniqueHand(owner);
    internal bool IsAnniversaryHanSongForeignDrawContext(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c) => _resolve().IsAnniversaryHanSongForeignDrawContext(candidate, c);
    internal void IssueHanSongDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueHanSongDraw(f);
    internal void PublishHanSong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishHanSong(f);
    internal bool ResumeHanSong(long id) => _resolve().ResumeHanSong(id);
    internal void ReturnHanSongDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnHanSongDraw(supplied, outcome);
    internal bool ReturnHanSongMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHanSongMovement(f);
    internal bool ValidHanSong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHanSong(f);
}
