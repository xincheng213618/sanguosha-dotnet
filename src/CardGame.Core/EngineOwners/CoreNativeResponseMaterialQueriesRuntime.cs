#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeResponseMaterialQueriesRuntime
{
    private readonly RuntimeEntries.CharacterCaoPiMouEntries _characterCaoPiMouOperations;
    private readonly RuntimeEntries.CharacterCuiYanOlEntries _characterCuiYanOlOperations;
    private readonly RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly RuntimeEntries.CharacterKongShuOlEntries _characterKongShuOlOperations;
    private readonly RuntimeEntries.CharacterMaChaoAnniversaryGodEntries _characterMaChaoAnniversaryGodOperations;
    private readonly RuntimeEntries.CharacterPangJiOlEntries _characterPangJiOlOperations;
    private readonly RuntimeEntries.CharacterXiahouHuiAnniversaryEntries _characterXiahouHuiAnniversaryOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangWumiaoEntries _characterZhugeLiangWumiaoOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineActionDiscardColorsEntries _coreActionDiscardColorsOperations;
    private readonly RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardConversionsEntries _coreCardConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineLastZoneConversionsEntries _coreLastZoneConversionsOperations;
    private readonly RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries _coreNativePublicPileOriginalCardsOperations;
    private readonly RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries _coreOwnPublicPileConversionsOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineRecordedSuitSlashEntries _coreRecordedSuitSlashOperations;
    private readonly RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries _coreResponseEntityExchangeOperations;
    private readonly RuntimeEntries.CoreNativeTargetLegalityEntries _coreNativeTargetLegalityOperations;
    private readonly RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly RuntimeEntries.CoreProgramCardLegalityEntries _coreProgramCardLegalityOperations;
    internal CoreNativeResponseMaterialQueriesRuntime(RuntimeEntries.CharacterCaoPiMouEntries characterCaoPiMouOperations, RuntimeEntries.CharacterCuiYanOlEntries characterCuiYanOlOperations, RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, RuntimeEntries.CharacterKongShuOlEntries characterKongShuOlOperations, RuntimeEntries.CharacterMaChaoAnniversaryGodEntries characterMaChaoAnniversaryGodOperations, RuntimeEntries.CharacterPangJiOlEntries characterPangJiOlOperations, RuntimeEntries.CharacterXiahouHuiAnniversaryEntries characterXiahouHuiAnniversaryOperations, RuntimeEntries.CharacterZhugeLiangWumiaoEntries characterZhugeLiangWumiaoOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineActionDiscardColorsEntries coreActionDiscardColorsOperations, RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, RuntimeEntries.CoreGameEngineCardConversionsEntries coreCardConversionsOperations, RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, RuntimeEntries.CoreGameEngineLastZoneConversionsEntries coreLastZoneConversionsOperations, RuntimeEntries.CoreGameEngineNativePublicPileOriginalCardsEntries coreNativePublicPileOriginalCardsOperations, RuntimeEntries.CoreGameEngineOwnPublicPileConversionsEntries coreOwnPublicPileConversionsOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineRecordedSuitSlashEntries coreRecordedSuitSlashOperations, RuntimeEntries.CoreGameEngineResponseEntityExchangeEntries coreResponseEntityExchangeOperations, RuntimeEntries.CoreNativeTargetLegalityEntries coreNativeTargetLegalityOperations, RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, RuntimeEntries.CoreProgramCardLegalityEntries coreProgramCardLegalityOperations)
    {
        this._characterCaoPiMouOperations = characterCaoPiMouOperations;
        this._characterCuiYanOlOperations = characterCuiYanOlOperations;
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterKongShuOlOperations = characterKongShuOlOperations;
        this._characterMaChaoAnniversaryGodOperations = characterMaChaoAnniversaryGodOperations;
        this._characterPangJiOlOperations = characterPangJiOlOperations;
        this._characterXiahouHuiAnniversaryOperations = characterXiahouHuiAnniversaryOperations;
        this._characterZhugeLiangWumiaoOperations = characterZhugeLiangWumiaoOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActionDiscardColorsOperations = coreActionDiscardColorsOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardConversionsOperations = coreCardConversionsOperations;
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreLastZoneConversionsOperations = coreLastZoneConversionsOperations;
        this._coreNativePublicPileOriginalCardsOperations = coreNativePublicPileOriginalCardsOperations;
        this._coreOwnPublicPileConversionsOperations = coreOwnPublicPileConversionsOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreRecordedSuitSlashOperations = coreRecordedSuitSlashOperations;
        this._coreResponseEntityExchangeOperations = coreResponseEntityExchangeOperations;
        this._coreNativeTargetLegalityOperations = coreNativeTargetLegalityOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._coreProgramCardLegalityOperations = coreProgramCardLegalityOperations;
    }
}
