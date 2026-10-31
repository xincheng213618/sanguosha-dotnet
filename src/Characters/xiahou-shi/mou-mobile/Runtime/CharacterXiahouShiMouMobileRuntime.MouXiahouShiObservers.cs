#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiahouShiMouMobileRuntime
{
    internal void SealMouXiahouShiMovements(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = f.MouXiahouShiYanyu is { } y ? y with { After = after } : null, MouXiahouShiQiaoshi = f.MouXiahouShiQiaoshi is { } q ? q with { After = after } : null });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after))
        {
            var fact = new MouXiahouShiMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouXiahouShiMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool MouXiahouShiMovementSigned(ProgramSkillFrame f, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXiahouShiMovementSignedEvent>().Count(e => e == new MouXiahouShiMovementSignedEvent(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)) == 1;
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 390)]
    internal bool IsMouXiahouShiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation pending) => e is not null && MouXiahouShiComposition.Handles(e.Op) && ValidMouXiahouShi(f) && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && (f.MouXiahouShiYanyu?.Stage is MouXiahouShiYanyuStage.DiscardChildren or MouXiahouShiYanyuStage.DrawChildren || f.MouXiahouShiQiaoshi?.Stage is MouXiahouShiQiaoshiStage.RecoveryChildren or MouXiahouShiQiaoshiStage.DrawChildren);
    internal bool DrainMouXiahouShiChildren(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MouXiahouShiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouXiahouShi(f))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return f.NativeDraw is not null && (f.MouXiahouShiYanyu?.Stage == MouXiahouShiYanyuStage.Drawing || f.MouXiahouShiQiaoshi?.Stage == MouXiahouShiQiaoshiStage.Drawing);
        if (child is RecoveryReplacementFrame recovery)
            return IsMouXiahouShiDirectRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp && f.MouXiahouShiQiaoshi is { Stage: MouXiahouShiQiaoshiStage.RecoveryChildren, Branch: "recover-and-draw" } q)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(f, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()) && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == q.DamageSourceSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.HpBefore == q.RecoveryHpBefore;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && IsMouXiahouShiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
        {
            var before = f.MouXiahouShiYanyu?.Before ?? f.MouXiahouShiQiaoshi!.Before;
            var after = f.MouXiahouShiYanyu?.After ?? f.MouXiahouShiQiaoshi!.After;
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > before && m.Sequence <= after && MouXiahouShiMovementSigned(f, m));
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool IsMouXiahouShiDirectRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.MouXiahouShiQiaoshi is { Stage: MouXiahouShiQiaoshiStage.RecoveryChildren, Branch: "recover-and-draw", RecoveryReturned: false } q && ValidMouXiahouShiQiaoshi(f) && recovery.ParentFrameId == f.Id && recovery.Attempt.SourceSeat == q.DamageSourceSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.HpBefore == q.RecoveryHpBefore && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
    internal bool MouXiahouShiObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && MouXiahouShiFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool MouXiahouShiObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame dying && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterXiahouShiMouMobileOperations.MouXiahouShiObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouXiahouShiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterXiahouShiMouMobileOperations.MouXiahouShiFirstChild(f, c), (n) => _characterXiahouShiMouMobileOperations.MouXiahouShiObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1370)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1480)]
    internal bool HasMouXiahouShiDamageObserver(long id) => MouXiahouShiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1500)]
    internal bool HasMouXiahouShiBeforeDamageObserver(long id) => MouXiahouShiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 760)]
    internal bool IsMouXiahouShiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouXiahouShiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 540)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 530)]
    internal bool IsMouXiahouShiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (IsMouXiahouShiDirectRecoveryParent(f, recovery))
            return true;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return index >= 0 && child > index + 1 && MouXiahouShiFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && MouXiahouShiObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    internal void ContinueMouXiahouShi(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.MouXiahouShiYanyu is null && f.MouXiahouShiQiaoshi is null)
            return;
        AssertMouXiahouShi(f);
        if (f.MouXiahouShiYanyu?.Stage is MouXiahouShiYanyuStage.TargetChoice or MouXiahouShiYanyuStage.Drawing || f.MouXiahouShiQiaoshi?.Stage is MouXiahouShiQiaoshiStage.SourceChoice or MouXiahouShiQiaoshiStage.Drawing)
            return;
        if (DrainMouXiahouShiChildren(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouXiahouShi(f);
            return;
        }

        if (f.MouXiahouShiYanyu is { Stage: MouXiahouShiYanyuStage.DiscardChildren } y)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            {
                FinishMouXiahouShi(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiYanyu = y with { Stage = MouXiahouShiYanyuStage.Drawing }, PendingMovementContinuation = null });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, new(MouXiahouShiDrawReason));
            return;
        }

        if (f.MouXiahouShiQiaoshi is { Stage: MouXiahouShiQiaoshiStage.RecoveryChildren, Branch: "recover-and-draw" } q)
        {
            if (q.RecoveryReturned)
                throw new InvalidOperationException("Qiaoshi cannot resume an already returned recovery twice.");
            var requests = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramRecoveryRequestedEvent>().Where(e => e.ProgramFrameId == f.Id && e.InstructionIndex == f.InstructionIndex).ToArray();
            var direct = _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramRecoveryAppliedEvent>().Where(e => e.ProgramFrameId == f.Id && e.Receipt.InstructionIndex == f.InstructionIndex).ToArray();
            if (q.BaseRecovery > 0 && requests.Length + direct.Length != 1 || q.BaseRecovery == 0 && requests.Length + direct.Length != 0 || requests.Any(e => e.GameplayHash != f.GameplayHash || e.SourceSeat != q.DamageSourceSeat || e.TargetSeat != f.OwnerSeat || e.HpBefore != q.RecoveryHpBefore) || direct.Any(e => e.Receipt.GameplayHash != f.GameplayHash || e.Receipt.SourceSeat != q.DamageSourceSeat || e.Receipt.TargetSeat != f.OwnerSeat || e.Receipt.HpBefore != q.RecoveryHpBefore))
                throw new InvalidOperationException("Qiaoshi must return its single genuine native recovery attempt before drawing.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiQiaoshi = q with { RecoveryReturned = true }, PendingMovementContinuation = null });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXiahouShiQiaoshiRecoveryReturnedEvent(f.Id, q.DamageSourceSeat, f.OwnerSeat, q.RequestedRecovery));
            if (!_coreMatchParticipantsPlayersAccess._players[q.DamageSourceSeat].IsAlive)
            {
                FinishMouXiahouShi(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouXiahouShiQiaoshi = f.MouXiahouShiQiaoshi!with { Stage = MouXiahouShiQiaoshiStage.Drawing } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, q.DamageSourceSeat, 2, new(MouXiahouShiQiaoshiDrawReason));
            return;
        }

        if (f.MouXiahouShiYanyu?.Stage != MouXiahouShiYanyuStage.DrawChildren && f.MouXiahouShiQiaoshi?.Stage != MouXiahouShiQiaoshiStage.DrawChildren)
            throw new InvalidOperationException("Mou Xiahou Shi cannot advance an unreturned native effect.");
        FinishMouXiahouShi(f);
    }

    internal void AssertMouXiahouShi(ProgramSkillFrame f)
    {
        if ((f.MouXiahouShiYanyu is not null || f.MouXiahouShiQiaoshi is not null) && !ValidMouXiahouShi(f))
            throw new InvalidOperationException("Mou Xiahou Shi lost its exact source, actual boundary or paid native receipt.");
    }
    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 29800)]
    internal void AssertMouXiahouShiFrames()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
            AssertMouXiahouShi(f);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29800)]
    internal bool PermitAcceptedMouXiahouShi(ProgramSkillFrame f) => PermitIssuedMouXiahouShi(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 29800)]
    internal bool ResumeMouXiahouShi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.MouXiahouShiYanyu is null && f.MouXiahouShiQiaoshi is null)
            return false;
        ContinueMouXiahouShi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 29800)]
    internal bool ReturnMouXiahouShiMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouXiahouShi(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29800)]
    internal bool AdvanceMouXiahouShiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouXiahouShiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
