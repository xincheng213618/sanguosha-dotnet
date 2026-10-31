#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchingRecastObserversEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastObserversRuntime> _resolve;
    private CoreGameEngineMatchingRecastObserversEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastObserversRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchingRecastObserversEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastObserversRuntime> resolve) => new(resolve);
}
