#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries _characterAHuiNanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBaoSanNiangOlEntries _characterBaoSanNiangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries _characterCuiYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuLieMouEntries _characterHuLieMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries _characterHuangYueyingOlMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries _characterTaoQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTianChouOlEntries _characterTianChouOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangXiuMouEntries _characterZhangXiuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouEntries _characterZhouYuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCappedConversionBenefitsEntries _coreCappedConversionBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries _coreRoundOnceCardlessNativeUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    internal CoreGameEngineTieredRoundZeroReturnsRuntime(global::CardGame.Core.RuntimeEntries.CharacterAHuiNanOlEntries characterAHuiNanOlOperations,global::CardGame.Core.RuntimeEntries.CharacterBaoSanNiangOlEntries characterBaoSanNiangOlOperations,global::CardGame.Core.RuntimeEntries.CharacterCuiYanOlEntries characterCuiYanOlOperations,global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations,global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations,global::CardGame.Core.RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations,global::CardGame.Core.RuntimeEntries.CharacterHuLieMouEntries characterHuLieMouOperations,global::CardGame.Core.RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations,global::CardGame.Core.RuntimeEntries.CharacterHuangYueyingOlMouEntries characterHuangYueyingOlMouOperations,global::CardGame.Core.RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations,global::CardGame.Core.RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations,global::CardGame.Core.RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations,global::CardGame.Core.RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations,global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries characterTaoQianOlOperations,global::CardGame.Core.RuntimeEntries.CharacterTianChouOlEntries characterTianChouOlOperations,global::CardGame.Core.RuntimeEntries.CharacterZhangXiuMouEntries characterZhangXiuMouOperations,global::CardGame.Core.RuntimeEntries.CharacterZhouYuMouEntries characterZhouYuMouOperations,global::CardGame.Core.RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCappedConversionBenefitsEntries coreCappedConversionBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries coreRoundOnceCardlessNativeUsesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations,global::CardGame.Core.RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations)
    {
        this._characterAHuiNanOlOperations = characterAHuiNanOlOperations;
        this._characterBaoSanNiangOlOperations = characterBaoSanNiangOlOperations;
        this._characterCuiYanOlOperations = characterCuiYanOlOperations;
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterHuCheErMouOperations = characterHuCheErMouOperations;
        this._characterHuLieMouOperations = characterHuLieMouOperations;
        this._characterHuanShuJunOlOperations = characterHuanShuJunOlOperations;
        this._characterHuangYueyingOlMouOperations = characterHuangYueyingOlMouOperations;
        this._characterLiuYanOlOperations = characterLiuYanOlOperations;
        this._characterLiuZhangOlOperations = characterLiuZhangOlOperations;
        this._characterLuKangWumiaoOperations = characterLuKangWumiaoOperations;
        this._characterQinLangOlOperations = characterQinLangOlOperations;
        this._characterTaoQianOlOperations = characterTaoQianOlOperations;
        this._characterTianChouOlOperations = characterTianChouOlOperations;
        this._characterZhangXiuMouOperations = characterZhangXiuMouOperations;
        this._characterZhouYuMouOperations = characterZhouYuMouOperations;
        this._characterZhugeLiangMouOperations = characterZhugeLiangMouOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCappedConversionBenefitsOperations = coreCappedConversionBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreCardUseLifecycleOperations = coreCardUseLifecycleOperations;
        this._coreRoundOnceCardlessNativeUsesOperations = coreRoundOnceCardlessNativeUsesOperations;
        this._coreTieredRoundZeroUsesOperations = coreTieredRoundZeroUsesOperations;
        this._coreParticipantHandDamageRidesOperations = coreParticipantHandDamageRidesOperations;
    }
}
