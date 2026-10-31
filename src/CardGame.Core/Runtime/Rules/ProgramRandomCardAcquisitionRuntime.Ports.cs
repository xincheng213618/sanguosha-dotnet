namespace CardGame.Core;

internal interface IProgramRandomCardAcquisitionFacts : IProgramSelectionQuery
{
    IReadOnlyList<CharacterState> Players { get; }
    int RelativeTableOrder(int from, int target);
}

// Native movements retain their existing producer receipt adjacency. No
// selection, random draw, target loop or completion fact is delegated here.
internal interface IProgramRandomCardAcquisitionNativeEntries : IProgramRuntimeOperations
{
    void MoveToProcessingWithOwnedTransferReceipt(Card card, CardLocation source, CardMoveReason reason,
        int recipientSeat, long frameId, CardMovementDisclosure disclosure);
    bool MoveProcessingUnlessDestroyed(Card card, CardLocation destination, CardMoveReason reason);
    void AddLog(string category, string message, int ownerSeat);
}
