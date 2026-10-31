#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreSnapshotValueProjectionRuntime))]
internal sealed partial class CoreSnapshotValueProjectionEntries
{
    private readonly Func<EngineOwners.CoreSnapshotValueProjectionRuntime> _resolve;
    private CoreSnapshotValueProjectionEntries(Func<EngineOwners.CoreSnapshotValueProjectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreSnapshotValueProjectionEntries Create(Func<EngineOwners.CoreSnapshotValueProjectionRuntime> resolve) => new(resolve);
}
