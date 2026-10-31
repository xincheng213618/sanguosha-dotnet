#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreCommandSessionQueriesRuntime
{
    internal bool _isExecutingPublicOperation => _commandSessionOperations._commandSession.IsExecuting;
    internal long Revision => _kernelCoreGameEngineCommandSessionOperations._revision;
    internal IReadOnlyList<ObserverFailure> ObserverFailures => _coreCommandJournalStorageObserverFailuresAccess._observerFailures.AsReadOnly();
    internal IReadOnlyList<GameCommand> AcceptedCommands => _commandSessionOperations._commandSession.AcceptedCommands;

    internal GameCheckpoint CreateCheckpoint() => _commandSessionOperations._commandSession.CreateCheckpoint(_kernelCoreGameEngineCommandSessionOperations.CommandRuntime);
}
