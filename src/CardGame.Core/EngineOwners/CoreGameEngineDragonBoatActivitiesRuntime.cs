#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDragonBoatActivitiesRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    private readonly EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDragonBoatActivitiesRuntime(IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations, EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
