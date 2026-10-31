#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeResponseMaterialQueriesEntries
{
    internal bool CanConvertResponse(CharacterState responder, Card card, CardKind requiredCardKind) => _resolve().CanConvertResponse(responder, card, requiredCardKind);
    internal bool CanUseCardAsResponse(CharacterState responder, Card card, CardKind requiredCardKind) => _resolve().CanUseCardAsResponse(responder, card, requiredCardKind);
    internal IReadOnlyList<Card>? FindZhangbaSlashPair(CharacterState responder, IReadOnlyList<int> requestedCardIds) => _resolve().FindZhangbaSlashPair(responder, requestedCardIds);
    internal CardKind GetEffectiveResponseKind(CharacterState responder, Card responseCard, CardKind requiredCardKind) => _resolve().GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
    internal IReadOnlyList<Card> GetResponseCards(CharacterState responder, CardKind requiredCardKind) => _resolve().GetResponseCards(responder, requiredCardKind);
    internal IReadOnlyList<IReadOnlyList<Card>> GetZhangbaSlashPairs(CharacterState responder) => _resolve().GetZhangbaSlashPairs(responder);
}
