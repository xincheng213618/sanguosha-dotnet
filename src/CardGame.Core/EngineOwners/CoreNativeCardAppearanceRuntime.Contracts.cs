#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreNativeCardAppearanceCoreCardDeclarationsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.DeclaredCardPayment? LiveDeclarationPayment(int cardId);
}

internal interface ICoreNativeCardAppearanceCoreProgramCardPoliciesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit GetProgramEffectiveSuit(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card, global::CardGame.Core.CardLocation? identityLocation = default);
}

internal interface ICoreNativeCardAppearanceCharacterZhangFeiAnniversaryGodOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.Suit? ShenZfIdentitySuit(global::CardGame.Core.CharacterState p, global::CardGame.Core.Card c, global::CardGame.Core.CardLocation? identityLocation = default);
}
