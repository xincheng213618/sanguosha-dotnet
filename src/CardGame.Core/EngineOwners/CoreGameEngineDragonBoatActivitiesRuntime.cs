#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDragonBoatActivitiesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineDragonBoatActivitiesRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
