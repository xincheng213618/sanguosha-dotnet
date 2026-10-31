namespace CardGame.Core;

public sealed record ProgramQuotaLossIssuedEvent(long FrameId, int OwnerSeat, string SkillId, string SkillInstanceId, int OtherSeat) : IGameEvent;
