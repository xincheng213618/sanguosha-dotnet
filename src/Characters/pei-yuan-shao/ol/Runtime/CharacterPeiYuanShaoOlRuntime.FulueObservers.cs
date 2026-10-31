#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPeiYuanShaoOlRuntime
{

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27120)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27120)]
    internal bool ContinueIssuedFulue(ProgramSkillFrame f)
    {
        if (f.Fulue is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is { } id && f.ActivationId == id && f.WindowContext is { } context && CanRunIssuedFulueCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, 0), context);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 570)]
    internal bool IsFulueMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.Fulue?.Stage != FulueStage.MovementChildren || effect is null || !PeiYuanShaoComposition.Handles(effect.Op) || pending != f.PendingMovementContinuation)
            return false;
        return true;
    }

    internal bool FulueLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason) => f.Fulue is { Stage: FulueStage.MovementChildren, Obtained: { CardKind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } cost } r && cost.From.OwnerSeat == seat && reason?.Value == FulueObtainReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == cost.CardId && m.From == cost.From && m.Reason == reason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 840)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 780)]
    internal bool IsFulueRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Fulue?.Stage == FulueStage.MovementChildren && f.PendingMovementContinuation is not null && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && FulueLionProof(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    internal bool FulueFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Fulue is not { Stage: FulueStage.MovementChildren } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame recovery)
            return IsFulueRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && FulueLionProof(f, hp.Change.TargetSeat, new(FulueObtainReason));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? FulueObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterPeiYuanShaoOlOperations.FulueFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 680)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 790)]
    internal bool HasFulueDamageObserver(long id) => FulueObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 820)]
    internal bool HasFulueBeforeDamageObserver(long id) => FulueObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27120)]
    internal bool AdvanceFulueSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FulueObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
