#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGanNingMouMobileRuntime
{
    internal bool ValidMouGn(ProgramSkillFrame f)
    {
        if (f.MouGanNing is not { } r || r.Source != MouGnSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Wrong is < 0 or > 4 || r.Discarded < 0 || r.Discarded > r.Wrong || r.Deposited is < 0 or > 3 || r.GuessedMask is < 0 or > 15 || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingStartedEvent>().Count(e => e == new MouGanNingStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.Phase, r.TargetSeat, r.UseId, r.ActionId)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouGanNingWeiTarget)
        {
            if (f.SkillId != MouGanNingMobileComposition.Fenwei || f.TriggerId != "spend-wei" || f.ActivationId != f.TriggerId || r.Phase is not null || f.WindowContext is not { } c || MouGnParent(c)is not { } parent || parent.Id != r.UseId || parent.Action!.ActionId != r.ActionId || r.Targets.Count == 0 || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(t => !parent.TargetSeats.Contains(t)) || r.TargetIndex < 0 || r.TargetIndex >= r.Targets.Count)
                return false;
        }
        else if (f.TriggerId is not null || f.WindowContext is not null || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || (r.Op == SkillProgramEffectOp.ResolveMouGanNingQixi ? f.SkillId != MouGanNingMobileComposition.Qixi || f.ActivationId != "guess-suits" || !f.SelectedTargetSeats.SequenceEqual([r.TargetSeat]) : f.SkillId != MouGanNingMobileComposition.Fenwei || f.ActivationId != "place-wei" || f.SelectedTargetSeats.Count != 0))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingMovementEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To && m.Reason == e.Reason) == 1) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingDiscardedEvent>().Count(e => e.FrameId == f.Id) == r.Discarded && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingWeiPlacedEvent>().Count(e => e.FrameId == f.Id && e.To.Zone == CardZoneKind.PublicPersistentPile && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.To == e.To && m.From == e.From)) == r.Deposited;
    }

    internal void SealMouGn(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouGanNing!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGanNing = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouGanNingMovementEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMouGn(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MouGnLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGn(f) || f.MouGanNing is not { Stage: MouGanNingStage.MovementChildren } r)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingMovementEvent>().LastOrDefault(e => e.FrameId == f.Id && e.Sequence > r.Before && e.Kind == CardKind.SilverLion && e.From.Zone == CardZoneKind.Equipment);
        if (paid?.From.OwnerSeat is not { } owner)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == paid.Reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == owner && recovery.Attempt.TargetSeat == owner;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == owner && hp.Change.TargetSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == owner && e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.Reason == paid.Reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MouGnFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGn(f))
            return false;
        var r = f.MouGanNing!;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouGanNingStage.Drawing && f.NativeDraw is not null;
        if (MouGnLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGanNingMovementEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouGnSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && this.MouGnFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouGnObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MouGnFirstChild(f, child), (i) => this.MouGnSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31900)]
    internal bool IsMouGnDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGnObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31900)]
    internal bool MouGnRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouGn(f))
            return false;
        if (MouGnLionChild(f, recovery) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i + 1 && MouGnFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouGnSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31900)]
    internal bool KeepMouGnAccepted(ProgramSkillFrame f) => f.MouGanNing is { Accepted: true } && ValidMouGn(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 31900)]
    internal bool ResumeMouGn(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGanNing: not null } f || f.Id != id)
            return false;
        ContinueMouGn(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31900)]
    internal bool ReturnMouGnMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouGn(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31900)]
    internal bool AdvanceMouGnChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGnObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);

    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMouGanNingWei(int seat, IReadOnlyList<PlayerMarkerSnapshot>? ordinary) => MouGnWei(seat)is not null ? Array.AsReadOnly((ordinary ?? []).Where(m => m.Kind != PlayerMarkerKind.MouGanNingWei).Append(new PlayerMarkerSnapshot(PlayerMarkerKind.MouGanNingWei, "威", 1)).OrderBy(m => m.Kind).ToArray()) : ordinary;
}
