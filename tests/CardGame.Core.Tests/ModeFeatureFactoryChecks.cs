using CardGame.Core;

internal static class ModeFeatureFactoryChecks
{
    private const string Mode = "test:factory-mode";

    public static void ExplicitFactoriesFreezeMetadataAndPreserveContentHash()
    {
        var factory = new FixtureFactory("test:mode-runtime", Mode);
        var registry = ContentRegistry.Build(new FixturePackage([factory]));
        var withoutRuntime = ContentRegistry.Build(new FixturePackage([]));
        Require(factory.CreateCalls == 0 && registry.ContentHash == withoutRuntime.ContentHash,
            "Registering an executable factory neither constructs match state nor changes the existing content hash.");
        factory.Id = "test:mutated-runtime";
        factory.ModeId = "test:unregistered-mode";
        Require(registry.ModeFeatureFactories.Single() is { Id: "test:mode-runtime", ModeId: Mode } &&
            registry.ModeFeatureFactories is IList<IGameModeFeatureFactory> { IsReadOnly: true },
            "A frozen registry retains its registered mode identity even if a caller mutates its factory metadata.");
        var host = new FixtureHost();
        var context = new ModeFeatureCreationContext(17, registry, contract => contract == typeof(IFixtureHost) ? host : null);
        var first = (FixtureFeature)registry.ModeFeatureFactories.Single().Create(context);
        var second = (FixtureFeature)registry.ModeFeatureFactories.Single().Create(context);
        Require(factory.CreateCalls == 2 && !ReferenceEquals(first, second) &&
            first.Seed == 17 && ReferenceEquals(first.Host, host) && ReferenceEquals(second.Host, host),
            "An explicit installed factory constructs fresh feature state from the supplied seed and dedicated native host.");

        var a = new FixtureFactory("test:a-runtime", Mode);
        var z = new FixtureFactory("test:z-runtime", Mode);
        var forward = ContentRegistry.Build(new FixturePackage([a, z]));
        var reversed = ContentRegistry.Build(new FixturePackage([z, a]));
        Require(forward.ModeFeatureFactories.Select(item => item.Id).SequenceEqual(reversed.ModeFeatureFactories.Select(item => item.Id)) &&
            forward.ContentHash == reversed.ContentHash && forward.ContentHash == withoutRuntime.ContentHash,
            "Runtime factory composition is sorted independently of registration order while content recovery identity stays unchanged.");
    }

    public static void FactoriesRejectDuplicateUnknownAndUnsupportedHosts()
    {
        Reject<InvalidOperationException>(() => ContentRegistry.Build(new FixturePackage(
            [new FixtureFactory("test:same-runtime", Mode), new FixtureFactory("test:same-runtime", Mode)])),
            "Duplicate executable factory ids must fail content registration.");
        Reject<InvalidOperationException>(() => ContentRegistry.Build(new FixturePackage(
            [new FixtureFactory("test:missing-mode-runtime", "test:missing-mode")])),
            "A runtime factory cannot reference an unregistered mode.");
        var registry = ContentRegistry.Build(new FixturePackage([]));
        var context = new ModeFeatureCreationContext(1, registry, _ => null);
        Reject<NotSupportedException>(() => context.GetHost<IFixtureHost>(),
            "A missing dedicated host fails explicitly instead of discovering or loading another assembly.");
        Reject<ArgumentException>(() => context.GetHost<FixtureHost>(),
            "A mode factory requests narrow host interfaces rather than arbitrary engine or implementation classes.");
        var nullFactory = new FixtureFactory("test:null-runtime", Mode) { ReturnNull = true };
        var invalid = ContentRegistry.Build(new FixturePackage([nullFactory]));
        Reject<InvalidOperationException>(() => invalid.ModeFeatureFactories.Single().Create(context),
            "A factory cannot install a null feature into match composition.");
    }

    private interface IFixtureHost { }
    private sealed class FixtureHost : IFixtureHost { }
    private sealed record FixtureFeature(int Seed, IFixtureHost Host) : IGameModeFeature;
    private sealed class FixtureFactory(string id, string modeId) : IGameModeFeatureFactory
    {
        public string Id { get; set; } = id;
        public string ModeId { get; set; } = modeId;
        internal int CreateCalls { get; private set; }
        internal bool ReturnNull { get; init; }
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        {
            CreateCalls++;
            return ReturnNull ? null! : new FixtureFeature(context.Seed, context.GetHost<IFixtureHost>());
        }
    }
    private sealed class FixturePackage(IGameModeFeatureFactory[] factories) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:mode-factories", new Version(1, 0, 0), []);
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddMode(new(Mode, "工厂装配测试", 4, 4, new Dictionary<string, int>(),
                ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { ["team:a"] = 2, ["team:b"] = 2 }));
            foreach (var factory in factories) builder.AddModeFeatureFactory(factory);
        }
    }
    private static void Reject<T>(Action action, string message) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException(message);
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
