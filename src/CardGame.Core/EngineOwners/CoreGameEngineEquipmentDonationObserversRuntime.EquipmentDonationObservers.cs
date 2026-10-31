#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineEquipmentDonationObserversRuntime
{
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCardCount, 30)]
    internal bool AllowsDonateAllEquipmentAndOfferRecipientBenefitsActivation(ProgramActivationLegalityContext gate)
    {
        var owner = gate.Owner;
        var program = gate.Program;
        var features = gate.Features;
        return !(features.First(SkillProgramEffectOp.DonateAllEquipmentAndOfferRecipientBenefits)is { } donation && !_coreEquipmentDonationProgramsOperations.HasPayableAllEquipmentDonation(owner.Seat, program.Id, donation.StateId!));
    }

    internal bool IsValidAllEquipmentDonation(ProgramSkillFrame f)
    {
        if (f.EquipmentDonation is not { } r || f.TriggerId is not null || r.InstructionIndex != 0 || f.InstructionIndex != 1 || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.SelectedTargetSeats is not [var recipient] || recipient != r.RecipientSeat || recipient == f.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || r.PaidCardIds.Count == 0 || r.PaidCardIds.Distinct().Count() != r.PaidCardIds.Count || r.ActualDeliveredCount < 0 || r.ActualDeliveredCount > r.PaidCardIds.Count || r.SequenceBefore < 0 || r.SequenceAfter <= r.SequenceBefore || r.DamageCursor < 0 || r.DamageCursor > r.DamageTargets.Count || r.DamageTargets.Count > r.ActualDeliveredCount || r.DamageTargets.Distinct().Count() != r.DamageTargets.Count || r.DamageTargets.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == recipient) || _skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineEquipmentDonationProgramsRuntime.EquipmentDonationUsage(r.UsageId), SkillUsageScope.Game) != 1)
            return false;
        var instructions = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions;
        if (instructions.Count != 1 || instructions[0].Op is not (SkillProgramEffectOp.DonateAllEquipmentAndOfferRecipientBenefits or SkillProgramEffectOp.ResolveOrdinaryCaiFuRenXianzhou) || instructions[0].StateId != r.UsageId)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<EquipmentDonationPaidEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.UsageId == r.UsageId && e.TurnNumber == r.TurnNumber && e.TurnOwnerSeat == r.TurnOwnerSeat && e.RecipientSeat == recipient && e.PaidCount == r.PaidCardIds.Count && e.ActualDeliveredCount == r.ActualDeliveredCount && e.SequenceBefore == r.SequenceBefore && e.SequenceAfter == r.SequenceAfter) != 1)
            return false;
        var facts = history.OfType<EquipmentDonationEntityPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (!facts.Select(e => e.CardId).SequenceEqual(r.PaidCardIds) || facts.Where((e, i) => e.PaidIndex != i || e.Source != r.Source || e.RecipientSeat != recipient || e.MovementSequence <= r.SequenceBefore || e.MovementSequence > r.SequenceAfter).Any())
            return false;
        var movement = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter).ToArray();
        foreach (var fact in facts)
        {
            if (movement.Count(m => m.Sequence == fact.MovementSequence && m.CardId == fact.CardId && m.From == CardLocation.Equipment(f.OwnerSeat) && m.To == CardLocation.Processing && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(m.CardKind) && m.Reason.Value == "program.all-equipment-donation.give") != 1)
                return false;
            if (fact.Delivered != (movement.Count(m => m.CardId == fact.CardId && m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient) && m.Reason.Value == "program.all-equipment-donation.give") == 1))
                return false;
        }

        var paidOx = movement.Any(m => r.PaidCardIds.Contains(m.CardId) && m.CardKind == CardKind.WoodenOx && m.From == CardLocation.Equipment(f.OwnerSeat) && m.To == CardLocation.Processing && m.Reason.Value == "program.all-equipment-donation.give");
        return facts.Count(e => e.Delivered) == r.ActualDeliveredCount && movement.All(m => paidOx && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard || r.PaidCardIds.Contains(m.CardId) && m.Reason.Value == "program.all-equipment-donation.give" && (m.From == CardLocation.Equipment(f.OwnerSeat) && m.To == CardLocation.Processing || m.From == CardLocation.Processing && m.To == CardLocation.Hand(recipient)));
    }

    internal bool IsValidActualEndedEquipment(ProgramSkillFrame f)
    {
        if (f.ActualEndedEquipment is not { } r || !_coreActualEndedTurnEquipmentOperations.MatchesActualEndedEquipmentRoot(f) || r.InstructionIndex != 0 || f.InstructionIndex != 1 || r.Source != new CardConversionSource(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId) || r.GameplayHash != f.GameplayHash || r.ParentFrameId != f.WindowContext!.ParentFrameId || r.OccurrenceIndex != f.WindowContext.OccurrenceIndex || r.Qualification != f.WindowContext.ActualEndedEquipment || r.Qualification.MaximumDistinctOptions is < 1 or > 2 || (r.EquipmentIssued ? 1 : 0) + (r.DrawIssued ? 1 : 0) > r.Qualification.MaximumDistinctOptions)
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ActualEndedTurnEquipmentOptionIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray();
        if (facts.Length != (r.EquipmentIssued ? 1 : 0) + (r.DrawIssued ? 1 : 0) || facts.Select(e => e.Option).Distinct().Count() != facts.Length || facts.Any(e => e.Source != r.Source || e.GameplayHash != r.GameplayHash || e.TurnNumber != r.Qualification.TurnNumber || e.TurnOwnerSeat != r.Qualification.TurnOwnerSeat || e.MaximumDistinctOptions != r.Qualification.MaximumDistinctOptions || e.SequenceBefore < 0 || e.SequenceAfter < e.SequenceBefore))
            return false;
        foreach (var fact in facts)
        {
            var ledger = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > fact.SequenceBefore && m.Sequence <= fact.SequenceAfter).ToArray();
            if (fact.Option == "draw")
            {
                if (!r.DrawIssued || fact.EquipmentCardId is not null || fact.ReplacedEquipmentCardId is not null || fact.ReplacedGeneralWeapon || fact.ActualDrawCount is < 0 or > 1 || ledger.Count(m => m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "program.actual-ended-turn.draw") != fact.ActualDrawCount || ledger.Any(m => !(m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "program.actual-ended-turn.draw") && !(m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle)))
                    return false;
            }
            else if (fact.Option == "equipment")
            {
                var replacements = ledger.Where(m => m.From == CardLocation.Equipment(f.OwnerSeat) && m.Reason == CardMoveReasons.EquipmentReplace).ToArray();
                if (fact.ReplacedEquipmentCardId is { } replaced)
                {
                    if (replaced == fact.EquipmentCardId || replacements is not [var replacement] || replacement.CardId != replaced || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(replacement.CardKind) || _coreAdvancedLifecycleOperations.GetAdvancedCard(replaced).IsGeneralWeapon != fact.ReplacedGeneralWeapon || replacement.To != (fact.ReplacedGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile))
                        return false;
                }
                else if (fact.ReplacedGeneralWeapon || replacements.Length != 0)
                    return false;
                if (!r.EquipmentIssued || fact.EquipmentCardId is not { } id || fact.ActualDrawCount != 0 || ledger.Count(m => m.CardId == id && m.From == CardLocation.Equipment(r.Qualification.TurnOwnerSeat) && m.To == CardLocation.Equipment(f.OwnerSeat) && m.Reason.Value == "program.actual-ended-turn.equipment") != 1 || ledger.Any(m => !(m.CardId == id && m.From == CardLocation.Equipment(r.Qualification.TurnOwnerSeat) && m.To == CardLocation.Equipment(f.OwnerSeat) && m.Reason.Value == "program.actual-ended-turn.equipment") && !(fact.ReplacedEquipmentCardId == m.CardId && m.From == CardLocation.Equipment(f.OwnerSeat) && m.To == (fact.ReplacedGeneralWeapon ? CardLocation.OutsideGame : CardLocation.DiscardPile) && m.Reason == CardMoveReasons.EquipmentReplace) && !(ledger.Any(ox => ox.CardId == id && ox.CardKind == CardKind.WoodenOx && ox.From == CardLocation.Equipment(r.Qualification.TurnOwnerSeat) && ox.To == CardLocation.Equipment(f.OwnerSeat)) && m.From == CardLocation.WoodenOxGrain(r.Qualification.TurnOwnerSeat) && m.To == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.Reason == CardMoveReasons.WoodenOxTransfer) && !(ledger.Any(ox => ox.CardKind == CardKind.WoodenOx && ox.From == CardLocation.Equipment(f.OwnerSeat) && ox.To == CardLocation.DiscardPile) && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard)))
                    return false;
            }
            else
                return false;
        }

        if (facts.Length == 0)
            return r.LastPayment is null;
        var last = facts[^1];
        return r.LastPayment is { } payment && payment.Option == last.Option && payment.EquipmentCardId == last.EquipmentCardId && payment.SequenceBefore == last.SequenceBefore && payment.SequenceAfter == last.SequenceAfter && payment.ActualDrawCount == last.ActualDrawCount && payment.ReplacedEquipmentCardId == last.ReplacedEquipmentCardId && payment.ReplacedGeneralWeapon == last.ReplacedGeneralWeapon;
    }

    internal bool IsEquipmentDonationAttack(ProgramSkillFrame f)
    {
        if (!IsValidAllEquipmentDonation(f) || f.EquipmentDonation is not { Stage: EquipmentDonationStage.Damaging, DamageCursor: > 0 } r || f.AttackAttempt is not { } attack || f.AttackReturn is null || attack.SourceSeat != r.RecipientSeat || attack.SourceLess || attack.IsChainPropagation || attack.Nature != DamageNature.Normal)
            return false;
        var original = r.DamageTargets[r.DamageCursor - 1];
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        if (history.OfType<EquipmentDonationBenefitIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Source == r.Source && e.RecipientSeat == r.RecipientSeat && e.Benefit == "damage" && e.ActualDeliveredCount == r.ActualDeliveredCount && e.TargetSeat == original && e.Ordinal == r.DamageCursor - 1) != 1)
            return false;
        // The original selected target remains frozen. Native damage redirection owns
        // the changed current recipient and its real transfer fact; it never rewrites X.
        return !attack.DamageRedirected ? attack.TargetSeat == original : history.OfType<ProgramDamageTransferredEvent>().Count(e => e.ResolutionId == f.Id && e.SourceSeat == r.RecipientSeat && e.OwnerSeat == original && e.TargetSeat == attack.TargetSeat && e.DamageAmount > 0 && e.Nature == attack.Nature) == 1;
    }

    internal bool EquipmentDonationInitialObserverEdge(int rootIndex, ProgramSkillFrame root)
    {
        var child = _resolutionStackOperations._resolutionStack[rootIndex + 1];
        long before, after;
        if (root.EquipmentDonation is { Stage: EquipmentDonationStage.PaidMovement } donation)
        {
            before = donation.SequenceBefore;
            after = donation.SequenceAfter;
        }
        else if (root.ActualEndedEquipment is { Stage: ActualEndedTurnEquipmentStage.Moving, LastPayment: { } payment })
        {
            before = payment.SequenceBefore;
            after = payment.SequenceAfter;
        }
        else if (root.EquipmentDonation is { Stage: EquipmentDonationStage.RecoveryIssued } recovery)
        {
            if (child is RecoveryReplacementFrame replaced)
                return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(replaced, root) && replaced.Return.Continuation == PostEventContinuation.Program && replaced.Attempt.SourceSeat == recovery.RecipientSeat && replaced.Attempt.TargetSeat == root.OwnerSeat && replaced.Attempt.Amount > 0 && replaced.Attempt.Amount <= recovery.ActualDeliveredCount && replaced.Attempt.Completion.Producer == RecoveryAttemptProducer.Program;
            return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == recovery.RecipientSeat && hp.Change.TargetSeat == root.OwnerSeat && hp.Change.Amount > 0 && hp.Change.Amount <= recovery.ActualDeliveredCount;
        }
        else
            return false;
        if (root.PendingMovementContinuation is not { SubjectSeat: var subject } || subject != root.OwnerSeat)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && (moved.Batch.AwaitingProgramFrameId is null || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > before && m.Sequence <= after);
        var removals = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= after && m.CardKind == CardKind.SilverLion && m.From.Zone == CardZoneKind.Equipment && m.From.OwnerSeat is not null && m.To != m.From).ToArray();
        if (child is HpChangedTriggerWindowFrame lion)
            return lion.Change.ParentFrameId == root.Id && lion.ResumeFrameId == root.Id && lion.Continuation == PostEventContinuation.AwaitedProgramMovement && lion.Change.Kind == HpChangeKind.Recovery && lion.Change.Amount == 1 && lion.Change.SourceSeat == lion.Change.TargetSeat && removals.Any(m => m.From.OwnerSeat == lion.Change.TargetSeat);
        return child is RecoveryReplacementFrame silver && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(silver, root) && silver.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && silver.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && silver.Attempt.Amount == 1 && silver.Attempt.SourceSeat == silver.Attempt.TargetSeat && removals.Any(m => m.From.OwnerSeat == silver.Attempt.TargetSeat && silver.Attempt.Completion.MoveReason == m.Reason);
    }

    internal ProgramSkillFrame? EquipmentDonationObserverRoot(long rootId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == rootId);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || !(root.EquipmentDonation is not null && IsValidAllEquipmentDonation(root) || root.ActualEndedEquipment is not null && IsValidActualEndedEquipment(root)))
            return null;
        if (index == _resolutionStackOperations._resolutionStack.Count - 1 || root.AttackAttempt is not null && IsEquipmentDonationAttack(root))
            return root;
        if (!EquipmentDonationInitialObserverEdge(index, root))
            return null;
        for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            // 621's local union retains exact dying-program/hash/turn-over proofs; it does not broaden the old edge.
            if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child))
                return null;
            if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying)))
                break;
        }

        return root;
    }
    internal bool HasEquipmentDonationDamageObserver(long? damageWindowId = null)
    {
        foreach (var root in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(IsEquipmentDonationAttack))
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
            if (damageWindowId is { } id && !_resolutionStackOperations._resolutionStack.Skip(index + 1).Any(f => f.Id == id && f is DamageTriggerWindowFrame))
                continue;
            var aligned = true;
            for (var child = index + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!_coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child))
                {
                    aligned = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying)))
                    break;
            }

            if (aligned)
                return true;
        }

        return false;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 520)]
    internal bool IsEquipmentDonationProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { ResumesProgramSkill: true } && (HasEquipmentDonationDamageObserver() || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => (f.EquipmentDonation is not null || f.ActualEndedEquipment is not null) && f.AttackAttempt is null && EquipmentDonationObserverRoot(f.Id)is not null));
}
