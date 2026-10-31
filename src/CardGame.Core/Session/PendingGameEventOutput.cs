namespace CardGame.Core;

internal interface IPendingGameEventOutput
{
    void Queue(IGameEvent payload);
}

// Output projection and private history capture share the original pending journal.
// Construction only stores callbacks; all facts are read at the original queue boundary.
internal sealed class PendingGameEventOutput(
    Func<bool> started,
    Func<IGameEvent, IGameEvent> project,
    Action<IGameEvent> append,
    Func<IGameEvent> lastEvent,
    Func<long> nextSequence,
    Action<IGameEvent, long> capture) : IPendingGameEventOutput
{
    public void Queue(IGameEvent payload)
    {
        if (!started()) return;
        append(project(payload));
        capture(lastEvent(), nextSequence());
    }
}
