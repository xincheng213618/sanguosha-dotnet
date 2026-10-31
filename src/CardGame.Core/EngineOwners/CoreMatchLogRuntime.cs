#nullable enable

namespace CardGame.Core.EngineOwners;
[Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchLogRuntime
{
    private readonly RuntimeEntries.CharacterSimaYiMouEntries _characterSimaYiMouOperations;
    private readonly IKernelObserversAccess _kernelCoreGameEngineObserversOperations;
    private readonly EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess _coreActualTurnProgressTurnNumberAccess;
    private readonly EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess _coreCommandJournalStoragePendingNotificationsAccess;
    private readonly EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess _coreGameLogLogLogSequenceAccess;
    internal CoreMatchLogRuntime(RuntimeEntries.CharacterSimaYiMouEntries characterSimaYiMouOperations, IKernelObserversAccess kernelCoreGameEngineObserversOperations, EngineStates.Contracts.ICoreActualTurnProgressTurnNumberAccess coreActualTurnProgressTurnNumberAccess, EngineStates.Contracts.ICoreCommandJournalStoragePendingNotificationsAccess coreCommandJournalStoragePendingNotificationsAccess, EngineStates.Contracts.ICoreGameLogLogLogSequenceAccess coreGameLogLogLogSequenceAccess)
    {
        this._characterSimaYiMouOperations = characterSimaYiMouOperations;
        this._kernelCoreGameEngineObserversOperations = kernelCoreGameEngineObserversOperations;
        this._coreActualTurnProgressTurnNumberAccess = coreActualTurnProgressTurnNumberAccess;
        this._coreCommandJournalStoragePendingNotificationsAccess = coreCommandJournalStoragePendingNotificationsAccess;
        this._coreGameLogLogLogSequenceAccess = coreGameLogLogLogSequenceAccess;
    }
}
