#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativePromptBoundaryRuntime
{
    internal CommandError? ValidateHumanPrompt(int actorSeat, DecisionKind expectedKind, PromptId? promptId, CommandErrorCode inactiveCode)
    {
        if (!_coreActualTurnProgressCurrentSeatStartedAccess._started)
        {
            return new CommandError(CommandErrorCode.NotStarted, "Call StartGameCommand before submitting a player action.");
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            return new CommandError(CommandErrorCode.Completed, "The game is already completed.");
        }

        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat))
        {
            return new CommandError(CommandErrorCode.InvalidActor, "The actor seat is invalid.");
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision.Kind != expectedKind)
        {
            return new CommandError(inactiveCode, "The engine is not waiting for this player action.");
        }

        if (_coreProgramParticipantPromptsOperations.GetPendingDecisionForActor(actorSeat)is null)
        {
            return new CommandError(CommandErrorCode.NotActorTurn, "Only the published prompt responder may act.");
        }

        if (promptId is { } supplied && supplied != _corePendingDecisionPolicyPipelineOperations._pendingDecision.PromptId)
        {
            return new CommandError(CommandErrorCode.InvalidPrompt, "The prompt id is no longer current.");
        }

        return null;
    }

    internal void RequestHumanPlay()
    {
        _programPipelineHooksOperations._programPipelineHooks.PreparePlayPhaseDecision(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatStartedAccess._currentSeat]);
        var legal = _coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatStartedAccess._currentSeat]);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.PlayCard, _coreActualTurnProgressCurrentSeatStartedAccess._currentSeat, "请选择一张牌和目标，或结束出牌阶段。", legal.Where(action => action.CardId.HasValue).Select(action => action.CardId!.Value).Distinct().ToArray(), legal.SelectMany(action => action.TargetSeats).Distinct().ToArray())
        {
            PromptId = CreatePromptId(),
            Choices = CreatePlayChoices(legal),
            ForcedHandSelection = _programPipelineHooksOperations._programPipelineHooks.ForcedPlayHandSelection(_coreActualTurnProgressCurrentSeatStartedAccess._currentSeat)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.AwaitingHumanPlay;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal PromptId CreatePromptId() => new(++_corePlayerDecisionNextPromptIdAccess._nextPromptId);
    internal static IReadOnlyList<PromptChoice> CreatePlayChoices(IReadOnlyList<LegalAction> legalActions) => RuntimeSkills.RuntimeRules_Core_CommonRules.CreatePlayChoices(legalActions);
    internal IReadOnlyList<PromptChoice> CreateResponseChoices(IReadOnlyList<Card> responseCards, CardKind requiredCardKind, CardKind? incomingCard = null, Func<Card, CardKind>? effectiveCardKindSelector = null, bool includeBagua = false, bool includeFactionDefense = false, bool includeFactionSlash = false, CharacterState? responder = null)
    {
        var responseName = CardCatalog.Get(requiredCardKind).DisplayName;
        var isDodge = requiredCardKind == CardKind.Dodge;
        var responseAction = isDodge ? "dodge" : "slash";
        var incomingName = incomingCard is { } incoming ? CardCatalog.Get(incoming).DisplayName : "这张牌";
        var isGroupAttack = incomingCard is CardKind.BarbarianAssault or CardKind.ArrowBarrage;
        var isSlashAttack = incomingCard is CardKind.Slash or CardKind.FireSlash or CardKind.ThunderSlash or CardKind.ColdSlash;
        var incomingAttackName = isGroupAttack || isSlashAttack ? $"【{(incomingName)}】" : "【杀】";
        var damageDescription = isDodge ? isGroupAttack ? $"不响应，受到【{(incomingName)}】造成的伤害。" : $"不响应，受到{(incomingAttackName)}造成的伤害。" : isGroupAttack ? $"不打出【杀】，受到【{(incomingName)}】造成的伤害。" : "不打出【杀】，受到【决斗】造成的伤害。";
        var choices = responseCards.SelectMany(card =>
        {
            // A multi-card response contributes its first material to the
            // response pool, but only its complete source-bound choice may
            // pay it. Do not parse that material as an independent card.
            if (responder is not null && !_coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(responder, card, requiredCardKind))
                return Array.Empty<PromptChoice>();
            var effectiveCardKind = effectiveCardKindSelector?.Invoke(card) ?? requiredCardKind;
            var cardDescription = CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, requiredCardKind) ? $"打出【{(responseName)}】" : $"将【{(card.DisplayName)}】当作【{(responseName)}】";
            var description = isDodge ? isGroupAttack ? $"{(cardDescription)}，响应【{(incomingName)}】。" : _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } uncancelable && _coreSlashSuitDiscardOperations.Value.IsSlashDodgeCancellationPrevented(uncancelable) ? $"{(cardDescription)}，响应这次{(incomingAttackName)}；此杀不能被抵消。" : $"{(cardDescription)}，抵消这次{(incomingAttackName)}。" : isGroupAttack ? $"{(cardDescription)}，响应【{(incomingName)}】。" : $"{(cardDescription)}，继续应战【决斗】。";
            var programSources = responder is null ? [] : _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, effectiveCardKind, forResponse: true);
            var hasIdentity = responder is not null && _coreCardConversionsOperations.HasProgramCardIdentity(responder, card);
            var identitySources = responder is null ? [] : _coreCardConversionsOperations.GetProgramCardIdentitySources(responder, card, effectiveCardKind, forResponse: true);
            var originalPile = responder is not null && _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(responder, card, effectiveCardKind, true);
            var includeUnspecified = !hasIdentity && (responder is null || !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(responder, card) || originalPile) && CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, requiredCardKind);
            var sources = new List<CardConversionSource?>();
            if (includeUnspecified)
                sources.Add(null);
            if (hasIdentity)
                sources.AddRange(identitySources);
            else
            {
                sources.AddRange(programSources);
            }

            return sources.Select((source, index) =>
            {
                var parameters = new Dictionary<string, string>
                {
                    ["response"] = responseAction,
                    ["required-card"] = responseName,
                    ["response-card-kind"] = effectiveCardKind.ToString()
                };
                if (source is null && originalPile)
                    parameters["native-original-public-pile"] = "true";
                if (source is not null)
                    CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
                var suffix = source is null ? string.Empty : $".conversion-{(index)}";
                return new PromptChoice(new ChoiceId($"respond.{(responseAction)}.card-{(card.Id)}{(suffix)}"), _coreCardConversionsOperations.DescribeConversion(source, description), [card.Id], [], parameters);
            });
        }).ToList();
        if (isDodge && responder is not null)
            choices.AddRange(_coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Dodge));
        if (!isDodge && requiredCardKind == CardKind.Slash && responder is not null)
        {
            foreach (var pair in _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(responder))
            {
                choices.Add(new PromptChoice(new ChoiceId($"respond.zhangba-slash.cards-{(pair[0].Id)}-{(pair[1].Id)}"), isGroupAttack ? $"发动【丈八蛇矛】，将两张手牌当【杀】响应【{(incomingName)}】。" : "发动【丈八蛇矛】，将两张手牌当【杀】继续应战【决斗】。", [pair[0].Id, pair[1].Id], [], new Dictionary<string, string> { ["response"] = "zhangba-slash", ["required-card"] = responseName, ["response-card-kind"] = CardKind.Slash.ToString(), ["equipment"] = CardKind.ZhangbaSerpentSpear.ToString() }));
            }

            foreach (var selection in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Slash, forResponse: true))
            {
                var cardIds = selection.Cards.Select(card => card.Id).ToArray();
                var parameters = new Dictionary<string, string>
                {
                    ["response"] = "program-view-as-slash",
                    ["required-card"] = responseName,
                    ["response-card-kind"] = CardKind.Slash.ToString()
                };
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
                choices.Add(new PromptChoice(new ChoiceId($"respond.program-view-as-slash.{(selection.Source.SkillId.Length)}:" + $"{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.cards-{(string.Join('-', cardIds))}"), isGroupAttack ? $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，将 {(cardIds.Length)} 张手牌当【杀】响应【{(incomingName)}】。" : $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，将 {(cardIds.Length)} 张手牌当【杀】继续应战【决斗】。", cardIds, [], parameters));
            }
        }

        if (isDodge && responder is not null)
            choices.AddRange(_coreProgramDodgeResponsesOperations.ProgramDodgeResponseChoices(responder));
        if (isDodge && includeBagua)
        {
            choices.Add(new PromptChoice(new ChoiceId("respond.bagua"), isSlashAttack ? _coreActiveNativeAttackQueriesOperations.ActiveCardAttack is { } uncancelableBagua && _coreSlashSuitDiscardOperations.Value.IsSlashDodgeCancellationPrevented(uncancelableBagua) ? $"发动【八卦阵】判定，红色牌视为打出【闪】响应{(incomingAttackName)}；此杀不能被抵消。" : $"发动【八卦阵】判定，红色牌视为打出【闪】抵消{(incomingAttackName)}。" : "发动【八卦阵】判定，红色牌视为打出【闪】。", [], [], new Dictionary<string, string> { ["response"] = "bagua", ["required-card"] = responseName, ["equipment"] = CardKind.BaguaFormation.ToString() }));
            if (responder?.IsHuman == true)
                choices.Add(CoreGameEngineBaguaResponsesRuntime.BaguaSkipChoice());
        }

        if (isDodge && includeFactionDefense)
        {
            var policySource = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(responder!, CardKind.Dodge)!;
            choices.Add(new PromptChoice(new ChoiceId("respond.hujia"), $"发动【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(policySource))}】，依次询问{(_coreProgramFactionPoliciesOperations.FactionProviderDescription(policySource))}是否替你打出【闪】。", [], [], new Dictionary<string, string> { ["response"] = "faction-defense-request", ["required-card"] = responseName, ["skill"] = policySource.SkillId }));
        }

        if (!isDodge && requiredCardKind == CardKind.Slash && includeFactionSlash)
        {
            var policySource = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(responder!, CardKind.Slash)!;
            choices.Add(new PromptChoice(new ChoiceId("respond.jijiang"), $"发动【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(policySource))}】，依次询问{(_coreProgramFactionPoliciesOperations.FactionProviderDescription(policySource))}是否替你打出【杀】。", [], [], new Dictionary<string, string> { ["response"] = "faction-slash-request", ["required-card"] = responseName, ["skill"] = policySource.SkillId }));
        }

        choices.Add(new PromptChoice(new ChoiceId("respond.take-damage"), damageDescription, [], [], new Dictionary<string, string> { ["response"] = "take-damage" }));
        return choices;
    }

    internal void RefreshNationalResponseDecision()
    {
        if (!_coreNativeDamageHandlesOperations.Value.IsNationalWarMode || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondDodge or DecisionKind.RespondSlash } pending || pending.RequiredCardKind is not { } requiredCardKind)
        {
            return;
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[pending.PlayerSeat];
        var responseCards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, requiredCardKind);
        var includeBagua = pending.Choices.Any(choice => choice.Parameters.TryGetValue("response", out var response) && response == "bagua");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = pending with
        {
            PromptId = CreatePromptId(),
            ValidCardIds = responseCards.Select(card => card.Id).ToArray(),
            Choices = CreateResponseChoices(responseCards, requiredCardKind, pending.IncomingCard, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, requiredCardKind), includeBagua, responder: responder)
        };
    }

    internal bool IsHumanDecisionPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { } decision && (_coreMultiplayerOperations.IsHumanControlledSeat(decision.PlayerSeat) || _coreProgramParticipantPromptsOperations.IsIssuedParticipantPrompt(decision));
    internal bool IsAiResponsePending() => ((_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.StoneAxe or DecisionKind.CixiongDoubleSwords or DecisionKind.QinglongCrescentBlade or DecisionKind.IceSword or DecisionKind.QilinBow or DecisionKind.ZhuqueFan)) && !_coreMultiplayerOperations.IsHumanControlledSeat(_corePendingDecisionPolicyPipelineOperations._pendingDecision.PlayerSeat);
    internal bool IsAiNullificationPending() => _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is { } pending && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.Nullification } decision && pending.CandidateIndex < pending.CandidateSeats.Count && decision.PlayerSeat == pending.CandidateSeats[pending.CandidateIndex] && !_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat].IsHuman;
    internal bool IsAiHarvestPending() => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.PublicDraft } && _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.SelectHarvestCard && !_coreMultiplayerOperations.IsHumanControlledSeat(_corePendingDecisionPolicyPipelineOperations._pendingDecision.PlayerSeat);
    internal bool IsAiTargetCardSelectionPending() => _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is { } pending && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.SelectTargetCard, PlayerSeat: var playerSeat } decision && decision.PlayerSeat == pending.SourceSeat && !_coreMatchParticipantsPlayersAccess._players[playerSeat].IsHuman;
    internal bool IsAiFireAttackPending() => _coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not null && _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard, PlayerSeat: var playerSeat } && !_coreMultiplayerOperations.IsHumanControlledSeat(playerSeat);
    internal bool IsAiSkipDiscardPolicyPending() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.SkipDiscardPolicy, PlayerSeat: var playerSeat } && playerSeat == _coreActualTurnProgressCurrentSeatStartedAccess._currentSeat && !_coreMatchParticipantsPlayersAccess._players[playerSeat].IsHuman;
    internal bool IsAiDyingResponsePending() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && dying.ResponderIndex < dying.ResponderSeats.Count && !_coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat].IsHuman;
    internal void RequireHumanDecision(DecisionKind expected)
    {
        EnsureStarted();
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind != expected || !_coreMultiplayerOperations.IsHumanControlledSeat(_corePendingDecisionPolicyPipelineOperations._pendingDecision.PlayerSeat))
        {
            throw new InvalidOperationException($"The engine is not waiting for human decision {(expected)}.");
        }
    }

    internal void ClearPendingDecision()
    {
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None ? EngineStatus.Running : EngineStatus.Completed;
    }

    internal void EnsureStarted()
    {
        if (!_coreActualTurnProgressCurrentSeatStartedAccess._started)
        {
            throw new InvalidOperationException("Call Start before advancing the game.");
        }
    }
}
