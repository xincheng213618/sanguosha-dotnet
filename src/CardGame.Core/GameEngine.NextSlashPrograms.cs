namespace CardGame.Core;
public sealed record ProgramRecoveryReceipt(long RecoveryFrameId, int InstructionIndex, int TargetSeat, int Recovered, bool Drawn = false);
public sealed record NamedSlashCancellation(string SkillId, int OwnerSeat, int TargetSeat);
public sealed record NextSlashDamageReservedEvent(int OwnerSeat, string SkillId, int TurnNumber, int TurnSeat, long CardUseFrameId, int TargetSeat, int Amount) : IGameEvent;
public sealed record NextSlashDamageConsumedEvent(int OwnerSeat, int TurnNumber, int TurnSeat, long CardUseFrameId, int Amount) : IGameEvent;
public sealed record NextSlashDamageExpiredEvent(int OwnerSeat, string SkillId, int TurnNumber, int TurnSeat, int Amount) : IGameEvent;
