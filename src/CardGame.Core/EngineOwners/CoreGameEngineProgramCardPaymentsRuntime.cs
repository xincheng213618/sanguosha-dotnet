#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardPaymentsRuntime
{
    private readonly ICoreProgramCardPaymentsCharacterSunYuanOlOperations _characterSunYuanOlOperations;
    private readonly ICoreProgramCardPaymentsCoreActionQueriesOperations _coreActionQueriesOperations;
    private readonly ICoreProgramCardPaymentsCoreProgramDodgeResponsesOperations _coreProgramDodgeResponsesOperations;
    private readonly ICoreProgramCardPaymentsCoreProgramPersistentCombatPoliciesOperations _coreProgramPersistentCombatPoliciesOperations;
    private readonly ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly ICoreProgramCardPaymentsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramCardPaymentsProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardPaymentsRuntime(ICoreProgramCardPaymentsCharacterSunYuanOlOperations characterSunYuanOlOperations, ICoreProgramCardPaymentsCoreActionQueriesOperations coreActionQueriesOperations, ICoreProgramCardPaymentsCoreProgramDodgeResponsesOperations coreProgramDodgeResponsesOperations, ICoreProgramCardPaymentsCoreProgramPersistentCombatPoliciesOperations coreProgramPersistentCombatPoliciesOperations, ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, ICoreProgramCardPaymentsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreProgramCardPaymentsProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._characterSunYuanOlOperations = characterSunYuanOlOperations;
        this._coreActionQueriesOperations = coreActionQueriesOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramPersistentCombatPoliciesOperations = coreProgramPersistentCombatPoliciesOperations;
        this._coreNativeRuleQueriesOperations = coreNativeRuleQueriesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
