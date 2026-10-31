#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiRuntime
{
    private readonly ICorePhaseHandExtraTurnAiResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePhaseHandExtraTurnAiSkillRuntimeStateOperations _skillRuntimeStateOperations;
    private readonly ICorePhaseHandExtraTurnAiCoreNationalSkillsOperations _coreNationalSkillsOperations;
    private readonly ICorePhaseHandExtraTurnAiCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePhaseHandExtraTurnAiCoreNativeSkillContextQueriesOperations _coreNativeSkillContextQueriesOperations;
    private readonly ICorePhaseHandExtraTurnAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    internal CoreGameEnginePhaseHandExtraTurnAiRuntime(ICorePhaseHandExtraTurnAiResolutionStackOperations resolutionStackOperations, ICorePhaseHandExtraTurnAiSkillRuntimeStateOperations skillRuntimeStateOperations, ICorePhaseHandExtraTurnAiCoreNationalSkillsOperations coreNationalSkillsOperations, ICorePhaseHandExtraTurnAiCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePhaseHandExtraTurnAiCoreNativeSkillContextQueriesOperations coreNativeSkillContextQueriesOperations, ICorePhaseHandExtraTurnAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess)
    {
        this._resolutionStackOperations = resolutionStackOperations;
        this._skillRuntimeStateOperations = skillRuntimeStateOperations;
        this._coreNationalSkillsOperations = coreNationalSkillsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeSkillContextQueriesOperations = coreNativeSkillContextQueriesOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._coreActualTurnProgressCurrentSeatAccess = coreActualTurnProgressCurrentSeatAccess;
        this._coreMatchOutcomeWinnerAccess = coreMatchOutcomeWinnerAccess;
    }
}
