namespace CardGame.Core;

internal interface IProgramPhaseFrameQueries : IProgramSelectionQuery
{
    bool IsExactOwnPlayEndingProgram(ProgramSkillFrame frame);
    bool IsForeignActualTurnPindian(ProgramSkillFrame frame);
}

// Phase-bound operations share the same original parent/candidate proof without
// depending on a contest producer or retaining an alternate phase cursor.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramPhaseFrameQueries(IProgramRuntimeFrames frames, IProgramTurnQueries turn,
    IProgramPhaseQueries phase, INativeFrameRoutingFacts nativeFrames) : IProgramPhaseFrameQueries
{
    public bool IsExactOwnPlayEndingProgram(ProgramSkillFrame frame)
    {
        if (frames.Find(frame.Id) is null || frame.TriggerId is null || frame.SelectedCardIds.Count != 0 ||
            frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayEnding } context ||
            frames.ParentOf(frame.Id) is not ProgramLifecycleTriggerWindowFrame window ||
            window.Id != context.ParentFrameId || window.Window != context.Window ||
            window.OwnerSeat != frame.OwnerSeat || context.OwnerSeat != frame.OwnerSeat ||
            window.OwnerSeat != turn.CurrentSeat || phase.Phase != TurnPhase.Play ||
            turn.TurnOwnerSeat != turn.CurrentSeat || turn.TurnOwnerTurn != turn.TurnNumber ||
            window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count)
            return false;
        return CandidateMatches(frame, window.Candidates[window.CandidateIndex]) && frame.ActivationId == frame.TriggerId;
    }

    public bool IsForeignActualTurnPindian(ProgramSkillFrame frame) =>
        frame.WindowContext is { Window: SkillProgramTriggerWindow.OtherActualTurnStarted } context &&
        context.SourceSeat == turn.CurrentSeat && context.TargetSeat == turn.CurrentSeat &&
        context.OwnerSeat == frame.OwnerSeat && frame.OwnerSeat != turn.CurrentSeat &&
        SingleForeignActualTurnParent(context.ParentFrameId) is { } parent &&
        parent.OwnerSeat == turn.CurrentSeat && parent.ActualTurnNumber == turn.TurnNumber &&
        parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count &&
        CandidateMatches(frame, parent.Candidates[parent.CandidateIndex]);

    private ForeignActualTurnStartWindowFrame? SingleForeignActualTurnParent(long parentFrameId)
    {
        ForeignActualTurnStartWindowFrame? result = null;
        for (var index = 0; index < nativeFrames.FrameCount; index++)
        {
            if (nativeFrames.FrameAt(index) is not ForeignActualTurnStartWindowFrame candidate || candidate.Id != parentFrameId) continue;
            if (result is not null) throw new InvalidOperationException("Sequence contains more than one matching element");
            result = candidate;
        }
        return result;
    }

    private static bool CandidateMatches(ProgramSkillFrame frame, ProgramTriggerCandidate candidate) =>
        RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(frame, candidate);
}
