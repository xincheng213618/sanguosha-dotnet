#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConditionalDiscardDuelObserversRuntime
{
    internal ProgramSkillFrame? ConditionalDuelPaidObserverRoot()
    {
        for (var rootIndex = 0; rootIndex + 1 < _resolutionStackOperations._resolutionStack.Count; rootIndex++)
        {
            if (_resolutionStackOperations._resolutionStack[rootIndex] is not ProgramSkillFrame root || root.ConditionalDiscardDuel is not { Stage: ConditionalDiscardDuelStage.OwnerChildren or ConditionalDiscardDuelStage.TargetChildren } d || root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || !_coreConditionalDiscardDuelReturnsOperations.ValidConditionalDuelPayments(root))
                continue;
            var paid = d.Stage == ConditionalDiscardDuelStage.OwnerChildren ? d.OwnerPayment! : d.TargetPayment!;
            if (pending.SubjectSeat != paid.PayerSeat)
                continue;
            var first = _resolutionStackOperations._resolutionStack[rootIndex + 1];
            var reason = d.Stage == ConditionalDiscardDuelStage.OwnerChildren ? "skill-program.conditional-discard-duel.owner-cost" : "skill-program.conditional-discard-duel.target-cost";
            if (first is CardsMovedTriggerWindowFrame moved)
            {
                if (moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != root.Id || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements is not [var m] || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) || m.CardId != paid.CardId || m.Sequence <= paid.SequenceBefore || m.Sequence > paid.SequenceAfter || m.Reason.Value != reason)
                    continue;
            }
            else
            {
                if (paid.PrintedKind != CardKind.SilverLion || paid.From != CardLocation.Equipment(paid.PayerSeat))
                    continue;
                if (first is HpChangedTriggerWindowFrame hp)
                {
                    if (hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.ParentFrameId != root.Id || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.SourceSeat != paid.PayerSeat || hp.Change.TargetSeat != paid.PayerSeat || hp.Change.Amount != 1)
                        continue;
                }
                else if (first is RecoveryReplacementFrame recovery)
                {
                    if (!_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != paid.PayerSeat || recovery.Attempt.TargetSeat != paid.PayerSeat || recovery.Attempt.Amount != 1 || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Completion.MoveReason?.Value != reason)
                        continue;
                }
                else
                    continue;
            }

            var valid = true;
            for (var i = rootIndex + 1; i < _resolutionStackOperations._resolutionStack.Count; i++)
            {
                if (!_corePaidColorDamageClaimObserversOperations.PaidColorDamageClaimObserverEdge(i))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[i] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(i, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(i, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(i, dying)))
                    break;
            }

            if (valid)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 350)]
    internal bool IsConditionalDiscardDuelProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ConditionalDuelPaidObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
