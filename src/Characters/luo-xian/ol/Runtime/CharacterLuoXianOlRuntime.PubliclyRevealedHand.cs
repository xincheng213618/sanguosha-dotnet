#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuoXianOlRuntime
{
    internal int PubliclyRevealedHandCount(int owner) => _publicHandRevealHistory
        .Read()
        .Where(fact => fact.OwnerSeat == owner &&
            _currentHandEntries.CurrentHandEntry(owner, fact.CardId) == fact.HandEntryMovementSequence)
        .Select(fact => fact.CardId).Distinct().Count();
}
