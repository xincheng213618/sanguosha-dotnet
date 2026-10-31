#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouFeiLeEntries
{
    internal bool AdvanceLeZhouFeiXianshuChildren() => _resolve().AdvanceLeZhouFeiXianshuChildren();
    internal global::CardGame.Core.SkillProgramStepOutcome BeginLeZhouFeiXianshu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginLeZhouFeiXianshu(supplied);
    internal bool CanActivateLeZhouFeiXianshu(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().CanActivateLeZhouFeiXianshu(gate);
    internal bool CanOfferLingkong(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferLingkong(gate);
    internal void CaptureLeZhouFeiXianshuHpLoss(global::CardGame.Core.ProgramSkillHpLostEvent fact) => _resolve().CaptureLeZhouFeiXianshuHpLoss(fact);
    internal void CaptureLeZhouFeiXianshuPreventedHpLoss(global::CardGame.Core.IdentityHpDeductionPreventedEvent fact) => _resolve().CaptureLeZhouFeiXianshuPreventedHpLoss(fact);
    internal global::CardGame.Core.CardMovementBatchContext CaptureLingkongGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureLingkongGainBatch(batch);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureLingkongInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureLingkongInitialEntities(window);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectLingkongMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectLingkongMovementCandidates(batch);
    internal void CollectWindow_CollectLingkongMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectLingkongMovementCandidates(batch, result);
    internal void CompleteLeZhouFeiXianshu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CompleteLeZhouFeiXianshu(frame);
    internal void CompleteLingkongNativeDrawQualification(global::CardGame.Core.NativeDrawInvocationProof proof) => _resolve().CompleteLingkongNativeDrawQualification(proof);
    internal void CompleteLingkongNativeDrawReplacement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawAttemptFrame attempt, global::CardGame.Core.NativeDrawReplacementResult replacement) => _resolve().CompleteLingkongNativeDrawReplacement(frame, attempt, replacement);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureLingkongGainBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureLingkongGainBatch(batch, completed);
    internal void ContinueLeZhouFeiXianshu(long frameId) => _resolve().ContinueLeZhouFeiXianshu(frameId);
    internal bool ContinuePaidLeZhouFeiXianshu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinuePaidLeZhouFeiXianshu(frame);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateLingkongMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().CreateLingkongMovementContext(window, candidate);
    internal bool DrainLeZhouFeiXianshu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().DrainLeZhouFeiXianshu(frame);
    internal bool ExactLingkongGainQualification(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardConversionSource source, string hash, scoped out global::CardGame.Core.LingkongGainQualifiedEvent qualified) => _resolve().ExactLingkongGainQualification(batch, source, hash, out qualified);
    internal bool ExcludeOrdinaryMovement_IsLingkongGainTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsLingkongGainTrigger(candidate, trigger);
    internal bool HasLeZhouFeiXianshuBeforeDamageObserver(long id) => _resolve().HasLeZhouFeiXianshuBeforeDamageObserver(id);
    internal bool HasLeZhouFeiXianshuDamageObserver(long id) => _resolve().HasLeZhouFeiXianshuDamageObserver(id);
    internal bool HasLingkongEntityMark(int cardId) => _resolve().HasLingkongEntityMark(cardId);
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _resolve().HasLingkongHandEntityMark(ownerSeat, cardId);
    internal bool HasPaidLeZhouFeiXianshuSelection(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().HasPaidLeZhouFeiXianshuSelection(frame);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeLingkong(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeLingkong(supplied);
    internal bool IsIssuedLingkongCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedLingkongCandidate(candidate, context);
    internal bool IsIssuedLingkongGainCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedLingkongGainCandidate(candidate, context);
    internal bool IsIssuedLingkongInitialCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedLingkongInitialCandidate(candidate, context);
    internal bool IsLeZhouFeiXianshuMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsLeZhouFeiXianshuMovement(frame, effect, pending);
    internal bool IsLeZhouFeiXianshuRecoveryParent(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsLeZhouFeiXianshuRecoveryParent(frame, recovery);
    internal bool IsLingkongHandLimitExempt(global::CardGame.Core.CharacterState owner, int cardId) => _resolve().IsLingkongHandLimitExempt(owner, cardId);
    internal bool LeZhouFeiXianshuFirstChild(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.ResolutionFrame child) => _resolve().LeZhouFeiXianshuFirstChild(frame, child);
    internal bool LeZhouFeiXianshuLifecycleEdge(int index) => _resolve().LeZhouFeiXianshuLifecycleEdge(index);
    internal global::CardGame.Core.ProgramSkillFrame? LeZhouFeiXianshuObserverRoot() => _resolve().LeZhouFeiXianshuObserverRoot();
    internal bool LeZhouFeiXianshuPaidMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementRecord movement) => _resolve().LeZhouFeiXianshuPaidMovement(frame, movement);
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? LeZhouFeiXianshuPlay(int ownerSeat) => _resolve().LeZhouFeiXianshuPlay(ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome MarkLingkongGain(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().MarkLingkongGain(supplied);
    internal void MergeLingkongGainInvocation(long? parent, int recipient, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.LingkongGainEntity> entities, long lastBatchId, long? invocationId, long? attemptId) => _resolve().MergeLingkongGainInvocation(parent, recipient, entities, lastBatchId, invocationId, attemptId);
    internal void ObserveLeZhouFeiXianshuHealth(global::CardGame.Core.IGameEvent fact) => _resolve().ObserveLeZhouFeiXianshuHealth(fact);
    internal bool PermitIssuedLingkong(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PermitIssuedLingkong(f);
    internal void PrepareLingkongGainBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().PrepareLingkongGainBatch(batch);
    internal void ProjectLingkongCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectLingkongCardSkillMark(cardId, ref projection);
    internal bool ResumeLeZhouFeiXianshu(long id) => _resolve().ResumeLeZhouFeiXianshu(id);
    internal void ReturnLeZhouFeiXianshuDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnLeZhouFeiXianshuDraw(supplied, outcome);
    internal bool ReturnLeZhouFeiXianshuMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnLeZhouFeiXianshuMovement(frame);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateLingkongMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateLingkongMovementContext(frame, candidate);
    internal bool HasLeZhouFeiLingkongCapability { get => _resolve().HasLeZhouFeiLingkongCapability; }
}
