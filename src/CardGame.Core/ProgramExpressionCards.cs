namespace CardGame.Core;

/// <summary>Known card facts only; missing faces and source residency remain unknown.</summary>
public sealed record ProgramExpressionCardFact(int CardId, CardKind? PrintedKind = null,
    CardKind? EffectiveKind = null, Suit? EffectiveSuit = null,
    CardLocation? CapturedLocation = null, long? SourceEntrySequence = null, bool? IsActualGain = null)
{
    internal static ProgramExpressionCardFact FromHandGain(HandGainCardSnapshot card, int recipient) =>
        new(card.CardId, card.PrintedKind, card.EffectiveKind, card.EffectiveSuit,
            card.IsActualGain && card.GainSequence > 0 ? CardLocation.Hand(recipient) : null,
            card.IsActualGain && card.GainSequence > 0 ? card.GainSequence : null, card.IsActualGain);
}

internal static class ProgramGivenCards
{
    // Identity and original delivery are known to the two parties. A history
    // read grants neither private face access nor ownership of these entities.
    internal static IReadOnlyList<ProgramExpressionCardFact>? Read(INativeGivenCardQueries? history,
        IProgramTurnQueries? turn, int viewer, int giver, int recipient) =>
        history is null || turn is null || viewer != giver && viewer != recipient ? null :
            Array.AsReadOnly(history.GiftsForCurrentScope(giver, turn.TurnNumber, turn.TurnOwnerSeat)
                .Where(gift => gift.Recipient == recipient).SelectMany(gift => gift.Movements)
                .Reverse().DistinctBy(move => move.CardId).Reverse()
                .Select(move => new ProgramExpressionCardFact(move.CardId,
                    CapturedLocation: move.To, SourceEntrySequence: move.Sequence)).ToArray());
}

// Both expression evaluation and native consumers compose this same pure view.
// A historical public identity never authorizes reading its later private face.
internal static class ProgramPublicRevealCards
{
    internal static IReadOnlyList<ProgramExpressionCardFact>? Read(
        IReadOnlyList<PublicCardRevealHistoryFact> facts, string scope, string purpose,
        int? actualTurn, int? turnOwner, ActualDiscardRecoveryPhaseKey? actualPhase)
    {
        IEnumerable<PublicCardRevealHistoryFact> selected;
        if (scope == "all") selected = facts;
        else if (scope == "actualTurn" && actualTurn is { } turn && turnOwner is { } owner)
            selected = facts.Where(fact => fact.ActualTurn == turn && fact.TurnOwnerSeat == owner);
        else if (scope == "actualPhase" && actualPhase is { } phase)
            selected = facts.Where(fact => fact.ActualPhase == phase);
        else return null;
        if (purpose == "explicit") selected = selected.Where(fact => fact.Purpose == PublicCardRevealPurpose.Explicit);
        else if (purpose != "all") return null;
        return Array.AsReadOnly(selected.Reverse().DistinctBy(fact => fact.CardId).Reverse()
            .Select(fact => new ProgramExpressionCardFact(fact.CardId,
                CapturedLocation: CardLocation.Hand(fact.OwnerSeat),
                SourceEntrySequence: fact.HandEntryMovementSequence)).ToArray());
    }
}
