#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoLeEntries
{
    internal bool AdvanceQiqinChildren() => _resolve().AdvanceQiqinChildren();
    internal bool AdvanceWeiwanChildren() => _resolve().AdvanceWeiwanChildren();
    internal void AssertWeiwanUses() => _resolve().AssertWeiwanUses();
    internal SkillProgramStepOutcome BeginQiqinCollect(ProgramSkillFrame supplied) => _resolve().BeginQiqinCollect(supplied);
    internal SkillProgramStepOutcome BeginWeiwan(ProgramSkillFrame supplied) => _resolve().BeginWeiwan(supplied);
    internal bool CanActivateWeiwan(ProgramActivationLegalityContext gate) => _resolve().CanActivateWeiwan(gate);
    internal bool CanOfferQiqin(ProgramTriggerLegalityContext gate) => _resolve().CanOfferQiqin(gate);
    internal bool CanPayWeiwanQin(int owner, Card card) => _resolve().CanPayWeiwanQin(owner, card);
    internal ProgramLifecycleTriggerWindowFrame CaptureQiqinInitialEntities(ProgramLifecycleTriggerWindowFrame window) => _resolve().CaptureQiqinInitialEntities(window);
    internal DeclaredCardPayment CaptureWeiwanDeclarationPayment(DeclaredCardPayment paid, CardDeclarationParentReturn parent) => _resolve().CaptureWeiwanDeclarationPayment(paid, parent);
    internal WeiwanUseSnapshot CaptureWeiwanUseSnapshot(int actor) => _resolve().CaptureWeiwanUseSnapshot(actor);
    internal void CaptureWeiwanZeroMaterialUse(IGameEvent payload) => _resolve().CaptureWeiwanZeroMaterialUse(payload);
    internal void CompleteWeiwan(ProgramSkillFrame f) => _resolve().CompleteWeiwan(f);
    internal bool ContinuePaidWeiwan(ProgramSkillFrame f) => _resolve().ContinuePaidWeiwan(f);
    internal void ContinueQiqinCollect(long id) => _resolve().ContinueQiqinCollect(id);
    internal void ContinueWeiwan(long id) => _resolve().ContinueWeiwan(id);
    internal bool CurrentWeiwanLease(WeiwanTurnLease lease) => _resolve().CurrentWeiwanLease(lease);
    internal bool DistanceCardUseExemption40HasWeiwanUseDistance(ProgramCardDistanceQuery q) => _resolve().DistanceCardUseExemption40HasWeiwanUseDistance(q);
    internal bool DrainWeiwan(ProgramSkillFrame f) => _resolve().DrainWeiwan(f);
    internal void FinishWeiwanObtain(long id, long batch, IReadOnlyList<CardMovementRecord> moves) => _resolve().FinishWeiwanObtain(id, batch, moves);
    internal bool HasPaidWeiwanSelection(ProgramSkillFrame f) => _resolve().HasPaidWeiwanSelection(f);
    internal bool HasQiqinEntityMark(int id) => _resolve().HasQiqinEntityMark(id);
    internal bool HasQiqinHandEntityMark(int owner, int id) => _resolve().HasQiqinHandEntityMark(owner, id);
    internal bool HasWeiwanQuota(CharacterState actor, CharacterState target) => _resolve().HasWeiwanQuota(actor, target);
    internal bool HasWeiwanUseDistance(CharacterState actor, CharacterState target, CardKind kind, long? useId = default) => _resolve().HasWeiwanUseDistance(actor, target, kind, useId);
    internal SkillProgramStepOutcome InitializeQiqin(ProgramSkillFrame supplied) => _resolve().InitializeQiqin(supplied);
    internal bool IsIssuedQiqinCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => _resolve().IsIssuedQiqinCandidate(c, context);
    internal bool IsQiqinHandLimitExempt(CharacterState owner, int id) => _resolve().IsQiqinHandLimitExempt(owner, id);
    internal bool IsQiqinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsQiqinRecoveryParent(f, recovery);
    internal bool IsWeiwanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsWeiwanRecoveryParent(f, recovery);
    internal bool IsWeiwanTargetForbidden(int actor, int target, CardKind kind, long? useId = default) => _resolve().IsWeiwanTargetForbidden(actor, target, kind, useId);
    internal void IssueWeiwanUse(CardUseFrame use) => _resolve().IssueWeiwanUse(use);
    internal void ObserveWeiwanHpLoss(IGameEvent payload) => _resolve().ObserveWeiwanHpLoss(payload);
    internal bool PermitIssuedQiqin(ProgramSkillFrame f) => _resolve().PermitIssuedQiqin(f);
    internal void ProjectQiqinCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectQiqinCardSkillMark(cardId, ref projection);
    internal bool QiqinBatchMatches(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().QiqinBatchMatches(f, batch);
    internal bool QiqinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().QiqinFirstChild(f, child);
    internal ProgramSkillFrame? QiqinObserverRoot() => _resolve().QiqinObserverRoot();
    internal bool QiqinObserverSuffix(int index) => _resolve().QiqinObserverSuffix(index);
    internal bool ResumeQiqin(long id) => _resolve().ResumeQiqin(id);
    internal bool ResumeWeiwan(long id) => _resolve().ResumeWeiwan(id);
    internal bool ReturnQiqinMovement(ProgramSkillFrame f) => _resolve().ReturnQiqinMovement(f);
    internal bool ReturnWeiwanMovement(ProgramSkillFrame f) => _resolve().ReturnWeiwanMovement(f);
    internal WeiwanUseReceipt? ValidIssuedWeiwanUse(long id, int actor) => _resolve().ValidIssuedWeiwanUse(id, actor);
    internal bool ValidQiqinReceipt(ProgramSkillFrame f) => _resolve().ValidQiqinReceipt(f);
    internal bool ValidWeiwanDeclaration(DeclaredCardPayment paid, WeiwanUseSnapshot q) => _resolve().ValidWeiwanDeclaration(paid, q);
    internal bool ValidWeiwanLease(WeiwanTurnLease lease) => _resolve().ValidWeiwanLease(lease);
    internal bool ValidWeiwanSnapshot(WeiwanUseSnapshot q) => _resolve().ValidWeiwanSnapshot(q);
    internal bool WeiwanDirectedTargetProhibited(ProgramDirectedTargetQuery query) => _resolve().WeiwanDirectedTargetProhibited(query);
    internal bool WeiwanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WeiwanFirstChild(f, child);
    internal bool WeiwanLifecycleEdge(int index) => _resolve().WeiwanLifecycleEdge(index);
    internal bool WeiwanLionRemoval(ProgramSkillFrame f, int seat) => _resolve().WeiwanLionRemoval(f, seat);
    internal ProgramSkillFrame? WeiwanObserverRoot() => _resolve().WeiwanObserverRoot();
    internal bool WeiwanObserverSuffix(int index) => _resolve().WeiwanObserverSuffix(index);
    internal bool WeiwanOxSpill(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().WeiwanOxSpill(f, batch);
    internal ActualDiscardRecoveryPhaseKey? WeiwanPlay(int actor) => _resolve().WeiwanPlay(actor);
    internal IReadOnlyList<WeiwanTurnLease> WeiwanPreUseLeases(int actor) => _resolve().WeiwanPreUseLeases(actor);
    internal bool WeiwanPrimaryMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().WeiwanPrimaryMovement(f, m);
    internal IReadOnlyList<WeiwanTurnLease> WeiwanQueryLeases(int actor, long? useId) => _resolve().WeiwanQueryLeases(actor, useId);
    internal bool WeiwanTargetQuotaAllowed(ProgramTargetQuotaQuery query) => _resolve().WeiwanTargetQuotaAllowed(query);
    internal bool HasLeXiaoQiaoQiqinCapability { get => _resolve().HasLeXiaoQiaoQiqinCapability; }
    internal bool HasLeXiaoQiaoWeiwanCapability { get => _resolve().HasLeXiaoQiaoWeiwanCapability; }
}
