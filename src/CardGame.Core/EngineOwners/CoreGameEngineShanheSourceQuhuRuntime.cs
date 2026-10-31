#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheSourceQuhuRuntime : global::CardGame.Core.IShanheSourceQuhuHost
{
    private readonly ICoreShanheSourceQuhuCoreProgramCardPaymentsOperations _coreProgramCardPaymentsOperations;
    private readonly ICoreShanheSourceQuhuCoreProgramDodgeResponsesOperations _coreProgramDodgeResponsesOperations;
    private readonly ICoreShanheSourceQuhuCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreShanheSourceQuhuCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheSourceQuhuCoreSkillProgramsOperations _coreSkillProgramsOperations;
    private readonly ICoreShanheSourceQuhuCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheSourceQuhuRuntime(ICoreShanheSourceQuhuCoreProgramCardPaymentsOperations coreProgramCardPaymentsOperations, ICoreShanheSourceQuhuCoreProgramDodgeResponsesOperations coreProgramDodgeResponsesOperations, ICoreShanheSourceQuhuCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreShanheSourceQuhuCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheSourceQuhuCoreSkillProgramsOperations coreSkillProgramsOperations, ICoreShanheSourceQuhuCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreProgramCardPaymentsOperations = coreProgramCardPaymentsOperations;
        this._coreProgramDodgeResponsesOperations = coreProgramDodgeResponsesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreSkillProgramsOperations = coreSkillProgramsOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
