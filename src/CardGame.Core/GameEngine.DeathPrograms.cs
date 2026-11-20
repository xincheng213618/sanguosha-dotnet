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

internal interface IProgramExtraTurnPreludeGrantedEvent : IGameEvent
{
    long FrameId { get; }
    CardConversionSource Source { get; }
    string GameplayHash { get; }
    int IssuedTurn { get; }
    int RecipientSeat { get; }
}

internal interface IProgramExtraTurnPreludeEnteredEvent : IGameEvent
{
    long GrantFrameId { get; }
}
