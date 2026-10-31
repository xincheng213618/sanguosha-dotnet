#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageArmorPaymentsEntries
{
    internal GameEngine.IDamageAttempt? DamageArmorPaymentOwner(DamageFrame damage) => _resolve().DamageArmorPaymentOwner(damage);
    internal bool IsExactArmorDamageRequest(DamageFrame damage, GameEngine.IDamageAttempt attack, bool requireUnpaid = false) => _resolve().IsExactArmorDamageRequest(damage, attack, requireUnpaid);
    internal bool IsIssuedDamageArmorPayment(DamageFrame damage) => _resolve().IsIssuedDamageArmorPayment(damage);
    internal bool IsValidDamageArmorPayment(DamageFrame damage, bool requireApplied) => _resolve().IsValidDamageArmorPayment(damage, requireApplied);
    internal DamageArmorPaymentReceipt? PrepareDamageArmorPayment(long frameId, GameEngine.IDamageAttempt attack) => _resolve().PrepareDamageArmorPayment(frameId, attack);
    internal void RequireExactDamageArmorPayment(IGameEvent payload) => _resolve().RequireExactDamageArmorPayment(payload);
}
