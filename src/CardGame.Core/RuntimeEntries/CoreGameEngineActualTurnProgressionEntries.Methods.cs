#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualTurnProgressionEntries
{
    internal void AdvanceActualTurnProgression(CharacterState previous) => _resolve().AdvanceActualTurnProgression(previous);
}
