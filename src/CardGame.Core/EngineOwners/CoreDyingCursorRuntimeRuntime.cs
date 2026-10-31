#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreDyingCursorRuntimeRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDianWeiHappyGodEntries _characterDianWeiHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries _characterHeJinOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMobileGodEntries _characterJiangWeiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangBiaoAnniversaryEntries _characterYangBiaoAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries _characterZhangFeiWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathGrowthSkillObserversEntries _coreDeathGrowthSkillObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries _coreDyingSuitsAndHistoricalUseObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawThresholdDyingEntries _coreNativeDrawThresholdDyingOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeRecoveryDamageEntries _coreNativeRecoveryDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries _coreOwnedDeathBenefitReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnerDeathDamageEntries _coreProgramOwnerDeathDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkContinuationsEntries _coreRecipientCategoryMarkContinuationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries _coreWenhePoisonOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellKingProgramsEntries _coreHellKingProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramDamageInsertionRuntimeEntries _coreProgramDamageInsertionRuntimeOperations;
    internal CoreDyingCursorRuntimeRuntime(global::CardGame.Core.RuntimeEntries.CharacterDianWeiHappyGodEntries characterDianWeiHappyGodOperations, global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations, global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries characterHeJinOlOperations, global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMobileGodEntries characterJiangWeiMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYangBiaoAnniversaryEntries characterYangBiaoAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries characterZhangFeiWumiaoOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathGrowthSkillObserversEntries coreDeathGrowthSkillObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries coreDyingSuitsAndHistoricalUseObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeDrawThresholdDyingEntries coreNativeDrawThresholdDyingOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNativeRecoveryDamageEntries coreNativeRecoveryDamageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries coreOwnedDeathBenefitReturnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnerDeathDamageEntries coreProgramOwnerDeathDamageOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkContinuationsEntries coreRecipientCategoryMarkContinuationsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineWenhePoisonEntries coreWenhePoisonOperations, global::CardGame.Core.RuntimeEntries.CoreHellKingProgramsEntries coreHellKingProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreProgramDamageInsertionRuntimeEntries coreProgramDamageInsertionRuntimeOperations)
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
