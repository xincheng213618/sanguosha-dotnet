#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrepPoisonObserversRuntime
{
    internal bool PrepPoisonFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.PrepPoisonOutcome is not { } r)
            return false;
        _corePrepPoisonOutcomesOperations.RequirePrepPoisonOutcome(root);
        if (r.Cured)
            return _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(root, child);
        if (!r.HpLossApplied)
            return false;
        if (child is DyingFrame dying)
            return r.HpAfter <= 0 && dying.ParentFrameId == root.Id && dying.VictimSeat == root.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<PlayerDyingEvent>().Count(e => e.ResolutionId == dying.Id && e.VictimSeat == root.OwnerSeat && e.KillerSeat is null) == 1;
        return child is HpChangedTriggerWindowFrame hp && hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.SourceSeat is null && hp.Change.Kind == HpChangeKind.Loss && hp.Change.HpBefore == r.HpBefore && hp.Change.HpAfter == r.HpAfter && hp.Change.Amount == r.ActualHpLoss && hp.Continuation == PostEventContinuation.Program;
    }

    internal ProgramSkillFrame? PrepPoisonObserverRoot(long? rootId = null) => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.PrepPoisonFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]) || _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)), rootId);
    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 1248)]
    internal bool AdvancePrepPoisonObserver()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || PrepPoisonObserverRoot()is null)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is NativeDrawAttemptFrame native)
        {
            _coreRuntimeOperations.AdvanceRuntimeFrame(native.Id);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return true;
        }

        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
