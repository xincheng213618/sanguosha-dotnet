#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageArmorPaymentsEntries
{
    internal global::CardGame.Core.GameEngine.IDamageAttempt? DamageArmorPaymentOwner(global::CardGame.Core.DamageFrame damage) => _resolve().DamageArmorPaymentOwner(damage);
    internal bool IsExactArmorDamageRequest(global::CardGame.Core.DamageFrame damage, global::CardGame.Core.GameEngine.IDamageAttempt attack, bool requireUnpaid = false) => _resolve().IsExactArmorDamageRequest(damage, attack, requireUnpaid);
    internal bool IsIssuedDamageArmorPayment(global::CardGame.Core.DamageFrame damage) => _resolve().IsIssuedDamageArmorPayment(damage);
    internal bool IsValidDamageArmorPayment(global::CardGame.Core.DamageFrame damage, bool requireApplied) => _resolve().IsValidDamageArmorPayment(damage, requireApplied);
    internal global::CardGame.Core.DamageArmorPaymentReceipt? PrepareDamageArmorPayment(long frameId, global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().PrepareDamageArmorPayment(frameId, attack);
    internal void RequireExactDamageArmorPayment(global::CardGame.Core.IGameEvent payload) => _resolve().RequireExactDamageArmorPayment(payload);
}
