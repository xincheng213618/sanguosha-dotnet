#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreMatchLogRuntime
{
    internal void AddLog(string type, string message, int? actor = null, int? target = null)
    {
        var entry = new GameLogEntry(++_coreGameLogLogLogSequenceAccess._logSequence, _coreActualTurnProgressTurnNumberAccess._turnNumber, type, message, actor, target);
        _coreGameLogLogLogSequenceAccess._log.Add(entry);
        _characterSimaYiMouOperations.CaptureMouSmSecretLog(entry);
        _coreCommandJournalStoragePendingNotificationsAccess._pendingNotifications.Enqueue(new LogNotification(entry));
    }

    internal void FlushNotifications(GameSnapshot preparedSnapshot) => _kernelCoreGameEngineObserversOperations.Observers.Flush(preparedSnapshot);
}
