namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool ProgramAttackRangeAidChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.AttackRangeAid?.Payment is not { } payment || !ValidProgramAttackRangeAid(frame)) return false;
        if (NativeDrawAttemptRidesOn(child, frame)) return payment.Stage == ProgramAttackRangeAidPaymentStage.Drawing && frame.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery) return ProgramRecoveryReplacementReceiptMatches(frame, recovery, CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(frame, hp);
        if (child is CardsMovedTriggerWindowFrame moved) return payment.Stage == ProgramAttackRangeAidPaymentStage.MovementChildren && frame.PendingMovementContinuation is not null &&
            ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(move => _cardMovements.Contains(move) && move.Sequence > payment.Before && move.Sequence <= payment.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == frame.Id &&
            (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram ||
                state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && ProgramEventHistory<CharacterStateChangedEvent>().Any(entry => entry.Change.Id == state.Id && entry.Change.ParentFrameId == frame.Id));
    }
    private bool ProgramAttackRangeAidSuffix(int index) => HasObserverSuffix(index, static (engine, i) => engine.IsMovementObserverEdge(i) || engine.IsSameNameHandObserverEdge(i) || engine.IsYanPuDyingExitEdge(i) ||
        engine.TypedDamagePostEventEdge(engine._resolutionStack[i - 1], engine._resolutionStack[i]) ||
        engine._resolutionStack[i - 1] is ProgramSkillFrame frame && engine.ProgramAttackRangeAidChild(frame, engine._resolutionStack[i]) ||
        NativeDrawAttemptRidesOn(engine._resolutionStack[i], engine._resolutionStack[i - 1]) || engine.RecoveryReplacementFrameRidesOn(engine._resolutionStack[i], engine._resolutionStack[i - 1]),
        static (engine, i, dying) => engine.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    private ProgramSkillFrame? ProgramAttackRangeAidRoot() => FindProgramObserverRoot(static (engine, frame, child) => engine.ProgramAttackRangeAidChild(frame, child), static (engine, index) => engine.ProgramAttackRangeAidSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 56900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 56900)]
    private bool ProgramAttackRangeAidDamage(long id) => ProgramAttackRangeAidRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == id && child is DamageTriggerWindowFrame) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 56900)]
    private bool ProgramAttackRangeAidBeforeDamage(long id) => ProgramAttackRangeAidRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == id && child is BeforeDamageProgramWindowFrame) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 56900)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 56900)]
    private bool ProgramAttackRangeAidDying() => ActiveDying is { } dying && ProgramAttackRangeAidRoot() is { } frame && _resolutionStack.FindIndex(child => child.Id == dying.Id) > _resolutionStack.FindIndex(child => child.Id == frame.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 56900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 56900)]
    private bool ProgramAttackRangeAidRecovery(ProgramSkillFrame frame, RecoveryReplacementFrame child)
    {
        if (!ValidProgramAttackRangeAid(frame)) return false;
        if (ProgramRecoveryReplacementReceiptMatches(frame, child, CompleteProgramEventHistory().ToArray())) return true;
        var a = _resolutionStack.FindIndex(candidate => candidate.Id == frame.Id); var b = _resolutionStack.FindIndex(candidate => candidate.Id == child.Id);
        return a >= 0 && b > a + 1 && ProgramAttackRangeAidChild(frame, _resolutionStack[a + 1]) && ProgramAttackRangeAidSuffix(a) && RecoveryReplacementFrameRidesOn(child, _resolutionStack[b - 1]);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 56900)]
    private bool AdvanceProgramAttackRangeAidChild() => _pendingDecision is null && ProgramAttackRangeAidRoot() is not null && TryAdvanceObserverSubtree(true);
}
