#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreTableSeatQueriesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> _resolve;
    private CoreTableSeatQueriesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreTableSeatQueriesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreTableSeatQueriesRuntime> resolve) => new(resolve);
}
