#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePublicPileCollectionEntries
{
    private readonly Func<EngineOwners.CoreGameEnginePublicPileCollectionRuntime> _resolve;
    private CoreGameEnginePublicPileCollectionEntries(Func<EngineOwners.CoreGameEnginePublicPileCollectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePublicPileCollectionEntries Create(Func<EngineOwners.CoreGameEnginePublicPileCollectionRuntime> resolve) => new(resolve);
}
