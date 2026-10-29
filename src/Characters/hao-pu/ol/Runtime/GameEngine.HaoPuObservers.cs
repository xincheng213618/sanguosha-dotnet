namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 120)]
    private bool IsZhenyingMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) =>
        effect?.Op == SkillProgramEffectOp.ResolveZhenyingSimultaneousHands && f.Zhenying is { Stage: ZhenyingStage.AdjustmentChildren } &&
        p is { BeforeCount: 0, CoverageResultBind: null } && p.SubjectSeat == f.OwnerSeat && ValidZhenying(f);
    private bool ZhenyingFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Zhenying is not { Stage: ZhenyingStage.AdjustmentChildren } r || f.PendingMovementContinuation is null || !ValidZhenying(f)) return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.OriginOwnerSeat == f.OwnerSeat &&
                moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId &&
                moved.Batch.Movements.All(m => _cardMovements.Contains(m) && m.Sequence > r.PaymentBefore && m.Sequence <= r.PaymentAfter &&
                    (m.Reason.Value is ZhenyingDiscardReason or ZhenyingDrawReason || m.Reason == CardMoveReasons.Reshuffle));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills &&
            skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }
    private ProgramSkillFrame? ZhenyingObserverRoot() => FindProgramObserverRoot(static (e, root, child) => e.ZhenyingFirstChild(root, child),
        static (e, index) => e.HasMovementObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 430)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 540)]
    private bool HasZhenyingDamageObserver(long id) => ZhenyingObserverRoot() is { } root &&
        _resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 570)]
    private bool HasZhenyingBeforeDamageObserver(long id) => ZhenyingObserverRoot() is { } root &&
        _resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && _resolutionStack.FindIndex(f => f.Id == id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 400)]
    private bool IsZhenyingDying() => ActiveDying is { } dying && ZhenyingObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26500)]
    private bool AdvanceZhenyingSubtree() => _pendingDecision is null && ZhenyingObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
