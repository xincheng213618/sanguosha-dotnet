#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineRecipientScopedDamageRuntime
{
    private readonly RuntimeEntries.CharacterCheZhouAnniversaryEntries _characterCheZhouAnniversaryOperations;
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly IKernelMatchModeRulesAccess _modeRulesOperations;
    private readonly RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly RuntimeEntries.CoreGameEngineCardAttackStateEntries _coreCardAttackStateOperations;
    private readonly RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries _coreLateFixedTargetDamageCapScopesOperations;
    private readonly RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries _corePassiveDamageCapsOperations;
    private readonly RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries _corePassiveRulePoliciesOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries _coreProgramVirtualDuelOperations;
    private readonly RuntimeEntries.CoreGameEngineSignedDamagePaymentsEntries _coreSignedDamagePaymentsOperations;
    private readonly RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineRecipientScopedDamageRuntime(RuntimeEntries.CharacterCheZhouAnniversaryEntries characterCheZhouAnniversaryOperations,IKernelContentRegistryAccess contentRegistryOperations,IKernelMatchModeRulesAccess modeRulesOperations,RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations,RuntimeEntries.CoreGameEngineCardAttackStateEntries coreCardAttackStateOperations,RuntimeEntries.CoreGameEngineLateFixedTargetDamageCapScopesEntries coreLateFixedTargetDamageCapScopesOperations,RuntimeEntries.CoreGameEnginePassiveDamageCapsEntries corePassiveDamageCapsOperations,RuntimeEntries.CoreGameEnginePassiveRulePoliciesEntries corePassiveRulePoliciesOperations,RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations,RuntimeEntries.CoreGameEngineProgramVirtualDuelEntries coreProgramVirtualDuelOperations,RuntimeEntries.CoreGameEngineSignedDamagePaymentsEntries coreSignedDamagePaymentsOperations,RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations,RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations,IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations,EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterCheZhouAnniversaryOperations = characterCheZhouAnniversaryOperations;
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
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
