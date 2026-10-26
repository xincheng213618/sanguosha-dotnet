namespace CardGame.Core;

public sealed record ShanheMarketDiscardQuote(long DiscardFrameId, int OwnerSeat, int ActualTurnNumber,
    int QuotaMultiplier, int PaidCount);

public sealed partial class GameEngine
{
    private IReadOnlySet<CardKind> GetCurrentHandLimitExemptKinds(CharacterState owner) =>
        _turnCardUseEffects.GetHandLimitExemptCardKinds(_turnNumber, _turnProgression.OwnerSeat, owner.Seat)
            .Concat(GetSkillBindingShard(owner).GetCardPolicies(SkillProgramCardPolicyKind.HandLimitCardKindExemption)
                .SelectMany(binding => binding.Policy.CardKinds).Distinct()
                .Where(kind => HasCardPolicy(owner, SkillProgramCardPolicyKind.HandLimitCardKindExemption, kind)))
            .ToHashSet();

    private ShanheMarketDiscardQuote? CaptureShanheMarketDiscardQuote()
    {
        if (_pendingDecision is not { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 } ||
            _resolutionStack.LastOrDefault() is not ManualDiscardFrame frame) return null;
        if (frame.OwnerSeat != 0 || frame.ActualTurnNumber != _turnNumber || frame.QuotaMultiplier <= 0)
            throw new InvalidOperationException("A market discard quote lost its original actual-turn owner.");
        return new(frame.Id, frame.OwnerSeat, frame.ActualTurnNumber, frame.QuotaMultiplier, frame.PaidCount);
    }

    private bool RefreshShanheMarketDiscardPrompt(ShanheMarketDiscardQuote? quote)
    {
        if (quote is null) return false;
        if (_pendingDecision is not { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 } ||
            _resolutionStack.LastOrDefault() is not ManualDiscardFrame frame ||
            frame.Id != quote.DiscardFrameId || frame.OwnerSeat != quote.OwnerSeat || frame.OwnerSeat != 0 ||
            frame.ActualTurnNumber != quote.ActualTurnNumber || frame.ActualTurnNumber != _turnNumber ||
            frame.QuotaMultiplier != quote.QuotaMultiplier || quote.QuotaMultiplier <= 0 ||
            frame.PaidCount != quote.PaidCount || quote.PaidCount < 0)
            throw new InvalidOperationException("A market acquisition lost its original unpaid discard quote.");
        var owner = _players[frame.OwnerSeat];
        // Rebuild the obligation before its real physical payments, retaining
        // the original multiplier and subtracting those payments exactly once.
        // Purchases can forgive an obligation but cannot charge a larger one.
        var paid = (long)frame.PaidCount;
        var basisCount = checked(GetDiscardEligibleHand(owner).Count + paid);
        var totalOwed = Math.Min(checked(Math.Max(0L, basisCount - GetHandLimit(owner)) * frame.QuotaMultiplier),
            checked(GetSelfDiscardableHand(owner).Count + paid));
        var remaining = (int)Math.Max(0L, Math.Min(frame.RemainingCount, totalOwed - paid));
        ReplaceRuntimeTop(frame with { RemainingCount = remaining });
        ClearPendingDecision();
        _status = EngineStatus.Running;
        if (remaining > 0) RequestHumanDiscard(owner);
        else ContinueManualDiscard(frame.Id);
        return true;
    }
}
