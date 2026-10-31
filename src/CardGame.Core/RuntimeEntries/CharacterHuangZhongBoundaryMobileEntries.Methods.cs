#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangZhongBoundaryMobileEntries
{
    internal int BoundaryHuangZhongCardDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().BoundaryHuangZhongCardDamage(attack);
    internal bool IsBoundaryHuangZhongOrdinaryRange(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind kind, int? excludedEquipmentId, global::CardGame.Core.EquipmentSlot? excludedEquipmentSlot) => _resolve().IsBoundaryHuangZhongOrdinaryRange(actor, target, kind, excludedEquipmentId, excludedEquipmentSlot);
    internal bool IsBoundaryHuangZhongUnrespondable(long frameId) => _resolve().IsBoundaryHuangZhongUnrespondable(frameId);
}
