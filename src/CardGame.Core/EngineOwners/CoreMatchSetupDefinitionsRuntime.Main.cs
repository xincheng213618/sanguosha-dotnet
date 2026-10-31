#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchSetupDefinitionsRuntime
{
    internal static void ValidateOptions(GameOptions options, ContentRegistry registry) => RuntimeSkills.RuntimeRules_Core_CommonRules.ValidateOptions(options, () => ModeFeatureComposition.ValidateInitialOptions(options, registry, ModeConfigurationValidationPhase.Adapter));
    internal static ContentModeDefinition ResolveModeDefinition(ContentRegistry contentRegistry, GameOptions options)
    {
        ModeFeatureComposition.ValidateInitialOptions(options, contentRegistry, ModeConfigurationValidationPhase.BeforeModeResolution);
        var modeId = options.ModeId ?? ModeFeatureComposition.ResolveDefaultMode(options, contentRegistry);
        if (!contentRegistry.Modes.TryGetValue(modeId, out var registeredMode))
        {
            throw new InvalidOperationException($"The supplied content registry does not contain mode '{(modeId)}'.");
        }

        if (options.PlayerCount < registeredMode.MinPlayers || options.PlayerCount > registeredMode.MaxPlayers)
        {
            throw new InvalidOperationException($"Mode '{(modeId)}' does not support {(options.PlayerCount)} players.");
        }

        return registeredMode;
    }

    internal IReadOnlyList<string> GetModeTeamIds() => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.TeamCounts is { } teamCounts ? teamCounts.Keys.OrderBy(teamId => teamId, StringComparer.Ordinal).ToArray() : [];
    internal IReadOnlyList<string> GetModeFactionIds() => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.FactionCounts is { } factionCounts ? factionCounts.Keys.OrderBy(factionId => factionId, StringComparer.Ordinal).ToArray() : [];
    internal string? GetEffectiveFactionId(CharacterState player) => _coreModeRulesOperations.Value.ModeFactionPolicy.EffectiveFaction(new(player.TeamId, player.NationalFactionId, player.NativeEffectiveFactionOverrideId, _corePrivateGeneralLibrariesOperations.GetPrivateGeneralLibraryFaction(player), player.ChosenFactionId, player.General.FactionId));
    internal bool RequiresGodFactionSelection(CharacterState player) => _coreNativeDamageHandlesOperations.Value.SupportsGodFactionSelection && player.GeneralSelected && string.Equals(player.General.FactionId, "god", StringComparison.Ordinal) && player.ChosenFactionId is null;
    internal string ChooseAutomaticGodFaction(CharacterState player) => CoreMainDefinitions.GodFactionChoices[(int)(((uint)_coreMatchDefinitionModeDefinitionOptionsAccess._options.Seed + (uint)player.Seat) % (uint)CoreMainDefinitions.GodFactionChoices.Count)];
}
