#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreHellActivityProgramsRuntime : global::CardGame.Core.IHellActivityProgramHost
{
    private readonly ICoreHellActivityProgramsCardZonesOperations _cardZonesOperations;
    private readonly ICoreHellActivityProgramsCoreHellEncounterHostOperations _coreHellEncounterHostOperations;
    private readonly ICoreHellActivityProgramsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreHellActivityProgramsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreHellActivityProgramsCoreProgramNativeDrawOperations _coreProgramNativeDrawOperations;
    private readonly ICoreHellActivityProgramsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreHellActivityProgramsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreHellActivityProgramsCorePhysicalOwnedCardQueriesOperations _corePhysicalOwnedCardQueriesOperations;
    private readonly ICoreHellActivityProgramsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess _coreActualTurnProgressTurnProgressionAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreHellActivityProgramsRuntime(ICoreHellActivityProgramsCardZonesOperations cardZonesOperations, ICoreHellActivityProgramsCoreHellEncounterHostOperations coreHellEncounterHostOperations, ICoreHellActivityProgramsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreHellActivityProgramsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreHellActivityProgramsCoreProgramNativeDrawOperations coreProgramNativeDrawOperations, ICoreHellActivityProgramsCoreRuntimeOperations coreRuntimeOperations, ICoreHellActivityProgramsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreHellActivityProgramsCorePhysicalOwnedCardQueriesOperations corePhysicalOwnedCardQueriesOperations, ICoreHellActivityProgramsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnProgressionAccess coreActualTurnProgressTurnProgressionAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._cardZonesOperations = cardZonesOperations;
        this._coreHellEncounterHostOperations = coreHellEncounterHostOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreProgramNativeDrawOperations = coreProgramNativeDrawOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._corePhysicalOwnedCardQueriesOperations = corePhysicalOwnedCardQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnProgressionAccess = coreActualTurnProgressTurnProgressionAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
