#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class MovementRecordObservers(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterJuShouMouEntries> getCharacterJuShouMouRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries> getCharacterBianFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries> getCharacterDuFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries> getCoreGameEngineOriginalHandEntitiesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries> getCoreGameEngineSourceCursesRuntime) : IMovementRecordObservers
{
    public void ObserveMouJsMovement(CardMovementRecord movement) => getCharacterJuShouMouRuntime().ObserveMouJsMovement(movement);
    public void CaptureBianFuRenForeignDiscardProducer(CardMovementRecord movement) => getCharacterBianFuRenOlRuntime().CaptureBianFuRenForeignDiscardProducer(movement);
    public void CaptureDuFuRenOwnedOrigin(CardMovementRecord movement) => getCharacterDuFuRenOlRuntime().CaptureDuFuRenOwnedOrigin(movement);
    public void ObserveOriginalHandMovement(CardMovementRecord movement) => getCoreGameEngineOriginalHandEntitiesRuntime().ObserveOriginalHandMovement(movement);
    public void ObserveSourceCurseMovement(CardMovementRecord movement) => getCoreGameEngineSourceCursesRuntime().ObserveSourceCurseMovement(movement);
}
