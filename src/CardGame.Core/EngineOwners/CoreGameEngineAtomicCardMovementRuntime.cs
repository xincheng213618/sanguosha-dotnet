#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineAtomicCardMovementRuntime : global::CardGame.Core.IAtomicCardMovementEffects, global::CardGame.Core.IAtomicCardMovementFacts, global::CardGame.Core.IAtomicCardMovementPolicies
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries _characterZhugeLiangWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries _characterZuoCiHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries _coreActualDiscardPhaseLedgerOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries _coreAdjacentDiscardStorageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries _coreEquipmentSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries _coreHellSoulCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly ICoreAtomicCardMovementAtomicCardMovementOperations _atomicCardMovementOperations;
    internal CoreGameEngineAtomicCardMovementRuntime(global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangWumiaoEntries characterZhugeLiangWumiaoOperations, global::CardGame.Core.RuntimeEntries.CharacterZuoCiHappyGodEntries characterZuoCiHappyGodOperations, global::CardGame.Core.RuntimeEntries.CoreActualDiscardPhaseLedgerEntries coreActualDiscardPhaseLedgerOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries coreAdjacentDiscardStorageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEquipmentSkillsEntries coreEquipmentSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellSoulCardsEntries coreHellSoulCardsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, global::CardGame.Core.RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations, global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, ICoreAtomicCardMovementAtomicCardMovementOperations atomicCardMovementOperations)
    {
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterWeiYanHappyGodOperations = characterWeiYanHappyGodOperations;
        this._characterZhongHuiAnniversaryGodOperations = characterZhongHuiAnniversaryGodOperations;
        this._characterZhugeLiangWumiaoOperations = characterZhugeLiangWumiaoOperations;
        this._characterZuoCiHappyGodOperations = characterZuoCiHappyGodOperations;
        this._coreActualDiscardPhaseLedgerOperations = coreActualDiscardPhaseLedgerOperations;
        this._coreAdjacentDiscardStorageOperations = coreAdjacentDiscardStorageOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardDeclarationsOperations = coreCardDeclarationsOperations;
        this._coreEphemeralCardMaterialsOperations = coreEphemeralCardMaterialsOperations;
        this._coreEquipmentSkillsOperations = coreEquipmentSkillsOperations;
        this._coreHellActivitiesOperations = coreHellActivitiesOperations;
        this._coreHellSoulCardsOperations = coreHellSoulCardsOperations;
        this._coreShanheStartingResourcesOperations = coreShanheStartingResourcesOperations;
        this._coreWushuangShopCardsOperations = coreWushuangShopCardsOperations;
        this._coreHappyWeiYanGainDestinationsOperations = coreHappyWeiYanGainDestinationsOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._atomicCardMovementOperations = atomicCardMovementOperations;
    }
}
