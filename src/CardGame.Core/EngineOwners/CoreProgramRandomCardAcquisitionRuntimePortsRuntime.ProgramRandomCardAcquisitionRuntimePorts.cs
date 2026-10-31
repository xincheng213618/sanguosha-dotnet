#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramRandomCardAcquisitionRuntimePortsRuntime
{
    IReadOnlyList<CharacterState> global::CardGame.Core.IProgramRandomCardAcquisitionFacts.Players => _coreMatchParticipantsPlayersAccess._players;

    int global::CardGame.Core.IProgramRandomCardAcquisitionFacts.RelativeTableOrder(int from, int target) => _coreTableOrderOperations.RelativeTableOrder(from, target);
    void global::CardGame.Core.IProgramRandomCardAcquisitionNativeEntries.MoveToProcessingWithOwnedTransferReceipt(Card card, CardLocation source, CardMoveReason reason, int recipientSeat, long frameId, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, source, CardLocation.Processing, reason, record => _characterBianFuRenOlOperations.RecordBianFuRenOwnedTransferProducer(record, recipientSeat, frameId), disclosure: disclosure);
    bool global::CardGame.Core.IProgramRandomCardAcquisitionNativeEntries.MoveProcessingUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason) => _coreAdvancedWeaponsOperations.MoveProcessingCardUnlessDestroyed(card, destination, reason);
    void global::CardGame.Core.IProgramRandomCardAcquisitionNativeEntries.AddLog(string category, string message, int ownerSeat) => _coreMatchLogOperations.AddLog(category, message, ownerSeat);
}
