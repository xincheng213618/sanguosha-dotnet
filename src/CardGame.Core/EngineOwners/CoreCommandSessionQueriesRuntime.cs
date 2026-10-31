#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreCommandSessionQueriesRuntime
{
    private readonly IKernelCommandSessionAccess _commandSessionOperations;
    private readonly IKernelCommandRuntimeAndRevisionAccess _kernelCoreGameEngineCommandSessionOperations;
    private readonly EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess _coreCommandJournalStorageObserverFailuresAccess;
    internal CoreCommandSessionQueriesRuntime(IKernelCommandSessionAccess commandSessionOperations, IKernelCommandRuntimeAndRevisionAccess kernelCoreGameEngineCommandSessionOperations, EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess coreCommandJournalStorageObserverFailuresAccess)
    {
        this._commandSessionOperations = commandSessionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
        this._coreCommandJournalStorageObserverFailuresAccess = coreCommandJournalStorageObserverFailuresAccess;
    }
}
