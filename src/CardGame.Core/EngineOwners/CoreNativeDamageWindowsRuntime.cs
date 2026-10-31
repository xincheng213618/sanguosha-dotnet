#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageWindowsRuntime
{
    private readonly RuntimeEntries.CharacterBaoXinOlEntries _characterBaoXinOlOperations;
    private readonly RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly RuntimeEntries.CharacterHuLieMouEntries _characterHuLieMouOperations;
    private readonly RuntimeEntries.CharacterHuangQuanOlEntries _characterHuangQuanOlOperations;
    private readonly RuntimeEntries.CharacterKanZeAnniversaryEntries _characterKanZeAnniversaryOperations;
    private readonly RuntimeEntries.CharacterLuSuMouEntries _characterLuSuMouOperations;
    private readonly RuntimeEntries.CharacterMangYaChangAnniversaryEntries _characterMangYaChangAnniversaryOperations;
    private readonly RuntimeEntries.CharacterSimaHuiOlEntries _characterSimaHuiOlOperations;
    private readonly RuntimeEntries.CharacterTaoQianOlEntries _characterTaoQianOlOperations;
    private readonly RuntimeEntries.CharacterXiZhiCaiOlEntries _characterXiZhiCaiOlOperations;
    private readonly RuntimeEntries.CharacterZhangFenAnniversaryEntries _characterZhangFenAnniversaryOperations;
    private readonly RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly RuntimeEntries.CharacterZhaoXiangOlEntries _characterZhaoXiangOlOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly RuntimeEntries.CoreDamageAdjacencyServicesEntries _coreDamageAdjacencyServicesOperations;
    private readonly RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries _coreDamageCounterpartAcquisitionOperations;
    private readonly RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries _coreLastDamageSourceReciprocityOperations;
    private readonly RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossEntries _coreOrderedPrintedSkillLossOperations;
    private readonly RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeDamageWindowsRuntime(RuntimeEntries.CharacterBaoXinOlEntries characterBaoXinOlOperations, RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, RuntimeEntries.CharacterHuLieMouEntries characterHuLieMouOperations, RuntimeEntries.CharacterHuangQuanOlEntries characterHuangQuanOlOperations, RuntimeEntries.CharacterKanZeAnniversaryEntries characterKanZeAnniversaryOperations, RuntimeEntries.CharacterLuSuMouEntries characterLuSuMouOperations, RuntimeEntries.CharacterMangYaChangAnniversaryEntries characterMangYaChangAnniversaryOperations, RuntimeEntries.CharacterSimaHuiOlEntries characterSimaHuiOlOperations, RuntimeEntries.CharacterTaoQianOlEntries characterTaoQianOlOperations, RuntimeEntries.CharacterXiZhiCaiOlEntries characterXiZhiCaiOlOperations, RuntimeEntries.CharacterZhangFenAnniversaryEntries characterZhangFenAnniversaryOperations, RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, RuntimeEntries.CharacterZhaoXiangOlEntries characterZhaoXiangOlOperations, IKernelContentRegistryAccess contentRegistryOperations, IKernelProgramPipelineHooksAccess programPipelineHooksOperations, RuntimeEntries.CoreDamageAdjacencyServicesEntries coreDamageAdjacencyServicesOperations, RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries coreDamageCounterpartAcquisitionOperations, RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries coreLastDamageSourceReciprocityOperations, RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossEntries coreOrderedPrintedSkillLossOperations, RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterBaoXinOlOperations = characterBaoXinOlOperations;
        this._characterBeiMiHuOlOperations = characterBeiMiHuOlOperations;
        this._characterGuanHaiOlOperations = characterGuanHaiOlOperations;
        this._characterHuLieMouOperations = characterHuLieMouOperations;
        this._characterHuangQuanOlOperations = characterHuangQuanOlOperations;
        this._characterKanZeAnniversaryOperations = characterKanZeAnniversaryOperations;
        this._characterLuSuMouOperations = characterLuSuMouOperations;
        this._characterMangYaChangAnniversaryOperations = characterMangYaChangAnniversaryOperations;
        this._characterSimaHuiOlOperations = characterSimaHuiOlOperations;
        this._characterTaoQianOlOperations = characterTaoQianOlOperations;
        this._characterXiZhiCaiOlOperations = characterXiZhiCaiOlOperations;
        this._characterZhangFenAnniversaryOperations = characterZhangFenAnniversaryOperations;
        this._characterZhangYanOlOperations = characterZhangYanOlOperations;
        this._characterZhaoXiangOlOperations = characterZhaoXiangOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._coreDamageAdjacencyServicesOperations = coreDamageAdjacencyServicesOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDamageCounterpartAcquisitionOperations = coreDamageCounterpartAcquisitionOperations;
        this._coreLastDamageSourceReciprocityOperations = coreLastDamageSourceReciprocityOperations;
        this._coreOrderedPrintedSkillLossOperations = coreOrderedPrintedSkillLossOperations;
        this._corePlayCardHistoryOperations = corePlayCardHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchDefinitionPlayerCountAccess = coreMatchDefinitionPlayerCountAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
