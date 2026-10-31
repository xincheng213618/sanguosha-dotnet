#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardSnapshotFactsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSnapshotFactsRuntime> _resolve;
    private CoreGameEngineCardSnapshotFactsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSnapshotFactsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardSnapshotFactsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardSnapshotFactsRuntime> resolve) => new(resolve);
}
