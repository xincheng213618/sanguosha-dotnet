#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeMaterialSlashEntries
{
    internal void AddNativeMaterialSlashEquipmentActions(global::System.Collections.Generic.List<global::CardGame.Core.LegalAction> actions, global::CardGame.Core.CharacterState owner) => _resolve().AddNativeMaterialSlashEquipmentActions(actions, owner);
    internal void AssertNativeMaterialSlashExpansions() => _resolve().AssertNativeMaterialSlashExpansions();
    internal void BeginNativeMaterialSlashExpansion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::System.Collections.Generic.IReadOnlyList<int> seats) => _resolve().BeginNativeMaterialSlashExpansion(owner, cards, seats);
    internal void CompleteNativeMaterialSlash(long id, global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards, global::CardGame.Core.CardKind kind, global::System.Collections.Generic.IReadOnlyList<int> seats, global::CardGame.Core.CardConversionSource? conversion, bool armor) => _resolve().CompleteNativeMaterialSlash(id, owner, cards, kind, seats, conversion, armor);
    internal bool IsExactNativeMaterialSlashExpansion(global::CardGame.Core.NativeOwners.CardAttackHandle attack, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> processing) => _resolve().IsExactNativeMaterialSlashExpansion(attack, processing);
    internal bool MatchesNativeMaterialSlashExpansion(global::CardGame.Core.CardUseFrame use) => _resolve().MatchesNativeMaterialSlashExpansion(use);
    internal (global::System.Collections.Generic.IReadOnlyList<int> LegalTargets, int OrdinaryMaximum, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramSlashTargetExpansion> Claims) NativeMaterialSlashExpansion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> cards) => _resolve().NativeMaterialSlashExpansion(owner, cards);
    internal bool TryGetNativeMaterialSlashExpansion(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> ids, global::System.Collections.Generic.IReadOnlyList<int> targets, scoped out int ordinaryMaximum, scoped out global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramSlashTargetExpansion> claims) => _resolve().TryGetNativeMaterialSlashExpansion(owner, ids, targets, out ordinaryMaximum, out claims);
}
