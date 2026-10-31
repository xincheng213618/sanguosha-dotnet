#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreNativeDamageWindowsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBaoXinOlEntries _characterBaoXinOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries _characterBeiMiHuOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries _characterGuanHaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuLieMouEntries _characterHuLieMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterHuangQuanOlEntries _characterHuangQuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterKanZeAnniversaryEntries _characterKanZeAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries _characterLuSuMouOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterMangYaChangAnniversaryEntries _characterMangYaChangAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaHuiOlEntries _characterSimaHuiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries _characterTaoQianOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiZhiCaiOlEntries _characterXiZhiCaiOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries _characterZhangFenAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries _characterZhangYanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoXiangOlEntries _characterZhaoXiangOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess _programPipelineHooksOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageAdjacencyServicesEntries _coreDamageAdjacencyServicesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries _coreDamageCounterpartAcquisitionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries _coreLastDamageSourceReciprocityOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossEntries _coreOrderedPrintedSkillLossOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries _corePlayCardHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries _coreTableOrderOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _coreMatchDefinitionPlayerCountAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreNativeDamageWindowsRuntime(global::CardGame.Core.RuntimeEntries.CharacterBaoXinOlEntries characterBaoXinOlOperations, global::CardGame.Core.RuntimeEntries.CharacterBeiMiHuOlEntries characterBeiMiHuOlOperations, global::CardGame.Core.RuntimeEntries.CharacterGuanHaiOlEntries characterGuanHaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterHuLieMouEntries characterHuLieMouOperations, global::CardGame.Core.RuntimeEntries.CharacterHuangQuanOlEntries characterHuangQuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterKanZeAnniversaryEntries characterKanZeAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterLuSuMouEntries characterLuSuMouOperations, global::CardGame.Core.RuntimeEntries.CharacterMangYaChangAnniversaryEntries characterMangYaChangAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterSimaHuiOlEntries characterSimaHuiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterTaoQianOlEntries characterTaoQianOlOperations, global::CardGame.Core.RuntimeEntries.CharacterXiZhiCaiOlEntries characterXiZhiCaiOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangFenAnniversaryEntries characterZhangFenAnniversaryOperations, global::CardGame.Core.RuntimeEntries.CharacterZhangYanOlEntries characterZhangYanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterZhaoXiangOlEntries characterZhaoXiangOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelProgramPipelineHooksAccess programPipelineHooksOperations, global::CardGame.Core.RuntimeEntries.CoreDamageAdjacencyServicesEntries coreDamageAdjacencyServicesOperations, global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineDamageCounterpartAcquisitionEntries coreDamageCounterpartAcquisitionOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLastDamageSourceReciprocityEntries coreLastDamageSourceReciprocityOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineOrderedPrintedSkillLossEntries coreOrderedPrintedSkillLossOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePlayCardHistoryEntries corePlayCardHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTableOrderEntries coreTableOrderOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess coreMatchDefinitionPlayerCountAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
