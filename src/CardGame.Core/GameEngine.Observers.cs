namespace CardGame.Core;

public sealed partial class GameEngine
{
    private ObserverDispatcher? _observers;
    private ObserverDispatcher Observers =>
        _observers ??= new(_pendingNotifications, _observerFailures, this.ComposeNativeObserverSource());
}
