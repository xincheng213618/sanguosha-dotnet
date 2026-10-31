#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangQuanOlEntries
{
    internal bool AllowsRegisteredIsDianhuDrawTrigger(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().AllowsRegisteredIsDianhuDrawTrigger(gate);
    internal global::CardGame.Core.SkillProgramStepOutcome ArmDianhuRelation(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().ArmDianhuRelation(supplied, target);
    internal void AssertHuangQuanNativeUses() => _resolve().AssertHuangQuanNativeUses();
    internal void AssertJianjiNativeUseCompletion(global::CardGame.Core.CardUseFrame use) => _resolve().AssertJianjiNativeUseCompletion(use);
    internal void BindJianjiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindJianjiNativeUse(use);
    internal bool CanContinueIssuedHuangQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedHuangQuan(f);
    internal bool CanOfferDianhuDamage(int owner, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().CanOfferDianhuDamage(owner, t, attack);
    internal bool CanRunDianhu(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanRunDianhu(c, context);
    internal bool CanUseJianjiOrdinaryAlcohol(global::CardGame.Core.CharacterState actor, int? selectedId) => _resolve().CanUseJianjiOrdinaryAlcohol(actor, selectedId);
    internal void CaptureDianhuRecoveryQualification(global::CardGame.Core.HpChangeContext change) => _resolve().CaptureDianhuRecoveryQualification(change);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectDianhuHpCandidates(global::CardGame.Core.HpChangeContext change) => _resolve().CollectDianhuHpCandidates(change);
    internal bool CompleteJianjiUseReturn(global::CardGame.Core.CardUseFrame use) => _resolve().CompleteJianjiUseReturn(use);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateDianhuDamageContext(global::CardGame.Core.DamageTriggerWindowFrame w, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateDianhuDamageContext(w, c);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateDianhuHpContext(global::CardGame.Core.HpChangeContext change, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateDianhuHpContext(change, c);
    internal bool DianhuConsumed(global::CardGame.Core.DianhuOccurrence r) => _resolve().DianhuConsumed(r);
    internal global::CardGame.Core.DianhuOccurrence? DianhuQualification(global::CardGame.Core.ProgramTriggerCandidate c, long nativeId) => _resolve().DianhuQualification(c, nativeId);
    internal bool DianhuSourceEnabled(global::CardGame.Core.DianhuRelation g) => _resolve().DianhuSourceEnabled(g);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawAndOfferJianjiExactCard(global::CardGame.Core.ProgramSkillFrame supplied, int target) => _resolve().DrawAndOfferJianjiExactCard(supplied, target);
    internal global::CardGame.Core.SkillProgramStepOutcome DrawForDianhuActualOccurrence(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().DrawForDianhuActualOccurrence(supplied);
    internal void FinishHuangQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishHuangQuan(f);
    internal bool HasHuangQuanBeforeDamageObserver(long id) => _resolve().HasHuangQuanBeforeDamageObserver(id);
    internal bool HasHuangQuanDamageObserver(long id) => _resolve().HasHuangQuanDamageObserver(id);
    internal bool HuangQuanFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().HuangQuanFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? HuangQuanObserverRoot() => _resolve().HuangQuanObserverRoot();
    internal global::CardGame.Core.CardConversionSource HuangQuanSource(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HuangQuanSource(f);
    internal bool IsDianhuDrawTrigger(global::CardGame.Core.SkillProgramTrigger t) => _resolve().IsDianhuDrawTrigger(t);
    internal bool IsHuangQuanDying() => _resolve().IsHuangQuanDying();
    internal bool IsHuangQuanMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation continuation) => _resolve().IsHuangQuanMovement(f, e, continuation);
    internal bool IsJianjiExactCardQuery(global::CardGame.Core.CharacterState actor, int? selectedId) => _resolve().IsJianjiExactCardQuery(actor, selectedId);
    internal bool IsJianjiOffTurnNativeUse(long id, int actor) => _resolve().IsJianjiOffTurnNativeUse(id, actor);
    internal global::CardGame.Core.SkillProgramStepOutcome IssueHuangQuanDraw(global::CardGame.Core.ProgramSkillFrame f, int target, string kind, global::CardGame.Core.DianhuOccurrence? occurrence) => _resolve().IssueHuangQuanDraw(f, target, kind, occurrence);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LegalAction> JianjiActions(global::CardGame.Core.ProgramSkillFrame f) => _resolve().JianjiActions(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> JianjiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().JianjiChoices(f);
    internal global::System.Collections.Generic.IReadOnlyList<int> JianjiExpectedNativeTargets(global::CardGame.Core.JianjiNativeReceipt r) => _resolve().JianjiExpectedNativeTargets(r);
    internal bool JianjiNativeUseRidesOn(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.ProgramSkillFrame parent) => _resolve().JianjiNativeUseRidesOn(use, parent);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.DianhuRelation> LiveDianhuRelations() => _resolve().LiveDianhuRelations();
    internal void ObserveDianhuDamageQualification(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveDianhuDamageQualification(payload);
    internal void ObserveJianjiAlcoholExpiry(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveJianjiAlcoholExpiry(payload);
    internal bool? ProveDianhuHealth(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveDianhuHealth(trigger, candidate, context);
    internal void PublishJianjiChoice(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishJianjiChoice(f);
    internal void RecordJianjiOrdinaryAlcoholUse(long id) => _resolve().RecordJianjiOrdinaryAlcoholUse(id);
    internal void RequireIssuedHuangQuan(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireIssuedHuangQuan(f);
    internal bool ResumeHuangQuan(long id) => _resolve().ResumeHuangQuan(id);
    internal bool ReturnHuangQuanMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnHuangQuanMovement(f);
    internal void ReturnJianjiNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnJianjiNativeUse(use);
    internal bool SameJianjiReceipt(global::CardGame.Core.JianjiNativeReceipt a, global::CardGame.Core.JianjiNativeReceipt b) => _resolve().SameJianjiReceipt(a, b);
    internal bool TryAdvanceHuangQuanSubtree() => _resolve().TryAdvanceHuangQuanSubtree();
    internal bool TryResolveJianjiChoice(global::CardGame.Core.PromptChoice choice) => _resolve().TryResolveJianjiChoice(choice);
    internal bool TryResumeJianjiAfterNativeAttack() => _resolve().TryResumeJianjiAfterNativeAttack();
    internal bool ValidHuangQuanDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidHuangQuanDraw(f);
    internal bool ValidJianjiReceipt(global::CardGame.Core.JianjiNativeReceipt r, global::CardGame.Core.ProgramSkillFrame parent, bool bound) => _resolve().ValidJianjiReceipt(r, parent, bound);
    internal bool HasDianhuRelation { get => _resolve().HasDianhuRelation; }
}
