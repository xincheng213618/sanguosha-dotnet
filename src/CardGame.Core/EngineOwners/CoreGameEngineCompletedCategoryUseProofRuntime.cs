#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCompletedCategoryUseProofRuntime
{
    private readonly RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly RuntimeEntries.CharacterChengYuMouOlEntries _characterChengYuMouOlOperations;
    private readonly RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly RuntimeEntries.CharacterYuanTanYuanShangOlEntries _characterYuanTanYuanShangOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries _coreAdjacentRedTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries _coreDamageTargetBudgetOperations;
    private readonly RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries _coreOverflowTargetCancellationObserversOperations;
    private readonly RuntimeEntries.CoreGameEngineRecipientCategorySlashTargetsEntries _coreRecipientCategorySlashTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries _coreRoundGainedTrickTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries _coreSameTypeActualUseAidOperations;
    private readonly RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries _coreShortRangeSlashTargetsOperations;
    private readonly RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries _coreShownEntityTurnPoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries _coreSoleTargetQuarantineOperations;
    private readonly RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries _coreSoleTargetRedirectOperations;
    private readonly RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries _coreTargetedOrdinaryTrickOperations;
    private readonly RuntimeEntries.CoreGameEngineUniqueLeaderTrickTargetsEntries _coreUniqueLeaderTrickTargetsOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    internal CoreGameEngineCompletedCategoryUseProofRuntime(RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, RuntimeEntries.CharacterChengYuMouOlEntries characterChengYuMouOlOperations, RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations, RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations, RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, RuntimeEntries.CharacterYuanTanYuanShangOlEntries characterYuanTanYuanShangOlOperations, IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries coreAdjacentRedTargetsOperations, RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries coreDamageTargetBudgetOperations, RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries coreOverflowTargetCancellationObserversOperations, RuntimeEntries.CoreGameEngineRecipientCategorySlashTargetsEntries coreRecipientCategorySlashTargetsOperations, RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries coreRoundGainedTrickTargetsOperations, RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries coreSameTypeActualUseAidOperations, RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries coreShortRangeSlashTargetsOperations, RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries coreShownEntityTurnPoliciesOperations, RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries coreSoleTargetQuarantineOperations, RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries coreSoleTargetRedirectOperations, RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries coreTargetedOrdinaryTrickOperations, RuntimeEntries.CoreGameEngineUniqueLeaderTrickTargetsEntries coreUniqueLeaderTrickTargetsOperations, RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations)
    {
        this._characterBeiMiHuOlOperations = characterBeiMiHuOlOperations;
        this._characterChengYuMouOlOperations = characterChengYuMouOlOperations;
        this._characterCuiLieAnniversaryOperations = characterCuiLieAnniversaryOperations;
        this._characterHuaManOlOperations = characterHuaManOlOperations;
        this._characterLiuPanOlOperations = characterLiuPanOlOperations;
        this._characterMaChengOlOperations = characterMaChengOlOperations;
        this._characterMaXiuMaTieOlOperations = characterMaXiuMaTieOlOperations;
        this._characterQianZhaoOlOperations = characterQianZhaoOlOperations;
        this._characterSunQuanOlMagicOperations = characterSunQuanOlMagicOperations;
        this._characterXunYuMobileGodOperations = characterXunYuMobileGodOperations;
        this._characterYangFuOlOperations = characterYangFuOlOperations;
        this._characterYuanTanYuanShangOlOperations = characterYuanTanYuanShangOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreAdjacentRedTargetsOperations = coreAdjacentRedTargetsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreDamageTargetBudgetOperations = coreDamageTargetBudgetOperations;
        this._coreDesignatedExtraTargetsOperations = coreDesignatedExtraTargetsOperations;
        this._coreOverflowTargetCancellationObserversOperations = coreOverflowTargetCancellationObserversOperations;
        this._coreRecipientCategorySlashTargetsOperations = coreRecipientCategorySlashTargetsOperations;
        this._coreRoundGainedTrickTargetsOperations = coreRoundGainedTrickTargetsOperations;
        this._coreSameTypeActualUseAidOperations = coreSameTypeActualUseAidOperations;
        this._coreShortRangeSlashTargetsOperations = coreShortRangeSlashTargetsOperations;
        this._coreShownEntityTurnPoliciesOperations = coreShownEntityTurnPoliciesOperations;
        this._coreSoleTargetProtectionOperations = coreSoleTargetProtectionOperations;
        this._coreSoleTargetQuarantineOperations = coreSoleTargetQuarantineOperations;
        this._coreSoleTargetRedirectOperations = coreSoleTargetRedirectOperations;
        this._coreTargetedOrdinaryTrickOperations = coreTargetedOrdinaryTrickOperations;
        this._coreUniqueLeaderTrickTargetsOperations = coreUniqueLeaderTrickTargetsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
    }
}
