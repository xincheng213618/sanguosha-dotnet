#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceBiriObserversRuntime
{
    internal bool ShanheSourceBiriFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.ShanheSourceBiri is not { } receipt)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (child is RecoveryReplacementFrame recovery)
            return receipt.Stage == ShanheSourceBiriStage.RecoveryChildren && recovery.Id == receipt.RecoveryAttemptId && recovery.Attempt.Amount == receipt.RecoveryAttemptAmount && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(root, recovery, history);
        if (child is HpChangedTriggerWindowFrame hp)
            return receipt.Stage == ShanheSourceBiriStage.RecoveryChildren && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(root, hp, history) && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == receipt.ActualRecovered && root.RecoveryApplications is { } applications && applications.Any(application => application.HpChangeId == hp.Change.Id && application.RecoveryFrameId == receipt.AppliedRecoveryFrameId);
        if (child is CardsMovedTriggerWindowFrame movement)
            return receipt.Stage == ShanheSourceBiriStage.DrawChildren && _coreSuitPreventionBenefitObserversOperations.SuitPlacementMovementRoot(root, movement) && movement.Batch.Movements.All(move => move.Sequence > receipt.MovementSequenceBefore && move.Sequence <= receipt.MovementSequenceAfter && (move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(root.OwnerSeat) && move.Reason.Value == "skill-program.shanhe-source-biri-14039.draw" || move.From == CardLocation.DiscardPile && move.To == CardLocation.DrawPile && move.Reason == CardMoveReasons.Reshuffle));
        // Shared native draw replacement and exact skill/state changes retain
        // their own return checks rather than borrowing an unrelated root.
        return receipt.Stage == ShanheSourceBiriStage.DrawChildren && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, root) || _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(root, child);
    }

    internal bool ShanheSourceBiriObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        if (parent is ProgramSkillFrame { ShanheSourceBiri: not null } root)
            return ShanheSourceBiriFirstChild(root, child);
        if (parent is RecoveryReplacementFrame recovery)
        {
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            if (child is HpChangedTriggerWindowFrame hp)
                return _coreTypedRecoveryDamageCursorOperations.TypedRecoveryHpReturnMatches(recovery, hp, history);
            if (child is CardsMovedTriggerWindowFrame moved)
                return _coreTypedRecoveryDamageCursorOperations.TypedRecoveryRewardReturnMatches(recovery, moved, history);
        }

        return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(parent, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    }

    internal ProgramSkillFrame? ShanheSourceBiriObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => root.ShanheSourceBiri is not null && _coreShanheSourceBiriObserversOperations.ShanheSourceBiriFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreShanheSourceBiriObserversOperations.ShanheSourceBiriObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 62047)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 62047)]
    internal bool IsShanheSourceBiriRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.ShanheSourceBiri is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == recovery.Id);
        return parent >= 0 && child > parent && ShanheSourceBiriFirstChild(frame, _resolutionStackOperations._resolutionStack[parent + 1]) && _coreObserverSubtreesOperations.HasObserverSuffix(parent, (index) => _coreShanheSourceBiriObserversOperations.ShanheSourceBiriObserverEdge(index), (index, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index, dying)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 62047)]
    internal bool IsShanheSourceBiriObserverDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ShanheSourceBiriObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 62047)]
    internal bool AdvanceShanheSourceBiriChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShanheSourceBiriObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
