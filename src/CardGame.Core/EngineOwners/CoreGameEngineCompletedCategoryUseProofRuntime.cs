#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCompletedCategoryUseProofRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterChengYuMouOlEntries _characterChengYuMouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries _characterCuiLieAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries _characterHuaManOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries _characterLiuPanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries _characterMaChengOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries _characterMaXiuMaTieOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries _characterQianZhaoOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries _characterSunQuanOlMagicOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries _characterXunYuMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries _characterYangFuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries _characterYuanTanYuanShangOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries _coreAdjacentRedTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries _coreDamageTargetBudgetOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries _coreDesignatedExtraTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries _coreOverflowTargetCancellationObserversOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategorySlashTargetsEntries _coreRecipientCategorySlashTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries _coreRoundGainedTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries _coreSameTypeActualUseAidOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries _coreShortRangeSlashTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries _coreShownEntityTurnPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries _coreSoleTargetProtectionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries _coreSoleTargetQuarantineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries _coreSoleTargetRedirectOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries _coreTargetedOrdinaryTrickOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineUniqueLeaderTrickTargetsEntries _coreUniqueLeaderTrickTargetsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    internal CoreGameEngineCompletedCategoryUseProofRuntime(global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterChengYuMouOlEntries characterChengYuMouOlOperations, global::CardGame.Core.RuntimeEntries.CharacterCuiLieAnniversaryEntries characterCuiLieAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterHuaManOlEntries characterHuaManOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuPanOlEntries characterLiuPanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaChengOlEntries characterMaChengOlOperations, global::CardGame.Core.RuntimeEntries.CharacterMaXiuMaTieOlEntries characterMaXiuMaTieOlOperations, global::CardGame.Core.RuntimeEntries.CharacterQianZhaoOlEntries characterQianZhaoOlOperations, global::CardGame.Core.RuntimeEntries.CharacterSunQuanOlMagicEntries characterSunQuanOlMagicOperations, global::CardGame.Core.RuntimeEntries.CharacterXunYuMobileGodEntries characterXunYuMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterYangFuOlEntries characterYangFuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYuanTanYuanShangOlEntries characterYuanTanYuanShangOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAdjacentRedTargetsEntries coreAdjacentRedTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageTargetBudgetEntries coreDamageTargetBudgetOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDesignatedExtraTargetsEntries coreDesignatedExtraTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOverflowTargetCancellationObserversEntries coreOverflowTargetCancellationObserversOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientCategorySlashTargetsEntries coreRecipientCategorySlashTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundGainedTrickTargetsEntries coreRoundGainedTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSameTypeActualUseAidEntries coreSameTypeActualUseAidOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShortRangeSlashTargetsEntries coreShortRangeSlashTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineShownEntityTurnPoliciesEntries coreShownEntityTurnPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetProtectionEntries coreSoleTargetProtectionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetQuarantineEntries coreSoleTargetQuarantineOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSoleTargetRedirectEntries coreSoleTargetRedirectOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTargetedOrdinaryTrickEntries coreTargetedOrdinaryTrickOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineUniqueLeaderTrickTargetsEntries coreUniqueLeaderTrickTargetsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations)
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
