#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchLogRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    private readonly global::CardGame.Core.EngineOwners.IKernelObserversAccess _kernelCoreGameEngineObserversOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess _coreCommandJournalStoragePendingNotificationsAccess;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess _coreGameLogLogLogSequenceAccess;
    internal CoreMatchLogRuntime(global::CardGame.Core.RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations, global::CardGame.Core.EngineOwners.IKernelObserversAccess kernelCoreGameEngineObserversOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, global::CardGame.Core.EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess coreCommandJournalStoragePendingNotificationsAccess, global::CardGame.Core.EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess coreGameLogLogLogSequenceAccess)
    {
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
        this._kernelCoreGameEngineObserversOperations = kernelCoreGameEngineObserversOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCommandJournalStoragePendingNotificationsAccess = coreCommandJournalStoragePendingNotificationsAccess;
        this._coreGameLogLogLogSequenceAccess = coreGameLogLogLogSequenceAccess;
    }
}
