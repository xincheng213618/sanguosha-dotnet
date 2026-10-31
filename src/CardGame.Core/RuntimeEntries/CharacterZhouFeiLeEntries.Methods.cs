#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFeiLeEntries
{
    internal bool AdvanceLeZhouFeiXianshuChildren() => _resolve().AdvanceLeZhouFeiXianshuChildren();
    internal SkillProgramStepOutcome BeginLeZhouFeiXianshu(ProgramSkillFrame supplied) => _resolve().BeginLeZhouFeiXianshu(supplied);
    internal bool CanActivateLeZhouFeiXianshu(ProgramActivationLegalityContext gate) => _resolve().CanActivateLeZhouFeiXianshu(gate);
    internal bool CanOfferLingkong(ProgramTriggerLegalityContext gate) => _resolve().CanOfferLingkong(gate);
    internal void CaptureLeZhouFeiXianshuHpLoss(ProgramSkillHpLostEvent fact) => _resolve().CaptureLeZhouFeiXianshuHpLoss(fact);
    internal void CaptureLeZhouFeiXianshuPreventedHpLoss(IdentityHpDeductionPreventedEvent fact) => _resolve().CaptureLeZhouFeiXianshuPreventedHpLoss(fact);
    internal CardMovementBatchContext CaptureLingkongGainBatch(CardMovementBatchContext batch) => _resolve().CaptureLingkongGainBatch(batch);
    internal ProgramLifecycleTriggerWindowFrame CaptureLingkongInitialEntities(ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureLingkongInitialEntities(window);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectLingkongMovementCandidates(CardMovementBatchContext batch) => _resolve().CollectLingkongMovementCandidates(batch);
    internal void CollectWindow_CollectLingkongMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectLingkongMovementCandidates(batch, result);
    internal void CompleteLeZhouFeiXianshu(ProgramSkillFrame frame) => _resolve().CompleteLeZhouFeiXianshu(frame);
    internal void CompleteLingkongNativeDrawQualification(NativeDrawInvocationProof proof) => _resolve().CompleteLingkongNativeDrawQualification(proof);
    internal void CompleteLingkongNativeDrawReplacement(ProgramSkillFrame frame, NativeDrawAttemptFrame attempt, NativeDrawReplacementResult replacement) => _resolve().CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
    internal CardMovementBatchContext CompleteMovementBatchCaptureLingkongGainBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureLingkongGainBatch(batch, completed);
    internal void ContinueLeZhouFeiXianshu(long frameId) => _resolve().ContinueLeZhouFeiXianshu(frameId);
    internal bool ContinuePaidLeZhouFeiXianshu(ProgramSkillFrame frame) => _resolve().ContinuePaidLeZhouFeiXianshu(frame);
    internal ProgramSkillWindowContext CreateLingkongMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate) => _resolve().CreateLingkongMovementContext(window, candidate);
    internal bool DrainLeZhouFeiXianshu(ProgramSkillFrame frame) => _resolve().DrainLeZhouFeiXianshu(frame);
    internal bool ExactLingkongGainQualification(CardMovementBatchContext batch, CardConversionSource source, string hash, scoped out LingkongGainQualifiedEvent qualified) => _resolve().ExactLingkongGainQualification(batch, source, hash, out qualified);
    internal bool ExcludeOrdinaryMovement_IsLingkongGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsLingkongGainTrigger(candidate, trigger);
    internal bool HasLingkongEntityMark(int cardId) => _resolve().HasLingkongEntityMark(cardId);
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _resolve().HasLingkongHandEntityMark(ownerSeat, cardId);
    internal bool HasPaidLeZhouFeiXianshuSelection(ProgramSkillFrame frame) => _resolve().HasPaidLeZhouFeiXianshuSelection(frame);
    internal SkillProgramStepOutcome InitializeLingkong(ProgramSkillFrame supplied) => _resolve().InitializeLingkong(supplied);
    internal bool IsIssuedLingkongCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedLingkongCandidate(candidate, context);
    internal bool IsIssuedLingkongGainCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedLingkongGainCandidate(candidate, context);
    internal bool IsIssuedLingkongInitialCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedLingkongInitialCandidate(candidate, context);
    internal bool IsLeZhouFeiXianshuRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsLeZhouFeiXianshuRecoveryParent(frame, recovery);
    internal bool IsLingkongHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsLingkongHandLimitExempt(owner, cardId);
    internal bool LeZhouFeiXianshuFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().LeZhouFeiXianshuFirstChild(frame, child);
    internal bool LeZhouFeiXianshuLifecycleEdge(int index) => _resolve().LeZhouFeiXianshuLifecycleEdge(index);
    internal ProgramSkillFrame? LeZhouFeiXianshuObserverRoot() => _resolve().LeZhouFeiXianshuObserverRoot();
    internal bool LeZhouFeiXianshuPaidMovement(ProgramSkillFrame frame, CardMovementRecord movement) => _resolve().LeZhouFeiXianshuPaidMovement(frame, movement);
    internal ActualDiscardRecoveryPhaseKey? LeZhouFeiXianshuPlay(int ownerSeat) => _resolve().LeZhouFeiXianshuPlay(ownerSeat);
    internal SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame supplied) => _resolve().MarkLingkongGain(supplied);
    internal void MergeLingkongGainInvocation(long? parent, int recipient, IReadOnlyList<LingkongGainEntity> entities, long lastBatchId, long? invocationId, long? attemptId) => _resolve().MergeLingkongGainInvocation(parent, recipient, entities, lastBatchId, invocationId, attemptId);
    internal void ObserveLeZhouFeiXianshuHealth(IGameEvent fact) => _resolve().ObserveLeZhouFeiXianshuHealth(fact);
    internal bool PermitIssuedLingkong(ProgramSkillFrame f) => _resolve().PermitIssuedLingkong(f);
    internal void PrepareLingkongGainBatch(CardMovementBatchContext batch) => _resolve().PrepareLingkongGainBatch(batch);
    internal void ProjectLingkongCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectLingkongCardSkillMark(cardId, ref projection);
    internal bool ResumeLeZhouFeiXianshu(long id) => _resolve().ResumeLeZhouFeiXianshu(id);
    internal void ReturnLeZhouFeiXianshuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnLeZhouFeiXianshuDraw(supplied, outcome);
    internal bool ReturnLeZhouFeiXianshuMovement(ProgramSkillFrame frame) => _resolve().ReturnLeZhouFeiXianshuMovement(frame);
    internal ProgramSkillWindowContext? TryCreateLingkongMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateLingkongMovementContext(frame, candidate);
    internal bool HasLeZhouFeiLingkongCapability { get => _resolve().HasLeZhouFeiLingkongCapability; }
}
