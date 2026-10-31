#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramLoseDeathSourceSkillsOperationRuntime
{
    private readonly ICoreProgramLoseDeathSourceSkillsOperationCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramLoseDeathSourceSkillsOperationCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramLoseDeathSourceSkillsOperationKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramLoseDeathSourceSkillsOperationRuntime(ICoreProgramLoseDeathSourceSkillsOperationCoreMatchLogOperations coreMatchLogOperations, ICoreProgramLoseDeathSourceSkillsOperationCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramLoseDeathSourceSkillsOperationKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
