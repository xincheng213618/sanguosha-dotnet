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
    private class FrozenModeFeatureFactory : IGameModeFeatureFactory
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

    private sealed class FrozenModeSetupFeatureFactory(IGameModeSetupFeatureFactory source)
        : FrozenModeFeatureFactory(source), IGameModeSetupFeatureFactory
    {
        private readonly Action<GameOptions, ModeConfigurationValidationPhase> _validateInitial = source.ValidateInitialOptions;
        private readonly Func<GameOptions, ContentRegistry, string?> _resolveDefault = source.ResolveDefaultModeId;
        public void ValidateInitialOptions(GameOptions options, ModeConfigurationValidationPhase phase) => _validateInitial(options, phase);
        public string? ResolveDefaultModeId(GameOptions options, ContentRegistry registry) => _resolveDefault(options, registry);
    }

    private sealed class FrozenModeProfileFeatureFactory(IGameModeProfileFeatureFactory source)
        : FrozenModeFeatureFactory(source), IGameModeProfileFeatureFactory { }

    private sealed partial class RegistryBuilder
    {
        private readonly Dictionary<string, IGameModeFeatureFactory> _modeFeatureFactories = new(StringComparer.Ordinal);

        public bool HasModeFeatureFactory(string id) => _modeFeatureFactories.ContainsKey(id);

        public void AddModeFeatureFactory(IGameModeFeatureFactory factory)
        {
            ArgumentNullException.ThrowIfNull(factory);
            if (factory is IGameModeSetupFeatureFactory && factory is IGameModeProfileFeatureFactory)
                throw new InvalidOperationException("A mode factory cannot target both the setup and profile slots.");
            FrozenModeFeatureFactory frozen = factory switch
            {
                IGameModeSetupFeatureFactory setup => new FrozenModeSetupFeatureFactory(setup),
                IGameModeProfileFeatureFactory profile => new FrozenModeProfileFeatureFactory(profile),
                _ => new FrozenModeFeatureFactory(factory)
            };
            Add(_modeFeatureFactories, frozen, frozen.Id, "mode feature factory");
        }

        private void ValidateModeFeatureReferences()
        {
            ModeFeatureComposition.ValidateSetupTargets(_modeFeatureFactories.Values);
            foreach (var factory in _modeFeatureFactories.Values)
                if (factory.ModeId != ModeFeatureTargets.AllModes && !_modes.ContainsKey(factory.ModeId))
                    throw new InvalidOperationException($"Mode feature factory '{factory.Id}' references unknown mode '{factory.ModeId}'.");
        }
    }
}
