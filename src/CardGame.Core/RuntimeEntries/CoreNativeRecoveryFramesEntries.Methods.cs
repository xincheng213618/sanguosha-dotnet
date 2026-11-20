#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeRecoveryFramesEntries
{
    internal (int Before, int After) ApplyRecovery(long recoveryId) => _resolve().ApplyRecovery(recoveryId);
    internal long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount) => _resolve().BeginRecovery(parentFrameId, sourceSeat, targetSeat, amount);
}
