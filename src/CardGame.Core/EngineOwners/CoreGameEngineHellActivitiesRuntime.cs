#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHellActivitiesRuntime
{
    private readonly ICoreHellActivitiesResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreHellActivitiesCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICoreHellActivitiesCoreHellEncounterHostOperations _coreHellEncounterHostOperations;
    private readonly ICoreHellActivitiesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    private readonly ICoreHellActivitiesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreHellActivitiesCoreHellMengpoProgramsOperations _coreHellMengpoProgramsOperations;
    private readonly ICoreHellActivitiesCoreNativeEquipmentAndFactionQueriesOperations _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineHellActivitiesRuntime(ICoreHellActivitiesResolutionStackOperations resolutionStackOperations, ICoreHellActivitiesCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICoreHellActivitiesCoreHellEncounterHostOperations coreHellEncounterHostOperations, ICoreHellActivitiesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations, ICoreHellActivitiesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreHellActivitiesCoreHellMengpoProgramsOperations coreHellMengpoProgramsOperations, ICoreHellActivitiesCoreNativeEquipmentAndFactionQueriesOperations coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreHellEncounterHostOperations = coreHellEncounterHostOperations;
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreHellMengpoProgramsOperations = coreHellMengpoProgramsOperations;
        this._coreNativeEquipmentAndFactionQueriesOperations = coreNativeEquipmentAndFactionQueriesOperations;
        this._coreActualTurnProgressCurrentSeatPhaseAccess = coreActualTurnProgressCurrentSeatPhaseAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
