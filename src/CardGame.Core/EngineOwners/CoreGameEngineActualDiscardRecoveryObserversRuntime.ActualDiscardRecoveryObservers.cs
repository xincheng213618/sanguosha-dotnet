#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualDiscardRecoveryObserversRuntime
{
    internal bool ActualEquipmentOrDiscardFirstObserver(int index, ProgramSkillFrame root)
    {
        var child = _resolutionStackOperations._resolutionStack[index + 1];
        long before, after;
        if (root.CapturedEquipmentDraw is { } equipment)
        {
            if (equipment.Stage == CapturedEquipmentDrawStage.Placed)
            {
                before = equipment.SequenceBefore;
                after = equipment.SequenceAfter;
            }
            else if (equipment.Stage == CapturedEquipmentDrawStage.Drawn)
            {
                before = equipment.DrawSequenceBefore;
                after = equipment.DrawSequenceAfter;
            }
            else
                return false;
        }
        else if (root.ActualDiscardRecovery is { Stage: ActualDiscardRecoveryStage.ReturnPaid } returned)
        {
            before = returned.ReturnMovementSequence - 1;
            after = returned.ReturnMovementSequence;
        }
        else if (root.ActualDiscardRecovery is { Stage: ActualDiscardRecoveryStage.ClaimPaid } claim)
        {
            before = claim.ClaimSequenceBefore;
            after = claim.ClaimSequenceAfter;
        }
        else
            return false;
        if (root.PendingMovementContinuation is not { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } || subject != root.OwnerSeat)
            return false;
        if (child is CardsMovedTriggerWindowFrame movement)
            return movement.Batch.ParentFrameId == root.Id && (movement.Batch.AwaitingProgramFrameId is null || movement.Batch.AwaitingProgramFrameId == root.Id) && movement.Batch.OriginSkillId == root.SkillId && movement.Batch.OriginSkillInstanceId == root.SkillInstanceId && movement.Batch.OriginOwnerSeat == root.OwnerSeat && movement.Batch.Movements.Count > 0 && movement.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after);
        if (root.CapturedEquipmentDraw is not { Stage: CapturedEquipmentDrawStage.Placed })
            return false;
        var removals = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after && m.CardKind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment && m.From.OwnerSeat is not null && m.To != m.From).ToArray();
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && removals.Any(m => m.From.OwnerSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.MoveReason == m.Reason);
        return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && removals.Any(m => m.From.OwnerSeat == hp.Change.TargetSeat);
    }

    internal ProgramSkillFrame? ActualEquipmentOrDiscardObserverRoot(long id)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || root.CapturedEquipmentDraw is null && root.ActualDiscardRecovery is null)
            return null;
        if (index == _resolutionStackOperations._resolutionStack.Count - 1)
            return root;
        if (!ActualEquipmentOrDiscardFirstObserver(index, root))
            return null;
        for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            if (child > index + 1 && !_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child))
                return null;
            if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying)))
                break;
        }

        return root;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 570)]
    internal bool IsActualEquipmentOrDiscardProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => (f.CapturedEquipmentDraw is not null || f.ActualDiscardRecovery is not null) && ActualEquipmentOrDiscardObserverRoot(f.Id)is not null && _resolutionStackOperations._resolutionStack.FindIndex(d => d.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(r => r.Id == f.Id));
}
