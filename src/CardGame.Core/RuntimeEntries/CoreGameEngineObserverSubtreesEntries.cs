#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineObserverSubtreesRuntime))]
internal sealed partial class CoreGameEngineObserverSubtreesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineObserverSubtreesRuntime> _resolve;
    private CoreGameEngineObserverSubtreesEntries(Func<EngineOwners.CoreGameEngineObserverSubtreesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineObserverSubtreesEntries Create(Func<EngineOwners.CoreGameEngineObserverSubtreesRuntime> resolve) => new(resolve);
}
