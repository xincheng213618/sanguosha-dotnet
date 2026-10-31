#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSnapshotFactsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardSnapshotFactsRuntime> _resolve;
    private CoreGameEngineCardSnapshotFactsEntries(Func<EngineOwners.CoreGameEngineCardSnapshotFactsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardSnapshotFactsEntries Create(Func<EngineOwners.CoreGameEngineCardSnapshotFactsRuntime> resolve) => new(resolve);
}
