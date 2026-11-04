using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeMaterialClaimResidency { HandDrawDiscard, Any }

/// <summary>An actual delivery of an original native owner's physical material.</summary>
public sealed record NativeMaterialClaim(int CardId, int? RecipientSeat, long MovementSequence,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    NativeMaterialClaimResidency Residency = NativeMaterialClaimResidency.HandDrawDiscard);

public sealed partial record CardUseFrame
{
    private readonly IReadOnlyList<NativeMaterialClaim> _materialClaims =
        Array.AsReadOnly(Array.Empty<NativeMaterialClaim>());
    public IReadOnlyList<NativeMaterialClaim> MaterialClaims
    { get => _materialClaims; init => _materialClaims = Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record JudgmentFrame
{
    private readonly IReadOnlyList<NativeMaterialClaim> _materialClaims =
        Array.AsReadOnly(Array.Empty<NativeMaterialClaim>());
    public IReadOnlyList<NativeMaterialClaim> MaterialClaims
    { get => _materialClaims; init => _materialClaims = Array.AsReadOnly(value.ToArray()); }
}

internal interface INativeMaterialClaims : IProgramRuntimeOperations
{
    void CaptureDelivery(long ownerId, IReadOnlyList<CardMovementRecord> deliveries,
        NativeMaterialClaimResidency residency = NativeMaterialClaimResidency.Any);
    bool CanReadMaterial(long ownerId, int cardId);
    bool HasRelinquishedMaterial(long ownerId, int cardId);
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
    ICardMovementHistoryQueries movements, ICardZoneQueries zones) : ICardUseMaterialClaims, INativeMaterialClaims
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
        var deliveries = new List<CardMovementRecord>();
        foreach (var card in cards)
        {
            if (delivered[card.Id].ToArray() is not [var move])
                throw new InvalidOperationException("A card-use material claim requires one actual Processing-to-recipient-Hand movement.");
            deliveries.Add(move);
        }
        CaptureDelivery(useId, deliveries, NativeMaterialClaimResidency.HandDrawDiscard);
    }

    public void CaptureDelivery(long ownerId, IReadOnlyList<CardMovementRecord> deliveries,
        NativeMaterialClaimResidency residency = NativeMaterialClaimResidency.Any)
    {
        if (deliveries.Count == 0) return;
        if (!Enum.IsDefined(residency))
            throw new InvalidOperationException("A native material delivery requires a declared residency policy.");
        var owner = frames.Find(ownerId);
        IReadOnlyList<int> materials = owner switch
        {
            CardUseFrame use => use.PhysicalCardIds ?? (use.CardId > 0 ? [use.CardId] : []),
            JudgmentFrame { DelayedCardId: > 0 } judgment => [judgment.DelayedCardId.Value],
            _ => throw new InvalidOperationException("A native material delivery requires its original use or delayed judgment owner.")
        };
        if (deliveries.Select(move => move.CardId).Distinct().Count() != deliveries.Count ||
            deliveries.Select(move => move.Sequence).Distinct().Count() != deliveries.Count ||
            deliveries.Any(move => move.CardId <= 0 || !materials.Contains(move.CardId) ||
                move.Sequence <= 0 || move.From == move.To ||
                (owner is CardUseFrame original && move.Sequence <= original.NativeRepresentationIssuedAfterMovementSequence) ||
                movements.Read().SingleOrDefault(actual => actual.Sequence == move.Sequence) != move))
            throw new InvalidOperationException("A native material delivery requires distinct original materials and their actual physical movements.");
        var claims = ReadClaims(owner).ToList();
        foreach (var move in deliveries)
        {
            var claim = new NativeMaterialClaim(move.CardId,
                move.To.Zone == CardZoneKind.Hand ? move.To.OwnerSeat : null, move.Sequence, residency);
            var previous = claims.SingleOrDefault(item => item.CardId == move.CardId);
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
        if (claims.Count != ReadClaims(owner).Count)
            frames.Replace(ownerId, owner switch
            {
                CardUseFrame use => use with { MaterialClaims = claims },
                JudgmentFrame judgment => judgment with { MaterialClaims = claims },
                _ => throw new InvalidOperationException("The native material owner cannot retain its delivery.")
            });
    }

    public bool WasClaimed(long useId, int cardId) =>
        frames.Find(useId) is CardUseFrame && HasRelinquishedMaterial(useId, cardId);

    public bool CanFinishClaimedMaterial(long useId, int cardId) => WasClaimed(useId, cardId) &&
        zones.GetLocation(cardId).Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile;

    public IReadOnlyList<int> ClaimedCardIds(long useId) => frames.Find(useId) is CardUseFrame use
        ? Array.AsReadOnly(use.MaterialClaims.Select(claim => claim.CardId).ToArray()) : Array.Empty<int>();

    public bool HasRelinquishedMaterial(long ownerId, int cardId) =>
        ReadClaims(frames.Find(ownerId)).Any(claim => claim.CardId == cardId);

    public bool CanReadMaterial(long ownerId, int cardId) =>
        ReadClaims(frames.Find(ownerId)).Any(claim => claim.CardId == cardId &&
            (claim.Residency == NativeMaterialClaimResidency.Any ||
             claim.Residency == NativeMaterialClaimResidency.HandDrawDiscard &&
             zones.GetLocation(cardId).Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile));

    private static IReadOnlyList<NativeMaterialClaim> ReadClaims(ResolutionFrame? owner) => owner switch
    {
        CardUseFrame use => use.MaterialClaims,
        JudgmentFrame judgment => judgment.MaterialClaims,
        _ => Array.Empty<NativeMaterialClaim>()
    };
}
