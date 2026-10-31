using CardGame.Core;
using CardGame.Modes.Standard;

internal static class ModeSetupPlanChecks
{
    private const string ModeId = "identity:classic-setup-contract-4";

    public static void FrozenPrivateDraftsUseSharedDeterministicRandom()
    {
        var registry = ContentRegistry.Build(new Fixture());
        var mode = registry.Modes[ModeId];
        var random = new TrackedRandom(19);
        var feature = Feature(registry, mode, random);
        var plan = feature.BuildPlayers();
        Require(plan.InitialActorSeat is null && plan.Seats.Count == 4 && plan.Seats.Single(p => p.Seat == 0) is
            { IsHuman: true, Role: Role.Lord, GeneralSelected: false } && random.Shuffles == 2,
            "Identity setup uses the shared host for role and available-roster shuffles, retaining a private human Lord.");
        Require(plan.Seats is IList<ModeSeatSetup> { IsReadOnly: true } &&
            plan.AvailableGeneralIds is IList<string> { IsReadOnly: true } &&
            plan.SelectionOrder is IList<int> { IsReadOnly: true },
            "The external setup boundary freezes every exposed seat, roster and selection-order collection.");
        var seats = plan.Seats.Select(Facts).ToArray();
        var owner = seats.Single(seat => seat.Seat == 0);
        var first = feature.GetGeneralOffers(new(owner, plan.AvailableGeneralIds, seats));
        Require(first.FrozenCandidateIds is { Count: 2 } && first.Offers.Count == 4 &&
            first.Offers is IList<ModeGeneralOffer> { IsReadOnly: true } &&
            first.FrozenCandidateIds is IList<string> { IsReadOnly: true },
            "A family draft publishes two person tickets and their editions, returning its new private ownership receipt.");
        var draws = random.Draws;
        var resumed = feature.GetGeneralOffers(new(owner, plan.AvailableGeneralIds, seats, first.FrozenCandidateIds));
        var full = feature.GetGeneralOffers(new(owner, plan.AvailableGeneralIds, seats, first.FrozenCandidateIds, fullRoster: true));
        Require(random.Draws == draws && resumed.FrozenCandidateIds is null && resumed.Offers.SequenceEqual(first.Offers) &&
            full.FrozenCandidateIds is null && full.Offers.Count == 8,
            "The original seat's retained draft and a full-roster query consume no new randomness or private candidate receipt.");
        Require(plan.AvailableGeneralIds.Count == 8 && registry.Generals.Count == 10 &&
            first.Offers.All(offer => mode.GeneralSelectionPoolIds!.Contains(offer.GeneralId)),
            "Opening eligibility stays separate from the complete general library used by later transformations.");
    }

    private static IMatchModeSetupFeature Feature(ContentRegistry registry, ContentModeDefinition mode, TrackedRandom random) =>
        (IMatchModeSetupFeature)new StandardModeSetupFactory().Create(new ModeFeatureCreationContext(new GameOptions
        { Seed = 19, PlayerCount = 4, HumanSeat = 0, HumanRole = Role.Lord, UseInteractiveSetup = true }, mode, registry,
            new ModeHostRegistry().Register<IModeSetupRandom>(() => random)));
    private static ModeSeatSetupFacts Facts(ModeSeatSetup seat) => new(seat.Seat, seat.IsHuman, seat.Role,
        seat.TeamId, seat.NationalFactionId, seat.GeneralId, seat.GeneralSelected);
    private sealed class TrackedRandom(int seed) : IModeSetupRandom
    {
        private readonly DeterministicRandom _random = new(seed);
        internal int Draws { get; private set; }
        internal int Shuffles { get; private set; }
        public int Next(int exclusiveMax) { Draws++; return _random.Next(exclusiveMax); }
        public void Shuffle<T>(IList<T> values) { Shuffles++; _random.Shuffle(values); }
    }
    private sealed class Fixture : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:mode-setup-contract", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddCard(new("fixture:setup-slash", "杀", "基本牌", "", CardKind.Slash));
            builder.AddDeck(new("fixture:setup-deck", "Setup contract deck", 4, 2, [new("fixture:setup-slash", 24)]));
            var ids = Enumerable.Range(0, 10).Select(index => $"fixture:setup-general-{index}").ToArray();
            foreach (var index in Enumerable.Range(0, ids.Length)) builder.AddGeneral(new(ids[index], ids[index], "", null, "wei",
                index == 0 ? 3 : 4) { CharacterId = index == 0 ? "character:guan-yu" : null, InitialHp = index == 0 ? 2 : null });
            builder.AddMode(new(ModeId, "Setup contract identity", 4, 4,
                new Dictionary<string, int> { [nameof(Role.Lord)] = 1, [nameof(Role.Rebel)] = 2, [nameof(Role.Renegade)] = 1 },
                "fixture:setup-deck", 2, ids)
            {
                GeneralSelectionPoolIds = ids.Take(8).ToArray(),
                GeneralVariantGroups = Enumerable.Range(0, 5).ToDictionary(index => $"fixture:person-{index}",
                    index => (IReadOnlyList<string>)ids.Skip(index * 2).Take(2).ToArray(), StringComparer.Ordinal)
            });
        }
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
