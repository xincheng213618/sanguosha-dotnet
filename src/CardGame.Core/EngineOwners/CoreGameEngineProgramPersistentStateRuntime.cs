#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramPersistentStateRuntime
{
    private readonly ICoreProgramPersistentStateContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramPersistentStateKernelCoreGameEngineRuleProgressionOperations _kernelCoreGameEngineRuleProgressionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess _coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    internal CoreGameEngineProgramPersistentStateRuntime(ICoreProgramPersistentStateContentRegistryOperations contentRegistryOperations, ICoreProgramPersistentStateKernelCoreGameEngineRuleProgressionOperations kernelCoreGameEngineRuleProgressionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreGameEngineProgramPersistentStateProgramBooleanStatesAccess coreGameEngineProgramPersistentStateProgramBooleanStatesAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._kernelCoreGameEngineRuleProgressionOperations = kernelCoreGameEngineRuleProgressionOperations;
        this._coreGameEngineProgramPersistentStateProgramBooleanStatesAccess = coreGameEngineProgramPersistentStateProgramBooleanStatesAccess;
    }
}
