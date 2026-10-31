#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLiFengOlEntries
{
    internal bool AdvanceLiFengChildren() => _resolve().AdvanceLiFengChildren();
    internal void AnswerLiFeng(PromptChoice selected) => _resolve().AnswerLiFeng(selected);
    internal SkillProgramStepOutcome BeginLiFeng(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginLiFeng(supplied, op);
    internal bool CanContinueIssuedLiFeng(ProgramSkillFrame f) => _resolve().CanContinueIssuedLiFeng(f);
    internal bool CanOfferLiFeng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferLiFeng(candidate, trigger, context);
    internal void CaptureJiyunMovementBatch(CardMovementBatchContext batch) => _resolve().CaptureJiyunMovementBatch(batch);
    internal PromptChoice ChooseLiFengAi(PendingDecision prompt, ProgramSkillFrame f) => _resolve().ChooseLiFengAi(prompt, f);
    internal IReadOnlyList<ProgramCardTriggerCandidate> CollectJiyunCompletedUseCandidates(CardActionContext action, ProgramCardContinuation? continuation) => _resolve().CollectJiyunCompletedUseCandidates(action, continuation);
    internal CardMovementBatchContext CompleteMovementBatchCaptureJiyunMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureJiyunMovementBatch(batch, completed);
    internal void ContinueLiFeng(long id) => _resolve().ContinueLiFeng(id);
    internal ProgramSkillWindowContext CreateJiyunCompletedUseContext(ProgramCardTriggerWindowFrame mother, ProgramCardTriggerCandidate candidate, ProgramSkillWindowContext ordinary) => _resolve().CreateJiyunCompletedUseContext(mother, candidate, ordinary);
    internal bool DrainLiFeng(ProgramSkillFrame f) => _resolve().DrainLiFeng(f);
    internal void FinishLiFeng(ProgramSkillFrame f) => _resolve().FinishLiFeng(f);
    internal bool HasJiyunCardMark(int ownerSeat, int cardId) => _resolve().HasJiyunCardMark(ownerSeat, cardId);
    internal bool HasJiyunResponseCompletion(CardActionContext action, ProgramCardContinuation continuation) => _resolve().HasJiyunResponseCompletion(action, continuation);
    internal bool HasLiveJiyunHandLease(JiyunLeaseIssuedEvent l) => _resolve().HasLiveJiyunHandLease(l);
    internal bool IsIssuedJiyunCompletedUseCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedJiyunCompletedUseCandidate(candidate, context);
    internal bool IsIssuedJiyunRetainedEntity(PublicPersistentPileSource pile, int cardId) => _resolve().IsIssuedJiyunRetainedEntity(pile, cardId);
    internal bool IsJiyunPile(PublicPersistentPileSource p) => _resolve().IsJiyunPile(p);
    internal bool IsLiFengCollectedEquipment(ProgramSkillFrame f, CardKind kind) => _resolve().IsLiFengCollectedEquipment(f, kind);
    internal bool IsLiFengPaidMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsLiFengPaidMovement(f, m);
    internal bool IsLiFengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsLiFengRecoveryParent(f, recovery);
    internal bool IsLiveJiyunPile(PublicPersistentPileSource p) => _resolve().IsLiveJiyunPile(p);
    internal bool IsShuliangGrainQuotaSpent(int providerSeat, int actualTurn, int turnOwnerSeat) => _resolve().IsShuliangGrainQuotaSpent(providerSeat, actualTurn, turnOwnerSeat);
    internal bool IsShuliangOriginalGrainChoice(NativePublicPileCardRequest request, NativePublicPileCardSelection selection) => _resolve().IsShuliangOriginalGrainChoice(request, selection);
    internal CardMovementRecord? JiyunActualPayment(JiyunLeaseIssuedEvent lease, CardActionContext action, long nativeId, ProgramCompletedResponseKind? responseKind) => _resolve().JiyunActualPayment(lease, action, nativeId, responseKind);
    internal JiyunCompletedEntity[] JiyunCompletedEntities(JiyunCompletedUseContext r) => _resolve().JiyunCompletedEntities(r);
    internal (long NativeId, ProgramCompletedResponseKind? ResponseKind)? JiyunCompletedOrigin(CardActionContext action, ProgramCardContinuation? continuation) => _resolve().JiyunCompletedOrigin(action, continuation);
    internal JiyunLeaseDepartedEvent? JiyunDeparture(long id) => _resolve().JiyunDeparture(id);
    internal IEnumerable<JiyunLeaseIssuedEvent> JiyunLeases() => _resolve().JiyunLeases();
    internal bool JiyunTerminalEntity(JiyunLeaseIssuedEvent lease, CardActionContext action, long nativeId, ProgramCompletedResponseKind? responseKind) => _resolve().JiyunTerminalEntity(lease, action, nativeId, responseKind);
    internal IReadOnlyList<PromptChoice> LiFengChoices(ProgramSkillFrame f) => _resolve().LiFengChoices(f);
    internal bool LiFengFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().LiFengFirstChild(f, child);
    internal ProgramSkillFrame? LiFengObserverRoot() => _resolve().LiFengObserverRoot();
    internal bool LiFengObserverSuffix(int index) => _resolve().LiFengObserverSuffix(index);
    internal bool PermitIssuedLiFeng(ProgramSkillFrame f) => _resolve().PermitIssuedLiFeng(f);
    internal void ProjectJiyunCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectJiyunCardSkillMark(cardId, ref projection);
    internal void PublishLiFeng(ProgramSkillFrame f) => _resolve().PublishLiFeng(f);
    internal void RecordShuliangGrainPayment(NativePublicPileCardPayment payment) => _resolve().RecordShuliangGrainPayment(payment);
    internal bool ResumeLiFeng(long id) => _resolve().ResumeLiFeng(id);
    internal bool RetainIssuedJiyunPile(PublicPersistentPileSource pile) => _resolve().RetainIssuedJiyunPile(pile);
    internal void ReturnJiyunPreparationDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnJiyunPreparationDraw(f, outcome);
    internal bool ReturnLiFengMovement(ProgramSkillFrame f) => _resolve().ReturnLiFengMovement(f);
    internal IReadOnlyList<NativePublicPileCardSelection> ShuliangOriginalGrainChoices(NativePublicPileCardRequest request) => _resolve().ShuliangOriginalGrainChoices(request);
    internal bool ValidJiyunClaim(JiyunClaimedEntityEvent e) => _resolve().ValidJiyunClaim(e);
    internal bool HasLiFengCapability { get => _resolve().HasLiFengCapability; }
}
