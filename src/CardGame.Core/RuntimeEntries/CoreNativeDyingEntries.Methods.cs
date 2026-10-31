#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDyingEntries
{
    internal void ApplyDyingResponse(CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, CardConversionSource? peachConversionSource = default, CardConversionSource? alcoholConversionSource = default) => _resolve().ApplyDyingResponse(responder, usePeach, peachCardId, useAlcohol, alcoholCardId, peachConversionSource, alcoholConversionSource);
    internal void BeginDying(GameEngine.IDamageAttempt attack, long damageFrameId, CharacterState victim, CharacterState killer) => _resolve().BeginDying(attack, damageFrameId, victim, killer);
    internal IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat) => _resolve().BuildDyingResponderSeats(victimSeat);
    internal void CompleteDamageAfterDying(GameEngine.DyingCompletionReceipt dying) => _resolve().CompleteDamageAfterDying(dying);
    internal void CompleteDying(DyingFrame dying, bool survived) => _resolve().CompleteDying(dying, survived);
    internal void CompleteDyingAfterDeath(DyingFrame dying, bool survived) => _resolve().CompleteDyingAfterDeath(dying, survived);
    internal void CompleteDyingCardResponse(DyingResponseEvent response) => _resolve().CompleteDyingCardResponse(response);
    internal void ContinueDyingAfterFactionRecoveryDebts(GameEngine.DyingCompletionReceipt dying, bool survived) => _resolve().ContinueDyingAfterFactionRecoveryDebts(dying, survived);
    internal void ExposeHumanDyingPrompt() => _resolve().ExposeHumanDyingPrompt();
    internal Card[] GetDyingAlcohols(CharacterState responder, int victimSeat) => _resolve().GetDyingAlcohols(responder, victimSeat);
    internal IReadOnlyList<CardConversionSource> GetDyingPeachConversionSources(CharacterState responder, Card card) => _resolve().GetDyingPeachConversionSources(responder, card);
    internal Card[] GetDyingPeaches(CharacterState responder) => _resolve().GetDyingPeaches(responder);
    internal void RequestHumanDyingResponse(CharacterState responder, IReadOnlyList<Card> peaches, IReadOnlyList<Card> alcohols) => _resolve().RequestHumanDyingResponse(responder, peaches, alcohols);
    internal void RunOneDyingStep(bool skipRequestedDeckBasic = false) => _resolve().RunOneDyingStep(skipRequestedDeckBasic);
}
