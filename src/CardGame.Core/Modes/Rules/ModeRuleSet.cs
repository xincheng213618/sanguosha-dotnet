namespace CardGame.Core;

public interface IMatchModeRuleProvider : IGameModeFeature
{
    ModeRuleSet Rules { get; }
}

/// <summary>Immutable rule profile supplied by the installed mode implementation, with no mode selection in Core.</summary>
public sealed class ModeRuleSet
{
    public ModeRuleSet(IModeOpeningRule opening, IModeDeathRule death, ModeCardRules cards,
        bool usesGeneralBaseHp, bool usesClassicBattleRules, bool supportsGodFactionSelection,
        bool reducesFirstActorInitialHand, bool allowsDiscardRecycling = true,
        bool publishesCardMovementDisclosure = false, bool revealsTeamsOnStart = false,
        ModeVisibilityRules? visibility = null, IEnumerable<Type>? requiredFeatureContracts = null,
        bool isClassicIdentityContext = false, bool requiresRoundTracking = false,
        bool requiresRecipientDamageReevaluation = false, ModeRuntimeRequirements? runtimeRequirements = null)
    {
        Opening = opening ?? throw new ArgumentNullException(nameof(opening));
        Death = death ?? throw new ArgumentNullException(nameof(death));
        Cards = cards ?? throw new ArgumentNullException(nameof(cards));
        UsesGeneralBaseHp = usesGeneralBaseHp;
        UsesClassicBattleRules = usesClassicBattleRules;
        IsClassicIdentityContext = isClassicIdentityContext;
        RequiresRoundTracking = requiresRoundTracking;
        RequiresRecipientDamageReevaluation = requiresRecipientDamageReevaluation;
        RuntimeRequirements = runtimeRequirements ?? new ModeRuntimeRequirements();
        SupportsGodFactionSelection = supportsGodFactionSelection;
        ReducesFirstActorInitialHand = reducesFirstActorInitialHand;
        AllowsDiscardRecycling = allowsDiscardRecycling;
        PublishesCardMovementDisclosure = publishesCardMovementDisclosure;
        RevealsTeamsOnStart = revealsTeamsOnStart;
        Visibility = visibility ?? new ModeVisibilityRules();
        var required = (requiredFeatureContracts ?? []).ToArray();
        if (required.Any(contract => contract is null || !contract.IsInterface || contract.ContainsGenericParameters ||
                contract == typeof(IGameModeFeature) || !typeof(IGameModeFeature).IsAssignableFrom(contract)) ||
            required.Distinct().Count() != required.Length)
            throw new ArgumentException("A mode profile requires unique concrete mode-feature interfaces.", nameof(requiredFeatureContracts));
        RequiredFeatureContracts = Array.AsReadOnly(required);
    }

    public IModeOpeningRule Opening { get; }
    public IModeDeathRule Death { get; }
    public ModeCardRules Cards { get; }
    public bool UsesGeneralBaseHp { get; }
    public bool UsesClassicBattleRules { get; }
    public bool IsClassicIdentityContext { get; }
    public bool RequiresRoundTracking { get; }
    public bool RequiresRecipientDamageReevaluation { get; }
    public ModeRuntimeRequirements RuntimeRequirements { get; }
    public bool SupportsGodFactionSelection { get; }
    public bool ReducesFirstActorInitialHand { get; }
    public bool AllowsDiscardRecycling { get; }
    public bool PublishesCardMovementDisclosure { get; }
    public bool RevealsTeamsOnStart { get; }
    public ModeVisibilityRules Visibility { get; }
    public IReadOnlyList<Type> RequiredFeatureContracts { get; }
    public int? FixedDistance { get; init; }
    public int? InitialHandSizeOverride { get; init; }
}

/// <summary>Trusted life facts only; no mutable character, hidden cards or runtime store crosses this boundary.</summary>
public readonly record struct ModePlayerLife(int Seat, Role Role, bool IsAlive,
    string? TeamId, string? NationalFactionId);
