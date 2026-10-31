#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePhaseHandExtraTurnAiRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess _skillRuntimeStateOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries _coreNationalSkillsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries _coreNativeSkillContextQueriesOperations;
    private readonly ICorePhaseHandExtraTurnAiCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess _coreActualTurnProgressCurrentSeatAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess _coreMatchOutcomeWinnerAccess;
    internal CoreGameEnginePhaseHandExtraTurnAiRuntime(global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.EngineOwners.IKernelSkillRuntimeStateAccess skillRuntimeStateOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineNationalSkillsEntries coreNationalSkillsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreNativeSkillContextQueriesEntries coreNativeSkillContextQueriesOperations, ICorePhaseHandExtraTurnAiCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatAccess coreActualTurnProgressCurrentSeatAccess, global::CardGame.Core.EngineStates.Contracts.ICoreMatchOutcomeWinnerAccess coreMatchOutcomeWinnerAccess)
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
