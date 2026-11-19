namespace CardGame.Core;

/// <summary>Public facts after both physical cards have been revealed. Ties are not wins.</summary>
public sealed record PindianResult(
    int SourceSeat, int OpponentSeat, int SourceCardId, int OpponentCardId,
    int SourceRank, int OpponentRank)
{
    public bool SourceWon => SourceRank > OpponentRank;
    public bool WonBy(int seat) => seat == SourceSeat ? SourceWon :
        seat == OpponentSeat && OpponentRank > SourceRank;
    internal bool SourceMayClaimAll(int seat) => seat == SourceSeat && !WonBy(OpponentSeat);
    public int CardOf(int seat) => seat == SourceSeat ? SourceCardId :
        seat == OpponentSeat ? OpponentCardId : throw new ArgumentOutOfRangeException(nameof(seat));
}

public sealed record PindianDiscardEntry(int CardId, int EntrySequence);

public sealed record PindianProcessingEntry(int CardId, long EntrySequence)
{
    internal static bool IsCurrent(ResolutionFrame parent, ICardZoneEntryQueries zoneEntries, int cardId)
    {
        var entries = parent switch
        {
            PindianFrame pair => pair.ProcessingEntries,
            ProgramSkillFrame { SimultaneousPindian: { } group } => group.ProcessingEntries,
            _ => null
        };
        var entry = entries?.SingleOrDefault(item => item.CardId == cardId);
        return entry is not null && zoneEntries.CurrentEntry(CardLocation.Processing, cardId) == entry.EntrySequence;
    }
}

public enum PindianStep { ChooseParticipants, ChooseSourceCard, ChooseOpponentCard, ClaimResult = 780 }

/// <summary>Trusted-host data only. It is not included in player snapshots.</summary>
public sealed partial record PindianFrame(
    long Id, long ParentFrameId, string SkillId, SkillPromptPresentation Presentation,
    int SourceSeat, int? OpponentSeat = null, int? SourceCardId = null,
    PindianStep PindianStep = PindianStep.ChooseParticipants,
    PindianResult? Result = null,
    string? ProgramResultBind = null,
    SkillProgramCardSetVisibility ProgramResultVisibility = SkillProgramCardSetVisibility.Public,
    IReadOnlyList<int>? ParentProcessingCardIds = null,
    ResolutionFrameStep Step = ResolutionFrameStep.AwaitingResponse,
    IReadOnlyList<int>? ClaimSeats = null,
    int ClaimIndex = 0)
    : ResolutionFrame(Id, ResolutionFrameKind.Pindian, Step)
{
    [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
    public bool SourceUsesDrawPileTop { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public int? SourceTopReservationSequence { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<PindianProcessingEntry>? ProcessingEntries { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public PindianRandomSelection? RandomSelection { get; init; }
    [System.Text.Json.Serialization.JsonIgnore(Condition=System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
    public PindianPolicyClaims? PolicyClaims { get; init; }
}

public sealed record PindianResultDeterminedEvent(
    long FrameId, string SkillId, PindianResult Result) : IGameEvent;
