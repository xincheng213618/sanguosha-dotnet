#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHeQiOlRuntime
{
    bool IHeQiSelectionQueries.HasRoundRange(int sourceSeat, int targetSeat) => _characterSunYuanOlOperations.HasSayingRoundRange(sourceSeat, targetSeat);
    int IHeQiSelectionQueries.CombatDistance(int sourceSeat, int targetSeat) => _coreNativeRuleQueriesOperations.GetCombatDistance(sourceSeat, targetSeat);
    int IHeQiSelectionQueries.AttackRange(int seat) => _coreNativeRuleQueriesOperations.GetAttackRange(seat);
    int IHeQiSelectionQueries.EquipmentSlotCapacity(int seat, EquipmentSlot slot) => _coreMatchParticipantsPlayersAccess._players[seat].EquipmentSlotCapacity(slot);
    int IHeQiSelectionQueries.EquipmentCountInSlot(int seat, EquipmentSlot slot) => _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat]).Count(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == slot);
}
