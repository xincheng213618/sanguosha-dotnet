namespace CardGame.Core;

/// <summary>Explicitly composed, frozen registrations; bundles own no runtime state or execution loop.</summary>
public sealed class ModeFeatureBundle : IGameModeFeature
{
    public ModeFeatureBundle(IEnumerable<IGameModeFeature> features)
    {
        ArgumentNullException.ThrowIfNull(features);
        var copy = features.ToArray();
        if (copy.Any(feature => feature is null)) throw new ArgumentException("A mode bundle cannot contain null features.", nameof(features));
        Features = Array.AsReadOnly(copy);
    }
    public IReadOnlyList<IGameModeFeature> Features { get; }
}
