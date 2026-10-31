#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHellActivitiesRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries _coreActualDiscardRecoveryPhasesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineHellEncounterHostEntries _coreHellEncounterHostOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries _coreProgramCardPoliciesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries _coreHellMengpoProgramsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries _coreNativeEquipmentAndFactionQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess _coreActualTurnProgressCurrentSeatPhaseAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineHellActivitiesRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineActualDiscardRecoveryPhasesEntries coreActualDiscardRecoveryPhasesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineHellEncounterHostEntries coreHellEncounterHostOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardPoliciesEntries coreProgramCardPoliciesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreHellMengpoProgramsEntries coreHellMengpoProgramsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries coreNativeEquipmentAndFactionQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseAccess coreActualTurnProgressCurrentSeatPhaseAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
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
