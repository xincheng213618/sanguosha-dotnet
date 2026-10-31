#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanOlEntries
{
    internal bool AllowsRegisteredIsDianhuDrawTrigger(ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredIsDianhuDrawTrigger(gate);
    internal SkillProgramStepOutcome ArmDianhuRelation(ProgramSkillFrame supplied, int target) => _resolve().ArmDianhuRelation(supplied, target);
    internal void AssertJianjiNativeUseCompletion(CardUseFrame use) => _resolve().AssertJianjiNativeUseCompletion(use);
    internal void BindJianjiNativeUse(CardUseFrame use) => _resolve().BindJianjiNativeUse(use);
    internal bool CanContinueIssuedHuangQuan(ProgramSkillFrame f) => _resolve().CanContinueIssuedHuangQuan(f);
    internal bool CanOfferDianhuDamage(int owner, SkillProgramTrigger t, GameEngine.IDamageAttempt attack) => _resolve().CanOfferDianhuDamage(owner, t, attack);
    internal bool CanRunDianhu(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CanRunDianhu(c, context);
    internal bool CanUseJianjiOrdinaryAlcohol(CharacterState actor, int? selectedId) => _resolve().CanUseJianjiOrdinaryAlcohol(actor, selectedId);
    internal void CaptureDianhuRecoveryQualification(HpChangeContext change) => _resolve().CaptureDianhuRecoveryQualification(change);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectDianhuHpCandidates(HpChangeContext change) => _resolve().CollectDianhuHpCandidates(change);
    internal bool CompleteJianjiUseReturn(CardUseFrame use) => _resolve().CompleteJianjiUseReturn(use);
    internal ProgramSkillWindowContext CreateDianhuDamageContext(DamageTriggerWindowFrame w, ProgramTriggerCandidate c) => _resolve().CreateDianhuDamageContext(w, c);
    internal ProgramSkillWindowContext CreateDianhuHpContext(HpChangeContext change, ProgramTriggerCandidate c) => _resolve().CreateDianhuHpContext(change, c);
    internal bool DianhuConsumed(DianhuOccurrence r) => _resolve().DianhuConsumed(r);
    internal DianhuOccurrence? DianhuQualification(ProgramTriggerCandidate c, long nativeId) => _resolve().DianhuQualification(c, nativeId);
    internal bool DianhuSourceEnabled(DianhuRelation g) => _resolve().DianhuSourceEnabled(g);
    internal SkillProgramStepOutcome DrawAndOfferJianjiExactCard(ProgramSkillFrame supplied, int target) => _resolve().DrawAndOfferJianjiExactCard(supplied, target);
    internal SkillProgramStepOutcome DrawForDianhuActualOccurrence(ProgramSkillFrame supplied) => _resolve().DrawForDianhuActualOccurrence(supplied);
    internal void FinishHuangQuan(ProgramSkillFrame f) => _resolve().FinishHuangQuan(f);
    internal bool HuangQuanFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().HuangQuanFirstChild(root, child);
    internal ProgramSkillFrame? HuangQuanObserverRoot() => _resolve().HuangQuanObserverRoot();
    internal CardConversionSource HuangQuanSource(ProgramSkillFrame f) => _resolve().HuangQuanSource(f);
    internal bool IsDianhuDrawTrigger(SkillProgramTrigger t) => _resolve().IsDianhuDrawTrigger(t);
    internal bool IsJianjiExactCardQuery(CharacterState actor, int? selectedId) => _resolve().IsJianjiExactCardQuery(actor, selectedId);
    internal bool IsJianjiOffTurnNativeUse(long id, int actor) => _resolve().IsJianjiOffTurnNativeUse(id, actor);
    internal SkillProgramStepOutcome IssueHuangQuanDraw(ProgramSkillFrame f, int target, string kind, DianhuOccurrence? occurrence) => _resolve().IssueHuangQuanDraw(f, target, kind, occurrence);
    internal IReadOnlyList<LegalAction> JianjiActions(ProgramSkillFrame f) => _resolve().JianjiActions(f);
    internal IReadOnlyList<PromptChoice> JianjiChoices(ProgramSkillFrame f) => _resolve().JianjiChoices(f);
    internal IReadOnlyList<int> JianjiExpectedNativeTargets(JianjiNativeReceipt r) => _resolve().JianjiExpectedNativeTargets(r);
    internal bool JianjiNativeUseRidesOn(CardUseFrame use, ProgramSkillFrame parent) => _resolve().JianjiNativeUseRidesOn(use, parent);
    internal IEnumerable<DianhuRelation> LiveDianhuRelations() => _resolve().LiveDianhuRelations();
    internal void ObserveDianhuDamageQualification(IGameEvent payload) => _resolve().ObserveDianhuDamageQualification(payload);
    internal void ObserveJianjiAlcoholExpiry(IGameEvent payload) => _resolve().ObserveJianjiAlcoholExpiry(payload);
    internal bool? ProveDianhuHealth(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveDianhuHealth(trigger, candidate, context);
    internal void PublishJianjiChoice(ProgramSkillFrame f) => _resolve().PublishJianjiChoice(f);
    internal void RecordJianjiOrdinaryAlcoholUse(long id) => _resolve().RecordJianjiOrdinaryAlcoholUse(id);
    internal bool ResumeHuangQuan(long id) => _resolve().ResumeHuangQuan(id);
    internal bool ReturnHuangQuanMovement(ProgramSkillFrame f) => _resolve().ReturnHuangQuanMovement(f);
    internal void ReturnJianjiNativeUse(CardUseFrame use) => _resolve().ReturnJianjiNativeUse(use);
    internal bool SameJianjiReceipt(JianjiNativeReceipt a, JianjiNativeReceipt b) => _resolve().SameJianjiReceipt(a, b);
    internal bool TryAdvanceHuangQuanSubtree() => _resolve().TryAdvanceHuangQuanSubtree();
    internal bool TryResolveJianjiChoice(PromptChoice choice) => _resolve().TryResolveJianjiChoice(choice);
    internal bool TryResumeJianjiAfterNativeAttack() => _resolve().TryResumeJianjiAfterNativeAttack();
    internal bool ValidHuangQuanDraw(ProgramSkillFrame f) => _resolve().ValidHuangQuanDraw(f);
    internal bool ValidJianjiReceipt(JianjiNativeReceipt r, ProgramSkillFrame parent, bool bound) => _resolve().ValidJianjiReceipt(r, parent, bound);
    internal bool HasDianhuRelation { get => _resolve().HasDianhuRelation; }
}
