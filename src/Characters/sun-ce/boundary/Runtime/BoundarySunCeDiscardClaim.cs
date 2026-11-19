using CardGame.Core.RuntimeSkills;

namespace CardGame.Core;

[ProgramRuntimeModule]
internal sealed class BoundarySunCeDiscardClaim(ICardMovementHistoryQueries movements, ICardZoneQueries zones,
    INativeLogicalDiscardOrigins discards)
{
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 1200)]
    private int[]? SelectFirstDiscardBatch(CardMovementBatchContext batch, ProgramTriggerCandidate candidate,
        SkillProgramTrigger trigger)
    {
        if (candidate.SkillId != "boundary:jiang-current" ||
            trigger.MovementOccurrence != SkillProgramMovementOccurrence.PerBatch ||
            !trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ClaimMovedCards))
            return null;
        if (batch.TurnNumber <= 0)
            return [];

        var qualifying = batch.Movements.Select((move, index) => (move, index))
            .Where(item => IsQualifyingDiscard(item.move)).ToArray();
        if (qualifying.Length == 0)
            return [];

        // The first opportunity belongs to the physical discard event even if it
        // was declined, or all its cards have already left the pile.
        var firstSequence = qualifying.Min(item => item.move.Sequence);
        var recorded = movements.Read();
        for (var index = recorded.Count - 1; index >= 0; index--)
        {
            var move = recorded[index];
            if (move.TurnNumber < batch.TurnNumber)
                break;
            if (move.TurnNumber == batch.TurnNumber && move.Sequence < firstSequence &&
                move.BatchId > 0 && IsQualifyingDiscard(move))
                return [];
        }

        return qualifying.Select(item => item.index).ToArray();
    }

    private bool IsQualifyingDiscard(CardMovementRecord move) =>
        move.To == CardLocation.DiscardPile &&
        (move.CardKind == CardKind.Duel || RuntimeRules_Core_CommonRules.IsSlashCard(move.CardKind) &&
            move.PrintedSuitAtMovement(zones) is Suit.Heart or Suit.Diamond) &&
        discards.GetProgramDiscardSource(move) is { OwnerSeat: not null };
}
