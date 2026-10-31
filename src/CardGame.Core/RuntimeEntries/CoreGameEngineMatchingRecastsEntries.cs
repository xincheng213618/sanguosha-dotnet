#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineMatchingRecastsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> _resolve;
    private CoreGameEngineMatchingRecastsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineMatchingRecastsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineMatchingRecastsRuntime> resolve) => new(resolve);
}
