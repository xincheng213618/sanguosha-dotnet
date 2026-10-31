#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramArmorOperationsRuntime : global::CardGame.Core.IArmorProgramHost
{
    private readonly ICoreProgramArmorOperationsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramArmorOperationsModeRulesOperations _modeRulesOperations;
    private readonly ICoreProgramArmorOperationsCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreProgramArmorOperationsCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramArmorOperationsCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreProgramArmorOperationsCoreTableSeatQueriesOperations _coreTableSeatQueriesOperations;
    private readonly ICoreProgramArmorOperationsKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramArmorOperationsRuntime(ICoreProgramArmorOperationsContentRegistryOperations contentRegistryOperations, ICoreProgramArmorOperationsModeRulesOperations modeRulesOperations, ICoreProgramArmorOperationsCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreProgramArmorOperationsCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramArmorOperationsCoreMatchLogOperations coreMatchLogOperations, ICoreProgramArmorOperationsCoreTableSeatQueriesOperations coreTableSeatQueriesOperations, ICoreProgramArmorOperationsKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._modeRulesOperations = modeRulesOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._coreTableSeatQueriesOperations = coreTableSeatQueriesOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
