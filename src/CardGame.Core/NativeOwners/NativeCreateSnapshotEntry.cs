#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeCreateSnapshotEntry
{
private readonly Func<EngineOwners.CorePlayerViewProjectionRuntime> resolve;
private NativeCreateSnapshotEntry(Func<EngineOwners.CorePlayerViewProjectionRuntime> resolve) { this.resolve = resolve; }
internal static NativeCreateSnapshotEntry Create(Func<EngineOwners.CorePlayerViewProjectionRuntime> resolve) => new(resolve);
internal GameSnapshot Invoke(int viewerSeat, bool revealAll = false) => resolve().CreateSnapshot(viewerSeat, revealAll);
}
