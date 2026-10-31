#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTieredRoundZeroReturnsRuntime
{
    private readonly RuntimeEntries.CharacterAHuiNanOlEntries _characterAHuiNanOlOperations;
    private readonly RuntimeEntries.CharacterBaoSanNiangOlEntries _characterBaoSanNiangOlOperations;
    private readonly RuntimeEntries.CharacterCuiYanOlEntries _characterCuiYanOlOperations;
    private readonly RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly RuntimeEntries.CharacterHuCheErMouEntries _characterHuCheErMouOperations;
    private readonly RuntimeEntries.CharacterHuLieMouEntries _characterHuLieMouOperations;
    private readonly RuntimeEntries.CharacterHuanShuJunOlEntries _characterHuanShuJunOlOperations;
    private readonly RuntimeEntries.CharacterHuangYueyingOlMouEntries _characterHuangYueyingOlMouOperations;
    private readonly RuntimeEntries.CharacterLiuYanOlEntries _characterLiuYanOlOperations;
    private readonly RuntimeEntries.CharacterLiuZhangOlEntries _characterLiuZhangOlOperations;
    private readonly RuntimeEntries.CharacterLuKangWumiaoEntries _characterLuKangWumiaoOperations;
    private readonly RuntimeEntries.CharacterQinLangOlEntries _characterQinLangOlOperations;
    private readonly RuntimeEntries.CharacterTaoQianOlEntries _characterTaoQianOlOperations;
    private readonly RuntimeEntries.CharacterTianChouOlEntries _characterTianChouOlOperations;
    private readonly RuntimeEntries.CharacterZhangXiuMouEntries _characterZhangXiuMouOperations;
    private readonly RuntimeEntries.CharacterZhouYuMouEntries _characterZhouYuMouOperations;
    private readonly RuntimeEntries.CharacterZhugeLiangMouEntries _characterZhugeLiangMouOperations;
    private readonly IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCappedConversionBenefitsEntries _coreCappedConversionBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineCardUseLifecycleEntries _coreCardUseLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries _coreRoundOnceCardlessNativeUsesOperations;
    private readonly RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries _coreTieredRoundZeroUsesOperations;
    private readonly RuntimeEntries.CoreParticipantHandDamageRidesEntries _coreParticipantHandDamageRidesOperations;
    internal CoreGameEngineTieredRoundZeroReturnsRuntime(RuntimeEntries.CharacterAHuiNanOlEntries characterAHuiNanOlOperations,RuntimeEntries.CharacterBaoSanNiangOlEntries characterBaoSanNiangOlOperations,RuntimeEntries.CharacterCuiYanOlEntries characterCuiYanOlOperations,RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations,RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations,RuntimeEntries.CharacterHuCheErMouEntries characterHuCheErMouOperations,RuntimeEntries.CharacterHuLieMouEntries characterHuLieMouOperations,RuntimeEntries.CharacterHuanShuJunOlEntries characterHuanShuJunOlOperations,RuntimeEntries.CharacterHuangYueyingOlMouEntries characterHuangYueyingOlMouOperations,RuntimeEntries.CharacterLiuYanOlEntries characterLiuYanOlOperations,RuntimeEntries.CharacterLiuZhangOlEntries characterLiuZhangOlOperations,RuntimeEntries.CharacterLuKangWumiaoEntries characterLuKangWumiaoOperations,RuntimeEntries.CharacterQinLangOlEntries characterQinLangOlOperations,RuntimeEntries.CharacterTaoQianOlEntries characterTaoQianOlOperations,RuntimeEntries.CharacterTianChouOlEntries characterTianChouOlOperations,RuntimeEntries.CharacterZhangXiuMouEntries characterZhangXiuMouOperations,RuntimeEntries.CharacterZhouYuMouEntries characterZhouYuMouOperations,RuntimeEntries.CharacterZhugeLiangMouEntries characterZhugeLiangMouOperations,IKernelFrameStoreAccess resolutionStackOperations,RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,RuntimeEntries.CoreGameEngineCappedConversionBenefitsEntries coreCappedConversionBenefitsOperations,RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,RuntimeEntries.CoreGameEngineCardUseLifecycleEntries coreCardUseLifecycleOperations,RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeUsesEntries coreRoundOnceCardlessNativeUsesOperations,RuntimeEntries.CoreGameEngineTieredRoundZeroUsesEntries coreTieredRoundZeroUsesOperations,RuntimeEntries.CoreParticipantHandDamageRidesEntries coreParticipantHandDamageRidesOperations)
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
