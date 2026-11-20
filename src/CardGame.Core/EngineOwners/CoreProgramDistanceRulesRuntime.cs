#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule, Core.ProgramCapability]
internal sealed partial class CoreProgramDistanceRulesRuntime(
    IKernelDistanceRulesAccess _distanceRulesOperations,
    IKernelBindRuntimeHandlerOperations _kernelCoreProgramRuntimeModulesOperations,
    IProgramContentQueries _content,
    ICardZoneQueries _zones,
    IProgramEventHistoryQueries _history,
    RuntimeEntries.CoreGameEngineNationalSkillsEntries _nationalSkills,
    EngineStates.Contracts.ICoreActualRoundProgressRoundNumberAccess _round,
    EngineStates.Contracts.ICoreMatchDefinitionPlayerCountAccess _matchDefinition,
    EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _participants) : IDistanceRuleRuntimeQueries
{
}
