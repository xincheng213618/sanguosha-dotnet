#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CategoryTalismanSelectionQuery(EngineStates.CoreMatchParticipantsState matchParticipantsState, Func<RuntimeEntries.CharacterZhangLingOlEntries> getCharacterZhangLingOlRuntime) : ICategoryTalismanSelectionQuery
{
    public IReadOnlyList<int> EligibleRecipients(int ownerSeat) => Array.AsReadOnly(matchParticipantsState.ReferenceToPlayers.Where(p => p.IsAlive && p.Seat != ownerSeat && getCharacterZhangLingOlRuntime().LiveCategoryTalisman(p.Seat)is null).Select(p => p.Seat).ToArray());
}
