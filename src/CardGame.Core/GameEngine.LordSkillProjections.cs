namespace CardGame.Core;
public sealed record LordSkillProjectionChangedEvent(int OwnerSeat, string GrantId, string SkillId, LordSkillProjectionSource Source, bool Added) : IGameEvent;
