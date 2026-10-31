#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiMobileGodEntries
{
    internal void ClearMobileSimaRenjieResponse(NativeOwners.CardAttackHandle attack) => _resolve().ClearMobileSimaRenjieResponse(attack);
    internal void MarkMobileSimaRenjieResponse(NativeOwners.CardAttackHandle attack, int responderSeat, CardKind requiredKind) => _resolve().MarkMobileSimaRenjieResponse(attack, responderSeat, requiredKind);
    internal bool MobileSimaRenjieStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().MobileSimaRenjieStructuralEdge(parent, child);
    internal bool TryBeginMobileSimaRenjieBorrowedSword(NativeOwners.BorrowedSwordHandle pending) => _resolve().TryBeginMobileSimaRenjieBorrowedSword(pending);
    internal bool TryBeginMobileSimaRenjieFailedAttack(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginMobileSimaRenjieFailedAttack(attack);
    internal bool TryBeginMobileSimaRenjieNullification(NullificationWindowFrame supplied, bool explicitPass) => _resolve().TryBeginMobileSimaRenjieNullification(supplied, explicitPass);
    internal bool TryBeginMobileSimaRenjieQizheng(CardUseFrame use) => _resolve().TryBeginMobileSimaRenjieQizheng(use);
}
