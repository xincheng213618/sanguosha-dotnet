#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEnginePlannedPhaseQuantitiesRuntime
{
    private readonly ICorePlannedPhaseQuantitiesContentRegistryOperations _contentRegistryOperations;
    private readonly ICorePlannedPhaseQuantitiesResolutionStackOperations _resolutionStackOperations;
    private readonly ICorePlannedPhaseQuantitiesCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICorePlannedPhaseQuantitiesCoreRuleQueriesOperations _coreRuleQueriesOperations;
    internal CoreGameEnginePlannedPhaseQuantitiesRuntime(ICorePlannedPhaseQuantitiesContentRegistryOperations contentRegistryOperations, ICorePlannedPhaseQuantitiesResolutionStackOperations resolutionStackOperations, ICorePlannedPhaseQuantitiesCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICorePlannedPhaseQuantitiesCoreRuleQueriesOperations coreRuleQueriesOperations)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreRuleQueriesOperations = coreRuleQueriesOperations;
    }
}
