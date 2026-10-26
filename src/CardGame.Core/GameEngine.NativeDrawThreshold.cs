namespace CardGame.Core;

public sealed partial class GameEngine
{
    private bool TracksNativeDrawThresholds => _contentRegistry.ProgramDependencies
        .HasTriggerOperation(SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold);
    private static bool UsesNativeDrawThreshold(SkillProgramTrigger trigger) =>
        trigger.Effects is [{ Op: SkillProgramEffectOp.DamageRandomEnemyAtDrawThreshold }];

    private IReadOnlyList<NativeDrawThresholdEntitlement>? CaptureNativeDrawThresholdEntitlements(
        long invocationId, int ownerSeat, long sequenceBefore)
    {
        if (!TracksNativeDrawThresholds) return null;
        var history = CompleteProgramEventHistory().OfType<NativeDrawThresholdProgressedEvent>().ToArray();
        var owner = _players[ownerSeat];
        var sources = owner.IsAlive ? GetSkillBindingShard(owner).GetInstanceTriggers(SkillProgramTriggerWindow.CardsGained)
            .Where(binding => UsesNativeDrawThreshold(binding.Trigger) && HasRuntimeSkillInstance(owner, binding.SkillId, binding.SkillInstanceId))
            .Select(binding =>
            {
                var source = new CardConversionSource(binding.SkillId, binding.Trigger.Id, ownerSeat, binding.SkillInstanceId);
                var threshold = binding.Trigger.Effects[0].MinimumValue;
                var prior = history.LastOrDefault(item => item.Source == source && item.GameplayHash == binding.Program.GameplayHash && item.Threshold == threshold);
                return new NativeDrawThresholdEntitlement(source, binding.Program.GameplayHash, threshold, prior?.RemainderAfter ?? 0);
            }).Distinct().OrderBy(item => item.Source.SkillId, StringComparer.Ordinal)
                .ThenBy(item => item.Source.BindingId, StringComparer.Ordinal)
                .ThenBy(item => item.Source.SkillInstanceId, StringComparer.Ordinal).ToArray() : [];
        foreach (var entitlement in sources)
            AdvanceEventRulesAndQueueFact(new NativeDrawThresholdSourceCapturedEvent(invocationId, ownerSeat, sequenceBefore, entitlement));
        return Array.AsReadOnly(sources);
    }

    private static IReadOnlyList<NativeDrawThresholdProgress>? FreezeNativeDrawThresholdProgress(
        NativeDrawInvocationDraft draft, int actualCount) => draft.ThresholdEntitlements is not { } entitlements ? null :
        Array.AsReadOnly(entitlements.Select(item =>
        {
            var total = checked((long)item.RemainderBefore + actualCount);
            return new NativeDrawThresholdProgress(item.Source, item.GameplayHash, item.Threshold,
                item.RemainderBefore, actualCount, checked((int)(total / item.Threshold)), checked((int)(total % item.Threshold)));
        }).ToArray());

    private void CompleteNativeDrawThresholdProgress(NativeDrawInvocationProof proof)
    {
        if (proof.ThresholdProgress is not { } progress) return;
        foreach (var item in progress)
            AdvanceEventRulesAndQueueFact(new NativeDrawThresholdProgressedEvent(proof.InvocationId, proof.OwnerSeat,
                item.Source, item.GameplayHash, item.Threshold, item.RemainderBefore, item.ActualCount, item.Crossings, item.RemainderAfter));
    }

    private bool ValidNativeDrawThresholdProof(NativeDrawInvocationProof proof)
    {
        if (proof.ThresholdProgress is not { } progress) return !TracksNativeDrawThresholds;
        if (progress is not System.Collections.IList { IsReadOnly: true } ||
            progress.Select(item => item.Source).Distinct().Count() != progress.Count) return false;
        var history = CompleteProgramEventHistory().ToArray();
        if (history.OfType<NativeDrawThresholdSourceCapturedEvent>().Count(item => item.InvocationId == proof.InvocationId) != progress.Count ||
            history.OfType<NativeDrawThresholdProgressedEvent>().Count(item => item.InvocationId == proof.InvocationId) != progress.Count) return false;
        foreach (var item in progress)
        {
            if (item.Source.OwnerSeat != proof.OwnerSeat || item.Threshold <= 0 ||
                item.RemainderBefore < 0 || item.RemainderBefore >= item.Threshold || item.ActualCount != proof.ActualCount ||
                item.Crossings != ((long)item.RemainderBefore + item.ActualCount) / item.Threshold ||
                item.RemainderAfter != ((long)item.RemainderBefore + item.ActualCount) % item.Threshold) return false;
            var captured = history.Select((fact, index) => (fact, index)).Where(pair => pair.fact is NativeDrawThresholdSourceCapturedEvent e &&
                e.InvocationId == proof.InvocationId && e.OwnerSeat == proof.OwnerSeat && e.SequenceBefore == proof.SequenceBefore &&
                e.Entitlement == new NativeDrawThresholdEntitlement(item.Source, item.GameplayHash, item.Threshold, item.RemainderBefore)).ToArray();
            var recorded = history.OfType<NativeDrawThresholdProgressedEvent>().Where(e => e.InvocationId == proof.InvocationId && e.Source == item.Source).ToArray();
            if (captured is not [var capture] || recorded is not [var result] || result.GameplayHash != item.GameplayHash ||
                result.Threshold != item.Threshold || result.RemainderBefore != item.RemainderBefore || result.ActualCount != item.ActualCount ||
                result.Crossings != item.Crossings || result.RemainderAfter != item.RemainderAfter) return false;
            var prior = history.Take(capture.index).OfType<NativeDrawThresholdProgressedEvent>().LastOrDefault(e =>
                e.Source == item.Source && e.GameplayHash == item.GameplayHash && e.Threshold == item.Threshold);
            if ((prior?.RemainderAfter ?? 0) != item.RemainderBefore) return false;
        }
        return true;
    }

    private NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.NativeDrawInvocation is not { } proof || proof.OwnerSeat != candidate.OwnerSeat || proof.LastBatchId != batch.Id ||
            proof.ParentFrameId != batch.ParentFrameId || proof.ActualTurnNumber != batch.TurnNumber || !ValidNativeDrawInvocation(proof)) return null;
        return proof.ThresholdProgress?.SingleOrDefault(item => item.Source ==
            new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) &&
            item.GameplayHash == candidate.GameplayHash && item.Crossings > candidate.OccurrenceIndex && candidate.OccurrenceIndex >= 0);
    }

    private int[] MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (MatchingNativeDrawThresholdProgress(batch, candidate) is null) return [];
        var proof = batch.NativeDrawInvocation!; var last = proof.Materials[^1];
        return batch.Movements.Select((movement, index) => (movement, index)).Where(pair =>
            pair.movement.Sequence == last.MovementSequence && pair.movement.CardId == last.CardId &&
            pair.movement.From == CardLocation.DrawPile && pair.movement.To == CardLocation.Hand(candidate.OwnerSeat) &&
            pair.movement.Reason == proof.Reason).Select(pair => pair.index).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeCondition, 62022)]
    private bool CanRunNativeDrawThreshold(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!UsesNativeDrawThreshold(trigger)) return true;
        return context.Window == SkillProgramTriggerWindow.CardsGained && context.MovementBatch is { } batch &&
            _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId) is { } window &&
            window.Batch.Id == batch.Id && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count &&
            window.Candidates[window.CandidateIndex] == candidate && MatchingNativeDrawThresholdIndexes(batch, candidate).Length == 1;
    }

    [ProgramCapability]
    private sealed class NativeDrawThresholdCapability(GameEngine engine) : INativeDrawThresholdHost
    {
        public SkillProgramStepOutcome DamageRandomEnemyAtDrawThreshold(ProgramSkillFrame frame, int threshold, int amount) =>
            engine.BeginNativeDrawThresholdDamage(frame, threshold, amount);
    }

    private SkillProgramStepOutcome BeginNativeDrawThresholdDamage(ProgramSkillFrame frame, int threshold, int amount)
    {
        var active = GetActiveProgramFrame(frame.Id);
        if (active.WindowContext is not { MovementBatch: { } batch } context ||
            _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(item => item.Id == context.ParentFrameId) is not { } window ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !MountObserverCandidateMatches(active, window.Candidates[window.CandidateIndex]) ||
            MatchingNativeDrawThresholdProgress(batch, window.Candidates[window.CandidateIndex]) is not { } progress || progress.Threshold != threshold)
            throw new InvalidOperationException("Native draw-threshold damage lost its exact paid invoice and current occurrence.");
        if (_winner != Winner.None || !_players[active.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[active.OwnerSeat], active.SkillId, active.SkillInstanceId)) return SkillProgramStepOutcome.Continue;
        var enemies = _players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat && PublicTeamEnemy(active.OwnerSeat, player))
            .OrderBy(player => player.Seat).ToArray();
        if (enemies.Length == 0) return SkillProgramStepOutcome.Continue;
        var target = enemies[_random.Next(enemies.Length)].Seat;
        ReplaceRuntimeTop(active = active with { SelectedTargetSeats = Array.AsReadOnly(new[] { target }) });
        AdvanceEventRulesAndQueueFact(new NativeDrawThresholdTargetChosenEvent(active.Id, batch.NativeDrawInvocation!.InvocationId,
            context.OccurrenceIndex, progress.Source, target));
        return BeginProgramSkillDamage(active, target, amount);
    }

    private bool NativeDrawThresholdProgramProducerMatches(ProgramSkillFrame producer,
        CardsMovedTriggerWindowFrame movement, NativeDrawInvocationProof proof)
    {
        if (proof.ParentFrameId != producer.Id || movement.Batch.ParentFrameId != producer.Id || proof.LastBatchId != movement.Batch.Id)
            return false;
        if (producer.NativeDraw is { RecipientApplied: true } paid)
            return paid.InstructionIndex == producer.InstructionIndex && paid.RecipientIndex >= 0 &&
                paid.RecipientIndex < paid.Recipients.Count && paid.Recipients[paid.RecipientIndex] == proof.OwnerSeat &&
                paid.RequestedCount == proof.RequestedCount && paid.Reason == proof.Reason;
        // The established synchronous Draw fast path owns no NativeDraw cursor.
        // Its exact paused typed instruction and standard movement return still
        // bind the paid invoice; do not recalculate a mutable numeric expression.
        if (producer.NativeDraw is not null || producer.InstructionIndex < 1 ||
            !RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(producer, movement)) return false;
        var plan = ProgramInstructionResolver.Default.Resolve(producer, _contentRegistry.GetSkill(producer.SkillId).Program!);
        if (producer.InstructionIndex > plan.Instructions.Count ||
            plan.GetPausedInstruction(producer.InstructionIndex).Effect.CompiledInstruction is not DrawProgramInstruction draw ||
            proof.Reason.Value != $"skill-program.{producer.SkillId}.{SkillProgramEffectOp.Draw}" ||
            draw.Amount is FixedProgramAmount fixedAmount && fixedAmount.Value != proof.RequestedCount) return false;
        return draw.Target == SkillProgramEffectTarget.SelectedTargets
            ? producer.SelectedTargetSeats.Contains(proof.OwnerSeat)
            : (draw.TargetReference is { } reference ? ResolveProgramParticipant(producer, reference)
                : ResolveProgramEffectTarget(producer, draw.Target)) == proof.OwnerSeat;
    }

    private long? NativeDrawThresholdSuspendedAttack(ProgramSkillFrame observer)
    {
        if (observer.WindowContext is not { MovementBatch: { NativeDrawInvocation: { } proof } batch } context ||
            _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(item => item.Id == context.ParentFrameId) is not { } moved ||
            moved.Batch.Id != batch.Id || moved.CandidateIndex < 0 || moved.CandidateIndex >= moved.Candidates.Count ||
            !MountObserverCandidateMatches(observer, moved.Candidates[moved.CandidateIndex]) ||
            MatchingNativeDrawThresholdProgress(batch, moved.Candidates[moved.CandidateIndex]) is null ||
            _resolutionStack.SingleOrDefault(item => item.Id == proof.ParentFrameId) is not ProgramSkillFrame producer ||
            !NativeDrawThresholdProgramProducerMatches(producer, moved, proof)) return null;
        var index = _resolutionStack.FindIndex(item => item.Id == producer.Id);
        while (index > 0 && NativeDrawThresholdObserverEdge(_resolutionStack[index - 1], _resolutionStack[index])) index--;
        if (index < 1 || _resolutionStack[index] is not ProgramSkillFrame root || root.WindowContext is not { } origin) return null;
        if (origin.Window == SkillProgramTriggerWindow.BeforeDamageApplied &&
            _resolutionStack[index - 1] is BeforeDamageProgramWindowFrame before && before.Id == origin.ParentFrameId &&
            origin.SourceSeat == (BeforeDamageHasNoSource(before) ? (int?)null : before.SourceSeat) &&
            origin.TargetSeat == before.TargetSeat && origin.Amount == before.Amount &&
            before.CandidateIndex >= 0 && before.CandidateIndex < before.Candidates.Count &&
            MountObserverCandidateMatches(root, before.Candidates[before.CandidateIndex].Candidate))
            return before.ContinuationAttackResolutionId ?? before.ParentFrameId;
        if (origin.Window is SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.DamageAppliedBeforeDying &&
            index >= 2 && _resolutionStack[index - 1] is DamageTriggerWindowFrame after &&
            _resolutionStack[index - 2] is DamageFrame damage && after.Id == origin.ParentFrameId && after.ParentFrameId == damage.Id &&
            origin.DamageFrameId == damage.Id && origin.TargetSeat == damage.TargetSeat && origin.Amount == damage.Amount &&
            after.CandidateIndex >= 0 && after.CandidateIndex < after.Candidates.Count &&
            MountObserverCandidateMatches(root, after.Candidates[after.CandidateIndex].ToProgramCandidate()))
            return damage.ParentFrameId;
        return null;
    }

    private bool NativeDrawThresholdObserverEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (TypedDamagePostEventEdge(parent, child)) return true;
        return parent is ProgramSkillFrame { NativeDraw: { RecipientApplied: true } paid } producer &&
            child is CardsMovedTriggerWindowFrame moved && moved.Batch.NativeDrawInvocation is { } draw &&
            draw.ParentFrameId == producer.Id && moved.Batch.ParentFrameId == producer.Id && draw.LastBatchId == moved.Batch.Id &&
            paid.InstructionIndex == producer.InstructionIndex && paid.RecipientIndex >= 0 && paid.RecipientIndex < paid.Recipients.Count &&
            paid.Recipients[paid.RecipientIndex] == draw.OwnerSeat && paid.RequestedCount == draw.RequestedCount && paid.Reason == draw.Reason &&
            ValidNativeDrawInvocation(draw);
    }

    [ProgramNestedDamageProof(ProgramNestedDamageProofPhase.AfterFactionDebts, 62022)]
    private bool AllowsNativeDrawThresholdDamage(ProgramSkillFrame frame, int target, int amount,
        ProgramParticipantReference? source, DamageNature? nature, bool sourceLess)
    {
        if (source is not null || nature is not null || sourceLess || frame.AttackAttempt is not null || ActiveDying is not null ||
            _resolutionStack.LastOrDefault()?.Id != frame.Id || frame.SelectedTargetSeats is not [var selected] || selected != target ||
            !UsesNativeDrawThreshold(GetProgramTrigger(frame)) || GetProgramTrigger(frame).Effects[0].Amount != amount ||
            NativeDrawThresholdSuspendedAttack(frame) is not { } originalAttack || CurrentDamageAttempt?.ResolutionId != originalAttack)
            return false;
        return ProgramEventHistory<NativeDrawThresholdTargetChosenEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray()
            is [var chosen] && chosen.TargetSeat == target && chosen.OccurrenceIndex == frame.WindowContext!.OccurrenceIndex;
    }
}
