#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreMovementRecordsModeRulesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    ref readonly global::CardGame.Core.ModeRuleSet _modeRules { get; }
}

internal interface ICoreMovementRecordsCharacterZhongHuiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordLishiDiscardRedirection(global::CardGame.Core.CardMovementRecord move);
}

internal interface ICoreMovementRecordsCoreGainGiftProgramsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.RedOwnedLossMaterial? CaptureRedOwnedLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, long sequence);
}

internal interface ICoreMovementRecordsCoreShanheStartingResourcesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordShanheStartingResourceDestruction(global::CardGame.Core.CardMovementRecord move);
}

internal interface ICoreMovementRecordsCharacterZhangHengAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreMovementRecordsCoreOriginalHandEntitiesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveOriginalHandMovement(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCharacterHuaTuoHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, long sequence);
}

internal interface ICoreMovementRecordsCharacterZuoCiHappyGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateHappyZuoCiPhantomMovement(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCoreRequestedDeckBasicsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureRequestedDeckBasicPaid(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreMovementRecordsCharacterJuShouMouOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveMouJsMovement(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCharacterDengAiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordShenDaCarrierDestruction(global::CardGame.Core.CardMovementRecord m);
}

internal interface ICoreMovementRecordsCoreSourceCursesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ObserveSourceCurseMovement(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCoreAdjacentDiscardStorageOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureAdjacentDiscardOrigin(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardKind? frozenIdentity);
}

internal interface ICoreMovementRecordsCharacterDiaoChanLeOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordDiouCarrierDestruction(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCharacterBianFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCoreProgramParticipantPhaseOperationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CollectFullDiscardPhaseSuit(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord move);
}

internal interface ICoreMovementRecordsCoreHappyWeiYanGainDestinationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void ValidateHappyWeiYanGainDestinationRecord(global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCharacterZhugeLiangWumiaoOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordWuZglCopyMovement(global::CardGame.Core.CardMovementRecord move);
}

internal interface ICoreMovementRecordsCharacterDuFuRenOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureDuFuRenOwnedOrigin(global::CardGame.Core.CardMovementRecord m);
}

internal interface ICoreMovementRecordsCoreProgramActualPlaySlashLossOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to);
}

internal interface ICoreMovementRecordsCoreDiscardedProvenanceOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void CaptureDiscardedEntityOrigin(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord movement);
}

internal interface ICoreMovementRecordsCharacterGuoZhaoAnniversaryOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.AnniversaryGuoZhaoLoss? CaptureAnniversaryGuoZhaoLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, int sequence);
}

internal interface ICoreMovementRecordsCoreWushuangShopCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    void RecordWushuangShopEquipmentDestruction(global::CardGame.Core.CardMovementRecord movement);
}
