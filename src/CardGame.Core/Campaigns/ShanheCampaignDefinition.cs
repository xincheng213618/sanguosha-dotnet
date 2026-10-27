using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace CardGame.Core.Campaigns;

public enum ShanheNodeKind { Battle, EliteBattle, BossBattle, Event, Shop, Camp }
public enum ShanheCampaignPhase { Map, Battle, Reward, NodeChoice, Won, Lost, OpeningSkillDraft }
public enum ShanheRewardCategory { Tactic, Card, Skill, Supply }

/// <summary>Content-defined, additive bonuses. A campaign never changes the global rules epoch.</summary>
public sealed record ShanheTacticBonuses(int MaxHp = 0, int InitialHand = 0, int Draw = 0, int Slash = 0);

public sealed record ShanheStartingCard(CardKind Kind, Suit Suit = Suit.Spade, int Rank = 1);

/// <summary>A reserved second form replaces the indexed enemy in the same battle.</summary>
public sealed record ShanheBossTransition(int EnemyIndex, string GeneralId, int MaxHp, int Hp, int DrawCards = 0)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheCombatantDefinition? Combatant { get; init; }
    private readonly IReadOnlyList<ShanheStartingCard>? _startingEquipment;
    /// <summary>Physical equipment installed once when the reserved form enters.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheStartingCard>? StartingEquipment
    {
        get => _startingEquipment;
        init => _startingEquipment = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

public sealed record ShanheCombatantDefinition(
    string GeneralId, int MaxHpBonus = 0, int? Hp = null, int InitialHandBonus = 0,
    int DrawBonus = 0, int SlashBonus = 0, IReadOnlyList<string>? ExtraSkillIds = null)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int InitialArmor { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ArmorRecoveryPerRound { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ArmorRecoveryCap { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ExtraDamage { get; init; }
    private readonly IReadOnlyList<ShanheSourceEnemyTierAttributes>? _sourceDifficultyAttributes;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheSourceEnemyTierAttributes>? SourceDifficultyAttributes
    {
        get => _sourceDifficultyAttributes;
        init => _sourceDifficultyAttributes = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    private readonly IReadOnlyList<ShanheStartingCard>? _startingHand;
    private readonly IReadOnlyList<ShanheStartingCard>? _startingCards;
    private readonly IReadOnlyList<ShanheStartingCard>? _startingEquipment;
    private readonly IReadOnlyList<string>? _qualifiedPrintedLordSkillIds;
    /// <summary>Explicit scenario qualification for these original primary-general Lord skills only.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? QualifiedPrintedLordSkillIds
    {
        get => _qualifiedPrintedLordSkillIds;
        init => _qualifiedPrintedLordSkillIds = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    /// <summary>Replaces the ordinary opening hand; null retains the deck's usual base. Bonuses add random cards.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheStartingCard>? StartingHand
    {
        get => _startingHand;
        init => _startingHand = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    /// <summary>Extra physical cards added to hand after the ordinary or explicit opening hand has been dealt.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheStartingCard>? StartingCards
    {
        get => _startingCards;
        init => _startingCards = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    /// <summary>Opening NPC equipment installed after the hand is dealt. Reserved forms use ShanheBossTransition.StartingEquipment instead.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheStartingCard>? StartingEquipment
    {
        get => _startingEquipment;
        init => _startingEquipment = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
}

/// <summary>The entire immutable input to one encounter, including its run identity.</summary>
public sealed record ShanheBattleSetup(
    int RunSeed, int EncounterOrdinal, string NodeId, string PlayerGeneralId, int Hp,
    int MaxHpBonus, int InitialHandBonus, int DrawBonus, int SlashBonus,
    IReadOnlyList<string> ExtraSkillIds, IReadOnlyList<ShanheStartingCard> StartingCards,
    IReadOnlyList<ShanheStartingCard> StartingEquipment,
    IReadOnlyList<ShanheCombatantDefinition> EnemyRoster,
    IReadOnlyList<ShanheCombatantDefinition> AllyRoster, string CampaignHash)
{
    public const string BattleModeId = "campaign:shanhe-battle";
    public const string PlayerTeamId = "team:shanhe-player";
    public const string EnemyTeamId = "team:shanhe-enemy";
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SourceDifficultyId { get; init; }
    public IReadOnlyList<string> TacticSkillIds { get; init; } = Array.Empty<string>();
    public IReadOnlyList<int> ActiveSchoolIds { get; init; } = Array.Empty<int>();
    /// <summary>Unlocked campaign skill capacity; printed, tactic and rank-privilege skills do not occupy these slots.</summary>
    public int ExtraSkillSlots { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TerrainSkillId { get; init; }
    /// <summary>Fallback for a printed God general with no chosen or native effective faction.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultGodFactionId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? PlayerRank { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int SharedAllyRank { get; init; }
    /// <summary>Allied roster seats whose own rank-five privilege was spent earlier in this expedition.</summary>
    public IReadOnlyList<int> AllyRankRebirthUsedSeats { get; init; } = Array.Empty<int>();
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool RebirthAvailable { get; init; }

    /// <summary>Initial hand resources leave play instead of their ordinary post-use discard. Starting equipment is installed directly.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool StartingResourcesExpireOnUse { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheBossTransition? BossTransition { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheBattleMarketSetup? MarketSetup { get; init; }

    /// <summary>Zero preserves historical recipes; each issued challenge owns a new deterministic shuffle.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int AttemptNumber { get; init; }

    public int BattleSeed => AttemptNumber == 0 ? DeriveSeed(RunSeed, NodeId, EncounterOrdinal)
        : DeriveSeed(DeriveSeed(RunSeed, NodeId, EncounterOrdinal), "challenge", AttemptNumber);

    public static bool ValueEquals(ShanheBattleSetup? left, ShanheBattleSetup? right) =>
        left is null || right is null ? left is null && right is null :
        ShanheCampaignJson.SerializeValue(Freeze(left)) == ShanheCampaignJson.SerializeValue(Freeze(right));

    public static ShanheBattleSetup Freeze(ShanheBattleSetup setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        if (setup.AttemptNumber < 0) throw new ArgumentException("A battle attempt cannot be negative.", nameof(setup));
        if (setup.ExtraSkillSlots is < 0 or > 8)
            throw new ArgumentException("Campaign extra skill capacity must be between zero and eight.", nameof(setup));
        if (setup.SourceDifficultyId is < 1 or > 25) throw new ArgumentException("Unknown source battle difficulty.", nameof(setup));
        GodFactionSelectionRules.ValidateDefault(setup.DefaultGodFactionId);
        if (setup.TerrainSkillId is not null) ShanheCampaignContent.ValidateTerrainSkill(setup.TerrainSkillId);
        _ = ShanheGeneralRanks.GetBonuses(setup.PlayerRank ?? 1);
        if (setup.SharedAllyRank != 0 && setup.SharedAllyRank != Math.Max(3, setup.PlayerRank ?? 1))
            throw new ArgumentException("Shared ally rank must match the expedition's qualified general rank.", nameof(setup));
        if (setup.RebirthAvailable && setup.PlayerRank != 5)
            throw new ArgumentException("Only a fifth-rank Shanhe general may receive expedition rebirth.", nameof(setup));
        ArgumentNullException.ThrowIfNull(setup.ExtraSkillIds);
        ArgumentNullException.ThrowIfNull(setup.StartingCards);
        ArgumentNullException.ThrowIfNull(setup.StartingEquipment);
        ArgumentNullException.ThrowIfNull(setup.EnemyRoster);
        ArgumentNullException.ThrowIfNull(setup.AllyRoster);
        ArgumentNullException.ThrowIfNull(setup.TacticSkillIds);
        ArgumentNullException.ThrowIfNull(setup.ActiveSchoolIds);
        ArgumentNullException.ThrowIfNull(setup.AllyRankRebirthUsedSeats);
        if (setup.AllyRankRebirthUsedSeats.Any(seat => seat <= 0 || seat > setup.AllyRoster.Count) ||
            setup.AllyRankRebirthUsedSeats.Distinct().Count() != setup.AllyRankRebirthUsedSeats.Count ||
            setup.AllyRankRebirthUsedSeats.Count > 0 && setup.PlayerRank != 5)
            throw new ArgumentException("Spent allied rebirth seats must identify distinct fifth-rank expedition allies.", nameof(setup));
        if (setup.ActiveSchoolIds.Any(id => id is < 1 or > 9) ||
            setup.ActiveSchoolIds.Distinct().Count() != setup.ActiveSchoolIds.Count)
            throw new ArgumentException("A campaign school must be distinct and belong to the nine-school catalogue.", nameof(setup));
        if (setup.EnemyRoster.Any(value => value is null) || setup.AllyRoster.Any(value => value is null))
            throw new ArgumentException("A campaign roster cannot contain null combatants.", nameof(setup));
        if (setup.StartingCards.Any(value => value is null) || setup.StartingEquipment.Any(value => value is null))
            throw new ArgumentException("A campaign opening cannot contain null physical cards.", nameof(setup));
        ShanheCampaignContent.ValidateBossTransition(setup.BossTransition, setup.EnemyRoster);
        return setup with
        {
            ExtraSkillIds = ShanheCampaignContent.Freeze(setup.ExtraSkillIds),
            StartingCards = ShanheCampaignContent.Freeze(setup.StartingCards),
            StartingEquipment = ShanheCampaignContent.Freeze(setup.StartingEquipment),
            TacticSkillIds = ShanheCampaignContent.Freeze(setup.TacticSkillIds),
            ActiveSchoolIds = ShanheCampaignContent.Freeze(setup.ActiveSchoolIds),
            AllyRankRebirthUsedSeats = ShanheCampaignContent.Freeze(setup.AllyRankRebirthUsedSeats),
            EnemyRoster = ShanheCampaignContent.Freeze(setup.EnemyRoster.Select(ShanheCampaignContent.FreezeCombatant)),
            AllyRoster = ShanheCampaignContent.Freeze(setup.AllyRoster.Select(ShanheCampaignContent.FreezeCombatant)),
            BossTransition = ShanheCampaignContent.FreezeBossTransition(setup.BossTransition),
            MarketSetup = setup.MarketSetup is null ? null : ShanheBattleMarketSetup.Freeze(setup.MarketSetup)
        };
    }

    internal static int DeriveSeed(int seed, string nodeId, int ordinal)
    {
        // String.GetHashCode and System.Random are deliberately absent from persisted paths.
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{seed.ToString(System.Globalization.CultureInfo.InvariantCulture)}|{ordinal.ToString(System.Globalization.CultureInfo.InvariantCulture)}|{nodeId}"));
        return unchecked((int)((uint)bytes[0] | (uint)bytes[1] << 8 | (uint)bytes[2] << 16 | (uint)bytes[3] << 24));
    }
}

public sealed record ShanheEncounterDefinition(
    string Id, string Name, IReadOnlyList<ShanheCombatantDefinition> Enemies,
    IReadOnlyList<ShanheCombatantDefinition>? Allies = null, int VictoryGold = 0, int HealAfterVictory = 0)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SourceBattleId { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SourceCityId { get; init; }
    /// <summary>Counts source roster entries whose hide flag is not one; local visible summons do not change this multiplier.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SourceVisibleEnemyCount { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TerrainSkillId { get; init; }
    /// <summary>Optional authored encounter narrative; omitted by historical recipes.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StoryText { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheBossTransition? BossTransition { get; init; }
}

public sealed partial record ShanheRewardDefinition(
    string Id, string Name, int Gold = 0, int Heal = 0, ShanheTacticBonuses? Bonuses = null,
    string? SkillId = null, IReadOnlyList<ShanheStartingCard>? StartingCards = null,
    IReadOnlyList<ShanheStartingCard>? StartingEquipment = null, int AddSkillSlots = 0)
{
    public string Description { get; init; } = "";
    /// <summary>Authored reward rarity: zero unspecified, one common through four legendary.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Rarity { get; init; }
    /// <summary>A campaign tactic program. This never consumes an extra general-skill slot.</summary>
    public string? TacticSkillId { get; init; }
    /// <summary>Projects this owned tactic program only when entering a boss battle.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool BossBattleOnly { get; init; }
    /// <summary>A native expedition-only tactic; it never consumes a general-skill slot.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheCampaignEconomy? CampaignEconomy { get; init; }
    private readonly IReadOnlyList<string>? _openingSkillRewardIds;
    /// <summary>An initial tactic's one-time three-choice skill pool; it is not a battle program.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? OpeningSkillRewardIds
    {
        get => _openingSkillRewardIds;
        init => _openingSkillRewardIds = value is null ? null : Array.AsReadOnly(value.ToArray());
    }
    private readonly IReadOnlyList<ShanheCombatantDefinition>? _openingCompanionPool;
    /// <summary>One companion is selected once when this tactic is equipped before the expedition.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheCombatantDefinition>? OpeningCompanionPool
    {
        get => _openingCompanionPool;
        init => _openingCompanionPool = value is null ? null :
            ShanheCampaignContent.Freeze(value.Select(member => member is null
                ? throw new ArgumentException("An opening companion cannot be null.") : ShanheCampaignContent.FreezeCombatant(member)));
    }
}

public sealed record ShanheNodeChoiceDefinition(
    string Id, string Name, int GoldCost = 0, int HpCost = 0, string? RewardId = null, bool LeavesNode = true)
{
    public string Description { get; init; } = "";
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheCombatantDefinition? Companion { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BattleNodeId { get; init; }
}

public sealed record ShanheStoryEventDefinition(string Id, string Name, string StoryText,
    IReadOnlyList<ShanheNodeChoiceDefinition> Choices)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<string>? SkillRewardIds { get; init; }
}

public sealed record ShanheNodeDefinition(
    string Id, string Name, ShanheNodeKind Kind, IReadOnlyList<string> NextNodeIds,
    IReadOnlyList<ShanheEncounterDefinition>? Encounters = null, IReadOnlyList<string>? RewardIds = null,
    int RewardChoiceCount = 3, IReadOnlyList<ShanheNodeChoiceDefinition>? Choices = null)
{
    /// <summary>Authored same-chapter elite profile used by a held ordinary-battle upgrade tactic.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? EliteUpgradeNodeId { get; init; }
    public string StoryText { get; init; } = "";
    public string Description { get; init; } = "";
    public string VictoryText { get; init; } = "";
    /// <summary>One story is selected on entry, independently of battle encounters.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<ShanheStoryEventDefinition>? StoryEvents { get; init; }
    /// <summary>A battle entered through a story choice, omitted from the ordinary map.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsStoryBattle { get; init; }
    public int AddSkillSlotsOnVictory { get; init; }
    /// <summary>Compensation for declining this node's published reward offer; zero disables it.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int RewardSkipGold { get; init; }
    /// <summary>Choose a reward category before revealing its frozen candidate offer.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool CategorizedRewards { get; init; }
    /// <summary>The location's seed determines its reward category before the battle.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AutomaticRewardCategory { get; init; }
    /// <summary>Base tactic-offer quality; zero retains unrestricted historical sampling. Promotions use the full reward pool.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int PrimaryTacticRarity { get; init; }
    /// <summary>Base skill-offer quality; zero retains unrestricted historical sampling.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int PrimarySkillRarity { get; init; }
    /// <summary>Base card/equipment-offer quality; zero retains unrestricted historical sampling.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int PrimaryCardRarity { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int ChapterNumber { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ChapterTitle { get; init; }
    /// <summary>Optional completion bundle. Fixed credits replace ordinary encounter credits.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheBossRewardDefinition? BossRewards { get; init; }
}

/// <summary>One named edition and difficulty. Sources and narrative belong to content, not the view model.</summary>
public sealed partial record ShanheCampaignDefinition(
    string Id, string Name, IReadOnlyList<string> PlayerGeneralIds, IReadOnlyList<string> StartingNodeIds,
    IReadOnlyList<ShanheNodeDefinition> Nodes, IReadOnlyList<ShanheRewardDefinition> Rewards,
    int StartingGold = 0, int ExtraSkillSlots = 2, ShanheTacticBonuses? StartingBonuses = null)
{
    /// <summary>A source tier whose actual rules are implemented by this recipe. Null denotes a local recipe.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? SourceDifficultyId { get; init; }
    /// <summary>Immutable source data used by this execution contract; its canonical hash is serialized below.</summary>
    [JsonIgnore]
    public ShanheSourceEnemyGrowthCalculator? SourceEnemyGrowth { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceEnemyGrowthHash { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceGrowthPolicy { get; init; }
    public string Description { get; init; } = "";
    public string IntroText { get; init; } = "";
    public string VictoryText { get; init; } = "";
    public string DefeatText { get; init; } = "";
    public bool RandomStartingNode { get; init; }
    /// <summary>Retains unlocked, unvisited locations until the expedition advances to the next chapter.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool AllowChapterDetours { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ShanheShopRarityPolicy? ShopRarityPolicy { get; init; }
    /// <summary>Applies the initial-resource contract to every battle in this expedition; false preserves legacy recipes.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool StartingResourcesExpireOnUse { get; init; }
    /// <summary>Optional edition-specific default; explicit effective factions retain precedence.</summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultGodFactionId { get; init; }
}
