#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMovementRecordsRuntime
{
    int IMovementRecordFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    int IMovementRecordFacts.TurnNumber => _coreActualTurnProgressStartedTurnNumberAccess._turnNumber;

    bool IMovementRecordFacts.PublishesDisclosure => _modeRulesOperations._modeRules.PublishesCardMovementDisclosure;

    bool IMovementRecordFacts.Started => _coreActualTurnProgressStartedTurnNumberAccess._started;

    CardMoveReason IMovementRecordFacts.SimultaneousTopReservationReason => CoreGameEngineSimultaneousPindianSelectionsDefinitions.SimultaneousTopReservationReason;

    string IMovementRecordFacts.PrivatePindianReserveReason => "skill-program.wu-lu-kang.shenduan.reserve";

    ActualPlayPhysicalSlashLossStamp? IMovementRecordMetadata.CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to) => _coreProgramActualPlaySlashLossOperations.CaptureActualPlaySlashLoss(card, from, to);
    RedOwnedLossMaterial? IMovementRecordMetadata.CaptureRedOwnedLoss(Card card, CardLocation from, CardLocation to, long sequence) => _coreGainGiftProgramsOperations.CaptureRedOwnedLoss(card, from, to, sequence);
    HappyMedicineHandLoss? IMovementRecordMetadata.CaptureHappyHuaTuoHandLoss(Card card, CardLocation from, CardLocation to, long sequence) => _characterHuaTuoHappyGodOperations.CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
    AnniversaryGuoZhaoLoss? IMovementRecordMetadata.CaptureAnniversaryGuoZhaoLoss(Card card, CardLocation from, CardLocation to, int sequence) => _characterGuoZhaoAnniversaryOperations.CaptureAnniversaryGuoZhaoLoss(card, from, to, sequence);
    AnniversaryZhangHengMovement? IMovementRecordMetadata.CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to) => _characterZhangHengAnniversaryOperations.CaptureAnniversaryZhangHengMovement(card, from, to);
    int IMovementRecordJournal.NextSequence() => ++_coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
    void IMovementRecordJournal.Append(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Add(movement);
    void IMovementRecordMaterials.ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement) => _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainDestinationRecord(movement);
    void IMovementRecordMaterials.ValidateHappyZuoCiPhantomMovement(CardMovementRecord movement) => _characterZuoCiHappyGodOperations.ValidateHappyZuoCiPhantomMovement(movement);
    void IMovementRecordMaterials.RecordDiouCarrierDestruction(CardMovementRecord movement) => _characterDiaoChanLeOperations.RecordDiouCarrierDestruction(movement);
    void IMovementRecordMaterials.RecordShenDaCarrierDestruction(CardMovementRecord movement) => _characterDengAiAnniversaryGodOperations.RecordShenDaCarrierDestruction(movement);
    void IMovementRecordMaterials.RecordWushuangShopEquipmentDestruction(CardMovementRecord movement) => _coreWushuangShopCardsOperations.RecordWushuangShopEquipmentDestruction(movement);
    void IMovementRecordMaterials.RecordShanheStartingResourceDestruction(CardMovementRecord movement) => _coreShanheStartingResourcesOperations.RecordShanheStartingResourceDestruction(movement);
    void IMovementRecordMaterials.RecordWuZglCopyMovement(CardMovementRecord movement) => _characterZhugeLiangWumiaoOperations.RecordWuZglCopyMovement(movement);
    void IMovementRecordObservers.ObserveMouJsMovement(CardMovementRecord movement) => _characterJuShouMouOperations.ObserveMouJsMovement(movement);
    void IMovementRecordObservers.CaptureBianFuRenForeignDiscardProducer(CardMovementRecord movement) => _characterBianFuRenOlOperations.CaptureBianFuRenForeignDiscardProducer(movement);
    void IMovementRecordObservers.CaptureDuFuRenOwnedOrigin(CardMovementRecord movement) => _characterDuFuRenOlOperations.CaptureDuFuRenOwnedOrigin(movement);
    void IMovementRecordObservers.ObserveOriginalHandMovement(CardMovementRecord movement) => _coreOriginalHandEntitiesOperations.ObserveOriginalHandMovement(movement);
    void IMovementRecordObservers.ObserveSourceCurseMovement(CardMovementRecord movement) => _coreSourceCursesOperations.ObserveSourceCurseMovement(movement);
    void IMovementRecordOrigins.RecordLishiDiscardRedirection(CardMovementRecord movement) => _characterZhongHuiAnniversaryGodOperations.RecordLishiDiscardRedirection(movement);
    void IMovementRecordOrigins.CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to) => _coreRequestedDeckBasicsOperations.CaptureRequestedDeckBasicPaid(card, from, to);
    void IMovementRecordOrigins.CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement) => _coreDiscardedProvenanceOperations.CaptureDiscardedEntityOrigin(card, movement);
    void IMovementRecordOrigins.CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord movement, CardKind? frozenIdentity) => _coreAdjacentDiscardStorageOperations.CaptureAdjacentDiscardOrigin(card, movement, frozenIdentity);
    void IMovementRecordOrigins.CollectFullDiscardPhaseSuit(Card card, CardMovementRecord movement) => _coreProgramParticipantPhaseOperationsOperations.CollectFullDiscardPhaseSuit(card, movement);
    void IMovementRecordNotifications.Enqueue(CardMovementRecord movement) => _coreCommandJournalStoragePendingNotificationsAccess._pendingNotifications.Enqueue(new CardMovedNotification(movement));
}
