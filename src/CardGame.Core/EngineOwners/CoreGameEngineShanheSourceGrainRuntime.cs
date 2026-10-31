#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheSourceGrainRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries _coreProgramAttackAttemptOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    internal CoreGameEngineShanheSourceGrainRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries coreProgramAttackAttemptOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess)
    {
        this._coreProgramAttackAttemptOperations = coreProgramAttackAttemptOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
    }
}
