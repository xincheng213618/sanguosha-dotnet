#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHanSongAnniversaryEntries
{
    internal IEnumerable<RuleQueryContribution> AnniversaryHanSongNumericContributions(CharacterState owner, SkillRuleQuery query) => _resolve().AnniversaryHanSongNumericContributions(owner, query);
    internal void AnswerHanSong(PromptChoice choice) => _resolve().AnswerHanSong(choice);
    internal IReadOnlyList<ProgramTriggerCandidate> AppendAnniversaryHanSongDrawEndedCandidates(CharacterState phaseOwner, IReadOnlyList<ProgramTriggerCandidate> existing) => _resolve().AppendAnniversaryHanSongDrawEndedCandidates(phaseOwner, existing);
    internal RuleQueryEvaluation ApplyAnniversaryHanSongHandLimit(CharacterState owner, RuleQueryEvaluation ordinary) => _resolve().ApplyAnniversaryHanSongHandLimit(owner, ordinary);
    internal SkillProgramStepOutcome BeginAnniversaryHanSong(ProgramSkillFrame supplied) => _resolve().BeginAnniversaryHanSong(supplied);
    internal bool CanOfferAnniversaryHanSong(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext c) => _resolve().CanOfferAnniversaryHanSong(candidate, trigger, c);
    internal void ContinueHanSong(long id) => _resolve().ContinueHanSong(id);
    internal bool ContinuePaidHanSong(ProgramSkillFrame f) => _resolve().ContinuePaidHanSong(f);
    internal bool ExactHanSongParent(ProgramSkillFrame f) => _resolve().ExactHanSongParent(f);
    internal void FinishHanSong(ProgramSkillFrame f) => _resolve().FinishHanSong(f);
    internal PromptChoice HanSongAi(PendingDecision p) => _resolve().HanSongAi(p);
    internal bool HanSongCardDistance(ProgramCardDistanceQuery q) => _resolve().HanSongCardDistance(q);
    internal bool HanSongCardDistanceLimit(CharacterState owner) => _resolve().HanSongCardDistanceLimit(owner);
    internal PromptChoice HanSongChoice(ProgramSkillFrame f, string verb, string suffix, string label, IReadOnlyList<int>? cards = default, IReadOnlyDictionary<string, string>? extra = default) => _resolve().HanSongChoice(f, verb, suffix, label, cards, extra);
    internal IReadOnlyList<PromptChoice> HanSongChoices(ProgramSkillFrame f) => _resolve().HanSongChoices(f);
    internal IEnumerable<(Card Card, CardLocation From, int Slot, bool Hidden)> HanSongDiscardOptions(ProgramSkillFrame f) => _resolve().HanSongDiscardOptions(f);
    internal bool HanSongSlashDistance(ProgramSlashDistanceQuery q) => _resolve().HanSongSlashDistance(q);
    internal bool HasAnniversaryHanSongUniqueHand(CharacterState owner) => _resolve().HasAnniversaryHanSongUniqueHand(owner);
    internal bool IsAnniversaryHanSongForeignDrawContext(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c) => _resolve().IsAnniversaryHanSongForeignDrawContext(candidate, c);
    internal void IssueHanSongDraw(ProgramSkillFrame f) => _resolve().IssueHanSongDraw(f);
    internal void PublishHanSong(ProgramSkillFrame f) => _resolve().PublishHanSong(f);
    internal bool ResumeHanSong(long id) => _resolve().ResumeHanSong(id);
    internal void ReturnHanSongDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnHanSongDraw(supplied, outcome);
    internal bool ReturnHanSongMovement(ProgramSkillFrame f) => _resolve().ReturnHanSongMovement(f);
    internal bool ValidHanSong(ProgramSkillFrame f) => _resolve().ValidHanSong(f);
}
