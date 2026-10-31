#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreProgramParticipantPromptsRuntime
{
    private readonly ICoreProgramParticipantPromptsProgramPipelineHooksOperations _programPipelineHooksOperations;
    private readonly ICoreProgramParticipantPromptsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreProgramParticipantPromptsCorePendingDecisionPolicyPipelineOperations _corePendingDecisionPolicyPipelineOperations;
    private readonly ICoreProgramParticipantPromptsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramParticipantPromptsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramParticipantPromptsRuntime(ICoreProgramParticipantPromptsProgramPipelineHooksOperations programPipelineHooksOperations, ICoreProgramParticipantPromptsResolutionStackOperations resolutionStackOperations, ICoreProgramParticipantPromptsCorePendingDecisionPolicyPipelineOperations corePendingDecisionPolicyPipelineOperations, ICoreProgramParticipantPromptsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramParticipantPromptsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._programPipelineHooksOperations = programPipelineHooksOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
