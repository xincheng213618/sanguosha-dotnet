namespace CardGame.Core;
public sealed record ProgramBooleanStateChangedEvent(int OwnerSeat, string SkillId, string SkillInstanceId, string StateId, bool Value, SkillProgramStateVisibility Visibility) : IGameEvent;
