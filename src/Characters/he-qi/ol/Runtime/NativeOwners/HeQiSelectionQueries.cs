#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HeQiSelectionQueries(Func<RuntimeEntries.CharacterSunYuanOlEntries> getCharacterSunYuanOlRuntime, KernelGetCombatDistanceCall getCombatDistance, KernelGetAttackRangeCall getAttackRange, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<EquipmentContentRegistry> getEquipmentContentRegistry) : IHeQiSelectionQueries
{
    public bool HasRoundRange(int sourceSeat, int targetSeat) => getCharacterSunYuanOlRuntime().HasSayingRoundRange(sourceSeat, targetSeat);
    public int CombatDistance(int sourceSeat, int targetSeat) => getCombatDistance(sourceSeat, targetSeat);
    public int AttackRange(int seat) => getAttackRange(seat);
    public int EquipmentSlotCapacity(int seat, EquipmentSlot slot) => matchParticipantsState.ReferenceToPlayers[seat].EquipmentSlotCapacity(slot);
    public int EquipmentCountInSlot(int seat, EquipmentSlot slot) => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]).Count(card => getEquipmentContentRegistry().Get(card.Kind).Slot == slot);
}
