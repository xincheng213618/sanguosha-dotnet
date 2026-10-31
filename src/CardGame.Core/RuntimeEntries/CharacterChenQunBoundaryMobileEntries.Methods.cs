#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterChenQunBoundaryMobileEntries
{
    internal bool AdvanceBoundaryDingpinObservers() => _resolve().AdvanceBoundaryDingpinObservers();
    internal bool BoundaryDingpinActiveTurn(int turn, int owner) => _resolve().BoundaryDingpinActiveTurn(turn, owner);
    internal bool BoundaryDingpinCanDiscard(int owner, global::CardGame.Core.Card card) => _resolve().BoundaryDingpinCanDiscard(owner, card);
    internal bool BoundaryDingpinCanTarget(int owner, int target) => _resolve().BoundaryDingpinCanTarget(owner, target);
    internal (global::CardGame.Core.NativeLogicalDiscardInvocationRecordedEvent Invocation, global::CardGame.Core.NativeLogicalDiscardMaterialCompletedEvent Material)? BoundaryDingpinCost(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BoundaryDingpinCost(f);
    internal bool BoundaryDingpinDamageObserver(long id) => _resolve().BoundaryDingpinDamageObserver(id);
    internal bool BoundaryDingpinDyingObserver() => _resolve().BoundaryDingpinDyingObserver();
    internal bool BoundaryDingpinFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().BoundaryDingpinFirstChild(f, child);
    internal bool BoundaryDingpinMovementProof(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? e, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().BoundaryDingpinMovementProof(f, e, p);
    internal global::CardGame.Core.ProgramSkillFrame? BoundaryDingpinObserverRoot() => _resolve().BoundaryDingpinObserverRoot();
    internal bool BoundaryDingpinRecovery(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().BoundaryDingpinRecovery(f, recovery);
    internal bool CanActivateBoundaryDingpin(global::CardGame.Core.ProgramActivationLegalityContext c) => _resolve().CanActivateBoundaryDingpin(c);
    internal void ContinueBoundaryDingpinDraw(long id) => _resolve().ContinueBoundaryDingpinDraw(id);
    internal bool KeepBoundaryDingpinPaidFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().KeepBoundaryDingpinPaidFrame(f);
    internal global::CardGame.Core.SkillProgramStepOutcome PrepareBoundaryDingpin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().PrepareBoundaryDingpin(supplied);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveBoundaryDingpin(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveBoundaryDingpin(supplied);
    internal bool ResumeBoundaryDingpin(long id) => _resolve().ResumeBoundaryDingpin(id);
    internal void ReturnBoundaryDingpinDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnBoundaryDingpinDraw(supplied, outcome);
    internal bool ReturnBoundaryDingpinMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnBoundaryDingpinMovement(f);
}
