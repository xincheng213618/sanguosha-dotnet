#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRecoveryAmountBonusesEntries
{
    internal bool HasOwnRecoveryAmountSupport(int targetSeat) => _resolve().HasOwnRecoveryAmountSupport(targetSeat);
    internal global::CardGame.Core.RecoveryAmountBonusReceipt? PrepareOwnRecoveryAmountBonus(global::CardGame.Core.PreparedRecoveryApplication application) => _resolve().PrepareOwnRecoveryAmountBonus(application);
    internal void RecordActualTurnRecovery(global::CardGame.Core.RecoveryAppliedEvent applied) => _resolve().RecordActualTurnRecovery(applied);
    internal void RequireAtomicRecoveryAmountBonus(global::CardGame.Core.RecoveryAmountBonusAppliedEvent paid) => _resolve().RequireAtomicRecoveryAmountBonus(paid);
    internal void RequireRecoveryAmountBonus(global::CardGame.Core.RecoveryReplacementFrame frame, global::CardGame.Core.RecoveryAmountBonusReceipt bonus) => _resolve().RequireRecoveryAmountBonus(frame, bonus);
    internal int SupportedRecoveryAmount(global::CardGame.Core.RecoveryReplacementFrame recovery, int baseAmount) => _resolve().SupportedRecoveryAmount(recovery, baseAmount);
    internal bool TracksRecoveryAmountBonuses { get => _resolve().TracksRecoveryAmountBonuses; }
}
