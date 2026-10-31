namespace CardGame.Core;

// Runs only the paid threshold occurrence on its existing movement/program frames.
// Random selection, the public fact and damage entry retain their original order.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeDrawThresholdRuntime(IProgramRuntimeFrames frames,
    INativeFrameRoutingFacts nativeFrames, IProgramRuntimeEvents events,
    IProgramContentQueries content, IProgramEventHistoryQueries history,
    IMatchParticipantStateAccess participants, IMatchRandomSource random,
    INativeDrawInvoiceQueries invoices, INativeDrawProducerQueries draws,
    IProgramWindowReturnQueries windows, IProgramTargetFacts targets,
    INativeDrawThresholdNativeOperations operations) : INativeDrawThresholdHost
{
    private static bool UsesNativeDrawThreshold(SkillProgramTrigger trigger) =>
        NativeDrawThresholdInvoiceRuntime.UsesNativeDrawThreshold(trigger);

    private int FrameIndex(long id)
    {
        for (var index = 0; index < nativeFrames.FrameCount; index++)
            if (nativeFrames.FrameAt(index).Id == id) return index;
        return -1;
    }

    internal NativeDrawThresholdProgress? MatchingNativeDrawThresholdProgress(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
    {
        if (batch.NativeDrawInvocation is not { } proof || proof.OwnerSeat != candidate.OwnerSeat || proof.LastBatchId != batch.Id ||
            proof.ParentFrameId != batch.ParentFrameId || proof.ActualTurnNumber != batch.TurnNumber || !invoices.IsValid(proof)) return null;
        return proof.ThresholdProgress?.SingleOrDefault(item => item.Source ==
            new CardConversionSource(candidate.SkillId, candidate.BindingId, candidate.OwnerSeat, candidate.SkillInstanceId) &&
            item.GameplayHash == candidate.GameplayHash && item.Crossings > candidate.OccurrenceIndex && candidate.OccurrenceIndex >= 0);
    }

    internal int[] MatchingNativeDrawThresholdIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate)
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
            frames.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId) is { } window &&
            window.Batch.Id == batch.Id && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count &&
            window.Candidates[window.CandidateIndex] == candidate && MatchingNativeDrawThresholdIndexes(batch, candidate).Length == 1;
    }

    public SkillProgramStepOutcome DamageRandomEnemyAtDrawThreshold(ProgramSkillFrame frame, int threshold, int amount)
    {
        var active = frames.ActiveProgram(frame.Id);
        if (active.WindowContext is not { MovementBatch: { } batch } context ||
            frames.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(item => item.Id == context.ParentFrameId) is not { } window ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(active, window.Candidates[window.CandidateIndex]) ||
            MatchingNativeDrawThresholdProgress(batch, window.Candidates[window.CandidateIndex]) is not { } progress || progress.Threshold != threshold)
            throw new InvalidOperationException("Native draw-threshold damage lost its exact paid invoice and current occurrence.");
        if (draws.Winner != Winner.None || !participants.At(active.OwnerSeat).IsAlive ||
            !draws.HasSkillInstance(active.OwnerSeat, active.SkillId, active.SkillInstanceId)) return SkillProgramStepOutcome.Continue;
        var enemies = participants.Players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat && targets.PublicTeamEnemy(active.OwnerSeat, player))
            .OrderBy(player => player.Seat).ToArray();
        if (enemies.Length == 0) return SkillProgramStepOutcome.Continue;
        var target = enemies[random.Next(enemies.Length)].Seat;
        frames.ReplaceTop(active = active with { SelectedTargetSeats = Array.AsReadOnly(new[] { target }) });
        events.AdvanceFact(new NativeDrawThresholdTargetChosenEvent(active.Id, batch.NativeDrawInvocation!.InvocationId,
            context.OccurrenceIndex, progress.Source, target));
        return operations.BeginDamage(active, target, amount);
    }

    internal bool HasExactNativeDrawThresholdSelection(ProgramSkillFrame frame)
    {
        if (frame.TriggerId is null || frame.InstructionIndex != 1 || frame.SelectedTargetSeats is not [var target] ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained,
                MovementBatch: { NativeDrawInvocation: { } proof } batch } context ||
            !UsesNativeDrawThreshold(windows.GetProgramTrigger(frame)) ||
            frames.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(item => item.Id == context.ParentFrameId) is not { } movement ||
            movement.Batch != batch || movement.CandidateIndex < 0 || movement.CandidateIndex >= movement.Candidates.Count ||
            !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, movement.Candidates[movement.CandidateIndex]) ||
            MatchingNativeDrawThresholdProgress(batch, movement.Candidates[movement.CandidateIndex]) is not { } progress)
            return false;
        return history.Read<NativeDrawThresholdTargetChosenEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray()
            is [var chosen] && chosen.InvocationId == proof.InvocationId && chosen.OccurrenceIndex == context.OccurrenceIndex &&
            chosen.Source == progress.Source && chosen.TargetSeat == target;
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
        if (producer.NativeDraw is not null || producer.InstructionIndex < 1 ||
            !RuntimeSkills.RuntimeRules_Core_CommonRules.ProgramMovementReturnMatches(producer, movement)) return false;
        // The native draw itself freezes its exact issuing instruction. This
        // covers paid native operations without a per-skill damage exception.
        if (proof.ProgramOrigin is { } origin)
            return origin == invoices.CaptureProgramOrigin(producer) && invoices.IsValid(proof);
        // Native operations retain their own completed draw receipt on the
        // producer frame, rather than pausing a generic JSON Draw instruction.
        if (operations.CompletedNativeDrawMatches(producer, proof)) return true;
        var plan = ProgramInstructionResolver.Default.Resolve(producer, content.Skill(producer.SkillId).Program!);
        if (producer.InstructionIndex <= plan.Instructions.Count && producer.NativeOperationDrawReturns.Count(returned =>
                returned.InstructionIndex == producer.InstructionIndex && returned.Operation == plan.Instructions[producer.InstructionIndex - 1].Op &&
                draws.HasOperationReturn(returned.Operation) && returned.Reason == proof.Reason &&
                returned.Outcome is { Replacement: null } outcome && outcome.RecipientSeat == proof.OwnerSeat &&
                outcome.RequestedCount == proof.RequestedCount && outcome.SequenceBefore == proof.SequenceBefore &&
                outcome.SequenceAfter == proof.SequenceAfter && outcome.ActualDrawIds.SequenceEqual(proof.Materials.Select(material => material.CardId))) == 1)
            return true;
        // The synchronous Draw fast path owns no NativeDraw cursor. Bind its
        // paused typed instruction without recalculating mutable draw amounts.
        if (producer.InstructionIndex > plan.Instructions.Count ||
            plan.GetPausedInstruction(producer.InstructionIndex).Effect.CompiledInstruction is not DrawProgramInstruction draw ||
            proof.Reason.Value != $"skill-program.{producer.SkillId}.{SkillProgramEffectOp.Draw}" ||
            draw.Amount is FixedProgramAmount fixedAmount && fixedAmount.Value != proof.RequestedCount ||
            draw.Amount is RuntimeExpressionProgramAmount && ProgramExpressionAmounts.IssuedAmount(producer) != proof.RequestedCount) return false;
        return draw.Target == SkillProgramEffectTarget.SelectedTargets
            ? producer.SelectedTargetSeats.Contains(proof.OwnerSeat)
            : (draw.TargetReference is { } reference ? draws.ResolveParticipant(producer, reference)
                : RuntimeSkills.RuntimeRules_Core_ProgramChoices.ResolveProgramEffectTarget(producer, draw.Target)) == proof.OwnerSeat;
    }

    // A real self-dying program may draw before it recovers. The exact draw
    // invoice lends authority only to its own threshold occurrence, never to an
    // arbitrary gain observer or another program in the rescue subtree.
    internal bool TryDescribeNativeDrawThresholdDyingDamage(ProgramSkillFrame frame, int target, int amount,
        ProgramParticipantReference? source, DamageNature? nature, bool sourceLess, int? explicitSource,
        out NativeDrawThresholdDyingOrigin? origin)
    {
        origin = null;
        if (frame.TriggerId is null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained,
                MovementBatch: { NativeDrawInvocation: { } proof } batch } context ||
            source is not null || nature is not null || sourceLess || explicitSource is not null || frame.InstructionIndex != 1 ||
            frame.SelectedTargetSeats is not [var selected] || selected != target || !UsesNativeDrawThreshold(windows.GetProgramTrigger(frame)) ||
            windows.GetProgramTrigger(frame).Effects[0].Amount != amount) return false;
        var index = FrameIndex(frame.Id);
        if (index < 3 || nativeFrames.FrameAt(index - 1) is not CardsMovedTriggerWindowFrame movement ||
            movement.Id != context.ParentFrameId || movement.Batch != batch || movement.CandidateIndex < 0 ||
            movement.CandidateIndex >= movement.Candidates.Count || !RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, movement.Candidates[movement.CandidateIndex]) ||
            MatchingNativeDrawThresholdProgress(batch, movement.Candidates[movement.CandidateIndex]) is null ||
            nativeFrames.FrameAt(index - 2) is not ProgramSkillFrame producer ||
            !IsNativeDrawThresholdOwnerDrawProducer(producer) ||
            !NativeDrawThresholdProgramProducerMatches(producer, movement, proof) ||
            producer.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } rescue ||
            nativeFrames.FrameAt(index - 3) is not DyingFrame dying || dying.Id != producer.ParentFrameId ||
            rescue.ParentFrameId != dying.Id || rescue.OwnerSeat != producer.OwnerSeat || rescue.TargetSeat != dying.VictimSeat ||
            producer.OwnerSeat != dying.VictimSeat || proof.OwnerSeat != frame.OwnerSeat || frame.OwnerSeat != dying.VictimSeat ||
            dying.ResponderIndex < 0 || dying.ResponderIndex >= dying.ResponderSeats.Count || dying.ResponderSeat != dying.VictimSeat ||
            producer.TriggerId is null || windows.GetProgramTrigger(producer).Window != SkillProgramTriggerWindow.SelfDyingResponse ||
            dying.AttemptedSelfDyingBindings.Contains(RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.SelfDyingBindingKey(producer.SkillId, producer.TriggerId, producer.SkillInstanceId)))
            return false;
        var choices = history.Read<NativeDrawThresholdTargetChosenEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray();
        if (choices is not [var chosen] || chosen.InvocationId != proof.InvocationId || chosen.OccurrenceIndex != context.OccurrenceIndex ||
            chosen.Source != new CardConversionSource(frame.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            chosen.TargetSeat != target) return false;
        origin = new(dying.Id, producer.Id, proof.InvocationId, frame.InstructionIndex, target, amount);
        return true;
    }

    private bool IsNativeDrawThresholdOwnerDrawProducer(ProgramSkillFrame producer)
    {
        if (producer.InstructionIndex < 1) return false;
        var plan = ProgramInstructionResolver.Default.Resolve(producer, content.Skill(producer.SkillId).Program!);
        return producer.InstructionIndex <= plan.Instructions.Count &&
            plan.GetPausedInstruction(producer.InstructionIndex).Effect.CompiledInstruction is
                DrawProgramInstruction { Target: SkillProgramEffectTarget.Owner, TargetReference: null };
    }

    // Deliberately do not query ActiveDying here: this method is part of its
    // projection. Only the original token is hidden; any inner victim remains
    // visible to the ordinary dying, rescue and death machinery.
    internal bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(DyingFrame dying)
    {
        foreach (var frame in frames.OfType<ProgramSkillFrame>())
        {
            if (frame.AttackAttempt is not { SourceLess: false } attack || attack.SourceSeat != frame.OwnerSeat ||
                frame.AttackReturn?.NativeDrawDyingOrigin is not { } paid || paid.DyingFrameId != dying.Id) continue;
            if (TryDescribeNativeDrawThresholdDyingDamage(frame, paid.TargetSeat, paid.Amount, null, null, false, null, out var described) &&
                described == paid) return true;
        }
        return false;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 62022)]
    internal void AssertNativeDrawThresholdDyingOrigin(ProgramSkillFrame frame)
    {
        if (frame.AttackReturn?.NativeDrawDyingOrigin is not { } paid) return;
        if (frame.AttackAttempt is not { SourceLess: false } attack || attack.SourceSeat != frame.OwnerSeat ||
            !TryDescribeNativeDrawThresholdDyingDamage(frame, paid.TargetSeat, paid.Amount, null, null, false, null, out var described) ||
            described != paid)
            throw new InvalidOperationException("Draw-threshold damage lost its exact self-dying producer, native invoice or original dying token.");
    }
}
