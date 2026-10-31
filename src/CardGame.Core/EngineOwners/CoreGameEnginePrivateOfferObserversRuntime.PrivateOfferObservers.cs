#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateOfferObserversRuntime
{
    internal bool PrivateOfferFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        long before, after;
        int actor;
        bool draw;
        if (root.PrivateOffer is { } r && _corePrivateOfferReturnsOperations.ValidPrivateOfferReceipt(root))
        {
            if (r.Stage == PrivateOfferStage.HpLoss)
            {
                if (child is DyingFrame dying)
                    return dying.ParentFrameId == root.Id && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.VictimSeat == r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat && _coreDyingCursorRuntimeOperations.ActiveDying?.FrameId == dying.Id && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillHpLostEvent>().Count(e => e.FrameId == root.Id && e.SkillId == root.SkillId && e.TargetSeat == dying.VictimSeat && e.RemainingHp == 0) == 1;
                return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat && hp.Change.Kind == HpChangeKind.Loss;
            }

            if (r.Stage == PrivateOfferStage.ChoosingExchange || root.PendingMovementContinuation is null)
                return false;
            before = r.SequenceBefore;
            after = r.SequenceAfter;
            actor = r.Stage == PrivateOfferStage.DepositChildren ? root.OwnerSeat : r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat;
            draw = false;
        }
        else if (root.GameTargetHandHp is { Stage: "children" } game && _coreGameTargetHandHpChoicesOperations.ValidGameHandHpReceipt(root) && root.PendingMovementContinuation is not null)
        {
            before = game.SequenceBefore;
            after = game.SequenceAfter;
            actor = game.TargetSeat;
            draw = game.Draw;
        }
        else
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId == root.Id && (moved.Batch.AwaitingProgramFrameId is null || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && (m.Sequence > before && m.Sequence <= after || root.GameTargetHandHp is { Draw: false } paid && m.Sequence > after && m.From == CardLocation.WoodenOxGrain(actor) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && paid.CardIds.Any(id => _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(cost => cost.Sequence > before && cost.Sequence <= after && cost.CardId == id && cost.CardKind == CardKind.WoodenOx && cost.From == CardLocation.Equipment(actor) && cost.To == CardLocation.DiscardPile && cost.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime.GameHandHpReason(root)))));
        if (draw || root.GameTargetHandHp is not { Draw: false } discard || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > before && m.Sequence <= after && m.CardKind == CardKind.SilverLion && m.From == CardLocation.Equipment(actor) && m.To == CardLocation.DiscardPile && discard.CardIds.Contains(m.CardId) && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime.GameHandHpReason(root)))
            return false;
        if (child is HpChangedTriggerWindowFrame lion)
            return lion.Change.ParentFrameId == root.Id && lion.ResumeFrameId == root.Id && lion.Continuation == PostEventContinuation.AwaitedProgramMovement && lion.Change.Kind == HpChangeKind.Recovery && lion.Change.SourceSeat == actor && lion.Change.TargetSeat == actor && lion.Change.Amount == 1;
        return child is RecoveryReplacementFrame replacement && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(replacement, root) && replacement.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && replacement.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && replacement.Attempt.SourceSeat == actor && replacement.Attempt.TargetSeat == actor && replacement.Attempt.Amount == 1 && replacement.Attempt.Completion.MoveReason?.Value == global::CardGame.Core.EngineOwners.CoreGameEngineGameTargetHandHpChoicesRuntime.GameHandHpReason(root);
    }

    internal ProgramSkillFrame? PrivateOfferObserverRoot()
    {
        for (var index = 0; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || !PrivateOfferFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]))
                continue;
            if (_resolutionStackOperations._resolutionStack[index + 1] is DyingFrame original && index + 2 < _resolutionStackOperations._resolutionStack.Count && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(index + 1, original) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(index + 1, original) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(index + 1, original) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(index + 1, original) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(index + 1, original) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(index + 1, original)))))
                return root;
            var exact = true;
            for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!(_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child)))
                {
                    exact = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && child + 1 < _resolutionStackOperations._resolutionStack.Count && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying)))))
                    break;
            }

            if (exact)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 300)]
    internal bool IsPrivateOfferProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && PrivateOfferObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
