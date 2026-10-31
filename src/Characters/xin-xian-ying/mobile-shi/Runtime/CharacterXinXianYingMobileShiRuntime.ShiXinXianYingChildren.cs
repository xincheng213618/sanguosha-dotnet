#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinXianYingMobileShiRuntime
{
    internal bool ShiXxyFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ShiXinXianYing is not { } r || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.GameplayHash != f.GameplayHash || r.Source != ShiXxySource(f))
            return false;
        if (child is SkillInvocationFrame invocation)
            return r.Stage == ShiXinXianYingStage.QingshiInvocation && r.Jiejie && r.QingIssued && invocation.ParentFrameId == f.Id && invocation.ActorSeat == f.OwnerSeat && invocation.SkillKey == ShiXxyQingshi && invocation.SkillInstanceId == r.QingshiInstance && invocation.OccurrenceKey == "shi-jiejie-qingshi:" + f.Id && _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(f, invocation);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == ShiXinXianYingStage.Drawing;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && ShiXxyMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == ShiXinXianYingStage.QingDiscardChildren && r.DiscardPaid && r.Selected.Any(m => _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment))
        {
            if (child is RecoveryReplacementFrame recovery)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && r.Selected.Any(m => m.From.OwnerSeat == recovery.Attempt.TargetSeat && m.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == CardKind.SilverLion);
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && r.Selected.Any(m => m.From.OwnerSeat == hp.Change.TargetSeat && m.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == CardKind.SilverLion);
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool ShiXxyChildEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiXxyFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiXxyChildSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterXinXianYingMobileShiOperations.ShiXxyChildEdge(i), (i, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? ShiXxyObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterXinXianYingMobileShiOperations.ShiXxyFirstChild(f, c), (n) => _characterXinXianYingMobileShiOperations.ShiXxyChildSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 48200)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 48200)]
    internal bool ShiXxyDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiXxyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 48200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 48200)]
    internal bool ShiXxyDamageChild(long id) => ShiXxyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 48200)]
    internal bool ShiXxyBeforeDamageChild(long id) => ShiXxyObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 48200)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 48200)]
    internal bool ShiXxyRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiXxyFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && ShiXxyFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && ShiXxyChildSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 48200)]
    internal bool AdvanceShiXxyChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiXxyObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
