namespace CardGame.Core;

internal readonly record struct HandLimitCurrentTurn(int TurnNumber, int OwnerSeat, HandLimitTurnStatus Status)
{
    internal static HandLimitCurrentTurn Capture(IProgramTurnQueries turn, IHandLimitExemptionCardFacts facts,
        HandLimitExemptionExpiry scope) => scope == HandLimitExemptionExpiry.Permanent ? default :
        new(turn.TurnNumber, turn.TurnOwnerSeat, scope == HandLimitExemptionExpiry.ActualTurnOpen ? facts.TurnStatus : default);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandLimitExemptionStore(IProgramTurnQueries turn, IHandLimitExemptionCardFacts facts,
    IHandLimitExemptionSequences sequences)
    : IHandLimitExemptionStore, IHandLimitExemptionBoundaries, IHandLimitExemptionNativeLifetimes
{
    private readonly List<HandLimitExemptionGrant> _grants = [];
    private IReadOnlyList<HandLimitExemptionGrant>? _view;
    public IReadOnlyList<HandLimitExemptionGrant> Grants => _view ??= _grants.AsReadOnly();
    public HandLimitExemptionGrant Add(HandLimitExemptionGrant grant)
    {
        var issued = grant with { Sequence = sequences.Next() };
        _grants.Add(issued);
        return issued;
    }
    public void ProcessBoundary(SkillStateBoundary boundary)
    {
        var scope = HandLimitExemptionExpiry.Permanent;
        foreach (var grant in _grants)
        {
            if (grant.Expired || grant.Issue.Spec.NativeExpiration is not null) continue;
            if (grant.Issue.Spec.Expiry == HandLimitExemptionExpiry.ActualTurnOpen) { scope = HandLimitExemptionExpiry.ActualTurnOpen; break; }
            if (grant.Issue.Spec.Expiry == HandLimitExemptionExpiry.FrozenTurnCoordinates) scope = HandLimitExemptionExpiry.FrozenTurnCoordinates;
        }
        var current = HandLimitCurrentTurn.Capture(turn, facts, scope);
        for (var index = 0; index < _grants.Count; index++)
        {
            var grant = _grants[index];
            if (!grant.Expired && grant.Issue.Spec.NativeExpiration is null && (!HasTurnScope(grant, current) ||
                grant.Lifetime?.Until.Any(end => ProgramRuleLifetimeBoundaries.Matches(end, boundary)) == true))
                _grants[index] = grant with { Expired = true };
        }
    }

    public IReadOnlyList<long> ExpireTurn(int turnNumber, int turnSeat) => ExpireNative(grant =>
        grant.Issue.Spec.NativeExpiration == SkillProgramDamageModifierExpiration.CurrentTurnEnd &&
        grant.IssuedTurn.TurnNumber == turnNumber && grant.IssuedTurn.OwnerSeat == turnSeat);
    public IReadOnlyList<long> ExpireOwnerTurnStart(int owner, int currentTurn) => ExpireNative(grant =>
        grant.Issue.Spec.NativeExpiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart &&
        grant.Issue.Source.OwnerSeat == owner && grant.IssuedTurn.TurnNumber < currentTurn);
    public IReadOnlyList<long> ExpireOwnerDeath(int owner) => ExpireNative(grant =>
        grant.Issue.Spec.NativeExpiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart && grant.Issue.Source.OwnerSeat == owner);

    private IReadOnlyList<long> ExpireNative(Func<HandLimitExemptionGrant, bool> matches)
    {
        var expired = new List<long>();
        for (var index = 0; index < _grants.Count; index++)
            if (!_grants[index].Expired && matches(_grants[index]))
            {
                expired.Add(_grants[index].Sequence);
                _grants[index] = _grants[index] with { Expired = true };
            }
        expired.Sort();
        return expired.ToArray();
    }

    internal static bool HasTurnScope(HandLimitExemptionGrant grant, HandLimitCurrentTurn current)
    {
        var spec = grant.Issue.Spec;
        if (grant.Expired) return false;
        if (spec.Expiry == HandLimitExemptionExpiry.Permanent) return true;
        var issued = spec.Turn ?? grant.IssuedTurn;
        if (issued.TurnNumber != current.TurnNumber || issued.OwnerSeat != current.OwnerSeat) return false;
        if (spec.Expiry == HandLimitExemptionExpiry.FrozenTurnCoordinates || spec.AllowBeforeFirstTurn && current.TurnNumber == 0) return true;
        var required = spec.TurnRequirements;
        var status = current.Status;
        return (!required.HasFlag(HandLimitTurnRequirements.SetupComplete) || status.SetupComplete) &&
            (!required.HasFlag(HandLimitTurnRequirements.PositiveTurn) || current.TurnNumber > 0) &&
            (!required.HasFlag(HandLimitTurnRequirements.ProgressionTurnMatches) || status.ProgressionTurnMatches) &&
            (!required.HasFlag(HandLimitTurnRequirements.OwnerAtCurrentCursor) || status.OwnerAtCurrentCursor) &&
            (!required.HasFlag(HandLimitTurnRequirements.NoTurnEnded) || !status.TurnEnded) &&
            (!required.HasFlag(HandLimitTurnRequirements.NoTurnReturned) || !status.TurnReturned);
    }
}

// A recipient relation and its count effect share the same membership and card selector.
// Counting adds actual Hand residency and declared recipient survival; it never grants card-use permission.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class HandLimitExemptionRuntime(IHandLimitExemptionStore store, IProgramRuntimeFrames frames,
    IProgramTurnQueries turn, IProgramRuleLifetimes lifetimes, IHandLimitExemptionCardFacts facts,
    IMatchParticipantStateAccess participants, ICardZoneQueries cards, ICardEntityMarkQueries marks,
    ICurrentHandEntryQueries entries) : IHandLimitExemptions
{
    public HandLimitExemptionGrant Grant(ProgramSkillFrame supplied, string relationId, int recipientSeat,
        HandLimitExemptionSpec spec, int issuanceSlot = 0)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        return Grant(new(frame.Id, frame.InstructionIndex - 1, issuanceSlot, relationId,
            new(frame.SkillId, frame.TriggerId ?? frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId),
            frame.GameplayHash, recipientSeat, spec));
    }

    public HandLimitExemptionGrant Grant(HandLimitExemptionIssue issue)
    {
        ArgumentNullException.ThrowIfNull(issue);
        if (issue.IssuerFrameId <= 0 || issue.InstructionIndex < 0 || issue.IssuanceSlot < 0 ||
            string.IsNullOrWhiteSpace(issue.RelationId) || issue.Source is null || !Valid(issue.Source.OwnerSeat) ||
            string.IsNullOrWhiteSpace(issue.GameplayHash) || !Valid(issue.RecipientSeat) || issue.Spec is null)
            throw new InvalidOperationException("A hand-limit relation requires its actual issuer, recipient and immutable declaration.");
        var previous = store.Grants.SingleOrDefault(grant => grant.Issue.IssuerFrameId == issue.IssuerFrameId &&
            grant.Issue.InstructionIndex == issue.InstructionIndex && grant.Issue.IssuanceSlot == issue.IssuanceSlot &&
            grant.Issue.RelationId == issue.RelationId);
        if (previous is not null)
        {
            if (previous.Issue.Source != issue.Source || previous.Issue.GameplayHash != issue.GameplayHash ||
                previous.Issue.RecipientSeat != issue.RecipientSeat || !SameSpec(previous.Issue.Spec, issue.Spec))
                throw new InvalidOperationException("An issued hand-limit relation cannot change on reentry.");
            return previous;
        }
        var spec = issue.Spec;
        var issuedTurn = spec.Turn ?? new HandLimitTurnCoordinates(turn.TurnNumber, turn.TurnOwnerSeat);
        if (issuedTurn.TurnNumber < 0 || !Valid(issuedTurn.OwnerSeat))
            throw new InvalidOperationException("A hand-limit relation requires valid original turn coordinates.");
        var lifetime = spec.Until.Count == 0 ? null : lifetimes.CreateApplicationLifetime(issue.Source.OwnerSeat,
            issue.RecipientSeat, spec.Until, new(issue.Source.OwnerSeat, issue.RecipientSeat, IssuedTurnNumber: issuedTurn.TurnNumber));
        if (spec.Until.Count != 0 && lifetime is null)
            throw new InvalidOperationException("A hand-limit relation lost its declared boundary lifetime.");
        var frozenEntries = new List<HandLimitExemptionHandEntry>();
        if (spec.RequireCurrentHandEntry)
            foreach (var id in spec.ExactCardIds!)
            {
                if (entries.CurrentHandEntry(issue.RecipientSeat, id) is not { } entry)
                    throw new InvalidOperationException("A kept hand entity requires its actual original Hand entry.");
                frozenEntries.Add(new(id, entry));
            }
        return store.Add(new(0, issue, issuedTurn, lifetime) { HandEntries = frozenEntries });
    }

    public HandLimitExemptionGrant? Get(long sequence) => store.Grants.SingleOrDefault(grant => grant.Sequence == sequence && !grant.Expired);
    public IReadOnlyList<HandLimitExemptionGrant> ActiveRelations(string relationId, int recipientSeat)
    {
        var related = store.Grants.Where(grant => !grant.Expired && grant.Issue.RelationId == relationId && grant.Issue.RecipientSeat == recipientSeat).ToArray();
        var current = CaptureFor(related);
        return Array.AsReadOnly(related.Where(grant => HandLimitExemptionStore.HasTurnScope(grant, current)).ToArray());
    }

    public HandLimitExemptionGrant? Apply(ProgramSkillFrame supplied, int recipientSeat, ProgramHandLimitExemption declaration)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        var spec = declaration.Spec;
        if (declaration.CardsBind is { } bind)
        {
            var ids = frame.CardSetBindings.Single(binding => binding.Name == bind).CardIds;
            if (ids.Count == 0) return null;
            spec = new(spec.Match, ids, spec.Expiry, spec.Turn, spec.RequireCurrentHandEntry, spec.Until,
                spec.AllowBeforeFirstTurn, spec.TurnRequirements, spec.NativeExpiration);
        }
        return Grant(frame, declaration.RelationId, recipientSeat, spec, recipientSeat);
    }
    public bool IsMember(long sequence, int recipientSeat, int cardId) =>
        Get(sequence) is { } grant && FindCard(cardId) is { } card &&
        IsMember(grant, recipientSeat, card, HandLimitCurrentTurn.Capture(turn, facts, grant.Issue.Spec.Expiry));
    public IReadOnlyList<HandLimitExemptionGrant> GetMatchingRelations(string relationId, int recipientSeat, int cardId)
    {
        if (FindCard(cardId) is not { } card) return [];
        var related = store.Grants.Where(grant => !grant.Expired && grant.Issue.RelationId == relationId && grant.Issue.RecipientSeat == recipientSeat).ToArray();
        var current = CaptureFor(related);
        return Array.AsReadOnly(related.Where(grant => IsMember(grant, recipientSeat, card, current)).ToArray());
    }
    public bool HasEntityRelation(string relationId, int cardId)
    {
        if (FindCard(cardId) is not { } card) return false;
        var related = store.Grants.Where(grant => !grant.Expired && grant.Issue.RelationId == relationId).ToArray();
        var current = CaptureFor(related);
        return related.Any(grant => IsMember(grant, grant.Issue.RecipientSeat, card, current));
    }
    public bool IsIssuedExempt(int recipientSeat, Card card) => Valid(recipientSeat) &&
        cards.TryGetLocation(card.Id, out var location) && location == CardLocation.Hand(recipientSeat) &&
        GetExemptCardIds(recipientSeat, [card], []).Contains(card.Id);
    public bool Matches(int holderSeat, Card card, HandLimitMatch match) => MatchCard(holderSeat, card, match, true);

    public IReadOnlySet<int> GetExemptCardIds(int recipientSeat, IReadOnlyList<Card> physicalHand,
        IReadOnlyList<HandLimitMatch> currentDeclarations)
    {
        if (!Valid(recipientSeat)) return new HashSet<int>();
        var relatedGrants = store.Grants.Where(grant => !grant.Expired && grant.Issue.RecipientSeat == recipientSeat).ToArray();
        var current = CaptureFor(relatedGrants);
        var active = relatedGrants.Where(grant => HandLimitExemptionStore.HasTurnScope(grant, current)).ToArray();
        var selectors = currentDeclarations.Concat(active.Select(grant => grant.Issue.Spec.Match)).ToArray();
        if (selectors.Length == 0) return new HashSet<int>();
        var markIds = selectors.Where(match => match.Mark is not null).Select(match => match.Mark!)
            .Distinct(StringComparer.Ordinal).ToDictionary(mark => mark, mark => marks.MarkedCardIds(mark).ToHashSet(), StringComparer.Ordinal);
        var needAlias = selectors.Any(match => match.Face == HandLimitCardFace.LegacyHandIdentity && match.CardKinds.Count != 0);
        var needSuit = selectors.Any(match => match.EffectiveSuits.Count != 0 || match.IsRed is not null);
        var currentEntries = new Dictionary<int, long?>();
        var byEntity = new Dictionary<int, List<HandLimitExemptionGrant>>();
        var anyEntity = new List<HandLimitExemptionGrant>();
        foreach (var grant in active)
            if (grant.Issue.Spec.ExactCardIds is { } ids)
                foreach (var id in ids)
                {
                    if (!byEntity.TryGetValue(id, out var related)) byEntity[id] = related = [];
                    related.Add(grant);
                }
            else anyEntity.Add(grant);
        var living = participants.At(recipientSeat).IsAlive;
        var result = new HashSet<int>();
        foreach (var card in physicalHand)
        {
            if (!cards.TryGetLocation(card.Id, out var location) || location != CardLocation.Hand(recipientSeat)) continue;
            var alias = needAlias ? facts.LegacyHandKind(recipientSeat, card) : card.Kind;
            var suit = needSuit ? facts.EffectiveSuit(recipientSeat, card) : null;
            var exempt = false;
            foreach (var match in currentDeclarations)
                if (MatchCardValue(card, match, living, true, match.Mark is null || markIds[match.Mark].Contains(card.Id), alias, suit))
                { exempt = true; break; }
            if (!exempt)
                foreach (var grant in anyEntity)
                    if (MatchesBatchGrant(grant, recipientSeat, card, living, alias, suit, markIds, currentEntries))
                    { exempt = true; break; }
            if (!exempt && byEntity.TryGetValue(card.Id, out var related))
                foreach (var grant in related)
                    if (MatchesBatchGrant(grant, recipientSeat, card, living, alias, suit, markIds, currentEntries))
                    { exempt = true; break; }
            if (exempt) result.Add(card.Id);
        }
        return result;
    }

    private bool MatchesBatchGrant(HandLimitExemptionGrant grant, int recipient, Card card, bool living,
        CardKind alias, Suit? suit, Dictionary<string, HashSet<int>> markIds, Dictionary<int, long?> currentEntries)
    {
        if (grant.Issue.Spec.RequireCurrentHandEntry)
        {
            if (!currentEntries.TryGetValue(card.Id, out var actual))
                currentEntries[card.Id] = actual = entries.CurrentHandEntry(recipient, card.Id);
            if (!grant.HandEntries.Any(entry => entry.CardId == card.Id && actual == entry.EntrySequence)) return false;
        }
        var match = grant.Issue.Spec.Match;
        return MatchCardValue(card, match, living, true, match.Mark is null || markIds[match.Mark].Contains(card.Id), alias, suit);
    }

    private bool IsMember(HandLimitExemptionGrant grant, int recipient, Card card, HandLimitCurrentTurn current) =>
        grant.Issue.RecipientSeat == recipient && HandLimitExemptionStore.HasTurnScope(grant, current) &&
        (grant.Issue.Spec.ExactCardIds is not { } ids || ids.Contains(card.Id)) &&
        (!grant.Issue.Spec.RequireCurrentHandEntry || grant.HandEntries.Any(entry => entry.CardId == card.Id &&
            entries.CurrentHandEntry(recipient, card.Id) == entry.EntrySequence)) &&
        MatchCard(recipient, card, grant.Issue.Spec.Match, false);

    private bool MatchCard(int holder, Card card, HandLimitMatch match, bool requireLiving)
    {
        if (!Valid(holder)) return false;
        return MatchCardValue(card, match, participants.At(holder).IsAlive, requireLiving,
            match.Mark is null || marks.Has(card.Id, match.Mark),
            match.Face == HandLimitCardFace.LegacyHandIdentity ? facts.LegacyHandKind(holder, card) : card.Kind,
            match.EffectiveSuits.Count != 0 || match.IsRed is not null ? facts.EffectiveSuit(holder, card) : null);
    }

    private static bool MatchCardValue(Card card, HandLimitMatch match, bool living, bool requireLiving,
        bool hasMark, CardKind aliasKind, Suit? suit)
    {
        if (requireLiving && match.RequireLivingHolder && !living || !hasMark ||
            match.PrintedRank is { } rank && card.Rank != rank ||
            match.CardKinds.Count != 0 && !match.CardKinds.Contains(card.Kind) &&
                (match.Face != HandLimitCardFace.LegacyHandIdentity || !match.CardKinds.Contains(aliasKind))) return false;
        if (match.EffectiveSuits.Count == 0 && match.IsRed is null) return true;
        if (match.EffectiveSuits.Count != 0 && (suit is null || !match.EffectiveSuits.Contains(suit.Value))) return false;
        bool? red = suit switch { Suit.Heart or Suit.Diamond => true, Suit.Spade or Suit.Club => false, _ => null };
        return match.IsRed is null || red is not null && match.IsRed == red;
    }

    private bool Valid(int seat) => seat >= 0 && seat < participants.Players.Count;
    private HandLimitCurrentTurn CaptureFor(IReadOnlyList<HandLimitExemptionGrant> grants) =>
        HandLimitCurrentTurn.Capture(turn, facts,
            grants.Any(grant => grant.Issue.Spec.Expiry == HandLimitExemptionExpiry.ActualTurnOpen)
                ? HandLimitExemptionExpiry.ActualTurnOpen
                : grants.Any(grant => grant.Issue.Spec.Expiry == HandLimitExemptionExpiry.FrozenTurnCoordinates)
                    ? HandLimitExemptionExpiry.FrozenTurnCoordinates : HandLimitExemptionExpiry.Permanent);
    // A projected Card value need not identify a registered physical entity.
    // Unknown identity has no issued relation; strict movement/payment reads retain GetLocation.
    private Card? FindCard(int id) => id > 0 && cards.TryGetLocation(id, out var location)
        ? cards.CardsAt(location).SingleOrDefault(card => card.Id == id) : null;
    private static bool SameSpec(HandLimitExemptionSpec a, HandLimitExemptionSpec b) => SameMatch(a.Match, b.Match) &&
        (a.ExactCardIds is null ? b.ExactCardIds is null : b.ExactCardIds is not null && a.ExactCardIds.SequenceEqual(b.ExactCardIds)) &&
        a.Expiry == b.Expiry && a.Turn == b.Turn && a.RequireCurrentHandEntry == b.RequireCurrentHandEntry &&
        a.AllowBeforeFirstTurn == b.AllowBeforeFirstTurn && a.TurnRequirements == b.TurnRequirements &&
        a.NativeExpiration == b.NativeExpiration && a.Until.SequenceEqual(b.Until);
    internal static bool SameMatch(HandLimitMatch a, HandLimitMatch b) => a.Mark == b.Mark && a.Face == b.Face &&
        a.RequireLivingHolder == b.RequireLivingHolder && a.IsRed == b.IsRed && a.PrintedRank == b.PrintedRank &&
        a.CardKinds.SequenceEqual(b.CardKinds) && a.EffectiveSuits.SequenceEqual(b.EffectiveSuits);
}
