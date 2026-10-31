#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAdjacentDiscardStorageEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> _resolve;
    private CoreGameEngineAdjacentDiscardStorageEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineAdjacentDiscardStorageEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineAdjacentDiscardStorageRuntime> resolve) => new(resolve);
}
