#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreDyingCursorRuntimeRuntime
{
    private readonly RuntimeEntries.CharacterDianWeiHappyGodEntries _characterDianWeiHappyGodOperations;
    private readonly RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly RuntimeEntries.CharacterHeJinOlEntries _characterHeJinOlOperations;
    private readonly RuntimeEntries.CharacterJiangWeiMobileGodEntries _characterJiangWeiMobileGodOperations;
    private readonly RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly RuntimeEntries.CharacterYangBiaoAnniversaryEntries _characterYangBiaoAnniversaryOperations;
    private readonly RuntimeEntries.CharacterZhangFeiWumiaoEntries _characterZhangFeiWumiaoOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreGameEngineDeathGrowthSkillObserversEntries _coreDeathGrowthSkillObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries _coreDyingSuitsAndHistoricalUseObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineNativeDrawThresholdDyingEntries _coreNativeDrawThresholdDyingOperations;
    private readonly RuntimeEntries.CoreGameEngineNativeRecoveryDamageEntries _coreNativeRecoveryDamageOperations;
    private readonly RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries _coreOwnedDeathBenefitReturnsOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramOwnerDeathDamageEntries _coreProgramOwnerDeathDamageOperations;
    private readonly RuntimeEntries.CoreGameEngineRecipientCategoryMarkContinuationsEntries _coreRecipientCategoryMarkContinuationsOperations;
    private readonly RuntimeEntries.CoreGameEngineWenhePoisonEntries _coreWenhePoisonOperations;
    private readonly RuntimeEntries.CoreHellKingProgramsEntries _coreHellKingProgramsOperations;
    private readonly RuntimeEntries.CoreProgramDamageInsertionRuntimeEntries _coreProgramDamageInsertionRuntimeOperations;
    internal CoreDyingCursorRuntimeRuntime(RuntimeEntries.CharacterDianWeiHappyGodEntries characterDianWeiHappyGodOperations, RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, RuntimeEntries.CharacterHeJinOlEntries characterHeJinOlOperations, RuntimeEntries.CharacterJiangWeiMobileGodEntries characterJiangWeiMobileGodOperations, RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, RuntimeEntries.CharacterYangBiaoAnniversaryEntries characterYangBiaoAnniversaryOperations, RuntimeEntries.CharacterZhangFeiWumiaoEntries characterZhangFeiWumiaoOperations, IKernelFrameStoreAccess resolutionStackOperations, RuntimeEntries.CoreGameEngineDeathGrowthSkillObserversEntries coreDeathGrowthSkillObserversOperations, RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries coreDyingSuitsAndHistoricalUseObserversOperations, RuntimeEntries.CoreGameEngineNativeDrawThresholdDyingEntries coreNativeDrawThresholdDyingOperations, RuntimeEntries.CoreGameEngineNativeRecoveryDamageEntries coreNativeRecoveryDamageOperations, RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries coreOwnedDeathBenefitReturnsOperations, RuntimeEntries.CoreGameEngineProgramOwnerDeathDamageEntries coreProgramOwnerDeathDamageOperations, RuntimeEntries.CoreGameEngineRecipientCategoryMarkContinuationsEntries coreRecipientCategoryMarkContinuationsOperations, RuntimeEntries.CoreGameEngineWenhePoisonEntries coreWenhePoisonOperations, RuntimeEntries.CoreHellKingProgramsEntries coreHellKingProgramsOperations, RuntimeEntries.CoreProgramDamageInsertionRuntimeEntries coreProgramDamageInsertionRuntimeOperations)
    {
        this._characterDianWeiHappyGodOperations = characterDianWeiHappyGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterHeJinOlOperations = characterHeJinOlOperations;
        this._characterJiangWeiMobileGodOperations = characterJiangWeiMobileGodOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._characterYangBiaoAnniversaryOperations = characterYangBiaoAnniversaryOperations;
        this._characterZhangFeiWumiaoOperations = characterZhangFeiWumiaoOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDeathGrowthSkillObserversOperations = coreDeathGrowthSkillObserversOperations;
        this._coreDyingSuitsAndHistoricalUseObserversOperations = coreDyingSuitsAndHistoricalUseObserversOperations;
        this._coreNativeDrawThresholdDyingOperations = coreNativeDrawThresholdDyingOperations;
        this._coreNativeRecoveryDamageOperations = coreNativeRecoveryDamageOperations;
        this._coreOwnedDeathBenefitReturnsOperations = coreOwnedDeathBenefitReturnsOperations;
        this._coreProgramOwnerDeathDamageOperations = coreProgramOwnerDeathDamageOperations;
        this._coreRecipientCategoryMarkContinuationsOperations = coreRecipientCategoryMarkContinuationsOperations;
        this._coreWenhePoisonOperations = coreWenhePoisonOperations;
        this._coreHellKingProgramsOperations = coreHellKingProgramsOperations;
        this._coreProgramDamageInsertionRuntimeOperations = coreProgramDamageInsertionRuntimeOperations;
    }
}
