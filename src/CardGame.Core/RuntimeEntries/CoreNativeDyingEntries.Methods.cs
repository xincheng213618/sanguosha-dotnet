#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeDyingEntries
{
    internal void ApplyDyingResponse(global::CardGame.Core.CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, global::CardGame.Core.CardConversionSource? peachConversionSource = default, global::CardGame.Core.CardConversionSource? alcoholConversionSource = default) => _resolve().ApplyDyingResponse(responder, usePeach, peachCardId, useAlcohol, alcoholCardId, peachConversionSource, alcoholConversionSource);
    internal void BeginDying(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, global::CardGame.Core.CharacterState victim, global::CardGame.Core.CharacterState killer) => _resolve().BeginDying(attack, damageFrameId, victim, killer);
    internal global::System.Collections.Generic.IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat) => _resolve().BuildDyingResponderSeats(victimSeat);
    internal void CompleteDamageAfterDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying) => _resolve().CompleteDamageAfterDying(dying);
    internal void CompleteDying(global::CardGame.Core.DyingFrame dying, bool survived) => _resolve().CompleteDying(dying, survived);
    internal void CompleteDyingAfterDeath(global::CardGame.Core.DyingFrame dying, bool survived) => _resolve().CompleteDyingAfterDeath(dying, survived);
    internal void CompleteDyingCardResponse(global::CardGame.Core.DyingResponseEvent response) => _resolve().CompleteDyingCardResponse(response);
    internal void ContinueDyingAfterFactionRecoveryDebts(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived) => _resolve().ContinueDyingAfterFactionRecoveryDebts(dying, survived);
    internal void ExposeHumanDyingPrompt() => _resolve().ExposeHumanDyingPrompt();
    internal global::CardGame.Core.Card[] GetDyingAlcohols(global::CardGame.Core.CharacterState responder, int victimSeat) => _resolve().GetDyingAlcohols(responder, victimSeat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.CardConversionSource> GetDyingPeachConversionSources(global::CardGame.Core.CharacterState responder, global::CardGame.Core.Card card) => _resolve().GetDyingPeachConversionSources(responder, card);
    internal global::CardGame.Core.Card[] GetDyingPeaches(global::CardGame.Core.CharacterState responder) => _resolve().GetDyingPeaches(responder);
    internal void RequestHumanDyingResponse(global::CardGame.Core.CharacterState responder, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> peaches, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.Card> alcohols) => _resolve().RequestHumanDyingResponse(responder, peaches, alcohols);
    internal void RunOneDyingStep(bool skipRequestedDeckBasic = false) => _resolve().RunOneDyingStep(skipRequestedDeckBasic);
}
