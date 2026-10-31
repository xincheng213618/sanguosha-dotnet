#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineScenarioGodFactionsRuntime
{
    internal string? GetScenarioGodFaction(CharacterState player) => _coreModeRulesOperations.Value.ModeFactionPolicy.ScenarioGodFaction(player.General.FactionId);
}
