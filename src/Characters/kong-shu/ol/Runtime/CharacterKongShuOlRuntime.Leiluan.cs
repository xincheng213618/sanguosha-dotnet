#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKongShuOlRuntime
{
    internal IReadOnlyList<Card> LeiluanTrickCandidates(ProgramSkillFrame f)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var started = history.OfType<TurnStartedEvent>().LastOrDefault(e => e.TurnNumber == f.KongShu!.ActualTurn);
        if (started is null)
            return[];
        // Movement records carry the actual turn, including response uses and the old turn's Ending tail.
        return _cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile).Where(c => CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(c.Kind, [SkillProgramCardCategory.InstantTrick]) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == c.Id && m.To == CardLocation.DiscardPile && m.TurnNumber == f.KongShu!.ActualTurn)).OrderBy(c => c.Id).ToArray();
    }
}
