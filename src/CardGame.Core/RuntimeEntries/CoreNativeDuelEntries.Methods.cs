#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDuelEntries
{
    internal void BeginDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, bool keepCurrentRequest = false) => _resolve().BeginDuelResponse(duel, keepCurrentRequest);
    internal void ContinueDuelAfterSuccessfulSlash(global::CardGame.Core.NativeOwners.DuelHandle duel, int responderSeat) => _resolve().ContinueDuelAfterSuccessfulSlash(duel, responderSeat);
    internal void ResolveDuel(global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.Card duel, global::CardGame.Core.CardKind? playedCardKind = default) => _resolve().ResolveDuel(source, target, duel, playedCardKind);
    internal void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card? selectedSlash) => _resolve().ResolveDuelResponse(duel, responder, selectedSlash);
    internal void ResolveDuelZhangbaResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> pair) => _resolve().ResolveDuelZhangbaResponse(duel, responder, pair);
}
