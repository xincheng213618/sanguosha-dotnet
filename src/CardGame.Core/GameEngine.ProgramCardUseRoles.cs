namespace CardGame.Core;

public sealed record ProgramCardUseActorReplacedEvent(long FrameId, string SkillId, int OwnerSeat,
    long CardUseFrameId, int PreviousActorSeat, int ActorSeat, int ProviderSeat) : IGameEvent;
public sealed record ProgramCardUseTargetAddedEvent(long FrameId, string SkillId, int OwnerSeat,
    long CardUseFrameId, int TargetSeat) : IGameEvent;
