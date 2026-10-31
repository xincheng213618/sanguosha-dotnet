#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineAlternativeViewAsCostsEntries
{
    internal CardMovementRecord? DestroyedAlternativeResponsePayment(CardActionContext action, long parentFrameId, CardActionCost cost, ProgramCardContinuation continuation) => _resolve().DestroyedAlternativeResponsePayment(action, parentFrameId, cost, continuation);
    internal void FinishSingleBasicCardUseCost(CardUseFrame use, Card card) => _resolve().FinishSingleBasicCardUseCost(use, card);
    internal void FinishSingleCardResponse(Card card, CardConversionSource? source) => _resolve().FinishSingleCardResponse(card, source);
    internal bool HasProgramUsedBasicCardThisTurn(int actorSeat, CardKind kind) => _resolve().HasProgramUsedBasicCardThisTurn(actorSeat, kind);
    internal bool IsProgramAlternativeCost(CardActionContext? action, int cardId) => _resolve().IsProgramAlternativeCost(action, cardId);
    internal bool IsProgramResponseCardUse(CharacterState responder, CardKind effectiveKind) => _resolve().IsProgramResponseCardUse(responder, effectiveKind);
    internal bool IsProgramTopDeckDodgeSource(CardConversionSource? source) => _resolve().IsProgramTopDeckDodgeSource(source);
    internal CardLocation NormalizeProgramViewAsCostDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason) => _resolve().NormalizeProgramViewAsCostDestination(card, from, to, reason);
    internal void PaySingleCardResponse(Card card, CharacterState provider, CardKind effectiveKind, CardConversionSource? source) => _resolve().PaySingleCardResponse(card, provider, effectiveKind, source);
    internal void RecordProgramUsedBasicCard(int actorSeat, CardKind effectiveKind) => _resolve().RecordProgramUsedBasicCard(actorSeat, effectiveKind);
}
