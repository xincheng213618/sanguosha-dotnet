namespace CardGame.Core;

public sealed record AfterTurnEndedProgramChild(long FrameId, ProgramTriggerCandidate Candidate);
public sealed record AfterTurnEndedWindow(IReadOnlyList<TurnEndingBoundaryItem> Items,
    int ItemIndex = 0, AfterTurnEndedProgramChild? CurrentChild = null);
