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
    internal void CompleteLeZhouFeiXianshu(ProgramSkillFrame frame) => _resolve().CompleteLeZhouFeiXianshu(frame);
    internal void ContinueLeZhouFeiXianshu(long frameId) => _resolve().ContinueLeZhouFeiXianshu(frameId);
    internal bool ContinuePaidLeZhouFeiXianshu(ProgramSkillFrame frame) => _resolve().ContinuePaidLeZhouFeiXianshu(frame);
    internal bool DrainLeZhouFeiXianshu(ProgramSkillFrame frame) => _resolve().DrainLeZhouFeiXianshu(frame);
    internal bool HasLingkongEntityMark(int cardId) => _resolve().HasLingkongEntityMark(cardId);
    internal bool HasLingkongHandEntityMark(int ownerSeat, int cardId) => _resolve().HasLingkongHandEntityMark(ownerSeat, cardId);
    internal bool HasPaidLeZhouFeiXianshuSelection(ProgramSkillFrame frame) => _resolve().HasPaidLeZhouFeiXianshuSelection(frame);
    internal bool IsLeZhouFeiXianshuRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => _resolve().IsLeZhouFeiXianshuRecoveryParent(frame, recovery);
    internal bool IsLingkongHandLimitExempt(CharacterState owner, int cardId) => _resolve().IsLingkongHandLimitExempt(owner, cardId);
    internal bool LeZhouFeiXianshuFirstChild(ProgramSkillFrame frame, ResolutionFrame child) => _resolve().LeZhouFeiXianshuFirstChild(frame, child);
    internal bool LeZhouFeiXianshuLifecycleEdge(int index) => _resolve().LeZhouFeiXianshuLifecycleEdge(index);
    internal ProgramSkillFrame? LeZhouFeiXianshuObserverRoot() => _resolve().LeZhouFeiXianshuObserverRoot();
    internal bool LeZhouFeiXianshuPaidMovement(ProgramSkillFrame frame, CardMovementRecord movement) => _resolve().LeZhouFeiXianshuPaidMovement(frame, movement);
    internal ActualDiscardRecoveryPhaseKey? LeZhouFeiXianshuPlay(int ownerSeat) => _resolve().LeZhouFeiXianshuPlay(ownerSeat);
    internal SkillProgramStepOutcome MarkLingkongGain(ProgramSkillFrame supplied) => _resolve().MarkLingkongGain(supplied);
    internal void ObserveLeZhouFeiXianshuHealth(IGameEvent fact) => _resolve().ObserveLeZhouFeiXianshuHealth(fact);
    internal bool ResumeLeZhouFeiXianshu(long id) => _resolve().ResumeLeZhouFeiXianshu(id);
    internal void ReturnLeZhouFeiXianshuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnLeZhouFeiXianshuDraw(supplied, outcome);
    internal bool ReturnLeZhouFeiXianshuMovement(ProgramSkillFrame frame) => _resolve().ReturnLeZhouFeiXianshuMovement(frame);
}
