#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineMovementRecordsRuntime
{
    int global::CardGame.Core.IMovementRecordFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    int global::CardGame.Core.IMovementRecordFacts.TurnNumber => _coreActualTurnProgressStartedTurnNumberAccess._turnNumber;

    bool global::CardGame.Core.IMovementRecordFacts.PublishesDisclosure => _modeRulesOperations._modeRules.PublishesCardMovementDisclosure;

    bool global::CardGame.Core.IMovementRecordFacts.Started => _coreActualTurnProgressStartedTurnNumberAccess._started;

    CardMoveReason global::CardGame.Core.IMovementRecordFacts.SimultaneousTopReservationReason => global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsDefinitions.SimultaneousTopReservationReason;

    string global::CardGame.Core.IMovementRecordFacts.PrivatePindianReserveReason => "skill-program.wu-lu-kang.shenduan.reserve";

    ActualPlayPhysicalSlashLossStamp? global::CardGame.Core.IMovementRecordMetadata.CaptureActualPlaySlashLoss(Card card, CardLocation from, CardLocation to) => _coreProgramActualPlaySlashLossOperations.CaptureActualPlaySlashLoss(card, from, to);
    RedOwnedLossMaterial? global::CardGame.Core.IMovementRecordMetadata.CaptureRedOwnedLoss(Card card, CardLocation from, CardLocation to, long sequence) => _coreGainGiftProgramsOperations.CaptureRedOwnedLoss(card, from, to, sequence);
    HappyMedicineHandLoss? global::CardGame.Core.IMovementRecordMetadata.CaptureHappyHuaTuoHandLoss(Card card, CardLocation from, CardLocation to, long sequence) => _characterHuaTuoHappyGodOperations.CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
    AnniversaryGuoZhaoLoss? global::CardGame.Core.IMovementRecordMetadata.CaptureAnniversaryGuoZhaoLoss(Card card, CardLocation from, CardLocation to, int sequence) => _characterGuoZhaoAnniversaryOperations.CaptureAnniversaryGuoZhaoLoss(card, from, to, sequence);
    AnniversaryZhangHengMovement? global::CardGame.Core.IMovementRecordMetadata.CaptureAnniversaryZhangHengMovement(Card card, CardLocation from, CardLocation to) => _characterZhangHengAnniversaryOperations.CaptureAnniversaryZhangHengMovement(card, from, to);
    int global::CardGame.Core.IMovementRecordJournal.NextSequence() => ++_coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
    void global::CardGame.Core.IMovementRecordJournal.Append(CardMovementRecord movement) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Add(movement);
    void global::CardGame.Core.IMovementRecordMaterials.ValidateHappyWeiYanGainDestinationRecord(CardMovementRecord movement) => _coreHappyWeiYanGainDestinationsOperations.ValidateHappyWeiYanGainDestinationRecord(movement);
    void global::CardGame.Core.IMovementRecordMaterials.ValidateHappyZuoCiPhantomMovement(CardMovementRecord movement) => _characterZuoCiHappyGodOperations.ValidateHappyZuoCiPhantomMovement(movement);
    void global::CardGame.Core.IMovementRecordMaterials.RecordDiouCarrierDestruction(CardMovementRecord movement) => _characterDiaoChanLeOperations.RecordDiouCarrierDestruction(movement);
    void global::CardGame.Core.IMovementRecordMaterials.RecordShenDaCarrierDestruction(CardMovementRecord movement) => _characterDengAiAnniversaryGodOperations.RecordShenDaCarrierDestruction(movement);
    void global::CardGame.Core.IMovementRecordMaterials.RecordWushuangShopEquipmentDestruction(CardMovementRecord movement) => _coreWushuangShopCardsOperations.RecordWushuangShopEquipmentDestruction(movement);
    void global::CardGame.Core.IMovementRecordMaterials.RecordShanheStartingResourceDestruction(CardMovementRecord movement) => _coreShanheStartingResourcesOperations.RecordShanheStartingResourceDestruction(movement);
    void global::CardGame.Core.IMovementRecordMaterials.RecordWuZglCopyMovement(CardMovementRecord movement) => _characterZhugeLiangWumiaoOperations.RecordWuZglCopyMovement(movement);
    void global::CardGame.Core.IMovementRecordObservers.ObserveMouJsMovement(CardMovementRecord movement) => _characterJuShouMouOperations.ObserveMouJsMovement(movement);
    void global::CardGame.Core.IMovementRecordObservers.CaptureBianFuRenForeignDiscardProducer(CardMovementRecord movement) => _characterBianFuRenOlOperations.CaptureBianFuRenForeignDiscardProducer(movement);
    void global::CardGame.Core.IMovementRecordObservers.CaptureDuFuRenOwnedOrigin(CardMovementRecord movement) => _characterDuFuRenOlOperations.CaptureDuFuRenOwnedOrigin(movement);
    void global::CardGame.Core.IMovementRecordObservers.ObserveOriginalHandMovement(CardMovementRecord movement) => _coreOriginalHandEntitiesOperations.ObserveOriginalHandMovement(movement);
    void global::CardGame.Core.IMovementRecordObservers.ObserveSourceCurseMovement(CardMovementRecord movement) => _coreSourceCursesOperations.ObserveSourceCurseMovement(movement);
    void global::CardGame.Core.IMovementRecordOrigins.RecordLishiDiscardRedirection(CardMovementRecord movement) => _characterZhongHuiAnniversaryGodOperations.RecordLishiDiscardRedirection(movement);
    void global::CardGame.Core.IMovementRecordOrigins.CaptureRequestedDeckBasicPaid(Card card, CardLocation from, CardLocation to) => _coreRequestedDeckBasicsOperations.CaptureRequestedDeckBasicPaid(card, from, to);
    void global::CardGame.Core.IMovementRecordOrigins.CaptureDiscardedEntityOrigin(Card card, CardMovementRecord movement) => _coreDiscardedProvenanceOperations.CaptureDiscardedEntityOrigin(card, movement);
    void global::CardGame.Core.IMovementRecordOrigins.CaptureAdjacentDiscardOrigin(Card card, CardMovementRecord movement, CardKind? frozenIdentity) => _coreAdjacentDiscardStorageOperations.CaptureAdjacentDiscardOrigin(card, movement, frozenIdentity);
    void global::CardGame.Core.IMovementRecordOrigins.CollectFullDiscardPhaseSuit(Card card, CardMovementRecord movement) => _coreProgramParticipantPhaseOperationsOperations.CollectFullDiscardPhaseSuit(card, movement);
    void global::CardGame.Core.IMovementRecordNotifications.Enqueue(CardMovementRecord movement) => _coreCommandJournalStoragePendingNotificationsAccess._pendingNotifications.Enqueue(new CardMovedNotification(movement));
}
