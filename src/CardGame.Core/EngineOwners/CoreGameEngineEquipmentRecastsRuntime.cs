#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineEquipmentRecastsRuntime
{
    private readonly ICoreEquipmentRecastsModeRulesOperations _modeRulesOperations;
    private readonly ICoreEquipmentRecastsResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreEquipmentRecastsCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreEquipmentRecastsCoreHpChangeProgramsOperations _coreHpChangeProgramsOperations;
    private readonly ICoreEquipmentRecastsCoreModeCardRecastsOperations _coreModeCardRecastsOperations;
    private readonly ICoreEquipmentRecastsCoreNativeRecastInvocationsOperations _coreNativeRecastInvocationsOperations;
    private readonly ICoreEquipmentRecastsCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreEquipmentRecastsCoreHellDizangProgramsOperations _coreHellDizangProgramsOperations;
    private readonly ICoreEquipmentRecastsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreEquipmentRecastsCoreNativeOwnedCardQueriesOperations _coreNativeOwnedCardQueriesOperations;
    private readonly ICoreEquipmentRecastsCorePhysicalDrawFacadesOperations _corePhysicalDrawFacadesOperations;
    private readonly ICoreEquipmentRecastsCorePhysicalMovementFacadesOperations _corePhysicalMovementFacadesOperations;
    private readonly ICoreEquipmentRecastsCoreResolutionIdentityBoundaryOperations _coreResolutionIdentityBoundaryOperations;
    private readonly ICoreEquipmentRecastsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineEquipmentRecastsRuntime(ICoreEquipmentRecastsModeRulesOperations modeRulesOperations, ICoreEquipmentRecastsResolutionStackOperations resolutionStackOperations, ICoreEquipmentRecastsCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreEquipmentRecastsCoreHpChangeProgramsOperations coreHpChangeProgramsOperations, ICoreEquipmentRecastsCoreModeCardRecastsOperations coreModeCardRecastsOperations, ICoreEquipmentRecastsCoreNativeRecastInvocationsOperations coreNativeRecastInvocationsOperations, ICoreEquipmentRecastsCoreRuntimeOperations coreRuntimeOperations, ICoreEquipmentRecastsCoreHellDizangProgramsOperations coreHellDizangProgramsOperations, ICoreEquipmentRecastsCoreMatchLogOperations coreMatchLogOperations, ICoreEquipmentRecastsCoreNativeOwnedCardQueriesOperations coreNativeOwnedCardQueriesOperations, ICoreEquipmentRecastsCorePhysicalDrawFacadesOperations corePhysicalDrawFacadesOperations, ICoreEquipmentRecastsCorePhysicalMovementFacadesOperations corePhysicalMovementFacadesOperations, ICoreEquipmentRecastsCoreResolutionIdentityBoundaryOperations coreResolutionIdentityBoundaryOperations, ICoreEquipmentRecastsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._modeRulesOperations = modeRulesOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreHpChangeProgramsOperations = coreHpChangeProgramsOperations;
        this._coreModeCardRecastsOperations = coreModeCardRecastsOperations;
        this._coreNativeRecastInvocationsOperations = coreNativeRecastInvocationsOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreHellDizangProgramsOperations = coreHellDizangProgramsOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreNativeOwnedCardQueriesOperations = coreNativeOwnedCardQueriesOperations;
        this._corePhysicalDrawFacadesOperations = corePhysicalDrawFacadesOperations;
        this._corePhysicalMovementFacadesOperations = corePhysicalMovementFacadesOperations;
        this._coreResolutionIdentityBoundaryOperations = coreResolutionIdentityBoundaryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
