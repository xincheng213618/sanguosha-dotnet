#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuRanMouMobileRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 34400)]
    internal bool MouZhuRanMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => effect?.Op is SkillProgramEffectOp.ResolveMouZhuRanZhenwei or SkillProgramEffectOp.ResolveMouZhuRanHeyuan && MouZhuRan.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouZhuRanFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouZhuRan.Valid(f) || f.MouZhuRan is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouZhuRanStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouZhuRanMovementReturn(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouZhuRanStage.CostChildren)
        {
            var reason = r.Operation == SkillProgramEffectOp.ResolveMouZhuRanZhenwei ? MouZhuRanRuntime.CostReason : MouZhuRanRuntime.HeyuanCostReason;
            var lion = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => r.Paying.Any(c => c.CardId == m.CardId && c.From == m.From) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(r.Payer) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == reason).ToArray();
            if (lion.Length > 0 && child is RecoveryReplacementFrame recovery)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == reason && recovery.Attempt.SourceSeat == r.Payer && recovery.Attempt.TargetSeat == r.Payer && recovery.Attempt.Amount == 1;
            if (lion.Length > 0 && child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.Payer && hp.Change.TargetSeat == r.Payer && _coreProgramEventHistoryOperations.ProgramEventHistory<SilverLionRemovedRecoveryEvent>().Any(e => e.ResolutionId > f.Id && e.ResolutionId < hp.Change.Id && e.PlayerSeat == r.Payer && e.Reason.Value == reason && e.RecoveredAmount == hp.Change.Amount && e.RemainingHp == hp.Change.HpAfter);
        }

        if (r.Stage == MouZhuRanStage.Damage && f.AttackAttempt is { } a && f.AttackReturn is not null && a.SourceSeat == f.OwnerSeat && a.TargetSeat == r.Recipient && !a.SourceLess && _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhuRanEffectIssuedEvent>().Count(e => e.FrameId == f.Id && e.Index == r.EffectIndex && e.Effect == MouZhuRanEffect.Damage && e.SourceSeat == f.OwnerSeat && e.RecipientSeat == r.Recipient) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouZhuRanNativeEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouZhuRanFirstNativeChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouZhuRanNativeSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterZhuRanMouMobileOperations.MouZhuRanNativeEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? MouZhuRanNativeRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhuRanMouMobileOperations.MouZhuRanFirstNativeChild(f, c), (n) => _characterZhuRanMouMobileOperations.MouZhuRanNativeSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 34400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 34400)]
    internal bool MouZhuRanDamageWindow(long id) => MouZhuRanNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 34400)]
    internal bool MouZhuRanBeforeDamage(long id) => MouZhuRanNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 34400)]
    internal bool MouZhuRanDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouZhuRanNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 34400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 34400)]
    internal bool MouZhuRanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouZhuRanFirstNativeChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouZhuRanFirstNativeChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouZhuRanNativeSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 34400)]
    internal bool MouZhuRanAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZhuRanNativeRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
