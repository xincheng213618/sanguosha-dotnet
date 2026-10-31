#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMouMobileRuntime
{
    internal bool ValidMouLb(ProgramSkillFrame f) => f.MouLuBuLiyu is { } r && r.Source == MouLbSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && f.TriggerId == "obtain-draw-and-choose" && f.ActivationId == f.TriggerId && f.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied } c && c.ParentFrameId == r.WindowId && c.DamageFrameId == r.DamageFrameId && c.SourceSeat == f.OwnerSeat && c.TargetSeat == r.VictimSeat && c.Amount == r.DamageAmount && Enum.IsDefined(r.Stage) && r.DamageAmount > 0 && r.Selections >= 0 && r.Selections <= r.DamageAmount && r.ActualObtained >= 0 && r.ActualObtained <= r.Selections && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.CategoryMask is >= 0 and <= 7 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuStartedEvent>().Count(e => e == new MouLuBuLiyuStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.DamageFrameId, r.OuterUseId, r.OuterActionId, r.VictimSeat, r.DamageAmount, r.ActualTurn)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuObtainedEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason.Value == MouLbObtainReason) == 1) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuObtainedEvent>().Count(e => e.FrameId == f.Id && e.ActualGain) == r.ActualObtained;
    internal void SealMouLb(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuBuLiyu = f.MouLuBuLiyu!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var e = new MouLuBuLiyuMovementEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuMovementEvent>().Contains(e))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(e);
        }
    }

    internal bool DrainMouLb(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 31600)]
    internal bool MouLbMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveMouLuBuLiyu && ValidMouLb(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouLbLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouLuBuLiyu is not { Stage: MouLuBuLiyuStage.ObtainChildren } || !ValidMouLb(f))
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuObtainedEvent>().LastOrDefault(e => e.FrameId == f.Id && e.PrintedKind == CardKind.SilverLion && e.From.Zone == CardZoneKind.Equipment);
        if (paid?.From.OwnerSeat is not { } owner || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == paid.Sequence)is not { } movement)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == movement.Reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == owner && recovery.Attempt.TargetSeat == owner;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == owner && hp.Change.SourceSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == owner && e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.Reason == movement.Reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouLbFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouLb(f))
            return false;
        var r = f.MouLuBuLiyu!;
        if (child is CardUseFrame use)
            return r.Stage == MouLuBuLiyuStage.DuelIssued && use.DamageTargetDuelOrigin is { } o && MatchesMouLbDuelParent(f, o);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouLuBuLiyuStage.Drawing && f.NativeDraw is not null;
        if (MouLbLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuMovementEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouLbNativeEdge(int index)
    {
        if (_resolutionStackOperations._resolutionStack[index - 1] is CardUseFrame use && use.DamageTargetDuelOrigin is { } o && IsMouLbDuelOrigin(o) && _resolutionStackOperations._resolutionStack[index] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)?.DamageTargetDuelOrigin is { } origin && IsMouLbDuelOrigin(origin) && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame child && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(child, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && child.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext(parent, c);
        }

        return false;
    }

    internal bool MouLbObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterLuBuMouMobileOperations.MouLbNativeEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterLuBuMouMobileOperations.MouLbFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouLbObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterLuBuMouMobileOperations.MouLbFirstChild(f, child), (n) => _characterLuBuMouMobileOperations.MouLbObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 31600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 31600)]
    internal bool MouLbDamageObserver(long id) => MouLbObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 31600)]
    internal bool MouLbBeforeDamageObserver(long id) => MouLbObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 31600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31600)]
    internal bool MouLbDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouLbObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31600)]
    internal bool MouLbRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouLb(f))
            return false;
        if (MouLbLionChild(f, recovery) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i + 1 && MouLbFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouLbObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 31600)]
    internal void AssertMouLb(ProgramSkillFrame f)
    {
        if (f.MouLuBuLiyu is not null && !ValidMouLb(f))
            throw new InvalidOperationException("Mobile Liyu lost its actual damage, issued cost or native child receipt.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31600)]
    internal bool KeepPaidMouLb(ProgramSkillFrame f) => f.MouLuBuLiyu is { Paid: true } && ValidMouLb(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 31600)]
    internal bool ResumeMouLb(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuBuLiyu: not null } f || f.Id != id)
            return false;
        ContinueMouLb(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31600)]
    internal bool ReturnMouLbMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouLb(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31600)]
    internal bool AdvanceMouLbChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouLbObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
