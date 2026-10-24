namespace CardGame.Core;

/// <summary>Selects independent setup and profile implementations without replacing unrelated match features.</summary>
internal static class ModeFeatureComposition
{
    private static IGameModeSetupFeatureFactory[] SetupFactories(GameOptions options, ContentRegistry registry)
    {
        var factories = registry.ModeFeatureFactories.OfType<IGameModeSetupFeatureFactory>().ToArray();
        var exact = options.ModeId is { } modeId ? factories.Where(factory => factory.ModeId == modeId).ToArray() : [];
        return exact.Length != 0 ? exact : factories.Where(factory => factory.ModeId == ModeFeatureTargets.AllModes).ToArray();
    }

    internal static void ValidateInitialOptions(GameOptions options, ContentRegistry registry, ModeConfigurationValidationPhase phase)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(registry);
        if (!Enum.IsDefined(phase)) throw new ArgumentOutOfRangeException(nameof(phase));
        foreach (var factory in SetupFactories(options, registry)) factory.ValidateInitialOptions(options, phase);
    }

    internal static string ResolveDefaultMode(GameOptions options, ContentRegistry registry)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(registry);
        var resolved = SetupFactories(options with { ModeId = null }, registry)
            .Select(factory => factory.ResolveDefaultModeId(options, registry)).OfType<string>().ToArray();
        return resolved.Length == 1 ? resolved[0] : throw new InvalidOperationException(
            resolved.Length == 0 ? "No installed setup factory supplies a default mode." :
                "Multiple setup factories supplied a default mode.");
    }

    internal static void ValidateSetupTargets(IEnumerable<IGameModeFeatureFactory> factories)
    {
        var all = factories.ToArray();
        if (all.Any(factory => factory is IGameModeSetupFeatureFactory && factory is IGameModeProfileFeatureFactory))
            throw new InvalidOperationException("A mode factory cannot target both the setup and profile slots.");
        var conflict = all.OfType<IGameModeSetupFeatureFactory>()
            .GroupBy(factory => factory.ModeId, StringComparer.Ordinal).FirstOrDefault(group => group.Count() > 1);
        if (conflict is not null)
            throw new InvalidOperationException($"Multiple setup feature factories target mode '{conflict.Key}'.");
        var profileConflict = all.OfType<IGameModeProfileFeatureFactory>()
            .GroupBy(factory => factory.ModeId, StringComparer.Ordinal).FirstOrDefault(group => group.Count() > 1);
        if (profileConflict is not null)
            throw new InvalidOperationException($"Multiple profile feature factories target mode '{profileConflict.Key}'.");
    }

    internal static List<object> Create(ModeFeatureCreationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var modeId = context.ModeDefinition?.Id ??
            throw new InvalidOperationException("Mode feature composition requires the selected match mode.");
        var applicable = context.ContentRegistry.ModeFeatureFactories.Where(factory =>
            factory.ModeId == modeId || factory.ModeId == ModeFeatureTargets.AllModes).ToArray();
        var hasExactSetup = applicable.Any(factory => factory is IGameModeSetupFeatureFactory && factory.ModeId == modeId);
        var hasExactProfile = applicable.Any(factory => factory is IGameModeProfileFeatureFactory && factory.ModeId == modeId);
        var features = new List<object>();
        foreach (var factory in applicable)
        {
            if (hasExactSetup && factory is IGameModeSetupFeatureFactory && factory.ModeId == ModeFeatureTargets.AllModes)
                continue;
            if (hasExactProfile && factory is IGameModeProfileFeatureFactory && factory.ModeId == ModeFeatureTargets.AllModes)
                continue;
            var feature = factory.Create(context);
            if (factory is IGameModeSetupFeatureFactory && feature is not IMatchModeSetupFeature)
                throw new InvalidOperationException($"Setup feature factory '{factory.Id}' did not create an IMatchModeSetupFeature.");
            if (factory is IGameModeProfileFeatureFactory && feature is not IMatchModeRuleProvider)
                throw new InvalidOperationException($"Profile feature factory '{factory.Id}' did not create an IMatchModeRuleProvider.");
            AddFeature(feature, features);
        }
        return features;
    }

    private static void AddFeature(IGameModeFeature feature, List<object> features)
    {
        if (feature is ModeFeatureBundle bundle)
            foreach (var child in bundle.Features) AddFeature(child, features);
        else features.Add(feature);
    }
}
