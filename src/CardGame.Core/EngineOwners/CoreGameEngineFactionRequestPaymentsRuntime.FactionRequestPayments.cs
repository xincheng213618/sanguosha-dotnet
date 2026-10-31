#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFactionRequestPaymentsRuntime
{
    internal IReadOnlyList<PromptChoice> FactionRequestCostChoices(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        return _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Select(card => new PromptChoice(new ChoiceId("faction-request-cost." + card.Id), "弃置" + card.DisplayName + "，请求同势力角色提供杀。", [card.Id], [], new Dictionary<string, string> { ["response"] = "faction-request-cost", ["skill"] = _coreNativeFactionSlashOperations.GetFactionRequestSkillId(pending) })).ToArray();
    }

    internal bool TryPublishFactionRequestCost(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending)
    {
        if (pending.PolicySource?.DiscardCost is not > 0 || pending.CostPaid)
            return false;
        if (pending.PolicySource.DiscardCost != 1 || pending.CandidateSeats.Count == 0)
            throw new InvalidOperationException("A paid faction request requires one physical cost and another eligible provider.");
        var choices = FactionRequestCostChoices(pending);
        if (choices.Count == 0)
            throw new InvalidOperationException("The requester has no physical hand or equipment card to pay.");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.RespondSlash, pending.OwnerSeat, "发动主公技：先弃置一张牌。", [], [], pending.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            RequiredCardKind = CardKind.Slash,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return true;
    }

    internal void ResolveFactionRequestCostChoice(PromptChoice selected, bool advanceToHumanBoundary)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("Faction request payment lost its pending request.");
        var canonical = FactionRequestCostChoices(pending).SingleOrDefault(choice => choice.Id == selected.Id);
        if (pending.CostPaid || pending.PolicySource?.DiscardCost != 1 || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } prompt || prompt.PlayerSeat != pending.OwnerSeat || canonical is null || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([canonical], [selected]))
            throw new InvalidOperationException("Faction request payment changed its physical owner cost.");
        var cardId = canonical.Cards.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = _cardZonesOperations._cardZones.CardsAt(from).Single(item => item.Id == cardId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending.CostPaid = true;
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(pending.OwnerSeat, pending.OwnerSeat, card, from, new CardMoveReason("program.faction-request.cost"));
        if (_coreFactionRequestCostRecoveriesOperations.TryPausePaidFactionRequestCost(pending, card, from))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (advanceToHumanBoundary)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
            return;
        }

        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal global::CardGame.Core.GameEngine.FactionProviderRewardReceipt CaptureFactionProviderReward(global::CardGame.Core.NativeOwners.FactionCardRequestHandle pending) => new(pending.OwnerFrameId, pending.ProviderRewarded ? 0 : pending.PolicySource?.ProviderDrawCount ?? 0);
    internal void RewardFactionRequestProvider(global::CardGame.Core.GameEngine.FactionProviderRewardReceipt receipt, int providerSeat)
    {
        if (receipt.DrawCount <= 0)
            return;
        if (_resolutionStackOperations._resolutionStack.Any(frame => frame.Id == receipt.OwnerFrameId))
            _coreCardAttackStateOperations.UpdateCardContinuations(receipt.OwnerFrameId, state => state with { FactionCardRequest = state.FactionCardRequest is { } request ? request with { ProviderRewarded = true } : null });
        if (_coreMatchParticipantsPlayersAccess._players[providerSeat].IsAlive)
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[providerSeat], receipt.DrawCount, true, new CardMoveReason("program.faction-request.provider-reward"));
    }
}
