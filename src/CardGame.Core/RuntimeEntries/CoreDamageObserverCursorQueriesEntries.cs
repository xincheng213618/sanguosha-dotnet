#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreDamageObserverCursorQueriesRuntime))]
internal sealed partial class CoreDamageObserverCursorQueriesEntries
{
    private readonly Func<EngineOwners.CoreDamageObserverCursorQueriesRuntime> _resolve;
    private CoreDamageObserverCursorQueriesEntries(Func<EngineOwners.CoreDamageObserverCursorQueriesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreDamageObserverCursorQueriesEntries Create(Func<EngineOwners.CoreDamageObserverCursorQueriesRuntime> resolve) => new(resolve);
}
