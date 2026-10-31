#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCappedConversionBenefitObserversRuntime
{
    internal ProgramSkillFrame? CappedConversionObserverRoot(long? damageWindowId = null)
    {
        for (var index = 1; index + 1 < _resolutionStackOperations._resolutionStack.Count; index++)
        {
            if (_resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || root.CappedConversionBenefit is not { AwaitingMovement: true, ActualDrawAttempted: true } r || !_coreCappedConversionBenefitsOperations.Value.ValidCappedConversionBenefit(root) || _resolutionStackOperations._resolutionStack[index - 1] is not DamageTriggerWindowFrame original || original.Id != r.DamageWindowFrameId || damageWindowId is { } requested && original.Id != requested && !_resolutionStackOperations._resolutionStack.Skip(index + 1).Any(w => w.Id == requested))
                continue;
            if (_resolutionStackOperations._resolutionStack[index + 1] is not CardsMovedTriggerWindowFrame movement || movement.Batch.ParentFrameId != root.Id || movement.Batch.AwaitingProgramFrameId != root.Id || movement.ResumeProgramFrameId is { } resume && resume != root.Id || movement.Batch.OriginOwnerSeat != root.OwnerSeat || movement.Batch.OriginSkillId != root.SkillId || movement.Batch.OriginSkillInstanceId != root.SkillInstanceId || movement.Batch.Movements is not [var actual] || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(actual) || actual.Sequence <= r.SequenceBefore || actual.Sequence > r.SequenceAfter || actual.From != CardLocation.DrawPile || actual.To != CardLocation.Hand(root.OwnerSeat) || actual.Reason.Value != $"skill-program.{(root.SkillId)}.capped-conversion-draw")
                continue;
            var aligned = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                // The incoming edge must be proved before a full rescue suffix
                // can cover the remainder. No mere Dying/CardUse presence.
                if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child))
                {
                    aligned = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && child + 1 < _resolutionStackOperations._resolutionStack.Count && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying)))))
                    break;
            }

            if (aligned)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 690)]
    internal bool IsCappedConversionBenefitProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is not null && CappedConversionObserverRoot()is not null;
}
