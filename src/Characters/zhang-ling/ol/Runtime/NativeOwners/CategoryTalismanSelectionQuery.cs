#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CategoryTalismanSelectionQuery(global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState, global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterZhangLingOlEntries> getCharacterZhangLingOlRuntime) : ICategoryTalismanSelectionQuery
{
    public IReadOnlyList<int> EligibleRecipients(int ownerSeat) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != ownerSeat && getCharacterZhangLingOlRuntime().LiveCategoryTalisman(p.Seat)is null).Select(p => p.Seat).ToArray());
}
