#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardAppearanceRuntime
{
    internal Suit EffectiveSuit(CharacterState owner, Card card, CardLocation? identityLocation = null) =>
        _coreCardDeclarationsOperations.LiveDeclarationPayment(card.Id)?.FrozenSuit ??
        _characterZhangFeiAnniversaryGodOperations.ShenZfIdentitySuit(owner, card, identityLocation) ??
        _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card, identityLocation);
}
