#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRandomCardAcquisitionRuntimePortsRuntime
{
    IReadOnlyList<CharacterState> IProgramRandomCardAcquisitionFacts.Players => _coreMatchParticipantsPlayersAccess._players;

    int IProgramRandomCardAcquisitionFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    void IProgramRandomCardAcquisitionNativeEntries.MoveToProcessingWithOwnedTransferReceipt(Card card, CardLocation source, CardMoveReason reason, int recipientSeat, long frameId, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.Processing, reason, record => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(record, recipientSeat, frameId), disclosure: disclosure);
    bool IProgramRandomCardAcquisitionNativeEntries.MoveProcessingUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason) => _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(card, destination, reason);
    void IProgramRandomCardAcquisitionNativeEntries.AddLog(string category, string message, int ownerSeat) => _coreMatchLogOperations.AddLog(category, message, ownerSeat);
}
