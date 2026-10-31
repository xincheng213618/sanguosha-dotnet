#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineTurnDefaultStatsValidationRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess _resolutionStackOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries _coreTurnDefaultStatsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries _coreNativeCardClassificationsOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries _corePendingDecisionPolicyPipelineOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries _coreTableSeatQueriesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess _coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess;
    internal CoreGameEngineTurnDefaultStatsValidationRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.EngineOwners.IKernelFrameStoreAccess resolutionStackOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreGameEngineTurnDefaultStatsEntries coreTurnDefaultStatsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardClassificationsEntries coreNativeCardClassificationsOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.RuntimeEntries.CorePendingDecisionPolicyPipelineEntries corePendingDecisionPolicyPipelineOperations, global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries coreTableSeatQueriesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._resolutionStackOperations = resolutionStackOperations;
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreTurnDefaultStatsOperations = coreTurnDefaultStatsOperations;
        this._coreNativeCardClassificationsOperations = coreNativeCardClassificationsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._corePendingDecisionPolicyPipelineOperations = corePendingDecisionPolicyPipelineOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess = coreGameEngineTurnDefaultStatsTurnDefaultStatLedgerStartedTurnDefaultStatStatesAccess;
    }
}
