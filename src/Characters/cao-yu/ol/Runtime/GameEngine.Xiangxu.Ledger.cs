namespace CardGame.Core;
public sealed partial class GameEngine
{
internal sealed record CaoYuDeathMinimumCapture(long DeathFrameId,long ParentFrameId,int VictimSeat,int? KillerSeat,DeathReturnKind ReturnKind,int ActualTurn,int TurnOwner,IReadOnlyList<ProgramTriggerCandidate> Sources,IReadOnlyList<int> HandCounts,IReadOnlyList<bool> Alive)
 {
  public IReadOnlyList<ProgramTriggerCandidate> Sources{get;init;}=Array.AsReadOnly(Sources.ToArray());
  public IReadOnlyList<int> HandCounts{get;init;}=Array.AsReadOnly(HandCounts.ToArray());
  public IReadOnlyList<bool> Alive{get;init;}=Array.AsReadOnly(Alive.ToArray());
 }
}
