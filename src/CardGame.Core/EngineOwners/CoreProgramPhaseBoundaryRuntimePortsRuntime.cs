#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramPhaseBoundaryRuntimePortsRuntime : global::CardGame.Core.IProgramPhaseBoundaryFacts, global::CardGame.Core.IProgramPhaseBoundaryProtocols
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiWanOlEntries _characterLiWanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLiuBaOlEntries _characterLiuBaOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries _characterLuZhiWeiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries _characterRuanYuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterWenYangOlEntries _characterWenYangOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries _characterZhangJiaoAnniversaryGodOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineConvertingGiftsEntries _coreConvertingGiftsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries _coreEarnedActualEndingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries _coreFixedDistanceOneDebtOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNeighborDiscardOpportunitiesEntries _coreNeighborDiscardOpportunitiesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries _corePrepDiscardEndingPromisesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreWenheActivityProgramsEntries _coreWenheActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    internal CoreProgramPhaseBoundaryRuntimePortsRuntime(global::CardGame.Core.RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiWanOlEntries characterLiWanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLiuBaOlEntries characterLiuBaOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLuZhiWeiOlEntries characterLuZhiWeiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterRuanYuOlEntries characterRuanYuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterWenYangOlEntries characterWenYangOlOperations, global::CardGame.Core.RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries characterZhangJiaoAnniversaryGodOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineConvertingGiftsEntries coreConvertingGiftsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries coreEarnedActualEndingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries coreFixedDistanceOneDebtOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNeighborDiscardOpportunitiesEntries coreNeighborDiscardOpportunitiesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries corePrepDiscardEndingPromisesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, global::CardGame.Core.RuntimeEntries.CoreWenheActivityProgramsEntries coreWenheActivityProgramsOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelCompleteGameOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess)
    {
        this._characterGuoTuOlOperations = characterGuoTuOlOperations;
        this._characterLiWanOlOperations = characterLiWanOlOperations;
        this._characterLiuBaOlOperations = characterLiuBaOlOperations;
        this._characterLuZhiWeiOlOperations = characterLuZhiWeiOlOperations;
        this._characterLvQianOlOperations = characterLvQianOlOperations;
        this._characterRuanYuOlOperations = characterRuanYuOlOperations;
        this._characterWenYangOlOperations = characterWenYangOlOperations;
        this._characterYanPuOlOperations = characterYanPuOlOperations;
        this._characterZhangJiaoAnniversaryGodOperations = characterZhangJiaoAnniversaryGodOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreConvertingGiftsOperations = coreConvertingGiftsOperations;
        this._coreEarnedActualEndingBenefitsOperations = coreEarnedActualEndingBenefitsOperations;
        this._coreFixedDistanceOneDebtOperations = coreFixedDistanceOneDebtOperations;
        this._coreNeighborDiscardOpportunitiesOperations = coreNeighborDiscardOpportunitiesOperations;
        this._corePrepDiscardEndingPromisesOperations = corePrepDiscardEndingPromisesOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._coreTreatedWoundedOperations = coreTreatedWoundedOperations;
        this._coreWenheActivityProgramsOperations = coreWenheActivityProgramsOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
    }
}
