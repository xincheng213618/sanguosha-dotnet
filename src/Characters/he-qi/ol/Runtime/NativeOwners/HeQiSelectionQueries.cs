#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class HeQiSelectionQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunYuanOlEntries> getCharacterSunYuanOlRuntime, global::CardGame.Core.NativeOwners.KernelGetCombatDistanceCall getCombatDistance, global::CardGame.Core.NativeOwners.KernelGetAttackRangeCall getAttackRange, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.EquipmentContentRegistry> getEquipmentContentRegistry) : IHeQiSelectionQueries
{
    public bool HasRoundRange(int sourceSeat, int targetSeat) => getCharacterSunYuanOlRuntime().HasSayingRoundRange(sourceSeat, targetSeat);
    public int CombatDistance(int sourceSeat, int targetSeat) => getCombatDistance(sourceSeat, targetSeat);
    public int AttackRange(int seat) => getAttackRange(seat);
    public int EquipmentSlotCapacity(int seat, EquipmentSlot slot) => matchParticipantsState.ReferenceToPlayers[seat].EquipmentSlotCapacity(slot);
    public int EquipmentCountInSlot(int seat, EquipmentSlot slot) => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment(matchParticipantsState.ReferenceToPlayers[seat]).Count(card => getEquipmentContentRegistry().Get(card.Kind).Slot == slot);
}
