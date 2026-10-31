#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternativeViewAsCostsEntries
{
    internal global::CardGame.Core.CardMovementRecord? DestroyedAlternativeResponsePayment(global::CardGame.Core.CardActionContext action, long parentFrameId, global::CardGame.Core.CardActionCost cost, global::CardGame.Core.ProgramCardContinuation continuation) => _resolve().DestroyedAlternativeResponsePayment(action, parentFrameId, cost, continuation);
    internal void FinishSingleBasicCardUseCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card) => _resolve().FinishSingleBasicCardUseCost(use, card);
    internal void FinishSingleCardResponse(global::CardGame.Core.Card card, global::CardGame.Core.CardConversionSource? source) => _resolve().FinishSingleCardResponse(card, source);
    internal bool HasProgramUsedBasicCardThisTurn(int actorSeat, global::CardGame.Core.CardKind kind) => _resolve().HasProgramUsedBasicCardThisTurn(actorSeat, kind);
    internal bool IsProgramAlternativeCost(global::CardGame.Core.CardActionContext? action, int cardId) => _resolve().IsProgramAlternativeCost(action, cardId);
    internal bool IsProgramResponseCardUse(global::CardGame.Core.CharacterState responder, global::CardGame.Core.CardKind effectiveKind) => _resolve().IsProgramResponseCardUse(responder, effectiveKind);
    internal bool IsProgramTopDeckDodgeSource(global::CardGame.Core.CardConversionSource? source) => _resolve().IsProgramTopDeckDodgeSource(source);
    internal global::CardGame.Core.CardLocation NormalizeProgramViewAsCostDestination(global::CardGame.Core.Card card, global::CardGame.Core.CardLocation from, global::CardGame.Core.CardLocation to, global::CardGame.Core.CardMoveReason reason) => _resolve().NormalizeProgramViewAsCostDestination(card, from, to, reason);
    internal void PaySingleCardResponse(global::CardGame.Core.Card card, global::CardGame.Core.CharacterState provider, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardConversionSource? source) => _resolve().PaySingleCardResponse(card, provider, effectiveKind, source);
    internal void RecordProgramUsedBasicCard(int actorSeat, global::CardGame.Core.CardKind effectiveKind) => _resolve().RecordProgramUsedBasicCard(actorSeat, effectiveKind);
}
