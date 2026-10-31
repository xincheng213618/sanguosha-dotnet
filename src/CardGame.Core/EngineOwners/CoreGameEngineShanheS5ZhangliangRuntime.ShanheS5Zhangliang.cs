#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheS5ZhangliangRuntime
{
    internal static bool IsOwnTurnEnemyHandGainTrigger(SkillProgramTrigger trigger) => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.BindOwnTurnEnemyHandGain);
    internal int[] OwnTurnEnemyHandGainIndexes(CardMovementBatchContext batch, int target)
    {
        var indexes = _coreActualHandGainProgramsOperations.MatchingActualHandGainIndexes(batch, target, CardLocation.Hand(target));
        if (batch.NativeDrawInvocation is { } draw)
        {
            if (draw.OwnerSeat != target || draw.LastBatchId != batch.Id || draw.ParentFrameId != batch.ParentFrameId || draw.ActualTurnNumber != batch.TurnNumber || !_coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw))
                return[];
            var last = draw.Materials[^1];
            return indexes.Where(i => batch.Movements[i].Sequence == last.MovementSequence && batch.Movements[i].CardId == last.CardId).ToArray();
        }

        // DrawCards moves physical entities separately. Its existing complete
        // invoice belongs to the last batch; earlier materials cannot issue
        // additional gain occurrences for that same logical draw.
        var draws = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<NativeDrawInvocationRecordedEvent>().Where(e => e.OwnerSeat == target).ToArray();
        return indexes.Where(i => !draws.Any(e => batch.Movements[i] is { } move && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(target) && move.Reason == e.Reason && move.Sequence > e.SequenceBefore && move.Sequence <= e.SequenceAfter)).ToArray();
    }

    internal int? OwnTurnEnemyHandGainTarget(CardMovementBatchContext batch, int owner, int threshold)
    {
        if (batch.TurnNumber <= 0 || batch.TurnNumber != _coreActualTurnProgressTurnNumberAccess._turnNumber || batch.MovementTiming is not { } timing || timing.Phase is TurnPhase.NotStarted or TurnPhase.Finished || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(timing.ActualTurnOwnerSeat) || !_coreActualHandGainProgramsOperations.ActualHandGainTurnOpen(timing.ActualTurnOwnerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) || !_coreMatchParticipantsPlayersAccess._players[owner].IsAlive)
            return null;
        var target = timing.ActualTurnOwnerSeat;
        if (target == owner || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || !_coreMatchParticipantsPlayersAccess._players[owner].TeamRevealed || !_coreMatchParticipantsPlayersAccess._players[target].TeamRevealed || !_coreTeamActivityProgramsOperations.PublicTeamEnemy(owner, _coreMatchParticipantsPlayersAccess._players[target]) || batch.DestinationCounts?.SingleOrDefault(item => item.Location == CardLocation.Hand(target))is not { } count || count.CountAfter <= threshold)
            return null;
        var indexes = OwnTurnEnemyHandGainIndexes(batch, target);
        return indexes.Length > 0 && indexes.All(index => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(batch.Movements[index])) ? target : null;
    }

    internal IEnumerable<ProgramTriggerCandidate> CollectOwnTurnEnemyHandGainCandidates(CardMovementBatchContext batch)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.BindOwnTurnEnemyHandGain))
            yield break;
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(owner, SkillProgramTriggerWindow.CardsGained))
            {
                var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
                if (!IsOwnTurnEnemyHandGainTrigger(trigger) || OwnTurnEnemyHandGainTarget(batch, owner.Seat, trigger.Effects[0].Amount)is not { } target)
                    continue;
                var indexes = OwnTurnEnemyHandGainIndexes(batch, target);
                yield return candidate with
                {
                    OccurrenceIndex = indexes[0]
                };
            }
    }

    internal ProgramSkillWindowContext CreateOwnTurnEnemyHandGainContext(CardsMovedTriggerWindowFrame parent, ProgramTriggerCandidate candidate)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        var target = OwnTurnEnemyHandGainTarget(parent.Batch, candidate.OwnerSeat, trigger.Effects[0].Amount) ?? throw new InvalidOperationException("An own-turn enemy hand-gain candidate lost its original real gain batch.");
        var indexes = OwnTurnEnemyHandGainIndexes(parent.Batch, target);
        var count = parent.Batch.DestinationCounts!.Single(item => item.Location == CardLocation.Hand(target));
        return new(SkillProgramTriggerWindow.CardsGained, parent.Id, candidate.OwnerSeat, SourceSeat: parent.Batch.OriginOwnerSeat, TargetSeat: target, OccurrenceIndex: candidate.OccurrenceIndex, MovementIndex: candidate.OccurrenceIndex, Facts: _coreCardMovementProgramsOperations.CaptureCardsMovedTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], parent.Batch.NativeDrawInvocation?.ActualCount ?? indexes.Length, count, SkillProgramTriggerWindow.CardsGained, parent.Batch.MovementTiming), MovementBatch: parent.Batch);
    }

    internal bool CanRunOwnTurnEnemyHandGain(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger2(candidate);
        if (!IsOwnTurnEnemyHandGainTrigger(trigger) || context.Window != SkillProgramTriggerWindow.CardsGained || context.OwnerSeat != candidate.OwnerSeat || context.MovementBatch is not { } batch || context.ParentFrameId != batch.Id || context.SourceSeat != batch.OriginOwnerSeat || context.OccurrenceIndex != candidate.OccurrenceIndex || context.MovementIndex != candidate.OccurrenceIndex || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == batch.Id)is not CardsMovedTriggerWindowFrame parent || parent.Batch != batch || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || parent.Candidates[parent.CandidateIndex] != candidate || OwnTurnEnemyHandGainTarget(batch, candidate.OwnerSeat, trigger.Effects[0].Amount)is not { } target || context.TargetSeat != target)
            return false;
        return OwnTurnEnemyHandGainIndexes(batch, target)[0] == candidate.OccurrenceIndex;
    }

    internal void BindOwnTurnEnemyHandGain(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var trigger = _coreProgramLifecycleOperations.GetProgramTrigger(active);
        if (active.WindowContext is not { } context || active.InstructionIndex != 1 || !CanRunOwnTurnEnemyHandGain(new(active.OwnerSeat, active.SkillId, active.TriggerId!, active.SkillInstanceId, active.GameplayHash, trigger.Priority, context.OccurrenceIndex), context))
            throw new InvalidOperationException("An own-turn enemy hand-gain binding requires its exact current movement candidate.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { SelectedTargetSeats = Array.AsReadOnly(new[] { context.TargetSeat!.Value }) });
    }

    void IOwnTurnEnemyHandGainProgramHost.BindOwnTurnEnemyHandGain(ProgramSkillFrame frame) => BindOwnTurnEnemyHandGain(frame);
    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 200)]
    internal void CollectWindow_CollectOwnTurnEnemyHandGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectOwnTurnEnemyHandGainCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 500)]
    internal bool ExcludeOrdinaryMovement_IsOwnTurnEnemyHandGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => IsOwnTurnEnemyHandGainTrigger(trigger);
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 800)]
    internal ProgramSkillWindowContext? TryCreateOwnTurnEnemyHandGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (IsOwnTurnEnemyHandGainTrigger(_coreProgramLifecycleOperations.GetProgramTrigger2(candidate)))
            return CreateOwnTurnEnemyHandGainContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 130, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveOwnTurnEnemyGain(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => IsOwnTurnEnemyHandGainTrigger(trigger) ? CanRunOwnTurnEnemyHandGain(candidate, context) : null;
}
