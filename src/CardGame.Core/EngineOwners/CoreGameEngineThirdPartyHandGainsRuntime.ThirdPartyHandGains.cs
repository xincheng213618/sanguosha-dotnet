#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineThirdPartyHandGainsRuntime
{
    internal static bool IsThirdPartyHandGainTrigger(SkillProgramTrigger trigger) => RuntimeSkills.RuntimeRules_Core_ThirdPartyHandGains.IsThirdPartyHandGainTrigger(trigger);
    internal IEnumerable<ProgramTriggerCandidate> CollectThirdPartyHandGainCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
                if (!IsThirdPartyHandGainTrigger(trigger))
                    continue;
                if (trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.KanggeGainDraw))
                    continue;
                var indexes = batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.From != item.movement.To && item.movement.From.Zone == CardZoneKind.Hand && item.movement.From.OwnerSeat is { } source && source != item.movement.To.OwnerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source) && _coreMatchParticipantsPlayersAccess._players[source].IsAlive && item.movement.To.OwnerSeat is { } destination && _coreTableSeatQueriesOperations.IsValidPlayerSeat(destination) && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value)).Select(item => item.index).ToArray();
                foreach (var index in indexes)
                {
                    var movement = batch.Movements[index];
                    var destinationCount = _coreProgramMovementSourceCountsOperations.ProgramMovementSourceCounts(batch, trigger.Window).FirstOrDefault(item => item.Count.Location == movement.To).Count is { } matched ? matched : new CardMovementSourceCount(movement.To, 0, 1);
                    var facts = _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(owner, 1, destinationCount, trigger.Window, batch.MovementTiming);
                    if (trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId))
                        yield return candidate with
                        {
                            OccurrenceIndex = index
                        };
                }
            }
    }

    internal static bool IsOwnerSourceHandGainTrigger(SkillProgramTrigger trigger) => RuntimeSkills.RuntimeRules_Core_ThirdPartyHandGains.IsOwnerSourceHandGainTrigger(trigger);
    internal IEnumerable<ProgramTriggerCandidate> CollectOwnerSourceHandGainCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
                if (!IsOwnerSourceHandGainTrigger(trigger))
                    continue;
                var indexes = batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.From != item.movement.To && item.movement.From.Zone == CardZoneKind.Hand && item.movement.From.OwnerSeat == candidate.OwnerSeat && item.movement.To.OwnerSeat is { } destination && _coreTableSeatQueriesOperations.IsValidPlayerSeat(destination) && destination != candidate.OwnerSeat && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value)).Select(item => item.index).ToArray();
                foreach (var index in indexes)
                {
                    var movement = batch.Movements[index];
                    var sourceCount = _coreProgramMovementSourceCountsOperations.ProgramMovementSourceCounts(batch, trigger.Window).FirstOrDefault(item => item.Count.Location == movement.From).Count is { } matched ? matched : new CardMovementSourceCount(movement.From, 0, 1);
                    var facts = _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(owner, 1, sourceCount, trigger.Window, batch.MovementTiming);
                    if (trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId))
                        yield return candidate with
                        {
                            OccurrenceIndex = index
                        };
                }
            }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectKanggeHandGainCandidates(CardMovementBatchContext batch)
    {
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
                if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.KanggeGainDraw))
                    continue;
                var indexes = batch.Movements.Select((movement, index) => (movement, index)).Where(item => item.movement.From != item.movement.To && item.movement.To.Zone == CardZoneKind.Hand && item.movement.To.OwnerSeat is { } gainer && _coreTableSeatQueriesOperations.IsValidPlayerSeat(gainer) && _coreMatchParticipantsPlayersAccess._players[gainer].IsAlive && gainer != owner.Seat && _characterTangJiOlOperations.IsKanggeMarkedSeat(gainer, owner.Seat) && gainer != _coreActualTurnProgressCurrentSeatAccess._currentSeat && (trigger.MovementReasons.Count == 0 || trigger.MovementReasons.Contains(item.movement.Reason.Value)) && !trigger.ExcludedMovementReasons.Contains(item.movement.Reason.Value)).Select(item => item.index).ToArray();
                foreach (var index in indexes)
                {
                    var movement = batch.Movements[index];
                    var destinationCount = _coreProgramMovementSourceCountsOperations.ProgramMovementSourceCounts(batch, trigger.Window).FirstOrDefault(item => item.Count.Location == movement.To).Count is { } matched ? matched : new CardMovementSourceCount(movement.To, 0, 1);
                    var facts = _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(owner, 1, destinationCount, trigger.Window, batch.MovementTiming);
                    if (trigger.Condition.Evaluate(facts, candidate.SkillId, candidate.SkillInstanceId))
                        yield return candidate with
                        {
                            OccurrenceIndex = index
                        };
                }
            }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1100)]
    internal void CollectWindow_CollectThirdPartyHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectThirdPartyHandGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1200)]
    internal void CollectWindow_CollectOwnerSourceHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectOwnerSourceHandGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1500)]
    internal void CollectWindow_CollectKanggeHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectKanggeHandGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2700)]
    internal bool ExcludeOrdinaryMovement_IsThirdPartyHandGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsThirdPartyHandGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2800)]
    internal bool ExcludeOrdinaryMovement_IsOwnerSourceHandGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsOwnerSourceHandGainTrigger(trigger);
}
