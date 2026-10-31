#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCharacterTurnedOverProgramsEntries
{
    internal bool CharacterTurnedOverDamageObserverRidesOn(int index) => _resolve().CharacterTurnedOverDamageObserverRidesOn(index);
    internal void RecordCharacterTurnedOver(int targetSeat, bool wasFaceDown) => _resolve().RecordCharacterTurnedOver(targetSeat, wasFaceDown);
}
