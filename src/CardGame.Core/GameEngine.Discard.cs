namespace CardGame.Core;

public sealed record ManualDiscardFrame(long Id, int OwnerSeat, int ActualTurnNumber,
    int InitialRequiredCount, int RemainingCount)
    : ResolutionFrame(Id, ResolutionFrameKind.ManualDiscard, ResolutionFrameStep.ResolvingEffect)
{
    public int QuotaMultiplier { get; init; } = 1;
    public int PaidCount { get; init; }
}
