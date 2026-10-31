#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryAmountBonusesEntries
{
    internal bool HasOwnRecoveryAmountSupport(int targetSeat) => _resolve().HasOwnRecoveryAmountSupport(targetSeat);
    internal RecoveryAmountBonusReceipt? PrepareOwnRecoveryAmountBonus(PreparedRecoveryApplication application) => _resolve().PrepareOwnRecoveryAmountBonus(application);
    internal void RecordActualTurnRecovery(RecoveryAppliedEvent applied) => _resolve().RecordActualTurnRecovery(applied);
    internal void RequireAtomicRecoveryAmountBonus(RecoveryAmountBonusAppliedEvent paid) => _resolve().RequireAtomicRecoveryAmountBonus(paid);
    internal void RequireRecoveryAmountBonus(RecoveryReplacementFrame frame, RecoveryAmountBonusReceipt bonus) => _resolve().RequireRecoveryAmountBonus(frame, bonus);
    internal int SupportedRecoveryAmount(RecoveryReplacementFrame recovery, int baseAmount) => _resolve().SupportedRecoveryAmount(recovery, baseAmount);
    internal bool TracksRecoveryAmountBonuses { get => _resolve().TracksRecoveryAmountBonuses; }
}
