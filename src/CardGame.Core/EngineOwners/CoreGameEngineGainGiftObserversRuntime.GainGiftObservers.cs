#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGainGiftObserversRuntime
{
    internal bool ValidGainGiftReceipt(ProgramSkillFrame f)
    {
        if (f.GainGiftReceipt is not { } r || r.InstructionIndex != f.InstructionIndex || _coreGainGiftProgramsOperations.GainGiftPausedEffect(f)?.Op != r.Operation || r.Before < 0 || r.After < r.Before || r.GivenTargets.Count > 2 || r.GivenTargets.Count != r.GivenCards.Count || r.GivenTargets.Distinct().Count() != r.GivenTargets.Count || r.GivenTargets.Contains(f.OwnerSeat) || r.GivenTargets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s)) || r.GivenCards.Distinct().Count() != r.GivenCards.Count || r.LossIndex < 0 || r.LossIndex > r.RedLosses.Count || f.WindowContext is not { } context || context.ParentFrameId != r.OriginalParentId)
            return false;
        if (r.Operation == SkillProgramEffectOp.DelegateJudgmentReplacement)
        {
            if (r.Stage != GainGiftStage.ReplacementChildren || r.BatchId <= 0 || r.PaymentFrom is not { OwnerSeat: var owner } from || owner != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || context.JudgmentReplacement is not { } replacement || r.PaymentCardId != replacement.ReplacementCardId || r.PaymentTarget != replacement.SubjectSeat || _coreJudgmentCursorRuntimeOperations.Value.ActiveJudgment is not { } judgment || judgment.Id != r.OriginalParentId || judgment.DelegatedReplacement is not { Stage: DelegatedJudgmentStage.Paid } draft || !_coreDelegatedJudgmentChoicesOperations.Value.ValidDelegatedJudgmentDraft(judgment, draft) || draft.Source != CoreGameEngineGainGiftProgramsRuntime.GainGiftSource(f) || draft.GameplayHash != f.GameplayHash || draft.SelectedCardId != replacement.ReplacementCardId || draft.OldCardId != replacement.OldCardId || r.After <= r.Before || _coreNativeDamageHandlesOperations.Value.GetJudgmentCard(judgment)?.Id != replacement.ReplacementCardId)
                return false;
            return _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == replacement.ReplacementCardId && m.From == from && m.To == CardLocation.Processing && m.Reason == CardMoveReasons.ProgramJudgmentReplace) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == replacement.ReplacementCardId && m.From == CardLocation.Processing && m.To == CardLocation.Judgment(replacement.SubjectSeat) && m.Reason == CardMoveReasons.ProgramJudgmentReplace) == 1 && _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == replacement.OldCardId && m.From == CardLocation.Judgment(replacement.SubjectSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.ProgramJudgmentOldCard) == 1;
        }

        var parent = _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault(w => w.Id == r.OriginalParentId);
        if (parent is null || context.MovementBatch is not { } original || parent.Batch.Id != original.Id || !parent.Batch.Movements.SequenceEqual(original.Movements) || parent.Batch.Id != r.BatchId || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        if (r.Operation == SkillProgramEffectOp.GiveAfterBatchGain)
        {
            if (context.Window != SkillProgramTriggerWindow.CardsGained || r.RedLosses.Count != 0 || r.LossIndex != 0 || r.Phase is not { } phase || phase != parent.Batch.DiscardRecoveryPhase || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(phase) || _coreGainGiftProgramsOperations.MatchingActualGainGiftIndexes(parent.Batch, f.OwnerSeat, CardLocation.Hand(f.OwnerSeat)).Length < 2 || r.Stage is not (GainGiftStage.Choosing or GainGiftStage.GiftChildren))
                return false;
            if (r.Stage == GainGiftStage.Choosing && _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt || prompt.PlayerSeat != f.OwnerSeat || prompt.TargetSeat != f.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, _coreGainGiftProgramsOperations.GainGiftChoices(f))))
                return false;
            var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GainGiftPhaseIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
            if (r.GivenTargets.Count == 0)
                return r.Stage == GainGiftStage.Choosing && issued.Length == 0 && f.PendingMovementContinuation is null;
            if (issued is not [var fact] || fact.Source != CoreGameEngineGainGiftProgramsRuntime.GainGiftSource(f) || fact.GameplayHash != f.GameplayHash || fact.BatchId != r.BatchId || fact.Phase != phase)
                return false;
            var gifts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GainGiftPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
            bool GiftSettled(GainGiftPaidEvent gift)
            {
                if (gift.After < gift.Before || gift.From.OwnerSeat != f.OwnerSeat || gift.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment))
                    return false;
                var destination = CardLocation.Hand(gift.TargetSeat);
                var reason = new CardMoveReason("program.batch-gain.gift");
                var movements = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > gift.Before && m.Sequence <= gift.After && m.CardId == gift.CardId && m.From == gift.From && m.Reason == reason).ToArray();
                var retained = (f.HappyWeiYanRetainedGains ?? []).Where(item => item.InstructionIndex == r.InstructionIndex && item.Plan.CardId == gift.CardId && item.Plan.From == gift.From && item.Plan.RequestedTo == destination && item.Plan.Reason == reason && item.Plan.SequenceBefore == gift.Before).ToArray();
                return retained is [var payment] ? gift.After == gift.Before && gift.From == CardLocation.Hand(f.OwnerSeat) && movements.Length == 0 && _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRetainedGainSettlement(payment, f.Id, gift.CardId, destination, gift.Before) : retained.Length == 0 && movements is [var movement] && (movement.To == destination || _coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(movement, destination));
            }

            if (gifts.Length != r.GivenTargets.Count || !gifts.Select(e => e.CardId).SequenceEqual(r.GivenCards) || !gifts.Select(e => e.TargetSeat).SequenceEqual(r.GivenTargets) || gifts.Any(gift => !GiftSettled(gift)))
                return false;
            var last = gifts[^1];
            return r.PaymentCardId == last.CardId && r.PaymentFrom == last.From && r.PaymentTarget == last.TargetSeat && r.Before == last.Before && r.After >= last.After;
        }

        if (r.Operation != SkillProgramEffectOp.RevealRedLossAndDraw || context.Window != SkillProgramTriggerWindow.CardsMoved || r.Stage != GainGiftStage.RedDrawChildren || r.LossIndex == 0 || r.GivenCards.Count != 0 || r.GivenTargets.Count != 0 || r.RedLosses.Count == 0 || r.RedLosses.Select(l => l.Card.Id).Distinct().Count() != r.RedLosses.Count || !r.RedLosses.SequenceEqual(_coreCardMovementProgramsOperations.MatchingOwnerBatchMovementIndexes(parent.Batch, parent.Candidates[parent.CandidateIndex], _coreProgramLifecycleOperations.GetProgramTrigger(f)).Select(i => parent.Batch.Movements[i].RedOwnedLoss!)) || r.RedLosses.Any(l => l.OwnerSeat != f.OwnerSeat || l.EffectiveSuit is not (Suit.Heart or Suit.Diamond) || l.Timing.Phase == TurnPhase.Play && l.Timing.PhaseActorSeat == f.OwnerSeat || !parent.Batch.Movements.Any(m => m.RedOwnedLoss == l && m.Sequence == l.MovementSequence && m.CardId == l.Card.Id && m.From.OwnerSeat == f.OwnerSeat && CoreGameEngineGainGiftProgramsRuntime.OwnedLossZone(m.From.Zone) && !(m.To.OwnerSeat == f.OwnerSeat && CoreGameEngineGainGiftProgramsRuntime.OwnedLossZone(m.To.Zone)))) || r.ActualDrawCount is < 0 or > 1)
            return false;
        var reveals = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<RedOwnedLossRevealedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        var draws = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GainGiftDrawPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (reveals.Length != r.LossIndex || draws.Length != r.LossIndex || !reveals.Select(e => e.OriginalMovementSequence).SequenceEqual(r.RedLosses.Take(r.LossIndex).Select(l => l.MovementSequence)) || reveals.Where((e, i) => e.OwnerSeat != f.OwnerSeat || e.Card != r.RedLosses[i].Card || e.EffectiveSuit != r.RedLosses[i].EffectiveSuit).Any() || !draws.Select(e => e.OriginalMovementSequence).SequenceEqual(reveals.Select(e => e.OriginalMovementSequence)) || draws.Any(e => e.ActualCount is < 0 or > 1 || e.After < e.Before || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > e.Before && m.Sequence <= e.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "program.red-owned-loss.draw") != e.ActualCount))
            return false;
        return draws[^1].Before == r.Before && draws[^1].After == r.After && draws[^1].ActualCount == r.ActualDrawCount;
    }

    internal bool GainGiftFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!ValidGainGiftReceipt(root) || root.GainGiftReceipt is not { Stage: not GainGiftStage.Choosing } r || root.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage is not (GainGiftStage.GiftChildren or GainGiftStage.ReplacementChildren) || r.PaymentFrom != CardLocation.Equipment(root.OwnerSeat) || r.PaymentCardId is not { } card || !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == card && m.CardKind == CardKind.SilverLion && m.From == r.PaymentFrom))
            return false;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == root.OwnerSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount == 1;
        var reason = r.Stage == GainGiftStage.GiftChildren ? "program.batch-gain.gift" : CardMoveReasons.ProgramJudgmentReplace.Value;
        return child is RecoveryReplacementFrame recovery && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, root) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == root.OwnerSeat && recovery.Attempt.TargetSeat == root.OwnerSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason?.Value == reason;
    }

    internal ProgramSkillFrame? GainGiftObserverRoot()
    {
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame root || !GainGiftFirstChild(root, _resolutionStackOperations._resolutionStack[i + 1]))
                continue;
            var exact = true;
            for (var j = i + 2; j < _resolutionStackOperations._resolutionStack.Count; j++)
            {
                if (_resolutionStackOperations._resolutionStack[j] is ProgramLifecycleTriggerWindowFrame changed && _resolutionStackOperations._resolutionStack[j - 1] is ProgramSkillFrame owner && changed.Window == SkillProgramTriggerWindow.SkillsChanged && changed.ResumeProgramFrameId == owner.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && changed.CandidateIndex >= 0 && changed.CandidateIndex <= changed.Candidates.Count)
                    continue;
                if (!(_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(j) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(j) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[j - 1], _resolutionStackOperations._resolutionStack[j])))
                {
                    exact = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[j] is DyingFrame dying && j + 1 < _resolutionStackOperations._resolutionStack.Count && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(j, dying) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(j, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(j, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(j, dying) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(j, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(j, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(j, dying)))))
                    break;
            }

            if (exact)
                return root;
        }

        return null;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 200)]
    internal bool IsGainGiftProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && GainGiftObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
}
