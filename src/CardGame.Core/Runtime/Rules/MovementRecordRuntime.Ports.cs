namespace CardGame.Core;

internal interface IMovementRecordFacts : IProgramSelectionQuery
{
    int Sequence { get; }
    int TurnNumber { get; }
    bool PublishesDisclosure { get; }
    bool Started { get; }
    CardMoveReason SimultaneousTopReservationReason { get; }
    string PrivatePindianReserveReason { get; }
}

// These producers only read original rule facts. Each call remains at its
// previous object-initializer position, before the record enters the journal.
internal interface IMovementRecordMetadata : IProgramSelectionQuery
{
    ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to);
    RedOwnedLossMaterial? CaptureRedOwnedLoss(Card card, CardLocation from, CardLocation to, long sequence);
    HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(Card card, CardLocation from, CardLocation to, long sequence);
    AnniversaryGuoZhaoLoss? CaptureAnniversaryGuoZhaoLoss(Card card, CardLocation from, CardLocation to, int sequence);
    AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to);
}

// The engine's existing counter and list remain their sole storage owners.
internal interface IMovementRecordJournal : IProgramRuntimeOperations
{
    int NextSequence();
    void Append(CardMovementRecord movement);
}

internal interface IMovementRecordMaterials : IProgramRuntimeOperations
{
    void ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement);
    void ValidateHappyZuoCiPhantomMovement(CardMovementRecord movement);
    void RecordDiouCarrierDestruction(CardMovementRecord movement);
    void RecordShenDaCarrierDestruction(CardMovementRecord movement);
    void RecordWushuangShopEquipmentDestruction(CardMovementRecord movement);
    void RecordShanheStartingResourceDestruction(CardMovementRecord movement);
    void RecordWuZglCopyMovement(CardMovementRecord movement);
}

internal interface IMovementRecordObservers : IProgramRuntimeOperations
{
    void ObserveMouJsMovement(CardMovementRecord movement);
    void CaptureBianFuRenForeignDiscardProducer(CardMovementRecord movement);
    void CaptureDuFuRenOwnedOrigin(CardMovementRecord movement);
    void ObserveOriginalHandMovement(CardMovementRecord movement);
    void ObserveSourceCurseMovement(CardMovementRecord movement);
}

internal interface IMovementRecordOrigins : IProgramRuntimeOperations
{
    void RecordLishiDiscardRedirection(CardMovementRecord movement);
    void CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to);
    void CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement);
    void CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord movement, CardKind? frozenIdentity);
    void CollectFullDiscardPhaseSuit(Card card, CardMovementRecord movement);
}

internal interface IMovementRecordNotifications : IProgramRuntimeOperations
{
    void Enqueue(CardMovementRecord movement);
}
