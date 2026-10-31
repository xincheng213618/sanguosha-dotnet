#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGongsunZanMouOlEntries
{
    internal IEnumerable<MouGongsunZanBaojingGrantedEvent> ActiveMouGszBaojing(int target) => _resolve().ActiveMouGszBaojing(target);
    internal void AddMouGszBaojingRange(CharacterState player, List<RuleQueryContribution> contributions) => _resolve().AddMouGszBaojingRange(player, contributions);
    internal bool AdvanceMouGszChildren() => _resolve().AdvanceMouGszChildren();
    internal void AnswerMouGsz(PromptChoice choice) => _resolve().AnswerMouGsz(choice);
    internal SkillProgramStepOutcome BeginMouGszBaojing(ProgramSkillFrame supplied) => _resolve().BeginMouGszBaojing(supplied);
    internal SkillProgramStepOutcome BeginMouGszJiaodi(ProgramSkillFrame supplied) => _resolve().BeginMouGszJiaodi(supplied);
    internal bool CanOfferMouGszJiaodi(ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouGszJiaodi(gate);
    internal PromptChoice ChooseMouGszAi(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMouGszAi(p, f);
    internal void ContinueMouGsz(long id) => _resolve().ContinueMouGsz(id);
    internal bool DrainMouGsz(ProgramSkillFrame f) => _resolve().DrainMouGsz(f);
    internal void FinishMouGsz(ProgramSkillFrame f) => _resolve().FinishMouGsz(f);
    internal bool KeepAcceptedMouGsz(ProgramSkillFrame f) => _resolve().KeepAcceptedMouGsz(f);
    internal PromptChoice MouGszChoice(ProgramSkillFrame f, string branch, string text, int? target = default) => _resolve().MouGszChoice(f, branch, text, target);
    internal IReadOnlyList<PromptChoice> MouGszChoices(ProgramSkillFrame f) => _resolve().MouGszChoices(f);
    internal bool MouGszDying() => _resolve().MouGszDying();
    internal bool MouGszFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGszFirstChild(f, child);
    internal bool MouGszLionChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouGszLionChild(f, child);
    internal ProgramSkillFrame? MouGszObserverRoot() => _resolve().MouGszObserverRoot();
    internal bool MouGszRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => _resolve().MouGszRecoveryParent(f, child);
    internal CardConversionSource MouGszSource(ProgramSkillFrame f) => _resolve().MouGszSource(f);
    internal bool MouGszSuffix(int i) => _resolve().MouGszSuffix(i);
    internal CardUseFrame? MouGszUse(ProgramSkillFrame f) => _resolve().MouGszUse(f);
    internal RuleQueryEvaluation ProjectMouGszJiaodiRange(CharacterState player, RuleQueryEvaluation ordinary) => _resolve().ProjectMouGszJiaodiRange(player, ordinary);
    internal void PublishMouGsz(ProgramSkillFrame f, IReadOnlyList<PromptChoice> choices) => _resolve().PublishMouGsz(f, choices);
    internal bool ResumeMouGsz(long id) => _resolve().ResumeMouGsz(id);
    internal bool ReturnMouGszMovement(ProgramSkillFrame f) => _resolve().ReturnMouGszMovement(f);
    internal void SealMouGsz(long id, string purpose) => _resolve().SealMouGsz(id, purpose);
    internal bool ValidMouGsz(ProgramSkillFrame f) => _resolve().ValidMouGsz(f);
}
