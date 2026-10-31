#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramBooleanStateExpirationRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries _coreCardUseModulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries _coreProgramPersistentStateOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess _coreActualTurnProgressCurrentSeatTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess _coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess;
    internal CoreGameEngineProgramBooleanStateExpirationRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineCardUseModulesEntries coreCardUseModulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramPersistentStateEntries coreProgramPersistentStateOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatTurnNumberAccess coreActualTurnProgressCurrentSeatTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess)
    {
        this._coreCardUseModulesOperations = coreCardUseModulesOperations;
        this._coreProgramPersistentStateOperations = coreProgramPersistentStateOperations;
        this._coreActualTurnProgressCurrentSeatTurnNumberAccess = coreActualTurnProgressCurrentSeatTurnNumberAccess;
        this._coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess = coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess;
    }
}
