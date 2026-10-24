namespace CardGame.Core;

/// <summary>The match's existing deterministic source. Setup must not allocate an independent random stream.</summary>
public interface IModeSetupRandom
{
    int Next(int exclusiveMax);
    void Shuffle<T>(IList<T> values);
}

/// <summary>
/// Installs the setup service as its principal feature. An exact-mode setup replaces the all-modes default;
/// independent command, observer and encounter factories keep their ordinary composition.
/// </summary>
public interface IGameModeSetupFeatureFactory : IGameModeFeatureFactory { }

/// <summary>Pure setup algorithms. The engine alone applies plans to players, cards and private draft state.</summary>
public interface IMatchModeSetupFeature : IGameModeFeature
{
    ModeSetupPlan BuildPlayers();
    ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request);
    IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original);
    IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original);
    ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats);
    ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId);
}

public sealed record ModeSetupSkillGrant(string GrantId, string SkillId, string SkillInstanceId,
    string SourceId, bool IsModeRule = false);

/// <summary>No live character or mutable skill set is exposed by a seat recipe.</summary>
public sealed record ModeSeatSetup(int Seat, string Name, bool IsHuman, Role Role,
    string? GeneralId, int MaxHp, int Hp)
{
    public string? TeamId { get; init; }
    public bool TeamRevealed { get; init; }
    public bool RoleRevealed { get; init; }
    public string? NationalFactionId { get; init; }
    public bool FactionRevealed { get; init; }
    public bool GeneralSelected { get; init; }
    public bool GeneralRevealed { get; init; }
    public string? SecondaryGeneralId { get; init; }
    public bool SecondaryGeneralSelected { get; init; }
    public bool SecondaryGeneralRevealed { get; init; }
    public bool IsAlive { get; init; } = true;
    private readonly IReadOnlyList<string> _extraSkills = Array.Empty<string>();
    public IReadOnlyList<string> ExtraGeneralSkillIds
    { get => _extraSkills; init => _extraSkills = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<ModeSetupSkillGrant> _skillGrants = Array.Empty<ModeSetupSkillGrant>();
    public IReadOnlyList<ModeSetupSkillGrant> SkillGrants
    { get => _skillGrants; init => _skillGrants = Array.AsReadOnly(value.ToArray()); }
    private readonly IReadOnlyList<string>? _draftCandidates;
    public IReadOnlyList<string>? GeneralDraftCandidateIds
    { get => _draftCandidates; init => _draftCandidates = value is null ? null : Array.AsReadOnly(value.ToArray()); }
}

public sealed class ModeSetupPlan
{
    public ModeSetupPlan(IEnumerable<ModeSeatSetup> seats, IEnumerable<string> availableGeneralIds,
        IEnumerable<int> selectionOrder, IEnumerable<string>? shenwuSharedGodIds = null,
        string? shenwuLeaderGeneralId = null, GuanduBattleConfiguredEvent? guanduConfiguration = null)
    {
        Seats = Array.AsReadOnly(seats.ToArray());
        AvailableGeneralIds = Array.AsReadOnly(availableGeneralIds.ToArray());
        SelectionOrder = Array.AsReadOnly(selectionOrder.ToArray());
        ShenwuSharedGodIds = shenwuSharedGodIds is null ? null : Array.AsReadOnly(shenwuSharedGodIds.ToArray());
        ShenwuLeaderGeneralId = shenwuLeaderGeneralId;
        GuanduConfiguration = guanduConfiguration;
    }
    public IReadOnlyList<ModeSeatSetup> Seats { get; }
    public IReadOnlyList<string> AvailableGeneralIds { get; }
    public IReadOnlyList<int> SelectionOrder { get; }
    public IReadOnlyList<string>? ShenwuSharedGodIds { get; }
    public string? ShenwuLeaderGeneralId { get; }
    public GuanduBattleConfiguredEvent? GuanduConfiguration { get; }
}

/// <summary>Trusted scalar draft facts, kept separate from player-facing projections.</summary>
public sealed record ModeSeatSetupFacts(int Seat, bool IsHuman, Role Role, string? TeamId,
    string? NationalFactionId, string? GeneralId, bool GeneralSelected,
    string? SecondaryGeneralId = null, bool SecondaryGeneralSelected = false)
{
    public bool IsGuanduCommander { get; init; }
}

public sealed class ModeGeneralOfferRequest
{
    public ModeGeneralOfferRequest(ModeSeatSetupFacts seat, IEnumerable<string> availableGeneralIds,
        IEnumerable<ModeSeatSetupFacts> players, IEnumerable<string>? heldDraftCandidateIds = null,
        bool fullRoster = false, IEnumerable<string>? shenwuSharedGodIds = null,
        string? shenwuLeaderGeneralId = null, GuanduBattleConfiguredEvent? guanduConfiguration = null)
    {
        Seat = seat;
        AvailableGeneralIds = Array.AsReadOnly(availableGeneralIds.ToArray());
        Players = Array.AsReadOnly(players.ToArray());
        HeldDraftCandidateIds = heldDraftCandidateIds is null ? null : Array.AsReadOnly(heldDraftCandidateIds.ToArray());
        FullRoster = fullRoster;
        ShenwuSharedGodIds = shenwuSharedGodIds is null ? null : Array.AsReadOnly(shenwuSharedGodIds.ToArray());
        ShenwuLeaderGeneralId = shenwuLeaderGeneralId;
        GuanduConfiguration = guanduConfiguration;
    }
    public ModeSeatSetupFacts Seat { get; }
    public IReadOnlyList<string> AvailableGeneralIds { get; }
    public IReadOnlyList<ModeSeatSetupFacts> Players { get; }
    public IReadOnlyList<string>? HeldDraftCandidateIds { get; }
    public bool FullRoster { get; }
    public IReadOnlyList<string>? ShenwuSharedGodIds { get; }
    public string? ShenwuLeaderGeneralId { get; }
    public GuanduBattleConfiguredEvent? GuanduConfiguration { get; }
}

public sealed record ModeGeneralOffer(string GeneralId, string CandidateId);
public sealed class ModeGeneralOfferPlan
{
    public ModeGeneralOfferPlan(IEnumerable<ModeGeneralOffer> offers, IEnumerable<string>? frozenCandidateIds = null)
    {
        Offers = Array.AsReadOnly(offers.ToArray());
        FrozenCandidateIds = frozenCandidateIds is null ? null : Array.AsReadOnly(frozenCandidateIds.ToArray());
    }
    public IReadOnlyList<ModeGeneralOffer> Offers { get; }
    /// <summary>Non-null only when this request drew a new private family draft for its owning seat.</summary>
    public IReadOnlyList<string>? FrozenCandidateIds { get; }
}

public sealed class ModeSelectedGeneralPlan
{
    public ModeSelectedGeneralPlan(IEnumerable<string> extraSkillIds, int? maximumHp = null, int? hp = null)
    {
        ExtraSkillIds = Array.AsReadOnly(extraSkillIds.ToArray());
        MaximumHp = maximumHp;
        Hp = hp;
    }
    public IReadOnlyList<string> ExtraSkillIds { get; }
    public int? MaximumHp { get; }
    public int? Hp { get; }
}

public sealed record ModeInitialEquipmentPlacement(int Seat, int CardId);
public sealed class ModeSelectedEquipmentPlan
{
    public ModeSelectedEquipmentPlan(IEnumerable<Card> cards, IEnumerable<ModeInitialEquipmentPlacement> placements,
        IEnumerable<IGameEvent> completionFacts)
    {
        Cards = Array.AsReadOnly(cards.ToArray());
        Placements = Array.AsReadOnly(placements.ToArray());
        CompletionFacts = Array.AsReadOnly(completionFacts.ToArray());
    }
    public IReadOnlyList<Card> Cards { get; }
    public IReadOnlyList<ModeInitialEquipmentPlacement> Placements { get; }
    public IReadOnlyList<IGameEvent> CompletionFacts { get; }
}
