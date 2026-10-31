#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramOtherOwnedCardDiscardsRuntime
{
    private readonly IKernelContentRegistryAccess _contentRegistryOperations;
    private readonly RuntimeEntries.CoreNativeAiDecisionsEntries _coreNativeAiDecisionsOperations;
    private readonly ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations _programPaidCardOperations;
    private readonly EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess _coreAiBrainsAiBrainsAccess;
    private readonly EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess _coreAiDiagnosticsThoughtSequenceAccess;
    internal CoreGameEngineProgramOtherOwnedCardDiscardsRuntime(IKernelContentRegistryAccess contentRegistryOperations, RuntimeEntries.CoreNativeAiDecisionsEntries coreNativeAiDecisionsOperations, ICoreProgramOtherOwnedCardDiscardsCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreProgramOtherOwnedCardDiscardsProgramPaidCardOperations programPaidCardOperations, EngineStates.Contracts.ICoreAiBrainsAiBrainsAccess coreAiBrainsAiBrainsAccess, EngineStates.Contracts.ICoreAiDiagnosticsThoughtSequenceAccess coreAiDiagnosticsThoughtSequenceAccess)
    {
        this._contentRegistryOperations = contentRegistryOperations;
        this._coreNativeAiDecisionsOperations = coreNativeAiDecisionsOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._programPaidCardOperations = programPaidCardOperations;
        this._coreAiBrainsAiBrainsAccess = coreAiBrainsAiBrainsAccess;
        this._coreAiDiagnosticsThoughtSequenceAccess = coreAiDiagnosticsThoughtSequenceAccess;
    }
}
