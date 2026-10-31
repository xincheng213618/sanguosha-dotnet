#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreGameEngineShanheS5ZhangliangRuntime : global::CardGame.Core.IOwnTurnEnemyHandGainProgramHost
{
    private readonly ICoreShanheS5ZhangliangContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreShanheS5ZhangliangResolutionStackOperations _resolutionStackOperations;
    private readonly ICoreShanheS5ZhangliangCoreActualHandGainProgramsOperations _coreActualHandGainProgramsOperations;
    private readonly ICoreShanheS5ZhangliangCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly ICoreShanheS5ZhangliangCoreCardMovementProgramsOperations _coreCardMovementProgramsOperations;
    private readonly ICoreShanheS5ZhangliangCoreOutsidePhaseDrawDiscardOperations _coreOutsidePhaseDrawDiscardOperations;
    private readonly ICoreShanheS5ZhangliangCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreShanheS5ZhangliangCoreRuntimeOperations _coreRuntimeOperations;
    private readonly ICoreShanheS5ZhangliangCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreShanheS5ZhangliangCoreTeamActivityProgramsOperations _coreTeamActivityProgramsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess _coreCardMovementJournalCardMovementsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreGameEngineShanheS5ZhangliangRuntime(ICoreShanheS5ZhangliangContentRegistryOperations contentRegistryOperations, ICoreShanheS5ZhangliangResolutionStackOperations resolutionStackOperations, ICoreShanheS5ZhangliangCoreActualHandGainProgramsOperations coreActualHandGainProgramsOperations, ICoreShanheS5ZhangliangCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, ICoreShanheS5ZhangliangCoreCardMovementProgramsOperations coreCardMovementProgramsOperations, ICoreShanheS5ZhangliangCoreOutsidePhaseDrawDiscardOperations coreOutsidePhaseDrawDiscardOperations, ICoreShanheS5ZhangliangCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreShanheS5ZhangliangCoreRuntimeOperations coreRuntimeOperations, ICoreShanheS5ZhangliangCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreShanheS5ZhangliangCoreTeamActivityProgramsOperations coreTeamActivityProgramsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCardMovementJournalCardMovementsAccess coreCardMovementJournalCardMovementsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreActualHandGainProgramsOperations = coreActualHandGainProgramsOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreCardMovementProgramsOperations = coreCardMovementProgramsOperations;
        this._coreOutsidePhaseDrawDiscardOperations = coreOutsidePhaseDrawDiscardOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuntimeOperations = coreRuntimeOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreTeamActivityProgramsOperations = coreTeamActivityProgramsOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCardMovementJournalCardMovementsAccess = coreCardMovementJournalCardMovementsAccess;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
