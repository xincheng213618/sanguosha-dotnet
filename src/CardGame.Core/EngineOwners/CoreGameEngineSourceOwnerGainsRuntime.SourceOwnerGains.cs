#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceOwnerGainsRuntime
{
    internal IEnumerable<ProgramTriggerCandidate> CollectSourceOwnerGainCandidates(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, CardMovementSourceCount count)
    {
        if (trigger.Window != SkillProgramTriggerWindow.CardsGained)
            throw new InvalidOperationException("Source-owner gains require cardsGained.");
        var groups = batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.To == count.Location && item.movement.From != item.movement.To && item.movement.From.OwnerSeat is { } source && source != candidate.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value)).GroupBy(item => item.movement.From.OwnerSeat);
        foreach (var group in groups)
        {
            var facts = _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], group.Count(), count, trigger.Window, batch.MovementTiming);
            if (trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId))
                yield return candidate with
                {
                    OccurrenceIndex = group.First().index
                };
        }
    }
}
