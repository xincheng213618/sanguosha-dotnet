#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramDrawTwoBonusRuntime
{
    private readonly ICoreProgramDrawTwoBonusContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramDrawTwoBonusCoreNativeDrawAttemptsOperations _coreNativeDrawAttemptsOperations;
    private readonly ICoreProgramDrawTwoBonusCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreProgramDrawTwoBonusCoreWushuangCombatTacticsOperations _coreWushuangCombatTacticsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineProgramDrawTwoBonusRuntime(ICoreProgramDrawTwoBonusContentRegistryOperations contentRegistryOperations, ICoreProgramDrawTwoBonusCoreNativeDrawAttemptsOperations coreNativeDrawAttemptsOperations, ICoreProgramDrawTwoBonusCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreProgramDrawTwoBonusCoreWushuangCombatTacticsOperations coreWushuangCombatTacticsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeDrawAttemptsOperations = coreNativeDrawAttemptsOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreWushuangCombatTacticsOperations = coreWushuangCombatTacticsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
