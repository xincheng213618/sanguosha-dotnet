#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreTableSeatQueriesRuntime))]
internal sealed partial class CoreTableSeatQueriesEntries
{
    private readonly Func<EngineOwners.CoreTableSeatQueriesRuntime> _resolve;
    private CoreTableSeatQueriesEntries(Func<EngineOwners.CoreTableSeatQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreTableSeatQueriesEntries Create(Func<EngineOwners.CoreTableSeatQueriesRuntime> resolve) => new(resolve);
}
