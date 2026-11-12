using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum HandLimitCardFace { Printed, LegacyHandIdentity }
public enum HandLimitExemptionExpiry { Permanent, FrozenTurnCoordinates, ActualTurnOpen }
public sealed record HandLimitTurnCoordinates(int TurnNumber, int OwnerSeat);
[Flags]
public enum HandLimitTurnRequirements
{
    None = 0, SetupComplete = 1, PositiveTurn = 2, ProgressionTurnMatches = 4,
    OwnerAtCurrentCursor = 8, NoTurnEnded = 16, NoTurnReturned = 32
}
public readonly record struct HandLimitTurnStatus(bool SetupComplete, bool ProgressionTurnMatches,
    bool OwnerAtCurrentCursor, bool TurnEnded, bool TurnReturned);

/// <summary>A card selector shared by current skill declarations and already-issued recipient relations.</summary>
public sealed record HandLimitMatch
{
    public HandLimitMatch(string? Mark = null, IReadOnlyList<CardKind>? CardKinds = null,
        HandLimitCardFace Face = HandLimitCardFace.Printed, IReadOnlyList<Suit>? EffectiveSuits = null,
        bool? IsRed = null, bool RequireLivingHolder = false, int? PrintedRank = null)
    {
        if (Mark is not null && string.IsNullOrWhiteSpace(Mark) || !Enum.IsDefined(Face) ||
            CardKinds?.Any(kind => !Enum.IsDefined(kind)) == true || EffectiveSuits?.Any(suit => !Enum.IsDefined(suit)) == true ||
            CardKinds is not null && CardKinds.Distinct().Count() != CardKinds.Count ||
            EffectiveSuits is not null && EffectiveSuits.Distinct().Count() != EffectiveSuits.Count ||
            PrintedRank is < 1 or > 13)
            throw new InvalidOperationException("A hand-limit card selector requires known faces, suits and a declared mark identity.");
        this.Mark = Mark;
        this.CardKinds = Array.AsReadOnly((CardKinds ?? []).ToArray());
        this.Face = Face;
        this.EffectiveSuits = Array.AsReadOnly((EffectiveSuits ?? []).ToArray());
        this.IsRed = IsRed;
        this.RequireLivingHolder = RequireLivingHolder;
        this.PrintedRank = PrintedRank;
    }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public string? Mark { get; }
    public IReadOnlyList<CardKind> CardKinds { get; }
    public HandLimitCardFace Face { get; }
    public IReadOnlyList<Suit> EffectiveSuits { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public bool? IsRed { get; }
    public bool RequireLivingHolder { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public int? PrintedRank { get; }
}

public sealed record HandLimitExemptionSpec
{
    public HandLimitExemptionSpec(HandLimitMatch Match, IReadOnlyList<int>? ExactCardIds = null,
        HandLimitExemptionExpiry Expiry = HandLimitExemptionExpiry.Permanent, HandLimitTurnCoordinates? Turn = null,
        bool RequireCurrentHandEntry = false, IReadOnlyList<SkillStateBoundarySpec>? Until = null,
        bool AllowBeforeFirstTurn = false, HandLimitTurnRequirements TurnRequirements = HandLimitTurnRequirements.None,
        SkillProgramDamageModifierExpiration? NativeExpiration = null)
    {
        ArgumentNullException.ThrowIfNull(Match);
        if (!Enum.IsDefined(Expiry) || ExactCardIds?.Any(id => id <= 0) == true ||
            ExactCardIds is not null && ExactCardIds.Distinct().Count() != ExactCardIds.Count ||
            RequireCurrentHandEntry && ExactCardIds is null ||
            AllowBeforeFirstTurn && Expiry != HandLimitExemptionExpiry.ActualTurnOpen ||
            (TurnRequirements & ~(HandLimitTurnRequirements.SetupComplete | HandLimitTurnRequirements.PositiveTurn |
                HandLimitTurnRequirements.ProgressionTurnMatches | HandLimitTurnRequirements.OwnerAtCurrentCursor |
                HandLimitTurnRequirements.NoTurnEnded | HandLimitTurnRequirements.NoTurnReturned)) != 0 ||
            TurnRequirements != HandLimitTurnRequirements.None && Expiry != HandLimitExemptionExpiry.ActualTurnOpen ||
            Expiry == HandLimitExemptionExpiry.ActualTurnOpen && TurnRequirements == HandLimitTurnRequirements.None ||
            NativeExpiration is { } native && !Enum.IsDefined(native) || NativeExpiration is not null && Until is { Count: > 0 })
            throw new InvalidOperationException("An issued hand-limit relation requires its exact entity scope and declared remaining lifetime.");
        this.Match = Match;
        this.ExactCardIds = ExactCardIds is null ? null : Array.AsReadOnly(ExactCardIds.ToArray());
        this.Expiry = Expiry;
        this.Turn = Turn;
        this.RequireCurrentHandEntry = RequireCurrentHandEntry;
        this.Until = Array.AsReadOnly((Until ?? []).ToArray());
        this.AllowBeforeFirstTurn = AllowBeforeFirstTurn;
        this.TurnRequirements = TurnRequirements;
        this.NativeExpiration = NativeExpiration;
    }
    public HandLimitMatch Match { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public IReadOnlyList<int>? ExactCardIds { get; }
    public HandLimitExemptionExpiry Expiry { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public HandLimitTurnCoordinates? Turn { get; }
    public bool RequireCurrentHandEntry { get; }
    public IReadOnlyList<SkillStateBoundarySpec> Until { get; }
    public bool AllowBeforeFirstTurn { get; }
    public HandLimitTurnRequirements TurnRequirements { get; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public SkillProgramDamageModifierExpiration? NativeExpiration { get; }
}

public sealed record HandLimitExemptionIssue(long IssuerFrameId, int InstructionIndex, int IssuanceSlot,
    string RelationId, CardConversionSource Source, string GameplayHash, int RecipientSeat, HandLimitExemptionSpec Spec);
public sealed record HandLimitExemptionHandEntry(int CardId, long EntrySequence);
public sealed record HandLimitExemptionGrant(long Sequence, HandLimitExemptionIssue Issue,
    HandLimitTurnCoordinates IssuedTurn, SkillStateLifetime? Lifetime,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] bool Expired = false)
{
    private readonly IReadOnlyList<HandLimitExemptionHandEntry> _handEntries = [];
    public IReadOnlyList<HandLimitExemptionHandEntry> HandEntries
    {
        get => _handEntries;
        init => _handEntries = Array.AsReadOnly(value.ToArray());
    }
}

public sealed partial record SkillProgramCardPolicy
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public HandLimitMatch? HandLimitMatch { get; init; }
}

internal interface IHandLimitExemptionCardFacts : IProgramSelectionQuery
{
    CardKind LegacyHandKind(int holderSeat, Card card);
    Suit? EffectiveSuit(int holderSeat, Card card);
    HandLimitTurnStatus TurnStatus { get; }
}
internal interface IHandLimitExemptionBoundaries : IProgramRuntimeOperations
{
    void ProcessBoundary(SkillStateBoundary boundary);
}
internal interface IHandLimitExemptionSequences : IProgramRuntimeOperations
{
    long Next();
}
internal interface IHandLimitExemptionNativeLifetimes : IProgramRuntimeOperations
{
    IReadOnlyList<long> ExpireTurn(int turnNumber, int turnSeat);
    IReadOnlyList<long> ExpireOwnerTurnStart(int owner, int currentTurn);
    IReadOnlyList<long> ExpireOwnerDeath(int owner);
}
internal interface IHandLimitExemptionStore : IProgramRuntimeOperations
{
    IReadOnlyList<HandLimitExemptionGrant> Grants { get; }
    HandLimitExemptionGrant Add(HandLimitExemptionGrant grant);
}
internal interface IHandLimitExemptions : IProgramRuntimeOperations
{
    HandLimitExemptionGrant Grant(ProgramSkillFrame frame, string relationId, int recipientSeat,
        HandLimitExemptionSpec spec, int issuanceSlot = 0);
    HandLimitExemptionGrant Grant(HandLimitExemptionIssue issue);
    HandLimitExemptionGrant? Get(long sequence);
    IReadOnlyList<HandLimitExemptionGrant> ActiveRelations(string relationId, int recipientSeat);
    HandLimitExemptionGrant? Apply(ProgramSkillFrame frame, int recipientSeat, ProgramHandLimitExemption declaration);
    // Relation membership is not card-use authorization and does not imply current Hand or a living recipient.
    bool IsMember(long sequence, int recipientSeat, int cardId);
    IReadOnlyList<HandLimitExemptionGrant> GetMatchingRelations(string relationId, int recipientSeat, int cardId);
    bool HasEntityRelation(string relationId, int cardId);
    bool IsIssuedExempt(int recipientSeat, Card card);
    bool Matches(int holderSeat, Card card, HandLimitMatch match);
    IReadOnlySet<int> GetExemptCardIds(int recipientSeat, IReadOnlyList<Card> physicalHand,
        IReadOnlyList<HandLimitMatch> currentDeclarations);
}

public sealed record ProgramHandLimitExemption(string RelationId, HandLimitExemptionSpec Spec, string? CardsBind = null);
