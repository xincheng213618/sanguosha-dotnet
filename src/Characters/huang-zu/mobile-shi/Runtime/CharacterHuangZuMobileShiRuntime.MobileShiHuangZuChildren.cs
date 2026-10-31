#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangZuMobileShiRuntime
{
    internal bool ShiHzFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ShiHuangZu is not { } r || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.GameplayHash != f.GameplayHash || f.WindowContext?.ParentFrameId != r.WindowId || r.Source.OwnerSeat != f.OwnerSeat || f.SkillId != ShiHzDuanyang)
            return false;
        if (child is CardUseFrame use)
            return ShiHzUseRidesOn(use, f);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && ShiHzMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && (r.Stage == ShiHzStage.ReturnedUse || m.Sequence > r.Before && m.Sequence <= r.After));
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage is ShiHzStage.RecastDrawing or ShiHzStage.RewardDrawing;
        if (r.Stage == ShiHzStage.RecastCostChildren && r.Selected.Any(m => m.PrintedKind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiHzRecastPaidEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.From == m.From)))
        {
            if (child is RecoveryReplacementFrame recovery)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == r.Target && recovery.Attempt.TargetSeat == r.Target;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.TargetSeat == r.Target;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool ShiHzChildEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiHzFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiHzChildSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterHuangZuMobileShiOperations.ShiHzChildEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? ShiHzObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuangZuMobileShiOperations.ShiHzFirstChild(f, c), (n) => _characterHuangZuMobileShiOperations.ShiHzChildSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 47900)]
    internal bool ShiHzDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiHzObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 47900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 47900)]
    internal bool ShiHzDamageChild(long id) => ShiHzObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 47900)]
    internal bool ShiHzBeforeDamageChild(long id) => ShiHzObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 47900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 47900)]
    internal bool ShiHzRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiHzFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && ShiHzFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && ShiHzChildSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 47900)]
    internal bool AdvanceShiHzChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiHzObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
