#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhuoMouOlRuntime
{
    internal bool MouDongZhuoOlMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouDongZhuoOl is { Paid: true } r && effect?.Op == r.Operation && MouDongZhuoOl.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouDongZhuoOlFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouDongZhuoOl.Valid(f) || f.MouDongZhuoOl is not { } r)
            return false;
        if (child is CardUseFrame u)
            return r.Stage == MouDongZhuoOlStage.Wine && r.WineUseId == u.Id && IsMouDongZhuoOlWine(u.Id);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouDongZhuoOlMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage is MouDongZhuoOlStage.CostChildren or MouDongZhuoOlStage.PeerChildren)
        {
            var lion = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => r.Materials.Any(c => c.Id == m.CardId && c.From == m.From) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Payer) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == MouDongZhuoOlRuntime.CostReason).ToArray();
            if (lion.Length > 0 && child is RecoveryReplacementFrame recovery)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MouDongZhuoOlRuntime.CostReason && recovery.Attempt.SourceSeat == r.Payer && recovery.Attempt.TargetSeat == r.Payer;
            if (lion.Length > 0 && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.Payer && hp.Change.TargetSeat == r.Payer && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.PlayerSeat == r.Payer && e.Reason.Value == MouDongZhuoOlRuntime.CostReason && e.RemainingHp == hp.Change.HpAfter);
        }

        if (r.Stage == MouDongZhuoOlStage.Damage && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == f.OwnerSeat && a.TargetSeat == r.Payer && !a.SourceLess && _coreProgramEventHistoryOperations.ProgramEventHistory<MouDongZhuoOlDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.Target == r.Payer && e.Cursor == r.Cursor) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (r.Stage is MouDongZhuoOlStage.ZhibingRecovery or MouDongZhuoOlStage.ZhibingGrants or MouDongZhuoOlStage.Finishing)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && rr.Attempt.SourceSeat == f.OwnerSeat && rr.Attempt.TargetSeat == f.OwnerSeat;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.OwnerSeat;
        }

        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouDongZhuoOlEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouDongZhuoOlFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouDongZhuoOlSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterDongZhuoMouOlOperations.MouDongZhuoOlEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouDongZhuoOlRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDongZhuoMouOlOperations.MouDongZhuoOlFirstChild(f, c), (n) => _characterDongZhuoMouOlOperations.MouDongZhuoOlSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 34700)]
    internal bool MouDongZhuoOlDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouDongZhuoOlRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 34700)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 34700)]
    internal bool MouDongZhuoOlRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouDongZhuoOlFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouDongZhuoOlFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouDongZhuoOlSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 34700)]
    internal bool MouDongZhuoOlAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDongZhuoOlRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
