#nullable enable
namespace CardGame.Core.EngineStates;
internal sealed class CharacterZhangXunAnniversaryState : Contracts.ICharacterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess
{
    // Derived opt-in only. The command journal reissues this event on recovery;
    // every actual entitlement below still proves the original immutable lease.
    private bool _hasIssuedAnniversaryZhangXun = default !;
    internal ref bool ReferenceToHasIssuedAnniversaryZhangXun => ref _hasIssuedAnniversaryZhangXun;

    ref bool Contracts.ICharacterZhangXunAnniversaryHasIssuedAnniversaryZhangXunAccess._hasIssuedAnniversaryZhangXun => ref _hasIssuedAnniversaryZhangXun;
}
