#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRandomAreaAcquisitionObserversRuntime
{
    internal bool RandomAreaFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.RandomAreaAcquisition is not { } r || !_coreRandomAreaAcquisitionProofsOperations.ValidRandomAreaAcquisition(root))
            return false;
        if (_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(root, child))
            return true;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Count(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window) == 1;
        if (r.Stage != RandomAreaAcquisitionStage.MovementChildren || r.Steps.Count == 0)
            return false;
        var step = r.Steps[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId != root.Id || moved.ResumeProgramFrameId is not null || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.NativeDrawInvocation is not null)
                return false;
            if (moved.Batch.Id == step.MovementBatchId)
                return moved.Batch.Movements.SequenceEqual(step.Movements);
            // Only the exact paid loss of this Wooden Ox can own its native grain discard.
            return moved.Batch.ParentBatchId == step.MovementBatchId && step.Movements.Any(m => m.CardKind == CardKind.WoodenOx && m.From == CardLocation.Equipment(step.TargetSeat)) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > step.SequenceAfter && m.From == CardLocation.WoodenOxGrain(step.TargetSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
        }

        var lion = step.Movements.Where(m => m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(step.TargetSeat)).ToArray();
        if (lion.Length != 1)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == step.TargetSeat && recovery.Attempt.TargetSeat == step.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == global::CardGame.Core.EngineOwners.CoreGameEngineRandomAreaAcquisitionRuntime.RandomAreaReason(root, step.SeatIndex) && _coreTypedRecoveryDamageCursorOperations.TypedRecoveryApplicationBoundsMatch(recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == step.TargetSeat && hp.Change.TargetSeat == step.TargetSeat && hp.Change.Amount == 1;
    }

    internal bool RandomAreaObserverEdge(int index)
    {
        var parent = _resolutionStackOperations._resolutionStack[index - 1];
        var child = _resolutionStackOperations._resolutionStack[index];
        if (parent is ProgramSkillFrame { RandomAreaAcquisition: not null } root)
            return RandomAreaFirstChild(root, child);
        if (parent is RecoveryReplacementFrame recovery)
        {
            var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
            if (child is HpChangedTriggerWindowFrame hp)
                return _coreTypedRecoveryDamageCursorOperations.TypedRecoveryHpReturnMatches(recovery, hp, history);
            if (child is CardsMovedTriggerWindowFrame moved)
                return _coreTypedRecoveryDamageCursorOperations.TypedRecoveryRewardReturnMatches(recovery, moved, history);
        }

        return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, parent) || global::CardGame.Core.EngineOwners.CoreGameEngineCharacterTurnedOverProgramsRuntime.CharacterTurnedOverFrameRidesOn(child, parent) || _coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(parent, child) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child) || _coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(index);
    }

    internal ProgramSkillFrame? RandomAreaObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreRandomAreaAcquisitionObserversOperations.RandomAreaFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreRandomAreaAcquisitionObserversOperations.RandomAreaObserverEdge(child), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 10)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 10)]
    internal bool IsRandomAreaRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery) => frame.RandomAreaAcquisition is not null && RandomAreaFirstChild(frame, recovery);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 2010)]
    internal bool HasRandomAreaDamageObserver(long id) => RandomAreaObserverRoot()is { } root && (root.RandomAreaAcquisition!.DamageWindowId == id || _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamagePrelude, 20)]
    internal bool HasRandomAreaBeforeDamageObserver(long id) => RandomAreaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 20, group: 30)]
    internal bool IsRandomAreaDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && RandomAreaObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 1245)]
    internal bool AdvanceRandomAreaChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && RandomAreaObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
