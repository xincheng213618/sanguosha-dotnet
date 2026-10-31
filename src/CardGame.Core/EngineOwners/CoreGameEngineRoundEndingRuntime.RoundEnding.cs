#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundEndingRuntime
{
    internal bool TracksRoundEnding => _modeRulesOperations._modeRules.RuntimeRequirements.TracksRoundEnding || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerWindow(SkillProgramTriggerWindow.RoundEnding);

    internal bool TryBeginRoundEndingBoundary(CharacterState previous, bool skipped)
    {
        // Games without this boundary retain their existing event/sequence stream.
        if (!TracksRoundEnding || _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber == 0 || _coreMatchOutcomeWinnerAccess._winner != Winner.None || _coreProgramEventHistoryOperations.ProgramEventHistory<RoundEndedEvent>().Any(e => e.RoundNumber == _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber))
            return false;
        var progress = _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression;
        if (progress.PendingBeneficiarySeat is { } beneficiary && _coreMatchParticipantsPlayersAccess._players[beneficiary].IsAlive)
            return false;
        var resume = progress.NormalResumeSeat ?? _coreTableSeatQueriesOperations.FindNextAliveSeat(previous.Seat);
        var next = _coreMatchParticipantsPlayersAccess._players[resume].IsAlive ? resume : _coreTableSeatQueriesOperations.FindNextAliveSeat(resume);
        if (!_coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundTurnSeats.Contains(next))
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.Count != 0 || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Finished || progress.OwnerSeat != previous.Seat || previous.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || progress.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber)
            throw new InvalidOperationException("A round end lost its clean old actual turn.");
        var inherited = Array.AsReadOnly(_pendingCardsMovedBatchesOperations._pendingCardsMovedBatches.Where(b => b.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber && b.AwaitingProgramFrameId is null && (b.ParentFrameId is null || !_resolutionStackOperations._resolutionStack.Any(f => f.Id == b.ParentFrameId))).Select(b => b.Id).ToArray());
        var frame = new RoundEndingBoundaryFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber, previous.Seat, next, skipped, progress.Kind, progress.NormalResumeSeat, inherited);
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundEndingStartedEvent(frame.Id, frame.RoundNumber, frame.ActualTurnNumber, frame.OwnerSeat, frame.NextNormalSeat));
        _coreRuntimeOperations.AdvanceRuntimeFrame(frame.Id);
        return true;
    }

    internal bool IsRoundEndingMovement(RoundEndingBoundaryFrame parent, CardMovementBatchContext batch) => batch.TurnNumber == parent.ActualTurnNumber && batch.AwaitingProgramFrameId is null && (batch.ParentFrameId == parent.Id || parent.InheritedMovementBatchIds.Contains(batch.Id));
    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 150, NativeFrameAutomaticPending.RequireNoneAfterFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(RoundEndingBoundaryFrame), 360)]
    internal void ContinueRoundEndingBoundary(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RoundEndingBoundaryFrame frame || frame.Id != frameId || frame.ActiveWindowFrameId is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            throw new InvalidOperationException("A round end cannot advance before its exact child returns.");
        if (frame.Stage == RoundEndingBoundaryStage.ReturningHolds)
        {
            // Commit the cursor before returning old round holds: their real movements may open children.
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Stage = RoundEndingBoundaryStage.Programs });
            _characterGuanYuRuntimeOperations.ReturnDemonRoundHolds();
        }

        if (_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            return;
        if (frame.Stage == RoundEndingBoundaryStage.Programs)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { Stage = RoundEndingBoundaryStage.Completing });
            if (_coreMatchOutcomeWinnerAccess._winner == Winner.None)
            {
                var participants = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).ToArray();
                var facts = new System.Collections.ObjectModel.ReadOnlyDictionary<int, SkillProgramTriggerFacts>(participants.ToDictionary(p => p.Seat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts));
                var candidates = Array.AsReadOnly(participants.SelectMany(p => _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(p, SkillProgramTriggerWindow.RoundEnding, facts[p.Seat])).OrderBy(c => _coreTableOrderOperations.RelativeTableOrder(frame.OwnerSeat, c.OwnerSeat)).ThenByDescending(c => c.Priority).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.BindingId, StringComparer.Ordinal).ToArray());
                if (candidates.Count != 0)
                {
                    var child = new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, frame.OwnerSeat, SkillProgramTriggerWindow.RoundEnding, candidates, ProgramLifecycleContinuation.CompleteRoundEndingBoundary, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]))
                    {
                        RoundEndingBoundaryId = frame.Id,
                        ParticipantFacts = facts
                    };
                    _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ActiveWindowFrameId = child.Id });
                    _coreRuntimeOperations.PushRuntimeFrame(child);
                    _coreRuntimeOperations.AdvanceRuntimeFrame(child.Id);
                    return;
                }
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoundEndedEvent(frame.Id, frame.RoundNumber, frame.ActualTurnNumber, frame.OwnerSeat));
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.RoundEndingBoundary);
        _coreActualTurnsOperations.CompleteActualTurnCursorAfterRoundEnding(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkippedTurn);
    }

    internal bool IsExactRoundEndingCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.RoundEnding || _resolutionStackOperations._resolutionStack.FirstOrDefault()is not RoundEndingBoundaryFrame root || _resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[1] is not ProgramLifecycleTriggerWindowFrame child || child.Id != context.ParentFrameId || child.Id != root.ActiveWindowFrameId || child.RoundEndingBoundaryId != root.Id || child.Window != context.Window || child.Continuation != ProgramLifecycleContinuation.CompleteRoundEndingBoundary || root.Stage != RoundEndingBoundaryStage.Completing || root.RoundNumber != _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber || root.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || root.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || context.OwnerSeat != candidate.OwnerSeat || context.SourceSeat != root.OwnerSeat || context.TargetSeat != root.OwnerSeat || child.CandidateIndex < 0 || child.CandidateIndex >= child.Candidates.Count || child.Candidates[child.CandidateIndex] != candidate)
            return false;
        return true;
    }

    internal void ReturnRoundEndingLifecycle(ProgramLifecycleTriggerWindowFrame child)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RoundEndingBoundaryFrame root || root.ActiveWindowFrameId != child.Id || child.RoundEndingBoundaryId != root.Id || child.Window != SkillProgramTriggerWindow.RoundEnding || child.CandidateIndex != child.Candidates.Count || root.Stage != RoundEndingBoundaryStage.Completing)
            throw new InvalidOperationException("A round end lost its exact lifecycle return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(root with { ActiveWindowFrameId = null });
        _coreRuntimeOperations.AdvanceRuntimeFrame(root.Id);
    }

    internal void ReturnRoundEndingMovement(CardsMovedTriggerWindowFrame child)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not RoundEndingBoundaryFrame root || root.Id != child.ResumeRoundEndingBoundaryId || root.ActiveWindowFrameId != child.Id || !IsRoundEndingMovement(root, child.Batch))
            throw new InvalidOperationException("A round end lost its exact movement return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(root with { ActiveWindowFrameId = null });
        _coreRuntimeOperations.AdvanceRuntimeFrame(root.Id);
    }

    [ProgramWindowReturn(SkillProgramTriggerWindow.RoundEnding)]
    internal void ReturnRoundEndingProgram(ProgramSkillFrame frame, bool completed)
    {
        if (frame.WindowContext is not { } context || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramLifecycleTriggerWindowFrame child || child.CandidateIndex >= child.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, child.Candidates[child.CandidateIndex]) || !IsExactRoundEndingCandidate(child.Candidates[child.CandidateIndex], context))
            throw new InvalidOperationException("A round-end program lost its exact current binding.");
        _coreProgramLifecycleOperations.AdvanceProgramLifecycleCursor(child);
        _coreRuntimeOperations.AdvanceRuntimeFrame(child.Id);
    }

    internal bool HasRoundEndingBoundaryFrame()
    {
        if (_resolutionStackOperations._resolutionStack.FirstOrDefault()is not RoundEndingBoundaryFrame root)
            return false;
        if (!TracksRoundEnding || root.RoundNumber != _coreActualRoundProgressRoundNumberRoundTurnSeatsAccess._roundNumber || root.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || root.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || root.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Finished || root.OriginalActualTurnKind != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind || root.NormalResumeSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.NormalResumeSeat || !Enum.IsDefined(root.Stage) || root.InheritedMovementBatchIds is not System.Collections.IList { IsReadOnly: true } || _coreProgramEventHistoryOperations.ProgramEventHistory<RoundEndingStartedEvent>().Count(e => e == new RoundEndingStartedEvent(root.Id, root.RoundNumber, root.ActualTurnNumber, root.OwnerSeat, root.NextNormalSeat)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<RoundEndedEvent>().Any(e => e.RoundNumber == root.RoundNumber))
            throw new InvalidOperationException("A round-end root lost its old round, actual turn or once-only issuance.");
        if (root.ActiveWindowFrameId is { } id)
        {
            if (_resolutionStackOperations._resolutionStack.Count < 2 || _resolutionStackOperations._resolutionStack[1].Id != id || (_resolutionStackOperations._resolutionStack[1] is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.RoundEnding } lifecycle || lifecycle.RoundEndingBoundaryId != root.Id || root.Stage != RoundEndingBoundaryStage.Completing) && (_resolutionStackOperations._resolutionStack[1] is not CardsMovedTriggerWindowFrame movement || movement.ResumeRoundEndingBoundaryId != root.Id || !IsRoundEndingMovement(root, movement.Batch)))
                throw new InvalidOperationException("A round end retained an unissued first child.");
        }
        else if (_resolutionStackOperations._resolutionStack.Count != 1)
            throw new InvalidOperationException("A round end has an unowned child.");
        return true;
    }
}
