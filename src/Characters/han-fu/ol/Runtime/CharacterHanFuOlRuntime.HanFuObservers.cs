#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanFuOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27020)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27020)]
    internal bool ContinuePaidHanFu(ProgramSkillFrame f)
    {
        if (f.HanFu is not { Movements.Count: > 0 })
            return false;
        return true;
    }

    internal bool HanFuLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason)
    {
        var m = f.HanFu!.Movements![^1];
        return reason is not null && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > m.Before && move.Sequence <= m.After && move.CardKind == CardKind.SilverLion && move.From == CardLocation.Equipment(seat) && move.Reason == reason);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 760)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 700)]
    internal bool IsHanFuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.HanFu is not { Movements.Count: > 0 } || f.PendingMovementContinuation is null)
            return false;
        return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && HanFuLionProof(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    }

    internal bool HanFuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.HanFu is not { } r)
            return false;
        if (r.Stage == HanFuStage.DamageIssued)
            return f.AttackAttempt is { SourceLess: false } attack && attack.SourceSeat == f.OwnerSeat && attack.TargetSeat == r.TargetSeat && f.AttackReturn is not null && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        if (f.PendingMovementContinuation is null)
            return false;
        var m = r.Movements![^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > m.Before && move.Sequence <= m.After);
        if (child is RecoveryReplacementFrame replacement)
            return IsHanFuRecoveryParent(f, replacement);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.TargetSeat is var seat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > m.Before && move.Sequence <= m.After && move.CardKind == CardKind.SilverLion && move.From == CardLocation.Equipment(seat));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? HanFuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHanFuOlOperations.HanFuFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27020)]
    internal bool AdvanceHanFuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && HanFuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
