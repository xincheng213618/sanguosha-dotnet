#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsRuntime
{
    private readonly ICoreProgramOtherOwnedCardDiscardsContentRegistryOperations _contentRegistryOperations;
    private readonly ICoreProgramOtherOwnedCardDiscardsCoreNativeAiDecisionsOperations _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations _programPaidCardOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    internal CoreGameEngineProgramOtherOwnedCardDiscardsRuntime(ICoreProgramOtherOwnedCardDiscardsContentRegistryOperations contentRegistryOperations, ICoreProgramOtherOwnedCardDiscardsCoreNativeAiDecisionsOperations coreNativeAiDecisionsOperations, ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations programPaidCardOperations, global::CardGame.Core.EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
    }
}
