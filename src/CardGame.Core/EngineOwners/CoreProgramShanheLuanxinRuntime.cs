#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule, global::CardGame.Core.ProgramCapability]
internal sealed partial class CoreProgramShanheLuanxinRuntime : global::CardGame.Core.IShanheLuanxinProgramHost
{
    private readonly ICoreProgramShanheLuanxinContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramShanheLuanxinCoreActualDiscardRecoveryPhasesOperations _coreActualDiscardRecoveryPhasesOperations;
    private readonly ICoreProgramShanheLuanxinCoreActualHandGainProgramsOperations _coreActualHandGainProgramsOperations;
    private readonly ICoreProgramShanheLuanxinCoreProgramEventHistoryOperations _coreProgramEventHistoryOperations;
    private readonly ICoreProgramShanheLuanxinCoreProgramLifecycleOperations _coreProgramLifecycleOperations;
    private readonly ICoreProgramShanheLuanxinKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess _coreMatchParticipantsPlayersAccess;
    internal CoreProgramShanheLuanxinRuntime(ICoreProgramShanheLuanxinContentRegistryOperations contentRegistryOperations, ICoreProgramShanheLuanxinCoreActualDiscardRecoveryPhasesOperations coreActualDiscardRecoveryPhasesOperations, ICoreProgramShanheLuanxinCoreActualHandGainProgramsOperations coreActualHandGainProgramsOperations, ICoreProgramShanheLuanxinCoreProgramEventHistoryOperations coreProgramEventHistoryOperations, ICoreProgramShanheLuanxinCoreProgramLifecycleOperations coreProgramLifecycleOperations, ICoreProgramShanheLuanxinKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchParticipantsPlayersAccess coreMatchParticipantsPlayersAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreActualDiscardRecoveryPhasesOperations = coreActualDiscardRecoveryPhasesOperations;
        this._coreActualHandGainProgramsOperations = coreActualHandGainProgramsOperations;
        this._coreProgramEventHistoryOperations = coreProgramEventHistoryOperations;
        this._coreProgramLifecycleOperations = coreProgramLifecycleOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreMatchParticipantsPlayersAccess = coreMatchParticipantsPlayersAccess;
    }
}
