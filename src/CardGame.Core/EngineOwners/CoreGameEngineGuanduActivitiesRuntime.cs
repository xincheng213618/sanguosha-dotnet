#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineGuanduActivitiesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries _coreActivityBattleRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries _coreProgramEventHistoryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess _coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess;
    internal CoreGameEngineGuanduActivitiesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineActivityBattleRulesEntries coreActivityBattleRulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramEventHistoryEntries coreProgramEventHistoryOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess)
    {
        this._coreActivityBattleRulesOperations = coreActivityBattleRulesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
        this._coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess = coreGameEngineGuanduActivitiesGuanduInitialConfigurationGuanduQualificationDependenciesGuanduQualificationStampAccess;
    }
}
