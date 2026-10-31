#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeBorrowedSwordEntries
{
    internal void BeginBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool includeFactionSlash, bool keepCurrentRequest = false) => _resolve().BeginBorrowedSwordSlashChoice(pending, includeFactionSlash, keepCurrentRequest);
    internal bool CanRequestBorrowedSwordFactionSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => _resolve().CanRequestBorrowedSwordFactionSlash(pending);
    internal void CompleteBorrowedSwordAfterSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => _resolve().CompleteBorrowedSwordAfterSlash(pending);
    internal void CompleteBorrowedSwordWithoutSlash(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, bool transferWeapon) => _resolve().CompleteBorrowedSwordWithoutSlash(pending, transferWeapon);
    internal void FinishBorrowedSword(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending) => _resolve().FinishBorrowedSword(pending);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetBorrowedSwordSlashCards(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState slashTarget) => _resolve().GetBorrowedSwordSlashCards(weaponOwner, slashTarget);
    internal global::CardGame.Core.Card? GetWeapon(global::CardGame.Core.CharacterState player) => _resolve().GetWeapon(player);
    internal bool IsLegalBorrowedSwordSlashTarget(global::CardGame.Core.CharacterState weaponOwner, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind? effectiveKind = default, global::CardGame.Core.Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, global::System.Collections.Generic.IReadOnlyList<int>? physicalCardIds = default, global::CardGame.Core.CardConversionSource? conversionSource = default) => _resolve().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
    internal void ResolveBorrowedSwordProgramMultiCardSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.ProgramMultiCardViewAsSelection selection) => _resolve().ResolveBorrowedSwordProgramMultiCardSlashChoice(pending, selection);
    internal void ResolveBorrowedSwordSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::CardGame.Core.Card slash, global::CardGame.Core.CardKind? requestedEffectiveKind = default) => _resolve().ResolveBorrowedSwordSlashChoice(pending, slash, requestedEffectiveKind);
    internal void ResolveBorrowedSwordZhangbaSlashChoice(global::CardGame.Core.NativeOwners.BorrowedSwordHandle pending, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => _resolve().ResolveBorrowedSwordZhangbaSlashChoice(pending, pair);
}
