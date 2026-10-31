#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreGameEngineAtomicCardMovementRuntime : IAtomicCardMovementEffects, IAtomicCardMovementFacts, IAtomicCardMovementPolicies
{
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterWeiYanHappyGodEntries _characterWeiYanHappyGodOperations;
    private readonly RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries _characterZhongHuiAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangWumiaoEntries _characterZhugeLiangWumiaoOperations;
    private readonly RuntimeEntries.CharacterZuoCiHappyGodEntries _characterZuoCiHappyGodOperations;
    private readonly RuntimeEntries.CoreActualDiscardPhaseLedgerEntries _coreActualDiscardPhaseLedgerOperations;
    private readonly RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries _coreAdjacentDiscardStorageOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardDeclarationsEntries _coreCardDeclarationsOperations;
    private readonly RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries _coreEphemeralCardMaterialsOperations;
    private readonly RuntimeEntries.CoreGameEngineEquipmentSkillsEntries _coreEquipmentSkillsOperations;
    private readonly RuntimeEntries.CoreGameEngineHellActivitiesEntries _coreHellActivitiesOperations;
    private readonly RuntimeEntries.CoreGameEngineHellSoulCardsEntries _coreHellSoulCardsOperations;
    private readonly RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries _coreShanheStartingResourcesOperations;
    private readonly RuntimeEntries.CoreGameEngineWushuangShopCardsEntries _coreWushuangShopCardsOperations;
    private readonly RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries _coreHappyWeiYanGainDestinationsOperations;
    private readonly RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    private readonly RuntimeEntries.CorePhysicalMovementFacadesEntries _corePhysicalMovementFacadesOperations;
    private readonly ICoreAtomicCardMovementAtomicCardMovementOperations _atomicCardMovementOperations;
    internal CoreGameEngineAtomicCardMovementRuntime(RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterWeiYanHappyGodEntries characterWeiYanHappyGodOperations, RuntimeEntries.CharacterZhongHuiAnniversaryGodEntries characterZhongHuiAnniversaryGodOperations, RuntimeEntries.CharacterZhugeLiangWumiaoEntries characterZhugeLiangWumiaoOperations, RuntimeEntries.CharacterZuoCiHappyGodEntries characterZuoCiHappyGodOperations, RuntimeEntries.CoreActualDiscardPhaseLedgerEntries coreActualDiscardPhaseLedgerOperations, RuntimeEntries.CoreGameEngineAdjacentDiscardStorageEntries coreAdjacentDiscardStorageOperations, RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, RuntimeEntries.CoreGameEngineCardDeclarationsEntries coreCardDeclarationsOperations, RuntimeEntries.CoreGameEngineEphemeralCardMaterialsEntries coreEphemeralCardMaterialsOperations, RuntimeEntries.CoreGameEngineEquipmentSkillsEntries coreEquipmentSkillsOperations, RuntimeEntries.CoreGameEngineHellActivitiesEntries coreHellActivitiesOperations, RuntimeEntries.CoreGameEngineHellSoulCardsEntries coreHellSoulCardsOperations, RuntimeEntries.CoreGameEngineShanheStartingResourcesEntries coreShanheStartingResourcesOperations, RuntimeEntries.CoreGameEngineWushuangShopCardsEntries coreWushuangShopCardsOperations, RuntimeEntries.CoreHappyWeiYanGainDestinationsEntries coreHappyWeiYanGainDestinationsOperations, RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations, RuntimeEntries.CorePhysicalMovementFacadesEntries corePhysicalMovementFacadesOperations, ICoreAtomicCardMovementAtomicCardMovementOperations atomicCardMovementOperations)
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
