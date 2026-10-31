#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreForeignActualTurnWindowRuntimePortsRuntime : global::CardGame.Core.IForeignActualTurnWindowFacts, global::CardGame.Core.IForeignActualTurnWindowReturns
{
    private readonly ICoreForeignActualTurnWindowRuntimePortsCoreActualTurnsOperations _coreActualTurnsOperations;
    private readonly ICoreForeignActualTurnWindowRuntimePortsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreForeignActualTurnWindowRuntimePortsCoreTableOrderOperations _coreTableOrderOperations;
    private readonly ICoreForeignActualTurnWindowRuntimePortsKernelCoreMainOperations _kernelCoreMainOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess _coreMatchOutcomeStatusWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreForeignActualTurnWindowRuntimePortsRuntime(ICoreForeignActualTurnWindowRuntimePortsCoreActualTurnsOperations coreActualTurnsOperations, ICoreForeignActualTurnWindowRuntimePortsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreForeignActualTurnWindowRuntimePortsCoreTableOrderOperations coreTableOrderOperations, ICoreForeignActualTurnWindowRuntimePortsKernelCoreMainOperations kernelCoreMainOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeStatusWinnerAccess coreMatchOutcomeStatusWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreActualTurnsOperations = coreActualTurnsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTableOrderOperations = coreTableOrderOperations;
        this._kernelCoreMainOperations = kernelCoreMainOperations;
        this._coreMatchOutcomeStatusWinnerAccess = coreMatchOutcomeStatusWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
