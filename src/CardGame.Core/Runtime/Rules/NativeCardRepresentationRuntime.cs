namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCardRepresentationRuntime(IProgramRuntimeFrames frames, ICardZoneQueries cards,
    ICardMovementHistoryQueries movements,
    RuntimeEntries.CoreGameEngineCurrentUsePhysicalClaimsEntries claims) : INativeCardRepresentations
{
    public Card Get(long useId, int cardId, bool requireProcessing = false)
    {
        var use = Owner(useId);
        if (cardId == 0)
            return use.NativeRepresentation is { Id: 0 } representation
                ? representation
                : throw new InvalidOperationException("The native use has no zero-entity card representation.");
        if (!OwnsEntity(use, cardId))
            throw new InvalidOperationException("A native representation must belong to its original use.");
        var location = cards.GetLocation(cardId);
        if (requireProcessing && location != CardLocation.Processing &&
            !(use.NativeRepresentationIsCarrier && use.NativeRepresentation?.Id == cardId) &&
            !use.NativeRepresentationDepartures.Any(departure => departure.CardId == cardId &&
                departure.Destination == location) && !claims.IsClaimedUseCardEntity(useId, cardId))
            throw new InvalidOperationException("A native-use entity left Processing without an issued representation lifetime.");
        return cards.CardsAt(location).Single(card => card.Id == cardId);
    }

    public Card ReadAppearance(long useId, CardAppearanceReference appearance)
    {
        if (appearance.Id == 0)
        {
            var original = Get(useId, 0);
            var use = Owner(useId);
            if ((original.Kind != appearance.Kind || original.Suit != appearance.Suit || original.Rank != appearance.Rank) &&
                use.Continuations.GroupCard?.Card != appearance && use.Continuations.BorrowedSword?.Card != appearance)
                throw new InvalidOperationException("A zero-entity appearance must retain its original native representation.");
            // A real native continuation can issue a different effect face
            // without changing the underlying use or acquiring new material.
            return original with { Kind = appearance.Kind, Suit = appearance.Suit, Rank = appearance.Rank };
        }
        // The continuation owns its already-frozen effective appearance. The
        // physical entity remains the printed card in the one zone store.
        return Get(useId, appearance.Id) with
        {
            Kind = appearance.Kind,
            Suit = appearance.Suit,
            Rank = appearance.Rank
        };
    }

    public void CaptureDeparture(long useId, int cardId, long movementSequence)
    {
        var use = Owner(useId);
        if (cardId <= 0 || !OwnsEntity(use, cardId) || movementSequence <= use.NativeRepresentationIssuedAfterMovementSequence || movements.Read().SingleOrDefault(move =>
                move.Sequence == movementSequence && move.CardId == cardId) is not { } movement)
            throw new InvalidOperationException("A native representation departure requires its original entity and actual movement.");
        var departure = new NativeCardRepresentationDeparture(cardId, movement.To, movement.Sequence);
        var previous = use.NativeRepresentationDepartures.SingleOrDefault(item => item.MovementSequence == movementSequence);
        if (previous is not null)
        {
            if (previous != departure)
                throw new InvalidOperationException("A native representation departure cannot change its physical delivery.");
            return;
        }
        frames.Replace(useId, use with
        {
            NativeRepresentationDepartures = use.NativeRepresentationDepartures.Append(departure).ToArray()
        });
    }

    private CardUseFrame Owner(long useId) => frames.Find(useId) is CardUseFrame { NativeRepresentation: not null } use
        ? use
        : throw new InvalidOperationException("The original native use has no issued card representation.");

    private static bool OwnsEntity(CardUseFrame use, int cardId) =>
        use.NativeRepresentation?.Id == cardId || (use.PhysicalCardIds ?? []).Contains(cardId);
}
