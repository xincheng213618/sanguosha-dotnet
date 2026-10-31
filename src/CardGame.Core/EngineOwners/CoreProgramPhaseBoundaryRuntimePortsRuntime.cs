#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramPhaseBoundaryRuntimePortsRuntime : IProgramPhaseBoundaryFacts, IProgramPhaseBoundaryProtocols
{
    private readonly RuntimeEntries.CharacterGuoTuOlEntries _characterGuoTuOlOperations;
    private readonly RuntimeEntries.CharacterLiWanOlEntries _characterLiWanOlOperations;
    private readonly RuntimeEntries.CharacterLiuBaOlEntries _characterLiuBaOlOperations;
    private readonly RuntimeEntries.CharacterLuZhiWeiOlEntries _characterLuZhiWeiOlOperations;
    private readonly RuntimeEntries.CharacterLvQianOlEntries _characterLvQianOlOperations;
    private readonly RuntimeEntries.CharacterRuanYuOlEntries _characterRuanYuOlOperations;
    private readonly RuntimeEntries.CharacterWenYangOlEntries _characterWenYangOlOperations;
    private readonly RuntimeEntries.CharacterYanPuOlEntries _characterYanPuOlOperations;
    private readonly RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries _characterZhangJiaoAnniversaryGodOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreActualTurnsEntries _coreActualTurnsOperations;
    private readonly RuntimeEntries.CoreGameEngineConvertingGiftsEntries _coreConvertingGiftsOperations;
    private readonly RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries _coreEarnedActualEndingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries _coreFixedDistanceOneDebtOperations;
    private readonly RuntimeEntries.CoreGameEngineNeighborDiscardOpportunitiesEntries _coreNeighborDiscardOpportunitiesOperations;
    private readonly RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries _corePrepDiscardEndingPromisesOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CoreGameEngineTreatedWoundedEntries _coreTreatedWoundedOperations;
    private readonly RuntimeEntries.CoreWenheActivityProgramsEntries _coreWenheActivityProgramsOperations;
    private readonly IKernelAdvanceRulesAndPublishStateOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly IKernelCompleteGameOperations _kernelCoreMainOperations;
    private readonly EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    internal CoreProgramPhaseBoundaryRuntimePortsRuntime(RuntimeEntries.CharacterGuoTuOlEntries characterGuoTuOlOperations, RuntimeEntries.CharacterLiWanOlEntries characterLiWanOlOperations, RuntimeEntries.CharacterLiuBaOlEntries characterLiuBaOlOperations, RuntimeEntries.CharacterLuZhiWeiOlEntries characterLuZhiWeiOlOperations, RuntimeEntries.CharacterLvQianOlEntries characterLvQianOlOperations, RuntimeEntries.CharacterRuanYuOlEntries characterRuanYuOlOperations, RuntimeEntries.CharacterWenYangOlEntries characterWenYangOlOperations, RuntimeEntries.CharacterYanPuOlEntries characterYanPuOlOperations, RuntimeEntries.CharacterZhangJiaoAnniversaryGodEntries characterZhangJiaoAnniversaryGodOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreActualTurnsEntries coreActualTurnsOperations, RuntimeEntries.CoreGameEngineConvertingGiftsEntries coreConvertingGiftsOperations, RuntimeEntries.CoreGameEngineEarnedActualEndingBenefitsEntries coreEarnedActualEndingBenefitsOperations, RuntimeEntries.CoreGameEngineFixedDistanceOneDebtEntries coreFixedDistanceOneDebtOperations, RuntimeEntries.CoreGameEngineNeighborDiscardOpportunitiesEntries coreNeighborDiscardOpportunitiesOperations, RuntimeEntries.CoreGameEnginePrepDiscardEndingPromisesEntries corePrepDiscardEndingPromisesOperations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, RuntimeEntries.CoreGameEngineTreatedWoundedEntries coreTreatedWoundedOperations, RuntimeEntries.CoreWenheActivityProgramsEntries coreWenheActivityProgramsOperations, IKernelAdvanceRulesAndPublishStateOperations kernelCoreGameEngineRuleProgressionOperations, IKernelCompleteGameOperations kernelCoreMainOperations, EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess)
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
