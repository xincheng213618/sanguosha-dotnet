namespace CardGame.Core;
public sealed partial class GameEngine
{
    // Derived opt-in only. The command journal reissues this event on recovery;
    // every actual entitlement below still proves the original immutable lease.
    private ref bool _hasIssuedAnniversaryZhangXun => ref _characterZhangXunAnniversary.ReferenceToHasIssuedAnniversaryZhangXun;

    internal sealed record ZhangXunDueLease(ZhangXunSuizhengLeaseEvent Lease, int Ordinal, int StartIndex, int DueTurn);
}
