#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramAttackAttemptRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries _characterCheZhouAnniversaryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMobileGodEntries _characterJiangWeiMobileGodOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterLuXunWumiaoEntries _characterLuXunWumiaoOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries _characterZhaoZhongOlOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries _coreActiveNativeAttackQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries _coreDamageCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries _coreDyingCursorRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries _coreDynamicDiscardDamagePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries _coreLateFixedTargetDamageCapScopesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries _corePassiveDamageCapsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries _corePassiveRulePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnerDeathDamageEntries _coreProgramOwnerDeathDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientScopedDamageEntries _coreRecipientScopedDamageOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryDamageEdgesEntries _coreRecoveryDamageEdgesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries _coreRuntimeOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5YimieEntries _coreShanheS5YimieOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSuitDamagePreventionBenefitsEntries _coreSuitDamagePreventionBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries _coreNativeDamageApplicationOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries _corePhysicalDrawFacadesOperations;
    private readonly ICoreProgramAttackAttemptCoreProgramDamageHooksOperations _coreProgramDamageHooksOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCreateProgramAttackHandleOperations _kernelCoreGameEngineProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramAttackAttemptRuntime(global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries characterCheZhouAnniversaryOperations,global::CardGame.Core.RuntimeEntries.CharacterJiangWeiMobileGodEntries characterJiangWeiMobileGodOperations,global::CardGame.Core.RuntimeEntries.CharacterLuXunWumiaoEntries characterLuXunWumiaoOperations,global::CardGame.Core.RuntimeEntries.CharacterZhaoZhongOlEntries characterZhaoZhongOlOperations,global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations,global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations,global::CardGame.Core.RuntimeEntries.CoreActiveNativeAttackQueriesEntries coreActiveNativeAttackQueriesOperations,global::CardGame.Core.RuntimeEntries.CoreDamageCursorRuntimeEntries coreDamageCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreDyingCursorRuntimeEntries coreDyingCursorRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineDynamicDiscardDamagePaymentsEntries coreDynamicDiscardDamagePaymentsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries coreLateFixedTargetDamageCapScopesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries corePassiveDamageCapsOperations,global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries corePassiveRulePoliciesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramOwnerDeathDamageEntries coreProgramOwnerDeathDamageOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecipientScopedDamageEntries coreRecipientScopedDamageOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRecoveryDamageEdgesEntries coreRecoveryDamageEdgesOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineRuntimeEntries coreRuntimeOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineShanheS5YimieEntries coreShanheS5YimieOperations,global::CardGame.Core.RuntimeEntries.CoreGameEngineSuitDamagePreventionBenefitsEntries coreSuitDamagePreventionBenefitsOperations,global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations,global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamageApplicationEntries coreNativeDamageApplicationOperations,global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations,global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations,global::CardGame.Core.RuntimeEntries.CorePhysicalDrawFacadesEntries corePhysicalDrawFacadesOperations,ICoreProgramAttackAttemptCoreProgramDamageHooksOperations coreProgramDamageHooksOperations,global::CardGame.Core.EngineOwners.IKernelCreateProgramAttackHandleOperations kernelCoreGameEngineProgramAttackAttemptOperations,global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess,global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCheZhouAnniversaryOperations = characterCheZhouAnniversaryOperations;
        this._characterJiangWeiMobileGodOperations = characterJiangWeiMobileGodOperations;
        this._characterLuXunWumiaoOperations = characterLuXunWumiaoOperations;
        this._characterZhaoZhongOlOperations = characterZhaoZhongOlOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActiveNativeAttackQueriesOperations = coreActiveNativeAttackQueriesOperations;
        this._coreDamageCursorRuntimeOperations = coreDamageCursorRuntimeOperations;
        this._coreDyingCursorRuntimeOperations = coreDyingCursorRuntimeOperations;
        this._coreDynamicDiscardDamagePaymentsOperations = coreDynamicDiscardDamagePaymentsOperations;
        this._coreLateFixedTargetDamageCapScopesOperations = coreLateFixedTargetDamageCapScopesOperations;
        this._corePassiveDamageCapsOperations = corePassiveDamageCapsOperations;
        this._corePassiveRulePoliciesOperations = corePassiveRulePoliciesOperations;
        this._coreProgramOwnerDeathDamageOperations = coreProgramOwnerDeathDamageOperations;
        this._coreRecipientScopedDamageOperations = coreRecipientScopedDamageOperations;
        this._coreRecoveryDamageEdgesOperations = coreRecoveryDamageEdgesOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreShanheS5YimieOperations = coreShanheS5YimieOperations;
        this._coreSuitDamagePreventionBenefitsOperations = coreSuitDamagePreventionBenefitsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageApplicationOperations = coreNativeDamageApplicationOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._coreProgramDamageHooksOperations = coreProgramDamageHooksOperations;
        this._kernelCoreGameEngineProgramAttackAttemptOperations = kernelCoreGameEngineProgramAttackAttemptOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
