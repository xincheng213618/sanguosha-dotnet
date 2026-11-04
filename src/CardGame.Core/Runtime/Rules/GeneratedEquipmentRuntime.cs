using CardGame.Core.EngineOwners;
using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

/// <summary>Physical equipment provenance and lifetime. Skills only declare its source policy.</summary>
[ProgramRuntimeModule]
internal sealed class GeneratedEquipmentRuntime(ICardZoneQueries cards, ICardMovementHistoryQueries history,
    IProgramTurnQueries turn, DeferredRuntime<CoreGameEngineEquipmentContentRuntime> rules,
    CorePhysicalMovementFacadesEntries movement, IProgramRuntimeFrames frames,
    INativeCardRepresentations representations)
{
    internal const string DestroyReason = "equipment.lifetime.destroy";
    internal const string DestroyedThisTurn = "equipment.destroyedThisTurn";
    internal static bool IsNumberFact(string? name) => name == DestroyedThisTurn;
    internal Card Card(int id) => cards.CardsAt(cards.GetLocation(id)).Single(c => c.Id == id);

    internal CardLocation Normalize(Card card, CardLocation from, CardLocation to) => card.EquipmentOrigin is not { } origin ? to :
        rules.Value.EquipmentRules.NormalizeDestination(new(card, from, to, origin,
            history.Read().Any(m => m.CardId == card.Id),
            history.Read().Any(m => m.CardId == card.Id && m.Reason.Value == DestroyReason)));

    internal void DestroyOnEntry(CardMovementRecord fact)
    {
        if (fact.To != CardLocation.DiscardPile) return;
        var card = Card(fact.CardId);
        if (card.EquipmentOrigin is not { } origin ||
            !rules.Value.EquipmentRules.DestinationPolicies.TryGetValue(origin.SourceId, out var policy) ||
            !policy.DestroysOnArrival(fact.To) || cards.GetLocation(card.Id) != fact.To) return;
        movement.MoveCard(card, fact.To, CardLocation.OutsideGame, new(DestroyReason), beforeFact: destroyed =>
        {
            if (destroyed.From != CardLocation.DiscardPile || destroyed.To != CardLocation.OutsideGame ||
                destroyed.Reason.Value != DestroyReason) return;
            // This exact discard entry caused the destruction. A Processing
            // entry keeps its original physical payer rather than borrowing a
            // different active use that happens to mention the same entity.
            var payment = fact.From == CardLocation.Processing
                ? history.Last(previous => previous.CardId == fact.CardId && previous.Sequence < fact.Sequence)
                : fact;
            if (payment is null || payment.CardKind != fact.CardKind ||
                fact.From == CardLocation.Processing && payment.To != CardLocation.Processing) return;
            var use = frames.OfType<CardUseFrame>().LastOrDefault(use =>
                payment.Sequence > use.NativeRepresentationIssuedAfterMovementSequence &&
                use.Action?.PhysicalCards.Any(material => material.CardId == fact.CardId &&
                    material.CardKind == fact.CardKind && material.From == payment.From) == true);
            if (use is not null)
                representations.CaptureDeparture(use.Id, card.Id, destroyed.Sequence);
        });
    }

    internal void CaptureNativeMaterialDeparture(CardMovementRecord fact)
    {
        if (fact.To != CardLocation.OutsideGame || fact.From == fact.To ||
            Card(fact.CardId).EquipmentOrigin is not { } origin ||
            !rules.Value.EquipmentRules.DestinationPolicies.TryGetValue(origin.SourceId, out var policy) ||
            !policy.DestroyOnEquipmentExit || fact.From.Zone != CardZoneKind.Equipment) return;
        var use = frames.OfType<CardUseFrame>().LastOrDefault(use =>
            fact.Sequence > use.NativeRepresentationIssuedAfterMovementSequence &&
            use.Action?.PhysicalCards.Any(material => material.CardId == fact.CardId &&
                material.CardKind == fact.CardKind && material.From == fact.From) == true);
        if (use is not null)
            representations.CaptureDeparture(use.Id, fact.CardId, fact.Sequence);
    }

    internal bool IsDestroyedMaterial(CardMovementRecord m) => m.To == CardLocation.OutsideGame &&
        m.Reason.Value == DestroyReason && Card(m.CardId).EquipmentOrigin is not null;

    internal bool WasDestroyed(int cardId, CardLocation from, CardLocation to)
    {
        if (to != CardLocation.OutsideGame || Card(cardId).EquipmentOrigin is null) return false;
        var destroyed = history.Last(m => m.CardId == cardId && IsDestroyedMaterial(m));
        return destroyed is not null && (destroyed.From == from || history.Last(m => m.CardId == cardId &&
            m.Sequence < destroyed.Sequence && m.To == CardLocation.DiscardPile)?.From == from);
    }

    // Both records are existing public physical facts. Batch identity makes a simultaneous
    // multi-card discard one occurrence, while independent replacement discards stack.
    internal int DestroyedBatches(ProgramSkillFrame frame)
    {
        var movements = history.Read();
        var lost = movements.Where(m => m.TurnNumber == turn.TurnNumber && IsDestroyedMaterial(m) &&
            Card(m.CardId).EquipmentOrigin is { } origin && origin.InitialOwnerSeat == frame.OwnerSeat &&
            origin.IssuingSkillId == frame.SkillId).ToArray();
        return lost.Select(m => movements.Last(p => p.Sequence < m.Sequence && p.CardId == m.CardId &&
            p.To == CardLocation.DiscardPile)).Select(m => m.BatchId == 0 ? m.Sequence : m.BatchId).Distinct().Count();
    }
}
