namespace CardGame.Core;
public sealed record SlashTargetsReplacedEvent(long FrameId,string SkillId,int OwnerSeat,long CardUseFrameId,IReadOnlyList<int> PreviousTargets,IReadOnlyList<int> Targets) : IGameEvent;
public sealed partial class GameEngine
{
}
