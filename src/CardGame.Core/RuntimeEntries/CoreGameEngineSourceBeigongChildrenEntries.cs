#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSourceBeigongChildrenEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime> _resolve;
    private CoreGameEngineSourceBeigongChildrenEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSourceBeigongChildrenEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSourceBeigongChildrenRuntime> resolve) => new(resolve);
}
