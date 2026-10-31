#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeRecoveryFramesRuntime
{
    internal long BeginRecovery(long parentFrameId, int sourceSeat, int targetSeat, int amount)
    {
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new RecoveryFrame(frameId, parentFrameId, sourceSeat, targetSeat, amount, HpBefore: _coreMatchParticipantsPlayersAccess._players[targetSeat].Hp) { HappyQiaoPool = _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(targetSeat, parentFrameId) });
        return frameId;
    }
}
