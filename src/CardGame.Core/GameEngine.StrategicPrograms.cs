namespace CardGame.Core;
public sealed record ProgramSkillSuppressedEvent(int OwnerSeat, int TargetSeat, string SkillId, bool Suppressed) : IGameEvent;
