#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyWeiYanTargetQuery(Func<RuntimeEntries.CoreTableSeatQueriesEntries> getCoreTableSeatQueriesRuntime, EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime) : IHappyWeiYanTargetQuery
{
    public bool Allows(int owner, int target) => getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(owner) && getCoreTableSeatQueriesRuntime().IsValidPlayerSeat(target) && owner != target && matchParticipantsState.ReferenceToPlayers[owner].IsAlive && matchParticipantsState.ReferenceToPlayers[target].IsAlive && getCharacterWeiYanHappyGodRuntime().HappyBoneAvailableSlots(target).Any();
}
