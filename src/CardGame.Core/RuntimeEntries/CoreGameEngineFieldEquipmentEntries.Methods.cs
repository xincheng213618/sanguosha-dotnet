#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineFieldEquipmentEntries
{
    internal global::CardGame.Core.EquipmentSlot FieldEquipmentSlot(global::CardGame.Core.Card card) => _resolve().FieldEquipmentSlot(card);
    internal bool HasHappyWeiYanPotentialCancellation(int seat) => _resolve().HasHappyWeiYanPotentialCancellation(seat);
}
