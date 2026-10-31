#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSubsetViewsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSubsetViewsRuntime> _resolve;
    private CoreGameEngineCardSubsetViewsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSubsetViewsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardSubsetViewsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSubsetViewsRuntime> resolve) => new(resolve);
}
