#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineObserverSubtreesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries _characterDaQiaoXiaoQiaoHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDianWeiHappyGodEntries _characterDianWeiHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanHappyGodEntries _characterDiaoChanHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries _characterDiaoChanLeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries _characterHeJinOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMuShunAnniversaryEntries _characterMuShunAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterPangTongHappyGodEntries _characterPangTongHappyGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries _characterZhangFeiWumiaoOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardSupplyCompletionEntries _coreCardSupplyCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealObserversEntries _coreCompletedUndamagedTargetRevealObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries _coreDeathProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries _coreDyingSuitsAndHistoricalUseObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries _coreHalfHandPhaseDebtDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardObserversEntries _coreOffTurnUsedCardObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntityObserversEntries _coreOriginalHandEntityObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardObserversEntries _coreOutsidePhaseDrawDiscardObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries _coreOwnedDeathBenefitReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries _coreProgramChildResolutionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkContinuationsEntries _coreRecipientCategoryMarkContinuationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries _coreRecipientCategoryMarkObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries _coreResponseCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries _coreSameNameHandDiscardOrDamageObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries _coreUnrespondableCounterspellsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageCompletionEntries _coreNativeDamageCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries _coreParticipantHandProgramRescueRidesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery _resolutionStackOperations2;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries _coreDrawFundedDistinctBasicReturnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries _coreTieredRoundZeroReturnsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramAttackHandleOperations _kernelCoreGameEngineProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    internal CoreGameEngineObserverSubtreesRuntime(global::CardGame.Core.RuntimeEntries.CharacterDaQiaoXiaoQiaoHappyGodEntries characterDaQiaoXiaoQiaoHappyGodOperations,global::CardGame.Core.RuntimeEntries.CharacterDianWeiHappyGodEntries characterDianWeiHappyGodOperations,global::CardGame.Core.RuntimeEntries.CharacterDiaoChanHappyGodEntries characterDiaoChanHappyGodOperations,global::CardGame.Core.RuntimeEntries.CharacterDiaoChanLeEntries characterDiaoChanLeOperations,global::CardGame.Core.RuntimeEntries.CharacterHeJinOlEntries characterHeJinOlOperations,global::CardGame.Core.RuntimeEntries.CharacterMuShunAnniversaryEntries characterMuShunAnniversaryOperations,global::CardGame.Core.RuntimeEntries.CharacterPangTongHappyGodEntries characterPangTongHappyGodOperations,global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations,global::CardGame.Core.RuntimeEntries.CharacterZhangFeiWumiaoEntries characterZhangFeiWumiaoOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardSupplyCompletionEntries coreCardSupplyCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCompletedUndamagedTargetRevealObserversEntries coreCompletedUndamagedTargetRevealObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDeathProgramsEntries coreDeathProgramsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDyingSuitsAndHistoricalUseObserversEntries coreDyingSuitsAndHistoricalUseObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineHalfHandPhaseDebtDamageObserversEntries coreHalfHandPhaseDebtDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOffTurnUsedCardObserversEntries coreOffTurnUsedCardObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOriginalHandEntityObserversEntries coreOriginalHandEntityObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOutsidePhaseDrawDiscardObserversEntries coreOutsidePhaseDrawDiscardObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineOwnedDeathBenefitReturnsEntries coreOwnedDeathBenefitReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramChildResolutionEntries coreProgramChildResolutionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkContinuationsEntries coreRecipientCategoryMarkContinuationsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategoryMarkObserversEntries coreRecipientCategoryMarkObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineResponseCompletionEntries coreResponseCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSameNameHandDiscardOrDamageObserversEntries coreSameNameHandDiscardOrDamageObserversOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineUnrespondableCounterspellsEntries coreUnrespondableCounterspellsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamageCompletionEntries coreNativeDamageCompletionOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandProgramRescueRidesEntries coreParticipantHandProgramRescueRidesOperations,global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreQuery resolutionStackOperations2,global::CardGame.Core.RuntimeEntries.CoreGameEngineDrawFundedDistinctBasicReturnsEntries coreDrawFundedDistinctBasicReturnsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroReturnsEntries coreTieredRoundZeroReturnsOperations,global::CardGame.Core.EngineOwners.IKernelCreateProgramAttackHandleOperations kernelCoreGameEngineProgramAttackAttemptOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations)
    {
        this._characterDaQiaoXiaoQiaoHappyGodOperations = characterDaQiaoXiaoQiaoHappyGodOperations;
        this._characterDianWeiHappyGodOperations = characterDianWeiHappyGodOperations;
        this._characterDiaoChanHappyGodOperations = characterDiaoChanHappyGodOperations;
        this._characterDiaoChanLeOperations = characterDiaoChanLeOperations;
        this._characterHeJinOlOperations = characterHeJinOlOperations;
        this._characterMuShunAnniversaryOperations = characterMuShunAnniversaryOperations;
        this._characterPangTongHappyGodOperations = characterPangTongHappyGodOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._characterZhangFeiWumiaoOperations = characterZhangFeiWumiaoOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreCardSupplyCompletionOperations = coreCardSupplyCompletionOperations;
        this._coreCompletedUndamagedTargetRevealObserversOperations = coreCompletedUndamagedTargetRevealObserversOperations;
        this._coreDeathProgramsOperations = coreDeathProgramsOperations;
        this._coreDyingSuitsAndHistoricalUseObserversOperations = coreDyingSuitsAndHistoricalUseObserversOperations;
        this._coreHalfHandPhaseDebtDamageObserversOperations = coreHalfHandPhaseDebtDamageObserversOperations;
        this._coreOffTurnUsedCardObserversOperations = coreOffTurnUsedCardObserversOperations;
        this._coreOriginalHandEntityObserversOperations = coreOriginalHandEntityObserversOperations;
        this._coreOutsidePhaseDrawDiscardObserversOperations = coreOutsidePhaseDrawDiscardObserversOperations;
        this._coreOwnedDeathBenefitReturnsOperations = coreOwnedDeathBenefitReturnsOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramChildResolutionOperations = coreProgramChildResolutionOperations;
        this._coreRecipientCategoryMarkContinuationsOperations = coreRecipientCategoryMarkContinuationsOperations;
        this._coreRecipientCategoryMarkObserversOperations = coreRecipientCategoryMarkObserversOperations;
        this._coreResponseCompletionOperations = coreResponseCompletionOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSameNameHandDiscardOrDamageObserversOperations = coreSameNameHandDiscardOrDamageObserversOperations;
        this._coreUnrespondableCounterspellsOperations = coreUnrespondableCounterspellsOperations;
        this._coreNativeDamageCompletionOperations = coreNativeDamageCompletionOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
        this._coreParticipantHandProgramRescueRidesOperations = coreParticipantHandProgramRescueRidesOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._resolutionStackOperations2 = resolutionStackOperations2;
        this._coreDrawFundedDistinctBasicReturnsOperations = coreDrawFundedDistinctBasicReturnsOperations;
        this._coreTieredRoundZeroReturnsOperations = coreTieredRoundZeroReturnsOperations;
        this._kernelCoreGameEngineProgramAttackAttemptOperations = kernelCoreGameEngineProgramAttackAttemptOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
    }
}
