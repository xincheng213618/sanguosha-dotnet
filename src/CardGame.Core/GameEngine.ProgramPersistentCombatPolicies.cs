namespace CardGame.Core;
public sealed record ProgramDamageHandLimitChangedEvent(int OwnerSeat, string SkillId, int TurnNumber, int PhaseInstanceId, int DamageCount) : IGameEvent;
public sealed record ProgramGameFactionAttackRangeTargetsGrantedEvent(int OwnerSeat, string SkillId, string FactionId, IReadOnlyList<int> TargetSeats) : IGameEvent;
