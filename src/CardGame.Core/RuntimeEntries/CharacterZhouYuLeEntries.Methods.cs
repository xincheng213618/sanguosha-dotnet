#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuLeEntries
{
    internal bool AdvanceGuYinChildren() => _resolve().AdvanceGuYinChildren();
    internal int ApplyGuYinInitialHandSize(global::CardGame.Core.CharacterState player, int ordinaryCount) => _resolve().ApplyGuYinInitialHandSize(player, ordinaryCount);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginGuYinDraw(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginGuYinDraw(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginPinglu(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginPinglu(supplied);
    internal bool CanActivatePinglu(global::CardGame.Core.ProgramActivationLegalityContext gate) => _resolve().CanActivatePinglu(gate);
    internal bool CanContinueIssuedGuYin(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedGuYin(f);
    internal bool CanContinueIssuedPinglu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().CanContinueIssuedPinglu(frame);
    internal bool CanOfferGuYin(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().CanOfferGuYin(g);
    internal void CaptureGuYinDiscardBatch(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GuYinSourceSnapshot> sources) => _resolve().CaptureGuYinDiscardBatch(batch, sources);
    internal global::CardGame.Core.ProgramLifecycleTriggerWindowFrame CaptureGuYinInitialEntities(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureGuYinInitialEntities(window);
    internal void CaptureGuYinOpeningOrigins() => _resolve().CaptureGuYinOpeningOrigins();
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GuYinSourceSnapshot> CaptureGuYinSources() => _resolve().CaptureGuYinSources();
    internal void CaptureMovementBatchStartGuYinSources(global::CardGame.Core.CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartGuYinSources(batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> CollectGuYinDiscardCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectGuYinDiscardCandidates(batch);
    internal void CollectWindow_CollectGuYinDiscardCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectGuYinDiscardCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureGuYinDiscardBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureGuYinDiscardBatch(batch, completed);
    internal void ContinueGuYinDraw(long id) => _resolve().ContinueGuYinDraw(id);
    internal void ContinuePinglu(long id) => _resolve().ContinuePinglu(id);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateGuYinMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame window, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateGuYinMovementContext(window, c);
    internal bool DrainGuYinDraw(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DrainGuYinDraw(f);
    internal global::CardGame.Core.GuYinDiscardQualifiedEvent[] ExactGuYinQualifications(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardConversionSource source, string hash) => _resolve().ExactGuYinQualifications(batch, source, hash);
    internal bool ExcludeOrdinaryMovement_IsGuYinDiscardTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsGuYinDiscardTrigger(candidate, trigger);
    internal bool GuYinDrawMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementRecord m) => _resolve().GuYinDrawMovement(f, m);
    internal bool GuYinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().GuYinFirstChild(f, child);
    internal bool GuYinMotherMatches(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().GuYinMotherMatches(c, context);
    internal global::CardGame.Core.ProgramSkillFrame? GuYinObserverRoot() => _resolve().GuYinObserverRoot();
    internal bool GuYinObserverSuffix(int index) => _resolve().GuYinObserverSuffix(index);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.GuYinInitialEntity> GuYinOriginalEntities() => _resolve().GuYinOriginalEntities();
    internal bool HasGuYinBeforeDamageObserver(long id) => _resolve().HasGuYinBeforeDamageObserver(id);
    internal bool HasGuYinDamageObserver(long id) => _resolve().HasGuYinDamageObserver(id);
    internal bool HasPingluCustody(int owner, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().HasPingluCustody(owner, phase);
    internal global::CardGame.Core.SkillProgramStepOutcome InitializeGuYin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().InitializeGuYin(supplied);
    internal bool IsGuYinMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().IsGuYinMovement(f, effect, p);
    internal bool IsGuYinRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsGuYinRecoveryParent(f, recovery);
    internal bool IsIssuedGuYinCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedGuYinCandidate(c, context);
    internal bool IsIssuedGuYinInitialCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().IsIssuedGuYinInitialCandidate(c, context);
    internal bool IsPingluMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsPingluMovement(frame, effect, pending);
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? PingluPlay(int actor) => _resolve().PingluPlay(actor);
    internal int[] PingluTargets(int actor) => _resolve().PingluTargets(actor);
    internal bool ResumeGuYin(long id) => _resolve().ResumeGuYin(id);
    internal bool ResumePinglu(long id) => _resolve().ResumePinglu(id);
    internal void ReturnGuYinDraw(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnGuYinDraw(f, outcome);
    internal bool ReturnGuYinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnGuYinMovement(f);
    internal bool ReturnPingluMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnPingluMovement(frame);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateGuYinMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateGuYinMovementContext(frame, candidate);
    internal bool ValidGuYinInitialEntry(global::CardGame.Core.GuYinInitialEntity e) => _resolve().ValidGuYinInitialEntry(e);
    internal bool ValidGuYinReceipt(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidGuYinReceipt(f);
    internal bool ValidPingluPaid(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPingluPaid(frame);
    internal bool ValidPingluStarted(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ValidPingluStarted(frame);
    internal bool HasGuYinCapability { get => _resolve().HasGuYinCapability; }
    internal bool HasLeZhouYuPingluCapability { get => _resolve().HasLeZhouYuPingluCapability; }
}
