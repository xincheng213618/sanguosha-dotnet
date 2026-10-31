#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangTangOlEntries
{
    internal bool AdvanceTangTangChildren() => _resolve().AdvanceTangTangChildren();
    internal void AnswerTangTang(PromptChoice choice) => _resolve().AnswerTangTang(choice);
    internal SkillProgramStepOutcome BeginTangTang(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginTangTang(supplied, op);
    internal bool CanActivateZexing(ProgramActivationLegalityContext c) => _resolve().CanActivateZexing(c);
    internal void CaptureMovementBatchStartTangTangOwnedOrigins(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartTangTangOwnedOrigins(batch);
    internal CardMovementBatchContext CaptureTangTangGainBatch(CardMovementBatchContext batch, IReadOnlyList<TangTangOwnedOrigin>? origins) => _resolve().CaptureTangTangGainBatch(batch, origins);
    internal IReadOnlyList<TangTangOwnedOrigin>? CaptureTangTangOwnedOrigins(IEnumerable<CardLocation> sources) => _resolve().CaptureTangTangOwnedOrigins(sources);
    internal void CaptureZexingHpLoss(ProgramSkillHpLostEvent e) => _resolve().CaptureZexingHpLoss(e);
    internal IEnumerable<ProgramTriggerCandidate> CollectTangTangZhiyiCandidates(CardMovementBatchContext batch) => _resolve().CollectTangTangZhiyiCandidates(batch);
    internal void CollectWindow_CollectTangTangZhiyiCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectTangTangZhiyiCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureTangTangGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureTangTangGainBatch(batch, completed);
    internal void CompleteTangTangNativeDrawQualification(NativeDrawInvocationProof proof) => _resolve().CompleteTangTangNativeDrawQualification(proof);
    internal bool ContinuePaidTangTang(ProgramSkillFrame f) => _resolve().ContinuePaidTangTang(f);
    internal void ContinueTangTang(long id) => _resolve().ContinueTangTang(id);
    internal ProgramSkillWindowContext CreateTangTangZhiyiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate c) => _resolve().CreateTangTangZhiyiContext(frame, c);
    internal IReadOnlyList<TangTangGain>? CurrentTangTangGains(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().CurrentTangTangGains(c, context);
    internal bool DrainTangTang(ProgramSkillFrame f) => _resolve().DrainTangTang(f);
    internal bool ExcludeOrdinaryMovement_IsTangTangZhiyiTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsTangTangZhiyiTrigger(candidate, trigger);
    internal void FinishTangTang(ProgramSkillFrame f) => _resolve().FinishTangTang(f);
    internal bool IsTangTangPaidMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().IsTangTangPaidMovement(f, m);
    internal bool IsTangTangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsTangTangRecoveryParent(f, recovery);
    internal void IssueTangTangMovement(ProgramSkillFrame supplied, IReadOnlyList<int> ids, CardLocation destination, TangTangStage stage, string reason) => _resolve().IssueTangTangMovement(supplied, ids, destination, stage, reason);
    internal IEnumerable<CardSnapshot> ProjectTangTangPublicShownCards() => _resolve().ProjectTangTangPublicShownCards();
    internal bool? ProveTangTangMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveTangTangMovement(trigger, candidate, context);
    internal void PublishTangTang(ProgramSkillFrame f) => _resolve().PublishTangTang(f);
    internal bool QualifyTangTangZhiyi(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext context) => _resolve().QualifyTangTangZhiyi(c, t, context);
    internal void RecordTangTangOwnedTransferProducer(CardMovementRecord m, int recipientSeat, long producerId) => _resolve().RecordTangTangOwnedTransferProducer(m, recipientSeat, producerId);
    internal bool ResumeTangTang(long id) => _resolve().ResumeTangTang(id);
    internal bool ReturnTangTangMovement(ProgramSkillFrame f) => _resolve().ReturnTangTangMovement(f);
    internal void ReturnZexingPublicHand(ProgramSkillFrame f) => _resolve().ReturnZexingPublicHand(f);
    internal PromptChoice TangTangAi(PendingDecision p, ProgramSkillFrame f) => _resolve().TangTangAi(p, f);
    internal IReadOnlyList<PromptChoice> TangTangChoices(ProgramSkillFrame f) => _resolve().TangTangChoices(f);
    internal int TangTangChooser(ProgramSkillFrame f) => _resolve().TangTangChooser(f);
    internal TangTangEntity TangTangEntityAt(int holder, int id) => _resolve().TangTangEntityAt(holder, id);
    internal bool TangTangFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().TangTangFirstChild(f, child);
    internal int TangTangGainOccurrence(CardMovementBatchContext batch, IEnumerable<TangTangGain> gains) => _resolve().TangTangGainOccurrence(batch, gains);
    internal ProgramSkillFrame? TangTangObserverRoot() => _resolve().TangTangObserverRoot();
    internal IEnumerable<CardSnapshot> TangTangPrivatelyViewedCards(int viewerSeat) => _resolve().TangTangPrivatelyViewedCards(viewerSeat);
    internal IEnumerable<int> TangTangPublicShownCardIds() => _resolve().TangTangPublicShownCardIds();
    internal IReadOnlyList<Card> TangTangTransferCards(int chooser, int holder) => _resolve().TangTangTransferCards(chooser, holder);
    internal ProgramSkillWindowContext? TryCreateTangTangZhiyiContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateTangTangZhiyiContext(frame, candidate);
    internal bool ZexingShownStillOwned(ProgramSkillFrame f) => _resolve().ZexingShownStillOwned(f);
    internal bool ZexingTakeAllowed(ProgramSkillFrame f) => _resolve().ZexingTakeAllowed(f);
    internal IReadOnlyList<TangTangEntity> ZexingTakeCandidates(ProgramSkillFrame f) => _resolve().ZexingTakeCandidates(f);
    internal bool ZexingVisited(int owner, int peer) => _resolve().ZexingVisited(owner, peer);
    internal bool ZhiyiAvailable(int owner) => _resolve().ZhiyiAvailable(owner);
    internal bool HasTangTangZhiyi { get => _resolve().HasTangTangZhiyi; }
}
