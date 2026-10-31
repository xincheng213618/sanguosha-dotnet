#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNeighborDiscardOpportunitiesRuntime
{
    internal bool TracksNeighborDiscardOpportunities => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.PutOwnOrPreviousFirstDiscardOnTop);

    internal int PreviousLivingSeatFor(int owner)
    {
        for (var offset = 1; offset < _coreMatchParticipantsPlayersAccess._players.Count; offset++)
        {
            var seat = _coreTableOrderOperations.TableStepSeat(owner, -offset);
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                return seat;
        }

        return owner;
    }

    internal static bool IsNeighborDiscardTopTrigger(SkillProgramTrigger trigger) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_NeighborDiscardOpportunities.IsNeighborDiscardTopTrigger(trigger);
    internal IEnumerable<IGameEvent> NeighborDiscardHistory() => _coreCommandJournalStorageEventsPendingEventsAccess._events.Select(e => e.Payload).Concat(_coreCommandJournalStorageEventsPendingEventsAccess._pendingEvents);
    internal void CaptureNeighborDiscardOpportunity(long batchId, int actualTurn, IReadOnlyList<CardMovementRecord> movements)
    {
        if (!TracksNeighborDiscardOpportunities || actualTurn <= 0)
            return;
        var owners = movements.Select(_coreProgramDiscardOriginsOperations.GetProgramDiscardSource).Where(location => location is { OwnerSeat: not null, Zone: CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment }).Select(location => location!.Value.OwnerSeat!.Value).Distinct().ToArray();
        if (owners.Length == 0)
            return;
        foreach (var source in owners)
            if (!NeighborDiscardHistory().OfType<ActualTurnFirstOwnedDiscardBatchEvent>().Any(e => e.ActualTurnNumber == actualTurn && e.DiscardOwnerSeat == source))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ActualTurnFirstOwnedDiscardBatchEvent(actualTurn, _coreActualTurnProgressTurnProgressionAccess._turnProgression.OwnerSeat, source, batchId));
        // Capture every public living-seat relation, without enumerating skill
        // shards, hidden generals or private libraries. Firstness survives a
        // later source loss/acquisition and a refused optional opportunity.
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
        {
            var previous = PreviousLivingSeatFor(owner.Seat);
            var previousFirst = previous != owner.Seat && owners.Contains(previous) && NeighborDiscardHistory().OfType<ActualTurnFirstOwnedDiscardBatchEvent>().Any(e => e.ActualTurnNumber == actualTurn && e.DiscardOwnerSeat == previous && e.BatchId == batchId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DiscardNeighborhoodFrozenEvent(batchId, actualTurn, _coreActualTurnProgressTurnProgressionAccess._turnProgression.OwnerSeat, owner.Seat, previous, owners.Contains(owner.Seat), previousFirst));
        }
    }

    internal DiscardNeighborhoodFrozenEvent? NeighborDiscardFact(CardMovementBatchContext batch, int owner) => NeighborDiscardHistory().OfType<DiscardNeighborhoodFrozenEvent>().SingleOrDefault(e => e.BatchId == batch.Id && e.ActualTurnNumber == batch.TurnNumber && e.OwnerSeat == owner);
    internal int[] NeighborDiscardIndexes(CardMovementBatchContext batch, int owner)
    {
        if (NeighborDiscardFact(batch, owner)is not { } fact)
            return[];
        return batch.Movements.Select((move, index) => (move, index)).Where(item => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(item.move)is { OwnerSeat: { } source, Zone: CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment } && (source == owner && fact.IncludesOwnDiscard || source == fact.PreviousLivingSeat && fact.IncludesFirstPreviousDiscard)).Select(item => item.index).ToArray();
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectNeighborDiscardCandidates(CardMovementBatchContext batch)
    {
        if (!TracksNeighborDiscardOpportunities)
            yield break;
        var seen = new HashSet<(int Owner, string Skill, string Binding, string Instance)>();
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsMoved))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
                if (!IsNeighborDiscardTopTrigger(trigger) || NeighborDiscardIndexes(batch, owner.Seat).Length == 0 || !seen.Add((candidate.OwnerSeat, candidate.SkillId, candidate.BindingId, candidate.SkillInstanceId)))
                    continue;
                yield return candidate;
            }
    }

    internal int? NeighborDiscardSourceSeat(CardMovementBatchContext batch, int owner)
    {
        var sources = NeighborDiscardIndexes(batch, owner).Select(index => _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(batch.Movements[index])!.Value.OwnerSeat!.Value).Distinct().ToArray();
        return sources.Length == 1 ? sources[0] : null;
    }

    internal ProgramSkillWindowContext CreateNeighborDiscardContext(CardsMovedTriggerWindowFrame window, ProgramTriggerCandidate candidate)
    {
        var fact = NeighborDiscardFact(window.Batch, candidate.OwnerSeat) ?? throw new InvalidOperationException("A neighbor discard candidate lost its frozen actual batch relation.");
        return new(SkillProgramTriggerWindow.CardsMoved, window.Id, candidate.OwnerSeat, SourceSeat: NeighborDiscardSourceSeat(window.Batch, candidate.OwnerSeat), TargetSeat: candidate.OwnerSeat, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat])with { MovedCardCount = NeighborDiscardIndexes(window.Batch, candidate.OwnerSeat).Length, FrozenPreviousLivingSeat = fact.PreviousLivingSeat }, MovementBatch: window.Batch);
    }

    internal bool IsProgramDiscardTopMovement(ProgramSkillFrame frame, CardMovementRecord move)
    {
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        if (paused.Op != SkillProgramEffectOp.PutOwnOrPreviousFirstDiscardOnTop)
        {
            var source = _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(move);
            var zones = _coreProgramLifecycleOperations.GetProgramTrigger(frame).SourceZones;
            return source is { } discarded && discarded.OwnerSeat == frame.OwnerSeat && (zones.Count == 0 || zones.Contains(discarded.Zone));
        }

        if (frame.WindowContext?.MovementBatch is not { } batch || NeighborDiscardFact(batch, frame.OwnerSeat)is not { } fact || frame.WindowContext.Facts?.FrozenPreviousLivingSeat != fact.PreviousLivingSeat)
            return false;
        return NeighborDiscardIndexes(batch, frame.OwnerSeat).Any(index => batch.Movements[index] == move);
    }

    internal bool MatchesFrozenOwnOrPreviousEnding(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => context.SourceSeat == candidate.OwnerSeat || context.Facts?.FrozenPreviousLivingSeat == context.SourceSeat;
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 1000)]
    internal void CollectWindow_CollectNeighborDiscardCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectNeighborDiscardCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 2600)]
    internal bool ExcludeOrdinaryMovement_IsNeighborDiscardTopTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsNeighborDiscardTopTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 4500)]
    internal ProgramSkillWindowContext? TryCreateNeighborDiscardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(candidate);
        if (IsNeighborDiscardTopTrigger(trigger))
            return CreateNeighborDiscardContext(frame, candidate);
        return null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CaptureMovementBatchFacts, 200)]
    internal void CaptureMovementBatchFactsCaptureNeighborDiscardOpportunity(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements) => CaptureNeighborDiscardOpportunity(batch.Id, batch.TurnNumber, movements);
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 250, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveNeighborDiscardMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CardMovementBatchContext batch, CharacterState owner) => IsNeighborDiscardTopTrigger(trigger) ? NeighborDiscardIndexes(batch, owner.Seat).Length > 0 && context.TargetSeat == owner.Seat && context.SourceSeat == NeighborDiscardSourceSeat(batch, owner.Seat) && NeighborDiscardFact(batch, owner.Seat)?.PreviousLivingSeat == context.Facts?.FrozenPreviousLivingSeat : null;
}
