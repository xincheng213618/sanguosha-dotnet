#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineCommandSessionRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelCommandEventJournalAccess _commandEventJournalOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCommandSessionAccess _commandSessionOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries _coreMatchLogOperations;
    private readonly ICoreCommandSessionCorePlayerViewProjectionOperations _corePlayerViewProjectionOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelCommandRuntimeAccess _kernelCoreGameEngineCommandSessionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess _coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess;
    internal CoreGameEngineCommandSessionRuntime(global::CardGame.Core.EngineOwners.IKernelCommandEventJournalAccess commandEventJournalOperations, global::CardGame.Core.EngineOwners.IKernelCommandSessionAccess commandSessionOperations, global::CardGame.Core.RuntimeEntries.CoreMatchLogEntries coreMatchLogOperations, ICoreCommandSessionCorePlayerViewProjectionOperations corePlayerViewProjectionOperations, global::CardGame.Core.EngineOwners.IKernelCommandRuntimeAccess kernelCoreGameEngineCommandSessionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess)
    {
        this._commandEventJournalOperations = commandEventJournalOperations;
        this._commandSessionOperations = commandSessionOperations;
        this._coreMatchLogOperations = coreMatchLogOperations;
        this._corePlayerViewProjectionOperations = corePlayerViewProjectionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
        this._coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess = coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess;
    }
}
