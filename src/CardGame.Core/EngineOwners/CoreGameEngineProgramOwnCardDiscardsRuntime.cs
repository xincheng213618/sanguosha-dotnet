#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramOwnCardDiscardsProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    internal CoreGameEngineProgramOwnCardDiscardsRuntime(global::CardGame.Core.EngineOwners.IKernelContentRegistryAccess contentRegistryOperations, global::CardGame.Core.RuntimeEntries.CoreNativeAiDecisionsEntries coreNativeAiDecisionsOperations, ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramOwnCardDiscardsProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
    }
}
