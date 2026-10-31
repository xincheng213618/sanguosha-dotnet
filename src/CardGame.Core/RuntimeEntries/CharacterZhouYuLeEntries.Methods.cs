#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhouYuLeEntries
{
    internal bool AdvanceGuYinChildren() => _resolve().AdvanceGuYinChildren();
    internal int ApplyGuYinInitialHandSize(CharacterState player, int ordinaryCount) => _resolve().ApplyGuYinInitialHandSize(player, ordinaryCount);
    internal SkillProgramStepOutcome BeginGuYinDraw(ProgramSkillFrame supplied) => _resolve().BeginGuYinDraw(supplied);
    internal SkillProgramStepOutcome BeginPinglu(ProgramSkillFrame supplied) => _resolve().BeginPinglu(supplied);
    internal bool CanActivatePinglu(ProgramActivationLegalityContext gate) => _resolve().CanActivatePinglu(gate);
    internal bool CanContinueIssuedGuYin(ProgramSkillFrame f) => _resolve().CanContinueIssuedGuYin(f);
    internal bool CanContinueIssuedPinglu(ProgramSkillFrame frame) => _resolve().CanContinueIssuedPinglu(frame);
    internal bool CanOfferGuYin(ProgramTriggerLegalityContext g) => _resolve().CanOfferGuYin(g);
    internal void CaptureGuYinDiscardBatch(CardMovementBatchContext batch, IReadOnlyList<GuYinSourceSnapshot> sources) => _resolve().CaptureGuYinDiscardBatch(batch, sources);
    internal ProgramLifecycleTriggerWindowFrame CaptureGuYinInitialEntities(ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureGuYinInitialEntities(window);
    internal void CaptureGuYinOpeningOrigins() => _resolve().CaptureGuYinOpeningOrigins();
    internal IReadOnlyList<GuYinSourceSnapshot> CaptureGuYinSources() => _resolve().CaptureGuYinSources();
    internal void CaptureMovementBatchStartGuYinSources(CardMovementBatchBuilder batch) => _resolve().CaptureMovementBatchStartGuYinSources(batch);
    internal IReadOnlyList<ProgramTriggerCandidate> CollectGuYinDiscardCandidates(CardMovementBatchContext batch) => _resolve().CollectGuYinDiscardCandidates(batch);
    internal void CollectWindow_CollectGuYinDiscardCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectGuYinDiscardCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureGuYinDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureGuYinDiscardBatch(batch, completed);
    internal void ContinueGuYinDraw(long id) => _resolve().ContinueGuYinDraw(id);
    internal void ContinuePinglu(long id) => _resolve().ContinuePinglu(id);
    internal ProgramSkillWindowContext CreateGuYinMovementContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate c) => _resolve().CreateGuYinMovementContext(window, c);
    internal bool DrainGuYinDraw(ProgramSkillFrame f) => _resolve().DrainGuYinDraw(f);
    internal GuYinDiscardQualifiedEvent[] ExactGuYinQualifications(CardMovementBatchContext batch, CardConversionSource source, string hash) => _resolve().ExactGuYinQualifications(batch, source, hash);
    internal bool ExcludeOrdinaryMovement_IsGuYinDiscardTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsGuYinDiscardTrigger(candidate, trigger);
    internal bool GuYinDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().GuYinDrawMovement(f, m);
    internal bool GuYinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().GuYinFirstChild(f, child);
    internal bool GuYinMotherMatches(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().GuYinMotherMatches(c, context);
    internal ProgramSkillFrame? GuYinObserverRoot() => _resolve().GuYinObserverRoot();
    internal bool GuYinObserverSuffix(int index) => _resolve().GuYinObserverSuffix(index);
    internal IReadOnlyList<GuYinInitialEntity> GuYinOriginalEntities() => _resolve().GuYinOriginalEntities();
    internal bool HasPingluCustody(int owner, ActualDiscardRecoveryPhaseKey phase) => _resolve().HasPingluCustody(owner, phase);
    internal SkillProgramStepOutcome InitializeGuYin(ProgramSkillFrame supplied) => _resolve().InitializeGuYin(supplied);
    internal bool IsGuYinMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().IsGuYinMovement(f, effect, p);
    internal bool IsGuYinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsGuYinRecoveryParent(f, recovery);
    internal bool IsIssuedGuYinCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedGuYinCandidate(c, context);
    internal bool IsIssuedGuYinInitialCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedGuYinInitialCandidate(c, context);
    internal ActualDiscardRecoveryPhaseKey? PingluPlay(int actor) => _resolve().PingluPlay(actor);
    internal int[] PingluTargets(int actor) => _resolve().PingluTargets(actor);
    internal bool ResumeGuYin(long id) => _resolve().ResumeGuYin(id);
    internal bool ResumePinglu(long id) => _resolve().ResumePinglu(id);
    internal void ReturnGuYinDraw(ProgramSkillFrame f, NativeDrawOutcome outcome) => _resolve().ReturnGuYinDraw(f, outcome);
    internal bool ReturnGuYinMovement(ProgramSkillFrame f) => _resolve().ReturnGuYinMovement(f);
    internal bool ReturnPingluMovement(ProgramSkillFrame frame) => _resolve().ReturnPingluMovement(frame);
    internal ProgramSkillWindowContext? TryCreateGuYinMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateGuYinMovementContext(frame, candidate);
    internal bool ValidGuYinInitialEntry(GuYinInitialEntity e) => _resolve().ValidGuYinInitialEntry(e);
    internal bool ValidGuYinReceipt(ProgramSkillFrame f) => _resolve().ValidGuYinReceipt(f);
    internal bool ValidPingluStarted(ProgramSkillFrame frame) => _resolve().ValidPingluStarted(frame);
    internal bool HasGuYinCapability { get => _resolve().HasGuYinCapability; }
    internal bool HasLeZhouYuPingluCapability { get => _resolve().HasLeZhouYuPingluCapability; }
}
