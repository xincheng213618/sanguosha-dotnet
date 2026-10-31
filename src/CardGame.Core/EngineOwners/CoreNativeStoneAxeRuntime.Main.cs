#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeStoneAxeRuntime
{
    internal bool TryBeginStoneAxeChoice(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var effectiveKind = global::CardGame.Core.EngineOwners.CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack);
        var discardableCards = GetStoneAxeDiscardCards(source);
        if (!_coreNativeDamageHandlesOperations.UsesFormalStoneAxe || effectiveKind is not (CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash) || !source.IsAlive || !target.IsAlive || !_coreAdvancedWeaponsOperations.HasWeaponAbility(source, CardKind.StoneAxe) || discardableCards.Count < 2)
        {
            return false;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is not null)
        {
            throw new InvalidOperationException("The engine cannot open two Stone Axe choices at once.");
        }

        var choices = new List<PromptChoice>();
        for (var first = 0; first < discardableCards.Count - 1; first++)
        {
            for (var second = first + 1; second < discardableCards.Count; second++)
            {
                var firstCard = discardableCards[first];
                var secondCard = discardableCards[second];
                choices.Add(new PromptChoice(new ChoiceId($"stone-axe.use.resolution-{(attack.ResolutionId)}.cards-{(firstCard.Id)}-{(secondCard.Id)}"), $"弃置{(DescribeStoneAxeCost(source, firstCard))}与{(DescribeStoneAxeCost(source, secondCard))}，令此【{(CardCatalog.Get(effectiveKind).DisplayName)}】仍造成伤害。", [firstCard.Id, secondCard.Id], [], new Dictionary<string, string> { ["action"] = "stone-axe-use" }));
            }
        }

        choices.Add(new PromptChoice(new ChoiceId($"stone-axe.skip.resolution-{(attack.ResolutionId)}"), "不发动【贯石斧】，此【杀】被【闪】抵消。", [], [], new Dictionary<string, string> { ["action"] = "stone-axe-skip" }));
        _coreNativeCardContinuationCursorsOperations.ActiveStoneAxe = _kernelCoreMainOperations.CreateStoneAxeHandle(attack, discardableCards.Select(card => card.Id).ToArray());
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.AwaitingResponse);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.StoneAxe, source.Seat, $"{(target.Name)} 已用【闪】抵消你的【{(CardCatalog.Get(effectiveKind).DisplayName)}】，是否弃置两张牌发动【贯石斧】？", discardableCards.Select(card => card.Id).ToArray(), [], SourceSeat: source.Seat, IncomingCard: effectiveKind)
        {
            PromptId = source.IsHuman ? _coreNativePromptBoundaryOperations.CreatePromptId() : default,
            IsPrivate = true,
            TargetSeat = target.Seat,
            Choices = choices
        };
        _coreMatchOutcomeStatusAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return true;
    }

    internal IReadOnlyList<Card> GetStoneAxeDiscardCards(CharacterState source) => _corePhysicalOwnedCardQueriesOperations.GetHand(source).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(source)).Where(card => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(source.Seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal string DescribeStoneAxeCost(CharacterState source, Card card)
    {
        var zone = _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Equipment(source.Seat) ? "装备区" : "手牌";
        return $"【{(card.DisplayName)}】（{(zone)}）";
    }

    internal void ResolveStoneAxeChoice(IReadOnlyList<int> discardedCardIds)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveStoneAxe ?? throw new InvalidOperationException("There is no Stone Axe choice to resolve.");
        var attack = pending.Attack;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.StoneAxe } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The Stone Axe choice is not the current Slash continuation.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var distinctIds = discardedCardIds.Distinct().ToArray();
        var useStoneAxe = distinctIds.Length > 0;
        if (useStoneAxe && (distinctIds.Length != 2 || distinctIds.Any(id => !pending.CandidateCardIds.Contains(id))))
        {
            throw new InvalidOperationException("Stone Axe requires two distinct cards from the published candidate set.");
        }

        var currentCards = GetStoneAxeDiscardCards(source);
        var selectedCards = distinctIds.Select(id => currentCards.SingleOrDefault(card => card.Id == id) ?? throw new InvalidOperationException("A selected Stone Axe cost card is no longer owned by its source.")).ToArray();
        _coreNativeCardContinuationCursorsOperations.ActiveStoneAxe = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        if (useStoneAxe)
        {
            var originalLocations = selectedCards.ToDictionary(card => card.Id, card => _cardZonesOperations._cardZones.GetLocation(card.Id));
            var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(source.Seat, source.Seat, distinctIds, CardMoveReasons.StoneAxeDiscard, attack.ResolutionId);
            foreach (var card in selectedCards)
            {
                _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Processing, CardMoveReasons.StoneAxeDiscard);
            }

            foreach (var card in selectedCards)
            {
                if ((card.IsGeneralWeapon || global::CardGame.Core.EngineOwners.CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, originalLocations[card.Id], CardLocation.OutsideGame) || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(card.Id, originalLocations[card.Id], CardLocation.OutsideGame)) && originalLocations[card.Id] == CardLocation.Equipment(source.Seat) && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.OutsideGame)
                    continue;
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.StoneAxeDiscard);
            }

            _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new StoneAxeResolvedEvent(attack.ResolutionId, source.Seat, target.Seat, useStoneAxe, Array.AsReadOnly(distinctIds)));
        _coreMatchLogOperations.AddLog(useStoneAxe ? "EquipmentEffect" : "EquipmentSkipped", useStoneAxe ? $"{(source.Name)} 弃置两张牌发动【贯石斧】，令对 {(target.Name)} 的【杀】仍造成伤害。" : $"{(source.Name)} 未发动【贯石斧】，对 {(target.Name)} 的【杀】被【闪】抵消。", source.Seat, target.Seat);
        if (useStoneAxe)
        {
            if (_coreNativeEquipmentInvocationsOperations.TryBeginStoneAxeEquipmentInvocation(attack))
                return;
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }

            return;
        }

        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }
}
