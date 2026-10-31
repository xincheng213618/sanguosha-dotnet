namespace CardGame.Core;
internal sealed partial class QuHuangRuntime
{
    private bool QiejianBlocked(int owner,int target)=>ProgramEventHistory<QiejianRoundBlockedEvent>().Any(e=>e.OwnerSeat==owner&&e.TargetSeat==target&&e.RoundNumber==_roundNumber);
}
