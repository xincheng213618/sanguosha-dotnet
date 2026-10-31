#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrepDiscardObserversRuntime
{
    internal ProgramSkillFrame? PrepDiscardPaidObserverRoot(long? exactDamageWindow = null)
    {
        for (var rootIndex = 1; rootIndex < _resolutionStackOperations._resolutionStack.Count; rootIndex++)
        {
            if (_resolutionStackOperations._resolutionStack[rootIndex] is not ProgramSkillFrame root)
                continue;
            if (root.PrepDiscard is { Stage: PrepDiscardStage.ImmediateDrawAttempt } && _corePrepDiscardReceiptsOperations.ValidPrepDiscard(root))
            {
                if (exactDamageWindow is { } requestedWindow && !_resolutionStackOperations._resolutionStack.Skip(rootIndex + 1).Any(f => f.Id == requestedWindow && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame))
                    continue;
                if (rootIndex == _resolutionStackOperations._resolutionStack.Count - 1)
                    return root;
                if (!CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[rootIndex + 1], root))
                    continue;
                if (PrepDiscardObserverSuffix(rootIndex))
                    return root;
                continue;
            }

            if (root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending)
                continue;
            long before, after;
            PrepDiscardPayment? paid = null;
            string reason;
            int payer;
            if (root.PrepDiscard is { Stage: PrepDiscardStage.TargetChildren or PrepDiscardStage.OwnerChildren } d && _corePrepDiscardReceiptsOperations.ValidPrepDiscard(root))
            {
                var ownerCost = d.Stage == PrepDiscardStage.OwnerChildren;
                paid = ownerCost ? d.OwnerPayment : d.TargetPayment;
                if (paid is null)
                    continue;
                before = paid.SequenceBefore;
                after = paid.SequenceAfter;
                payer = paid.PayerSeat;
                reason = ownerCost ? "skill-program.preparation-discard.owner.ResolvePrepDiscardOrEnding" : "skill-program.preparation-discard.target.ResolvePrepDiscardOrEnding";
            }
            else if (root.PrepDiscardEndingDraw is { ChildrenCompleted: false } draw && _corePrepDiscardEndingPromisesOperations.ValidPrepDiscardEndingDraw(root))
            {
                before = draw.SequenceBefore;
                after = draw.SequenceAfter;
                payer = draw.Promise.TargetSeat;
                reason = "program.preparation-discard.ending-draw";
            }
            else if (root.PrepDiscard is { Stage: PrepDiscardStage.ImmediateDrawChildren, ImmediateDraw.Outcome: { } outcome } && _corePrepDiscardReceiptsOperations.ValidPrepDiscard(root))
            {
                before = outcome.SequenceBefore;
                after = outcome.SequenceAfter;
                payer = outcome.RecipientSeat;
                reason = CoreGameEnginePrepDiscardImmediateDrawDefinitions.PrepDiscardImmediateDrawReason.Value;
            }
            else
                continue;
            if (pending.SubjectSeat != payer || after <= before || exactDamageWindow is { } id && !_resolutionStackOperations._resolutionStack.Skip(rootIndex + 1).Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame))
                continue;
            if (rootIndex == _resolutionStackOperations._resolutionStack.Count - 1)
                return root;
            var first = _resolutionStackOperations._resolutionStack[rootIndex + 1];
            if (first is CardsMovedTriggerWindowFrame movement)
            {
                if (movement.Batch.ParentFrameId != root.Id || movement.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != root.Id || movement.Batch.OriginSkillId != root.SkillId || movement.Batch.OriginSkillInstanceId != root.SkillInstanceId || movement.Batch.OriginOwnerSeat != root.OwnerSeat || movement.Batch.Movements.Count == 0 || movement.Batch.Movements.Any(m => m.Sequence <= before || m.Sequence > after || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m)))
                    continue;
            }
            else
            {
                if (paid is null || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && paid.CardIds.Contains(m.CardId) && m.From == CardLocation.Equipment(payer) && m.To == CardLocation.DiscardPile && m.CardKind == CardKind.SilverLion && m.Reason.Value == reason))
                    continue;
                if (first is HpChangedTriggerWindowFrame hp)
                {
                    if (hp.Change.ParentFrameId != root.Id || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.SourceSeat != payer || hp.Change.TargetSeat != payer || hp.Change.Amount != 1)
                        continue;
                }
                else if (first is RecoveryReplacementFrame recovery)
                {
                    if (!_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != payer || recovery.Attempt.TargetSeat != payer || recovery.Attempt.Amount != 1 || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Completion.MoveReason?.Value != reason)
                        continue;
                }
                else
                    continue;
            }

            if (PrepDiscardObserverSuffix(rootIndex))
                return root;
        }

        return null;
    }

    internal bool PrepDiscardObserverSuffix(int rootIndex)
    {
        for (var index = rootIndex + 1; index < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            // The exact paid or native-attempt root edge is checked above.
            if (!CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) && !_corePaidColorDamageClaimObserversOperations.PaidColorDamageClaimObserverEdge(index))
                return false;
            if (_resolutionStackOperations._resolutionStack[index] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(index, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(index, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(index, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(index, dying)))
                break;
        }

        return true;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 650)]
    internal bool IsPrepDiscardProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && PrepDiscardPaidObserverRoot(_coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 20)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 20)]
    internal bool IsPrepDiscardRecoveryParent(ProgramSkillFrame root, RecoveryReplacementFrame recovery)
    {
        if (root.PrepDiscard is not { Stage: PrepDiscardStage.TargetChildren or PrepDiscardStage.OwnerChildren } draft || !_corePrepDiscardReceiptsOperations.ValidPrepDiscard(root) || recovery.ParentFrameId != root.Id || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Amount != 1)
            return false;
        var ownerCost = draft.Stage == PrepDiscardStage.OwnerChildren;
        var paid = ownerCost ? draft.OwnerPayment! : draft.TargetPayment!;
        var reason = ownerCost ? "skill-program.preparation-discard.owner.ResolvePrepDiscardOrEnding" : "skill-program.preparation-discard.target.ResolvePrepDiscardOrEnding";
        return root.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } pending && pending.SubjectSeat == paid.PayerSeat && recovery.Attempt.SourceSeat == paid.PayerSeat && recovery.Attempt.TargetSeat == paid.PayerSeat && recovery.Attempt.Completion.MoveReason?.Value == reason && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > paid.SequenceBefore && m.Sequence <= paid.SequenceAfter && paid.CardIds.Contains(m.CardId) && m.From == CardLocation.Equipment(paid.PayerSeat) && m.To == CardLocation.DiscardPile && m.CardKind == CardKind.SilverLion && m.Reason.Value == reason);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 1246)]
    internal bool AdvanceImmediatePrepDiscardChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && PrepDiscardPaidObserverRoot()is { } root && _corePrepDiscardPaymentsOperations.IsImmediatePrepDiscard(root) && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
