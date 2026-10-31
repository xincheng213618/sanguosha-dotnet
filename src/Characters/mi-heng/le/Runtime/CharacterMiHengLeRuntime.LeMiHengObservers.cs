#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMiHengLeRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28211)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28211)]
    internal bool PermitPaidJigu(ProgramSkillFrame f)
    {
        if (f.Jigu is not { Paid: true })
            return false;
        return true;
    }
    internal bool JiguDrawMovement(ProgramSkillFrame f, CardMovementRecord movement)
    {
        if (f.Jigu is not { Paid: true, Stage: JiguStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(movement) || movement.Sequence <= o.SequenceBefore || movement.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(movement.CardId) && movement.From == CardLocation.DrawPile && movement.To == CardLocation.Hand(f.OwnerSeat) && movement.Reason.Value == JiguDrawReason || o.ReplacementGainedIds.Contains(movement.CardId) && movement.To == CardLocation.Hand(f.OwnerSeat) || movement.From == CardLocation.DiscardPile && movement.To == CardLocation.DrawPile && movement.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool JiguFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Jigu is not { Paid: true } r)
            return false;
        if (r.Stage == JiguStage.Drawing && f.NativeDraw is not null)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => JiguDrawMovement(f, m));
    }

    internal bool JiguObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: { Paid: true } } replacement && _characterSunHongOlOperations.ZenrunFirstChild(replacement, _resolutionStackOperations2._resolutionStack()[n]) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? JiguObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.JiguFirstChild(f, child), (index) => this.JiguObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 360)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 350)]
    internal bool IsLeMiHengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Jigu is not { Paid: true } || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && JiguFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && JiguObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28211)]
    internal bool AdvanceJiguChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && JiguObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
