#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineMovementRecordsRuntime : global::CardGame.Core.IMovementRecordFacts, global::CardGame.Core.IMovementRecordJournal, global::CardGame.Core.IMovementRecordMaterials, global::CardGame.Core.IMovementRecordMetadata, global::CardGame.Core.IMovementRecordNotifications, global::CardGame.Core.IMovementRecordObservers, global::CardGame.Core.IMovementRecordOrigins
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries _characterBianFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries _characterDengAiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries _characterDuFuRenOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries _characterGuoZhaoAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaTuoHappyGodEntries _characterHuaTuoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJuShouMouEntries _characterJuShouMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangHengAnniversaryEntries _characterZhangHengAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries _characterZhugeLiangWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries _characterZuoCiHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries _coreAdjacentDiscardStorageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries _coreDiscardedProvenanceOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries _coreGainGiftProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries _coreOriginalHandEntitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries _coreRequestedDeckBasicsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries _coreSourceCursesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramActualPlaySlashLossEntries _coreProgramActualPlaySlashLossOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries _coreProgramParticipantPhaseOperationsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess _coreActualTurnProgressStartedTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess _coreCardMovementJournalCardMovementsMovementSequenceAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess _coreCommandJournalStoragePendingNotificationsAccess;
    internal CoreGameEngineMovementRecordsRuntime(global::CardGame.Core.RuntimeEntries.CharacterBianFuRenOlEntries characterBianFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterDengAiAnniversaryGodEntries characterDengAiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterDuFuRenOlEntries characterDuFuRenOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuoZhaoAnniversaryEntries characterGuoZhaoAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterHuaTuoHappyGodEntries characterHuaTuoHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterJuShouMouEntries characterJuShouMouOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangHengAnniversaryEntries characterZhangHengAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries characterZhugeLiangWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries characterZuoCiHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries coreAdjacentDiscardStorageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDiscardedProvenanceEntries coreDiscardedProvenanceOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineGainGiftProgramsEntries coreGainGiftProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntitiesEntries coreOriginalHandEntitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRequestedDeckBasicsEntries coreRequestedDeckBasicsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSourceCursesEntries coreSourceCursesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations, global::CardGame.Core.RuntimeEntries.CoreProgramActualPlaySlashLossEntries coreProgramActualPlaySlashLossOperations, global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPhaseOperationsEntries coreProgramParticipantPhaseOperationsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressStartedTurnNumberAccess coreActualTurnProgressStartedTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsMovementSequenceAccess coreCardMovementJournalCardMovementsMovementSequenceAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess coreCommandJournalStoragePendingNotificationsAccess)
    {
        this._characterBianFuRenOlOperations = characterBianFuRenOlOperations;
        this._characterDengAiAnniversaryGodOperations = characterDengAiAnniversaryGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterDuFuRenOlOperations = characterDuFuRenOlOperations;
        this._characterGuoZhaoAnniversaryOperations = characterGuoZhaoAnniversaryOperations;
        this._characterHuaTuoHappyGodOperations = characterHuaTuoHappyGodOperations;
        this._characterJuShouMouOperations = characterJuShouMouOperations;
        this._characterZhangHengAnniversaryOperations = characterZhangHengAnniversaryOperations;
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._characterZhugeLiangWumiaoOperations = characterZhugeLiangWumiaoOperations;
        this._characterZuoCiHappyGodOperations = characterZuoCiHappyGodOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreAdjacentDiscardStorageOperations = coreAdjacentDiscardStorageOperations;
        this._coreDiscardedProvenanceOperations = coreDiscardedProvenanceOperations;
        this._coreGainGiftProgramsOperations = coreGainGiftProgramsOperations;
        this._coreOriginalHandEntitiesOperations = coreOriginalHandEntitiesOperations;
        this._coreRequestedDeckBasicsOperations = coreRequestedDeckBasicsOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreSourceCursesOperations = coreSourceCursesOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._coreProgramActualPlaySlashLossOperations = coreProgramActualPlaySlashLossOperations;
        this._coreProgramParticipantPhaseOperationsOperations = coreProgramParticipantPhaseOperationsOperations;
        this._coreActualTurnProgressStartedTurnNumberAccess = coreActualTurnProgressStartedTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsMovementSequenceAccess = coreCardMovementJournalCardMovementsMovementSequenceAccess;
        this._coreCommandJournalStoragePendingNotificationsAccess = coreCommandJournalStoragePendingNotificationsAccess;
    }
}
