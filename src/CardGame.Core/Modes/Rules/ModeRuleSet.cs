namespace CardGame.Core;

/// <summary>Immutable rules selected once for a match. Strategies never retain its engine or live players.</summary>
internal sealed class ModeRuleSet
{
    private ModeRuleSet(IModeOpeningRule opening, IModeDeathRule death, ModeCardRules cards,
        bool usesGeneralBaseHp, bool usesClassicBattleRules, bool supportsGodFactionSelection,
        bool reducesFirstActorInitialHand)
    {
        Opening = opening;
        Death = death;
        Cards = cards;
        UsesGeneralBaseHp = usesGeneralBaseHp;
        UsesClassicBattleRules = usesClassicBattleRules;
        SupportsGodFactionSelection = supportsGodFactionSelection;
        ReducesFirstActorInitialHand = reducesFirstActorInitialHand;
    }

    internal IModeOpeningRule Opening { get; }
    internal IModeDeathRule Death { get; }
    internal ModeCardRules Cards { get; }
    internal bool UsesGeneralBaseHp { get; }
    internal bool UsesClassicBattleRules { get; }
    internal bool SupportsGodFactionSelection { get; }
    internal bool ReducesFirstActorInitialHand { get; }
    internal int? FixedDistance { get; private init; }
    internal int? InitialHandSizeOverride { get; private init; }

    internal static ModeRuleSet Create(ContentModeDefinition mode, GameOptions options)
    {
        ArgumentNullException.ThrowIfNull(mode);
        ArgumentNullException.ThrowIfNull(options);
        var teamIds = mode.TeamCounts?.Keys.OrderBy(id => id, StringComparer.Ordinal).ToArray() ?? [];
        var classicIdentity = mode.Id.StartsWith("identity:classic-", StringComparison.Ordinal);
        var wushuang = mode.Id == WushuangMode.ModeId;
        var survival = mode.ModeKind == ContentModeKind.Survival;
        var dragonBoat = mode.DragonBoatActivity is not null;
        var guandu = mode.GuanduActivity is not null;
        return new ModeRuleSet(SelectOpening(mode, options, teamIds), SelectDeath(mode, options, teamIds), ModeCardRules.Create(mode),
            usesGeneralBaseHp: classicIdentity || wushuang || survival || dragonBoat || guandu,
            usesClassicBattleRules: classicIdentity || wushuang || options.ShanheBattle is not null ||
                mode.TeamActivity is not null || mode.BossActivity is not null || mode.ShenwuActivity is not null ||
                mode.WenheActivity is not null || dragonBoat || guandu,
            supportsGodFactionSelection: mode.ModeKind == ContentModeKind.Identity,
            reducesFirstActorInitialHand: mode.ModeKind == ContentModeKind.Team && options.ShanheBattle is null &&
                !dragonBoat && !guandu && mode.TeamActivity is null && mode.BossActivity is null && mode.ShenwuActivity is null)
        {
            FixedDistance = wushuang ? 1 : null,
            InitialHandSizeOverride = wushuang ? 5 : null
        };
    }

    private static IModeOpeningRule SelectOpening(ContentModeDefinition mode, GameOptions options,
        IReadOnlyList<string> teamIds)
    {
        // Preserve precedence, including the campaign opening before the public-team defaults.
        if (options.ShanheBattle is not null) return new FixedSeatOpeningRule(0);
        if (mode.Id == WushuangMode.ModeId) return new RandomSeatOpeningRule(options.PlayerCount);
        if (mode.GuanduActivity is not null) return new GuanduOpeningRule();
        if (mode.ModeKind == ContentModeKind.Survival || mode.DragonBoatActivity is not null)
            return new RandomSeatOpeningRule(options.PlayerCount);
        if (mode.BossActivity is not null || mode.ShenwuActivity is not null)
            return new FixedSeatOpeningRule(Math.Max(0, options.HumanSeat));
        if (mode.ModeKind == ContentModeKind.NationalWarLite) return new FixedSeatOpeningRule(0);
        return mode.ModeKind == ContentModeKind.Team
            ? new PublicTeamOpeningRule(mode.Id, teamIds.FirstOrDefault())
            : new IdentityOpeningRule();
    }

    private static IModeDeathRule SelectDeath(ContentModeDefinition mode, GameOptions options,
        IReadOnlyList<string> teamIds)
    {
        if (mode.ModeKind == ContentModeKind.Survival) return new SurvivalDeathRule();
        if (mode.DragonBoatActivity is not null || mode.GuanduActivity is not null) return new DeferredDeathRule();
        if (mode.ModeKind == ContentModeKind.Team)
        {
            if (mode.ShenwuActivity is { } shenwu)
                return new ShenwuDeathRule(options.PlayerCount, options.HumanSeat,
                    shenwu.PlayerSeatOffsets, shenwu.EnemyTeamId);
            if (mode.BossActivity is { } boss) return new BossEncounterDeathRule(boss.PlayerTeamId, boss.EnemyTeamId);
            if (mode.TeamActivity is not null)
            {
                var playerTeam = options.HumanTeamId ?? teamIds.First();
                return new JiangeDeathRule(options.PlayerCount, options.HumanSeat, playerTeam,
                    teamIds.Single(team => team != playerTeam));
            }
            return new PublicTeamDeathRule();
        }
        return mode.ModeKind == ContentModeKind.NationalWarLite ? new NationalWarDeathRule() : new IdentityDeathRule();
    }
}

/// <summary>Trusted life facts only; no mutable character, hidden cards or runtime store crosses this boundary.</summary>
internal readonly record struct ModePlayerLife(int Seat, Role Role, bool IsAlive,
    string? TeamId, string? NationalFactionId);
