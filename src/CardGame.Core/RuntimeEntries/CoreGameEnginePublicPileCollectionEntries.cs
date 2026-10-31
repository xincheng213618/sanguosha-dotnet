#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCollectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCollectionRuntime> _resolve;
    private CoreGameEnginePublicPileCollectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCollectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileCollectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileCollectionRuntime> resolve) => new(resolve);
}
