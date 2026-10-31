#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineInspectedHandSlashObserversRuntime
{
    internal ProgramSkillFrame? InspectedHandPaidObserverRoot(long? damageWindow = null)
    {
        for (var index = 0; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || root.InspectedHandSlash is not { } d || d.Stage is not (InspectedHandSlashStage.HpChildren or InspectedHandSlashStage.DiscardChildren) || !_coreInspectedHandSlashReturnsOperations.ValidInspectedHandSlash(root))
                continue;
            if (damageWindow is { } window && !_resolutionStackOperations._resolutionStack.Skip(index + 1).Any(f => f.Id == window && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame))
                continue;
            if (index + 1 == _resolutionStackOperations._resolutionStack.Count)
                return damageWindow is null ? root : null;
            var first = _resolutionStackOperations._resolutionStack[index + 1];
            bool firstMatches;
            if (d.Stage == InspectedHandSlashStage.DiscardChildren)
                firstMatches = root.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == root.OwnerSeat && first is CardsMovedTriggerWindowFrame movement && movement.Batch.ParentFrameId == root.Id && (movement.Batch.AwaitingProgramFrameId is null || movement.Batch.AwaitingProgramFrameId == root.Id) && movement.Batch.OriginOwnerSeat == root.OwnerSeat && movement.Batch.OriginSkillId == root.SkillId && movement.Batch.OriginSkillInstanceId == root.SkillInstanceId && movement.Batch.Movements is [var actual] && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(actual) && actual.Sequence == d.SequenceAfter && actual.CardId == d.DiscardedCardId && actual.From == CardLocation.Hand(d.TargetSeat) && actual.To == CardLocation.DiscardPile && actual.Reason.Value == "program.hp-hand-inspection.discard";
            else
                firstMatches = first switch
                {
                    DyingFrame dying => dying.ParentFrameId == root.Id && dying.VictimSeat == root.OwnerSeat && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.KillerSeat is null && d.HpAfter == 0,
                    HpChangedTriggerWindowFrame hp => hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Loss && hp.Change.SourceSeat is null && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1,
                    _ => false
                };
            if (!firstMatches)
                continue;
            var valid = true;
            // The first typed edge is proven by the real HP cost or discard ledger above.
            for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                var preceding = _resolutionStackOperations._resolutionStack[child - 1];
                if (preceding is DyingFrame rescued && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child - 1, rescued) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child - 1, rescued) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child - 1, rescued) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child - 1, rescued)))
                    break;
                if (!_corePaidColorDamageClaimObserversOperations.PaidColorDamageClaimObserverEdge(child))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 360)]
    internal bool IsInspectedHandProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && InspectedHandPaidObserverRoot(_coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
