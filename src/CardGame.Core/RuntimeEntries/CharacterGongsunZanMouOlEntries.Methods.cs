#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterGongsunZanMouOlEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MouGongsunZanBaojingGrantedEvent> ActiveMouGszBaojing(int target) => _resolve().ActiveMouGszBaojing(target);
    internal void AddMouGszBaojingRange(global::CardGame.Core.CharacterState player, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().AddMouGszBaojingRange(player, contributions);
    internal bool AdvanceMouGszChildren() => _resolve().AdvanceMouGszChildren();
    internal void AnswerMouGsz(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMouGsz(choice);
    internal void AssertMouGsz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMouGsz(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouGszBaojing(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouGszBaojing(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouGszJiaodi(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMouGszJiaodi(supplied);
    internal bool CanOfferMouGszJiaodi(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouGszJiaodi(gate);
    internal global::CardGame.Core.PromptChoice ChooseMouGszAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMouGszAi(p, f);
    internal void ContinueMouGsz(long id) => _resolve().ContinueMouGsz(id);
    internal bool DrainMouGsz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainMouGsz(f);
    internal void FinishMouGsz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMouGsz(f);
    internal bool KeepAcceptedMouGsz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepAcceptedMouGsz(f);
    internal bool MouGszBeforeDamage(long id) => _resolve().MouGszBeforeDamage(id);
    internal global::CardGame.Core.PromptChoice MouGszChoice(global::CardGame.Core.ProgramSkillFrame f, string branch, string text, int? target = default) => _resolve().MouGszChoice(f, branch, text, target);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouGszChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGszChoices(f);
    internal bool MouGszDamage(long id) => _resolve().MouGszDamage(id);
    internal bool MouGszDying() => _resolve().MouGszDying();
    internal bool MouGszFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGszFirstChild(f, child);
    internal bool MouGszLionChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouGszLionChild(f, child);
    internal bool MouGszMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouGszMovementProof(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? MouGszObserverRoot() => _resolve().MouGszObserverRoot();
    internal bool MouGszRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().MouGszRecoveryParent(f, child);
    internal global::CardGame.Core.CardConversionSource MouGszSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGszSource(f);
    internal bool MouGszSuffix(int i) => _resolve().MouGszSuffix(i);
    internal global::CardGame.Core.CardUseFrame? MouGszUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouGszUse(f);
    internal global::CardGame.Core.RuleQueryEvaluation ProjectMouGszJiaodiRange(global::CardGame.Core.CharacterState player, global::CardGame.Core.RuleQueryEvaluation ordinary) => _resolve().ProjectMouGszJiaodiRange(player, ordinary);
    internal void PublishMouGsz(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> choices) => _resolve().PublishMouGsz(f, choices);
    internal bool ResumeMouGsz(long id) => _resolve().ResumeMouGsz(id);
    internal bool ReturnMouGszMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMouGszMovement(f);
    internal void SealMouGsz(long id, string purpose) => _resolve().SealMouGsz(id, purpose);
    internal bool ValidMouGsz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouGsz(f);
}
