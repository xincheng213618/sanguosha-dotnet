#nullable enable
namespace CardGame.Core.RuntimeEntries;

// Keeps a module lazy while its consumers call the actual owner directly.
internal sealed class DeferredRuntime<TOwner>(Func<TOwner> resolve) where TOwner : class
{
    internal TOwner Value => resolve();
}
