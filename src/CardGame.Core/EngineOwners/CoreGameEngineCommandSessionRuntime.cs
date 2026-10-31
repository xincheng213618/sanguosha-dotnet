#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCommandSessionRuntime
{
    private readonly IKernelCommandEventJournalAccess _commandEventJournalOperations;
    private readonly IKernelCommandSessionAccess _commandSessionOperations;
    private readonly RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly ICoreCommandSessionCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly IKernelCommandRuntimeAccess _kernelCoreGameEngineCommandSessionOperations;
    private readonly EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess _coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess;
    internal CoreGameEngineCommandSessionRuntime(IKernelCommandEventJournalAccess commandEventJournalOperations, IKernelCommandSessionAccess commandSessionOperations, RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, ICoreCommandSessionCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, IKernelCommandRuntimeAccess kernelCoreGameEngineCommandSessionOperations, EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess)
    {
        this._commandEventJournalOperations = commandEventJournalOperations;
        this._commandSessionOperations = commandSessionOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
        this._coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess = coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess;
    }
}
