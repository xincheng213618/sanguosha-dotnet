namespace CardGame.Core;

/// <summary>Copies executable bindings when composing an existing frozen content registry.</summary>
public static class ModeFeatureRegistrationExtensions
{
    public static void CopyModeFeatureFactoriesFrom(this IContentRegistryBuilder builder, ContentRegistry source)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(source);
        foreach (var factory in source.ModeFeatureFactories)
            if (!builder.HasModeFeatureFactory(factory.Id)) builder.AddModeFeatureFactory(factory);
    }
}
