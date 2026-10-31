#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramPopulationOperationsRuntime
{
    private readonly ICoreProgramPopulationOperationsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramPopulationOperationsCoreRuleQueriesOperations _coreRuleQueriesOperations;
    private readonly ICoreProgramPopulationOperationsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramPopulationOperationsRuntime(ICoreProgramPopulationOperationsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramPopulationOperationsCoreRuleQueriesOperations coreRuleQueriesOperations, ICoreProgramPopulationOperationsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
