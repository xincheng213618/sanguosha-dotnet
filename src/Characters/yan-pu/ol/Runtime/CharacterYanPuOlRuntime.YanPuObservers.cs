#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYanPuOlRuntime
{
    internal bool YanPuFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.YanPu is not { Movements.Count: > 0 } r || root.PendingMovementContinuation is null)
            return false;
        var m = r.Movements[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && (moved.ResumeProgramFrameId == root.Id || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(move => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) && move.Sequence > m.Before && move.Sequence <= m.After && (move.Reason.Value == m.Reason || m.Reason.EndsWith(".draw", StringComparison.Ordinal) && move.Reason == CardMoveReasons.Reshuffle));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is RecoveryReplacementFrame recovery)
            return IsYanPuRecoveryParent(root, recovery);
        if (child is not HpChangedTriggerWindowFrame hp || hp.Change.ParentFrameId != root.Id || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.Amount < 1)
            return false;
        return r.Stage == YanPuStage.RecoveryChildren && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == r.Target || r.Stage == YanPuStage.GiftChildren && r.Promise is { From.Zone: CardZoneKind.Equipment } promise && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(move => move.Sequence == promise.MovementSequence && move.CardId == promise.CardId && move.CardKind == CardKind.SilverLion);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 90)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 80)]
    internal bool IsYanPuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.YanPu is not { } r || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != f.OwnerSeat || recovery.Attempt.Amount != 1)
            return false;
        return r.Stage == YanPuStage.RecoveryChildren && recovery.Attempt.TargetSeat == r.Target && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == 1 || r.Stage == YanPuStage.GiftChildren && r.Promise is { From.Zone: CardZoneKind.Equipment } p && recovery.Attempt.TargetSeat == f.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == YanPuGiftReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == p.MovementSequence && m.CardId == p.CardId && m.CardKind == CardKind.SilverLion);
    }

    internal ProgramSkillFrame? YanPuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterYanPuOlOperations.YanPuFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1500)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1610)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1630)]
    internal bool HasYanPuDamageObserver(long id) => YanPuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26410)]
    internal bool AdvanceYanPuSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && YanPuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    internal bool IsYanPuDyingExitEdge(int index) => index > 0 && index < _resolutionStackOperations._resolutionStack.Count && _resolutionStackOperations._resolutionStack[index] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DyingExited, Continuation: ProgramLifecycleContinuation.ResumeYanPuDyingExit, YanPuDyingExit: { } ret } window && _resolutionStackOperations._resolutionStack[index - 1].Id == ret.DyingParentFrameId && window.OwnerSeat == ret.VictimSeat && ret.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && ret.Turn == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ret.TurnOwner == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<DyingResolvedEvent>().Count(e => e.ResolutionId == ret.DyingFrameId && e.VictimSeat == ret.VictimSeat && e.Survived) == 1;
}
