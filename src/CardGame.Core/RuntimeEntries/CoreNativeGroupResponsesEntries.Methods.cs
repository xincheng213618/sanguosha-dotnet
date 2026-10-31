#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupResponsesEntries
{
    internal void FinishZhangbaResponseCards(IReadOnlyList<Card> pair) => _resolve().FinishZhangbaResponseCards(pair);
    internal void MoveZhangbaResponseCards(CharacterState responder, IReadOnlyList<Card> pair, long resolutionId, int responseTargetSeat) => _resolve().MoveZhangbaResponseCards(responder, pair, resolutionId, responseTargetSeat);
    internal void ResolveGroupResponse(NativeOwners.GroupCardHandle group, CharacterState responder, Card? selectedResponse) => _resolve().ResolveGroupResponse(group, responder, selectedResponse);
    internal void ResolveGroupZhangbaResponse(NativeOwners.GroupCardHandle group, CharacterState responder, IReadOnlyList<Card> pair) => _resolve().ResolveGroupZhangbaResponse(group, responder, pair);
}
