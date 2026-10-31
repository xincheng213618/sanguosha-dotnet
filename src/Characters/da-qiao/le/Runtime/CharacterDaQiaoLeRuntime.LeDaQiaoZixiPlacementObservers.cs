#nullable enable

using System.Collections;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoLeRuntime
{
    internal bool ZixiPlacementRoot(ProgramSkillFrame f) => f.ZixiPlacement is { } r && ZixiPlacementMother(f) && r.Source == ZixiPlacementSource(f) && r.GameplayHash == f.GameplayHash && r.WindowId == f.WindowContext!.ParentFrameId && r.InstructionIndex == f.InstructionIndex && r.Phase.Kind == ActualDiscardRecoveryPhaseKind.Play && r.Phase.ActorSeat == f.OwnerSeat && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) && r.Entities is IList { IsReadOnly: true } && r.Grains is IList { IsReadOnly: true } && r.Entities.Count <= 2 && r.Entities.Select(e => e.CardId).Distinct().Count() == r.Entities.Count && r.Entities.Select(e => e.SlotKind).Distinct().Count() == r.Entities.Count && (r.Entities.Count == 0 ? r.TargetSeat is null : r.TargetSeat is { } target && _coreTableSeatQueriesOperations.IsValidPlayerSeat(target)) && r.Entities.All(e => _characterXiaoQiaoLeOperations.HasQiqinEntityMark(e.CardId) && CharacterDaQiaoLeDefinitions.ZixiSlots.Contains(e.SlotKind) && e.From.OwnerSeat == f.OwnerSeat && e.From.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind == e.PrintedKind) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZixiPlacementOpenedEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var opened] && opened == new ZixiPlacementOpenedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.Phase);
    internal bool HasPaidZixiPlacement(ProgramSkillFrame f)
    {
        if (!ZixiPlacementRoot(f) || f.ZixiPlacement is not { Paid: true, Stage: ZixiPlacementStage.MovementChildren, TargetSeat: { } target, BatchId: > 0 } r || r.Entities.Count is < 1 or > 2 || r.SequenceBefore < 0 || r.BatchId <= f.Id || r.PrimaryAfter != r.SequenceBefore + r.Entities.Count || r.SequenceAfter < r.PrimaryAfter || r.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZixiPlacementCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var commit] || commit != new ZixiPlacementCommittedEvent(f.Id, r.Source, r.GameplayHash, target, r.Entities.Count, r.SequenceBefore) || _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZixiPlacementPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid != new ZixiPlacementPaidEvent(f.Id, r.BatchId, r.Entities.Count, r.SequenceBefore, r.PrimaryAfter))
            return false;
        var selected = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZixiPlacementEntityFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        var moved = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ZixiQinPlacedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (selected.Length != r.Entities.Count || moved.Length != r.Entities.Count)
            return false;
        for (var i = 0; i < r.Entities.Count; i++)
        {
            var e = r.Entities[i];
            var m = moved[i];
            if (selected[i] != new ZixiPlacementEntityFrozenEvent(f.Id, i, e.CardId, e.PrintedKind, e.From, e.SlotKind) || m != new ZixiQinPlacedEvent(f.Id, i, r.Source, r.GameplayHash, target, e.CardId, e.PrintedKind, e.From, e.SlotKind, r.SequenceBefore + i + 1, r.BatchId) || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(p => p.Sequence == m.MovementSequence && p.CardId == e.CardId && p.CardKind == e.PrintedKind && p.From == e.From && p.To == CardLocation.Judgment(target) && p.Reason.Value == ZixiPlacementReason) != 1)
                return false;
        }

        return true;
    }
    internal bool ZixiPlacementBatch(ProgramSkillFrame f, CardMovementBatchContext batch)
    {
        if (!HasPaidZixiPlacement(f) || f.ZixiPlacement is not { } r || batch.ParentFrameId != f.Id || batch.AwaitingProgramFrameId != f.Id || batch.OriginOwnerSeat != f.OwnerSeat || batch.OriginSkillId != f.SkillId || batch.OriginSkillInstanceId != f.SkillInstanceId)
            return false;
        if (batch.Id == r.BatchId)
            return batch.Movements.Count == r.Entities.Count && !batch.Movements.Where((m, i) => m.CardId != r.Entities[i].CardId || m.CardKind != r.Entities[i].PrintedKind || m.From != r.Entities[i].From || m.To != CardLocation.Judgment(r.TargetSeat!.Value) || m.Reason.Value != ZixiPlacementReason || m.Sequence != r.SequenceBefore + i + 1).Any();
        return batch.ParentBatchId == r.BatchId && batch.Movements.Count > 0 && batch.Movements.All(m => r.Grains.Any(g => g.CardId == m.CardId && g.PrintedKind == m.CardKind && g.From == m.From) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && m.Sequence > r.PrimaryAfter && m.Sequence <= r.SequenceAfter && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
    }

    internal bool ZixiPlacementLionRemoval(ProgramSkillFrame f, int seat) => f.ZixiPlacement is { Paid: true } r && r.Entities.Any(e => e.PrintedKind == CardKind.SilverLion && e.From == CardLocation.Equipment(seat));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 650)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 590)]
    internal bool IsZixiPlacementRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!HasPaidZixiPlacement(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement)
            return recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason == new CardMoveReason(ZixiPlacementReason) && ZixiPlacementLionRemoval(f, recovery.Attempt.TargetSeat);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return index >= 0 && child > index + 1 && ZixiPlacementFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && ZixiPlacementObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    internal bool ZixiPlacementFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!HasPaidZixiPlacement(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is not null && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && ZixiPlacementBatch(f, moved.Batch);
        if (child is RecoveryReplacementFrame recovery)
            return IsZixiPlacementRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == hp.Change.TargetSeat && hp.Change.Amount == 1 && ZixiPlacementLionRemoval(f, hp.Change.TargetSeat);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
    }

    internal bool ZixiPlacementObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying));
    internal ProgramSkillFrame? ZixiPlacementObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.ZixiPlacementFirstChild(f, child), (index) => this.ZixiPlacementObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28710)]
    internal bool AdvanceZixiPlacementChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZixiPlacementObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
