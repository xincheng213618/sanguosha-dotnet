#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDuelEntries
{
    internal void BeginDuelResponse(NativeOwners.DuelHandle duel, bool keepCurrentRequest = false) => _resolve().BeginDuelResponse(duel, keepCurrentRequest);
    internal void ContinueDuelAfterSuccessfulSlash(NativeOwners.DuelHandle duel, int responderSeat) => _resolve().ContinueDuelAfterSuccessfulSlash(duel, responderSeat);
    internal void ResolveDuel(CharacterState source, CharacterState target, Card duel, CardKind? playedCardKind = default) => _resolve().ResolveDuel(source, target, duel, playedCardKind);
    internal void ResolveDuelResponse(NativeOwners.DuelHandle duel, CharacterState responder, Card? selectedSlash) => _resolve().ResolveDuelResponse(duel, responder, selectedSlash);
    internal void ResolveDuelZhangbaResponse(NativeOwners.DuelHandle duel, CharacterState responder, IReadOnlyList<Card> pair) => _resolve().ResolveDuelZhangbaResponse(duel, responder, pair);
}
