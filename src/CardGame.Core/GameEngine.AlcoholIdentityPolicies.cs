namespace CardGame.Core;

public sealed record ProgramDyingAlcoholPermission(long DyingFrameId, int ActorSeat, int ResponderSeat,
    string SkillId, string BindingId, string SkillInstanceId, string GameplayHash, SkillProgramTriggerWindow Window);
