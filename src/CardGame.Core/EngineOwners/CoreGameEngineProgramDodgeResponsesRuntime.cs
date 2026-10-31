#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramDodgeResponsesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterTaishiCiMobileGodEntries _characterTaishiCiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries _characterXiaoQiaoMouOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries _coreDamageObserverCursorQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries _coreAlternativeViewAsCostsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries _coreCardActionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries _coreChainedStateBasicConversionsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries _coreIssuedPlayUsePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries _coreProgramPersistentCombatPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries _coreRoundOnceCardlessNativeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries _coreSkillProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries _coreTieredRoundZeroResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries _coreNativeAttackCompletionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries _coreNativeCardContinuationCursorsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries _coreNativeCardUseFramesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries _coreNativeDodgeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeFactionDefenseEntries _coreNativeFactionDefenseOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries _coreNativePromptBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries _coreNativeResponseMaterialQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries _coreNativeResponseWindowsOperations;
    private readonly ICoreProgramDodgeResponsesCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries _coreResolutionIdentityBoundaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceToHumanBoundaryOperations _kernelCoreGameEngineRuntimeOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess _coreMatchOutcomeStatusAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramDodgeResponsesRuntime(global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, global::CardGame.Core.RuntimeEntries.CharacterTaishiCiMobileGodEntries characterTaishiCiMobileGodOperations, global::CardGame.Core.RuntimeEntries.CharacterXiaoQiaoMouOlEntries characterXiaoQiaoMouOlOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreDamageObserverCursorQueriesEntries coreDamageObserverCursorQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternativeViewAsCostsEntries coreAlternativeViewAsCostsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardActionsEntries coreCardActionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineChainedStateBasicConversionsEntries coreChainedStateBasicConversionsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineIssuedPlayUsePoliciesEntries coreIssuedPlayUsePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries coreProgramPersistentCombatPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRoundOnceCardlessNativeResponsesEntries coreRoundOnceCardlessNativeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSkillProgramsEntries coreSkillProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTieredRoundZeroResponsesEntries coreTieredRoundZeroResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAttackCompletionEntries coreNativeAttackCompletionOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardContinuationCursorsEntries coreNativeCardContinuationCursorsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardUseFramesEntries coreNativeCardUseFramesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDodgeEntries coreNativeDodgeOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeFactionDefenseEntries coreNativeFactionDefenseOperations, global::CardGame.Core.RuntimeEntries.CoreNativePromptBoundaryEntries coreNativePromptBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseMaterialQueriesEntries coreNativeResponseMaterialQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeResponseWindowsEntries coreNativeResponseWindowsOperations, ICoreProgramDodgeResponsesCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreResolutionIdentityBoundaryEntries coreResolutionIdentityBoundaryOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceToHumanBoundaryOperations kernelCoreGameEngineRuntimeOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusAccess coreMatchOutcomeStatusAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._characterTaishiCiMobileGodOperations = characterTaishiCiMobileGodOperations;
        this._characterXiaoQiaoMouOlOperations = characterXiaoQiaoMouOlOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDamageObserverCursorQueriesOperations = coreDamageObserverCursorQueriesOperations;
        this._coreAlternativeViewAsCostsOperations = coreAlternativeViewAsCostsOperations;
        this._coreCardActionsOperations = coreCardActionsOperations;
        this._coreChainedStateBasicConversionsOperations = coreChainedStateBasicConversionsOperations;
        this._coreIssuedPlayUsePoliciesOperations = coreIssuedPlayUsePoliciesOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramPersistentCombatPoliciesOperations = coreProgramPersistentCombatPoliciesOperations;
        this._coreRoundOnceCardlessNativeResponsesOperations = coreRoundOnceCardlessNativeResponsesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTieredRoundZeroResponsesOperations = coreTieredRoundZeroResponsesOperations;
        this._coreNativeAttackCompletionOperations = coreNativeAttackCompletionOperations;
        this._coreNativeCardContinuationCursorsOperations = coreNativeCardContinuationCursorsOperations;
        this._coreNativeCardUseFramesOperations = coreNativeCardUseFramesOperations;
        this._coreNativeDodgeOperations = coreNativeDodgeOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreNativeFactionDefenseOperations = coreNativeFactionDefenseOperations;
        this._coreNativePromptBoundaryOperations = coreNativePromptBoundaryOperations;
        this._coreNativeResponseMaterialQueriesOperations = coreNativeResponseMaterialQueriesOperations;
        this._coreNativeResponseWindowsOperations = coreNativeResponseWindowsOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._kernelCoreGameEngineRuntimeOperations = kernelCoreGameEngineRuntimeOperations;
        this._coreMatchOutcomeStatusAccess = coreMatchOutcomeStatusAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
