#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerDecisionProjectionRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPromptsEntries _coreProgramParticipantPromptsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CorePlayerDecisionProjectionRuntime(global::CardGame.Core.RuntimeEntries.CoreProgramParticipantPromptsEntries coreProgramParticipantPromptsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._coreProgramParticipantPromptsOperations = coreProgramParticipantPromptsOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
