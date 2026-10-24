namespace CardGame.Core;

/// <summary>Selects an explicitly registered setup implementation without replacing unrelated match features.</summary>
internal static class ModeFeatureComposition
{
    internal static void ValidateSetupTargets(IEnumerable<IGameModeFeatureFactory> factories)
    {
        var conflict = factories.OfType<IGameModeSetupFeatureFactory>()
            .GroupBy(factory => factory.ModeId, StringComparer.Ordinal).FirstOrDefault(group => group.Count() > 1);
        if (conflict is not null)
            throw new InvalidOperationException($"Multiple setup feature factories target mode '{conflict.Key}'.");
    }

    internal static List<object> Create(ModeFeatureCreationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var modeId = context.ModeDefinition?.Id ??
            throw new InvalidOperationException("Mode feature composition requires the selected match mode.");
        var applicable = context.ContentRegistry.ModeFeatureFactories.Where(factory =>
            factory.ModeId == modeId || factory.ModeId == ModeFeatureTargets.AllModes).ToArray();
        var hasExactSetup = applicable.Any(factory => factory is IGameModeSetupFeatureFactory && factory.ModeId == modeId);
        var features = new List<object>();
        foreach (var factory in applicable)
        {
            if (hasExactSetup && factory is IGameModeSetupFeatureFactory && factory.ModeId == ModeFeatureTargets.AllModes)
                continue;
            var feature = factory.Create(context);
            if (factory is IGameModeSetupFeatureFactory && feature is not IMatchModeSetupFeature)
                throw new InvalidOperationException($"Setup feature factory '{factory.Id}' did not create an IMatchModeSetupFeature.");
            features.Add(feature);
        }
        return features;
    }
}
