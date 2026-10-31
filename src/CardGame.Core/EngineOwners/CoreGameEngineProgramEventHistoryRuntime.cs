#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineProgramEventHistoryRuntime
{
    private readonly global::CardGame.Core.EngineOwners.IKernelProgramHistoryAccess _programHistoryOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess _coreCommandJournalStorageEventsPendingEventsAccess;
    internal CoreGameEngineProgramEventHistoryRuntime(global::CardGame.Core.EngineOwners.IKernelProgramHistoryAccess programHistoryOperations, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStorageEventsPendingEventsAccess coreCommandJournalStorageEventsPendingEventsAccess)
    {
        this._programHistoryOperations = programHistoryOperations;
        this._coreCommandJournalStorageEventsPendingEventsAccess = coreCommandJournalStorageEventsPendingEventsAccess;
    }
}
