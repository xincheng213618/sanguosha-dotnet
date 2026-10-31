#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDeferredTurnEndPreludesRuntime
{
    internal CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(CardMovementBatchContext batch, long? resumeProgramFrameId = null) => _cardsMovedWindowOperations.Get().CreateWindow(batch, resumeProgramFrameId);
    internal bool IsActualDeferredTurnEndPrelude(DeferredTurnEndFrame parent) => _corePrivateTurnHoldsOperations.HasActualTurnEndMovementPrelude && parent.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && parent.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Finished && parent.Current is null && parent.ItemIndex == 0 && parent.AfterTurnEnded is null && parent.Prelude is { Completed: false } && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnEndedEvent>().Any(e => e.TurnNumber == parent.TurnNumber && e.ActorSeat == parent.OwnerSeat);
    internal bool ContinueDeferredTurnEndPrelude(DeferredTurnEndFrame parent)
    {
        if (!IsActualDeferredTurnEndPrelude(parent) || parent.Prelude!.CurrentChild is not null || _resolutionStackOperations._resolutionStack[^1] != parent)
            throw new InvalidOperationException("The end prelude lost its actual turn or retained an unfinished movement child.");
        while (_pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Any(b => b.AwaitingProgramFrameId is null))
        {
            var pending = _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.AwaitingProgramFrameId is null).OrderBy(b => b.Id).First();
            _pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Remove(pending);
            var batch = pending with
            {
                Movements = Array.AsReadOnly(pending.Movements.ToArray()),
                SourceCounts = Array.AsReadOnly(pending.SourceCounts.ToArray()),
                DestinationCounts = pending.DestinationCounts is null ? null : Array.AsReadOnly(pending.DestinationCounts.ToArray())
            };
            var window = CreateCardsMovedProgramWindow(batch);
            if (window is null)
                continue;
            _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Prelude = parent.Prelude with { CurrentChild = new(window.Id, batch) } });
            _coreRuntimeOperations.PushRuntimeFrame(window with { DeferredTurnEndReturn = new(parent.Id, parent.OwnerSeat, parent.TurnNumber, batch.Id) });
            _coreRuntimeOperations.AdvanceRuntimeTop<CardsMovedTriggerWindowFrame>();
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Prelude = parent.Prelude with { Completed = true } });
        return false;
    }

    internal static bool SameDeferredTurnEndBatch(CardMovementBatchContext left, CardMovementBatchContext right) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_DeferredTurnEndPreludes.SameDeferredTurnEndBatch(left, right);
    internal bool MatchesDeferredTurnEndPreludeChild(DeferredTurnEndFrame parent, CardsMovedTriggerWindowFrame child) => IsActualDeferredTurnEndPrelude(parent) && parent.Prelude!.CurrentChild is { } current && current.FrameId == child.Id && child.ResumeProgramFrameId is null && child.DeferredTurnEndReturn is { } receipt && receipt.ParentFrameId == parent.Id && receipt.OwnerSeat == parent.OwnerSeat && receipt.TurnNumber == parent.TurnNumber && receipt.BatchId == current.Batch.Id && child.Id == child.Batch.Id && child.Batch.TurnNumber == parent.TurnNumber && child.Batch.AwaitingProgramFrameId is null && SameDeferredTurnEndBatch(current.Batch, child.Batch);
    internal void ReturnDeferredTurnEndPrelude(CardsMovedTriggerWindowFrame child)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not DeferredTurnEndFrame parent || !MatchesDeferredTurnEndPreludeChild(parent, child) || child.CandidateIndex != child.Candidates.Count)
            throw new InvalidOperationException("A completed movement batch lost its exact owning end prelude or full batch receipt.");
        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Prelude = parent.Prelude!with { CurrentChild = null } });
        _coreDeferredHandAlignmentsOperations.ContinueDeferredTurnEnd();
    }

    internal bool HasDeferredTurnEndPreludeBoundary(DeferredTurnEndFrame parent)
    {
        if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[1] is not CardsMovedTriggerWindowFrame window || !MatchesDeferredTurnEndPreludeChild(parent, window) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            throw new InvalidOperationException("The end prelude lost its exact movement batch child.");
        var candidate = window.Candidates[window.CandidateIndex];
        var context = _coreCardMovementProgramsOperations.CreateCardsMovedProgramContext(window, candidate);
        if (_resolutionStackOperations._resolutionStack.Count == 2 && window.Step == ResolutionFrameStep.AwaitingResponse && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Optional && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } decision && decision.PlayerSeat == (context.OptionalChooserSeat ?? candidate.OwnerSeat) && decision.SourceSeat == (context.OptionalChooserSeat is not null ? candidate.OwnerSeat : context.SourceSeat) && // Optional prompts use the owner when the movement context has no explicit target.
 // Keep this boundary aligned with ExposeProgramTriggerDecision.
        decision.TargetSeat == (context.OptionalChooserSeat ?? context.TargetSeat ?? candidate.OwnerSeat) && decision.Choices.Count == 2 && decision.Choices.All(c => c.Parameters.GetValueOrDefault("skill-id") == candidate.SkillId && c.Parameters.GetValueOrDefault("binding-id") == candidate.BindingId && c.Parameters.GetValueOrDefault("skill-instance-id") == candidate.SkillInstanceId) && decision.Choices.Select(c => c.Parameters.GetValueOrDefault("program-action")).Order(StringComparer.Ordinal).SequenceEqual(["activate", "skip"]))
            return true;
        if (_resolutionStackOperations._resolutionStack.Count < 3 || _resolutionStackOperations._resolutionStack[2] is not ProgramSkillFrame child || child.WindowContext != context || child.OwnerSeat != candidate.OwnerSeat || child.SkillId != candidate.SkillId || child.SkillInstanceId != candidate.SkillInstanceId || child.TriggerId != candidate.BindingId || child.ActivationId != candidate.BindingId || child.GameplayHash != candidate.GameplayHash)
            throw new InvalidOperationException("The end prelude movement window lost its exact candidate, prompt or program child.");
        return true;
    }
}
