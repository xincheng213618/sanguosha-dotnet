#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuMengMouRuntime
{
    internal const string YingboSkillId = "mou:yingbo";
    internal NativeOwners.YingboGiftHandle? ActiveYingboGift
    {
        get => _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.YingboGift?.Active == true)is { } id ? _kernelCharacterLuMengMouOperations.CreateYingboGiftHandle(id) : null;
        set
        {
            var id = value?.OwnerFrameId ?? _coreCardAttackStateOperations.FindCardContinuationOwner(state => state.YingboGift?.Active == true);
            if (id is { } owner)
                _coreCardAttackStateOperations.UpdateCardContinuations(owner, state => state with { YingboGift = state.YingboGift is { } current ? current with { Active = value is not null } : null });
        }
    }

    internal void RecordYingboCardUse(long resolutionId, int sourceSeat, CardKind cardKind)
    {
        if (!_coreMouLuMengOperations.Value.UsesFormalMouLuMeng || !CoreGameEngineMouLuMengRuntime.IsDamageCardKind(cardKind))
            return;
        var usageId = CoreGameEngineMouLuMengRuntime.GetRoundDamageCardUsageId(cardKind);
        var wasUsedEarlier = _coreMatchParticipantsPlayersAccess._players.Any(player => _skillRuntimeStateOperations._skillRuntimeState.GetUsage(player.Seat, "system:round-damage-card-history", usageId, SkillUsageScope.Round) > 0);
        var source = _coreMatchParticipantsPlayersAccess._players[sourceSeat];
        if (source.IsAlive && _coreNationalSkillsOperations.HasRuntimeSkill(source, YingboSkillId))
        {
            if (wasUsedEarlier)
                _coreCardUseLifecycleOperations.TryMarkYingboRepeated(resolutionId);
            else
                _coreCardUseLifecycleOperations.TryMarkYingboUnrespondable(resolutionId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YingboCardModeEvent(resolutionId, sourceSeat, cardKind, wasUsedEarlier, CannotBeRespondedTo: !wasUsedEarlier, ConvertsDamageToFire: wasUsedEarlier, DamageBonus: wasUsedEarlier ? 1 : 0));
            _coreMatchLogOperations.AddLog("SkillTriggered", wasUsedEarlier ? $"{(source.Name)} 的【英博】令本轮重复使用的【{(CardCatalog.Get(cardKind).DisplayName)}】伤害改为火焰且伤害 +1。" : $"{(source.Name)} 的【英博】令本轮首次使用的【{(CardCatalog.Get(cardKind).DisplayName)}】不能被响应。", sourceSeat);
        }

        if (_skillRuntimeStateOperations._skillRuntimeState.GetUsage(sourceSeat, "system:round-damage-card-history", usageId, SkillUsageScope.Round) == 0)
        {
            _skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(sourceSeat, "system:round-damage-card-history", usageId, SkillUsageScope.Round, limit: 1);
        }
    }

    internal bool IsYingboUnrespondable(long resolutionId) => _coreMouLuMengOperations.Value.UsesFormalMouLuMeng && (_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.YingboUnrespondable == true);
    internal bool IsYingboRepeated(long resolutionId) => _coreMouLuMengOperations.Value.UsesFormalMouLuMeng && (_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.YingboRepeated == true);
    internal bool TryBeginYingboGift(long resolutionId, int sourceSeat, Card card, CardKind cardKind, IReadOnlyList<Card> physicalCards, YingboGiftContinuation continuation, NativeOwners.CardAttackHandle? attack = null, NativeOwners.GroupCardHandle? group = null)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !IsYingboUnrespondable(resolutionId) || ActiveYingboGift is not null || physicalCards is not [var physicalCard] || physicalCard.Id != card.Id || _cardZonesOperations._cardZones.GetLocation(card.Id) != CardLocation.Processing)
            return false;
        var source = _coreMatchParticipantsPlayersAccess._players[sourceSeat];
        var targets = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != sourceSeat).OrderBy(player => player.Seat).ToArray();
        if (!source.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(source, YingboSkillId) || targets.Length == 0)
            return false;
        _coreCardAttackStateOperations.UpdateCardContinuations(resolutionId, state => state with { YingboGift = new(sourceSeat, new(card.Id, card.Kind, card.Suit, card.Rank), cardKind, continuation, attack?.ResolutionId, group?.OwnerFrameId) });
        var choices = targets.Select(target => new PromptChoice(new ChoiceId($"yingbo.gift.target-{(target.Seat)}.resolution-{(resolutionId)}"), $"将【{(card.DisplayName)}】交给 {(target.Name)}。", [card.Id], [target.Seat], new Dictionary<string, string> { ["action"] = "yingbo-gift" })).Append(new PromptChoice(new ChoiceId($"yingbo.gift.skip.resolution-{(resolutionId)}"), "不交出此牌。", [], [], new Dictionary<string, string> { ["action"] = "yingbo-skip" })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.Yingbo, sourceSeat, $"【{(card.DisplayName)}】结算完毕，是否发动【英博】将此牌交给一名其他角色？", [card.Id], targets.Select(target => target.Seat).ToArray(), SourceSeat: sourceSeat, IncomingCard: cardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = source.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return true;
    }

    internal CommandResult SubmitYingboPromptAnswer(PromptChoice selected)
    {
        if (ActiveYingboGift is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.Yingbo })
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的英博交牌选择。");
        return _kernelCoreMainOperations.Accept(() => HumanYingboCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands));
    }

    internal void HumanYingboCore(PromptChoice selected, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.Yingbo);
        ResolveYingboGiftChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void ResolveYingboGiftChoice(PromptChoice selected)
    {
        var pending = ActiveYingboGift ?? throw new InvalidOperationException("There is no Yingbo gift to resolve.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.Yingbo, PlayerSeat: var ownerSeat } || ownerSeat != pending.SourceSeat)
            throw new InvalidOperationException("The Yingbo gift prompt lost its owner.");
        var action = selected.Parameters.GetValueOrDefault("action");
        int? targetSeat = null;
        if (action == "yingbo-gift" && selected.Cards is [var cardId] && cardId == pending.Card.Id && selected.Targets is [var selectedTarget] && _coreTableSeatQueriesOperations.IsValidPlayerSeat(selectedTarget) && selectedTarget != pending.SourceSeat && _coreMatchParticipantsPlayersAccess._players[selectedTarget].IsAlive)
        {
            targetSeat = selectedTarget;
            _corePhysicalMovementFacadesOperations.MoveCard(pending.Card, CardLocation.Processing, CardLocation.Hand(selectedTarget), new CardMoveReason("skill.yingbo.gift"), _characterHuangQuanMouOperations.NativeGiveCompletion(pending.ResolutionId, pending.SourceSeat, selectedTarget, [pending.Card.Id], _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        }
        else if (action == "yingbo-skip" && selected.Cards.Count == 0 && selected.Targets.Count == 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(pending.Card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.UseFinished);
        }
        else
        {
            throw new InvalidOperationException("The Yingbo gift choice is malformed or no longer legal.");
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new YingboGiftResolvedEvent(pending.ResolutionId, pending.SourceSeat, pending.Card.Id, pending.CardKind, targetSeat));
        _coreMatchLogOperations.AddLog(targetSeat is null ? "SkillSkipped" : "SkillTriggered", targetSeat is { } recipient ? $"{(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].Name)} 发动【英博】，将【{(pending.Card.DisplayName)}】交给 {(_coreMatchParticipantsPlayersAccess._players[recipient].Name)}。" : $"{(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].Name)} 未发动【英博】交牌。", pending.SourceSeat, targetSeat);
        ActiveYingboGift = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (pending.Continuation == YingboGiftContinuation.Attack)
        {
            var attack = pending.Attack ?? throw new InvalidOperationException("The Yingbo attack gift lost its card continuation.");
            var completion = _coreCardAttackStateOperations.CaptureAttackCompletion(attack);
            if (!_coreNativeSlashCompletionOperations.Value.FinishAttack(attack, allowYingboGift: false))
            {
                _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(completion);
            }
        }
        else
        {
            _coreNativeGroupCompletionOperations.FinishGroupAttack(pending.Group ?? throw new InvalidOperationException("The Yingbo group gift lost its card continuation."), allowYingboGift: false);
        }
    }

    internal bool IsAiYingboPending() => ActiveYingboGift is { SourceSeat: var sourceSeat } && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.Yingbo, PlayerSeat: var decisionSeat } && sourceSeat == decisionSeat && !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsHuman;
    internal void ResolvePendingAiYingbo()
    {
        if (!IsAiYingboPending())
            throw new InvalidOperationException("There is no AI Yingbo gift to resolve.");
        // Giving a card is optional. The baseline AI avoids leaking role information
        // and deterministically keeps the card in the discard pile.
        var selected = _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Single(choice => choice.Parameters.GetValueOrDefault("action") == "yingbo-skip");
        ResolveYingboGiftChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
