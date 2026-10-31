#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAtomicCardMovesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries _characterZuoCiHappyGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess _cardZonesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries _coreActualDiscardPhaseLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries _coreCardMovementProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries _coreEquipmentSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries _coreHellSoulCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess _coreCardMovementJournalMovementSequenceAccess;
    internal CoreGameEngineProgramAtomicCardMovesRuntime(global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries characterZuoCiHappyGodOperations, global::CardGame.Core.EngineOwners.IKernelCardZoneStoreAccess cardZonesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries coreActualDiscardPhaseLedgerOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardMovementProgramsEntries coreCardMovementProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries coreEquipmentSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries coreHellSoulCardsOperations, global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations, global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalMovementSequenceAccess coreCardMovementJournalMovementSequenceAccess)
    {
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._characterZuoCiHappyGodOperations = characterZuoCiHappyGodOperations;
        this._cardZonesOperations = cardZonesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualDiscardPhaseLedgerOperations = coreActualDiscardPhaseLedgerOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._coreHellSoulCardsOperations = coreHellSoulCardsOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreCardMovementJournalMovementSequenceAccess = coreCardMovementJournalMovementSequenceAccess;
    }
}
