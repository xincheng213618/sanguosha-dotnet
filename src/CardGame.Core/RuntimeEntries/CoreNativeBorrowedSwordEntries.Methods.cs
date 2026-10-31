#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeBorrowedSwordEntries
{
    internal void BeginBorrowedSwordSlashChoice(NativeOwners.BorrowedSwordHandle pending, bool includeFactionSlash, bool keepCurrentRequest = false) => _resolve().BeginBorrowedSwordSlashChoice(pending, includeFactionSlash, keepCurrentRequest);
    internal bool CanRequestBorrowedSwordFactionSlash(NativeOwners.BorrowedSwordHandle pending) => _resolve().CanRequestBorrowedSwordFactionSlash(pending);
    internal void CompleteBorrowedSwordAfterSlash(NativeOwners.BorrowedSwordHandle pending) => _resolve().CompleteBorrowedSwordAfterSlash(pending);
    internal void CompleteBorrowedSwordWithoutSlash(NativeOwners.BorrowedSwordHandle pending, bool transferWeapon) => _resolve().CompleteBorrowedSwordWithoutSlash(pending, transferWeapon);
    internal void FinishBorrowedSword(NativeOwners.BorrowedSwordHandle pending) => _resolve().FinishBorrowedSword(pending);
    internal IReadOnlyList<Card> GetBorrowedSwordSlashCards(CharacterState weaponOwner, CharacterState slashTarget) => _resolve().GetBorrowedSwordSlashCards(weaponOwner, slashTarget);
    internal Card? GetWeapon(CharacterState player) => _resolve().GetWeapon(player);
    internal bool IsLegalBorrowedSwordSlashTarget(CharacterState weaponOwner, CharacterState target, CardKind? effectiveKind = default, Suit? physicalSuit = default, bool allowAnyPhysicalSuit = true, bool? effectiveColor = default, int? effectiveRank = default, IReadOnlyList<int>? physicalCardIds = default, CardConversionSource? conversionSource = default) => _resolve().IsLegalBorrowedSwordSlashTarget(weaponOwner, target, effectiveKind, physicalSuit, allowAnyPhysicalSuit, effectiveColor, effectiveRank, physicalCardIds, conversionSource);
    internal void ResolveBorrowedSwordProgramMultiCardSlashChoice(NativeOwners.BorrowedSwordHandle pending, ProgramMultiCardViewAsSelection selection) => _resolve().ResolveBorrowedSwordProgramMultiCardSlashChoice(pending, selection);
    internal void ResolveBorrowedSwordSlashChoice(NativeOwners.BorrowedSwordHandle pending, Card slash, CardKind? requestedEffectiveKind = default) => _resolve().ResolveBorrowedSwordSlashChoice(pending, slash, requestedEffectiveKind);
    internal void ResolveBorrowedSwordZhangbaSlashChoice(NativeOwners.BorrowedSwordHandle pending, IReadOnlyList<Card> pair) => _resolve().ResolveBorrowedSwordZhangbaSlashChoice(pending, pair);
}
