#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeMaterialSlashEntries
{
    internal void AddNativeMaterialSlashEquipmentActions(List<LegalAction> actions, CharacterState owner) => _resolve().AddNativeMaterialSlashEquipmentActions(actions, owner);
    internal void AssertNativeMaterialSlashExpansions() => _resolve().AssertNativeMaterialSlashExpansions();
    internal void BeginNativeMaterialSlashExpansion(CharacterState owner, IReadOnlyList<Card> cards, IReadOnlyList<int> seats) => _resolve().BeginNativeMaterialSlashExpansion(owner, cards, seats);
    internal void CompleteNativeMaterialSlash(long id, CharacterState owner, IReadOnlyList<Card> cards, CardKind kind, IReadOnlyList<int> seats, CardConversionSource? conversion, bool armor) => _resolve().CompleteNativeMaterialSlash(id, owner, cards, kind, seats, conversion, armor);
    internal bool MatchesNativeMaterialSlashExpansion(CardUseFrame use) => _resolve().MatchesNativeMaterialSlashExpansion(use);
    internal (IReadOnlyList<int> LegalTargets, int OrdinaryMaximum, IReadOnlyList<ProgramSlashTargetExpansion> Claims) NativeMaterialSlashExpansion(CharacterState owner, IReadOnlyList<Card> cards) => _resolve().NativeMaterialSlashExpansion(owner, cards);
    internal bool TryGetNativeMaterialSlashExpansion(CharacterState owner, IReadOnlyList<int> ids, IReadOnlyList<int> targets, scoped out int ordinaryMaximum, scoped out IReadOnlyList<ProgramSlashTargetExpansion> claims) => _resolve().TryGetNativeMaterialSlashExpansion(owner, ids, targets, out ordinaryMaximum, out claims);
}
