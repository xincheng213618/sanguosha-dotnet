#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeCardAppearanceRuntime
{
    internal Suit EffectiveSuit(CharacterState owner, Card card) => _coreCardDeclarationsOperations.LiveDeclarationPayment(card.Id)?.FrozenSuit ?? _characterZhangFeiAnniversaryGodOperations.ShenZfIdentitySuit(owner, card) ?? _coreProgramCardPoliciesOperations.GetProgramEffectiveSuit(owner, card);
}
