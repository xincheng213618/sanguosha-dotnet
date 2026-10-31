namespace CardGame.Core;

// Reads the scheduler's original bottom frame and validates its clean phase
// boundary. Pending continuations and prompts remain with their native owners.
[ProgramRuntimeModule]
internal sealed class ProgramBoundaryInvariantRuntime(INativeFrameRoutingFacts frames,
    IProgramTurnQueries turn, IProgramPhaseQueries phase, IProgramBoundaryInvariantFacts facts)
{
    internal bool HasProgramLifecycleBoundaryFrame()
    {
        if (frames.FrameCount == 0 || frames.FrameAt(0) is not ProgramLifecycleTriggerWindowFrame frame)
            return false;
        if (frame.OwnerSeat != turn.CurrentSeat && frame.CharacterStateContinuation is null || frame.CandidateIndex < 0 ||
            frame.CandidateIndex >= frame.Candidates.Count)
            throw new InvalidOperationException("The lifecycle boundary frame lost its owner or candidate cursor.");
        if (frame.Window == SkillProgramTriggerWindow.DrawPhaseEnded &&
            (frame.DrawPhaseEndedDelayedEffects is not { } effects || effects is < 0 or > 15 ||
                (effects & facts.SkipDrawPhaseMask) != 0 || frame.Continuation != ProgramLifecycleContinuation.CompleteDrawPhaseEnded) ||
            frame.Window != SkillProgramTriggerWindow.DrawPhaseEnded && frame.DrawPhaseEndedDelayedEffects is not null)
            throw new InvalidOperationException("Draw end continuation must describe an actual completed draw phase.");
        if (frame.Window == SkillProgramTriggerWindow.PlayEnding && phase.Phase != TurnPhase.Play && !facts.IsTerminalPhaseHandDebtBoundary(frame))
            throw new InvalidOperationException("A PlayEnding lifecycle frame must remain in its Play phase.");
        if (frame.Window is (SkillProgramTriggerWindow.DrawPhaseStarting or
            SkillProgramTriggerWindow.AfterNormalDraw or SkillProgramTriggerWindow.DrawPhaseEnded) && phase.Phase != TurnPhase.Draw)
            throw new InvalidOperationException("A DrawPhaseStarting lifecycle frame must remain in its Draw phase.");
        if (frame.Window == SkillProgramTriggerWindow.DiscardPhaseStarting && phase.Phase != TurnPhase.Discard)
            throw new InvalidOperationException("A DiscardPhaseStarting lifecycle frame must remain in its Discard phase.");
        if (frame.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && phase.Phase != TurnPhase.Discard)
            throw new InvalidOperationException("A DiscardPhaseEnded lifecycle frame must remain in its Discard phase.");
        if (frame.Window != SkillProgramTriggerWindow.DrawPhaseStarting && frame.NormalDrawAdjustment != 0)
            throw new InvalidOperationException("Only a DrawPhaseStarting lifecycle frame may adjust normal draws.");
        if (frame.Window == SkillProgramTriggerWindow.GameStarting && phase.Phase != TurnPhase.NotStarted && !facts.IsExactHellAppearanceWindow(frame) && !facts.IsExactShanheMarketOpeningWindow(frame))
            throw new InvalidOperationException("A game-starting lifecycle frame must precede the first turn.");
        if (frame.Window == SkillProgramTriggerWindow.RoundStarting &&
            (frame.Continuation != ProgramLifecycleContinuation.RoundProgramsTurnStart || facts.RoundNumber <= 0 ||
             facts.TurnKind == ActualTurnKind.Extra))
            throw new InvalidOperationException("A round-starting boundary requires its normal turn continuation and actual round.");
        if (frame.Window is not (SkillProgramTriggerWindow.ActualPhaseSkipped or SkillProgramTriggerWindow.RoundStarting or SkillProgramTriggerWindow.GameStarting or SkillProgramTriggerWindow.TurnStartBeforeNormalFlow or
            SkillProgramTriggerWindow.DrawPhaseStarting or
            SkillProgramTriggerWindow.AfterNormalDraw or SkillProgramTriggerWindow.DrawPhaseEnded or
            SkillProgramTriggerWindow.PlayEnding or
            SkillProgramTriggerWindow.DiscardPhaseStarting or
            SkillProgramTriggerWindow.DiscardPhaseEnded or SkillProgramTriggerWindow.JudgmentPhaseStarting or
            SkillProgramTriggerWindow.CharacterTurnedOver or SkillProgramTriggerWindow.CharacterTurnedFaceUp or SkillProgramTriggerWindow.CharacterEnteredChain))
            throw new InvalidOperationException("This lifecycle window cannot own a clean phase boundary.");
        return true;
    }

    internal bool HasTurnEndingBoundaryFrame()
    {
        if (frames.FrameCount == 0 || frames.FrameAt(0) is not TurnEndingBoundaryFrame frame)
            return false;
        if (frame.OwnerSeat != turn.CurrentSeat || frame.TurnNumber != turn.TurnNumber ||
            frame.Facts is null || frame.ItemIndex < 0 || frame.ItemIndex >= frame.Items.Count)
            throw new InvalidOperationException("The turn-ending boundary lost its owner, turn or item cursor.");
        if (frames.FrameCount == 1 && frame.Step == ResolutionFrameStep.AwaitingResponse)
        {
            var decisionKind = frame.Items[frame.ItemIndex].Kind switch
            {
                TurnEndingBoundaryItemKind.Program => DecisionKind.ProgramTrigger,
                _ => throw new InvalidOperationException("Unsupported turn-ending item kind.")
            };
            if (facts.PendingDecisionKind != decisionKind)
                throw new InvalidOperationException("The turn-ending boundary is waiting without its matching prompt.");
        }
        return true;
    }

    internal bool HasPlayPhaseStartingBoundaryFrame()
    {
        if (frames.FrameCount == 0 || frames.FrameAt(0) is not PlayPhaseStartingBoundaryFrame frame)
            return false;
        if (frame.OwnerSeat != turn.CurrentSeat || frame.ItemIndex < 0 ||
            frame.ItemIndex > frame.Items.Count)
            throw new InvalidOperationException("The play-phase-starting boundary lost its owner or item cursor.");
        if (phase.Phase != TurnPhase.Play)
            throw new InvalidOperationException("A play-phase-starting boundary must remain in its Play phase.");
        if (frames.FrameCount == 1 && frame.Step == ResolutionFrameStep.AwaitingResponse)
        {
            var decisionKind = frame.Items[frame.ItemIndex].Kind switch
            {
                TurnEndingBoundaryItemKind.Program => DecisionKind.ProgramTrigger,
                _ => throw new InvalidOperationException("Unsupported play-phase-starting item kind.")
            };
            if (facts.PendingDecisionKind != decisionKind)
                throw new InvalidOperationException("The play-phase-starting boundary is waiting without its matching prompt.");
        }
        return true;
    }
}
