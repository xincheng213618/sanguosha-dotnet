#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNamedAcquisitionObserversRuntime
{
    internal bool NamedAcquisitionParentMatches(ProgramSkillFrame root) => root.WindowContext is { Window: SkillProgramTriggerWindow.TurnEnding } c && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)is { } parent && parent.OwnerSeat == root.OwnerSeat && parent.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && parent.ItemIndex >= 0 && parent.ItemIndex < parent.Items.Count && parent.Items[parent.ItemIndex].Candidate is { } candidate && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(root, candidate);
    internal ProgramSkillFrame? NamedAcquisitionObserverRoot(long? damageWindowId = null)
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame { NamedCardAcquisition: { Stage: NamedCardAcquisitionStage.MovementChildren } d } root || !_coreNamedCardAcquisitionsOperations.ValidNamedCardAcquisition(root) || !NamedAcquisitionParentMatches(root) || root.PendingMovementContinuation is not { CoverageResultBind: null } pending || pending.SubjectSeat != root.OwnerSeat || damageWindowId is { } requested && !_resolutionStackOperations._resolutionStack.Skip(index + 1).Any(f => f.Id == requested))
                continue;
            var paid = _coreCardMovementJournalCardMovementsAccess._cardMovements.Single(m => m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter && m.CardId == d.CardId && m.From == d.From && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime.NamedAcquisitionReason(root));
            var first = _resolutionStackOperations._resolutionStack[index + 1];
            if (first is CardsMovedTriggerWindowFrame moved)
            {
                if (moved.Batch.ParentFrameId != root.Id || moved.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != root.Id || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements is not [var one] || one != paid)
                    continue;
            }
            else if (first is HpChangedTriggerWindowFrame hp)
            {
                if (paid.CardKind != CardKind.SilverLion || paid.From.Zone != CardZoneKind.Equipment || hp.ResumeFrameId != root.Id || hp.Continuation != PostEventContinuation.AwaitedProgramMovement || hp.Change.ParentFrameId != root.Id || hp.Change.Kind != HpChangeKind.Recovery || hp.Change.Amount != 1 || hp.Change.SourceSeat != paid.From.OwnerSeat || hp.Change.TargetSeat != paid.From.OwnerSeat)
                    continue;
            }
            else if (first is RecoveryReplacementFrame recovery)
            {
                if (paid.CardKind != CardKind.SilverLion || paid.From.Zone != CardZoneKind.Equipment || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) || recovery.Return.Continuation != PostEventContinuation.AwaitedProgramMovement || recovery.Attempt.SourceSeat != paid.From.OwnerSeat || recovery.Attempt.TargetSeat != paid.From.OwnerSeat || recovery.Attempt.Amount != 1 || recovery.Attempt.Completion.Producer != RecoveryAttemptProducer.SilverLion || recovery.Attempt.Completion.MoveReason?.Value != global::CardGame.Core.EngineOwners.CoreGameEngineNamedCardAcquisitionsRuntime.NamedAcquisitionReason(root))
                    continue;
            }
            else
                continue;
            var aligned = true;
            for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child))
                {
                    aligned = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying)))))
                    break;
            }

            if (aligned)
                return root;
        }

        return null;
    }

    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1710)]
    internal bool HasNamedAcquisitionDamageObserver(long id) => NamedAcquisitionObserverRoot(id)is not null;
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 330)]
    internal bool IsNamedAcquisitionProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is not null && NamedAcquisitionObserverRoot()is not null;
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 40)]
    internal bool CanStartLostHpTargetSelection(CharacterState owner, SkillProgramActivation activation)
    {
        if (!activation.Effects.Any(e => e.NumberExpression == SkillProgramNumberExpression.OwnerLostHpAtLeastOne))
            return true;
        var selection = activation.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.SelectOwnedCards);
        if (selection is null || selection.Target != SkillProgramEffectTarget.Owner)
            return false;
        return selection.Zones.SelectMany(zone => _cardZonesOperations._cardZones.CardsAt(new(zone, owner.Seat))).Any(card => (selection.Suits.Count == 0 || selection.Suits.Contains(_coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card))) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(owner.Seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard));
    }

    internal bool HasLostHpTargetSelection(ProgramSkillFrame frame) => ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Any(e => e.NumberExpression == SkillProgramNumberExpression.OwnerLostHpAtLeastOne);
}
