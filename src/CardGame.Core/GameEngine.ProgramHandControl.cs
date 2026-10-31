namespace CardGame.Core;

public sealed partial class GameEngine
{

}

public sealed record ProgramOwnerSkillsReplacedEvent(long ResolutionId, string SkillId,
    int OwnerSeat, IReadOnlyList<string> LostSkillIds, string GrantedSkillId) : IGameEvent;
