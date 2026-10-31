namespace CardGame.Core;

public sealed partial class GameEngine
{
    /// <summary>
    /// An internal rules failure may have changed mutable state before it threw.
    /// A faulted session must be reconstructed from a trusted command prefix.
    /// </summary>
    public bool IsFaulted =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime>().IsFaulted;

    /// <summary>
    /// The last complete command prefix captured before a failing operation,
    /// when this session was driven entirely through Submit.
    /// </summary>
    public GameCheckpoint? LastTrustedCheckpoint =>GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreCommandSessionQueriesRuntime>().LastTrustedCheckpoint;

    private void EnsureSessionHealthy() => _commandSession.EnsureHealthy();
}
