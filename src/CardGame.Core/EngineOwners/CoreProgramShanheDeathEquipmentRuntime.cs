#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramShanheDeathEquipmentRuntime : global::CardGame.Core.IShanheDeathEquipmentProgramHost
{
    private readonly ICoreProgramShanheDeathEquipmentCoreAdvancedLifecycleOperations _coreAdvancedLifecycleOperations;
    private readonly ICoreProgramShanheDeathEquipmentCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramShanheDeathEquipmentCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramShanheDeathEquipmentRuntime(ICoreProgramShanheDeathEquipmentCoreAdvancedLifecycleOperations coreAdvancedLifecycleOperations, ICoreProgramShanheDeathEquipmentCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramShanheDeathEquipmentCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._coreAdvancedLifecycleOperations = coreAdvancedLifecycleOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
