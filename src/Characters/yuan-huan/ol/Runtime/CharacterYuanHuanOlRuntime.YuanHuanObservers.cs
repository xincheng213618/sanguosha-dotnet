#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYuanHuanOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26920)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 26920)]
    internal bool ContinueIssuedYuanHuan(ProgramSkillFrame f)
    {
        if (f.YuanHuan is not { Stage: not (YuanHuanStage.Guess or YuanHuanStage.SourceDiscard) })
            return false;
        return true;
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 750)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 690)]
    internal bool IsYuanHuanRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.YuanHuan is not { Stage: YuanHuanStage.RecoveryChildren } r)
            return false;
        return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && r.GuessMask != (r.ActualMask ^ 15) && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == r.InstructionIndex;
    }

    internal bool YuanHuanFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YuanHuan is not { Movements.Count: > 0 } r || f.PendingMovementContinuation is null)
            return false;
        var m = r.Movements[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > m.Before && move.Sequence <= m.After && (move.Reason.Value == m.Reason || move.Reason == CardMoveReasons.Reshuffle));
        if (child is RecoveryReplacementFrame replacement)
            return IsYuanHuanRecoveryParent(f, replacement);
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == YuanHuanStage.RecoveryChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? YuanHuanObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterYuanHuanOlOperations.YuanHuanFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26920)]
    internal bool AdvanceYuanHuanSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YuanHuanObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
