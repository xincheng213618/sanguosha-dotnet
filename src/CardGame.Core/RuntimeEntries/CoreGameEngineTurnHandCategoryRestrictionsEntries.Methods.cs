#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTurnHandCategoryRestrictionsEntries
{
    internal bool CanSelectSelfDiscardBinding(ProgramSkillFrame f, string bind, int selectionActorSeat, Card card, CardLocation location) => _resolve().CanSelectSelfDiscardBinding(f, bind, selectionActorSeat, card, location);
    internal IReadOnlyList<Card> GetSelfDiscardableHand(CharacterState owner) => _resolve().GetSelfDiscardableHand(owner);
    internal IReadOnlyList<Card> GetSelfDiscardableOwnedHand(CharacterState owner) => _resolve().GetSelfDiscardableOwnedHand(owner);
    internal IReadOnlyList<TurnHandCategoryRestriction>? GetTurnHandCategoryRestrictionSnapshot(int seat) => _resolve().GetTurnHandCategoryRestrictionSnapshot(seat);
    internal void GrantDamageSourceHandCategory(ProgramSkillFrame f, SkillProgramCardCategory category) => _resolve().GrantDamageSourceHandCategory(f, category);
    internal bool IsHandCategoryMaterialRestricted(IReadOnlyList<int> ids) => _resolve().IsHandCategoryMaterialRestricted(ids);
    internal bool IsSelfHandCategoryDiscardForbidden(int actor, Card card, CardLocation from, OwnedCardMoveIntent intent) => _resolve().IsSelfHandCategoryDiscardForbidden(actor, card, from, intent);
    internal bool IsTurnHandCategoryRestricted(CharacterState owner, Card card) => _resolve().IsTurnHandCategoryRestricted(owner, card);
    internal bool ProgramSelectionWillSelfDiscard(ProgramSkillFrame f, string bind) => _resolve().ProgramSelectionWillSelfDiscard(f, bind);
    internal int RequiredSelfHandLimitDiscard(CharacterState owner) => _resolve().RequiredSelfHandLimitDiscard(owner);
}
