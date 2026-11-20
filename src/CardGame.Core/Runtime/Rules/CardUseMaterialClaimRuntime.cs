using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum NativeMaterialClaimResidency { HandDrawDiscard, Any }
public enum NativeMaterialClaimKind { Delivery, AcceptedClaim, NativeUseTransfer, NativeChildReservation, Retained, Retirement }
public enum NativeMaterialHandoffMode { Issued, UntilDelivery }

/// <summary>An original native owner's material disposition; MovementSequence identifies an actual physical movement.</summary>
public sealed record NativeMaterialClaim(int CardId, int? RecipientSeat, long MovementSequence,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    NativeMaterialClaimResidency Residency = NativeMaterialClaimResidency.HandDrawDiscard,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    NativeMaterialClaimKind Kind = NativeMaterialClaimKind.Delivery,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] long? ProducerFrameId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] int? InstructionIndex = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] long? ChildUseId = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] long? OriginalEntrySequence = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] CardLocation? Destination = null);

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

    [JsonInclude, JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    internal CardMovementRecord? NativeDelayedMaterialDeparture { get; init; }
}

internal interface INativeMaterialClaims : IProgramRuntimeOperations
{
    void CaptureDelivery(long ownerId, IReadOnlyList<CardMovementRecord> deliveries,
        NativeMaterialClaimResidency residency = NativeMaterialClaimResidency.Any);
    void CaptureRetirement(long ownerId, IReadOnlyList<CardMovementRecord> departures);
    void CaptureRetirement(CardMovementRecord departure);
    void CaptureDelayedMaterialDeparture(CardMovementRecord departure);
    bool AcceptClaim(long ownerId, long producerFrameId, int instructionIndex, IReadOnlyList<int> cardIds);
    void TransferToNativeUse(long ownerId, long childUseId, IReadOnlyList<int> cardIds, NativeMaterialHandoffMode mode);
    void DeclareRetainedMaterial(long ownerId, int cardId);
    void CompleteNativeChild(CardUseFrame childUse);
    bool CanReadMaterial(long ownerId, int cardId);
    bool HasRelinquishedMaterial(long ownerId, int cardId);
    bool PreservesMaterialAtCompletion(long ownerId, int cardId);
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
        var owner = NativeOwner(ownerId);
        var prepared = PrepareDeliveryClaims(owner, deliveries, residency);
        if (!ReferenceEquals(prepared, ReadClaims(owner))) ReplaceClaims(owner, prepared);
    }

    public void CaptureRetirement(long ownerId, IReadOnlyList<CardMovementRecord> departures)
    {
        if (departures.Count == 0) return;
        var owner = NativeOwner(ownerId);
        if (owner is not CardUseFrame original)
            throw new InvalidOperationException("A native-use retirement requires its actual CardUse owner; delayed judgments retain their own destruction protocol.");
        ValidateMaterials(owner, departures.Select(move => move.CardId).ToArray());
        var journal = movements.Read();
        var issuedAfter = original.NativeRepresentationIssuedAfterMovementSequence;
        var claims = ReadClaims(owner).ToList();
        var changed = false;
        foreach (var departure in departures)
        {
            if (departure.Sequence <= issuedAfter || departure.From == departure.To ||
                departure.To == CardLocation.Processing ||
                journal.SingleOrDefault(actual => actual.Sequence == departure.Sequence) != departure)
                throw new InvalidOperationException("A native material retirement requires its original material and actual non-Processing departure.");
            var entry = RetirementEntry(original, departure, journal) ??
                throw new InvalidOperationException("A native material retirement must retain its original payment source or Processing entry.");
            var retirement = new NativeMaterialClaim(departure.CardId, null, departure.Sequence,
                Kind: NativeMaterialClaimKind.Retirement, OriginalEntrySequence: entry,
                Destination: departure.To);
            var previous = claims.SingleOrDefault(claim => claim.CardId == departure.CardId && claim.Kind == NativeMaterialClaimKind.Retirement);
            if (previous is not null)
            {
                if (previous != retirement)
                    throw new InvalidOperationException("A native material cannot replace its original retirement receipt.");
                continue;
            }
            if (claims.Any(claim => claim.MovementSequence == departure.Sequence))
                throw new InvalidOperationException("A native movement cannot be both a delivery and a retirement.");
            claims.Add(retirement);
            changed = true;
        }
        // All source/entry relations are checked before publishing any receipt.
        if (changed) ReplaceClaims(owner, claims);
    }

    public void CaptureRetirement(CardMovementRecord departure)
    {
        var journal = movements.Read();
        if (departure.Sequence <= 0 || departure.From == departure.To || departure.To == CardLocation.Processing ||
            journal.SingleOrDefault(actual => actual.Sequence == departure.Sequence) != departure)
            throw new InvalidOperationException("A native retirement source requires its actual non-Processing movement.");
        // A physical lifetime can also run during an ordinary discard or pure
        // response. Only a real still-owning native use receives this receipt.
        foreach (var use in frames.OfType<CardUseFrame>().Reverse())
        {
            if (!Materials(use).Contains(departure.CardId) || RetirementEntry(use, departure, journal) is null) continue;
            // Repeated lifetime delivery must stop at the same real owner,
            // rather than falling through its released right to an ancestor.
            if (!HasRelinquishedMaterial(use.Id, departure.CardId))
                CaptureRetirement(use.Id, [departure]);
            return;
        }
    }

    public void CaptureDelayedMaterialDeparture(CardMovementRecord departure)
    {
        if (departure.From.Zone != CardZoneKind.Judgment || departure.From == departure.To)
            return;

        // The first departure ends this judgment's physical residency, even if
        // another effect later moves the same card back into the same zone.
        var judgment = frames.OfType<JudgmentFrame>().LastOrDefault(frame =>
            frame.TargetSeat == departure.From.OwnerSeat && frame.DelayedCardId == departure.CardId &&
            frame.CardAttack is { IsDelayedJudgmentDamage: true, EffectiveCardKind: CardKind.Lightning } attack &&
            attack.PhysicalCardIds.Contains(departure.CardId));
        if (judgment is { NativeDelayedMaterialDeparture: null })
            frames.Replace(judgment.Id, judgment with { NativeDelayedMaterialDeparture = departure });
    }

    private static long? RetirementEntry(CardUseFrame owner, CardMovementRecord departure,
        IReadOnlyList<CardMovementRecord> journal)
    {
        var issuedAfter = owner.NativeRepresentationIssuedAfterMovementSequence;
        if (departure.Sequence <= issuedAfter || owner.Action is { Type: not CardActionType.Use }) return null;
        var first = journal.FirstOrDefault(move => move.CardId == departure.CardId && move.Sequence > issuedAfter);
        if (first is null || first.Sequence > departure.Sequence ||
            owner.Action is { } action &&
            action.PhysicalCards.SingleOrDefault(material => material.CardId == departure.CardId) is { } cost &&
            first.From != cost.From)
            return null;
        // A direct payment leaves native custody immediately. Otherwise only the
        // first departure of this use's Processing entry can retire its material.
        // A synchronous discard-to-Outside destruction is one causal departure;
        // a later acquisition/reentry must not revive the original ownership.
        var exit = first.To == CardLocation.Processing
            ? journal.FirstOrDefault(move => move.CardId == departure.CardId && move.Sequence > first.Sequence)
            : first;
        var entry = first;
        if (first.From == CardLocation.Processing)
        {
            // A real child may inherit material already in Processing at its
            // issuance. Retain that actual entry, not the later exit sequence.
            entry = journal.LastOrDefault(move => move.CardId == departure.CardId && move.Sequence < first.Sequence);
            if (entry is null || entry.To != CardLocation.Processing || entry.From == entry.To || entry.Sequence > issuedAfter)
                return null;
        }
        if (exit == departure) return entry.Sequence;
        if (exit is { To.Zone: CardZoneKind.DiscardPile } &&
            journal.FirstOrDefault(move => move.CardId == departure.CardId && move.Sequence > exit.Sequence) == departure &&
            departure.From == CardLocation.DiscardPile && departure.To == CardLocation.OutsideGame)
            return entry.Sequence;
        return null;
    }

    private IReadOnlyList<NativeMaterialClaim> PrepareDeliveryClaims(ResolutionFrame owner,
        IReadOnlyList<CardMovementRecord> deliveries, NativeMaterialClaimResidency residency)
    {
        if (deliveries.Count == 0) return ReadClaims(owner);
        if (!Enum.IsDefined(residency))
            throw new InvalidOperationException("A native material delivery requires a declared residency policy.");
        var materials = Materials(owner);
        if (deliveries.Select(move => move.CardId).Distinct().Count() != deliveries.Count ||
            deliveries.Select(move => move.Sequence).Distinct().Count() != deliveries.Count ||
            deliveries.Any(move => move.CardId <= 0 || !materials.Contains(move.CardId) ||
                move.Sequence <= 0 || move.From == move.To ||
                (owner is CardUseFrame original && move.Sequence <= original.NativeRepresentationIssuedAfterMovementSequence) ||
                movements.Read().SingleOrDefault(actual => actual.Sequence == move.Sequence) != move))
            throw new InvalidOperationException("A native material delivery requires distinct original materials and their actual physical movements.");
        var claims = ReadClaims(owner).ToList();
        var changed = false;
        foreach (var move in deliveries)
        {
            var claim = new NativeMaterialClaim(move.CardId,
                move.To.Zone == CardZoneKind.Hand ? move.To.OwnerSeat : null, move.Sequence, residency);
            var previous = claims.SingleOrDefault(item => item.CardId == move.CardId && item.Kind is not
                (NativeMaterialClaimKind.Retained or NativeMaterialClaimKind.Retirement));
            if (previous is not null)
            {
                claim = claim with { ProducerFrameId = previous.ProducerFrameId, InstructionIndex = previous.InstructionIndex,
                    ChildUseId = previous.ChildUseId, OriginalEntrySequence = previous.OriginalEntrySequence };
                if (previous.MovementSequence > 0)
                {
                    if (previous != claim)
                        throw new InvalidOperationException("A native-use material cannot replace its delivered claim.");
                    continue;
                }
                if (previous.Residency != residency || previous.Kind is not
                    (NativeMaterialClaimKind.AcceptedClaim or NativeMaterialClaimKind.NativeUseTransfer or NativeMaterialClaimKind.NativeChildReservation))
                    throw new InvalidOperationException("A native-use material cannot replace its delivered claim.");
                claims[claims.IndexOf(previous)] = claim;
                changed = true;
                continue;
            }
            if (claims.Any(item => item.MovementSequence == move.Sequence))
                throw new InvalidOperationException("A material-claim movement cannot identify another native-use material.");
            claims.Add(claim);
            changed = true;
        }
        return changed ? claims : ReadClaims(owner);
    }

    public bool AcceptClaim(long ownerId, long producerFrameId, int instructionIndex, IReadOnlyList<int> cardIds)
    {
        if (cardIds.Count == 0) return false;
        var owner = NativeOwner(ownerId);
        ValidateMaterials(owner, cardIds);
        if (instructionIndex < 0 || frames.Find(producerFrameId) is null || !IsAncestor(ownerId, producerFrameId))
            throw new InvalidOperationException("An accepted native-material claim requires its original producer instruction.");
        var claims = ReadClaims(owner);
        var issued = claims.Where(claim => claim.ProducerFrameId == producerFrameId && claim.InstructionIndex == instructionIndex).ToArray();
        if (issued.Length != 0)
        {
            if (!issued.Select(claim => claim.CardId).Order().SequenceEqual(cardIds.Order()) ||
                issued.Any(claim => claim.ChildUseId is not null || claim.Residency != NativeMaterialClaimResidency.Any))
                throw new InvalidOperationException("An accepted material claim cannot change its original complete material set.");
            return false;
        }
        if (claims.Any(claim => claim.Kind != NativeMaterialClaimKind.Retained && cardIds.Contains(claim.CardId)))
            throw new InvalidOperationException("An original native material already has a different accepted disposition.");
        ReplaceClaims(owner, claims.Concat(cardIds.Select(id => new NativeMaterialClaim(id, null, 0,
            NativeMaterialClaimResidency.Any, NativeMaterialClaimKind.AcceptedClaim, producerFrameId, instructionIndex))).ToArray());
        return true;
    }

    public void TransferToNativeUse(long ownerId, long childUseId, IReadOnlyList<int> cardIds, NativeMaterialHandoffMode mode)
    {
        if (cardIds.Count == 0) return;
        var owner = NativeOwner(ownerId);
        ValidateMaterials(owner, cardIds);
        if (!Enum.IsDefined(mode) || ownerId == childUseId || !IsAncestor(ownerId, childUseId) ||
            frames.Find(childUseId) is not CardUseFrame child || child.Step == ResolutionFrameStep.Completed ||
            cardIds.Any(id => !ChildOwnsMaterial(child, id)))
            throw new InvalidOperationException("A material handoff requires the actual descendant native use and its committed Processing materials.");
        var kind = mode == NativeMaterialHandoffMode.Issued ? NativeMaterialClaimKind.NativeUseTransfer : NativeMaterialClaimKind.NativeChildReservation;
        var claims = ReadClaims(owner);
        var existing = claims.Where(claim => claim.ChildUseId == childUseId).ToArray();
        if (existing.Length != 0)
        {
            if (!existing.Select(claim => claim.CardId).Order().SequenceEqual(cardIds.Order()) || existing.Any(claim => claim.Kind != kind))
                throw new InvalidOperationException("A native handoff cannot change its original child or complete material set.");
            return;
        }
        var journal = movements.Read();
        var transfers = cardIds.Select(id =>
        {
            var entry = journal.LastOrDefault(move => move.CardId == id);
            if (zones.GetLocation(id) != CardLocation.Processing || entry is not { To: var destination } ||
                destination != CardLocation.Processing || entry.From == entry.To ||
                entry.Sequence > child.NativeRepresentationIssuedAfterMovementSequence ||
                claims.Any(claim => claim.CardId == id && claim.Kind != NativeMaterialClaimKind.Retained))
                throw new InvalidOperationException("A native handoff requires the unchanged actual Processing entry of an unclaimed original material.");
            return new NativeMaterialClaim(id, null, 0, NativeMaterialClaimResidency.Any, kind,
                ChildUseId: childUseId, OriginalEntrySequence: entry.Sequence);
        }).ToArray();
        ReplaceClaims(owner, claims.Concat(transfers).ToArray());
    }

    public void DeclareRetainedMaterial(long ownerId, int cardId)
    {
        var owner = NativeOwner(ownerId);
        ValidateMaterials(owner, [cardId]);
        var claims = ReadClaims(owner);
        if (claims.Any(claim => claim.CardId == cardId && claim.Kind == NativeMaterialClaimKind.Retained)) return;
        ReplaceClaims(owner, claims.Append(new(cardId, null, 0, NativeMaterialClaimResidency.Any,
            NativeMaterialClaimKind.Retained)).ToArray());
    }

    public void CompleteNativeChild(CardUseFrame childUse)
    {
        var owners = frames.OfType<ResolutionFrame>().Select(owner => (Owner: owner, Pending: ReadClaims(owner)
            .Where(claim => claim.Kind == NativeMaterialClaimKind.NativeChildReservation && claim.ChildUseId == childUse.Id)
            .ToArray())).Where(item => item.Pending.Length != 0).ToArray();
        if (owners.Length == 0) return;
        if (frames.Find(childUse.Id) is not CardUseFrame { Step: ResolutionFrameStep.Completed } live ||
            childUse.Step != ResolutionFrameStep.Completed)
            throw new InvalidOperationException("Only the original completed native child can settle its material handoff.");
        var journal = movements.Read();
        var settlements = new List<(ResolutionFrame Owner, IReadOnlyList<NativeMaterialClaim> Claims)>();
        foreach (var (owner, pending) in owners)
        {
            var delivered = new List<CardMovementRecord>();
            foreach (var claim in pending)
            {
                if (!ChildOwnsMaterial(live, claim.CardId) || claim.OriginalEntrySequence is not { } entry)
                    throw new InvalidOperationException("A completed native child lost its original committed material.");
                var first = journal.FirstOrDefault(move => move.CardId == claim.CardId && move.Sequence > entry);
                if (first is null) continue;
                if (first.From != CardLocation.Processing || first.To == CardLocation.Processing)
                    throw new InvalidOperationException("A native child's first material departure lost its real Processing delivery.");
                delivered.Add(first);
            }
            var prepared = PrepareDeliveryClaims(owner, delivered, NativeMaterialClaimResidency.Any);
            settlements.Add((owner, prepared.Where(claim => claim.Kind != NativeMaterialClaimKind.NativeChildReservation ||
                claim.ChildUseId != childUse.Id).ToArray()));
        }
        // Validate every original owner before publishing any of the settlements.
        foreach (var settlement in settlements)
            ReplaceClaims(settlement.Owner, settlement.Claims);
    }

    public bool WasClaimed(long useId, int cardId) =>
        frames.Find(useId) is CardUseFrame use && use.MaterialClaims.Any(claim => claim.CardId == cardId &&
            claim.Kind == NativeMaterialClaimKind.Delivery && claim.MovementSequence > 0);

    public bool CanFinishClaimedMaterial(long useId, int cardId) => WasClaimed(useId, cardId) &&
        zones.GetLocation(cardId).Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile;

    public IReadOnlyList<int> ClaimedCardIds(long useId) => frames.Find(useId) is CardUseFrame use
        ? Array.AsReadOnly(use.MaterialClaims.Where(claim => claim.Kind == NativeMaterialClaimKind.Delivery &&
            claim.MovementSequence > 0).Select(claim => claim.CardId).Distinct().ToArray()) : Array.Empty<int>();

    public bool HasRelinquishedMaterial(long ownerId, int cardId) =>
        ReadClaims(frames.Find(ownerId)).Any(claim => claim.CardId == cardId &&
            (claim.MovementSequence > 0 || claim.Kind is NativeMaterialClaimKind.AcceptedClaim or NativeMaterialClaimKind.NativeUseTransfer ||
             claim.Kind == NativeMaterialClaimKind.NativeChildReservation && LiveChildOwnsMaterial(claim)));

    public bool PreservesMaterialAtCompletion(long ownerId, int cardId) =>
        ReadClaims(frames.Find(ownerId)).Any(claim => claim.CardId == cardId && claim.Kind == NativeMaterialClaimKind.Retirement) ||
        CanReadMaterial(ownerId, cardId) ||
        HasRelinquishedMaterial(ownerId, cardId) && zones.GetLocation(cardId) == CardLocation.Processing;

    public bool CanReadMaterial(long ownerId, int cardId) =>
        ReadClaims(frames.Find(ownerId)).Any(claim => claim.CardId == cardId &&
            (claim.Kind == NativeMaterialClaimKind.Retirement ? claim.Destination == zones.GetLocation(cardId) :
             claim.Kind == NativeMaterialClaimKind.Retained ? zones.GetLocation(cardId) != CardLocation.Processing :
             (claim.MovementSequence > 0 || claim.Kind is NativeMaterialClaimKind.AcceptedClaim or NativeMaterialClaimKind.NativeUseTransfer ||
              claim.Kind == NativeMaterialClaimKind.NativeChildReservation && LiveChildOwnsMaterial(claim)) &&
             (claim.Residency == NativeMaterialClaimResidency.Any || claim.Residency == NativeMaterialClaimResidency.HandDrawDiscard &&
              zones.GetLocation(cardId).Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.DiscardPile)));

    private bool LiveChildOwnsMaterial(NativeMaterialClaim claim) => claim.ChildUseId is { } id &&
        frames.Find(id) is CardUseFrame child && ChildOwnsMaterial(child, claim.CardId);

    private static bool ChildOwnsMaterial(CardUseFrame child, int cardId) => Materials(child).Contains(cardId) &&
        (child.Action is null || child.Action.PhysicalCards.Any(cost => cost.CardId == cardId && cost.From == CardLocation.Processing));

    private bool IsAncestor(long ownerId, long descendantId)
    {
        for (var frame = frames.ParentOf(descendantId); frame is not null; frame = frames.ParentOf(frame.Id))
            if (frame.Id == ownerId) return true;
        return false;
    }

    private ResolutionFrame NativeOwner(long ownerId) => frames.Find(ownerId) is CardUseFrame or JudgmentFrame
        ? frames.Find(ownerId)! : throw new InvalidOperationException("A native material disposition requires its actual use or judgment owner.");

    private static IReadOnlyList<int> Materials(ResolutionFrame? owner) => owner switch
    {
        CardUseFrame use => use.PhysicalCardIds ?? (use.CardId > 0 ? [use.CardId] : []),
        JudgmentFrame { DelayedCardId: > 0 } judgment => [judgment.DelayedCardId.Value],
        _ => throw new InvalidOperationException("A native material disposition requires its original physical materials.")
    };

    private static void ValidateMaterials(ResolutionFrame owner, IReadOnlyList<int> cardIds)
    {
        var materials = Materials(owner);
        if (cardIds.Distinct().Count() != cardIds.Count || cardIds.Any(id => id <= 0 || !materials.Contains(id)))
            throw new InvalidOperationException("A material disposition requires distinct original native materials.");
    }

    private void ReplaceClaims(ResolutionFrame owner, IReadOnlyList<NativeMaterialClaim> claims) => frames.Replace(owner.Id, owner switch
    {
        CardUseFrame use => use with { MaterialClaims = claims },
        JudgmentFrame judgment => judgment with { MaterialClaims = claims },
        _ => throw new InvalidOperationException("The native material owner cannot retain its disposition.")
    });

    private static IReadOnlyList<NativeMaterialClaim> ReadClaims(ResolutionFrame? owner) => owner switch
    {
        CardUseFrame use => use.MaterialClaims,
        JudgmentFrame judgment => judgment.MaterialClaims,
        _ => Array.Empty<NativeMaterialClaim>()
    };
}
