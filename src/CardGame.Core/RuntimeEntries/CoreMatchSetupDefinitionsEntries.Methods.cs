#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchSetupDefinitionsEntries
{
    internal string ChooseAutomaticGodFaction(CharacterState player) => _resolve().ChooseAutomaticGodFaction(player);
    internal string? GetEffectiveFactionId(CharacterState player) => _resolve().GetEffectiveFactionId(player);
    internal IReadOnlyList<string> GetModeFactionIds() => _resolve().GetModeFactionIds();
    internal IReadOnlyList<string> GetModeTeamIds() => _resolve().GetModeTeamIds();
    internal bool RequiresGodFactionSelection(CharacterState player) => _resolve().RequiresGodFactionSelection(player);
}
