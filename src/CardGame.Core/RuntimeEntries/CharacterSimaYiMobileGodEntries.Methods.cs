#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaYiMobileGodEntries
{
    internal void ClearMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().ClearMobileSimaRenjieResponse(attack);
    internal bool HasMobileSimaRenjieNativeSubtree(long useId) => _resolve().HasMobileSimaRenjieNativeSubtree(useId);
    internal void MarkMobileSimaRenjieResponse(global::CardGame.Core.NativeOwners.CardAttackHandle attack, int responderSeat, global::CardGame.Core.CardKind requiredKind) => _resolve().MarkMobileSimaRenjieResponse(attack, responderSeat, requiredKind);
    internal bool MobileSimaRenjieStructuralEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().MobileSimaRenjieStructuralEdge(parent, child);
    internal bool TryBeginMobileSimaRenjieBorrowedSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => _resolve().TryBeginMobileSimaRenjieBorrowedSword(pending);
    internal bool TryBeginMobileSimaRenjieFailedAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginMobileSimaRenjieFailedAttack(attack);
    internal bool TryBeginMobileSimaRenjieNullification(global::CardGame.Core.NullificationWindowFrame supplied, bool explicitPass) => _resolve().TryBeginMobileSimaRenjieNullification(supplied, explicitPass);
    internal bool TryBeginMobileSimaRenjieQizheng(global::CardGame.Core.CardUseFrame use) => _resolve().TryBeginMobileSimaRenjieQizheng(use);
}
