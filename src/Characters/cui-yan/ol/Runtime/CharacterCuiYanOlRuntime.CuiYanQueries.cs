#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiYanOlRuntime
{
    internal bool IsCuiYanNativeResponseForbidden(CharacterState owner, CardKind required, IReadOnlyList<Card> cards)
    {
        if (!HasCuiYanCapability || required != CardKind.Nullification && (required != CardKind.Dodge || !_coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(owner, required)))
            return false;
        return IsCuiYanNextUseForbidden(owner.Seat, cards.Select(c => c.Id).ToArray(), cards.Count == 1 ? _coreNativeCardAppearanceOperations.EffectiveSuit(owner, cards[0]) : null);
    }
}
