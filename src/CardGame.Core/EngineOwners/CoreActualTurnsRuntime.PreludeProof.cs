#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreActualTurnsRuntime
{
    internal ProgramExtraTurnPendedEvent? PendingExtraTurnForPrelude(int recipientSeat, string skillId)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind != ActualTurnKind.Extra ||
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat != recipientSeat ||
            !_coreMatchParticipantsPlayersAccess._players[recipientSeat].IsAlive)
            return null;

        var pending = _eventHistory.Read<ProgramExtraTurnPendedEvent>().LastOrDefault();
        return pending is not null && pending.SkillId == skillId && pending.Seat == recipientSeat ? pending : null;
    }

    internal bool ValidBeforeTurnPrelude(BeiMiHuBeforeTurnProof proof)
    {
        if (_resolutionStackOperations._resolutionStack.FirstOrDefault() is not BeiMiHuTurnPreludeFrame parent ||
            parent.Id != proof.PreludeFrameId || parent.Proof != proof ||
            proof.UpcomingOwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat ||
            proof.UpcomingTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber + 1 ||
            proof.ActualRoundNumber != _coreActualRoundProgressRoundNumberAccess._roundNumber ||
            proof.RoundStartedEventSequence < 1 || parent.Stage is < 0 or > 2)
            return false;

        var ordinal = proof.RoundStartedEventSequence - 1;
        var events = _journal.Complete().ToArray();
        if (ordinal > int.MaxValue || events.ElementAtOrDefault((int)ordinal) is not RoundStartedEvent round ||
            round.RoundNumber != proof.ActualRoundNumber ||
            ordinal != Array.FindLastIndex(events, item => item is RoundStartedEvent latest && latest.RoundNumber == proof.ActualRoundNumber))
            return false;

        var lastTurn = Array.FindLastIndex(events, item => item is TurnStartedEvent);
        // An extra turn keeps the current round. A new round can outlive the
        // actor it originally named, and only that actor claims its start.
        var beganThisRound =
            _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.Kind == ActualTurnKind.Normal &&
            ordinal > lastTurn && round.ActorSeat == proof.UpcomingOwnerSeat;
        return parent.RoundAdvanced == beganThisRound;
    }

    internal bool ValidBeforeTurnPreludeWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        if (window.BeforeTurnPreludeProof is not { } proof || !ValidBeforeTurnPrelude(proof) ||
            _resolutionStackOperations._resolutionStack.FirstOrDefault() is not BeiMiHuTurnPreludeFrame parent ||
            parent.WindowId != window.Id || window.OwnerSeat != proof.UpcomingOwnerSeat ||
            window.CandidateIndex < 0 || window.CandidateIndex > window.Candidates.Count)
            return false;

        return parent.Stage switch
        {
            0 => parent.RoundAdvanced && window.Window == SkillProgramTriggerWindow.RoundStarting &&
                window.Continuation == ProgramLifecycleContinuation.BeiMiHuPreludeRound,
            1 => window.Window == SkillProgramTriggerWindow.TurnBeforeStart &&
                window.Continuation == ProgramLifecycleContinuation.BeiMiHuPreludeOwn,
            _ => false
        };
    }

    internal void ReturnBeforeTurnPreludeWindow(ProgramLifecycleTriggerWindowFrame window)
    {
        if (!ValidBeforeTurnPreludeWindow(window) || window.CandidateIndex != window.Candidates.Count ||
            _resolutionStackOperations._resolutionStack.LastOrDefault() is not BeiMiHuTurnPreludeFrame parent)
            throw new InvalidOperationException("Completed before-turn work lost its singular parent return.");

        _coreRuntimeOperations.ReplaceRuntimeTop(parent with { Stage = parent.Stage + 1, WindowId = null });
        _coreRuntimeOperations.AdvanceRuntimeFrame(parent.Id);
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 180, SkillProgramTriggerWindow.TurnBeforeStart)]
    internal bool ProveBeforeTurnPrelude(CharacterState owner, ProgramSkillWindowContext context) =>
        context.BeforeTurnPreludeProof is { } proof && ValidBeforeTurnPrelude(proof) &&
        context.SourceSeat == proof.UpcomingOwnerSeat && context.TargetSeat == proof.UpcomingOwnerSeat &&
        owner.Seat == proof.UpcomingOwnerSeat;
}
