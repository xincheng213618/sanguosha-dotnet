#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanMouMobileRuntime
{
    internal bool DrainMouDiaoChanChildren(ProgramSkillFrame f)
    {
        return _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id);
    }

    internal void SealMouDiaoChanMovement(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouDiaoChanMobile!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouDiaoChanMobile = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouDiaoChanMobileMovementEvent(id, m.CardId, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }
    internal bool IsMouDiaoChanMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op is SkillProgramEffectOp.ResolveMouDiaoChanLijian or SkillProgramEffectOp.ResolveMouDiaoChanBiyue && f.MouDiaoChanMobile is { Stage: MouDiaoChanMobileStage.CostChildren or MouDiaoChanMobileStage.DrawChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMouDiaoChan(f);
    internal bool MouDiaoChanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouDiaoChan(f) || f.MouDiaoChanMobile is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouDiaoChanMobileStage.Drawing && f.NativeDraw is not null;
        if (IsMouDiaoChanSilverLionChild(f, child))
            return true;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && IsMouDiaoChanMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileMovementEvent>().Count(e => e == new MouDiaoChanMobileMovementEvent(f.Id, m.CardId, m.From, m.To, m.Reason, m.Sequence)) == 1);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool IsMouDiaoChanSilverLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouDiaoChanMobile is not { Stage: MouDiaoChanMobileStage.CostChildren } r || !ValidMouDiaoChan(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouDiaoChanMobileCostEvent>().LastOrDefault(e => e.FrameId == f.Id && e.Kind == CardKind.SilverLion && e.From == CardLocation.Equipment(f.OwnerSeat))is not { } paid || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.CardKind == CardKind.SilverLion && m.From == paid.From && m.To == paid.To && m.Reason.Value == MouDiaoChanCostReason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouDiaoChanCostReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == f.OwnerSeat && e.Reason.Value == MouDiaoChanCostReason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouDiaoChanObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouDiaoChanFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouDiaoChanObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterDiaoChanMouMobileOperations.MouDiaoChanObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouDiaoChanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDiaoChanMouMobileOperations.MouDiaoChanFirstChild(f, c), (n) => _characterDiaoChanMouMobileOperations.MouDiaoChanObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31700)]
    internal bool IsMouDiaoChanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouDiaoChanObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31700)]
    internal bool IsMouDiaoChanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (IsMouDiaoChanSilverLionChild(f, recovery))
            return true;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouDiaoChanFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouDiaoChanObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 31700)]
    internal bool ResumeMouDiaoChan(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouDiaoChanMobile: not null } f || f.Id != id)
            return false;
        ContinueMouDiaoChan(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31700)]
    internal bool ReturnMouDiaoChanMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouDiaoChan(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31700)]
    internal bool AdvanceMouDiaoChanChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDiaoChanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31700)]
    internal bool ContinueAcceptedMouDiaoChan(ProgramSkillFrame f) => ValidMouDiaoChan(f) && f.MouDiaoChanMobile is { Paid: true };
}
