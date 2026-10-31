namespace CardGame.Core;

public sealed record PaidFactionRequestCostRecovery(long RequestOwnerFrameId, int RequestOwnerSeat,
    FactionCardRequestPurpose Purpose, FactionResponsePolicySource PolicySource,
    int CostCardId, CardLocation CostLocation);

