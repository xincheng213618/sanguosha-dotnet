namespace CardGame.Core;
public sealed partial class GameEngine
{

}
public sealed record ProgramHandComparisonSourceRevealedEvent(long FrameId,int OwnerSeat,int CardId):IGameEvent;
public sealed record ProgramHandComparisonCardRevealedEvent(long FrameId,int TargetSeat,int CardId):IGameEvent;
