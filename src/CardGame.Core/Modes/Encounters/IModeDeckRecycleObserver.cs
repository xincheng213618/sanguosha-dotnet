namespace CardGame.Core;

/// <summary>One synchronous notification at the native discard-pile recycle boundary.</summary>
public interface IModeDeckRecycleObserver
{
    void OnDeckRecycled();
}
