#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoAnniversaryGodRuntime
{
    internal bool IsCardWindowWaitingForSkillInvocation(ProgramCardTriggerWindowFrame window, int index)
    {
        if (index < 1 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index].Id != window.Id || window.CandidateIndex != 0 || window.Activated || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.GetCardActionWindow(window)is not (SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted) || _resolutionStackOperations._resolutionStack[index - 1].Id != window.ParentFrameId || _resolutionStackOperations._resolutionStack[index + 1] is not SkillInvocationFrame invocation || invocation.ParentFrameId != window.Id || invocation.ReturnKind != SkillInvocationReturnKind.RuntimeFrame || invocation.SourceFrameFingerprint is not { Length: 64 } fingerprint || fingerprint != SkillInvocationSourceFingerprint(window) || !SkillInvocationStructuralEdge(window, invocation) || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Count(e => e == new SkillInvocationStartedEvent(invocation.Id, window.Id, invocation.ActorSeat, invocation.SkillKey, invocation.OccurrenceKey, invocation.SkillInstanceId, invocation.SkillName, invocation.GameplayHash, invocation.RoundNumber, invocation.TurnNumber, invocation.TurnOwnerSeat, invocation.ReturnKind)) != 1)
            return false;
        var acceptedViewAs = window.Action.ConversionChain.Select((source, position) => (source, position)).Any(pair => IsAcceptedViewAsInvocation(pair.source) && invocation.ActorSeat == pair.source.OwnerSeat && invocation.SkillKey == pair.source.SkillId && invocation.SkillInstanceId == pair.source.SkillInstanceId && invocation.OccurrenceKey == "conversion:" + window.Action.ActionId + ":" + pair.position);
        var acceptedEquipment = invocation.SkillInstanceId.Length == 0 && SkillInvocationEquipmentSources(window.Action).Any(source => source.Actor == invocation.ActorSeat && source.Skill == invocation.SkillKey && source.Occurrence == invocation.OccurrenceKey);
        return (acceptedViewAs || acceptedEquipment) && HasSkillInvocationObserverSuffix(index);
    }

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
            else if (!global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(current, parent) && !_coreObserverSubtreesOperations.IsMovementObserverEdge(child))
                return false;
        }

        return true;
    }

    internal bool IsProgramWaitingForSkillInvocation(ProgramSkillFrame program)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == program.Id);
        if (program.InstructionIndex != 0 || index < 0 || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not SkillInvocationFrame invocation || invocation.ReturnKind != SkillInvocationReturnKind.RuntimeFrame || invocation.ParentFrameId != program.Id || invocation.ActorSeat != (program.ActivatingSeat ?? program.OwnerSeat) || invocation.SkillKey != program.SkillId || invocation.SkillInstanceId != program.SkillInstanceId || invocation.GameplayHash != program.GameplayHash || invocation.OccurrenceKey != "program:" + global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(program) || invocation.SourceFrameFingerprint is not { Length: 64 } fingerprint || fingerprint != SkillInvocationSourceFingerprint(program) || !SkillInvocationStructuralEdge(program, invocation) || program.SelectedCardIds.Distinct().Count() != program.SelectedCardIds.Count || program.SelectedTargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationStartedEvent>().Count(e => e == new SkillInvocationStartedEvent(invocation.Id, program.Id, invocation.ActorSeat, invocation.SkillKey, invocation.OccurrenceKey, invocation.SkillInstanceId, invocation.SkillName, invocation.GameplayHash, invocation.RoundNumber, invocation.TurnNumber, invocation.TurnOwnerSeat, invocation.ReturnKind)) != 1)
            return false;
        if (program.TriggerId is { } trigger)
            return program.WindowContext is { } context && _coreProgramLifecycleOperations.GetProgramTrigger(program).IsSkillInvocation && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == program.Id && e.SkillId == program.SkillId && e.BindingId == trigger && e.SkillInstanceId == program.SkillInstanceId && e.OwnerSeat == program.OwnerSeat && e.Window == context.Window) == 1;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == program.Id && e.OwnerSeat == program.OwnerSeat && e.SkillId == program.SkillId && e.ActivationId == program.ActivationId) == 1 && (program.ActivatingSeat is null || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSharedActivationStartedEvent>().Count(e => e.FrameId == program.Id && e.ActorSeat == invocation.ActorSeat && e.Source == new CardConversionSource(program.SkillId, program.ActivationId, program.OwnerSeat, program.SkillInstanceId)) == 1);
    }

    internal bool HasExactBoundarySkillInvocation()
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Discard || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not SkillInvocationFrame { ParentFrameId: 0, ReturnKind: SkillInvocationReturnKind.SkipDiscard } root || root.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || root.TurnOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || root.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !root.OccurrenceKey.StartsWith($"discard-policy:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:", StringComparison.Ordinal))
            return false;
        var policyId = root.OccurrenceKey[$"discard-policy:{(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber)}:".Length..];
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var turn = Array.FindLastIndex(facts, e => e is TurnStartedEvent t && t.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && t.ActorSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat);
        var accepted = Array.FindLastIndex(facts, e => e is ProgramCardPolicyResolvedEvent p && p.OwnerSeat == root.ActorSeat && p.SkillId == root.SkillKey && p.PolicyId == policyId && p.Applied);
        var started = Array.FindLastIndex(facts, e => e is SkillInvocationStartedEvent s && s.FrameId == root.Id && s.ParentFrameId == 0 && s.ActorSeat == root.ActorSeat && s.SkillKey == root.SkillKey && s.OccurrenceKey == root.OccurrenceKey && s.ReturnKind == SkillInvocationReturnKind.SkipDiscard);
        if (turn < 0 || accepted <= turn || started <= accepted)
            return false;
        return HasSkillInvocationObserverSuffix(0);
    }

    internal bool IsExactSkillInvocationDying(DyingFrame? dying)
    {
        if (dying is not { Continuation: DyingContinuationKind.SkillInvocation, KillerSeat: null } || _coreDyingCursorRuntimeOperations.ActiveDying?.Id != dying.Id)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is SkillInvocationFrame f && SkillInvocationStructuralEdge(f, dying) && f.Cursor < f.Reactions.Count && f.Reactions[f.Cursor] is { Kind: SkillInvocationReactionKind.Luxin, HpDelta: -1 } reaction && _coreProgramEventHistoryOperations.ProgramEventHistory<SkillInvocationHpPaidEvent>().Count(e => e.FrameId == f.Id && e.Cursor == f.Cursor && e.OwnerSeat == reaction.OwnerSeat && e.TargetSeat == dying.VictimSeat && e.Delta == -1 && e.After == e.Before - 1 && e.After <= 0) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ShenHuaTuoLuxinConsumedEvent>().Any(e => e.LuxinFrameId == reaction.LuxinFrameId && e.InvocationFrameId == f.Id && e.TargetSeat == dying.VictimSeat && e.HpDelta == -1);
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 38700, group: 38700)]
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
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, invocation) && invocation.Stage == SkillInvocationStage.Drawing && draw.Return.Kind == NativeDrawAttemptReturnKind.SkillInvocation;
        return child is DyingFrame dying && invocation.Stage == SkillInvocationStage.Children && dying.ParentFrameId == invocation.Id && dying.Id == invocation.DyingFrameId && dying.VictimSeat == invocation.ActorSeat && dying.Continuation == DyingContinuationKind.SkillInvocation && dying.KillerSeat is null;
    }

    internal void AssertSkillInvocations()
    {
        for (var i = 0; i < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            var current = _resolutionStackOperations._resolutionStack[i];
            if (current is SkillInvocationFrame f)
            {
                AssertSkillInvocation(f);
                if (f.ParentFrameId != 0 && (i == 0 || !SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], f)))
                    throw new InvalidOperationException("A skill invocation lost its exact suspended source edge.");
            }

            if (current is CardsMovedTriggerWindowFrame { ResumeSkillInvocationFrameId: not null } movement && (i == 0 || !SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], movement)))
                throw new InvalidOperationException("Jingyu lost its exact paid DrawPile movement child.");
            if (current is DyingFrame { Continuation: DyingContinuationKind.SkillInvocation } dying && (i == 0 || !SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], dying)))
                throw new InvalidOperationException("Luxin lost its exact invocation-owned dying child.");
            if (current is RecoveryReplacementFrame { Return.Continuation: PostEventContinuation.SkillInvocation } recovery && (i == 0 || !SkillInvocationStructuralEdge(_resolutionStackOperations._resolutionStack[i - 1], recovery)))
                throw new InvalidOperationException("Luxin lost its exact invocation recovery producer.");
        }
    }

    [ProgramPipelineHook(ProgramPipelinePhase.BeginMovementBatch, 100)]
    internal void CaptureMovementBatchStartInvocationOrigin(CardMovementBatchBuilder batch) => batch.InvocationOrigin = SkillInvocationMovementOrigin();
}
