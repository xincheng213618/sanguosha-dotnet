namespace CardGame.Core;
public sealed record CardUseDebitRecordedEvent(CardUseDebitIdentity Debit) : IGameEvent;
public sealed record CardUseDebitRefundedEvent(CardUseDebitIdentity Debit) : IGameEvent;
public sealed partial class GameEngine
{
    private ref int _cardUseDebitPhaseInstanceId => ref _cardUseDebits2.ReferenceToCardUseDebitPhaseInstanceId;
    private ref readonly global::System.Collections.Generic.Dictionary<long, global::CardGame.Core.CardUseDebitIdentity> _cardUseDebits => ref _cardUseDebits2.ReferenceToCardUseDebits;
    private ref readonly global::System.Collections.Generic.HashSet<long> _refundedCardUseDebits => ref _cardUseDebits2.ReferenceToRefundedCardUseDebits;
}
