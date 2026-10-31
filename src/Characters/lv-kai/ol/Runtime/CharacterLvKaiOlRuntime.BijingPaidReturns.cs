#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLvKaiOlRuntime
{
    internal void ContinueBijingPaidGroup(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BijingPunish: { Paid: true } } frame || frame.Id != id)
            return;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { BijingPunish = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 7171)]
    internal bool ResumeBijingPaidGroup(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BijingPunish.Paid: true } frame || frame.Id != id)
            return false;
        ContinueBijingPaidGroup(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 7171)]
    internal bool ReturnBijingPaidMovement(ProgramSkillFrame frame) => frame.BijingPunish is { Paid: true } && ResumeBijingPaidGroup(frame.Id);
    internal bool BijingLionChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.BijingPunish is not { Paid: true } state || !state.SelectedCardIds.Select((id, i) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind == CardKind.SilverLion && state.SelectedLocations[i] == CardLocation.Equipment(state.TurnOwnerSeat)).Any(lion => lion))
            return false;
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.bijing-punish");
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, frame) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == state.TurnOwnerSeat && recovery.Attempt.TargetSeat == state.TurnOwnerSeat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == frame.Id && hp.ResumeFrameId == frame.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == state.TurnOwnerSeat && hp.Change.TargetSeat == state.TurnOwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == state.TurnOwnerSeat && e.Reason == reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool BijingFirstPaidChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.BijingPunish is not { Paid: true } state)
            return false;
        if (BijingLionChild(frame, child))
            return true;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(frame, moved) && moved.Batch.Movements.All(m => m.Sequence > state.Before && m.Sequence <= state.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(frame, hp);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == frame.Id && (lifecycle.Window == SkillProgramTriggerWindow.SkillsChanged && lifecycle.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == lifecycle.Id && e.Change.ParentFrameId == frame.Id));
    }

    internal ProgramSkillFrame? BijingPaidObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.BijingFirstPaidChild(f, child), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 7171)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 7171)]
    internal bool IsBijingRecoveryParent(ProgramSkillFrame frame, RecoveryReplacementFrame recovery)
    {
        if (frame.BijingPunish is not { Paid: true })
            return false;
        if (BijingLionChild(frame, recovery) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(frame, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == recovery.Id);
        return index >= 0 && child > index + 1 && BijingFirstPaidChild(frame, _resolutionStackOperations._resolutionStack[index + 1]) && BijingPaidObserverRoot()?.Id == frame.Id && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 7171)]
    internal bool IsBijingPaidDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && BijingPaidObserverRoot()is { } frame && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 7171)]
    internal bool AdvanceBijingPaidChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BijingPaidObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 7171)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 7171)]
    internal bool KeepPaidBijingGroup(ProgramSkillFrame frame) => frame.BijingPunish is { Paid: true };
}
