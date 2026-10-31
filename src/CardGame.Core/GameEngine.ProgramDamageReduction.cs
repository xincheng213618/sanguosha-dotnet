namespace CardGame.Core;

public sealed record ProgramDamageReducedEvent(long FrameId, string SkillId, int OwnerSeat,
    int SourceSeat, int TargetSeat, int AmountBefore, int AmountAfter) : IGameEvent;
