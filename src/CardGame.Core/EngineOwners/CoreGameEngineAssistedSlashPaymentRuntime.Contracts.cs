#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal interface ICoreAssistedSlashPaymentCorePhysicalOwnedCardQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetPlayableCards(global::CardGame.Core.CharacterState player);
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetEquipment(global::CardGame.Core.CharacterState player);
}

internal interface ICoreAssistedSlashPaymentCoreNativeResponseMaterialQueriesOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder);
}

internal interface ICoreAssistedSlashPaymentCharacterKongShuOlOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    bool IsLeiluanSource(global::CardGame.Core.CardConversionSource? source);
}

internal interface ICoreAssistedSlashPaymentCoreNativePublicPileOriginalCardsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> NativePublicPileSlashUseCards(global::CardGame.Core.CharacterState provider);
}

internal interface ICoreAssistedSlashPaymentCoreOwnPublicPileConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetOwnPublicPileConversionCards(global::CardGame.Core.CharacterState owner);
}

internal interface ICoreAssistedSlashPaymentCoreCardConversionsOperations : global::CardGame.Core.IProgramRuntimeOperations
{
    global::CardGame.Core.ProgramMultiCardViewAsSelection? FindProgramMultiCardViewAsSelection(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<int> cardIds, global::CardGame.Core.CardKind outputKind, bool forResponse, global::CardGame.Core.CardConversionSource source);
    global::CardGame.Core.CardConversionSource? GetSelectedUseConversion(global::CardGame.Core.CharacterState actor, global::CardGame.Core.CharacterState provider, global::CardGame.Core.Card card, global::CardGame.Core.CardKind effectiveKind);
}
