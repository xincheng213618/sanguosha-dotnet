using CardGame.Core;

namespace CardGame.Modes.Standard;

/// <summary>Standard mode strategy selection; Core only executes the supplied immutable profile.</summary>
public static class StandardModeRuleProfiles
{
    public static IReadOnlyList<CardKind> WenheOrdinaryTrickKinds { get; } = Array.AsReadOnly(new[]
        { CardKind.StarShift, CardKind.BeamSwap, CardKind.HandRedistribution, CardKind.WenheChaos });
    public static ModeRuleSet Create(ContentModeDefinition mode, GameOptions options)
    {
        ArgumentNullException.ThrowIfNull(mode);
        ArgumentNullException.ThrowIfNull(options);
        var teamIds = mode.TeamCounts?.Keys.OrderBy(id => id, StringComparer.Ordinal).ToArray() ?? [];
        var classicIdentity = mode.Id.StartsWith("identity:classic-", StringComparison.Ordinal);
        var wushuang = mode.Id == WushuangMode.ModeId;
        var survival = mode.ModeKind == ContentModeKind.Survival;
        var dragonBoat = mode.DragonBoatActivity is not null;
        var guandu = mode.GuanduActivity is not null;
        return new ModeRuleSet(SelectOpening(mode, options, teamIds), SelectDeath(mode, options, teamIds), StandardModeCardProfiles.Create(mode),
            usesGeneralBaseHp: classicIdentity || wushuang || survival || dragonBoat || guandu,
            usesClassicBattleRules: classicIdentity || wushuang || options.ShanheBattle is not null ||
                mode.TeamActivity is not null || mode.BossActivity is not null || mode.ShenwuActivity is not null ||
                mode.WenheActivity is not null || dragonBoat || guandu,
            supportsGodFactionSelection: mode.ModeKind == ContentModeKind.Identity,
            reducesFirstActorInitialHand: mode.ModeKind == ContentModeKind.Team && options.ShanheBattle is null &&
                !dragonBoat && !guandu && mode.TeamActivity is null && mode.BossActivity is null && mode.ShenwuActivity is null,
            allowsDiscardRecycling: !guandu,
            publishesCardMovementDisclosure: mode.WenheActivity is not null,
            revealsTeamsOnStart: mode.ModeKind == ContentModeKind.Team,
            visibility: new(
                ShowsPublicTeams: mode.ModeKind == ContentModeKind.Team,
                ConcealsRoleCompletely: mode.ModeKind == ContentModeKind.NationalWarLite,
                UsesHiddenFaction: mode.ModeKind == ContentModeKind.NationalWarLite,
                PublishesActivityFaction: mode.ShenwuActivity is not null || mode.NianActivity is not null || dragonBoat || guandu,
                UsesSecondaryGeneral: mode.ModeKind == ContentModeKind.NationalWarLite || mode.DualGenerals,
                AllowsGeneralReveal: mode.ModeKind == ContentModeKind.NationalWarLite,
                UsesSurvivalRanking: survival,
                PublishesIdentityComposition: mode.ModeKind == ContentModeKind.Identity),
            requiredFeatureContracts: RequiredFeatures(mode, options),
            isClassicIdentityContext: classicIdentity,
            requiresRoundTracking: wushuang || mode.NianActivity is not null || dragonBoat || guandu,
            requiresRecipientDamageReevaluation: wushuang || guandu,
            runtimeRequirements: new(
                TracksActualDiscardRecoveryPhases: guandu,
                TracksActualPhaseEnds: guandu,
                TracksNativeDrawInvocations: wushuang,
                TracksRoundEnding: dragonBoat,
                CapturesRecipientDamageBase: guandu,
                TracksFinalizedCardTargets: guandu), allowsArmorGain: !wushuang)
        {
            FixedDistance = wushuang ? 1 : null,
            InitialHandSizeOverride = wushuang ? 5 : null
        };
    }

    private static IEnumerable<Type> RequiredFeatures(ContentModeDefinition mode, GameOptions options)
    {
        if (mode.Id == WushuangMode.ModeId) yield return typeof(IWushuangModeFeature);
        if (options.ShanheBattle is not null) yield return typeof(IShanheBattleFeature);
        if (mode.TeamActivity is not null) yield return typeof(IJiangeBattleFeature);
        if (mode.BossActivity is not null && mode.HellActivity is null) yield return typeof(IBossEncounterFlowFeature);
        if (mode.HulaoActivity is not null) yield return typeof(IModeFlowInterruptionHandler);
        if (mode.ShenwuActivity is not null) yield return typeof(IShenwuBattleFeature);
        if (mode.HellActivity is not null) yield return typeof(IHellEncounterFeature);
        if (mode.NianActivity is not null) yield return typeof(INianChallengeFeature);
        if (mode.ModeKind == ContentModeKind.Survival) yield return typeof(IMatchModeDeathRanking);
        if (mode.GuanduActivity is not null) yield return typeof(IGuanduBattleRulesFeature);
        if (mode.DragonBoatActivity is not null) yield return typeof(IDragonBoatBattleRulesFeature);
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
