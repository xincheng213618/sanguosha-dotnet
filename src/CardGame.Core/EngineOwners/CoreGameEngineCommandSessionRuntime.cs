#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCommandSessionRuntime
{
    private readonly ICoreCommandSessionCommandEventJournalOperations _commandEventJournalOperations;
    private readonly ICoreCommandSessionCommandSessionOperations _commandSessionOperations;
    private readonly ICoreCommandSessionCoreMatchLogOperations _coreMatchLogOperations;
    private readonly ICoreCommandSessionCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly ICoreCommandSessionKernelCoreGameEngineCommandSessionOperations _kernelCoreGameEngineCommandSessionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess _coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess;
    internal CoreGameEngineCommandSessionRuntime(ICoreCommandSessionCommandEventJournalOperations commandEventJournalOperations, ICoreCommandSessionCommandSessionOperations commandSessionOperations, ICoreCommandSessionCoreMatchLogOperations coreMatchLogOperations, ICoreCommandSessionCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, ICoreCommandSessionKernelCoreGameEngineCommandSessionOperations kernelCoreGameEngineCommandSessionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess)
    {
        this._commandEventJournalOperations = commandEventJournalOperations;
        this._commandSessionOperations = commandSessionOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
        this._coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess = coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess;
    }
}
