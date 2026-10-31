namespace CardGame.Core;
public sealed partial class GameEngine
{
    private readonly CommandSession _commandSession = new();
    private NativeOwners.CommandSessionRuntime? _commandSessionRuntime;
    private CommandEventJournal? _commandEventJournal;
    private ICommandSessionRuntime CommandRuntime => _commandSessionRuntime ??= this.ComposeNativeCommandSessionRuntime();
    private CommandEventJournal CommandEvents => _commandEventJournal ??= new(_events, _pendingEvents, _pendingNotifications);
    private long _revision => _commandSession.Revision;
    private GameCommand? _commandAwaitingCommit { get => _commandSession.CommandAwaitingCommit; set => _commandSession.CommandAwaitingCommit = value; }

    private EngineRunResult ExecuteExclusive(Action operation) => ExecuteCommandOperation(operation, () => State, this.MatchLogEntries.FlushNotifications);
    private EngineRunResult ExecuteCommandOperation(Action operation, Func<GameSnapshot> project, Action<GameSnapshot> deliver) => _commandSession.Execute(CommandRuntime, CommandEvents, operation, project, deliver);
}
