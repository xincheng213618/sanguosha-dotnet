#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreSnapshotValueProjectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> _resolve;
    private CoreSnapshotValueProjectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreSnapshotValueProjectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreSnapshotValueProjectionRuntime> resolve) => new(resolve);
}
