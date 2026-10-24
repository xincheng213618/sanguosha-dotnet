namespace CardGame.Core;

public sealed partial class ContentRegistry
{
    /// <summary>
    /// Explicit executable bindings, frozen in stable id order. Like the native
    /// rule implementation, these factories are not part of the content hash.
    /// </summary>
    public IReadOnlyList<IGameModeFeatureFactory> ModeFeatureFactories { get; }

    // Snapshot registration identity, so a caller cannot rewrite registry
    // metadata later by changing properties on its factory implementation.
    private sealed class FrozenModeFeatureFactory : IGameModeFeatureFactory
    {
        private readonly Func<ModeFeatureCreationContext, IGameModeFeature> _create;
        internal FrozenModeFeatureFactory(IGameModeFeatureFactory source)
        {
            Id = new ContentId(source.Id).Value;
            ModeId = source.ModeId == ModeFeatureTargets.AllModes ? ModeFeatureTargets.AllModes : new ContentId(source.ModeId).Value;
            _create = source.Create;
        }
        public string Id { get; }
        public string ModeId { get; }
        public IGameModeFeature Create(ModeFeatureCreationContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            return _create(context) ?? throw new InvalidOperationException($"Mode feature factory '{Id}' returned no match feature.");
        }
    }

    private sealed partial class RegistryBuilder
    {
        private readonly Dictionary<string, IGameModeFeatureFactory> _modeFeatureFactories = new(StringComparer.Ordinal);

        public bool HasModeFeatureFactory(string id) => _modeFeatureFactories.ContainsKey(id);

        public void AddModeFeatureFactory(IGameModeFeatureFactory factory)
        {
            ArgumentNullException.ThrowIfNull(factory);
            var frozen = new FrozenModeFeatureFactory(factory);
            Add(_modeFeatureFactories, frozen, frozen.Id, "mode feature factory");
        }

        private void ValidateModeFeatureReferences()
        {
            foreach (var factory in _modeFeatureFactories.Values)
                if (factory.ModeId != ModeFeatureTargets.AllModes && !_modes.ContainsKey(factory.ModeId))
                    throw new InvalidOperationException($"Mode feature factory '{factory.Id}' references unknown mode '{factory.ModeId}'.");
        }
    }
}
