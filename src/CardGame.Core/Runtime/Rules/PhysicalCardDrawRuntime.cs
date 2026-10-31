namespace CardGame.Core;

/// <summary>Owns physical draw selection, actual gains and recycling in their original rule order.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class PhysicalCardDrawRuntime(ICardZoneQueries zones, ICardMovementHistoryQueries history,
    ICardDrawPileOrder pileOrder, IMatchRandomSource random, INativeDrawInvocationInvoices invoices,
    IProgramRuntimeEvents events, IPhysicalCardDrawPolicies policies, IPhysicalCardDrawMovement movement,
    IPhysicalCardDrawLogs logs) : IPhysicalCardDrawOperations
{
    public IReadOnlyList<int> DrawCards(
        CharacterState player,
        int count,
        bool log,
        CardMoveReason? reason = null,
        CardConversionSource? directProducer = null)
    {
        var nativeDraw = invoices.Capture(player.Seat, count, reason ?? CardMoveReasons.Draw, directProducer);
        var drawnCardIds = new List<int>();
        for (var i = 0; i < count; i++)
        {
            var movementCursor = history.Read().Count;
            var card = DrawOne(player, reason ?? CardMoveReasons.Draw);
            if (card is null)
            {
                break;
            }

            var physicalDraw = FirstPhysicalDrawSince(movementCursor, card.Id);
            if (physicalDraw.To == CardLocation.Hand(player.Seat))
                drawnCardIds.Add(card.Id);
        }

        invoices.Complete(nativeDraw, drawnCardIds);
        // Presentation consumes the actual result of DrawCards, regardless of
        // the producer's reason. Initial dealing and replacement gains are not draws.
        if (drawnCardIds.Count > 0 && reason != CardMoveReasons.InitialDeal)
            events.AdvanceFact(new CardsDrawnEvent(player.Seat, drawnCardIds.Count));
        if (log && drawnCardIds.Count > 0)
        {
            logs.Add("CardsDrawn", $"{player.Name} 摸了 {drawnCardIds.Count} 张牌。", player.Seat);
        }

        return drawnCardIds;
    }

    private CardMovementRecord FirstPhysicalDrawSince(int cursor, int cardId)
    {
        var records = history.Read();
        // The read-only journal is a live view, not an IList. Start at the
        // captured index so an old match does not rescan its whole history.
        for (var index = cursor; index < records.Count; index++)
            if (records[index] is { } record && record.CardId == cardId && record.From == CardLocation.DrawPile)
                return record;
        throw new InvalidOperationException("Sequence contains no matching element");
    }

    public Card? DrawOne(CharacterState player, CardMoveReason reason)
    {
        if (!EnsureDrawPile())
        {
            return null;
        }

        var drawPile = zones.CardsAt(CardLocation.DrawPile);
        var card = reason != CardMoveReasons.InitialDeal && policies.DrawFromBottom(player) ? drawPile[0] : drawPile[^1];
        card = policies.MinimumDrawMaterial(player, reason, drawPile, card);
        if (policies.TryChooseDrawMaterial(player, reason, drawPile, out var chosenMaterial))
        {
            if (chosenMaterial is null) return null;
            card = chosenMaterial;
        }
        movement.Move(card, CardLocation.DrawPile, CardLocation.Hand(player.Seat), reason);
        return card;
    }

    public Card? DrawOneToProcessing(CardMoveReason reason)
    {
        if (!EnsureDrawPile())
        {
            return null;
        }

        var card = zones.CardsAt(CardLocation.DrawPile)[^1];
        movement.Move(card, CardLocation.DrawPile, CardLocation.Processing, reason);
        return card;
    }

    public bool EnsureDrawPile()
    {
        policies.ExcludeDeckEquipment(CardLocation.DrawPile);
        if (zones.Count(CardLocation.DrawPile) > 0)
        {
            return true;
        }

        if (!policies.AllowsDiscardRecycling)
        {
            policies.HandleEmptyDrawPile();
            return false;
        }

        policies.ExcludeDeckEquipment(CardLocation.DiscardPile);
        if (zones.Count(CardLocation.DiscardPile) == 0)
        {
            return false;
        }

        movement.MoveAll(
            CardLocation.DiscardPile,
            CardLocation.DrawPile,
            CardMoveReasons.Reshuffle);
        pileOrder.ShuffleDrawPile(random);
        logs.Add("DeckReshuffled", "摸牌堆耗尽，洗混弃牌堆形成新的摸牌堆。");
        policies.NotifyDeckRecycled();
        return true;
    }
}
