#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreCommandInputAdaptersRuntime
{
    internal CommandResult SubmitStart(StartGameCommand command)
    {
        if (_coreActualTurnProgressStartedAccess._started)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.AlreadyStarted, "The game has already started.");
        }

        return _kernelCoreMainOperations.Accept(_coreMatchSetupOperations.StartCore);
    }

    internal CommandResult SubmitAdvance(GameCommand command, bool singleStep)
    {
        if (command.ActorSeat != -1)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidActor, "Advance is a host command and must use actor seat -1.");
        }

        if (!_coreActualTurnProgressStartedAccess._started)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.NotStarted, "Call StartGameCommand before advancing the game.");
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None && _coreMatchOutcomeStatusWinnerAccess._status == EngineStatus.Completed)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.Completed, "The game is already completed.");
        }

        return _kernelCoreMainOperations.Accept(singleStep ? _coreRuntimeOperations.AdvanceOneStepCore : _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary);
    }

    internal CommandResult SubmitSelectGeneral(SelectGeneralCommand command)
    {
        var validation = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.SelectGeneral, command.PromptId, CommandErrorCode.InvalidGeneral);
        if (validation is not null)
        {
            return _kernelCoreMainOperations.Reject(validation.Code, validation.Message);
        }

        if (string.IsNullOrWhiteSpace(command.GeneralId))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidGeneral, "A general id is required.");
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null || !_corePendingDecisionPolicyPipelineOperations._pendingDecision.ValidContentIds.Contains(command.GeneralId, StringComparer.Ordinal))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidGeneral, "The general is not one of the published candidates.");
        }

        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanSelectGeneralCore(command.GeneralId, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitPlayCard(PlayCardCommand command)
    {
        var validation = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (validation is not null)
        {
            return _kernelCoreMainOperations.Reject(validation.Code, validation.Message);
        }

        var targets = command.TargetSeats?.ToArray() ?? [];
        if (targets.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidTarget, $"Every target seat must be between 0 and {(_coreMatchDefinitionOptionsPlayerCountAccess._playerCount - 1)}.");
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        if (command.TargetHandSlot is null)
        {
            if (_coreChainedStateBasicConversionsOperations.TrySubmitChainedStateBasicPlay(command, out var chainedResult))
                return chainedResult;
            if (_coreDrawFundedDistinctBasicOffersOperations.TrySubmitDrawFundedDistinctBasicPlay(command, out var drawFundedResult))
                return drawFundedResult;
            if (_coreRoundOnceCardlessNativeUsesOperations.TrySubmitRoundOnceCardlessPlay(command, out var roundCardlessResult))
                return roundCardlessResult;
            if (_coreTieredRoundZeroUsesOperations.TrySubmitTieredRoundZeroPlay(command, out var tieredZeroResult))
                return tieredZeroResult;
        }

        var card = _coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, command.CardId);
        if (card is null)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidCard, "The selected card is not in the actor's playable zones.");
        }

        var legal = _coreNativeLegalActionsOperations.BuildLegalActions(actor, selectedPhysicalCardId: command.CardId).Where(action => action.CardId == command.CardId && action.Kind != LegalActionKind.EndPlay).ToArray();
        var action = CoreNativeOwnedCardQueriesRuntime.SelectPlayAction(legal, card, targets, command.PlayedCardKind, command.TargetCardId, command.ConversionSource, command.AdditionalConversionSources);
        if (action is null)
        {
            return _kernelCoreMainOperations.Reject(legal.Length == 0 ? CommandErrorCode.IllegalAction : CommandErrorCode.InvalidTarget, "The selected card and exact target list are not a legal choice for this prompt.");
        }

        if (command.TargetHandSlot is { } slot)
        {
            if (action.Kind is not (LegalActionKind.Dismantlement or LegalActionKind.Snatch) || action.TargetCardId is not null || targets.Length != 1 || slot < 0 || slot >= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targets[0]]).Count)
                return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The selected hidden hand slot is not a legal target-card choice.");
            action = action with
            {
                TargetHandPreselection = new(targets[0], slot, _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targets[0]]).Count, _coreTargetCardPreselectionOperations.TargetHandMovementSequence(targets[0]))
            };
        }

        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanPlayCore(actor, action, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitUseEquipmentEffect(UseEquipmentEffectCommand command)
    {
        var validation = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (validation is not null)
        {
            return _kernelCoreMainOperations.Reject(validation.Code, validation.Message);
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var cardIds = command.CardIds?.Distinct().ToArray() ?? [];
        var targets = command.TargetSeats?.Distinct().ToArray() ?? [];
        var action = _coreNativeLegalActionsOperations.BuildLegalActions(actor).SingleOrDefault(candidate => candidate.Kind == LegalActionKind.UseEquipmentEffect && candidate.EquipmentKind == command.EquipmentKind);
        if (action is null)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The requested equipment effect is not legal in the current play phase.");
        }

        if (cardIds.Length < action.MinCardCount || cardIds.Length > action.MaxCardCount || command.CardIds?.Count != cardIds.Length || cardIds.Any(cardId => !action.SelectableCardIds.Contains(cardId)))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidCard, "The equipment effect card selection is invalid.");
        }

        if (targets.Length < action.MinTargetCount || targets.Length > action.MaxTargetCount || command.TargetSeats?.Count != targets.Length || targets.Any(target => !action.SelectableTargetSeats.Contains(target)))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidTarget, "The equipment effect target selection is invalid.");
        }

        if (command.EquipmentKind == CardKind.ZhangbaSerpentSpear && _coreNextActualUseTargetAdjustmentsOperations.ValidateNextActualUseZhangba(actor, cardIds, targets)is { } adjustedError)
            return _kernelCoreMainOperations.Reject(adjustedError.Code, adjustedError.Message);
        if (!CoreGameEngineZhangbaSelectionsRuntime.MatchesPublishedCardGroupTarget(action, cardIds, targets))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidTarget, "The selected materials cannot be used against this target.");
        if (command.EquipmentKind == CardKind.ZhangbaSerpentSpear && _coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(actor.Seat, cardIds))
            return _kernelCoreMainOperations.Reject(CommandErrorCode.IllegalAction, "The effective suit of this card use is forbidden this turn.");
        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanUseEquipmentEffectCore(actor, command.EquipmentKind, cardIds, targets, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitEndPlay(EndPlayPhaseCommand command)
    {
        var validation = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (validation is not null)
        {
            return _kernelCoreMainOperations.Reject(validation.Code, validation.Message);
        }

        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanEndPlayCore(advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitPromptAnswer(int actorSeat, PromptId prompt, ChoiceId choice)
    {
        if (_characterLiuFengBoundaryOperations.IsBoundaryLiuFengQinlingAnswer(choice))
            return _characterLiuFengBoundaryOperations.SubmitBoundaryLiuFengQinlingAnswer(actorSeat, prompt, choice);
        if (_characterHuanShuJunOlOperations.IsHuanpeiAnswer(choice))
            return _characterHuanShuJunOlOperations.SubmitHuanpeiAnswer(actorSeat, prompt, choice);
        if (_characterLiuZhangOlOperations.IsZonghuAnswer(choice))
            return _characterLiuZhangOlOperations.SubmitZonghuAnswer(actorSeat, prompt, choice);
        if (_characterGuoTuOlOperations.IsWeijieAnswer(choice))
            return _characterGuoTuOlOperations.SubmitWeijieAnswer(actorSeat, prompt, choice);
        if (_coreChainedStateBasicConversionsOperations.IsChainedStateBasicAnswer(choice))
            return _coreChainedStateBasicConversionsOperations.SubmitChainedStateBasicAnswer2(actorSeat, prompt, choice);
        if (_coreDrawFundedDistinctBasicPaymentOperations.IsDrawFundedDistinctBasicAnswer(choice))
            return _coreDrawFundedDistinctBasicPaymentOperations.SubmitDrawFundedDistinctBasicAnswer2(actorSeat, prompt, choice);
        if (_coreRequestedDeckBasicsOperations.IsRequestedDeckBasicAnswer(choice))
            return _coreRequestedDeckBasicsOperations.SubmitRequestedDeckBasicAnswer(actorSeat, prompt, choice);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.ProgramJudgmentTrigger)
        {
            return _coreJudgmentProgramsOperations.Value.SubmitProgramJudgmentTriggerAnswer(actorSeat, prompt, choice);
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.ProgramJudgmentReplacement)
        {
            return _coreJudgmentReplacementProgramsOperations.SubmitProgramJudgmentReplacementAnswer(actorSeat, prompt, choice);
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.ProgramTrigger)
        {
            return _coreProgramLifecycleOperations.SubmitProgramTriggerAnswer(actorSeat, prompt, choice);
        }

        if (!_coreActualTurnProgressStartedAccess._started)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.NotStarted, "Call StartGameCommand before answering a prompt.");
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.Completed, "The game is already completed.");
        }

        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidActor, "The prompt responder seat is invalid.");
        }

        var pending = _corePendingDecisionPolicyPipelineOperations._pendingDecision;
        if (pending is null || pending.SkillPrompt is null && pending.Kind is not (DecisionKind.SelectFaction or DecisionKind.OpeningHand or DecisionKind.RespondDodge or DecisionKind.RespondSlash or DecisionKind.RescueDying or DecisionKind.SelectHarvestCard or DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard or DecisionKind.ProgramTopReorder or DecisionKind.ProgramRepeatJudgment or DecisionKind.SkipDiscardPolicy or DecisionKind.Yingbo or DecisionKind.StoneAxe or DecisionKind.CixiongDoubleSwords or DecisionKind.QinglongCrescentBlade or DecisionKind.IceSword or DecisionKind.QilinBow or DecisionKind.ZhuqueFan or DecisionKind.Nullification or DecisionKind.SelectTargetCard))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "There is no answerable prompt awaiting a response.");
        }

        if (pending.ParticipantFrameId is not null)
            pending = _coreProgramParticipantPromptsOperations.GetPendingDecisionForActor(actorSeat);
        if (pending is null || pending.PlayerSeat != actorSeat)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.NotActorTurn, "Only the published prompt responder may answer it.");
        }

        if (pending.PromptId != prompt)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "The prompt id is no longer current.");
        }

        var selected = pending.Choices.FirstOrDefault(candidate => candidate.Id == choice);
        if (selected is null)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The choice was not published in the current prompt.");
        }

        if (pending.Kind == DecisionKind.OpeningHand)
            return _kernelCoreMainOperations.Accept(() => _coreOpeningCustomizationOperations.Value.ResolveOpeningHandChoice(selected));
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is RecoveryReplacementFrame && pending.Kind == DecisionKind.RecoveryReplacement)
            return _coreRecoveryReplacementOperations.SubmitRecoveryReplacementAnswer(selected);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is CardDeclarationChallengeFrame)
            return _kernelCoreMainOperations.Accept(() =>
            {
                _coreCardDeclarationsOperations.AnswerCardDeclaration(selected);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            });
        if (selected.Parameters.GetValueOrDefault("response") == "round-once-cardless-basic")
            return _kernelCoreMainOperations.Accept(() => _coreRoundOnceCardlessNativeResponsesOperations.TryResolveRoundOnceCardlessResponse(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        if (selected.Parameters.GetValueOrDefault("response") == "mou-xiao-qiao-jueyan")
            return _kernelCoreMainOperations.Accept(() =>
            {
                _characterXiaoQiaoMouOlOperations.BeginMouXqJueyanResponse(selected);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                    _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
            });
        if (selected.Parameters.GetValueOrDefault("response") == "tiered-round-zero-use")
            return _kernelCoreMainOperations.Accept(() => _coreTieredRoundZeroResponsesOperations.ResolveTieredRoundZeroResponse(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        if (selected.Parameters.GetValueOrDefault("response") == "extended-view-as")
            return _kernelCoreMainOperations.Accept(() => _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse2(selected));
        if (selected.Parameters.GetValueOrDefault("response") == "program-dodge")
            return _kernelCoreMainOperations.Accept(() => _coreProgramDodgeResponsesOperations.BeginProgramDodgeResponse(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        if (selected.Parameters.GetValueOrDefault("response") == "friend-duel-discard")
            return _kernelCoreMainOperations.Accept(() =>
            {
                _coreFriendDuelResponsesOperations.ResolveFriendDuelDiscard(selected);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                    _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
            });
        if (pending.Kind == DecisionKind.SelectFaction)
        {
            if (!selected.Parameters.TryGetValue("faction-id", out var factionId) || !CoreMainDefinitions.GodFactionChoices.Contains(factionId, StringComparer.Ordinal))
            {
                return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The god-faction choice is malformed.");
            }

            return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanSelectGodFactionCore(factionId, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        }

        if (pending.Kind == DecisionKind.ProgramTopReorder && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { TopReorder.RequiredTopCount: not null })
            return _coreProgramTopReorderOperations.SubmitProgramTopReorderAnswer(selected);
        if (pending.Kind == DecisionKind.ProgramTopReorder && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { TopReorder.Population: not null })
            return _coreProgramTopReorderOperations.SubmitProgramTopReorderAnswer(selected);
        if (pending.SkillPrompt is not null)
            return _coreSkillPromptsOperations.Value.SubmitPindianPromptAnswer(selected);
        if (pending.Kind is DecisionKind.RespondDodge or DecisionKind.RespondSlash)
            _coreCardConversionsOperations.CaptureSelectedResponseConversion(selected);
        if (pending.Kind == DecisionKind.RescueDying)
        {
            return SubmitDyingPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.SelectHarvestCard)
        {
            return SubmitHarvestPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.SelectTargetCard)
        {
            return SubmitTargetCardPromptAnswer(selected);
        }

        if (pending.Kind is DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard)
        {
            return SubmitFireAttackPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.ProgramTopReorder)
        {
            return _coreProgramTopReorderOperations.SubmitProgramTopReorderAnswer(selected);
        }

        if (pending.Kind == DecisionKind.ProgramRepeatJudgment)
        {
            return _coreProgramRepeatedJudgmentOperations.SubmitProgramRepeatJudgmentAnswer(selected);
        }

        if (pending.Kind == DecisionKind.SkipDiscardPolicy)
        {
            return SubmitSkipDiscardPolicyAnswer(selected);
        }

        if (pending.Kind == DecisionKind.Yingbo)
        {
            return _characterLuMengMouOperations.SubmitYingboPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.StoneAxe)
        {
            return SubmitStoneAxePromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.CixiongDoubleSwords)
        {
            return SubmitCixiongDoubleSwordsPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.QinglongCrescentBlade)
        {
            return SubmitQinglongCrescentBladePromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.IceSword)
        {
            return SubmitIceSwordPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.QilinBow)
        {
            return SubmitQilinBowPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.ZhuqueFan)
        {
            return SubmitZhuqueFanPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.Nullification)
        {
            return SubmitNullificationPromptAnswer(selected);
        }

        if (pending.Kind == DecisionKind.RespondSlash)
        {
            return SubmitSlashPromptAnswer(selected);
        }

        if (!selected.Parameters.TryGetValue("response", out var response))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The prompt choice has no supported response effect.");
        }

        if (response == "bagua-skip" && selected.Cards.Count == 0)
            return _kernelCoreMainOperations.Accept(_coreBaguaResponsesOperations.SkipBaguaResponseCore);
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null)
        {
            return SubmitFactionDefensePromptAnswer(selected, response);
        }

        return response switch
        {
            "faction-defense-request" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanRequestFactionDefenseCore(_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "dodge" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanRespondCore(useDodge: true, useBagua: false, requestedDodgeCardId: selected.Cards[0], requestedResponseCardKind: CoreNativeCardClassificationsRuntime.ReadResponseCardKind(selected), advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "bagua" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanRespondCore(useDodge: false, useBagua: true, requestedDodgeCardId: null, requestedResponseCardKind: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "take-damage" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanRespondCore(useDodge: false, useBagua: false, requestedDodgeCardId: null, requestedResponseCardKind: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The prompt choice is malformed for this response window.")};
    }

    internal CommandResult SubmitFactionDefensePromptAnswer(PromptChoice selected, string response) => response switch
    {
        "extended-view-as" when selected.Cards.Count == 2 && selected.Parameters.GetValueOrDefault("output-kind") == "Dodge" && CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out var source) && _characterGuanHaiOlOperations.IsXieweiSource(source) => _kernelCoreMainOperations.Accept(() => _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse2(selected)),
        "faction-defense-dodge" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFactionDefenseResponseCore(useDodge: true, useBagua: false, requestedCardId: selected.Cards[0], _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        "faction-defense-bagua" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFactionDefenseResponseCore(useDodge: false, useBagua: true, requestedCardId: null, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        "faction-defense-decline" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFactionDefenseResponseCore(useDodge: false, useBagua: false, requestedCardId: null, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The prompt choice is malformed for this FactionDefense response.")};
    internal CommandResult SubmitNullificationPromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is null || !selected.Parameters.TryGetValue("response", out var response))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "There is no Nullification window to answer.");
        }

        return response switch
        {
            "nullification" when selected.Cards.Count == 1 && selected.Targets.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanNullificationCore(useNullification: true, requestedNullificationCardId: selected.Cards[0], advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands, selectedConversionSource: CoreNativeNullificationRuntime.ReadPublishedNullificationSource(selected))),
            "pass" when selected.Cards.Count == 0 && selected.Targets.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanNullificationCore(useNullification: false, requestedNullificationCardId: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The Nullification choice is malformed.")};
    }

    internal CommandResult SubmitHarvestPromptAnswer(PromptChoice selected)
    {
        if (!selected.Parameters.TryGetValue("action", out var action) || action != "harvest-pick" || selected.Cards.Count != 1 || selected.Targets.Count != 0)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The FiveGrains choice is malformed.");
        }

        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanHarvestCardCore(selected.Cards[0], advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitTargetCardPromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection is not { } pending || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.SelectTargetCard } decision)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "There is no hidden target-card selection to answer.");
        }

        if (_coreExtraEffectForeignSelectionOperations.IsExtraForeignPublicAnswer(pending, selected))
            return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanTargetCardSelectionCore(int.Parse(selected.Parameters["slot-index"], System.Globalization.CultureInfo.InvariantCulture), advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        if (!selected.Parameters.TryGetValue("action", out var action) || action != "target-card-slot" || !selected.Parameters.TryGetValue("slot-index", out var slotText) || !int.TryParse(slotText, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var slot) || selected.Cards.Count != 0 || selected.Targets.Count != 1 || selected.Targets[0] != pending.TargetSeat || !decision.ValidTargetSeats.Contains(pending.TargetSeat) || slot < 0 || slot >= pending.CandidateSlots.Count)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The hidden target-card choice is malformed.");
        }

        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanTargetCardSelectionCore(slot, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitFireAttackPromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFireAttack is not { } pending || !selected.Parameters.TryGetValue("response", out var response))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "There is no FireAttack selection to answer.");
        }

        return response switch
        {
            "fire-attack-reveal" when pending.FireAttackSelection?.RevealedCardId is null && selected.Cards.Count == 1 && selected.Targets.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFireAttackCardCore(selected.Cards[0], advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "fire-attack-discard" when pending.FireAttackSelection?.RevealedCardId is { } && selected.Cards.Count == 1 && selected.Targets.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFireAttackCardCore(selected.Cards[0], advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "fire-attack-skip" when pending.FireAttackSelection?.RevealedCardId is { } && selected.Cards.Count == 0 && selected.Targets.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFireAttackSkipCore(advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The choice is malformed for the FireAttack prompt.")};
    }

    internal CommandResult SubmitStoneAxePromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.StoneAxe })
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的贯石斧触发窗口。");
        }

        if (!selected.Parameters.TryGetValue("action", out var action) || selected.Targets.Count != 0)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "贯石斧选择不符合当前攻击窗口。");
        }

        return action switch
        {
            "stone-axe-use" when selected.Cards.Count == 2 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanStoneAxeCore(selected.Cards, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "stone-axe-skip" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanStoneAxeCore([], advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "贯石斧必须精确弃置两张牌，或选择不发动。")};
    }

    internal CommandResult SubmitCixiongDoubleSwordsPromptAnswer(PromptChoice selected)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords;
        if (pending is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.CixiongDoubleSwords } decision || decision.PlayerSeat != (pending.Stage == CixiongDoubleSwordsStage.SourceActivation ? pending.Attack.SourceSeat : pending.Attack.TargetSeat))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的雌雄双股剑触发窗口。");
        }

        if (!selected.Parameters.TryGetValue("action", out var action) || selected.Targets.Count != 0)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "雌雄双股剑选择不符合当前攻击窗口。");
        }

        var valid = pending.Stage switch
        {
            CixiongDoubleSwordsStage.SourceActivation => action is "cixiong-use" or "cixiong-skip" && selected.Cards.Count == 0,
            CixiongDoubleSwordsStage.TargetChoice => action == "cixiong-draw" && selected.Cards.Count == 0 || action == "cixiong-discard" && selected.Cards.Count == 1,
            _ => false
        };
        return valid ? _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanCixiongDoubleSwordsCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)) : _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "雌雄双股剑必须发动或跳过；发动后目标须弃一张手牌或令来源摸一张牌。");
    }

    internal CommandResult SubmitQinglongCrescentBladePromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QinglongCrescentBlade })
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的青龙偃月刀触发窗口。");
        }

        if (!selected.Parameters.TryGetValue("action", out var action) || selected.Targets.Count is not 0 and not 1)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "青龙偃月刀选择不符合当前攻击窗口。");
        }

        return action switch
        {
            "tousui-request-open" when selected.Cards.Count == 0 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() => _characterChengJiChengCuiOlOperations.TryBeginTousuiQinglongSlash(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!, selected)),
            "kenshang-request-open" when selected.Cards.Count == 0 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() => _characterMaXiuMaTieOlOperations.TryBeginKenshangQinglongSlash(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade!, selected)),
            "mou-xiao-qiao-forced-slash" when selected.Cards.Count == 1 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() =>
            {
                _characterXiaoQiaoMouOlOperations.BeginMouXqForcedSlash(selected);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                    _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
            }),
            "round-once-cardless-forced-slash" when selected.Cards.Count == 0 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreRoundOnceCardlessNativeUsesOperations.TryResolveRoundOnceCardlessForcedSlash(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "tiered-round-zero-forced-slash" when selected.Cards.Count == 0 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreTieredRoundZeroUsesOperations.TryResolveTieredRoundZeroForcedSlash(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "qinglong-slash" when selected.Cards.Count == 1 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanQinglongCrescentBladeCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "qinglong-jijiang" when selected.Cards.Count == 0 && selected.Targets.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanQinglongCrescentBladeCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "qinglong-skip" when selected.Cards.Count == 0 && selected.Targets.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanQinglongCrescentBladeCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "青龙偃月刀必须对同一目标使用提示中精确的一张杀、发动激将，或选择不发动。")};
    }

    internal CommandResult SubmitZhuqueFanPromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { AwaitingZhuqueFanChoice: true } pending || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ZhuqueFan } decision || decision.PlayerSeat != pending.OwnerSeat)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的朱雀羽扇转换窗口。");
        }

        if (selected.Cards.Count != 0 || selected.Targets.Count != 0 || selected.Parameters.GetValueOrDefault("action")is not ("zhuque-fan-normal" or "zhuque-fan-fire"))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "朱雀羽扇必须选择保持普通杀或改为火杀。");
        }

        return _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanZhuqueFanCore(selected.Parameters.GetValueOrDefault("action") == "zhuque-fan-fire", advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
    }

    internal CommandResult SubmitIceSwordPromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.IceSword })
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的寒冰剑触发窗口。");
        }

        var action = selected.Parameters.GetValueOrDefault("action");
        var valid = action switch
        {
            "ice-sword-discard" => _coreNativeCardContinuationCursorsOperations.ActiveIceSword.Activated && selected.Targets.Count == 1 && selected.Cards.Count is 0 or 1,
            "ice-sword-activate" => !_coreNativeCardContinuationCursorsOperations.ActiveIceSword.Activated && selected.Targets.Count == 0 && selected.Cards.Count == 0,
            "ice-sword-damage" => !_coreNativeCardContinuationCursorsOperations.ActiveIceSword.Activated && selected.Targets.Count == 0 && selected.Cards.Count == 0,
            _ => false
        };
        return valid ? _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanIceSwordCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)) : _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "寒冰剑须先决定是否发动，发动后依次选择目标当前的暗手牌或公开装备。");
    }

    internal CommandResult SubmitQilinBowPromptAnswer(PromptChoice selected)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveQilinBow is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QilinBow })
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的麒麟弓触发窗口。");
        }

        var action = selected.Parameters.GetValueOrDefault("action");
        var valid = action switch
        {
            "qilin-bow-discard" => selected.Targets.Count == 1 && selected.Cards.Count == 1,
            "qilin-bow-skip" => selected.Targets.Count == 0 && selected.Cards.Count == 0,
            _ => false
        };
        return valid ? _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanQilinBowCore(selected, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)) : _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "麒麟弓必须选择目标当前的一张公开坐骑，或选择不发动。");
    }

    internal CommandResult SubmitSkipDiscardPolicyAnswer(PromptChoice selected)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.SkipDiscardPolicy })
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidPrompt, "没有等待响应的弃牌阶段政策窗口。");
        }

        if (!selected.Parameters.TryGetValue("action", out var action) || selected.Cards.Count != 0 || selected.Targets.Count != 0)
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "选择不符合当前弃牌阶段窗口。");
        }

        return action switch
        {
            "discard-policy-use" => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanSkipDiscardPolicyCore(useSkill: true, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "discard-policy-skip" => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanSkipDiscardPolicyCore(useSkill: false, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "弃牌阶段提示没有可识别的选择效果。")};
    }

    internal CommandResult SubmitSlashPromptAnswer(PromptChoice selected)
    {
        if (selected.Parameters.GetValueOrDefault("response") == "tousui-request-open")
            return _kernelCoreMainOperations.Accept(() => _characterChengJiChengCuiOlOperations.TryBeginTousuiBorrowedSlash(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!, selected));
        if (selected.Parameters.GetValueOrDefault("response") == "kenshang-request-open")
            return _kernelCoreMainOperations.Accept(() => _characterMaXiuMaTieOlOperations.TryBeginKenshangBorrowedSlash(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword!, selected));
        if (selected.Parameters.GetValueOrDefault("response") == "mou-xiao-qiao-forced-slash")
            return _kernelCoreMainOperations.Accept(() =>
            {
                _characterXiaoQiaoMouOlOperations.BeginMouXqForcedSlash(selected);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                if (_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)
                    _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
            });
        if (selected.Parameters.GetValueOrDefault("response") == "round-once-cardless-forced-slash")
            return _kernelCoreMainOperations.Accept(() => _coreRoundOnceCardlessNativeUsesOperations.TryResolveRoundOnceCardlessForcedSlash(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        if (selected.Parameters.GetValueOrDefault("response") == "tiered-round-zero-forced-slash")
            return _kernelCoreMainOperations.Accept(() => _coreTieredRoundZeroUsesOperations.TryResolveTieredRoundZeroForcedSlash(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
        if (!selected.Parameters.TryGetValue("response", out var response))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The Slash response prompt has no supported response effect.");
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null)
        {
            if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.IsAssistedProgramUse)
                return _kernelCoreMainOperations.Accept(() => _coreAssistedFactionSlashOperations.ResolveAssistedFactionSlashChoice(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands));
            return SubmitFactionSlashPromptAnswer(selected, response);
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true })
        {
            return response switch
            {
                "faction-slash-request" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanBorrowedSwordFactionSlashCore(_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
                "borrowed-sword-slash" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanBorrowedSwordResponseCore(useSlash: true, requestedSlashCardId: selected.Cards[0], requestedEffectiveKind: CoreNativeCardClassificationsRuntime.ReadResponseCardKind(selected), advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
                "zhangba-slash" when selected.Cards.Count == 2 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanZhangbaSlashResponseCore(selected.Cards, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
                "program-view-as-slash" when selected.Cards.Count >= 2 || selected.Cards.Count == 1 && _characterKongShuOlOperations.IsLeiluanSource(CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected)) => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanProgramViewAsSlashResponseCore(selected.Cards, CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected), _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
                "borrowed-sword-give-weapon" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanBorrowedSwordResponseCore(useSlash: false, requestedSlashCardId: null, requestedEffectiveKind: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
                _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The choice is malformed for this Borrowed Sword response window.")};
        }

        return response switch
        {
            "faction-slash-request" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanRequestFactionSlashCore(_coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "slash" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanSlashResponseCore(useSlash: true, requestedSlashCardId: selected.Cards[0], requestedResponseCardKind: CoreNativeCardClassificationsRuntime.ReadResponseCardKind(selected), advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "zhangba-slash" when selected.Cards.Count == 2 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanZhangbaSlashResponseCore(selected.Cards, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "program-view-as-slash" when selected.Cards.Count >= 2 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanProgramViewAsSlashResponseCore(selected.Cards, CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected), _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "take-damage" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanSlashResponseCore(useSlash: false, requestedSlashCardId: null, requestedResponseCardKind: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The choice is malformed for this Slash response window.")};
    }

    internal CommandResult SubmitFactionSlashPromptAnswer(PromptChoice selected, string response) => response switch
    {
        "faction-request-cost" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreFactionRequestPaymentsOperations.ResolveFactionRequestCostChoice(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        "faction-slash-slash" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFactionSlashResponseCore(useSlash: true, requestedCardId: selected.Cards[0], requestedEffectiveKind: CoreNativeCardClassificationsRuntime.ReadResponseCardKind(selected), _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        "zhangba-slash" when selected.Cards.Count == 2 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanZhangbaSlashResponseCore(selected.Cards, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        "program-view-as-slash" when selected.Cards.Count >= 2 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanProgramViewAsSlashResponseCore(selected.Cards, CoreGameEngineCardConversionsRuntime.RequireConversionSource(selected), _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        "faction-slash-decline" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanFactionSlashResponseCore(useSlash: false, requestedCardId: null, requestedEffectiveKind: null, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
        _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The prompt choice is malformed for this FactionSlash response.")};
    internal CommandResult SubmitDyingPromptAnswer(PromptChoice selected)
    {
        if (!selected.Parameters.TryGetValue("response", out var response))
        {
            return _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The dying prompt choice has no supported response effect.");
        }

        _ = CoreGameEngineCardConversionsRuntime.TryReadConversionSource(selected.Parameters, out var peachConversionSource);
        return response switch
        {
            "peach" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanDyingResponseCore(usePeach: true, requestedPeachCardId: selected.Cards[0], useAlcohol: false, requestedAlcoholCardId: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands, peachConversionSource: peachConversionSource)),
            "alcohol" when selected.Cards.Count == 1 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanDyingResponseCore(usePeach: false, requestedPeachCardId: null, useAlcohol: true, requestedAlcoholCardId: selected.Cards[0], advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands, alcoholConversionSource: peachConversionSource)),
            "program-trigger" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanDyingProgramTriggerCore(selected, _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            "let-die" when selected.Cards.Count == 0 => _kernelCoreMainOperations.Accept(() => _coreHumanNativeCommandsOperations.HumanDyingResponseCore(usePeach: false, requestedPeachCardId: null, useAlcohol: false, requestedAlcoholCardId: null, advanceToHumanBoundary: _coreMatchDefinitionOptionsPlayerCountAccess._options.AdvanceAfterHumanCommands)),
            _ => _kernelCoreMainOperations.Reject(CommandErrorCode.InvalidChoice, "The choice is malformed for this dying window.")};
    }
}
