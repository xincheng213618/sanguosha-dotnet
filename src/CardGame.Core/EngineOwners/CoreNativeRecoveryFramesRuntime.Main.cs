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

    internal static int CapRecoveryAmount(int requested, int currentHp, int maximumHp) =>
        Math.Min(requested, maximumHp - currentHp);

    internal (int Before, int After) ApplyRecovery(long recoveryId)
    {
        if (frames.Top is not RecoveryFrame recovery || recovery.Id != recoveryId)
            throw new InvalidOperationException("Applying recovery requires its current original frame.");

        var target = _coreMatchParticipantsPlayersAccess._players[recovery.TargetSeat];
        var before = target.Hp;
        target.Hp = before + CapRecoveryAmount(recovery.Amount, before, target.MaxHp);
        return (before, target.Hp);
    }
}
