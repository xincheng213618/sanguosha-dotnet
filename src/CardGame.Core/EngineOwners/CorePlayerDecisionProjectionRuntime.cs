#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CorePlayerDecisionProjectionRuntime
{
    private readonly RuntimeEntries.CoreProgramParticipantPromptsEntries _coreProgramParticipantPromptsOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess _coreMatchDefinitionOptionsAccess;
    internal CorePlayerDecisionProjectionRuntime(RuntimeEntries.CoreProgramParticipantPromptsEntries coreProgramParticipantPromptsOperations, EngineStates.Contracts.ICoreMatchDefinitionOptionsAccess coreMatchDefinitionOptionsAccess)
    {
        this._coreProgramParticipantPromptsOperations = coreProgramParticipantPromptsOperations;
        this._coreMatchDefinitionOptionsAccess = coreMatchDefinitionOptionsAccess;
    }
}
