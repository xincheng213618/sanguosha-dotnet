#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiangWeiMouMobileRuntime
{
    internal bool ValidMouJw(ProgramSkillFrame f)
    {
        if (f.MouJiangWei is not { } r || f.InstructionIndex != 1 || r.Source != MouJwSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || _coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiStartedEvent>().Count(e => e == new MouJiangWeiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.Phase)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouJiangWeiTiaoxin)
        {
            if (f.SkillId != MouJiangWeiComposition.Tiaoxin || f.ActivationId != "taunt" || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } p || p.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) || r.Budget is < 0 or > 4 || r.Targets.Count > r.Budget || r.Targets.Contains(f.OwnerSeat) || r.Cursor < 0 || r.Cursor > r.Targets.Count)
                return false;
            if (r.Accepted && (_coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiSelectionClosedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Count == r.Targets.Count) != 1 || r.Targets.Where((s, i) => !_coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiTargetAcceptedEvent>().Any(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.Index == i && e.TargetSeat == s)).Any()))
                return false;
            if (!r.Accepted && r.Stage != MouJiangWeiStage.Targets)
                return false;
        }
        else if (r.Op != SkillProgramEffectOp.ResolveMouJiangWeiZhiji || f.SkillId != MouJiangWeiComposition.Zhiji || f.TriggerId != "awakening" || f.ActivationId != f.TriggerId || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Preparation } prep || prep.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(prep) || f.WindowContext?.ParentFrameId != r.WindowId || !_resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().Any(w => w.Id == r.WindowId && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex])) || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiAwakenedEvent>().Any(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash))
            return false;
        if (r.Payment is { } paid && (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == paid.CardId && m.CardKind == paid.Kind && m.From == paid.From && m.To == paid.To && m.Sequence == paid.Sequence && m.Reason.Value == MouJwGiftReason) || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouJiangWeiGiftPaidEvent>().Any(e => e.FrameId == f.Id && e.Index == r.Cursor && e.Payment == paid)))
            return false;
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 33200)]
    internal bool MouJwMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => ValidMouJw(f) && p == f.PendingMovementContinuation && e?.Op == f.MouJiangWei!.Op && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && f.MouJiangWei.Stage is MouJiangWeiStage.GiftChildren or MouJiangWeiStage.AwakeningChildren;
    internal bool MouJwLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouJw(f) || f.MouJiangWei is not { Stage: MouJiangWeiStage.GiftChildren, Payment: { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } p } r)
            return false;
        var seat = p.From.OwnerSeat!.Value;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouJwGiftReason && recovery.Attempt.SourceSeat == seat && recovery.Attempt.TargetSeat == seat;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == seat && hp.Change.TargetSeat == seat && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == seat && e.Reason.Value == MouJwGiftReason && e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouJwFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouJw(f))
            return false;
        var r = f.MouJiangWei!;
        if (r.Stage == MouJiangWeiStage.NativeUse && child is CardUseFrame use)
            return ValidMouJwUse(use, f);
        if (MouJwLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouJwNativeEdge(int n)
    {
        if (_resolutionStackOperations._resolutionStack[n - 1] is CardUseFrame { MouJiangWeiRequest: { } o } use && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == o.FrameId)is { } parent && ValidMouJwUse(use, parent) && _resolutionStackOperations._resolutionStack[n] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[n - 1] is ProgramCardTriggerWindowFrame w && _coreCardUseLifecycleOperations.LifecycleCardUse(w.ParentFrameId)is { MouJiangWeiRequest: not null } && _resolutionStackOperations._resolutionStack[n] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(w, w.Candidates[w.CandidateIndex]);
        return false;
    }

    internal bool MouJwSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _characterJiangWeiMouMobileOperations.MouJwNativeEdge(i) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame f && _characterJiangWeiMouMobileOperations.MouJwFirstChild(f, _resolutionStackOperations2._resolutionStack()[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouJwObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterJiangWeiMouMobileOperations.MouJwFirstChild(f, child), (n) => _characterJiangWeiMouMobileOperations.MouJwSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 33200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 33200)]
    internal bool MouJwDamage(long id) => MouJwObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 33200)]
    internal bool MouJwBeforeDamage(long id) => MouJwObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 33200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 33200)]
    internal bool MouJwDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouJwObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 33200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 33200)]
    internal bool MouJwRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidMouJw(f))
            return false;
        if (MouJwLionChild(f, child) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return a >= 0 && b > a + 1 && MouJwFirstChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && MouJwSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 33200)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 33200)]
    internal bool KeepMouJwPaid(ProgramSkillFrame f) => f.MouJiangWei is { Accepted: true } && ValidMouJw(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 33200)]
    internal bool ResumeMouJw(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouJiangWei: not null } f || f.Id != id)
            return false;
        ContinueMouJw(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 33200)]
    internal bool ReturnMouJwMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouJw(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 33200)]
    internal bool AdvanceMouJwChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouJwObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 33200)]
    internal void AssertMouJw(ProgramSkillFrame f)
    {
        if (f.MouJiangWei is not null && !ValidMouJw(f))
            throw new InvalidOperationException("Mobile Jiang Wei lost its exact activation, charge reservation or typed child return.");
    }
}
