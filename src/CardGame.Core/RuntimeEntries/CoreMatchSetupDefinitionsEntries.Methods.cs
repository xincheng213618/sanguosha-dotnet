#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreMatchSetupDefinitionsEntries
{
    internal string ChooseAutomaticGodFaction(global::CardGame.Core.CharacterState player) => _resolve().ChooseAutomaticGodFaction(player);
    internal string? GetEffectiveFactionId(global::CardGame.Core.CharacterState player) => _resolve().GetEffectiveFactionId(player);
    internal global::System.Collections.Generic.IReadOnlyList<string> GetModeFactionIds() => _resolve().GetModeFactionIds();
    internal global::System.Collections.Generic.IReadOnlyList<string> GetModeTeamIds() => _resolve().GetModeTeamIds();
    internal bool RequiresGodFactionSelection(global::CardGame.Core.CharacterState player) => _resolve().RequiresGodFactionSelection(player);
}
