#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangMouMobileRuntime
{
    internal void SealMouHanDangMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouHanDang!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouHanDang = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouHanDangMovementSignedEvent(id, m.CardId, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 30700)]
    internal bool IsMouHanDangMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op is SkillProgramEffectOp.ResolveMouHanDangGongqi or SkillProgramEffectOp.ResolveMouHanDangJiefan && f.MouHanDang is { Stage: MouHanDangStage.DiscardChildren or MouHanDangStage.DrawChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidMouHanDang(f);
    internal bool MouHanDangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouHanDang(f) || f.MouHanDang is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouHanDangStage.Drawing && f.NativeDraw is not null;
        if (IsMouHanDangSilverLionChild(f, child))
            return true;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && IsMouHanDangMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangMovementSignedEvent>().Count(e => e == new MouHanDangMovementSignedEvent(f.Id, m.CardId, m.From, m.To, m.Reason, m.Sequence)) == 1);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool IsMouHanDangSilverLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouHanDang is not { Stage: MouHanDangStage.DiscardChildren, PaidCardId: { } id } r || !ValidMouHanDang(f) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouHanDangDiscardedEvent>().SingleOrDefault(e => e.FrameId == f.Id && e.Index == r.Index && e.CardId == id && e.Sequence == r.PaymentSequence)is not { From.Zone: CardZoneKind.Equipment } paid || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == id && m.CardKind == CardKind.SilverLion && m.From == paid.From && m.To == paid.To && m.Reason.Value == MouHanDangDiscardReason))
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouHanDangDiscardReason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == paid.Seat && recovery.Attempt.TargetSeat == paid.Seat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount > 0 && hp.Change.SourceSeat == paid.Seat && hp.Change.TargetSeat == paid.Seat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == paid.Seat && e.Reason.Value == MouHanDangDiscardReason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouHanDangObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouHanDangFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouHanDangObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterHanDangMouMobileOperations.MouHanDangObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouHanDangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHanDangMouMobileOperations.MouHanDangFirstChild(f, c), (n) => _characterHanDangMouMobileOperations.MouHanDangObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 30700)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 30700)]
    internal bool HasMouHanDangDamageObserver(long id) => MouHanDangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 30700)]
    internal bool HasMouHanDangBeforeDamageObserver(long id) => MouHanDangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 30700)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30700)]
    internal bool IsMouHanDangDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouHanDangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30700)]
    internal bool IsMouHanDangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (IsMouHanDangSilverLionChild(f, recovery))
            return true;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouHanDangFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouHanDangObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 30700)]
    internal bool ResumeMouHanDang(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouHanDang: not null } f || f.Id != id)
            return false;
        ContinueMouHanDang(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30700)]
    internal bool ReturnMouHanDangMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouHanDang(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30700)]
    internal bool AdvanceMouHanDangChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouHanDangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30700)]
    internal bool ContinueAcceptedMouHanDang(ProgramSkillFrame f) => ValidMouHanDang(f) && f.MouHanDang is { Gongqi: false, Branch: not null } or { Gongqi: true, PaidCardId: not null };
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 30700)]
    internal void AssertMouHanDang(ProgramSkillFrame f)
    {
        if (f.MouHanDang is not null && !ValidMouHanDang(f))
            throw new InvalidOperationException("Mou Han Dang lost its paid source or native effect cursor.");
    }
}
