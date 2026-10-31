#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed partial class CoreNativeOwnedCardQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> _resolve;
    private CoreNativeOwnedCardQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeOwnedCardQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeOwnedCardQueriesRuntime> resolve) => new(resolve);
}
