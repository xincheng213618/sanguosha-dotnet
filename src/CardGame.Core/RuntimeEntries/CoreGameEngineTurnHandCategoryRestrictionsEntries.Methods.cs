#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnHandCategoryRestrictionsEntries
{
    internal bool CanSelectSelfDiscardBinding(global::CardGame.Core.ProgramSkillFrame f, string bind, int selectionActorSeat, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation location) => _resolve().CanSelectSelfDiscardBinding(f, bind, selectionActorSeat, card, location);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableHand(global::CardGame.Core.CharacterState owner) => _resolve().GetSelfDiscardableHand(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetSelfDiscardableOwnedHand(global::CardGame.Core.CharacterState owner) => _resolve().GetSelfDiscardableOwnedHand(owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.TurnHandCategoryRestriction>? GetTurnHandCategoryRestrictionSnapshot(int seat) => _resolve().GetTurnHandCategoryRestrictionSnapshot(seat);
    internal void GrantDamageSourceHandCategory(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramCardCategory category) => _resolve().GrantDamageSourceHandCategory(f, category);
    internal bool IsHandCategoryMaterialRestricted(global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().IsHandCategoryMaterialRestricted(ids);
    internal bool IsSelfHandCategoryDiscardForbidden(int actor, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.OwnedCardMoveIntent intent) => _resolve().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
    internal bool IsTurnHandCategoryRestricted(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().IsTurnHandCategoryRestricted(owner, card);
    internal bool ProgramSelectionWillSelfDiscard(global::CardGame.Core.ProgramSkillFrame f, string bind) => _resolve().ProgramSelectionWillSelfDiscard(f, bind);
    internal int RequiredSelfHandLimitDiscard(global::CardGame.Core.CharacterState owner) => _resolve().RequiredSelfHandLimitDiscard(owner);
}
