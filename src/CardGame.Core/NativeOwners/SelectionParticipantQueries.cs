#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SelectionParticipantQueries(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CorePhysicalOwnedCardQueriesEntries> getCorePhysicalOwnedCardQueriesRuntime) : ISelectionParticipantQueries
{
    public IReadOnlyList<int> Seats => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Select(player => player.Seat).ToArray());

    public bool IsValidSeat(int seat) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(seat);
    public bool IsAlive(int seat) => matchParticipantsState.ReferenceToPlayers[seat].IsAlive;
    public int HandCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetHand(matchParticipantsState.ReferenceToPlayers[seat]).Count;
    public int EquipmentCount(int seat) => getCorePhysicalOwnedCardQueriesRuntime().GetEquipment2(seat).Count;
    public int Hp(int seat) => matchParticipantsState.ReferenceToPlayers[seat].Hp;
}
