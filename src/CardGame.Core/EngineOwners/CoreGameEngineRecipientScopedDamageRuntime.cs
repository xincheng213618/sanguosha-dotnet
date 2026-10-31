#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecipientScopedDamageRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries _characterCheZhouAnniversaryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries _coreLateFixedTargetDamageCapScopesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries _corePassiveDamageCapsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries _corePassiveRulePoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries _coreProgramVirtualDuelOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineSignedDamagePaymentsEntries _coreSignedDamagePaymentsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRecipientScopedDamageRuntime(global::CardGame.Core.RuntimeEntries.CharacterCheZhouAnniversaryEntries characterCheZhouAnniversaryOperations, global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelMatchModeRulesAccess modeRulesOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries coreLateFixedTargetDamageCapScopesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries corePassiveDamageCapsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries corePassiveRulePoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries coreProgramVirtualDuelOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineSignedDamagePaymentsEntries coreSignedDamagePaymentsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCheZhouAnniversaryOperations = characterCheZhouAnniversaryOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardAttackStateOperations = coreCardAttackStateOperations;
        this._coreLateFixedTargetDamageCapScopesOperations = coreLateFixedTargetDamageCapScopesOperations;
        this._corePassiveDamageCapsOperations = corePassiveDamageCapsOperations;
        this._corePassiveRulePoliciesOperations = corePassiveRulePoliciesOperations;
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreProgramVirtualDuelOperations = coreProgramVirtualDuelOperations;
        this._coreSignedDamagePaymentsOperations = coreSignedDamagePaymentsOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
