#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramRandomCardAcquisitionNativeEntries(global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries> getCorePhysicalMovementFacadesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries> getCharacterBianFuRenOlRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAdvancedWeaponsEntries> getCoreGameEngineAdvancedWeaponsRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries> getCoreMatchLogRuntime) : IProgramRandomCardAcquisitionNativeEntries
{
    public void MoveToProcessingWithOwnedTransferReceipt(Card card, CardLocation source, CardMoveReason reason, int recipientSeat, long frameId, CardMovementDisclosure disclosure) => getCorePhysicalMovementFacadesRuntime().MoveCard(card, source, CardLocation.Processing, reason, record => getCharacterBianFuRenOlRuntime().RecordBianFuRenOwnedTransferProducer(record, recipientSeat, frameId), disclosure: disclosure);
    public bool MoveProcessingUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason) => getCoreGameEngineAdvancedWeaponsRuntime().MoveProcessingCardUnlessDestroyed(card, destination, reason);
    public void AddLog(string category, string message, int ownerSeat) => getCoreMatchLogRuntime().AddLog(category, message, ownerSeat);
}
