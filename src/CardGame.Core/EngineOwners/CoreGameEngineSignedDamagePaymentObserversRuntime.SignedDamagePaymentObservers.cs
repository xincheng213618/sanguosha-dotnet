#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSignedDamagePaymentObserversRuntime
{
    internal bool SignedDamagePaymentFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!_coreSignedDamagePaymentsOperations.IsValidSignedDamagePayment(root, true) || root.SignedDamagePayment is not { Stage: SignedDamagePaymentStage.Paid } r || root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat)
            return false;
        if (child is CardsMovedTriggerWindowFrame movement)
            return movement.Batch.Id == movement.Id && movement.Batch.ParentFrameId == root.Id && (movement.Batch.AwaitingProgramFrameId is null || movement.Batch.AwaitingProgramFrameId == root.Id) && movement.Batch.OriginOwnerSeat == root.OwnerSeat && movement.Batch.OriginSkillId == root.SkillId && movement.Batch.OriginSkillInstanceId == root.SkillInstanceId && movement.Batch.Movements.Count == 1 && movement.Batch.Movements.All(m => m.Sequence == r.CostMovementSequence && m.CardId == r.CostCardId && m.From == r.CostFrom && m.To == CardLocation.DiscardPile && _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m));
        if (r.CostFrom != CardLocation.Equipment(root.OwnerSeat) || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == r.CostMovementSequence && m.CardId == r.CostCardId && m.CardKind == CardKind.SilverLion && m.From == r.CostFrom && m.To == CardLocation.DiscardPile))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == $"skill-program.{(root.SkillId)}.{(SkillProgramEffectOp.DiscardOwnedCardToAdjustCurrentDamage)}";
    }

    internal ProgramSkillFrame? SignedDamagePaymentObserverRoot(long? exactWindowId = null)
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || !SignedDamagePaymentFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]))
                continue;
            if (exactWindowId is { } id && root.SignedDamagePayment!.BeforeDamageFrameId != id && !_resolutionStackOperations._resolutionStack.Skip(index + 1).Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame or DamageTriggerWindowFrame))
                continue;
            var valid = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child))
                {
                    valid = false;
                    break;
                }

                // Prove the incoming Dying edge before using a complete mature
                // native/multi-material/provider/zero-entity/round Alcohol suffix.
                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying) || _coreRoundPricedPileAlcoholOperations.IsRoundPricedPileAlcoholRide(child, dying)))
                    break;
            }

            if (valid)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 540)]
    internal bool IsSignedDamagePaymentProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SignedDamagePaymentObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 570)]
    internal bool TryAdvanceSignedDamagePaymentSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || SignedDamagePaymentObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: false, incompleteAttackError: "Paid damage child must finish its actual damage/Dying before its program tail.");
    }
}
