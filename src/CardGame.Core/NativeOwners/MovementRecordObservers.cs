#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordObservers(Func<RuntimeEntries.CharacterJuShouMouEntries> getCharacterJuShouMouRuntime, Func<RuntimeEntries.CharacterBianFuRenOlEntries> getCharacterBianFuRenOlRuntime, Func<RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, Func<RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries> getCoreGameEngineOriginalHandEntitiesRuntime, Func<RuntimeEntries.CoreGameEngineSourceCursesEntries> getCoreGameEngineSourceCursesRuntime) : IMovementRecordObservers
{
    public void ObserveMouJsMovement(CardMovementRecord movement) => getCharacterJuShouMouRuntime().ObserveMouJsMovement(movement);
    public void CaptureBianFuRenForeignDiscardProducer(CardMovementRecord movement) => getCharacterBianFuRenOlRuntime().CaptureBianFuRenForeignDiscardProducer(movement);
    public void CaptureDuFuRenOwnedOrigin(CardMovementRecord movement) => getCharacterDuFuRenOlRuntime().CaptureDuFuRenOwnedOrigin(movement);
    public void ObserveOriginalHandMovement(CardMovementRecord movement) => getCoreGameEngineOriginalHandEntitiesRuntime().ObserveOriginalHandMovement(movement);
    public void ObserveSourceCurseMovement(CardMovementRecord movement) => getCoreGameEngineSourceCursesRuntime().ObserveSourceCurseMovement(movement);
}
