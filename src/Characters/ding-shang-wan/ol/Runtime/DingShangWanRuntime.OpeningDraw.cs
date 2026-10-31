namespace CardGame.Core;

public sealed record DingShangWanOpeningDrawReturnedEvent(long FrameId, int RecipientSeat, int RequestedCount,
    long? AttemptFrameId, long SequenceBefore, long SequenceAfter, int ActualCount, int ReplacementGainCount) : IGameEvent;

internal sealed partial class DingShangWanRuntime
{
    private void BeginDingShangWanOpeningDraw(ProgramSkillFrame frame, int seat, int count)
    {
        if (frame.DingShangWan is not { Operation: SkillProgramEffectOp.InitializeFudaoHandRecord,
                Stage: DingShangWanStage.OpeningDraw, OpeningDrawOutcome: null } receipt ||
            seat != frame.OwnerSeat || count != 3 || frame.NativeDraw is not null || frame.PendingMovementContinuation is not null)
            throw new InvalidOperationException("Fudao requires one exact new opening draw request.");
        frames.ReplaceTop(frame = frame with { DingShangWan = receipt with
            { Payer = seat, Before = PhysicalMovementSequence, After = PhysicalMovementSequence,
                ActualCount = 0, Candidates = [], Selected = [], WoodenOxGrainIds = [], Reshuffles = [] } });
        operationDraws.BeginOperationProgramDraw(frame, seat, count, new(DingShangWanDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.InitializeFudaoHandRecord)]
    private void ReturnDingShangWanOpeningDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.DingShangWan is not { Operation: SkillProgramEffectOp.InitializeFudaoHandRecord,
                Stage: DingShangWanStage.OpeningDraw, OpeningDrawOutcome: null } receipt ||
            frame.NativeDraw is not null || frame.PendingMovementContinuation is not null || outcome.RecipientSeat != frame.OwnerSeat ||
            outcome.RequestedCount != 3 || outcome.SequenceBefore < receipt.Before || outcome.SequenceAfter != PhysicalMovementSequence ||
            history.Read<DingShangWanOpeningDrawReturnedEvent>().Any(fact => fact.FrameId == frame.Id))
            throw new InvalidOperationException("Fudao cannot substitute or repeat its native opening draw return.");
        frames.ReplaceTop(frame = frame with { DingShangWan = receipt with
            { OpeningDrawOutcome = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceBefore },
            PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        events.AdvanceFact(new DingShangWanOpeningDrawReturnedEvent(frame.Id, outcome.RecipientSeat, outcome.RequestedCount,
            outcome.AttemptFrameId, outcome.SequenceBefore, outcome.SequenceAfter, outcome.ActualDrawIds.Count, outcome.ReplacementGainedIds.Count));
        RecordDingShangWanMovement(frame.Id);
        ContinueDingShangWan(frame.Id);
    }

}
