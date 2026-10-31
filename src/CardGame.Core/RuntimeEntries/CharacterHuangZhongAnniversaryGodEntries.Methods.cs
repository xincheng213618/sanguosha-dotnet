#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangZhongAnniversaryGodEntries
{
    internal IEnumerable<AnniversaryHzBodyIssuedEvent> ActiveAnniversaryHzBody(int target, AnniversaryHzBody body) => _resolve().ActiveAnniversaryHzBody(target, body);
    internal AnniversaryHzZhanjueIssuedEvent? ActiveAnniversaryHzZhanjue(int actor, int mode) => _resolve().ActiveAnniversaryHzZhanjue(actor, mode);
    internal bool AdvanceAnniversaryHzTianchongChildren() => _resolve().AdvanceAnniversaryHzTianchongChildren();
    internal bool AnniversaryHzAlreadyHit(int owner, int target) => _resolve().AnniversaryHzAlreadyHit(owner, target);
    internal PromptChoice AnniversaryHzChoice(ProgramSkillFrame f, string option, string label) => _resolve().AnniversaryHzChoice(f, option, label);
    internal IReadOnlyList<PromptChoice> AnniversaryHzChoices(ProgramSkillFrame f) => _resolve().AnniversaryHzChoices(f);
    internal bool AnniversaryHzCounterspellInvalidated(NullificationWindowFrame pending, CardActionContext action) => _resolve().AnniversaryHzCounterspellInvalidated(pending, action);
    internal int AnniversaryHzDijiDamage(GameEngine.IDamageAttempt a) => _resolve().AnniversaryHzDijiDamage(a);
    internal bool AnniversaryHzHeartForbidden(int actor, IEnumerable<int> ids) => _resolve().AnniversaryHzHeartForbidden(actor, ids);
    internal bool AnniversaryHzMaterialBan(ProgramCardMaterialQuery q) => _resolve().AnniversaryHzMaterialBan(q);
    internal bool AnniversaryHzProjectedBan(ProgramLegalActionQuery q) => _resolve().AnniversaryHzProjectedBan(q);
    internal AnniversaryHzZhanjueSpentEvent? AnniversaryHzRecoveryReservation(int actor, long? damageId) => _resolve().AnniversaryHzRecoveryReservation(actor, damageId);
    internal bool AnniversaryHzResponseBan(ProgramNativeResponseQuery q) => _resolve().AnniversaryHzResponseBan(q);
    internal bool AnniversaryHzSlashDistance(ProgramSlashDistanceQuery q) => _resolve().AnniversaryHzSlashDistance(q);
    internal void AnniversaryHzSubmittedBan(ProgramSubmittedUseQuery q) => _resolve().AnniversaryHzSubmittedBan(q);
    internal bool AnniversaryHzTianchongHealthChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().AnniversaryHzTianchongHealthChild(root, child);
    internal ProgramSkillFrame? AnniversaryHzTianchongObserverRoot() => _resolve().AnniversaryHzTianchongObserverRoot();
    internal bool AnniversaryHzTianchongSuffix(int rootIndex) => _resolve().AnniversaryHzTianchongSuffix(rootIndex);
    internal bool AnniversaryHzUseDistance(ProgramCardDistanceQuery q) => _resolve().AnniversaryHzUseDistance(q);
    internal void AnswerAnniversaryHz(PromptChoice choice) => _resolve().AnswerAnniversaryHz(choice);
    internal SkillProgramStepOutcome BeginAnniversaryHz(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginAnniversaryHz(supplied, op);
    internal bool CanTriggerAnniversaryHz(ProgramTriggerLegalityContext g) => _resolve().CanTriggerAnniversaryHz(g);
    internal PromptChoice ChooseAnniversaryHzBody(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversaryHzBody(p, f);
    internal PromptChoice ChooseAnniversaryHzDraw(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseAnniversaryHzDraw(p, f);
    internal void ContinueAnniversaryHz(long id) => _resolve().ContinueAnniversaryHz(id);
    internal bool ContinueAnniversaryHzIneffectiveDodge(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().ContinueAnniversaryHzIneffectiveDodge(attack, action, continuation);
    internal bool ContinueAnniversaryHzIneffectiveSimple(long id, ProgramSimpleCardContinuation continuation) => _resolve().ContinueAnniversaryHzIneffectiveSimple(id, continuation);
    internal bool ContinueIssuedAnniversaryHz(ProgramSkillFrame f) => _resolve().ContinueIssuedAnniversaryHz(f);
    internal bool DrainAnniversaryHz(long id) => _resolve().DrainAnniversaryHz(id);
    internal bool HasAnniversaryHzIneffectiveResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation) => _resolve().HasAnniversaryHzIneffectiveResponse(attack, action, continuation);
    internal bool HasAnniversaryHzPendingInvalidation(int actor) => _resolve().HasAnniversaryHzPendingInvalidation(actor);
    internal bool IsAnniversaryHzTianchongDying() => _resolve().IsAnniversaryHzTianchongDying();
    internal bool IsAnniversaryHzUseIneffective(CardUseFrame use) => _resolve().IsAnniversaryHzUseIneffective(use);
    internal void IssueAnniversaryHzActualUse(CardActionContext a, long parent) => _resolve().IssueAnniversaryHzActualUse(a, parent);
    internal void IssueAnniversaryHzLegacyUse(CardUseFrame use) => _resolve().IssueAnniversaryHzLegacyUse(use);
    internal void ObserveAnniversaryHzActualUse(IGameEvent payload) => _resolve().ObserveAnniversaryHzActualUse(payload);
    internal bool ObserveAnniversaryHzApplied(GameEngine.IDamageAttempt a, long damageId) => _resolve().ObserveAnniversaryHzApplied(a, damageId);
    internal void ObserveAnniversaryHzExpiryAndSuit(IGameEvent payload) => _resolve().ObserveAnniversaryHzExpiryAndSuit(payload);
    internal void PublishAnniversaryHz(ProgramSkillFrame f) => _resolve().PublishAnniversaryHz(f);
    internal void RecoverAnniversaryHz(ProgramSkillFrame f, int amount) => _resolve().RecoverAnniversaryHz(f, amount);
    internal bool ResumeAnniversaryHz(long id) => _resolve().ResumeAnniversaryHz(id);
    internal void ReturnAnniversaryHzDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnAnniversaryHzDraw(supplied, outcome);
    internal bool ReturnAnniversaryHzMovement(ProgramSkillFrame f) => _resolve().ReturnAnniversaryHzMovement(f);
    internal bool TryAnniversaryHzEquipmentCommitted(long id, Card equipment) => _resolve().TryAnniversaryHzEquipmentCommitted(id, equipment);
    internal bool ValidAnniversaryHzInvalidation(AnniversaryHzInvalidation proof, CardActionContext? action, long parent) => _resolve().ValidAnniversaryHzInvalidation(proof, action, parent);
    internal bool HasAnniversaryHuangZhongCapability { get => _resolve().HasAnniversaryHuangZhongCapability; }
}
