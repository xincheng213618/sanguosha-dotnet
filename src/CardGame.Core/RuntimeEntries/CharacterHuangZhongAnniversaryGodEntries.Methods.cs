#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangZhongAnniversaryGodEntries
{
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.AnniversaryHzBodyIssuedEvent> ActiveAnniversaryHzBody(int target, global::CardGame.Core.AnniversaryHzBody body) => _resolve().ActiveAnniversaryHzBody(target, body);
    internal global::CardGame.Core.AnniversaryHzZhanjueIssuedEvent? ActiveAnniversaryHzZhanjue(int actor, int mode) => _resolve().ActiveAnniversaryHzZhanjue(actor, mode);
    internal bool AdvanceAnniversaryHzTianchongChildren() => _resolve().AdvanceAnniversaryHzTianchongChildren();
    internal bool AnniversaryHzAlreadyHit(int owner, int target) => _resolve().AnniversaryHzAlreadyHit(owner, target);
    internal global::CardGame.Core.PromptChoice AnniversaryHzChoice(global::CardGame.Core.ProgramSkillFrame f, string option, string label) => _resolve().AnniversaryHzChoice(f, option, label);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> AnniversaryHzChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AnniversaryHzChoices(f);
    internal bool AnniversaryHzCounterspellInvalidated(global::CardGame.Core.NullificationWindowFrame pending, global::CardGame.Core.CardActionContext action) => _resolve().AnniversaryHzCounterspellInvalidated(pending, action);
    internal int AnniversaryHzDijiDamage(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().AnniversaryHzDijiDamage(a);
    internal bool AnniversaryHzHeartForbidden(int actor, global::System.Collections.Generic.IEnumerable<int> ids) => _resolve().AnniversaryHzHeartForbidden(actor, ids);
    internal bool AnniversaryHzMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().AnniversaryHzMaterialBan(q);
    internal bool AnniversaryHzProjectedBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().AnniversaryHzProjectedBan(q);
    internal global::CardGame.Core.AnniversaryHzZhanjueSpentEvent? AnniversaryHzRecoveryReservation(int actor, long? damageId) => _resolve().AnniversaryHzRecoveryReservation(actor, damageId);
    internal bool AnniversaryHzResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().AnniversaryHzResponseBan(q);
    internal bool AnniversaryHzSlashDistance(global::CardGame.Core.ProgramSlashDistanceQuery q) => _resolve().AnniversaryHzSlashDistance(q);
    internal void AnniversaryHzSubmittedBan(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().AnniversaryHzSubmittedBan(q);
    internal bool AnniversaryHzTianchongHealthChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().AnniversaryHzTianchongHealthChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? AnniversaryHzTianchongObserverRoot() => _resolve().AnniversaryHzTianchongObserverRoot();
    internal bool AnniversaryHzTianchongSuffix(int rootIndex) => _resolve().AnniversaryHzTianchongSuffix(rootIndex);
    internal bool AnniversaryHzUseDistance(global::CardGame.Core.ProgramCardDistanceQuery q) => _resolve().AnniversaryHzUseDistance(q);
    internal void AnswerAnniversaryHz(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerAnniversaryHz(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginAnniversaryHz(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginAnniversaryHz(supplied, op);
    internal bool CanTriggerAnniversaryHz(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanTriggerAnniversaryHz(g);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryHzBody(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversaryHzBody(p, f);
    internal global::CardGame.Core.PromptChoice ChooseAnniversaryHzDraw(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseAnniversaryHzDraw(p, f);
    internal void ContinueAnniversaryHz(long id) => _resolve().ContinueAnniversaryHz(id);
    internal bool ContinueAnniversaryHzIneffectiveDodge(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().ContinueAnniversaryHzIneffectiveDodge(attack, action, continuation);
    internal bool ContinueAnniversaryHzIneffectiveSimple(long id, global::CardGame.Core.ProgramSimpleCardContinuation continuation) => _resolve().ContinueAnniversaryHzIneffectiveSimple(id, continuation);
    internal bool ContinueIssuedAnniversaryHz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedAnniversaryHz(f);
    internal bool DrainAnniversaryHz(long id) => _resolve().DrainAnniversaryHz(id);
    internal bool HasAnniversaryHzIneffectiveResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasAnniversaryHzIneffectiveResponse(attack, action, continuation);
    internal bool HasAnniversaryHzPendingInvalidation(int actor) => _resolve().HasAnniversaryHzPendingInvalidation(actor);
    internal bool IsAnniversaryHzTianchongDying() => _resolve().IsAnniversaryHzTianchongDying();
    internal bool IsAnniversaryHzUseIneffective(global::CardGame.Core.CardUseFrame use) => _resolve().IsAnniversaryHzUseIneffective(use);
    internal void IssueAnniversaryHzActualUse(global::CardGame.Core.CardActionContext a, long parent) => _resolve().IssueAnniversaryHzActualUse(a, parent);
    internal void IssueAnniversaryHzLegacyUse(global::CardGame.Core.CardUseFrame use) => _resolve().IssueAnniversaryHzLegacyUse(use);
    internal void ObserveAnniversaryHzActualUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveAnniversaryHzActualUse(payload);
    internal bool ObserveAnniversaryHzApplied(global::CardGame.Core.GameEngine.IDamageAttempt a, long damageId) => _resolve().ObserveAnniversaryHzApplied(a, damageId);
    internal void ObserveAnniversaryHzExpiryAndSuit(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveAnniversaryHzExpiryAndSuit(payload);
    internal void PublishAnniversaryHz(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishAnniversaryHz(f);
    internal void RecoverAnniversaryHz(global::CardGame.Core.ProgramSkillFrame f, int amount) => _resolve().RecoverAnniversaryHz(f, amount);
    internal bool ResumeAnniversaryHz(long id) => _resolve().ResumeAnniversaryHz(id);
    internal void ReturnAnniversaryHzDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnAnniversaryHzDraw(supplied, outcome);
    internal bool ReturnAnniversaryHzMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnAnniversaryHzMovement(f);
    internal bool TryAnniversaryHzEquipmentCommitted(long id, global::CardGame.Core.Card equipment) => _resolve().TryAnniversaryHzEquipmentCommitted(id, equipment);
    internal bool ValidAnniversaryHzInvalidation(global::CardGame.Core.AnniversaryHzInvalidation proof, global::CardGame.Core.CardActionContext? action, long parent) => _resolve().ValidAnniversaryHzInvalidation(proof, action, parent);
    internal bool HasAnniversaryHuangZhongCapability { get => _resolve().HasAnniversaryHuangZhongCapability; }
}
