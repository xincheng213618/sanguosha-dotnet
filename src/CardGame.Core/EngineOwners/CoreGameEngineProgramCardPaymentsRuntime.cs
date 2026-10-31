#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramCardPaymentsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries _characterSunYuanOlOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries _coreActionQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries _coreProgramDodgeResponsesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries _coreProgramPersistentCombatPoliciesOperations;
    private readonly ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations _coreNativeRuleQueriesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreProgramCardPaymentsProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramCardPaymentsRuntime(global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries characterSunYuanOlOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActionQueriesEntries coreActionQueriesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramDodgeResponsesEntries coreProgramDodgeResponsesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentCombatPoliciesEntries coreProgramPersistentCombatPoliciesOperations, ICoreProgramCardPaymentsCoreNativeRuleQueriesOperations coreNativeRuleQueriesOperations, global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries corePhysicalOwnedCardQueriesOperations, ICoreProgramCardPaymentsProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
