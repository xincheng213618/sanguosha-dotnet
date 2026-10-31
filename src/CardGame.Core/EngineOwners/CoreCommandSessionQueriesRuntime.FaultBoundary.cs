#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreCommandSessionQueriesRuntime
{
    internal bool IsFaulted => _commandSessionOperations._commandSession.IsFaulted;
    internal GameCheckpoint? LastTrustedCheckpoint => IsFaulted ? _commandSessionOperations._commandSession.LastTrustedCheckpoint : null;
}
