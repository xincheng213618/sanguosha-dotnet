#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiOlRuntime
{
    int IHeQiSelectionQueries.EquipmentSlotCapacity(int seat, EquipmentSlot slot) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot);
    int IHeQiSelectionQueries.EquipmentCountInSlot(int seat, EquipmentSlot slot) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == slot);
}
