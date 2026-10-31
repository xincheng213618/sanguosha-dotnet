#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHappyWeiYanGainDestinationsRuntime
{
    internal HappyWeiYanGainDestinationPlan? PlanHappyWeiYanGainDestination(Card card, CardLocation from, CardLocation requestedTo, CardMoveReason reason)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.ResolveHappyWeiYanGujing))
            return null;
        var actualTo = _characterWeiYanHappyGodOperations.NormalizeHappyWeiYanGainDestination(card, from, requestedTo, reason);
        if (actualTo == requestedTo)
            return null;
        if (requestedTo is not { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } || actualTo is not { Zone: CardZoneKind.Hand, OwnerSeat: { } issuer } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(issuer) || !EquipmentCatalog.IsEquipment(card.Kind) || !_coreMatchParticipantsPlayersAccess._players[issuer].IsAlive || _coreMatchParticipantsPlayersAccess._players[issuer].HappyWeiYanGujing is not { } issue)
            throw new InvalidOperationException("A gain replacement lost its original requested and actual hands.");
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var fact = new HappyWeiYanGujingIssuedEvent(issue.FrameId, issue.Source, issue.GameplayHash);
        var matches = history.Select((payload, index) => (payload, index)).Where(item => Equals(item.payload, fact)).ToArray();
        var slot = EquipmentCatalog.Get(card.Kind).Slot;
        var capacity = _coreMatchParticipantsPlayersAccess._players[recipient].EquipmentSlotCapacity(slot);
        if (matches is not [var issued] || issue.Source.OwnerSeat != issuer || issue.Source.SkillId != HappyWeiYanComposition.Gujing || _contentRegistryOperations._contentRegistry.GetSkill(issue.Source.SkillId).Program?.GameplayHash != issue.GameplayHash || !_coreMatchParticipantsPlayersAccess._players[recipient].EquipmentAreaAbolished && capacity > 0)
            throw new InvalidOperationException("A gain replacement has no unique prior limited issue or abolished receiving slot.");
        return new(card.Id, card.Kind, from, requestedTo, actualTo, reason, slot, _coreMatchParticipantsPlayersAccess._players[recipient].EquipmentAreaAbolished, capacity, issue.FrameId, issue.Source, issue.GameplayHash, issued.index, history.Length, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence);
    }

    internal void ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement)
    {
        if (movement.HappyWeiYanGainDestination is not null && !IsHappyWeiYanRedirectedGain(movement, movement.HappyWeiYanGainDestination.RequestedTo))
            throw new InvalidOperationException("A redirected gain lost its frozen original destination and historical issuer.");
    }

    internal bool IsHappyWeiYanRedirectedGain(CardMovementRecord movement, CardLocation expectedTo)
    {
        if (movement.HappyWeiYanGainDestination is not { } plan || plan.RequestedTo != expectedTo || !IsHappyWeiYanGainDestinationPlan(plan) || movement.From == movement.To || movement.CardId != plan.CardId || movement.CardKind != plan.PrintedKind || movement.From != plan.From || movement.To != plan.ActualTo || movement.Reason != plan.Reason || movement.Sequence <= plan.SequenceBefore || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(actual => actual.Sequence == movement.Sequence && actual == movement) != 1)
            return false;
        return true;
    }

    internal bool IsHappyWeiYanGainDestinationPlan(HappyWeiYanGainDestinationPlan plan)
    {
        if (plan.RequestedTo is not { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } || plan.ActualTo is not { Zone: CardZoneKind.Hand, OwnerSeat: { } issuer } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(recipient) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(issuer) || recipient == issuer || plan.SequenceBefore < 0 || plan.SequenceBefore > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !EquipmentCatalog.IsEquipment(plan.PrintedKind) || EquipmentCatalog.Get(plan.PrintedKind).Slot != plan.Slot || !plan.RecipientEquipmentAreaAbolished && plan.RecipientSlotCapacity > 0 || plan.IssueFrameId <= 0 || plan.Source.OwnerSeat != issuer || plan.Source.SkillId != HappyWeiYanComposition.Gujing || _contentRegistryOperations._contentRegistry.GetSkill(plan.Source.SkillId).Program?.GameplayHash != plan.GameplayHash)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var issued = new HappyWeiYanGujingIssuedEvent(plan.IssueFrameId, plan.Source, plan.GameplayHash);
        return plan.IssueEventIndex >= 0 && plan.IssueEventIndex < plan.EventCountBefore && plan.EventCountBefore <= history.Length && Equals(history[plan.IssueEventIndex], issued) && history.Count(payload => Equals(payload, issued)) == 1;
    }

    internal bool TrySettleHappyWeiYanRetainedGain(Card card, CardLocation from, CardLocation requestedTo, CardMoveReason reason, long? producerFrameId = null)
    {
        var plan = PlanHappyWeiYanGainDestination(card, from, requestedTo, reason);
        return plan is not null && TrySettleHappyWeiYanRetainedGain(plan, producerFrameId);
    }

    internal bool TrySettleHappyWeiYanRetainedGain(HappyWeiYanGainDestinationPlan plan, long? producerFrameId = null)
    {
        if (plan.ActualTo != plan.From)
            return false;
        SettleHappyWeiYanRetainedGain(plan, producerFrameId);
        return true;
    }

    internal HappyWeiYanRetainedGainSettlement SettleHappyWeiYanRetainedGain(HappyWeiYanGainDestinationPlan plan, long? producerFrameId = null)
    {
        var producer = producerFrameId is { } id ? _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == id) : _resolutionStackOperations._resolutionStack.LastOrDefault();
        if (producer is null || producer.Step == ResolutionFrameStep.Completed || plan.From != plan.ActualTo || plan.From.Zone != CardZoneKind.Hand || plan.SequenceBefore != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !IsHappyWeiYanGainDestinationPlan(plan) || !_cardZonesOperations._cardZones.CardsAt(plan.From).Any(card => card.Id == plan.CardId && card.Kind == plan.PrintedKind))
            throw new InvalidOperationException("A retained gain requires its original live producer and unchanged physical hand entity.");
        var program = producer as ProgramSkillFrame;
        var previous = producer.HappyWeiYanRetainedGains ?? [];
        var settlement = new HappyWeiYanRetainedGainSettlement(producer.Id, producer.Kind, program?.InstructionIndex, program?.SkillId, program?.ActivationId, program?.SkillInstanceId, program?.GameplayHash, previous.Count, plan);
        if (previous.Any(item => item.InstructionIndex == settlement.InstructionIndex && item.Plan == plan))
            throw new InvalidOperationException("The original gain request was already settled without a movement.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(producer.Id, producer with { HappyWeiYanRetainedGains = previous.Append(settlement).ToArray() });
        return settlement;
    }

    internal bool IsHappyWeiYanRetainedGainSettlement(HappyWeiYanRetainedGainSettlement settlement, long producerFrameId, int cardId, CardLocation requestedTo, long before)
    {
        var producer = _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == producerFrameId);
        if (producer is null || settlement.ProducerFrameId != producer.Id || settlement.ProducerKind != producer.Kind || settlement.Ordinal < 0 || producer.HappyWeiYanRetainedGains is not { } receipts || settlement.Ordinal >= receipts.Count || receipts[settlement.Ordinal] != settlement || receipts.Count(item => item == settlement) != 1 || settlement.Plan.CardId != cardId || settlement.Plan.RequestedTo != requestedTo || settlement.Plan.SequenceBefore < before || settlement.Plan.From != settlement.Plan.ActualTo || settlement.Plan.From.Zone != CardZoneKind.Hand || !IsHappyWeiYanGainDestinationPlan(settlement.Plan))
            return false;
        return producer is ProgramSkillFrame program ? settlement.InstructionIndex is { } instruction && instruction >= 0 && instruction <= program.InstructionIndex && settlement.ProducerSkillId == program.SkillId && settlement.ProducerBindingId == program.ActivationId && settlement.ProducerSkillInstanceId == program.SkillInstanceId && settlement.ProducerGameplayHash == program.GameplayHash : settlement.InstructionIndex is null && settlement.ProducerSkillId is null && settlement.ProducerBindingId is null && settlement.ProducerSkillInstanceId is null && settlement.ProducerGameplayHash is null;
    }

    internal void ValidateHappyWeiYanGainRequestSettlement(HappyWeiYanGainRequestSettlement settlement, long producerFrameId, IReadOnlyList<int> intendedIds, CardLocation requestedTo, long before)
    {
        var actual = settlement.Movements;
        var retained = settlement.RetainedGains;
        if (settlement.ProducerFrameId != producerFrameId || settlement.SequenceBefore != before || before < 0 || before > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || intendedIds.Count == 0 || intendedIds.Distinct().Count() != intendedIds.Count || (actual.Count == 0 ? settlement.MovementBatchId is not null : settlement.MovementBatchId is not > 0) || actual.Select(move => move.CardId).Concat(retained.Select(receipt => receipt.Plan.CardId)).Order().SequenceEqual(intendedIds.Order())is false || actual.Any(move => move.Sequence <= before || move.Reason != settlement.Reason || move.From == move.To || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move) || move.To != requestedTo && move.To != CardLocation.OutsideGame && !IsHappyWeiYanRedirectedGain(move, requestedTo)) || retained.Any(receipt => receipt.Plan.SequenceBefore != before || receipt.Plan.Reason != settlement.Reason || !IsHappyWeiYanRetainedGainSettlement(receipt, producerFrameId, receipt.Plan.CardId, requestedTo, before)))
            throw new InvalidOperationException("A gain request lost its complete actual-movement and retained-entity settlement.");
    }

    internal void MoveNativeGivenCards(long producerFrameId, int giver, int recipient, IReadOnlyList<int> cardIds, CardMoveReason reason, Action<HappyWeiYanGainRequestSettlement>? afterRequestSettled = null, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
    {
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cardIds, CardLocation.Hand(recipient), reason, disclosure: disclosure, afterRequestSettled: _characterHuangQuanMouOperations.NativeGiveRequestCompletion(producerFrameId, giver, recipient, cardIds, before, afterRequestSettled), producerFrameId: producerFrameId);
    }
}
