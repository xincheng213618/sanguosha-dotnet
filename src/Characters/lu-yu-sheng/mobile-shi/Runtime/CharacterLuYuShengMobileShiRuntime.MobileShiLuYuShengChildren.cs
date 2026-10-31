#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuYuShengMobileShiRuntime
{
    internal bool ShiLysMovementProof(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MobileShiLuYuSheng is { Stage: 9 or 10 or 11 or 13 } r && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Source?.OwnerSeat == f.OwnerSeat && r.GameplayHash == f.GameplayHash && effect?.Op is SkillProgramEffectOp.ResolveMobileShiRunweiGrant or SkillProgramEffectOp.ResolveMobileShiRunweiPhaseEnd or SkillProgramEffectOp.ResolveMobileShiShuanghuaiGuard;
    internal bool ShiLysFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileShiLuYuSheng is not { } r || r.ActualTurn != _coreActualTurnProgressPhaseTurnNumberAccess._turnNumber || r.GameplayHash != f.GameplayHash || r.Source?.OwnerSeat != f.OwnerSeat || r.Source.SkillInstanceId != f.SkillInstanceId)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Operation == "shuanghuai" && r.Stage == 12 && r.SameAsLast;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && ShiLysMovementProof(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Operation == "shuanghuai" && r.Stage == 11)
        {
            if (child is DyingFrame dying)
                return r.HpPaid && dying.ParentFrameId == f.Id && dying.VictimSeat == f.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileShiShuanghuaiHpPaidEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.After <= 0 && e.Before - e.After == 1) == 1;
            if (child is HpChangedTriggerWindowFrame hp)
                return r.HpPaid && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == f.OwnerSeat;
            if (child is IdentityAppearanceFrame appearance)
                return r.HpAppearance && appearance.Receipt.Entry == IdentityAppearanceEntry.ProgramHpLoss && appearance.Receipt.ParentFrameId == f.Id && appearance.Receipt.OwnerSeat == f.OwnerSeat && appearance.Receipt.RequestedDeduction == 1;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool ShiLysChildEdge(int i) => _characterHuaTuoAnniversaryGodOperations.SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ShiLysFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool ShiLysChildSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterLuYuShengMobileShiOperations.ShiLysChildEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? ShiLysObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLuYuShengMobileShiOperations.ShiLysFirstChild(f, c), (n) => _characterLuYuShengMobileShiOperations.ShiLysChildSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 67400)]
    internal bool ShiLysDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ShiLysObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 67400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 67400)]
    internal bool ShiLysRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (ShiLysFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && ShiLysFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && ShiLysChildSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 67400)]
    internal bool AdvanceShiLysChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ShiLysObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
