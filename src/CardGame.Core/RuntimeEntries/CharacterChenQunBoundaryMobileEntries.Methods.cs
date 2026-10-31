#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunBoundaryMobileEntries
{
    internal bool AdvanceBoundaryDingpinObservers() => _resolve().AdvanceBoundaryDingpinObservers();
    internal bool BoundaryDingpinActiveTurn(int turn, int owner) => _resolve().BoundaryDingpinActiveTurn(turn, owner);
    internal bool BoundaryDingpinCanDiscard(int owner, Card card) => _resolve().BoundaryDingpinCanDiscard(owner, card);
    internal bool BoundaryDingpinCanTarget(int owner, int target) => _resolve().BoundaryDingpinCanTarget(owner, target);
    internal (NativeLogicalDiscardInvocationRecordedEvent Invocation, NativeLogicalDiscardMaterialCompletedEvent Material)? BoundaryDingpinCost(ProgramSkillFrame f) => _resolve().BoundaryDingpinCost(f);
    internal bool BoundaryDingpinDyingObserver() => _resolve().BoundaryDingpinDyingObserver();
    internal bool BoundaryDingpinFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().BoundaryDingpinFirstChild(f, child);
    internal ProgramSkillFrame? BoundaryDingpinObserverRoot() => _resolve().BoundaryDingpinObserverRoot();
    internal bool BoundaryDingpinRecovery(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().BoundaryDingpinRecovery(f, recovery);
    internal bool CanActivateBoundaryDingpin(ProgramActivationLegalityContext c) => _resolve().CanActivateBoundaryDingpin(c);
    internal void ContinueBoundaryDingpinDraw(long id) => _resolve().ContinueBoundaryDingpinDraw(id);
    internal bool KeepBoundaryDingpinPaidFrame(ProgramSkillFrame f) => _resolve().KeepBoundaryDingpinPaidFrame(f);
    internal SkillProgramStepOutcome PrepareBoundaryDingpin(ProgramSkillFrame supplied) => _resolve().PrepareBoundaryDingpin(supplied);
    internal SkillProgramStepOutcome ResolveBoundaryDingpin(ProgramSkillFrame supplied) => _resolve().ResolveBoundaryDingpin(supplied);
    internal bool ResumeBoundaryDingpin(long id) => _resolve().ResumeBoundaryDingpin(id);
    internal void ReturnBoundaryDingpinDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnBoundaryDingpinDraw(supplied, outcome);
    internal bool ReturnBoundaryDingpinMovement(ProgramSkillFrame f) => _resolve().ReturnBoundaryDingpinMovement(f);
}
