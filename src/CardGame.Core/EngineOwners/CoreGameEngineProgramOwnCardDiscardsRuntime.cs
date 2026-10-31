#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsRuntime
{
    private readonly ICoreProgramOwnCardDiscardsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramOwnCardDiscardsCoreNativeAiDecisionsOperations _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramOwnCardDiscardsProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    internal CoreGameEngineProgramOwnCardDiscardsRuntime(ICoreProgramOwnCardDiscardsContentRegistryOperations contentRegistryOperations, ICoreProgramOwnCardDiscardsCoreNativeAiDecisionsOperations coreNativeAiDecisionsOperations, ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramOwnCardDiscardsProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
    }
}
