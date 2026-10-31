#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuangZhongBoundaryMobileEntries
{
    internal int BoundaryHuangZhongCardDamage(GameEngine.IDamageAttempt attack) => _resolve().BoundaryHuangZhongCardDamage(attack);
    internal bool IsBoundaryHuangZhongOrdinaryRange(CharacterState actor, CharacterState target, CardKind kind, int? excludedEquipmentId, EquipmentSlot? excludedEquipmentSlot) => _resolve().IsBoundaryHuangZhongOrdinaryRange(actor, target, kind, excludedEquipmentId, excludedEquipmentSlot);
    internal bool IsBoundaryHuangZhongUnrespondable(long frameId) => _resolve().IsBoundaryHuangZhongUnrespondable(frameId);
}
