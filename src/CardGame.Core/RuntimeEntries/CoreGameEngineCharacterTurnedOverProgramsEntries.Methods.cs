#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterTurnedOverProgramsEntries
{
    internal bool CharacterTurnedOverDamageObserverRidesOn(int index) => _resolve().CharacterTurnedOverDamageObserverRidesOn(index);
    internal bool IsCharacterTurnedOverProgramDying() => _resolve().IsCharacterTurnedOverProgramDying();
    internal void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => _resolve().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
}
