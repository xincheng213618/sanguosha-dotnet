#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineNeighborDiscardOpportunitiesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> _resolve;
    private CoreGameEngineNeighborDiscardOpportunitiesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNeighborDiscardOpportunitiesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> resolve) => new(resolve);
}
