#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime))]
internal sealed class CoreMovementRecordsCharacterZhongHuiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterZhongHuiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhongHuiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public void RecordLishiDiscardRedirection(global::CardGame.Core.CardMovementRecord move) => this._targetFactory().RecordLishiDiscardRedirection(move);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime))]
internal sealed class CoreMovementRecordsCoreGainGiftProgramsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreGainGiftProgramsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineGainGiftProgramsRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.RedOwnedLossMaterial? CaptureRedOwnedLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, long sequence) => this._targetFactory().CaptureRedOwnedLoss(card, from, to, sequence);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime))]
internal sealed class CoreMovementRecordsCoreShanheStartingResourcesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreShanheStartingResourcesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineShanheStartingResourcesRuntime> _targetFactory = targetFactory;
    public void RecordShanheStartingResourceDestruction(global::CardGame.Core.CardMovementRecord move) => this._targetFactory().RecordShanheStartingResourceDestruction(move);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime))]
internal sealed class CoreMovementRecordsCharacterZhangHengAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterZhangHengAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhangHengAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AnniversaryZhangHengMovement? CaptureAnniversaryZhangHengMovement(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().CaptureAnniversaryZhangHengMovement(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime))]
internal sealed class CoreMovementRecordsCoreOriginalHandEntitiesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreOriginalHandEntitiesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineOriginalHandEntitiesRuntime> _targetFactory = targetFactory;
    public void ObserveOriginalHandMovement(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().ObserveOriginalHandMovement(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime))]
internal sealed class CoreMovementRecordsCharacterHuaTuoHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterHuaTuoHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterHuaTuoHappyGodRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.HappyMedicineHandLoss? CaptureHappyHuaTuoHandLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, long sequence) => this._targetFactory().CaptureHappyHuaTuoHandLoss(card, from, to, sequence);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime))]
internal sealed class CoreMovementRecordsCharacterZuoCiHappyGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterZuoCiHappyGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZuoCiHappyGodRuntime> _targetFactory = targetFactory;
    public void ValidateHappyZuoCiPhantomMovement(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().ValidateHappyZuoCiPhantomMovement(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime))]
internal sealed class CoreMovementRecordsCoreRequestedDeckBasicsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreRequestedDeckBasicsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRequestedDeckBasicsRuntime> _targetFactory = targetFactory;
    public void CaptureRequestedDeckBasicPaid(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().CaptureRequestedDeckBasicPaid(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime))]
internal sealed class CoreMovementRecordsCharacterJuShouMouAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterJuShouMouOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterJuShouMouRuntime> _targetFactory = targetFactory;
    public void ObserveMouJsMovement(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().ObserveMouJsMovement(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime))]
internal sealed class CoreMovementRecordsCharacterDengAiAnniversaryGodAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterDengAiAnniversaryGodOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDengAiAnniversaryGodRuntime> _targetFactory = targetFactory;
    public void RecordShenDaCarrierDestruction(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().RecordShenDaCarrierDestruction(m);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime))]
internal sealed class CoreMovementRecordsCoreSourceCursesAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreSourceCursesOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime> _targetFactory = targetFactory;
    public void ObserveSourceCurseMovement(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().ObserveSourceCurseMovement(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime))]
internal sealed class CoreMovementRecordsCoreAdjacentDiscardStorageAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreAdjacentDiscardStorageOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> _targetFactory = targetFactory;
    public void CaptureAdjacentDiscardOrigin(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.CardKind? frozenIdentity) => this._targetFactory().CaptureAdjacentDiscardOrigin(card, move, frozenIdentity);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime))]
internal sealed class CoreMovementRecordsCharacterDiaoChanLeAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterDiaoChanLeOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDiaoChanLeRuntime> _targetFactory = targetFactory;
    public void RecordDiouCarrierDestruction(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().RecordDiouCarrierDestruction(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime))]
internal sealed class CoreMovementRecordsCharacterBianFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterBianFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterBianFuRenOlRuntime> _targetFactory = targetFactory;
    public void CaptureBianFuRenForeignDiscardProducer(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().CaptureBianFuRenForeignDiscardProducer(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime))]
internal sealed class CoreMovementRecordsCoreProgramParticipantPhaseOperationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreProgramParticipantPhaseOperationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramParticipantPhaseOperationsRuntime> _targetFactory = targetFactory;
    public void CollectFullDiscardPhaseSuit(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord move) => this._targetFactory().CollectFullDiscardPhaseSuit(card, move);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime))]
internal sealed class CoreMovementRecordsCoreHappyWeiYanGainDestinationsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreHappyWeiYanGainDestinationsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreHappyWeiYanGainDestinationsRuntime> _targetFactory = targetFactory;
    public void ValidateHappyWeiYanGainDestinationRecord(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().ValidateHappyWeiYanGainDestinationRecord(movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime))]
internal sealed class CoreMovementRecordsCharacterZhugeLiangWumiaoAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterZhugeLiangWumiaoOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterZhugeLiangWumiaoRuntime> _targetFactory = targetFactory;
    public void RecordWuZglCopyMovement(global::CardGame.Core.CardMovementRecord move) => this._targetFactory().RecordWuZglCopyMovement(move);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime))]
internal sealed class CoreMovementRecordsCharacterDuFuRenOlAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterDuFuRenOlOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterDuFuRenOlRuntime> _targetFactory = targetFactory;
    public void CaptureDuFuRenOwnedOrigin(global::CardGame.Core.CardMovementRecord m) => this._targetFactory().CaptureDuFuRenOwnedOrigin(m);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime))]
internal sealed class CoreMovementRecordsCoreProgramActualPlaySlashLossAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreProgramActualPlaySlashLossOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreProgramActualPlaySlashLossRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.ActualPlayPhysicalSlashLossStamp? CaptureActualPlaySlashLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to) => this._targetFactory().CaptureActualPlaySlashLoss(card, from, to);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime))]
internal sealed class CoreMovementRecordsCoreDiscardedProvenanceAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreDiscardedProvenanceOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDiscardedProvenanceRuntime> _targetFactory = targetFactory;
    public void CaptureDiscardedEntityOrigin(global::CardGame.Core.Card card, global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().CaptureDiscardedEntityOrigin(card, movement);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime))]
internal sealed class CoreMovementRecordsCharacterGuoZhaoAnniversaryAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCharacterGuoZhaoAnniversaryOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CharacterGuoZhaoAnniversaryRuntime> _targetFactory = targetFactory;
    public global::CardGame.Core.AnniversaryGuoZhaoLoss? CaptureAnniversaryGuoZhaoLoss(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, int sequence) => this._targetFactory().CaptureAnniversaryGuoZhaoLoss(card, from, to, sequence);
}

[global::CardGame.Core.ProgramCapability(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime))]
internal sealed class CoreMovementRecordsCoreWushuangShopCardsAdapter(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> targetFactory) : global::CardGame.Core.EngineOwners.ICoreMovementRecordsCoreWushuangShopCardsOperations
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineWushuangShopCardsRuntime> _targetFactory = targetFactory;
    public void RecordWushuangShopEquipmentDestruction(global::CardGame.Core.CardMovementRecord movement) => this._targetFactory().RecordWushuangShopEquipmentDestruction(movement);
}
