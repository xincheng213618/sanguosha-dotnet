namespace CardGame.Core;

/// <summary>An actual Processing-to-Hand claim on this native use's material.</summary>
public sealed record CardUseMaterialClaim(int CardId, int RecipientSeat, long MovementSequence);

public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<CardUseMaterialClaim> _materialClaims =
        Array.AsReadOnly(Array.Empty<CardUseMaterialClaim>());
    public IReadOnlyList<CardUseMaterialClaim> MaterialClaims
    { get => _materialClaims; init => _materialClaims = Array.AsReadOnly(value.ToArray()); }
}

internal interface ICardUseMaterialClaims : IProgramRuntimeOperations
{
    void Capture(long useId, int recipientSeat, IReadOnlyList<Card> cards, long sequenceBefore, CardMoveReason reason);
    bool WasClaimed(long useId, int cardId);
    bool CanFinishClaimedMaterial(long useId, int cardId);
    IReadOnlyList<int> ClaimedCardIds(long useId);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class CardUseMaterialClaimRuntime(IProgramRuntimeFrames frames,
    ICardMovementHistoryQueries movements, ICardZoneQueries zones) : ICardUseMaterialClaims
{
    public void Capture(long useId, int recipientSeat, IReadOnlyList<Card> cards,
        long sequenceBefore, CardMoveReason reason)
    {
        // Judgment and other damage owners retain their own material protocol.
        if (frames.Find(useId) is not CardUseFrame use || cards.Count == 0)
            return;
        IReadOnlyList<int> materials = use.PhysicalCardIds ?? (use.CardId > 0 ? [use.CardId] : []);
        var ids = cards.Select(card => card.Id).ToHashSet();
        if (ids.Count != cards.Count || ids.Any(id => !materials.Contains(id)))
            throw new InvalidOperationException("A card-use material claim requires distinct original native-use materials.");
        var delivered = movements.Read().Where(move => move.Sequence > sequenceBefore &&
            ids.Contains(move.CardId) && move.From == CardLocation.Processing &&
            move.To == CardLocation.Hand(recipientSeat) && move.Reason == reason)
            .ToLookup(move => move.CardId);
        var claims = use.MaterialClaims.ToList();
        foreach (var card in cards)
        {
            if (delivered[card.Id].ToArray() is not [var move])
                throw new InvalidOperationException("A card-use material claim requires one actual Processing-to-recipient-Hand movement.");
            var claim = new CardUseMaterialClaim(card.Id, recipientSeat, move.Sequence);
            var previous = claims.SingleOrDefault(item => item.CardId == card.Id);
            if (previous is not null)
            {
                if (previous != claim)
                    throw new InvalidOperationException("A native-use material cannot replace its delivered claim.");
                continue;
            }
            if (claims.Any(item => item.MovementSequence == move.Sequence))
                throw new InvalidOperationException("A material-claim movement cannot identify another native-use material.");
            claims.Add(claim);
        }
        // Publish the verified batch once; a conflicting batch changes nothing.
        if (claims.Count != use.MaterialClaims.Count)
            frames.Replace(use.Id, use with { MaterialClaims = claims });
    }

    public bool WasClaimed(long useId, int cardId) =>
        frames.Find(useId) is CardUseFrame use && use.MaterialClaims.Any(claim => claim.CardId == cardId);

    public bool CanFinishClaimedMaterial(long useId, int cardId) => WasClaimed(useId, cardId) &&
        zones.GetLocation(cardId).Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile;

    public IReadOnlyList<int> ClaimedCardIds(long useId) => frames.Find(useId) is CardUseFrame use
        ? Array.AsReadOnly(use.MaterialClaims.Select(claim => claim.CardId).ToArray()) : Array.Empty<int>();
}
