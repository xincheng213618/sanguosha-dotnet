#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class BoundaryBuLianShiQueries(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime, global::System.Func<global::CardGame.Core.FrameStore> getFrameStore, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterBuLianShiBoundaryMobileEntries> getCharacterBuLianShiBoundaryMobileRuntime) : IBoundaryBuLianShiQueries
{
    public bool HasHe(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat) && matchParticipantsState.ReferenceToPlayers[seat].IsAlive && getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count + getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Count > 0;
    public IReadOnlyList<int> Least(int owner) => getFrameStore().LastOrDefault()is ProgramSkillFrame { OwnerSeat: var seat, BoundaryBuLianShiAnxu: { LeastSelected: true, LeastSeat: { } least } } f && seat == owner && getCharacterBuLianShiBoundaryMobileRuntime().ValidBoundaryBuLianShiAnxu(f) && matchParticipantsState.ReferenceToPlayers[least].IsAlive ? [least] : [];
}
