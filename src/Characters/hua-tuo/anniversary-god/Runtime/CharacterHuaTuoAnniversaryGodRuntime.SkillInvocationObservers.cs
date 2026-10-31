#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoAnniversaryGodRuntime
{

    internal bool HasSkillInvocationObserverSuffix(int rootIndex)
    {
        for (var child = rootIndex + 1; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            var parent = _resolutionStackOperations._resolutionStack[child - 1];
            var current = _resolutionStackOperations._resolutionStack[child];
            if (parent is SkillInvocationFrame || current is SkillInvocationFrame)
            {
                if (!SkillInvocationStructuralEdge(parent, current))
                    return false;
            }
            else if (!CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(current, parent) && !_coreObserverSubtreesOperations.IsMovementObserverEdge(child))
                return false;
        }

        return true;
    }

    internal bool IsExactSkillInvocationDying(DyingFrame? dying)
    {
        if (dying is not { Continuation: DyingContinuationKind.SkillInvocation, KillerSeat: null } || _coreDyingCursorRuntimeOperations.ActiveDying?.Id != dying.Id)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is SkillInvocationFrame f && SkillInvocationStructuralEdge(f, dying) && f.Cursor < f.Reactions.Count && f.Reactions[f.Cursor] is { Kind: SkillInvocationReactionKind.Luxin, HpDelta: -1 } reaction && _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationHpPaidEvent>().Count(e => e.FrameId == f.Id && e.Cursor == f.Cursor && e.OwnerSeat == reaction.OwnerSeat && e.TargetSeat == dying.VictimSeat && e.Delta == -1 && e.After == e.Before - 1 && e.After <= 0) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenHuaTuoLuxinConsumedEvent>().Any(e => e.LuxinFrameId == reaction.LuxinFrameId && e.InvocationFrameId == f.Id && e.TargetSeat == dying.VictimSeat && e.HpDelta == -1);
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 38700)]
    internal bool HasExactSkillInvocationDying() => IsExactSkillInvocationDying(_coreDyingCursorRuntimeOperations.ActiveDying);
    internal CardConversionSource? SkillInvocationMovementOrigin() => _resolutionStackOperations._resolutionStack.LastOrDefault()is SkillInvocationFrame f && f.Cursor < f.Reactions.Count && f.Reactions[f.Cursor] is { Kind: SkillInvocationReactionKind.Jingyu } r ? new(ShenHtJingyu, "draw-after-skill", r.OwnerSeat, r.SkillInstanceId) : null;
    internal bool IsSkillInvocationMovementBatch(SkillInvocationFrame f, CardMovementBatchContext batch) => f.Stage == SkillInvocationStage.Children && f.DrawOutcome is { } draw && batch.ParentFrameId == f.Id && batch.AwaitingProgramFrameId is null && batch.Movements.Count > 0 && batch.Movements.All(m => m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter && draw.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(draw.RecipientSeat) && m.Reason.Value == "skill.shen-hua-tuo.jingyu.draw");
    internal bool IsSkillInvocationHealthChild(SkillInvocationFrame f, HpChangedTriggerWindowFrame hp) => f.Stage == SkillInvocationStage.Children && f.Cursor < f.Reactions.Count && f.Reactions[f.Cursor].Kind == SkillInvocationReactionKind.Luxin && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.SkillInvocation && hp.Change.ParentFrameId == f.Id && hp.Change.TargetSeat == f.ActorSeat && hp.Change.Kind == (f.Reactions[f.Cursor].HpDelta > 0 ? HpChangeKind.Recovery : HpChangeKind.Loss);
    internal bool IsSkillInvocationHealthChild(SkillInvocationFrame f, RecoveryReplacementFrame recovery) => recovery.ParentFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.SkillInvocation && recovery.Return.ResumeFrameId == f.Id && IsSkillInvocationRecoveryProducer(f, recovery.Attempt);
    internal bool IsSkillInvocationRecoveryProducer(SkillInvocationFrame f, RecoveryAttempt a) => f.Stage == SkillInvocationStage.Children && f.Cursor < f.Reactions.Count && f.Reactions[f.Cursor] is { Kind: SkillInvocationReactionKind.Luxin, HpDelta: 1 } r && a.Completion == new RecoveryAttemptCompletion(RecoveryAttemptProducer.SkillInvocation, f.Cursor) && a.SourceSeat == r.OwnerSeat && a.TargetSeat == f.ActorSeat && a.Amount == 1;
    internal bool SkillInvocationStructuralEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (child is SkillInvocationFrame f)
            return f.ParentFrameId == parent.Id && parent.SkillInvocations?.Issued.Contains(InvocationSourceKey(f.ActorSeat, f.SkillKey, f.OccurrenceKey), StringComparer.Ordinal) == true && _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Count(e => e.FrameId == f.Id && e.ParentFrameId == parent.Id && e.ActorSeat == f.ActorSeat && e.SkillKey == f.SkillKey && e.OccurrenceKey == f.OccurrenceKey) == 1;
        if (parent is not SkillInvocationFrame invocation)
            return false;
        if (child is CardsMovedTriggerWindowFrame movement)
            return movement.ResumeSkillInvocationFrameId == invocation.Id && IsSkillInvocationMovementBatch(invocation, movement.Batch);
        if (child is HpChangedTriggerWindowFrame hp)
            return IsSkillInvocationHealthChild(invocation, hp);
        if (child is RecoveryReplacementFrame recovery)
            return IsSkillInvocationHealthChild(invocation, recovery);
        if (child is NativeDrawAttemptFrame draw)
            return CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, invocation) && invocation.Stage == SkillInvocationStage.Drawing && draw.Return.Kind == NativeDrawAttemptReturnKind.SkillInvocation;
        return child is DyingFrame dying && invocation.Stage == SkillInvocationStage.Children && dying.ParentFrameId == invocation.Id && dying.Id == invocation.DyingFrameId && dying.VictimSeat == invocation.ActorSeat && dying.Continuation == DyingContinuationKind.SkillInvocation && dying.KillerSeat is null;
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 100)]
    internal void CaptureMovementBatchStartInvocationOrigin(CardMovementBatchBuilder batch) => batch.InvocationOrigin = SkillInvocationMovementOrigin();
}
