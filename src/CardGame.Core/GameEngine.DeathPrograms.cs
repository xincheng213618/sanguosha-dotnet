namespace CardGame.Core;

public sealed record ProgramSkillCauseDeathDeclaredEvent(
    long FrameId,
    string SkillId,
    string BindingId,
    int SourceSeat,
    int TargetSeat) : IGameEvent;

public sealed record ProgramExtraTurnPendedEvent(
    long FrameId,
    string SkillId,
    int Seat) : IGameEvent;
