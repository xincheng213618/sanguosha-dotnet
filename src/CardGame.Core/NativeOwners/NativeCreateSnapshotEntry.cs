#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NativeCreateSnapshotEntry
{
private readonly global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> resolve;
private NativeCreateSnapshotEntry(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> resolve) { this.resolve = resolve; }
internal static NativeCreateSnapshotEntry Create(global::System.Func<global::CardGame.Core.EngineOwners.CorePlayerViewProjectionRuntime> resolve) => new(resolve);
internal global::CardGame.Core.GameSnapshot Invoke(int viewerSeat, bool revealAll = false) => resolve().CreateSnapshot(viewerSeat, revealAll);
}
