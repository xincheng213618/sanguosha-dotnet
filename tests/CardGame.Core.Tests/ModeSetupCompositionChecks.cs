using CardGame.Core;

internal static class ModeSetupCompositionChecks
{
    private const string Mode = "test:setup-composition";

    public static void ExactSetupReplacesOnlyDefaultAndConflictsStayExplicit()
    {
        var fallback = new SetupFactory("test:default-setup", ModeFeatureTargets.AllModes);
        var fallbackRegistry = Registry(fallback);
        Require(fallbackRegistry.ModeFeatureFactories.Single() is IGameModeSetupFeatureFactory && fallback.Creates == 0,
            "Registry snapshots preserve the setup marker without constructing a feature.");
        var fallbackFeatures = ModeFeatureComposition.Create(Context(fallbackRegistry));
        Require(fallback.Creates == 1 && fallbackFeatures.OfType<SetupFeature>().Single().Owner == "test:default-setup",
            "An explicitly installed wildcard setup remains the default when no exact setup was registered.");

        fallback = new("test:default-setup", ModeFeatureTargets.AllModes);
        var exact = new SetupFactory("test:exact-setup", Mode);
        var observer = new ObserverFactory();
        var overridden = Registry(fallback, exact, observer);
        Require(overridden.ModeFeatureFactories.OfType<IGameModeSetupFeatureFactory>().Count() == 2,
            "Both specific and default setup identities survive the actual frozen registry wrapper.");
        var features = ModeFeatureComposition.Create(Context(overridden));
        var runtime = new MatchModeRuntime(features.ToArray());
        runtime.RuleObservers.Single().Callback(null);
        Require(fallback.Creates == 0 && exact.Creates == 1 && observer.Creates == 1 &&
            runtime.Find<IMatchModeSetupFeature>() is SetupFeature { Owner: "test:exact-setup" } &&
            observer.Observed == 1 && features.Count == 2,
            "Exact setup skips construction of the default while an unrelated wildcard observer still composes and executes.");

        var first = new SetupFactory("test:first-specific", Mode);
        var second = new SetupFactory("test:second-specific", Mode);
        Reject(() => Registry(first, second), "Multiple setup feature factories",
            "Two different setup ids for the same exact mode must fail during ContentRegistry.Build.");
        Require(first.Creates == 0 && second.Creates == 0,
            "A conflicting setup target is rejected before either factory receives match state.");
        Reject(() => Registry(new SetupFactory("test:first-default", ModeFeatureTargets.AllModes),
            new SetupFactory("test:second-default", ModeFeatureTargets.AllModes)), "Multiple setup feature factories",
            "Two wildcard defaults remain an explicit conflict rather than depending on registration order.");

        var broken = Registry(new SetupFactory("test:wrong-product", Mode) { WrongProduct = true });
        Require(broken.ModeFeatureFactories.Single() is IGameModeSetupFeatureFactory,
            "Product validation observes the frozen setup marker rather than relying on the caller's original type.");
        Reject(() => ModeFeatureComposition.Create(Context(broken)), "did not create an IMatchModeSetupFeature",
            "An incorrectly labelled setup factory cannot silently install an unrelated feature.");
    }

    private static ContentRegistry Registry(params IGameModeFeatureFactory[] factories) => ContentRegistry.Build(new Package(factories));
    private static ModeFeatureCreationContext Context(ContentRegistry registry) => new(new GameOptions
        { PlayerCount = 4, HumanRole = null, ModeId = Mode }, registry.Modes[Mode], registry, new ModeHostRegistry());
    private sealed class Package(IGameModeFeatureFactory[] factories) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:setup-composition-package", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddMode(new(Mode, "Setup composition", 4, 4, new Dictionary<string, int>(),
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { ["team:a"] = 2, ["team:b"] = 2 }));
            foreach (var factory in factories) builder.AddModeFeatureFactory(factory);
        }
    }
    private sealed class SetupFactory(string id, string modeId) : IGameModeSetupFeatureFactory
    {
        public string Id { get; } = id;
        public string ModeId { get; } = modeId;
        internal int Creates { get; private set; }
        internal bool WrongProduct { get; init; }
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        { Creates++; return WrongProduct ? new OtherFeature() : new SetupFeature(Id); }
    }
    private sealed class OtherFeature : IGameModeFeature { }
    private sealed class SetupFeature(string owner) : IMatchModeSetupFeature
    {
        internal string Owner { get; } = owner;
        public ModeSetupPlan BuildPlayers() => new([], [], []);
        public ModeGeneralOfferPlan GetGeneralOffers(ModeGeneralOfferRequest request) => new([]);
        public IReadOnlyList<Card> PrepareDeck(IReadOnlyList<Card> original) => original;
        public IReadOnlyList<Card> ProjectDeckCardNames(IReadOnlyList<Card> original) => original;
        public ModeSelectedEquipmentPlan? PrepareSelectedEquipment(IReadOnlyList<Card> drawPile, IReadOnlyList<ModeSeatSetupFacts> seats) => null;
        public ModeSelectedGeneralPlan PrepareSelectedGeneral(ModeSeatSetupFacts seat, string generalId) => new([]);
    }
    private sealed class ObserverFactory : IGameModeFeatureFactory
    {
        public string Id => "test:independent-observer";
        public string ModeId => ModeFeatureTargets.AllModes;
        internal int Creates { get; private set; }
        internal int Observed { get; private set; }
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        { Creates++; return new ObserverFeature(() => Observed++); }
    }
    private sealed class ObserverFeature(Action observed) : IGameModeFeature, IMatchModeRuleObserverProvider
    {
        public IReadOnlyList<ModeRuleObserverRegistration> RuleObservers { get; } =
            Array.AsReadOnly<ModeRuleObserverRegistration>([ModeRuleObserverRegistration.BeforeStatePublish(93001, observed)]);
    }
    private static void Reject(Action action, string expected, string message)
    {
        try { action(); }
        catch (InvalidOperationException failure) when (failure.Message.Contains(expected, StringComparison.Ordinal)) { return; }
        throw new InvalidOperationException(message);
    }
    private static void Require(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
}
