#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterTangTangOlEntries
{
    internal bool AdvanceTangTangChildren() => _resolve().AdvanceTangTangChildren();
    internal void AnswerTangTang(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerTangTang(choice);
    internal void AssertTangTang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertTangTang(f);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginTangTang(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginTangTang(supplied, op);
    internal bool CanActivateZexing(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateZexing(c);
    internal void CaptureMovementBatchStartTangTangOwnedOrigins(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartTangTangOwnedOrigins(batch);
    internal global::CardGame.Core.CardMovementBatchContext CaptureTangTangGainBatch(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TangTangOwnedOrigin>? origins) => _resolve().CaptureTangTangGainBatch(batch, origins);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TangTangOwnedOrigin>? CaptureTangTangOwnedOrigins(global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardLocation> sources) => _resolve().CaptureTangTangOwnedOrigins(sources);
    internal void CaptureZexingHpLoss(global::CardGame.Core.ProgramSkillHpLostEvent e) => _resolve().CaptureZexingHpLoss(e);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectTangTangZhiyiCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectTangTangZhiyiCandidates(batch);
    internal void CollectWindow_CollectTangTangZhiyiCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectTangTangZhiyiCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureTangTangGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureTangTangGainBatch(batch, completed);
    internal void CompleteTangTangNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof) => _resolve().CompleteTangTangNativeDrawQualification(proof);
    internal bool ContinuePaidTangTang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidTangTang(f);
    internal void ContinueTangTang(long id) => _resolve().ContinueTangTang(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateTangTangZhiyiContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateTangTangZhiyiContext(frame, c);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TangTangGain>? CurrentTangTangGains(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CurrentTangTangGains(c, context);
    internal bool DrainTangTang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainTangTang(f);
    internal bool ExcludeOrdinaryMovement_IsTangTangZhiyiTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsTangTangZhiyiTrigger(candidate, trigger);
    internal void FinishTangTang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishTangTang(f);
    internal bool HasTangTangBeforeDamageObserver(long id) => _resolve().HasTangTangBeforeDamageObserver(id);
    internal bool HasTangTangDamageObserver(long id) => _resolve().HasTangTangDamageObserver(id);
    internal bool IsTangTangDying() => _resolve().IsTangTangDying();
    internal bool IsTangTangMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsTangTangMovement(f, e, p);
    internal bool IsTangTangPaidMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().IsTangTangPaidMovement(f, m);
    internal bool IsTangTangRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsTangTangRecoveryParent(f, recovery);
    internal void IssueTangTangMovement(global::CardGame.Core.ProgramSkillFrame supplied, global::System.Collections.Generic.IReadOnlyList<int> ids, global::CardGame.Core.CardLocation destination, global::CardGame.Core.TangTangStage stage, string reason) => _resolve().IssueTangTangMovement(supplied, ids, destination, stage, reason);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> ProjectTangTangPublicShownCards() => _resolve().ProjectTangTangPublicShownCards();
    internal bool? ProveTangTangMovement(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().ProveTangTangMovement(trigger, candidate, context);
    internal void PublishTangTang(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishTangTang(f);
    internal bool QualifyTangTangZhiyi(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.SkillProgramTrigger t, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().QualifyTangTangZhiyi(c, t, context);
    internal void RecordTangTangOwnedTransferProducer(global::CardGame.Core.CardMovementRecord m, int recipientSeat, long producerId) => _resolve().RecordTangTangOwnedTransferProducer(m, recipientSeat, producerId);
    internal bool ResumeTangTang(long id) => _resolve().ResumeTangTang(id);
    internal bool ReturnTangTangMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnTangTangMovement(f);
    internal void ReturnZexingPublicHand(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnZexingPublicHand(f);
    internal global::CardGame.Core.PromptChoice TangTangAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().TangTangAi(p, f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> TangTangChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TangTangChoices(f);
    internal int TangTangChooser(global::CardGame.Core.ProgramSkillFrame f) => _resolve().TangTangChooser(f);
    internal global::CardGame.Core.TangTangEntity TangTangEntityAt(int holder, int id) => _resolve().TangTangEntityAt(holder, id);
    internal bool TangTangFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().TangTangFirstChild(f, child);
    internal int TangTangGainOccurrence(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.IEnumerable<global::CardGame.Core.TangTangGain> gains) => _resolve().TangTangGainOccurrence(batch, gains);
    internal global::CardGame.Core.ProgramSkillFrame? TangTangObserverRoot() => _resolve().TangTangObserverRoot();
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardSnapshot> TangTangPrivatelyViewedCards(int viewerSeat) => _resolve().TangTangPrivatelyViewedCards(viewerSeat);
    internal global::System.Collections.Generic.IEnumerable<int> TangTangPublicShownCardIds() => _resolve().TangTangPublicShownCardIds();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> TangTangTransferCards(int chooser, int holder) => _resolve().TangTangTransferCards(chooser, holder);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateTangTangZhiyiContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateTangTangZhiyiContext(frame, candidate);
    internal bool ZexingShownStillOwned(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZexingShownStillOwned(f);
    internal bool ZexingTakeAllowed(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZexingTakeAllowed(f);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TangTangEntity> ZexingTakeCandidates(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ZexingTakeCandidates(f);
    internal bool ZexingVisited(int owner, int peer) => _resolve().ZexingVisited(owner, peer);
    internal bool ZhiyiAvailable(int owner) => _resolve().ZhiyiAvailable(owner);
    internal bool HasTangTangZhiyi { get => _resolve().HasTangTangZhiyi; }
}
