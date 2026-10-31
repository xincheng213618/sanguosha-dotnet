#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeGroupResponsesEntries
{
    internal void FinishZhangbaResponseCards(global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => _resolve().FinishZhangbaResponseCards(pair);
    internal void MoveZhangbaResponseCards(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair, long resolutionId, int responseTargetSeat) => _resolve().MoveZhangbaResponseCards(responder, pair, resolutionId, responseTargetSeat);
    internal void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedResponse) => _resolve().ResolveGroupResponse(group, responder, selectedResponse);
    internal void ResolveGroupZhangbaResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => _resolve().ResolveGroupZhangbaResponse(group, responder, pair);
}
