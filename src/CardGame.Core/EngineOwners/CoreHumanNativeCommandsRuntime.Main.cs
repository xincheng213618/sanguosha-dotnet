#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHumanNativeCommandsRuntime
{
    internal void HumanSelectGeneralCore(string generalId, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.SelectGeneral);
        var pending = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("There is no general-selection prompt.");
        if (!pending.ValidContentIds.Contains(generalId, StringComparer.Ordinal))
        {
            throw new InvalidOperationException("The selected general is not a legal candidate.");
        }

        var general = _coreMatchSetupAvailableGeneralsAccess._availableGenerals.SingleOrDefault(candidate => candidate.Id == generalId) ?? throw new InvalidOperationException("The selected general is no longer available.");
        _coreMatchParticipantSetupOperations.ApplyGeneralSelection(_coreMatchParticipantsPlayersAccess._players[pending.PlayerSeat], general);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanRevealGeneralCore(GeneralSelectionSlot slot, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.EnsureStarted();
        var pendingKind = _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind ?? throw new InvalidOperationException("There is no national-war action prompt.");
        if (pendingKind is not (DecisionKind.PlayCard or DecisionKind.RespondDodge or DecisionKind.RespondSlash))
        {
            throw new InvalidOperationException("The current prompt does not allow a national-war general reveal.");
        }

        _coreNativePromptBoundaryOperations.RequireHumanDecision(pendingKind);
        if (!_coreNativeDamageHandlesOperations.Value.IsNationalWarMode)
        {
            throw new InvalidOperationException("General reveal is only available in national-war lite mode.");
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[_coreMultiplayerOperations.HumanDecisionSeat];
        var action = _coreNationalGeneralRevealOperations.BuildNationalRevealActions(actor).SingleOrDefault(candidate => candidate.Kind == LegalActionKind.RevealGeneral && candidate.GeneralSlot == slot);
        if (action is null)
        {
            throw new InvalidOperationException("The selected general cannot be revealed now.");
        }

        var isResponse = pendingKind is DecisionKind.RespondDodge or DecisionKind.RespondSlash;
        if (!isResponse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        _coreNativeActionDispatchOperations.ExecuteAction(actor, action);
        if (isResponse)
        {
            _coreNativePromptBoundaryOperations.RefreshNationalResponseDecision();
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanPlayCore(CharacterState actor, LegalAction action, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.PlayCard);
        // Submit already selected this exact action. Entering the exclusive
        // command session does not advance rules or call observers.
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeActionDispatchOperations.ExecuteAction(actor, action);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanUseEquipmentEffectCore(CharacterState actor, CardKind equipmentKind, IReadOnlyList<int> cardIds, IReadOnlyList<int> targetSeats, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.PlayCard);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (equipmentKind == CardKind.WoodenOx)
        {
            _coreNativeWoodenOxOperations.ResolveWoodenOx(actor, cardIds[0], targetSeats.SingleOrDefault(-1));
        }
        else
        {
            var cards = cardIds.Select(cardId => _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(actor).Single(card => card.Id == cardId)).ToArray();
            if (targetSeats.Count > 1)
                _coreNextActualUseTargetAdjustmentsOperations.ResolveNextActualUseZhangba(actor, targetSeats, cards);
            else
                _coreNativeSlashUseOperations.ResolveZhangbaSlash(actor, _coreMatchParticipantsPlayersAccess._players[targetSeats[0]], cards);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanEndPlayCore(bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.PlayCard);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreProgramLifecycleOperations.CompleteCurrentPlayPhase();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanRequestFactionDefenseCore(bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondDodge);
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("There is no Dodge response awaiting FactionDefense.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.All(choice => choice.Parameters.GetValueOrDefault("response") != "faction-defense-request") != false)
        {
            throw new InvalidOperationException("FactionDefense is not available in the current response window.");
        }

        _coreNativeFactionDefenseOperations.BeginFactionDefenseRequest(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanFactionDefenseResponseCore(bool useDodge, bool useBagua, int? requestedCardId, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondDodge);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense ?? throw new InvalidOperationException("There is no FactionDefense request awaiting a response.");
        if (pending.CurrentCandidateSeat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current FactionDefense responder is not the human seat.");
        }

        _coreNativeFactionDefenseOperations.ResolveFactionDefenseCandidateResponse(pending, useDodge, useBagua, requestedCardId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanRequestFactionSlashCore(bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("There is no Slash response awaiting FactionSlash.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.All(choice => choice.Parameters.GetValueOrDefault("response") != "faction-slash-request") != false)
        {
            throw new InvalidOperationException("FactionSlash is not available in the current response window.");
        }

        _coreNativeFactionSlashOperations.BeginFactionSlashResponseRequest(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanFactionSlashResponseCore(bool useSlash, int? requestedCardId, CardKind? requestedEffectiveKind, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("There is no FactionSlash request awaiting a response.");
        if (pending.CurrentCandidateSeat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current FactionSlash responder is not the human seat.");
        }

        _coreNativeFactionSlashOperations.ResolveFactionSlashCandidateResponse(pending, useSlash, requestedCardId, requestedEffectiveKind);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanRespondCore(bool useDodge, bool useBagua, int? requestedDodgeCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.Nullification)
        {
            HumanNullificationCore(useDodge, requestedDodgeCardId, advanceToHumanBoundary);
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack })
        {
            HumanGroupResponseCore(useResponse: useDodge, useBagua: useBagua, requestedResponseCardId: requestedDodgeCardId, requestedResponseCardKind: requestedResponseCardKind, advanceToHumanBoundary: advanceToHumanBoundary);
            return;
        }

        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondDodge);
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("There is no Slash awaiting resolution.");
        var defender = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (useDodge && useBagua)
        {
            throw new InvalidOperationException("A response cannot use both Dodge and Bagua.");
        }

        if (useBagua && attack.IgnoresArmor)
        {
            throw new InvalidOperationException("The attacking weapon ignores armor, so Bagua cannot respond.");
        }

        if (useBagua && !_coreNativeEquipmentAndFactionQueriesOperations.HasBagua(defender))
        {
            throw new InvalidOperationException("The responding player has no Bagua equipment.");
        }

        var selectedDodge = useDodge ? _coreNativeResponseMaterialQueriesOperations.GetResponseCards(defender, CardKind.Dodge).FirstOrDefault(card => (!requestedDodgeCardId.HasValue || card.Id == requestedDodgeCardId.Value) && (!requestedResponseCardKind.HasValue || _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(defender, card, CardKind.Dodge) == requestedResponseCardKind.Value)) : null;
        if (useDodge && selectedDodge is null)
        {
            throw new InvalidOperationException("The responding player has no legal Dodge response card.");
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (useDodge)
        {
            _coreNativeDodgeOperations.ResolveDodgeResponse(attack, defender, selectedDodge!);
        }
        else if (useBagua)
        {
            var judgmentResult = _coreNativeJudgmentOperations.ResolveBaguaJudgment(attack, defender);
            if (judgmentResult is { } succeeded)
            {
                _coreNativeJudgmentOperations.CompleteBaguaResponse(attack, succeeded);
            }
        }
        else
        {
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanNullificationCore(bool useNullification, int? requestedNullificationCardId, bool advanceToHumanBoundary, CardConversionSource? selectedConversionSource = null)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.Nullification);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("There is no Nullification window.");
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("There is no Nullification prompt.");
        Card? selected = null;
        if (useNullification)
        {
            var cardId = requestedNullificationCardId ?? throw new InvalidOperationException("A Nullification response must name its published card.");
            selected = _coreNativeNullificationOperations.GetNullificationCards(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat]).SingleOrDefault(card => card.Id == cardId);
            if (selected is null || !decision.ValidCardIds.Contains(selected.Id))
            {
                throw new InvalidOperationException("The requested Nullification card is not in the published response choices.");
            }

            _coreNativeNullificationOperations.ValidateNullificationConversion(_coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat], selected, selectedConversionSource);
        }
        else if (requestedNullificationCardId is not null || selectedConversionSource is not null)
        {
            throw new InvalidOperationException("A passed Nullification response cannot name a card.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeNullificationOperations.ResolveNullificationChoice(pending, _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat], selected, selectedConversionSource);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanStoneAxeCore(IReadOnlyList<int> discardedCardIds, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.StoneAxe);
        _coreNativeStoneAxeOperations.Value.ResolveStoneAxeChoice(discardedCardIds);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanCixiongDoubleSwordsCore(PromptChoice selected, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.CixiongDoubleSwords);
        _coreNativeWeaponResponsesOperations.ResolveCixiongDoubleSwordsChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanQinglongCrescentBladeCore(PromptChoice selected, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.QinglongCrescentBlade);
        _coreNativeQinglongOperations.ResolveQinglongCrescentBladeChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanZhuqueFanCore(bool convertToFireSlash, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.ZhuqueFan);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("There is no FactionSlash Zhuque Fan continuation.");
        _coreNativeFactionSlashOperations.ResolveFactionSlashZhuqueFanChoice(pending, convertToFireSlash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanIceSwordCore(PromptChoice selected, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.IceSword);
        _coreNativeIceSwordOperations.ResolveIceSwordChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanQilinBowCore(PromptChoice selected, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.QilinBow);
        _coreNativeQilinBowOperations.ResolveQilinBowChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanDyingResponseCore(bool usePeach, int? requestedPeachCardId, bool useAlcohol, int? requestedAlcoholCardId, bool advanceToHumanBoundary, CardConversionSource? peachConversionSource = null, CardConversionSource? alcoholConversionSource = null)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RescueDying);
        var pending = _coreDyingCursorRuntimeOperations.Value.ActiveDying ?? throw new InvalidOperationException("There is no dying response awaiting resolution.");
        var responder = _coreMatchParticipantsPlayersAccess._players[pending.ResponderSeat];
        if (responder.Seat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current dying responder is not the human seat.");
        }

        if (usePeach && useAlcohol)
        {
            throw new InvalidOperationException("A dying response can use only one rescue source.");
        }

        if (usePeach)
        {
            var peach = _coreNativeDyingOperations.GetDyingPeaches(responder).FirstOrDefault(card => (!requestedPeachCardId.HasValue || card.Id == requestedPeachCardId.Value));
            if (peach is null)
            {
                throw new InvalidOperationException("The responding player has no requested Peach card.");
            }

            var sources = _coreNativeDyingOperations.GetDyingPeachConversionSources(responder, peach);
            if (peach.Kind == CardKind.Peach && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(responder, peach) && !_coreCardConversionsOperations.HasProgramCardIdentity(responder, peach) ? peachConversionSource is not null : peachConversionSource is null || !sources.Contains(peachConversionSource))
            {
                throw new InvalidOperationException("The Peach response has no matching published conversion source.");
            }
        }

        if (useAlcohol)
        {
            if (responder.Seat != pending.VictimSeat)
            {
                throw new InvalidOperationException("Alcohol can only rescue its dying holder.");
            }

            var alcohol = _coreNativeDyingOperations.GetDyingAlcohols(responder, pending.VictimSeat).FirstOrDefault(card => (!requestedAlcoholCardId.HasValue || card.Id == requestedAlcoholCardId.Value));
            if (alcohol is null)
            {
                throw new InvalidOperationException("The responding player has no requested Alcohol card.");
            }

            _coreProgramAlcoholConversionsOperations.Value.ValidateAlcoholConversion(responder, alcohol, alcoholConversionSource, forResponse: true);
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeDyingOperations.ApplyDyingResponse(responder, usePeach, requestedPeachCardId, useAlcohol, requestedAlcoholCardId, peachConversionSource, alcoholConversionSource);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanDyingProgramTriggerCore(PromptChoice selected, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RescueDying);
        var dying = _coreDyingCursorRuntimeOperations.Value.ActiveDying ?? throw new InvalidOperationException("There is no dying program response awaiting resolution.");
        var responder = _coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat];
        var candidate = _coreProgramLifecycleOperations.GetDyingProgramCandidates(responder, dying).SingleOrDefault(item => item.SkillId == selected.Parameters.GetValueOrDefault("skill-id") && item.BindingId == selected.Parameters.GetValueOrDefault("binding-id") && item.SkillInstanceId == selected.Parameters.GetValueOrDefault("skill-instance-id")) ?? throw new InvalidOperationException("The selected dying program binding is no longer eligible.");
        _coreProgramLifecycleOperations.BeginDyingProgramBinding(candidate, dying);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanHarvestCardCore(int cardId, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.SelectHarvestCard);
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("There is no FiveGrains draft awaiting a choice.");
        var picker = _coreMatchParticipantsPlayersAccess._players[group.TargetSeats[group.TargetIndex]];
        if (picker.Seat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current FiveGrains picker is not the human seat.");
        }

        if (!group.RevealedCardIds.Contains(cardId))
        {
            throw new InvalidOperationException("The selected card is not in the public FiveGrains reveal.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeHarvestOperations.Value.ResolveHarvestSelection(group, picker, cardId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanSkipDiscardPolicyCore(bool useSkill, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.SkipDiscardPolicy);
        _coreActualDiscardPhaseOperations.ResolveSkipDiscardPolicyChoice(useSkill);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanTargetCardSelectionCore(int slot, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.SelectTargetCard);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection ?? throw new InvalidOperationException("There is no hidden target-card selection awaiting a choice.");
        if (pending.SourceSeat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current hidden target-card source is not the human seat.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeTrickEffectsOperations.ResolveTargetCardSelection(pending, slot);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanFireAttackCardCore(int cardId, bool advanceToHumanBoundary)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFireAttack ?? throw new InvalidOperationException("There is no FireAttack selection awaiting a choice.");
        var decisionKind = pending.FireAttackSelection?.RevealedCardId is null ? DecisionKind.FireAttackReveal : DecisionKind.FireAttackDiscard;
        _coreNativePromptBoundaryOperations.RequireHumanDecision(decisionKind);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } decision || !decision.ValidCardIds.Contains(cardId))
        {
            throw new InvalidOperationException("The selected FireAttack card is not a legal choice.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (decisionKind == DecisionKind.FireAttackReveal)
        {
            _coreNativeFireAttackOperations.ResolveFireAttackReveal(pending, cardId);
        }
        else
        {
            _coreNativeFireAttackOperations.ResolveFireAttackDiscard(pending, cardId);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanFireAttackSkipCore(bool advanceToHumanBoundary)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFireAttack ?? throw new InvalidOperationException("There is no FireAttack discard selection awaiting a choice.");
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.FireAttackDiscard);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { } decision || !decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "fire-attack-skip"))
        {
            throw new InvalidOperationException("The current FireAttack prompt does not allow skipping.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeFireAttackOperations.ResolveFireAttackDiscard(pending, selectedCardId: null);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanSlashResponseCore(bool useSlash, int? requestedSlashCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null)
            HumanFactionSlashResponseCore(useSlash, requestedSlashCardId, requestedResponseCardKind, advanceToHumanBoundary);
        else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.Nullification)
            HumanNullificationCore(useNullification: false, requestedNullificationCardId: null, advanceToHumanBoundary);
        else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.FireAttackReveal)
            HumanFireAttackCardCore(_corePendingDecisionPolicyPipelineOperations._pendingDecision.ValidCardIds.First(), advanceToHumanBoundary);
        else if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.FireAttackDiscard)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision.ValidCardIds.Count > 0)
                HumanFireAttackCardCore(_corePendingDecisionPolicyPipelineOperations._pendingDecision.ValidCardIds.First(), advanceToHumanBoundary);
            else
                HumanFireAttackSkipCore(advanceToHumanBoundary);
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack })
            HumanGroupResponseCore(useResponse: useSlash, useBagua: false, requestedResponseCardId: requestedSlashCardId, requestedResponseCardKind: requestedResponseCardKind, advanceToHumanBoundary: advanceToHumanBoundary);
        else
            HumanDuelResponseCore(useSlash, requestedSlashCardId, requestedResponseCardKind, advanceToHumanBoundary);
    }

    internal void HumanZhangbaSlashResponseCore(IReadOnlyList<int> requestedCardIds, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var responder = _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat];
        var pair = _coreNativeResponseMaterialQueriesOperations.FindZhangbaSlashPair(responder, requestedCardIds) ?? throw new InvalidOperationException("The responding player has no matching Zhangba Slash pair.");
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } jijiang)
        {
            _coreNativeFactionSlashOperations.ResolveFactionSlashZhangbaCandidateResponse(jijiang, responder, pair);
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } borrowedSword)
        {
            _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordZhangbaSlashChoice(borrowedSword, pair);
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } group)
        {
            _coreNativeResponseWindowsOperations.PopResponseWindow(group.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeGroupResponsesOperations.ResolveGroupZhangbaResponse(group, responder, pair);
        }
        else
        {
            var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("There is no Slash response continuation.");
            _coreNativeResponseWindowsOperations.PopResponseWindow(duel.ResolutionId);
            _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeDuelOperations.ResolveDuelZhangbaResponse(duel, responder, pair);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanProgramViewAsSlashResponseCore(IReadOnlyList<int> requestedCardIds, CardConversionSource source, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var responder = _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat];
        var forResponse = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is null && _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not { IsProgramSkillUse: true } and not { IsBorrowedSwordUse: true } and not { IsQinglongCrescentBladeUse: true };
        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(responder, requestedCardIds, CardKind.Slash, forResponse, source) ?? throw new InvalidOperationException("The responding player has no matching configured Slash conversion.");
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { } jijiang)
        {
            _coreNativeFactionSlashOperations.ResolveFactionSlashProgramMultiCardCandidateResponse(jijiang, responder, selection);
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true } borrowedSword)
        {
            _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordProgramMultiCardSlashChoice(borrowedSword, selection);
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } group)
        {
            _coreNativeResponseWindowsOperations.PopResponseWindow(group.ResolutionId);
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreCardConversionsOperations.ResolveGroupProgramMultiCardResponse(group, responder, selection);
        }
        else
        {
            var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("There is no Slash response continuation.");
            _coreNativeResponseWindowsOperations.PopResponseWindow(duel.ResolutionId);
            _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreCardConversionsOperations.ResolveDuelProgramMultiCardResponse(duel, responder, selection);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanBorrowedSwordResponseCore(bool useSlash, int? requestedSlashCardId, CardKind? requestedEffectiveKind, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword ?? throw new InvalidOperationException("There is no Borrowed Sword awaiting a response.");
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        if (!pending.AwaitingSlashChoice || owner.Seat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current Borrowed Sword responder is not the human seat.");
        }

        var slash = useSlash ? _coreNativeBorrowedSwordOperations.GetBorrowedSwordSlashCards(owner, _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat]).SingleOrDefault(card => card.Id == requestedSlashCardId) : null;
        if (useSlash && slash is null)
        {
            throw new InvalidOperationException("The responding player has no matching legal Slash.");
        }

        if (slash is not null)
        {
            _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordSlashChoice(pending, slash, requestedEffectiveKind);
        }
        else
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(pending, transferWeapon: true);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanBorrowedSwordFactionSlashCore(bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword ?? throw new InvalidOperationException("There is no Borrowed Sword awaiting FactionSlash.");
        if (!pending.AwaitingSlashChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.All(choice => choice.Parameters.GetValueOrDefault("response") != "faction-slash-request") != false)
        {
            throw new InvalidOperationException("FactionSlash is not available in the current Borrowed Sword response.");
        }

        _coreNativeFactionSlashOperations.BeginBorrowedSwordFactionSlashRequest(pending);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanDuelResponseCore(bool useSlash, int? requestedSlashCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.RespondSlash);
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("There is no Duel awaiting a response.");
        var responder = _coreMatchParticipantsPlayersAccess._players[duel.ResponderSeat];
        if (responder.Seat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current Duel responder is not the human seat.");
        }

        var selectedSlash = useSlash ? _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, CardKind.Slash).FirstOrDefault(card => (!requestedSlashCardId.HasValue || card.Id == requestedSlashCardId.Value) && (!requestedResponseCardKind.HasValue || _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, CardKind.Slash) == requestedResponseCardKind.Value)) : null;
        if (useSlash && selectedSlash is null)
        {
            throw new InvalidOperationException("The responding player has no legal Slash response card.");
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(duel.ResolutionId);
        _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeDuelOperations.ResolveDuelResponse(duel, responder, selectedSlash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanGroupResponseCore(bool useResponse, bool useBagua, int? requestedResponseCardId, CardKind? requestedResponseCardKind, bool advanceToHumanBoundary)
    {
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("There is no group attack awaiting a response.");
        var requiredCardKind = group.RequiredCardKind ?? throw new InvalidOperationException("A group response attack must declare a required card kind.");
        var expectedDecision = requiredCardKind == CardKind.Dodge ? DecisionKind.RespondDodge : DecisionKind.RespondSlash;
        _coreNativePromptBoundaryOperations.RequireHumanDecision(expectedDecision);
        var attack = group.CurrentAttack ?? throw new InvalidOperationException("The group attack has no current target.");
        var responder = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (responder.Seat != _coreMultiplayerOperations.HumanDecisionSeat)
        {
            throw new InvalidOperationException("The current group responder is not the human seat.");
        }

        if (useResponse && useBagua)
        {
            throw new InvalidOperationException("A response cannot use both a physical card and Bagua.");
        }

        var canUseBagua = requiredCardKind == CardKind.Dodge && !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(responder);
        if (useBagua && !canUseBagua)
        {
            throw new InvalidOperationException("Bagua is not available for this group response under the active rules version.");
        }

        var selectedResponse = useResponse ? _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, requiredCardKind).FirstOrDefault(card => (!requestedResponseCardId.HasValue || card.Id == requestedResponseCardId.Value) && (!requestedResponseCardKind.HasValue || _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, requiredCardKind) == requestedResponseCardKind.Value)) : null;
        if (useResponse && selectedResponse is null)
        {
            throw new InvalidOperationException($"The responding player has no legal {(requiredCardKind)} response card.");
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(group.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (useBagua)
        {
            var judgmentResult = _coreNativeJudgmentOperations.ResolveBaguaJudgment(attack, responder);
            if (judgmentResult is { } succeeded)
            {
                _coreNativeJudgmentOperations.CompleteBaguaResponse(attack, succeeded);
            }
        }
        else
        {
            _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, responder, selectedResponse);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void HumanSelectGodFactionCore(string factionId, bool advanceToHumanBoundary)
    {
        _coreNativePromptBoundaryOperations.RequireHumanDecision(DecisionKind.SelectFaction);
        var pending = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("There is no god-faction selection prompt.");
        _coreMatchParticipantSetupOperations.ApplyGodFactionSelection(_coreMatchParticipantsPlayersAccess._players[pending.PlayerSeat], factionId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }
}
