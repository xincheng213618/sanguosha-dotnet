#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeResponseMaterialQueriesEntries
{
    internal bool CanConvertResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind requiredCardKind) => _resolve().CanConvertResponse(responder, card, requiredCardKind);
    internal bool CanUseCardAsResponse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card, global::CardGame.Core.CardKind requiredCardKind) => _resolve().CanUseCardAsResponse(responder, card, requiredCardKind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>? FindZhangbaSlashPair(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<int> requestedCardIds) => _resolve().FindZhangbaSlashPair(responder, requestedCardIds);
    internal global::CardGame.Core.CardKind GetEffectiveResponseKind(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card responseCard, global::CardGame.Core.CardKind requiredCardKind) => _resolve().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> GetResponseCards(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind requiredCardKind) => _resolve().GetResponseCards(responder, requiredCardKind);
    internal global::System.Collections.Generic.IReadOnlyList<global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card>> GetZhangbaSlashPairs(global::CardGame.Core.CharacterState responder) => _resolve().GetZhangbaSlashPairs(responder);
}
