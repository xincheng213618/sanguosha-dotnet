namespace CardGame.Core;
public sealed record ProgramPhaseColorRestriction(string SkillId, string SkillInstanceId, int OwnerSeat, int TargetSeat, int TurnNumber, bool IsRed, bool UsedSlash = false);
public sealed record ProgramPhaseColorRestrictionGrantedEvent(ProgramPhaseColorRestriction Restriction) : IGameEvent;
