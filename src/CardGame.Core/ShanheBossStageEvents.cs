namespace CardGame.Core;

public sealed record ShanheBossStageStartedEvent(int BossSeat, string PreviousGeneralId, string GeneralId,
    int TurnNumber, int MaxHp, int Hp) : IGameEvent;
public sealed record ShanheBossResolutionCancelledEvent(long FrameId, ResolutionFrameKind Kind, int TurnNumber) : IGameEvent;
public sealed record ShanheBossTurnInterruptedEvent(int TurnNumber, int PlayerSeat, int BossSeat) : IGameEvent;

// Native proof recorded while the cancelled frame and its exact paid movement still exist.
// This trusted material identity is intentionally absent from player projections.
public sealed record ShanheBossMaterialCancelledEvent(int CardId, long FrameId, long ActionId,
    int EntrySequence, CardActionType ActionType) : IGameEvent;
