#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSlashTargetPenaltyObserversRuntime
{
    internal bool ValidTargetPenaltyReward(ProgramSkillFrame f)
    {
        if (!_coreSlashTargetPenaltyPaymentsOperations.ValidTargetPenaltyPayment(f) || f.SlashTargetPenaltyDraft is not { Stage: SlashTargetPenaltyStage.RewardChildren, Recast: true, DrawAttempted: true } d || d.ActualDrawCount < 0 || d.ActualDrawCount > d.PaidCardIds.Count || d.RewardBefore < d.SequenceAfter || d.RewardAfter < d.RewardBefore)
            return false;
        var movements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > d.RewardBefore && m.Sequence <= d.RewardAfter && m.Reason == CardMoveReasons.RecastDraw && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(d.Identity.TargetSeat)).ToArray();
        return movements.Length == d.ActualDrawCount && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SlashTargetPenaltyDrawIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.TargetSeat == d.Identity.TargetSeat && e.RequestedCount == d.PaidCardIds.Count && e.ActualCount == d.ActualDrawCount && e.SequenceBefore == d.RewardBefore && e.SequenceAfter == d.RewardAfter) == 1;
    }

    internal ProgramSkillFrame? TargetPenaltyPaidObserverRoot(long? exactWindow = null)
    {
        for (var i = 1; i < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame root || root.SlashTargetPenaltyDraft is not { Stage: SlashTargetPenaltyStage.CostChildren or SlashTargetPenaltyStage.RewardChildren } d || !_coreSlashTargetPenaltyPaymentsOperations.ValidTargetPenaltyPayment(root) || root.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != d.Identity.TargetSeat || exactWindow is { } id && !_resolutionStackOperations._resolutionStack.Skip(i + 1).Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame or DamageTriggerWindowFrame))
                continue;
            if (d.Stage == SlashTargetPenaltyStage.RewardChildren && !ValidTargetPenaltyReward(root))
                continue;
            if (i == _resolutionStackOperations._resolutionStack.Count - 1)
                return root;
            var first = _resolutionStackOperations._resolutionStack[i + 1];
            var lower = d.Stage == SlashTargetPenaltyStage.CostChildren ? d.SequenceBefore : d.RewardBefore;
            var upper = d.Stage == SlashTargetPenaltyStage.CostChildren ? d.SequenceAfter : d.RewardAfter;
            if (first is CardsMovedTriggerWindowFrame moved)
            {
                if (moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != root.Id || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.Movements.Count == 0 || moved.Batch.Movements.Any(m => m.Sequence <= lower || m.Sequence > upper || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m)))
                    continue;
            }
            else
            {
                var reason = _coreSlashTargetPenaltyPaymentsOperations.SlashTargetPenaltyCostReason(root, d.Recast);
                if (d.Stage != SlashTargetPenaltyStage.CostChildren || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > lower && m.Sequence <= upper && d.PaidCardIds.Contains(m.CardId) && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(d.Identity.TargetSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == reason))
                    continue;
                if (first is HpChangedTriggerWindowFrame hp)
                {
                    if (hp.Change.ParentFrameId != root.Id || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.SourceSeat != d.Identity.TargetSeat || hp.Change.TargetSeat != d.Identity.TargetSeat || hp.Change.Amount != 1)
                        continue;
                }
                else if (first is RecoveryReplacementFrame recovery)
                {
                    if (!_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != d.Identity.TargetSeat || recovery.Attempt.TargetSeat != d.Identity.TargetSeat || recovery.Attempt.Amount != 1 || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Completion.MoveReason?.Value != reason)
                        continue;
                }
                else
                    continue;
            }

            var valid = true;
            for (var j = i + 1; j < _resolutionStackOperations._resolutionStack.Count; j++)
            {
                if (!_corePaidColorDamageClaimObserversOperations.PaidColorDamageClaimObserverEdge(j))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[j] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(j, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(j, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(j, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(j, dying)))
                    break;
            }

            if (valid)
                return root;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1830)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 210)]
    internal bool HasTargetPenaltyDamageObserver(long id) => TargetPenaltyPaidObserverRoot(id)is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 670)]
    internal bool IsTargetPenaltyProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && TargetPenaltyPaidObserverRoot(_coreDamageCursorRuntimeOperations.ActiveDamageTrigger?.Id)is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 100)]
    internal bool IsTargetPenaltyMovement(ProgramSkillFrame root, SkillProgramEffect? effect, ProgramMovementContinuation pending) => pending.BeforeCount == 0 && pending.CoverageResultBind is null && effect?.Op == SkillProgramEffectOp.RequireTargetDiscardOrEquipmentRecast && root.SlashTargetPenaltyDraft is { } d && pending.SubjectSeat == d.Identity.TargetSeat && _coreSlashTargetPenaltyPaymentsOperations.ValidTargetPenaltyPayment(root) && (d.Stage == SlashTargetPenaltyStage.CostChildren || d.Stage == SlashTargetPenaltyStage.RewardChildren && ValidTargetPenaltyReward(root));
}
