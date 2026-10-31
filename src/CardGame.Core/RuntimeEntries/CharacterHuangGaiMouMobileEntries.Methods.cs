#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangGaiMouMobileEntries
{
    internal bool AdvanceMouKurouObserverChildren() => _resolve().AdvanceMouKurouObserverChildren();
    internal void AnswerMouKurouGift(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerMouKurouGift(selected);
    internal void AssertMouZhaJiangAdmissions() => _resolve().AssertMouZhaJiangAdmissions();
    internal bool AwaitMouKurouChildren(long id) => _resolve().AwaitMouKurouChildren(id);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMouKurouGift(global::CardGame.Core.ProgramSkillFrame input) => _resolve().BeginMouKurouGift(input);
    internal bool CanOfferMouKurouGift(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferMouKurouGift(gate);
    internal void CaptureMouKurouNativeLoss(global::CardGame.Core.HpChangeContext change) => _resolve().CaptureMouKurouNativeLoss(change);
    internal global::CardGame.Core.MouZhaJiangAdmission CaptureMouZhaJiangAdmission(global::CardGame.Core.CharacterState actor) => _resolve().CaptureMouZhaJiangAdmission(actor);
    internal global::CardGame.Core.DeclaredCardPayment CaptureMouZhaJiangDeclaration(global::CardGame.Core.DeclaredCardPayment payment, global::CardGame.Core.CardDeclarationParentReturn parent) => _resolve().CaptureMouZhaJiangDeclaration(payment, parent);
    internal void CompleteMouKurou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteMouKurou(frame);
    internal void ContinueMouKurou(long id) => _resolve().ContinueMouKurou(id);
    internal (global::CardGame.Core.ProgramAiEstimate Estimate, int? TargetSeat) EstimateMouKurouGiftForAi(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext publicContext) => _resolve().EstimateMouKurouGiftForAi(owner, publicContext);
    internal bool ExactMouKurouArmorFrame(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactMouKurouArmorFrame(frame);
    internal bool ExactMouKurouGiftParent(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ExactMouKurouGiftParent(frame);
    internal void GrantMouKurouLossArmor(global::CardGame.Core.ProgramSkillFrame input) => _resolve().GrantMouKurouLossArmor(input);
    internal bool HasIssuedMouZhaJiangUse(long useId, int actorSeat) => _resolve().HasIssuedMouZhaJiangUse(useId, actorSeat);
    internal bool HasMouKurouBeforeDamageObserver(long id) => _resolve().HasMouKurouBeforeDamageObserver(id);
    internal bool HasMouKurouDamageObserver(long id) => _resolve().HasMouKurouDamageObserver(id);
    internal bool HasMouZhaJiangPreUsePrivilege(global::CardGame.Core.CharacterState actor) => _resolve().HasMouZhaJiangPreUsePrivilege(actor);
    internal bool HasMouZhaJiangUseDistance(global::CardGame.Core.CharacterState actor, long? useId = default) => _resolve().HasMouZhaJiangUseDistance(actor, useId);
    internal bool IsMouKurouDying() => _resolve().IsMouKurouDying();
    internal bool IsMouKurouMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsMouKurouMovement(frame, effect, pending);
    internal bool IsMouKurouRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsMouKurouRecoveryParent(frame, recovery);
    internal bool IsMouZhaJiangUnrespondable(long useId) => _resolve().IsMouZhaJiangUnrespondable(useId);
    internal void IssueMouZhaJiangLegacyDuel(global::CardGame.Core.ProgramSkillFrame owner, int actorSeat) => _resolve().IssueMouZhaJiangLegacyDuel(owner, actorSeat);
    internal void IssueMouZhaJiangLegacyDuelPipeline(global::CardGame.Core.ProgramSkillFrame frame, int sourceSeat, int targetSeat) => _resolve().IssueMouZhaJiangLegacyDuelPipeline(frame, sourceSeat, targetSeat);
    internal void IssueMouZhaJiangUse(global::CardGame.Core.CardUseFrame use) => _resolve().IssueMouZhaJiangUse(use);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MouKurouChoices(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().MouKurouChoices(frame);
    internal bool MouKurouFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().MouKurouFirstChild(frame, child);
    internal global::CardGame.Core.CardKind MouKurouGiftKind(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from) => _resolve().MouKurouGiftKind(owner, card, from);
    internal global::CardGame.Core.ProgramSkillFrame? MouKurouObserverRoot() => _resolve().MouKurouObserverRoot();
    internal bool MouKurouObserverSuffix(int index) => _resolve().MouKurouObserverSuffix(index);
    internal bool MouKurouRecoveryIsPubliclyCertain(global::CardGame.Core.GameSnapshot snapshot, int ownerSeat) => _resolve().MouKurouRecoveryIsPubliclyCertain(snapshot, ownerSeat);
    internal global::CardGame.Core.CardConversionSource? MouZhaJiangCounterspellSource(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().MouZhaJiangCounterspellSource(actor, ids);
    internal int MouZhaJiangPendingDeclarations(int actor, global::CardGame.Core.CurrentTurnCardUseKindsRecordedEvent[] facts) => _resolve().MouZhaJiangPendingDeclarations(actor, facts);
    internal int MouZhaJiangProspectiveOrdinal(int actor) => _resolve().MouZhaJiangProspectiveOrdinal(actor);
    internal global::CardGame.Core.ResolutionFrame? MouZhaJiangResponseOwner(int actor, global::CardGame.Core.CardKind kind) => _resolve().MouZhaJiangResponseOwner(actor, kind);
    internal bool MouZhaJiangSlashQuotaBeforeLimit(global::CardGame.Core.ProgramSlashQuotaQuery query) => _resolve().MouZhaJiangSlashQuotaBeforeLimit(query);
    internal void ObserveMouZhaJiangAdmission(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouZhaJiangAdmission(payload);
    internal bool PermitPaidMouKurou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().PermitPaidMouKurou(frame);
    internal void PrepareMouZhaJiangResponse(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardKind kind) => _resolve().PrepareMouZhaJiangResponse(actor, ids, kind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GameEngine.MouKurouAiCandidate> RankMouKurouAiCandidates(global::CardGame.Core.CharacterState owner, global::CardGame.Core.ProgramAiPublicContext publicContext, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice>? publishedChoices = default) => _resolve().RankMouKurouAiCandidates(owner, publicContext, publishedChoices);
    internal void RecordMouZhaJiangAwareAlcoholDebit(long useId, global::CardGame.Core.CharacterState actor) => _resolve().RecordMouZhaJiangAwareAlcoholDebit(useId, actor);
    internal bool ResumeMouKurou(long id) => _resolve().ResumeMouKurou(id);
    internal bool ReturnMouKurouMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnMouKurouMovement(frame);
    internal global::CardGame.Core.PromptChoice SelectAiMouKurouGift(global::CardGame.Core.PendingDecision decision, global::CardGame.Core.ProgramSkillFrame frame) => _resolve().SelectAiMouKurouGift(decision, frame);
    internal bool ValidMouKurou(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidMouKurou(frame);
    internal bool ValidMouZhaJiangAdmission(global::CardGame.Core.MouZhaJiangAdmission q) => _resolve().ValidMouZhaJiangAdmission(q);
    internal bool ValidMouZhaJiangDeclaration(global::CardGame.Core.DeclaredCardPayment p, global::CardGame.Core.MouZhaJiangAdmission q) => _resolve().ValidMouZhaJiangDeclaration(p, q);
    internal bool HasMouZhaJiangCapability { get => _resolve().HasMouZhaJiangCapability; }
}
