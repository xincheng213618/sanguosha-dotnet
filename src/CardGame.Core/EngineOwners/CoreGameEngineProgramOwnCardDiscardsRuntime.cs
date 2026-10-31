#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramOwnCardDiscardsRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreNativeAiDecisionsEntries _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramOwnCardDiscardsProgramPaidCardOperations _programPaidCardOperations;
    private readonly EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    internal CoreGameEngineProgramOwnCardDiscardsRuntime(IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreNativeAiDecisionsEntries coreNativeAiDecisionsOperations, ICoreProgramOwnCardDiscardsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramOwnCardDiscardsProgramPaidCardOperations programPaidCardOperations, EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
    }
}
