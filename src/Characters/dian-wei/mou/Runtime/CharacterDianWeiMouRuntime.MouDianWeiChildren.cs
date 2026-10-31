#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDianWeiMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 35300)]
    internal bool MouDianWeiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.MouDianWei is { } r && effect?.Op == r.Operation && MouDianWei.Valid(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouDianWeiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouDianWei.Valid(f) || f.MouDianWei is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouDianWeiStage.Drawing && f.NativeDraw is not null;
        if (child is PindianFrame contest)
            return r.Stage == MouDianWeiStage.Pindian && contest.Id == r.ContestId && contest.ParentFrameId == f.Id && contest.SourceSeat == f.OwnerSeat && contest.OpponentSeat == r.Opponent && contest.ProgramResultBind == MouDianWeiRuntime.Bind(r);
        if (child is CardUseFrame use)
            return r.Stage == MouDianWeiStage.Slash && IsMouDianWeiSlashUse(use.Id);
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } p && MouDianWeiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
        {
            var movement = moved.Batch.Movements;
            if (r.Stage == MouDianWeiStage.PindianChildren)
                return moved.ResumeProgramFrameId == f.Id && movement.Count > 0 && movement.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason == CardMoveReasons.PindianFinish);
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && movement.Count > 0 && movement.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        }

        if (r.Stage == MouDianWeiStage.RecoveryChildren)
        {
            if (child is RecoveryReplacementFrame rr)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(rr, f) && rr.Attempt.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.ProgramMaximum, 1) && rr.Attempt.SourceSeat == f.OwnerSeat && rr.Attempt.TargetSeat == f.OwnerSeat && rr.Attempt.Amount == r.Requested;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && f.RecoveryReceipt is { InstructionIndex: 1 } receipt && hp.Change.Amount == receipt.Recovered && hp.Change.HpBefore == r.HpBefore && hp.Change.HpAfter == r.HpBefore + receipt.Recovered;
        }

        if (r.Stage == MouDianWeiStage.LossChildren && child is HpChangedTriggerWindowFrame loss)
            return loss.ResumeFrameId == f.Id && loss.Change.ParentFrameId == f.Id && loss.Change.Kind == HpChangeKind.Loss && loss.Change.TargetSeat == f.OwnerSeat && loss.Change.HpBefore == r.HpBefore && loss.Change.Amount == r.ActualLoss && loss.Change.HpAfter == r.HpBefore - r.ActualLoss && loss.Change.HpAfter >= 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Any(e => e.FrameId == f.Id && e.Amount == r.ActualLoss && e.TargetSeat == f.OwnerSeat);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouDianWeiEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouDianWeiFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouDianWeiSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterDianWeiMouOperations.MouDianWeiEdge(i), (i, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouDianWeiRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterDianWeiMouOperations.MouDianWeiFirstChild(f, c), (n) => _characterDianWeiMouOperations.MouDianWeiSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 35300)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 35300)]
    internal bool MouDianWeiDamageWindow(long id) => MouDianWeiRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 35300)]
    internal bool MouDianWeiBeforeDamage(long id) => MouDianWeiRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 35300)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 35300)]
    internal bool MouDianWeiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouDianWeiRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 35300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 35300)]
    internal bool MouDianWeiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouDianWeiFirstChild(f, recovery))
            return true;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && MouDianWeiFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && MouDianWeiSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 35300)]
    internal bool MouDianWeiAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouDianWeiRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
