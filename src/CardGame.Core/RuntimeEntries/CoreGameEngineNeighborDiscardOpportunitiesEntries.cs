#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime))]
internal sealed partial class CoreGameEngineNeighborDiscardOpportunitiesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> _resolve;
    private CoreGameEngineNeighborDiscardOpportunitiesEntries(Func<EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNeighborDiscardOpportunitiesEntries Create(Func<EngineOwners.CoreGameEngineNeighborDiscardOpportunitiesRuntime> resolve) => new(resolve);
}
