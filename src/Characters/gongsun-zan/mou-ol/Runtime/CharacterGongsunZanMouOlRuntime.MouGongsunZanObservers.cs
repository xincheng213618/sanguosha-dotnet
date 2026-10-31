#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGongsunZanMouOlRuntime
{
    internal bool ValidMouGsz(ProgramSkillFrame f)
    {
        if (f.MouGongsunZan is not { } r || r.Source != MouGszSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.OriginalTarget) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        if (f.SkillId == MouGongsunZanComposition.Baojing)
            return f.TriggerId is null && f.ActivationId == "adjust-range" && r.Stage == MouGongsunZanStage.RangeChoice && r.UseId == 0 && r.ActionId == 0 && r.WindowId == 0 && f.SelectedTargetSeats.SequenceEqual([r.OriginalTarget]) && r.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } phase && phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase);
        return f.SkillId == MouGongsunZanComposition.Jiaodi && MouGszUse(f)is { } use && use.Id == r.UseId && use.Action!.ActionId == r.ActionId && use.TargetSeats.Contains(r.OriginalTarget) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanStartedEvent>().Count(e => e == new MouGongsunZanStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.UseId, r.ActionId, r.OriginalTarget, r.OwnerRange, r.TargetRange)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMovedEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason == e.Reason && m.Sequence == e.Sequence) == 1);
    }

    internal void SealMouGsz(long id, string purpose)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouGongsunZan!;
        var after = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGongsunZan = r with { After = after } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= after).ToArray())
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGongsunZanMovedEvent(id, purpose, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence));
    }

    internal bool DrainMouGsz(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 32700)]
    internal bool MouGszMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveMouGongsunZanJiaodi && ValidMouGsz(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && f.MouGongsunZan!.Stage is MouGongsunZanStage.ObtainChildren or MouGongsunZanStage.DiscardChildren;
    internal bool MouGszLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGsz(f) || f.MouGongsunZan is not { Stage: MouGongsunZanStage.DiscardChildren } r)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMovedEvent>().LastOrDefault(e => e.FrameId == f.Id && e.PrintedKind == CardKind.SilverLion && e.From.Zone == CardZoneKind.Equipment && e.Reason.Value == MouGszDiscardReason && e.Sequence > r.Before && e.Sequence <= r.After);
        if (paid?.From.OwnerSeat is not { } owner)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == paid.Reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == owner && recovery.Attempt.TargetSeat == owner;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == owner && hp.Change.TargetSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == owner && e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.Reason == paid.Reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouGszFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGsz(f) || f.MouGongsunZan!.Stage is not (MouGongsunZanStage.ObtainChildren or MouGongsunZanStage.DiscardChildren))
            return false;
        var r = f.MouGongsunZan!;
        if (MouGszLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGongsunZanMovedEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Sequence == m.Sequence && e.From == m.From && e.To == m.To && e.Reason == m.Reason));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouGszSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterGongsunZanMouOlOperations.MouGszFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouGszObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGongsunZanMouOlOperations.MouGszFirstChild(f, c), (i) => _characterGongsunZanMouOlOperations.MouGszSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 32700)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 32700)]
    internal bool MouGszDamage(long id) => MouGszObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 32700)]
    internal bool MouGszBeforeDamage(long id) => MouGszObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 32700)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 32700)]
    internal bool MouGszDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGszObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 32700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 32700)]
    internal bool MouGszRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidMouGsz(f))
            return false;
        if (MouGszLionChild(f, child) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return a >= 0 && b > a + 1 && MouGszFirstChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && MouGszSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32700)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 32700)]
    internal bool KeepAcceptedMouGsz(ProgramSkillFrame f) => f.SkillId == MouGongsunZanComposition.Jiaodi && ValidMouGsz(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 32700)]
    internal bool ResumeMouGsz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGongsunZan: not null } f || f.Id != id)
            return false;
        ContinueMouGsz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 32700)]
    internal bool ReturnMouGszMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouGsz(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 32700)]
    internal bool AdvanceMouGszChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGszObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 32700)]
    internal void AssertMouGsz(ProgramSkillFrame f)
    {
        if (f.MouGongsunZan is not null && !ValidMouGsz(f))
            throw new InvalidOperationException("Gongsun Zan lost his accepted native designation or paid original movement child.");
    }
}
