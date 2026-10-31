#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramHealthChildQueriesRuntime
{
    DyingFrame? IProgramHealthChildQueries.ActiveDying => _coreDyingCursorRuntimeOperations.Value.ActiveDying;

    HpChangeContext? IProgramHealthChildQueries.LastPendingLoss(long parentFrameId, int targetSeat) => _coreGameEngineHpChangeProgramsPendingHpChangesAccess._pendingHpChanges.LastOrDefault(c => c.ParentFrameId == parentFrameId && c.TargetSeat == targetSeat && c.Kind == HpChangeKind.Loss);
    bool IProgramHealthChildQueries.ProgramRecoveryChild(ProgramSkillFrame parent, ResolutionFrame child) => child switch
    {
        RecoveryReplacementFrame recovery => _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(parent, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()),
        HpChangedTriggerWindowFrame hp => _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(parent, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()),
        _ => false
    };
    bool IProgramHealthChildQueries.PaidObserverSuffix(long programFrameId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == programFrameId);
        if (index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count)
            return false;
        if (_resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying))
            return true;
        return _coreObserverSubtreesOperations.HasPaidDamageObserverSuffix(index);
    }

    bool IProgramHealthChildOperations.AdvanceObserverSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
