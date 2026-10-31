#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFieldEquipmentEntries
{
    internal EquipmentSlot FieldEquipmentSlot(Card card) => _resolve().FieldEquipmentSlot(card);
    internal bool HasHappyWeiYanPotentialCancellation(int seat) => _resolve().HasHappyWeiYanPotentialCancellation(seat);
}
