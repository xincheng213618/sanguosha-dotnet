#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiFengOlEntries
{
    internal bool AdvanceLiFengChildren() => _resolve().AdvanceLiFengChildren();
    internal void AnswerLiFeng(global::CardGame.Core.PromptChoice selected) => _resolve().AnswerLiFeng(selected);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLiFeng(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginLiFeng(supplied, op);
    internal bool CanContinueIssuedLiFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedLiFeng(f);
    internal bool CanOfferLiFeng(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferLiFeng(candidate, trigger, context);
    internal void CaptureJiyunMovementBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureJiyunMovementBatch(batch);
    internal global::CardGame.Core.PromptChoice ChooseLiFengAi(global::CardGame.Core.PendingDecision prompt, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseLiFengAi(prompt, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramCardTriggerCandidate> CollectJiyunCompletedUseCandidates(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation? continuation) => _resolve().CollectJiyunCompletedUseCandidates(action, continuation);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureJiyunMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureJiyunMovementBatch(batch, completed);
    internal void ContinueLiFeng(long id) => _resolve().ContinueLiFeng(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateJiyunCompletedUseContext(global::CardGame.Core.ProgramCardTriggerWindowFrame mother, global::CardGame.Core.ProgramCardTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext ordinary) => _resolve().CreateJiyunCompletedUseContext(mother, candidate, ordinary);
    internal bool DrainLiFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainLiFeng(f);
    internal void FinishLiFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishLiFeng(f);
    internal bool HasJiyunCardMark(int ownerSeat, int cardId) => _resolve().HasJiyunCardMark(ownerSeat, cardId);
    internal bool HasJiyunResponseCompletion(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().HasJiyunResponseCompletion(action, continuation);
    internal bool HasLiFengBeforeDamageObserver(long id) => _resolve().HasLiFengBeforeDamageObserver(id);
    internal bool HasLiFengDamageObserver(long id) => _resolve().HasLiFengDamageObserver(id);
    internal bool HasLiveJiyunHandLease(global::CardGame.Core.JiyunLeaseIssuedEvent l) => _resolve().HasLiveJiyunHandLease(l);
    internal bool IsIssuedJiyunCompletedUseCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedJiyunCompletedUseCandidate(candidate, context);
    internal bool IsIssuedJiyunRetainedEntity(global::CardGame.Core.PublicPersistentPileSource pile, int cardId) => _resolve().IsIssuedJiyunRetainedEntity(pile, cardId);
    internal bool IsJiyunPile(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().IsJiyunPile(p);
    internal bool IsLiFengCollectedEquipment(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardKind kind) => _resolve().IsLiFengCollectedEquipment(f, kind);
    internal bool IsLiFengMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsLiFengMovement(f, effect, p);
    internal bool IsLiFengPaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsLiFengPaidMovement(f, m);
    internal bool IsLiFengRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLiFengRecoveryParent(f, recovery);
    internal bool IsLiveJiyunPile(global::CardGame.Core.PublicPersistentPileSource p) => _resolve().IsLiveJiyunPile(p);
    internal bool IsShuliangGrainQuotaSpent(int providerSeat, int actualTurn, int turnOwnerSeat) => _resolve().IsShuliangGrainQuotaSpent(providerSeat, actualTurn, turnOwnerSeat);
    internal bool IsShuliangOriginalGrainChoice(global::CardGame.Core.NativePublicPileCardRequest request, global::CardGame.Core.NativePublicPileCardSelection selection) => _resolve().IsShuliangOriginalGrainChoice(request, selection);
    internal global::CardGame.Core.CardMovementRecord? JiyunActualPayment(global::CardGame.Core.JiyunLeaseIssuedEvent lease, global::CardGame.Core.CardActionContext action, long nativeId, global::CardGame.Core.ProgramCompletedResponseKind? responseKind) => _resolve().JiyunActualPayment(lease, action, nativeId, responseKind);
    internal global::CardGame.Core.JiyunCompletedEntity[] JiyunCompletedEntities(global::CardGame.Core.JiyunCompletedUseContext r) => _resolve().JiyunCompletedEntities(r);
    internal (long NativeId, global::CardGame.Core.ProgramCompletedResponseKind? ResponseKind)? JiyunCompletedOrigin(global::CardGame.Core.CardActionContext action, global::CardGame.Core.ProgramCardContinuation? continuation) => _resolve().JiyunCompletedOrigin(action, continuation);
    internal global::CardGame.Core.JiyunLeaseDepartedEvent? JiyunDeparture(long id) => _resolve().JiyunDeparture(id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.JiyunLeaseIssuedEvent> JiyunLeases() => _resolve().JiyunLeases();
    internal bool JiyunTerminalEntity(global::CardGame.Core.JiyunLeaseIssuedEvent lease, global::CardGame.Core.CardActionContext action, long nativeId, global::CardGame.Core.ProgramCompletedResponseKind? responseKind) => _resolve().JiyunTerminalEntity(lease, action, nativeId, responseKind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> LiFengChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().LiFengChoices(f);
    internal bool LiFengFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().LiFengFirstChild(f, child);
    internal global::CardGame.Core.ProgramSkillFrame? LiFengObserverRoot() => _resolve().LiFengObserverRoot();
    internal bool LiFengObserverSuffix(int index) => _resolve().LiFengObserverSuffix(index);
    internal bool PermitIssuedLiFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitIssuedLiFeng(f);
    internal void ProjectJiyunCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectJiyunCardSkillMark(cardId, ref projection);
    internal void PublishLiFeng(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishLiFeng(f);
    internal void RecordShuliangGrainPayment(global::CardGame.Core.NativePublicPileCardPayment payment) => _resolve().RecordShuliangGrainPayment(payment);
    internal bool ResumeLiFeng(long id) => _resolve().ResumeLiFeng(id);
    internal bool RetainIssuedJiyunPile(global::CardGame.Core.PublicPersistentPileSource pile) => _resolve().RetainIssuedJiyunPile(pile);
    internal void ReturnJiyunPreparationDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnJiyunPreparationDraw(f, outcome);
    internal bool ReturnLiFengMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnLiFengMovement(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.NativePublicPileCardSelection> ShuliangOriginalGrainChoices(global::CardGame.Core.NativePublicPileCardRequest request) => _resolve().ShuliangOriginalGrainChoices(request);
    internal bool ValidJiyunClaim(global::CardGame.Core.JiyunClaimedEntityEvent e) => _resolve().ValidJiyunClaim(e);
    internal bool HasLiFengCapability { get => _resolve().HasLiFengCapability; }
}
