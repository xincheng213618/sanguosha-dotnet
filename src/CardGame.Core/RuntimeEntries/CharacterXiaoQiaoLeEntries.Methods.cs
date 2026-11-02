#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXiaoQiaoLeEntries
{
    internal bool AdvanceQiqinChildren() => _resolve().AdvanceQiqinChildren();
    internal bool AdvanceWeiwanChildren() => _resolve().AdvanceWeiwanChildren();
    internal SkillProgramStepOutcome BeginQiqinCollect(ProgramSkillFrame supplied) => _resolve().BeginQiqinCollect(supplied);
    internal SkillProgramStepOutcome BeginWeiwan(ProgramSkillFrame supplied) => _resolve().BeginWeiwan(supplied);
    internal bool CanActivateWeiwan(ProgramActivationLegalityContext gate) => _resolve().CanActivateWeiwan(gate);
    internal bool CanOfferQiqin(ProgramTriggerLegalityContext gate) => _resolve().CanOfferQiqin(gate);
    internal bool CanPayWeiwanQin(int owner, Card card) => _resolve().CanPayWeiwanQin(owner, card);
    internal void CompleteWeiwan(ProgramSkillFrame f) => _resolve().CompleteWeiwan(f);
    internal bool ContinuePaidWeiwan(ProgramSkillFrame f) => _resolve().ContinuePaidWeiwan(f);
    internal void ContinueQiqinCollect(long id) => _resolve().ContinueQiqinCollect(id);
    internal void ContinueWeiwan(long id) => _resolve().ContinueWeiwan(id);
    internal bool DrainWeiwan(ProgramSkillFrame f) => _resolve().DrainWeiwan(f);
    internal void FinishWeiwanObtain(long id, long batch, IReadOnlyList<CardMovementRecord> moves) => _resolve().FinishWeiwanObtain(id, batch, moves);
    internal bool HasPaidWeiwanSelection(ProgramSkillFrame f) => _resolve().HasPaidWeiwanSelection(f);
    internal bool HasQiqinEntityMark(int id) => _resolve().HasQiqinEntityMark(id);
    internal bool HasQiqinHandEntityMark(int owner, int id) => _resolve().HasQiqinHandEntityMark(owner, id);
    internal bool IsQiqinHandLimitExempt(CharacterState owner, int id) => _resolve().IsQiqinHandLimitExempt(owner, id);
    internal bool IsQiqinRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsQiqinRecoveryParent(f, recovery);
    internal bool IsWeiwanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsWeiwanRecoveryParent(f, recovery);
    internal void ObserveWeiwanHpLoss(IGameEvent payload) => _resolve().ObserveWeiwanHpLoss(payload);
    internal bool PermitIssuedQiqin(ProgramSkillFrame f) => _resolve().PermitIssuedQiqin(f);
    internal bool QiqinBatchMatches(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().QiqinBatchMatches(f, batch);
    internal bool QiqinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().QiqinFirstChild(f, child);
    internal ProgramSkillFrame? QiqinObserverRoot() => _resolve().QiqinObserverRoot();
    internal bool QiqinObserverSuffix(int index) => _resolve().QiqinObserverSuffix(index);
    internal bool ResumeQiqin(long id) => _resolve().ResumeQiqin(id);
    internal bool ResumeWeiwan(long id) => _resolve().ResumeWeiwan(id);
    internal bool ReturnQiqinMovement(ProgramSkillFrame f) => _resolve().ReturnQiqinMovement(f);
    internal bool ReturnWeiwanMovement(ProgramSkillFrame f) => _resolve().ReturnWeiwanMovement(f);
    internal bool ValidQiqinReceipt(ProgramSkillFrame f) => _resolve().ValidQiqinReceipt(f);
    internal bool WeiwanFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().WeiwanFirstChild(f, child);
    internal bool WeiwanLifecycleEdge(int index) => _resolve().WeiwanLifecycleEdge(index);
    internal bool WeiwanLionRemoval(ProgramSkillFrame f, int seat) => _resolve().WeiwanLionRemoval(f, seat);
    internal ProgramSkillFrame? WeiwanObserverRoot() => _resolve().WeiwanObserverRoot();
    internal bool WeiwanObserverSuffix(int index) => _resolve().WeiwanObserverSuffix(index);
    internal bool WeiwanOxSpill(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().WeiwanOxSpill(f, batch);
    internal ActualDiscardRecoveryPhaseKey? WeiwanPlay(int actor) => _resolve().WeiwanPlay(actor);
    internal bool WeiwanPrimaryMovement(ProgramSkillFrame f, CardMovementRecord m) => _resolve().WeiwanPrimaryMovement(f, m);
    internal bool HasLeXiaoQiaoWeiwanCapability { get => _resolve().HasLeXiaoQiaoWeiwanCapability; }
}
