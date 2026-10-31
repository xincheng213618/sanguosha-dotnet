#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTopReorderRuntime
{
    internal SkillProgramStepOutcome BeginProgramTopReorder(ProgramSkillFrame frame, int maximumCards, SkillProgramNumberExpression? numberExpression, int? exactTopCount = null, ProgramPopulationThresholdCount? population = null, string? allBottomStateId = null, bool requireCurrentSeat = true)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.TopReorder is not null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || !_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsAlive || (requireCurrentSeat && active.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat))
            throw new InvalidOperationException("Top ordering requires one current owner and clean prompt.");
        if (numberExpression is not null and not SkillProgramNumberExpression.LivingPlayerCount)
            throw new InvalidOperationException("Unsupported top-ordering count expression.");
        _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        var count = Math.Min(maximumCards, _cardZonesOperations._cardZones.Count(CardLocation.DrawPile));
        if (numberExpression == SkillProgramNumberExpression.LivingPlayerCount)
            count = Math.Min(count, _coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive));
        var populationCount = population is { } policy ? (_coreMatchParticipantsPlayersAccess._players.Count(player => player.IsAlive) < policy.Threshold ? policy.BelowAmount : maximumCards) : (int? )null;
        if (populationCount is { } requested)
            count = Math.Min(count, requested);
        if (count == 0)
            return SkillProgramStepOutcome.Continue;
        var viewed = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(count).Reverse().Select(card => card.Id).ToArray();
        active = active with
        {
            TopReorder = new ProgramTopReorder(population is null ? viewed : Array.AsReadOnly(viewed), [], [], false)
            {
                RequiredTopCount = exactTopCount is { } exact ? Math.Min(exact, count) : null,
                Population = populationCount is { } frozenCount ? new(frame.InstructionIndex, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, frozenCount, allBottomStateId) : null
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(active);
        PublishProgramTopReorderPrompt(active);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void PublishProgramTopReorderPrompt(ProgramSkillFrame frame)
    {
        var order = frame.TopReorder ?? throw new InvalidOperationException("Top ordering lost its state.");
        if (order.RequiredTopCount is not null)
            _coreExactTopReorderProgramsOperations.Value.ValidateExactTopReorder(frame);
        if (order.Population is not null)
            _corePopulationTopReorderProgramsOperations.Value.ValidatePopulationTopReorder(frame);
        var used = order.TopCardIds.Concat(order.BottomCardIds).ToHashSet();
        var choices = order.ViewedCardIds.Where(id => !used.Contains(id)).Select(id =>
        {
            var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(item => item.Id == id);
            var action = order.ChoosingBottom ? "bottom" : "top";
            var label = order.RequiredTopCount is null && order.Population is null ? $"将【{(card.DisplayName)}】置于牌堆{((order.ChoosingBottom ? "底" : "顶"))}。" : $"将观看牌 {(Array.IndexOf(order.ViewedCardIds.ToArray(), id) + 1)}【{(card.DisplayName)}】{(CoreNativeCardClassificationsRuntime.GetSuitDisplayName(card.Suit))}{(card.RankText)}置于牌堆{((order.ChoosingBottom ? "底" : "顶"))}。";
            return new PromptChoice(new ChoiceId($"program-top-order.{(frame.Id)}.{(action)}.{(id)}"), label, [id], [], new Dictionary<string, string> { ["action"] = action });
        }).ToList();
        if (!order.ChoosingBottom && order.RequiredTopCount is null)
            choices.Add(new PromptChoice(new ChoiceId($"program-top-order.{(frame.Id)}.finish-top"), "结束牌堆顶排序，开始安排剩余牌到牌堆底。", [], [], new Dictionary<string, string> { ["action"] = "finish-top" }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTopReorder, frame.OwnerSeat, "依次排列牌堆顶和牌堆底。", choices.SelectMany(choice => choice.Cards).ToArray(), [])
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices.AsReadOnly(),
            TargetSeat = frame.OwnerSeat,
            DeckReorder = new(frame.Id, Array.AsReadOnly(order.ViewedCardIds.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Single(card => card.Id == id))).ToArray()), Array.AsReadOnly(order.TopCardIds.ToArray()), Array.AsReadOnly(order.BottomCardIds.ToArray()), order.ChoosingBottom, order.RequiredTopCount)
        };
        if (order.RequiredTopCount is { } required)
        {
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
            var stage = order.ChoosingBottom ? "牌堆底排序" : $"牌堆顶排序 {(order.TopCardIds.Count)}/{(required)}";
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = _corePendingDecisionPolicyPipelineOperations._pendingDecision with
            {
                Prompt = order.ChoosingBottom ? "依次选择最靠牌堆底的牌。" : $"依次选择牌堆顶的牌（首张下次先摸），需选 {(required)} 张。",
                SkillPrompt = new(frame.SkillId, skill.Name, $"{(skill.Name)} · {(stage)}", order.ChoosingBottom ? "首先选择的牌最靠牌堆底。" : "首先选择的牌下次先摸。")
            };
        }

        if (order.Population is not null)
        {
            var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = _corePendingDecisionPolicyPipelineOperations._pendingDecision with
            {
                Prompt = order.ChoosingBottom ? "依次选择最靠牌堆底的牌。" : "依次选择牌堆顶的牌；也可以将全部牌置于牌堆底。",
                SkillPrompt = new(frame.SkillId, skill.Name, skill.Name + " · " + (order.ChoosingBottom ? "牌堆底排序" : "牌堆顶排序"), order.ChoosingBottom ? "首先选择的牌最靠牌堆底。" : "首先选择的牌下次先摸；结束顶排序后，其余牌依次置底。")
            };
        }

        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal CommandResult SubmitProgramTopReorderAnswer(PromptChoice choice) => _kernelCoreMainOperations.Accept(() =>
    {
        ResolveProgramTopReorderChoice(choice);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    });
    internal void ResolveProgramTopReorderChoice(PromptChoice choice)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Top ordering lost its active program frame.");
        var state = frame.TopReorder ?? throw new InvalidOperationException("Top ordering lost its cards.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var seat } decision || seat != frame.OwnerSeat || !decision.Choices.Any(item => item.Id == choice.Id))
            throw new InvalidOperationException("Top ordering choice is not current.");
        if (state.RequiredTopCount is not null)
            _coreExactTopReorderProgramsOperations.Value.ValidateExactTopReorder(frame);
        if (_corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op == SkillProgramEffectOp.ReorderTopByCurrentFaluTokens && (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame)))
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "观看技能的来源已失效，未提交的排序结束。");
            return;
        }

        if (state.Population is not null)
        {
            _corePopulationTopReorderProgramsOperations.Value.ValidatePopulationTopReorder(frame);
            if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(frame))
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "观看技能的来源已失效，未提交的排序结束。");
                return;
            }
        }

        var used = state.TopCardIds.Concat(state.BottomCardIds).ToHashSet();
        if (state.ViewedCardIds.Any(id => !_cardZonesOperations._cardZones.Contains(CardLocation.DrawPile, id)))
            throw new InvalidOperationException("Top ordering cards changed before commitment.");
        var action = choice.Parameters.GetValueOrDefault("action");
        if (action == "finish-top" && state.RequiredTopCount is null && !state.ChoosingBottom && choice.Cards.Count == 0)
            state = state with
            {
                ChoosingBottom = true
            };
        else if (choice.Cards is [var id] && state.ViewedCardIds.Contains(id) && !used.Contains(id))
            state = action switch
            {
                "top" when !state.ChoosingBottom => state with
                {
                    TopCardIds = [..state.TopCardIds, id]
                },
                "bottom" when state.ChoosingBottom => state with
                {
                    BottomCardIds = [..state.BottomCardIds, id]
                },
                _ => throw new InvalidOperationException("Top ordering stage does not match the choice.")};
        else
            throw new InvalidOperationException("Top ordering choice has no current card.");
        if (state.RequiredTopCount is { } required && state.TopCardIds.Count == required)
            state = state with
            {
                ChoosingBottom = true
            };
        if (state.Population is not null)
            state = state with
            {
                TopCardIds = Array.AsReadOnly(state.TopCardIds.ToArray()),
                BottomCardIds = Array.AsReadOnly(state.BottomCardIds.ToArray())
            };
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        frame = frame with
        {
            TopReorder = state
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        if (state.TopCardIds.Count + state.BottomCardIds.Count == state.ViewedCardIds.Count)
        {
            _cardZonesOperations._cardZones.ReorderDrawPileTop(state.ViewedCardIds, state.TopCardIds, state.BottomCardIds);
            if (frame.ZhugeGuo is not null)
            {
                _characterZhugeGuoOlOperations.Value.MarkYuhuaTopPlacement(frame, state);
                frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            }

            if (state.Population?.AllBottomStateId is { } completionState)
                _coreProgramSkillChangesOperations.SetProgramBooleanState(frame, completionState, state.ViewedCardIds.Count > 0 && state.TopCardIds.Count == 0 && state.BottomCardIds.Count == state.ViewedCardIds.Count);
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { TopReorder = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        }
        else
            PublishProgramTopReorderPrompt(frame);
    }

    internal bool IsAiProgramTopReorderPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTopReorder, PlayerSeat: var seat } && !_coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    internal void ResolvePendingAiProgramTopReorder()
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("AI top ordering has no program frame.");
        var state = frame.TopReorder ?? throw new InvalidOperationException("AI top ordering has no cards.");
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI top ordering has no prompt.");
        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var delayed = _corePhysicalOwnedCardQueriesOperations.GetJudgment(owner).Count(card => !_characterDaQiaoLeOperations.IsZixiQinJudgmentCard(card.Id) && CoreNativeCardClassificationsRuntime.IsDelayedCard(_coreActualTurnsOperations.GetJudgmentEffectiveCardKind(card)));
        var desiredTop = state.RequiredTopCount ?? Math.Min(state.ViewedCardIds.Count, delayed + Math.Max(0, _coreActualTurnsOperations.GetTurnDrawCount(owner)));
        var selected = !state.ChoosingBottom && state.TopCardIds.Count >= desiredTop ? decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("action") == "finish-top") : decision.Choices.First(choice => choice.Cards.Count == 1);
        ResolveProgramTopReorderChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }
}
