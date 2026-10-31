#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreCommandSessionQueriesRuntime
{
    private readonly ICoreCommandSessionQueriesCommandSessionOperations _commandSessionOperations;
    private readonly ICoreCommandSessionQueriesKernelCoreGameEngineCommandSessionOperations _kernelCoreGameEngineCommandSessionOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess _coreCommandJournalStorageObserverFailuresAccess;
    internal CoreCommandSessionQueriesRuntime(ICoreCommandSessionQueriesCommandSessionOperations commandSessionOperations, ICoreCommandSessionQueriesKernelCoreGameEngineCommandSessionOperations kernelCoreGameEngineCommandSessionOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageObserverFailuresAccess coreCommandJournalStorageObserverFailuresAccess)
    {
        this._commandSessionOperations = commandSessionOperations;
        this._kernelCoreGameEngineCommandSessionOperations = kernelCoreGameEngineCommandSessionOperations;
        this._coreCommandJournalStorageObserverFailuresAccess = coreCommandJournalStorageObserverFailuresAccess;
    }
}
