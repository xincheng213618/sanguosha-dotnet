using System.Reflection;
using CardGame.Core;
using CardGame.Core.Campaigns;
using CardGame.Modes.Activities;

internal static class ModeActivityPolicyChecks
{
    internal static void TierEligibilityAndAlliedDamageCapsBelongToInstalledFeatures()
    {
        var registry = ContentRegistry.Build();
        var options = new GameOptions { Seed = 17, PlayerCount = 6, HumanSeat = 4 };
        var hosts = new ModeHostRegistry()
            .Register<IStandardBattleHost>(Cold<IStandardBattleHost>)
            .Register<IModeEncounterHost>(Cold<IModeEncounterHost>)
            .Register<IHellEncounterHost>(Cold<IHellEncounterHost>);
        var baseMode = new ContentModeDefinition("test:activity-policy", "Policy", 6, 6, new Dictionary<string, int>());
        foreach (var (edition, playerTier, enemyTier, playerTrigger, enemyTrigger, replacement) in new[]
        {
            (0, 5, 5, false, false, 0),
            (2020, 1, 4, false, true, 1),
            (2020, 2, 1, true, false, 0),
            (2022, 5, 5, true, true, 0)
        })
        {
            var mode = baseMode with { ShenwuActivity = new([], [], [], [], [0, 1, 2], 3, [4, 5],
                playerTier, [], new Dictionary<string, string>()) { Edition = edition, EnemyTier = enemyTier } };
            var feature = Features(new StandardBattleFeatureFactory(), options, mode, registry, hosts)
                .OfType<IShenwuBattleFeature>().Single();
            Need(new[] { 4, 5, 0 }.All(feature.IsPlayerSlot) && new[] { 1, 2, 3 }.All(seat => !feature.IsPlayerSlot(seat)),
                "The installed Shenwu rule interprets relative slots around the actual nonzero human seat.");
            Need(feature.AllowsTierTrigger(4, "shenwu:player-tier-2020") == playerTrigger &&
                feature.AllowsTierTrigger(1, "shenwu:enemy-tier") == enemyTrigger &&
                !feature.AllowsTierTrigger(1, "shenwu:player-tier-2020") &&
                !feature.AllowsTierTrigger(4, "shenwu:enemy-tier") &&
                feature.ReplacementDrawBonus(1) == replacement && feature.ReplacementDrawBonus(4) == 0,
                "Edition and tier policy must preserve each side's privilege and the 2020-only enemy replacement draw.");
        }
        for (var tier = 1; tier <= 5; tier++)
        {
            var mode = baseMode with { HellActivity = new(tier, "test:aux", "test:hidden"),
                BossActivity = new([], [], 5, [], EnemyTeamId: "test:enemy") };
            var feature = Features(new HellNianFeatureFactory(), options, mode, registry, hosts)
                .OfType<IHellEncounterFeature>().Single();
            Need(feature.IsNpcTeam("test:enemy") && !feature.IsNpcTeam(null) &&
                feature.AllowsTierTrigger("hell:player-tier", "any", "test:allies") &&
                !feature.AllowsTierTrigger("hell:player-tier", "any", "test:enemy") &&
                feature.AllowsTierTrigger("hell:boss-tier", "arrival-equipment", "test:enemy") == (tier >= 2) &&
                feature.AllowsTierTrigger("hell:boss-tier", "turn-slash", "test:enemy") == (tier >= 3) &&
                feature.AllowsTierTrigger("hell:boss-tier", "arrival-assault", "test:enemy") == (tier == 5) &&
                !feature.AllowsTierTrigger("hell:boss-tier", "unknown", "test:enemy"),
                "Hell's configured enemy team and exact tier thresholds belong to its optional feature, without reading a live host.");
        }

        var native = new CapHost();
        var capHosts = new ModeHostRegistry().Register<IStandardBattleHost>(() => native);
        var jiange = baseMode with { TeamActivity = new(new Dictionary<string, IReadOnlyList<string>>(),
            new Dictionary<string, IReadOnlyList<string>>(), new Dictionary<string, IReadOnlyList<string>>(),
            5, 1, 5, 3, new Dictionary<string, IReadOnlyList<string>>()) };
        var cap = Features(new StandardBattleFeatureFactory(), options, jiange, registry, capHosts)
            .OfType<IJiangeBattleFeature>().Single();
        Need(cap.AlliedDamageCap(0) == new ModeDamageCapPlan(1, "jiange:chiying", "allied-damage-cap", 1),
            "The first currently enabled allied owner supplies the mode's cap plan.");
        native.Seats[1] = native.Seats[1] with { IsAlive = false };
        Need(cap.AlliedDamageCap(0)?.OwnerSeat == 2, "A dead earlier owner cannot retain the allied cap.");
        native.Seats[2] = native.Seats[2] with { TeamId = null };
        Need(cap.AlliedDamageCap(0) is null, "An unassigned or opposing team cannot supply that allied cap.");
    }

    private static IReadOnlyList<IGameModeFeature> Features(IGameModeFeatureFactory factory, GameOptions options,
        ContentModeDefinition mode, ContentRegistry registry, ModeHostRegistry hosts) =>
        ((ModeFeatureBundle)factory.Create(new(options, mode, registry, hosts))).Features;
    private static T Cold<T>() where T : class => DispatchProxy.Create<T, ColdModeHostConstructionProbe>();
    private static void Need(bool value, string message) { if (!value) throw new InvalidOperationException(message); }

    private sealed class CapHost : IStandardBattleHost
    {
        internal StandardBattlePlayerFacts[] Seats = Enumerable.Range(0, 4).Select(seat =>
            new StandardBattlePlayerFacts(seat, "test:general", "General", true, true, 4, 4,
                seat == 3 ? "enemy" : "allies")).ToArray();
        public IReadOnlyList<StandardBattlePlayerFacts> Players => Array.AsReadOnly(Seats.ToArray());
        public StandardBattlePlayerFacts Player(int seat) => Seats[seat];
        public bool HasEffectiveSkill(int seat, string skillId) => seat != 0 && Seats[seat].IsAlive && skillId == "jiange:chiying";
        public int InitialHandSize => throw new NotSupportedException();
        public bool Started => throw new NotSupportedException();
        public bool IsWounded(int seat) => throw new NotSupportedException();
        public ShenwuBattleDraftFacts? ShenwuDraft() => throw new NotSupportedException();
        public void GrantSkill(int seat, SkillGrant grant) => throw new NotSupportedException();
        public void PublishShanheStartingResourceOrigins() => throw new NotSupportedException();
        public void PlaceStartingCard(ShanheStartingCard card, int seat, bool equipment) => throw new NotSupportedException();
    }
}
