#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramHandLimitKindExemptionsRuntime
{
    internal IReadOnlySet<CardKind> GetCurrentHandLimitExemptKinds(CharacterState owner) => _turnCardUseEffectsOperations._turnCardUseEffects.GetHandLimitExemptCardKinds(_coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber, _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat, owner.Seat).Concat(_coreNationalSkillsOperations.GetSkillBindingShard(owner).GetCardPolicies(SkillProgramCardPolicyKind.HandLimitCardKindExemption).SelectMany(binding => binding.Policy.CardKinds).Distinct().Where(kind => _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.HandLimitCardKindExemption, kind))).ToHashSet();
    internal ShanheMarketDiscardQuote? CaptureShanheMarketDiscardQuote()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 } || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ManualDiscardFrame frame)
            return null;
        return new(frame.Id, frame.OwnerSeat, frame.ActualTurnNumber, frame.QuotaMultiplier, frame.PaidCount);
    }

    internal bool RefreshShanheMarketDiscardPrompt(ShanheMarketDiscardQuote? quote)
    {
        if (quote is null)
            return false;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.DiscardCards, PlayerSeat: 0 } || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ManualDiscardFrame frame || frame.Id != quote.DiscardFrameId || frame.OwnerSeat != quote.OwnerSeat || frame.OwnerSeat != 0 || frame.ActualTurnNumber != quote.ActualTurnNumber || frame.ActualTurnNumber != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || frame.QuotaMultiplier != quote.QuotaMultiplier || quote.QuotaMultiplier <= 0 || frame.PaidCount != quote.PaidCount || quote.PaidCount < 0)
            throw new InvalidOperationException("A market acquisition lost its original unpaid discard quote.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        // Rebuild the obligation before its real physical payments, retaining
        // the original multiplier and subtracting those payments exactly once.
        // Purchases can forgive an obligation but cannot charge a larger one.
        var paid = (long)frame.PaidCount;
        var basisCount = checked(_coreDeferredCardsOperations.GetDiscardEligibleHand(owner).Count + paid);
        var totalOwed = Math.Min(checked(Math.Max(0L, basisCount - _coreNativeRuleQueriesOperations.GetHandLimit(owner)) * frame.QuotaMultiplier), checked(_coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableHand(owner).Count + paid));
        var remaining = (int)Math.Max(0L, Math.Min(frame.RemainingCount, totalOwed - paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RemainingCount = remaining });
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        if (remaining > 0)
            _coreDiscardOperations.RequestHumanDiscard(owner);
        else
            _coreDiscardOperations.ContinueManualDiscard(frame.Id);
        return true;
    }
}
