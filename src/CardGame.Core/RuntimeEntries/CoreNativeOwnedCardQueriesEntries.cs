#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreNativeOwnedCardQueriesRuntime))]
internal sealed partial class CoreNativeOwnedCardQueriesEntries
{
    private readonly Func<EngineOwners.CoreNativeOwnedCardQueriesRuntime> _resolve;
    private CoreNativeOwnedCardQueriesEntries(Func<EngineOwners.CoreNativeOwnedCardQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeOwnedCardQueriesEntries Create(Func<EngineOwners.CoreNativeOwnedCardQueriesRuntime> resolve) => new(resolve);
}
