namespace CardGame.Core;

internal sealed partial class MatchModeRuntime
{
    internal void NotifyDeckRecycled()
    {
        foreach (var feature in _features.Values.OfType<IModeDeckRecycleObserver>())
            feature.OnDeckRecycled();
    }

    internal void CompleteFlowInterruption(ModeFlowInterruption interruption)
    {
        var handlers = _features.Values.OfType<IModeFlowInterruptionHandler>()
            .Where(handler => handler.InterruptionHandlerId == interruption.HandlerId).ToArray();
        if (handlers.Length != 1)
            throw new InvalidOperationException("A mode flow interruption requires exactly its installed owning handler.");
        handlers[0].CompleteInterruption(interruption.Request);
    }
}
