namespace CardGame.Core;

public sealed partial class GameEngine
{
    // A real self-dying program may draw before it recovers. The exact draw
    // invoice lends authority only to its own threshold occurrence, never to an
    // arbitrary gain observer or another program in the rescue subtree.
    private bool TryDescribeNativeDrawThresholdDyingDamage(ProgramSkillFrame frame, int target, int amount,
        ProgramParticipantReference? source, DamageNature? nature, bool sourceLess, int? explicitSource,
        out NativeDrawThresholdDyingOrigin? origin)
    {
        origin = null;
        if (frame.TriggerId is null ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained,
                MovementBatch: { NativeDrawInvocation: { } proof } batch } context ||
            source is not null || nature is not null || sourceLess || explicitSource is not null || frame.InstructionIndex != 1 ||
            frame.SelectedTargetSeats is not [var selected] || selected != target || !UsesNativeDrawThreshold(GetProgramTrigger(frame)) ||
            GetProgramTrigger(frame).Effects[0].Amount != amount) return false;
        var index = _resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 3 || _resolutionStack[index - 1] is not CardsMovedTriggerWindowFrame movement ||
            movement.Id != context.ParentFrameId || movement.Batch != batch || movement.CandidateIndex < 0 ||
            movement.CandidateIndex >= movement.Candidates.Count || !MountObserverCandidateMatches(frame, movement.Candidates[movement.CandidateIndex]) ||
            MatchingNativeDrawThresholdProgress(batch, movement.Candidates[movement.CandidateIndex]) is null ||
            _resolutionStack[index - 2] is not ProgramSkillFrame producer ||
            !IsNativeDrawThresholdOwnerDrawProducer(producer) ||
            !NativeDrawThresholdProgramProducerMatches(producer, movement, proof) ||
            producer.WindowContext is not { Window: SkillProgramTriggerWindow.SelfDyingResponse } rescue ||
            _resolutionStack[index - 3] is not DyingFrame dying || dying.Id != producer.ParentFrameId ||
            rescue.ParentFrameId != dying.Id || rescue.OwnerSeat != producer.OwnerSeat || rescue.TargetSeat != dying.VictimSeat ||
            producer.OwnerSeat != dying.VictimSeat || proof.OwnerSeat != frame.OwnerSeat || frame.OwnerSeat != dying.VictimSeat ||
            dying.ResponderIndex < 0 || dying.ResponderIndex >= dying.ResponderSeats.Count || dying.ResponderSeat != dying.VictimSeat ||
            producer.TriggerId is null || GetProgramTrigger(producer).Window != SkillProgramTriggerWindow.SelfDyingResponse ||
            dying.AttemptedSelfDyingBindings.Contains(SelfDyingBindingKey(producer.SkillId, producer.TriggerId, producer.SkillInstanceId)))
            return false;
        var choices = ProgramEventHistory<NativeDrawThresholdTargetChosenEvent>().Where(item => item.ProgramFrameId == frame.Id).ToArray();
        if (choices is not [var chosen] || chosen.InvocationId != proof.InvocationId || chosen.OccurrenceIndex != context.OccurrenceIndex ||
            chosen.Source != new CardConversionSource(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId) ||
            chosen.TargetSeat != target) return false;
        origin = new(dying.Id, producer.Id, proof.InvocationId, frame.InstructionIndex, target, amount);
        return true;
    }

    private bool IsNativeDrawThresholdOwnerDrawProducer(ProgramSkillFrame producer)
    {
        if (producer.InstructionIndex < 1) return false;
        var plan = ProgramInstructionResolver.Default.Resolve(producer, _contentRegistry.GetSkill(producer.SkillId).Program!);
        return producer.InstructionIndex <= plan.Instructions.Count &&
            plan.GetPausedInstruction(producer.InstructionIndex).Effect.CompiledInstruction is
                DrawProgramInstruction { Target: SkillProgramEffectTarget.Owner, TargetReference: null };
    }

    // Deliberately do not query ActiveDying here: this method is part of its
    // projection. Only the original token is hidden; any inner victim remains
    // visible to the ordinary dying, rescue and death machinery.
    private bool IsOriginalDyingSuspendedByNativeDrawThresholdDamage(DyingFrame dying)
    {
        foreach (var frame in _resolutionStack.OfType<ProgramSkillFrame>())
        {
            if (frame.AttackAttempt is not { SourceLess: false } attack || attack.SourceSeat != frame.OwnerSeat ||
                frame.AttackReturn?.NativeDrawDyingOrigin is not { } paid || paid.DyingFrameId != dying.Id) continue;
            if (TryDescribeNativeDrawThresholdDyingDamage(frame, paid.TargetSeat, paid.Amount, null, null, false, null, out var described) &&
                described == paid) return true;
        }
        return false;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 62022)]
    private void AssertNativeDrawThresholdDyingOrigin(ProgramSkillFrame frame)
    {
        if (frame.AttackReturn?.NativeDrawDyingOrigin is not { } paid) return;
        if (frame.AttackAttempt is not { SourceLess: false } attack || attack.SourceSeat != frame.OwnerSeat ||
            !TryDescribeNativeDrawThresholdDyingDamage(frame, paid.TargetSeat, paid.Amount, null, null, false, null, out var described) ||
            described != paid)
            throw new InvalidOperationException("Draw-threshold damage lost its exact self-dying producer, native invoice or original dying token.");
    }
}
