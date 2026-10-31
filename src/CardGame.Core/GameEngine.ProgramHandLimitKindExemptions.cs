namespace CardGame.Core;

public sealed record ShanheMarketDiscardQuote(long DiscardFrameId, int OwnerSeat, int ActualTurnNumber,
    int QuotaMultiplier, int PaidCount);
