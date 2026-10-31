#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyWeiYanTargetQuery(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime) : IHappyWeiYanTargetQuery
{
    public bool Allows(int owner, int target) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(owner) && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(target) && owner != target && matchParticipantsState.ReferenceToPlayers[owner].IsAlive && matchParticipantsState.ReferenceToPlayers[target].IsAlive && getCharacterWeiYanHappyGodRuntime().HappyBoneAvailableSlots(target).Any();
}
