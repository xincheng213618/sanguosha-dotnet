#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineGameDomainsRuntime
{
    internal bool TracksFirstGameDomainCrossings => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveFirstGameDomainCrossing);

    internal static bool IsRuleOutsideGame(CardLocation at) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_GameDomains.IsRuleOutsideGame(at);
    internal void CaptureFirstGameDomainCrossings(long batchId, int turn, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!TracksFirstGameDomainCrossings || turn < 1 || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Any(e => e.TurnNumber == turn))
            return;
        foreach (var movement in movements.OrderBy(m => m.Sequence))
        {
            var from = IsRuleOutsideGame(movement.From);
            var to = IsRuleOutsideGame(movement.To);
            if (from == to)
                continue;
            var movedOut = to;
            if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstGameDomainCrossingEvent>().Any(e => e.TurnNumber == turn && e.MovedOut == movedOut))
                continue;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FirstGameDomainCrossingEvent(turn, batchId, movedOut, movement.Sequence));
        }
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectFirstDomainCandidates(CardMovementBatchContext batch)
    {
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstGameDomainCrossingEvent>().Where(e => e.BatchId == batch.Id).OrderBy(e => e.MovementOrdinal))
            foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatAccess._currentSeat, p.Seat)))
                foreach (var group in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.FirstGameDomainCrossing).GroupBy(c => c.SkillId, StringComparer.Ordinal))
                    yield return group.OrderByDescending(c => c.Priority).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).First()with
                    {
                        OccurrenceIndex = fact.MovedOut ? 0 : 1
                    };
    }

    internal FirstGameDomainCrossingEvent? FirstDomainFact(ProgramSkillWindowContext context) => context.MovementBatch is { } batch ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstGameDomainCrossingEvent>().SingleOrDefault(e => e.BatchId == batch.Id && e.MovedOut == (context.OccurrenceIndex == 0)) : null;
    internal ProgramSkillWindowContext CreateFirstDomainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var fact = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FirstGameDomainCrossingEvent>().Single(e => e.BatchId == frame.Batch.Id && e.MovedOut == (candidate.OccurrenceIndex == 0));
        var index = frame.Batch.Movements.Select((m, i) => (m, i)).Single(x => x.m.Sequence == fact.MovementOrdinal).i;
        var movement = frame.Batch.Movements[index];
        return new(SkillProgramTriggerWindow.FirstGameDomainCrossing, frame.Id, candidate.OwnerSeat, SourceSeat: movement.From.OwnerSeat, TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: index);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4400)]
    internal ProgramSkillWindowContext? TryCreateFirstDomainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        if (trigger.Window == SkillProgramTriggerWindow.FirstGameDomainCrossing)
            return CreateFirstDomainContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 900)]
    internal void CaptureMovementBatchFactsCaptureFirstGameDomainCrossings(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureFirstGameDomainCrossings(batch.Id, batch.TurnNumber, movements);
}
