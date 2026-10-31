#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryBuLianShiQueries(Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, Func<FrameStore> getFrameStore, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterBuLianShiBoundaryMobileRuntime>> getCharacterBuLianShiBoundaryMobileRuntime) : IBoundaryBuLianShiQueries
{
    public bool HasHe(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive && getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Count > 0;
    public IReadOnlyList<int> Least(int owner) => getFrameStore().LastOrDefault()is ProgramSkillFrame { OwnerSeat: var seat, BoundaryBuLianShiAnxu: { LeastSelected: true, LeastSeat: { } least } } f && seat == owner && getCharacterBuLianShiBoundaryMobileRuntime().Value.ValidBoundaryBuLianShiAnxu(f) && matchParticipantsState.ReferenceToPlayers[least].IsAlive ? [least] : [];
}
