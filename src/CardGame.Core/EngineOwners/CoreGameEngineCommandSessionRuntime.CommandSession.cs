#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCommandSessionRuntime
{
    internal CommandEventJournal CommandEvents => _commandEventJournalOperations._commandEventJournal ??= new(_coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._events, _coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._pendingEvents, _coreCommandJournalStorageEventsPendingEventsPendingNotificationsAccess._pendingNotifications);

    internal EngineRunResult ExecuteExclusive(Action operation) => ExecuteCommandOperation(operation, () => _corePlayerViewProjectionOperations.State, _coreMatchLogOperations.FlushNotifications);
    internal EngineRunResult ExecuteCommandOperation(Action operation, Func<GameSnapshot> project, Action<GameSnapshot> deliver) => _commandSessionOperations._commandSession.Execute(_kernelCoreGameEngineCommandSessionOperations.CommandRuntime, CommandEvents, operation, project, deliver);
}
