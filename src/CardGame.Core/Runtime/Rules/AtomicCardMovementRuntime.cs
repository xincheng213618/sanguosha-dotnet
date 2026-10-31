namespace CardGame.Core;
/// <summary>Executes single and batch physical movement through the same ordered rule boundaries.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class AtomicCardMovementRuntime(IAtomicCardMovementFacts facts, IAtomicCardMovementPolicies policies, IAtomicCardMovementZones zones, IAtomicCardMovementBatches batches, IAtomicCardMovementEffects effects, ICardMovementEventSuppression suppression) : IPhysicalCardDrawMovement, INativeLogicalDiscardMovement
{
    void INativeLogicalDiscardMovement.Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact, CardMovementDisclosure disclosure) => Move(card, from, to, reason, beforeFact, disclosure: disclosure);
    void INativeLogicalDiscardMovement.MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact, CardMovementDisclosure disclosure) => MoveMany(cards, from, to, reason, beforeFact, disclosure: disclosure);
    void IPhysicalCardDrawMovement.Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => Move(card, from, to, reason);
    void IPhysicalCardDrawMovement.MoveAll(CardLocation from, CardLocation to, CardMoveReason reason) => MoveAll(from, to, reason);
    internal void Move(Card card, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, bool emitCardMovedEvents = true, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
    {
        if (policies.ClaimDeclarationPayment(card, from, to))
            return;
        var phantomDestination = policies.PlanPhantomDestination(card, from, to, reason);
        to = policies.NormalizeViewAsCostDestination(card, from, to, reason);
        to = policies.NormalizeEphemeralDestination(card, from, to, reason);
        to = policies.NormalizeCopiedDestination(card, from, to, reason);
        to = policies.NormalizeDiscardDestination(card, from, to, reason);
        if ((facts.IsCarrier(card.Id) || facts.IsPurchasedEquipment(card.Id) || facts.IsStartingResource(card.Id)) && from == to)
            return;
        if (card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment || policies.IsGeneratedDeparture(card, from, to))
            to = CardLocation.OutsideGame;
        var happyGainDestination = policies.PlanGainDestination(card, from, to, reason);
        if (happyGainDestination is { } retainedPlan && retainedPlan.ActualTo == from)
        {
            if (beforeFact is not null)
                throw new InvalidOperationException("A retained gain requires its producer's typed request settlement instead of a physical-movement callback.");
            if (policies.SettleRetainedGain(retainedPlan))
                return;
        }

        to = happyGainDestination?.ActualTo ?? to;
        if (policies.NormalizePhantomDestination(card, from, to) != to)
            throw new InvalidOperationException("A second destination replacement cannot bypass a phantom's original request invoice.");
        var shiyou = policies.CanPlanDiscardTransfers(to) ? policies.PrepareDiscardTransfers([new CardTransfer(card.Id, from, to)], reason) : null;
        to = shiyou?.Transfers[0].To ?? to;
        var adjacentIdentity = policies.FreezeAdjacentIdentity(card, from, to, reason);
        var batch = batches.Begin([from], [to]);
        if (!emitCardMovedEvents)
            suppression.Suppress(batch.Id);
        var movements = new List<CardMovementRecord>(1);
        var committed = false;
        try
        {
            zones.Move(card.Id, from, to);
            movements.Add(effects.Record(card, from, to, reason, shiyou is null ? beforeFact : move => policies.RecordInterceptedMovement(shiyou, move, beforeFact), adjacentIdentity, disclosure, happyGainDestination, phantomDestination));
            effects.ResolveEquipmentSkillGrant(card, from, to);
            effects.ClearJudgmentEffectiveKind(card, from, to);
            effects.ResolveArmorRemoval(card, from, reason);
            effects.ResolveStorageEquipmentMove(card, from, to);
            effects.CollectDiscardPhaseHandDiscard(card, from, to);
            committed = true;
        }
        finally
        {
            if (!emitCardMovedEvents && (!suppression.TryPopSuppression(out var suppressed) || suppressed != batch.Id))
                throw new InvalidOperationException("The suppressed card-movement batch nesting changed unexpectedly.");
            batches.Complete(batch, movements, committed);
        }

        policies.DestroyReturnSoulIfDiscarded(card, to);
    }

    internal void MoveMany(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, bool emitCardMovedEvents = true, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
    {
        var phantomDestinations = cards.Select(card => (card.Id, Plan: policies.PlanPhantomDestination(card, from, to, reason))).Where(item => item.Plan is not null).ToDictionary(item => item.Id, item => item.Plan!);
        if (cards.Any(card => policies.NormalizeEphemeralDestination(card, from, to, reason) != to || policies.NormalizeCopiedDestination(card, from, to, reason) != to) || from.Zone == CardZoneKind.Equipment && cards.Any(card => card.IsGeneralWeapon || policies.IsGeneratedDeparture(card, from, to)))
        {
            foreach (var card in cards)
                Move(card, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
            return;
        }

        Dictionary<int, HappyWeiYanGainDestinationPlan>? happyGainDestinations = null;
        var retainedPlans = from.Zone == CardZoneKind.Hand && to.Zone == CardZoneKind.Hand && from != to ? cards.Select(card => policies.PlanGainDestination(card, from, policies.NormalizeDiscardDestination(card, from, to, reason), reason)).OfType<HappyWeiYanGainDestinationPlan>().Where(plan => plan.ActualTo == from).ToArray() : [];
        if (retainedPlans.Length > 0)
        {
            if (beforeFact is not null)
                throw new InvalidOperationException("A retained batch gain requires its producer's typed request settlement.");
            foreach (var plan in retainedPlans)
                policies.SettleRetainedGain(plan);
            var retainedIds = retainedPlans.Select(plan => plan.CardId).ToHashSet();
            var actualCards = cards.Where(card => !retainedIds.Contains(card.Id)).ToArray();
            if (actualCards.Length > 0)
                MoveMany(actualCards, from, to, reason, emitCardMovedEvents: emitCardMovedEvents, disclosure: disclosure);
            return;
        }

        if (cards.Count > 0)
        {
            CardLocation? commonDestination = null;
            foreach (var card in cards)
            {
                var requested = policies.NormalizeDiscardDestination(card, from, to, reason);
                var plan = policies.PlanGainDestination(card, from, requested, reason);
                var actual = plan?.ActualTo ?? requested;
                if (policies.NormalizePhantomDestination(card, from, actual) != actual)
                    throw new InvalidOperationException("A second batch destination replacement requires the phantom's original request invoice.");
                if (commonDestination is { } previous && previous != actual)
                {
                    foreach (var item in cards)
                        Move(item, from, to, reason, beforeFact, emitCardMovedEvents, disclosure);
                    return;
                }

                commonDestination = actual;
                if (plan is not null)
                    (happyGainDestinations ??= []).Add(card.Id, plan);
            }

            // A whole discarded hand redirected to the same recipient remains
            // one native movement/gain batch.
            to = commonDestination!.Value;
        }

        var shiyou = policies.CanPlanDiscardTransfers(to) ? policies.PrepareDiscardTransfers(cards.Select(card => new CardTransfer(card.Id, from, to)).ToArray(), reason) : null;
        var batch = batches.Begin([from], shiyou is null ? [to] : shiyou.Transfers.Select(t => t.To));
        if (!emitCardMovedEvents)
            suppression.Suppress(batch.Id);
        var adjacentIdentities = facts.TracksAdjacentDiscardStorage ? cards.ToDictionary(card => card.Id, card => policies.FreezeAdjacentIdentity(card, from, to, reason)) : null;
        var movements = new List<CardMovementRecord>(cards.Count);
        var committed = false;
        try
        {
            var moved = shiyou is null ? zones.MoveMany(cards.Select(card => card.Id), from, to) : zones.MoveBatch(shiyou.Transfers);
            foreach (var card in moved)
            {
                var destination = shiyou?.Transfers.Single(t => t.CardId == card.Id).To ?? to;
                movements.Add(effects.Record(card, from, destination, reason, shiyou is null ? beforeFact : move => policies.RecordInterceptedMovement(shiyou, move, beforeFact), adjacentDiscardIdentity: adjacentIdentities?.GetValueOrDefault(card.Id), disclosure: disclosure, happyWeiYanGainDestination: happyGainDestinations?.GetValueOrDefault(card.Id), happyZuoCiPhantomDestination: phantomDestinations.GetValueOrDefault(card.Id)));
                effects.ResolveEquipmentSkillGrant(card, from, destination);
                effects.ClearJudgmentEffectiveKind(card, from, destination);
                effects.ResolveArmorRemoval(card, from, reason);
                effects.ResolveStorageEquipmentMove(card, from, destination);
                effects.CollectDiscardPhaseHandDiscard(card, from, destination);
            }

            committed = true;
        }
        finally
        {
            if (!emitCardMovedEvents && (!suppression.TryPopSuppression(out var suppressed) || suppressed != batch.Id))
                throw new InvalidOperationException("The suppressed card-movement batch nesting changed unexpectedly.");
            batches.Complete(batch, movements, committed);
        }

        foreach (var card in cards.Where(card => card.Kind == CardKind.ReturnSoul))
            policies.DestroyReturnSoulIfDiscarded(card, zones.GetLocation(card.Id));
    }

    internal void MoveAll(CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified)
    {
        if (policies.CanPlanDiscardTransfers(to) || zones.CardsAt(from).Any(card => card.Kind == CardKind.ReturnSoul))
        {
            MoveMany(zones.CardsAt(from).ToArray(), from, to, reason, disclosure: disclosure);
            return;
        }

        if (zones.CardsAt(from).Any(card => policies.PlanPhantomDestination(card, from, to, reason)is not null || policies.NormalizeEphemeralDestination(card, from, to, reason) != to || policies.NormalizeCopiedDestination(card, from, to, reason) != to || policies.NormalizeDiscardDestination(card, from, to, reason) != to || policies.NormalizeGainDestination(card, from, to, reason) != to) || from.Zone == CardZoneKind.Equipment && zones.CardsAt(from).Any(card => card.IsGeneralWeapon || policies.IsGeneratedDeparture(card, from, to)))
        {
            MoveMany(zones.CardsAt(from).ToArray(), from, to, reason, disclosure: disclosure);
            return;
        }

        var batch = batches.Begin([from], [to]);
        var adjacentIdentities = facts.TracksAdjacentDiscardStorage ? zones.CardsAt(from).ToDictionary(card => card.Id, card => policies.FreezeAdjacentIdentity(card, from, to, reason)) : null;
        var movements = new List<CardMovementRecord>();
        var committed = false;
        try
        {
            var cards = zones.MoveAll(from, to);
            foreach (var card in cards)
            {
                movements.Add(effects.Record(card, from, to, reason, adjacentDiscardIdentity: adjacentIdentities?.GetValueOrDefault(card.Id), disclosure: disclosure));
                effects.ResolveEquipmentSkillGrant(card, from, to);
                effects.ClearJudgmentEffectiveKind(card, from, to);
                effects.ResolveArmorRemoval(card, from, reason);
                effects.ResolveStorageEquipmentMove(card, from, to);
                effects.CollectDiscardPhaseHandDiscard(card, from, to);
            }

            committed = true;
        }
        finally
        {
            batches.Complete(batch, movements, committed);
        }
    }
}
