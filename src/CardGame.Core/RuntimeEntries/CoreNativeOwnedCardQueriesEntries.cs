#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeOwnedCardQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _resolve;
    private CoreNativeOwnedCardQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeOwnedCardQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> resolve) => new(resolve);
}
