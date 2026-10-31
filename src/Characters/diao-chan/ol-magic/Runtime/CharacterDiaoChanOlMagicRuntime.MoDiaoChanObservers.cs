#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDiaoChanOlMagicRuntime
{
    internal bool ValidMoDc(ProgramSkillFrame f) => f.MoDiaoChan is { } r && r.Source == MoDcSource(f) && r.GameplayHash == f.GameplayHash && MoDiaoChanComposition.Handles(r.Op) && f.InstructionIndex == 1 && f.TriggerId is not null && f.TriggerId == f.ActivationId && f.WindowContext?.ParentFrameId == r.WindowId && Enum.IsDefined(r.Stage) && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && r.Cards.Distinct().Count() == r.Cards.Count && r.Targets.Distinct().Count() == r.Targets.Count && r.Cards is System.Collections.IList { IsReadOnly: true } && r.Targets is System.Collections.IList { IsReadOnly: true } && _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanStartedEvent>().Count(e => e == new MoDiaoChanStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.WindowId, r.ActualTurn, r.Round)) == 1 && (r.Op != SkillProgramEffectOp.ResolveMoDiaoChanHuanhuoUse || _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanUseClaimedEvent>().Any(e => e.Claim.PromiseId == r.PromiseId && e.Claim.ActionId == r.ActionId && e.Claim.Source == r.Source && e.Claim.ParentFrameId == r.UseId)) && _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanCostEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To) == 1);
    internal void SealMoDcMovement(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MoDiaoChan = f.MoDiaoChan!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MoDiaoChanMovementEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMoDc(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    internal bool MoDcLionChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MoDiaoChan is not { Stage: MoDiaoChanStage.CostChildren } || !ValidMoDc(f))
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanCostEvent>().LastOrDefault(e => e.FrameId == f.Id && e.Kind == CardKind.SilverLion && e.From.Zone == CardZoneKind.Equipment);
        if (paid?.From.OwnerSeat is not { } owner || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.SingleOrDefault(m => m.Sequence == paid.Sequence)is not { } movement)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason == movement.Reason && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == owner && recovery.Attempt.TargetSeat == owner;
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == owner && hp.Change.TargetSeat == owner && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == owner && e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.Reason == movement.Reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
    }

    internal bool MoDcFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMoDc(f))
            return false;
        var r = f.MoDiaoChan!;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MoDiaoChanStage.Drawing && f.NativeDraw is not null;
        if (MoDcLionChild(f, child))
            return true;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MoDiaoChanMovementEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MoDcObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame d && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && this.MoDcFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MoDcObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MoDcFirstChild(f, child), (n) => this.MoDcObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31400)]
    internal bool MoDcDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MoDcObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31400)]
    internal bool MoDcRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMoDc(f))
            return false;
        if (MoDcLionChild(f, recovery) || _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i + 1 && MoDcFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MoDcObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31400)]
    internal bool KeepMoDcPaid(ProgramSkillFrame f) => f.MoDiaoChan is { Paid: true } && ValidMoDc(f) || f.WindowContext is { } c && IsIssuedMoDiaoChanCandidate(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    [ProgramContinuation(ProgramContinuationKind.Resume, 31400)]
    internal bool ResumeMoDc(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MoDiaoChan: not null } f || f.Id != id)
            return false;
        ContinueMoDc(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31400)]
    internal bool ReturnMoDcMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMoDc(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31400)]
    internal bool AdvanceMoDcChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MoDcObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
