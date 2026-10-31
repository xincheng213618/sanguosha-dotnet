#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramPhaseColorRestrictionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries _coreProgramLifecycleOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries _coreNativeCardAppearanceOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess _coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess;
    internal CoreGameEngineProgramPhaseColorRestrictionsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramLifecycleEntries coreProgramLifecycleOperations, global::CardGame.Core.RuntimeEntries.CoreNativeCardAppearanceEntries coreNativeCardAppearanceOperations, global::CardGame.Core.EngineOwners.IKernelAdvanceEventRulesAndQueueFactOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressCurrentSeatPhaseTurnNumberAccess coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess)
    {
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreNativeCardAppearanceOperations = coreNativeCardAppearanceOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess = coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess;
        this._coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess = coreGameEngineProgramPhaseColorRestrictionsProgramPhaseColorRestrictionsAccess;
    }
}
