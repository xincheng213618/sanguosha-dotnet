#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDynamicDiscardDamageObserversRuntime
{
    internal bool DynamicDiscardDamageFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.DynamicDiscardDamage is not { } r || r.Stage == DynamicDiscardDamageStage.Choosing || !_coreDynamicDiscardDamagePaymentsOperations.ValidDynamicDiscardDamagePayments(root))
            return false;
        var effect = ProgramInstructionResolver.Default.Resolve(root, _contentRegistryOperations._contentRegistry.GetSkill(root.SkillId).Program!).GetPausedInstruction(root.InstructionIndex).Effect;
        if (effect.Op != SkillProgramEffectOp.DiscardTargetHpCardsAndDamage || r.InstructionIndex != root.InstructionIndex)
            return false;
        if (r.Stage == DynamicDiscardDamageStage.Paid && root.PendingMovementContinuation is { SubjectSeat: var subject, BeforeCount: 0, CoverageResultBind: null } && subject == root.OwnerSeat)
        {
            if (child is CardsMovedTriggerWindowFrame movement)
                return movement.Batch.ParentFrameId == root.Id && (movement.Batch.AwaitingProgramFrameId is null || movement.Batch.AwaitingProgramFrameId == root.Id) && movement.Batch.OriginOwnerSeat == root.OwnerSeat && movement.Batch.OriginSkillId == root.SkillId && movement.Batch.OriginSkillInstanceId == root.SkillInstanceId && movement.Batch.Movements.Count > 0 && movement.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && r.CardIds.Contains(m.CardId) && m.Sequence > r.PaymentSequenceBefore && m.Sequence <= r.PaymentSequenceAfter && m.Reason.Value == "program.dynamic-target-hp.discard");
            var removedLion = r.CardIds.Select((id, i) => (id, i)).Any(x => r.OriginalLocations[x.i] == CardLocation.Equipment(root.OwnerSeat) && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.PaymentSequenceBefore && m.Sequence <= r.PaymentSequenceAfter && m.CardId == x.id && m.CardKind == CardKind.SilverLion));
            if (!removedLion)
                return false;
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat;
            return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == "program.dynamic-target-hp.discard";
        }

        if (r.Stage == DynamicDiscardDamageStage.DamageIssued && root.AttackAttempt is { } attack && root.AttackReturn is not null)
        {
            if (child is DyingFrame { Continuation: DyingContinuationKind.AttackHpLoss } replaced)
                return _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverAttackHpLossDyingMatches(root, replaced);
            if (child is BeforeDamageProgramWindowFrame before)
                return before.ParentFrameId == root.Id && before.Continuation == BeforeDamageProgramContinuation.Attack && (before.ContinuationAttackResolutionId is null || before.ContinuationAttackResolutionId == root.Id) && before.SourceSeat == attack.SourceSeat && (before.RedirectedTargetSeat ?? before.TargetSeat) == attack.TargetSeat && before.Amount > 0 && before.Nature == attack.Nature;
            return child is DamageFrame damage && damage.Id == r.DamageFrameId && damage.ParentFrameId == root.Id && damage.SourceSeat == root.OwnerSeat && damage.TargetSeat == r.ActualDamageTargetSeat && damage.Amount == attack.DamageAmount && damage.Nature == DamageNature.Normal;
        }

        if (r.Stage == DynamicDiscardDamageStage.DamageCompleted && child is HpChangedTriggerWindowFrame replacementHp)
            return replacementHp.ResumeFrameId == root.Id && replacementHp.Change.ParentFrameId == root.Id && replacementHp.Continuation == PostEventContinuation.Program && replacementHp.Change.Kind == HpChangeKind.Loss && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<DamageReplacedWithHpLossEvent>().Any(e => e.ParentResolutionId == root.Id && e.PolicyOwnerSeat == root.OwnerSeat && e.TargetSeat == replacementHp.Change.TargetSeat && e.Amount == replacementHp.Change.Amount);
        if (r.Stage == DynamicDiscardDamageStage.PenaltyIssued)
        {
            if (child is HpChangedTriggerWindowFrame hp)
                return hp.ResumeFrameId == root.Id && hp.Change.ParentFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Loss && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1;
            return child is DyingFrame dying && dying.ParentFrameId == root.Id && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.VictimSeat == root.OwnerSeat && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillHpLostEvent>().Any(e => e.FrameId == root.Id && e.TargetSeat == root.OwnerSeat && e.Amount == 1);
        }

        return false;
    }

    internal ProgramSkillFrame? DynamicDiscardDamageObserverRoot(long? exactWindow = null)
    {
        for (var rootIndex = 0; rootIndex + 1 < _resolutionStackOperations._resolutionStack.Count; rootIndex++)
        {
            if (_resolutionStackOperations._resolutionStack[rootIndex] is not ProgramSkillFrame root || !DynamicDiscardDamageFirstChild(root, _resolutionStackOperations._resolutionStack[rootIndex + 1]))
                continue;
            if (exactWindow is { } id && !_resolutionStackOperations._resolutionStack.Skip(rootIndex + 1).Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame or DamageTriggerWindowFrame))
                continue;
            var valid = true;
            for (var index = rootIndex + 1; index < _resolutionStackOperations._resolutionStack.Count; index++)
            {
                if (index != rootIndex + 1 && !_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(index) && !_coreRequestedDeckBasicOwnershipOperations.RequestedDeckBasicFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]))
                {
                    valid = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[index] is DyingFrame d && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(index, d) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(index, d) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(index, d) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(index, d) || _coreRoundPricedPileAlcoholOperations.IsRoundPricedPileAlcoholRide(index, d)))
                    break;
            }

            if (valid)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 340)]
    internal bool IsDynamicDiscardDamageProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && DynamicDiscardDamageObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 560)]
    internal bool TryAdvanceDynamicDiscardDamageSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || DynamicDiscardDamageObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: false);
    }
}
