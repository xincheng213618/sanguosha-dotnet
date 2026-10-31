#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeRecoveryFramesEntries
{
    internal long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount) => _resolve().BeginRecovery(parentFrameId, sourceSeat, targetSeat, amount);
}
