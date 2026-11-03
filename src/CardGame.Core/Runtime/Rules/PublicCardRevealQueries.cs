namespace CardGame.Core;

internal interface IPublicCardRevealQueries : IProgramSelectionQuery
{
    (long FrameId, IReadOnlyList<int> CardIds) Describe(IGameEvent payload);
}

// Read the original public show without creating another history ledger or
// requalifying the native producer that already admitted its materials.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class PublicCardRevealQueries : IPublicCardRevealQueries
{
    public (long FrameId, IReadOnlyList<int> CardIds) Describe(IGameEvent payload)
    {
        (long frameId, IEnumerable<int> cardIds) = payload switch
        {
            CardsRevealedEvent value => (value.ResolutionId, value.Cards.Select(card => card.Id)),
            ProgramCardsRevealedEvent value => (value.FrameId, value.Cards.Select(card => card.Id)),
            FireAttackCardRevealedEvent value => (value.ResolutionId, [value.CardId]),
            ProgramHandComparisonSourceRevealedEvent value => (value.FrameId, [value.CardId]),
            ProgramHandComparisonCardRevealedEvent value => (value.FrameId, [value.CardId]),
            _ => (0L, [])
        };
        return (frameId, Array.AsReadOnly(cardIds.Distinct().ToArray()));
    }
}
