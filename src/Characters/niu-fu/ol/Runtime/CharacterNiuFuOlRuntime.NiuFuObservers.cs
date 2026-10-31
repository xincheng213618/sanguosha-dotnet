#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterNiuFuOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27050)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27050)]
    internal bool ContinuePaidNiuFu(ProgramSkillFrame f)
    {
        if (f.NiuFu is not { } r || r.Steps.Count == 0 && r.SourceBranch is null)
            return false;
        return true;
    }

    internal bool NiuFuLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason)
    {
        var step = f.NiuFu!.Steps[^1];
        return reason is not null && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > step.Before && m.Sequence <= step.After && m.From == CardLocation.Equipment(seat) && m.CardKind == CardKind.SilverLion && m.Reason == reason);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 780)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 720)]
    internal bool IsNiuFuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.NiuFu is not { Steps.Count: > 0 } || f.PendingMovementContinuation is null)
            return false;
        return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && NiuFuLionProof(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    }

    internal bool NiuFuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.NiuFu is not { } r)
            return false;
        if (r.Stage == NiuFuStage.SourceHpChildren)
        {
            if (child is DyingFrame dying)
                return dying.ParentFrameId == f.Id && dying.VictimSeat == r.DamageSourceSeat && dying.Continuation == DyingContinuationKind.ProgramSkill;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == r.DamageSourceSeat && hp.Change.Amount == 1 && hp.Change.SourceSeat is null;
        }

        if (child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id)
            return lifecycle.Window == SkillProgramTriggerWindow.SkillsChanged && lifecycle.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || r.Stage == NiuFuStage.SourceTurnChildren && lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && lifecycle.CharacterStateContinuation == CharacterStateContinuation.Program && lifecycle.OwnerSeat == r.DamageSourceSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == lifecycle.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == r.DamageSourceSeat && e.Change.Window == lifecycle.Window);
        if (f.PendingMovementContinuation is null || r.Steps.Count == 0)
            return false;
        var step = r.Steps[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > step.Before && m.Sequence <= step.After);
        if (child is RecoveryReplacementFrame recovery)
            return IsNiuFuRecoveryParent(f, recovery);
        return child is HpChangedTriggerWindowFrame recoveryHp && recoveryHp.Change.ParentFrameId == f.Id && recoveryHp.ResumeFrameId == f.Id && recoveryHp.Continuation == PostEventContinuation.AwaitedProgramMovement && recoveryHp.Change.Kind == HpChangeKind.Recovery && recoveryHp.Change.Amount == 1 && recoveryHp.Change.SourceSeat == recoveryHp.Change.TargetSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > step.Before && m.Sequence <= step.After && m.From == CardLocation.Equipment(recoveryHp.Change.TargetSeat) && m.CardKind == CardKind.SilverLion);
    }

    internal ProgramSkillFrame? NiuFuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterNiuFuOlOperations.NiuFuFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27050)]
    internal bool AdvanceNiuFuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && NiuFuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
