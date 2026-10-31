#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangFengOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27150)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27150)]
    internal bool ContinuePaidYangFeng(ProgramSkillFrame f)
    {
        if (f.YangFeng is not { Movements.Count: > 0 })
            return false;
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 580)]
    internal bool IsYangFengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p)
    {
        if (f.YangFeng is not { Movements.Count: > 0 } r || e?.Op != r.Op || p != f.PendingMovementContinuation)
            return false;
        return true;
    }

    internal bool YangFengLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason)
    {
        var m = f.YangFeng!.Movements[^1];
        return reason?.Value == JiaweiDiscard && m.Stage == YangFengStage.DiscardChildren && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence > m.Before && move.Sequence <= m.After && move.CardKind == CardKind.SilverLion && move.From == CardLocation.Equipment(seat) && move.Reason == reason);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 850)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 790)]
    internal bool IsYangFengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.YangFeng is not { Movements.Count: > 0 } || f.PendingMovementContinuation is null)
            return false;
        return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && YangFengLionProof(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    }

    internal bool YangFengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.YangFeng is not { Movements.Count: > 0 } r || f.PendingMovementContinuation is null)
            return false;
        var m = r.Movements[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > m.Before && move.Sequence <= m.After);
        if (child is RecoveryReplacementFrame replacement)
            return IsYangFengRecoveryParent(f, replacement);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && YangFengLionProof(f, hp.Change.TargetSeat, new(JiaweiDiscard));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? YangFengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterYangFengOlOperations.YangFengFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 690)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 800)]
    internal bool HasYangFengDamageObserver(long id) => YangFengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 830)]
    internal bool HasYangFengBeforeDamageObserver(long id) => YangFengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27150)]
    internal bool AdvanceYangFengSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YangFengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
