namespace CardGame.Core;

public sealed record ProgramOwnerSkillsReplacedEvent(long ResolutionId, string SkillId,
    int OwnerSeat, IReadOnlyList<string> LostSkillIds, string GrantedSkillId) : IGameEvent;
