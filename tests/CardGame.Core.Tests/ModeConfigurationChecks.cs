using CardGame.Core;
using CardGame.Core.RuntimeSkills;
using CardGame.Modes.Standard;

internal static class ModeConfigurationChecks
{
    private const string Mode = "test:configuration-three";

    public static void InitialOptionsKeepOriginalOrderAndFactoryOverrides()
    {
        var standard = ContentRegistry.Build(new Package(new StandardModeSetupFactory()));
        var invalid = new GameOptions { PlayerCount = 3, HumanSeat = 99, HumanRole = null };
        RejectParameter(() => Basic(invalid, standard), "PlayerCount");
        RejectParameter(() => Basic(invalid with { AiPolicyVersion = 0 }, standard), "AiPolicyVersion");
        RejectParameter(() => Basic(invalid with { PlayerCount = 4 }, standard), "HumanSeat");
        var invalidRole = new GameOptions { PlayerCount = 4, HumanSeat = -1, HumanRole = null,
            HumanGuanduRole = (GuanduSeatRole)int.MaxValue };
        Basic(invalidRole, standard);
        RejectParameter(() => ModeFeatureComposition.ValidateInitialOptions(invalidRole, standard,
            ModeConfigurationValidationPhase.BeforeModeResolution), "HumanGuanduRole");
        Require(ModeFeatureComposition.ResolveDefaultMode(new GameOptions { PlayerCount = 4 }, standard) == "team:standard-2v2" &&
            ModeFeatureComposition.ResolveDefaultMode(new GameOptions { PlayerCount = 6 }, standard) == "identity:standard-6",
            "The frozen standard setup factory retains the original default-table selection outside Core.");

        var exact = new CustomSetupFactory(Mode);
        var custom = ContentRegistry.Build(new Package(new StandardModeSetupFactory(), exact));
        var options = new GameOptions { PlayerCount = 3, HumanSeat = 0, HumanRole = null, ModeId = Mode };
        Basic(options, custom);
        ModeFeatureComposition.ValidateInitialOptions(options, custom, ModeConfigurationValidationPhase.BeforeModeResolution);
        Require(exact.Phases.SequenceEqual([ModeConfigurationValidationPhase.Adapter, ModeConfigurationValidationPhase.BeforeModeResolution]) &&
            exact.Creates == 0 && custom.ModeFeatureFactories.OfType<IGameModeSetupFeatureFactory>().Count() == 2,
            "An exact setup's frozen initial validators replace the wildcard's adapter limits before setup instances or random state exist.");
        var defaultFactory = new CustomSetupFactory(ModeFeatureTargets.AllModes);
        var ownDefault = ContentRegistry.Build(new Package(defaultFactory));
        Require(ModeFeatureComposition.ResolveDefaultMode(options with { ModeId = null }, ownDefault) == Mode &&
            defaultFactory.Defaults == 1 && defaultFactory.Creates == 0,
            "Registry snapshotting preserves a package's default resolver without constructing its live feature.");
        Reject(() => ModeFeatureComposition.ResolveDefaultMode(options, ContentRegistry.Build()));

        var team = new ContentModeDefinition("test:configuration-team", "Team", 4, 4, new Dictionary<string, int>(),
            ModeKind: ContentModeKind.Team, TeamCounts: new Dictionary<string, int> { ["team:a"] = 2, ["team:b"] = 2 });
        RejectParameter(() => StandardModeConfigurationRules.ValidateOptions(new GameOptions { PlayerCount = 4,
            HumanSeat = 0, HumanRole = Role.Lord }, team, standard), "HumanRole");
        RejectParameter(() => StandardModeConfigurationRules.ValidateOptions(new GameOptions { PlayerCount = 4,
            HumanSeat = 0, HumanRole = null, HumanTeamId = "team:missing" }, team, standard), "HumanTeamId");
        RejectParameter(() => StandardModeConfigurationRules.ValidateOptions(new GameOptions { PlayerCount = 3,
            HumanSeat = 0, HumanRole = null, HumanGuanduRole = GuanduSeatRole.General }, custom.Modes[Mode], custom), "HumanGuanduRole");
        var national = team with { ModeKind = ContentModeKind.NationalWarLite,
            FactionCounts = new Dictionary<string, int> { ["wei"] = 2, ["shu"] = 2 } };
        RejectParameter(() => StandardModeConfigurationRules.ValidateOptions(new GameOptions { PlayerCount = 4,
            HumanSeat = 0, HumanRole = null, HumanTeamId = "team:a" }, national, standard), "HumanTeamId");
    }

    private static void Basic(GameOptions options, ContentRegistry registry) =>
        RuntimeRules_Core_CommonRules.ValidateOptions(options, () => ModeFeatureComposition.ValidateInitialOptions(options,
            registry, ModeConfigurationValidationPhase.Adapter));
    private sealed class Package(params IGameModeFeatureFactory[] factories) : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("test:configuration-package", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            builder.AddMode(new(Mode, "Three", 3, 3,
                new Dictionary<string, int> { ["Lord"] = 1, ["Rebel"] = 2 }));
            foreach (var factory in factories) builder.AddModeFeatureFactory(factory);
        }
    }
    private sealed class CustomSetupFactory(string modeId) : IGameModeSetupFeatureFactory
    {
        public string Id => "test:custom-configuration";
        public string ModeId { get; } = modeId;
        internal List<ModeConfigurationValidationPhase> Phases { get; } = [];
        internal int Defaults;
        internal int Creates;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase) => Phases.Add(phase);
        public string? ResolveDefaultModeId(GameOptions options, ContentRegistry registry) { Defaults++; return Mode; }
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        { Creates++; throw new InvalidOperationException("Initial validation cannot construct setup state."); }
    }
    private static void RejectParameter(Action action, string parameter)
    {
        try { action(); }
        catch (ArgumentException failure) when (failure.ParamName == parameter) { return; }
        throw new InvalidOperationException("Mode validation changed its original exception parameter or ordering: " + parameter);
    }
    private static void Reject(Action action)
    {
        try { action(); }
        catch (InvalidOperationException) { return; }
        throw new InvalidOperationException("Default mode resolution accepted no installed resolver.");
    }
    private static void Require(bool condition, string message)
    { if (!condition) throw new InvalidOperationException(message); }
}
