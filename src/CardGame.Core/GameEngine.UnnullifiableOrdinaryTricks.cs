namespace CardGame.Core;

public sealed record OrdinaryTrickCannotNullifyReceipt(CardConversionSource Source, long CardUseFrameId,
    long ActionId, int ActorSeat, CardKind EffectiveKind);
public sealed record OrdinaryTrickCannotNullifyIssuedEvent(OrdinaryTrickCannotNullifyReceipt Receipt) : IGameEvent;
