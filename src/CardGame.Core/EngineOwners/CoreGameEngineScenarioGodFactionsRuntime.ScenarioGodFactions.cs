#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineScenarioGodFactionsRuntime
{
    internal string? GetScenarioGodFaction(CharacterState player) => _coreModeRulesOperations.ModeFactionPolicy.ScenarioGodFaction(player.General.FactionId);
}
