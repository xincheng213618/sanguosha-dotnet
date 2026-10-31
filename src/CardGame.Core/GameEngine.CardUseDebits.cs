namespace CardGame.Core;
public sealed record CardUseDebitRecordedEvent(CardUseDebitIdentity Debit) : IGameEvent;
public sealed record CardUseDebitRefundedEvent(CardUseDebitIdentity Debit) : IGameEvent;
