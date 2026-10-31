#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardActionsRuntime
{
    internal bool TryBeginFinalizedTrickPrograms(NativeOwners.JizhiResolution pending)
    {
        if (!_coreFinalizedCardTargetsOperations.HasFinalizedTargetProgram(pending.EffectiveCardKind) || !_coreCardUseLifecycleOperations.TryMarkFinalizedTrickProgramsStarted(pending.ResolutionId))
            return false;
        var use = _coreCurrentUseTargetCandidateRefreshOperations.NormalizeImplicitDrawTwoUseTarget(_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == pending.ResolutionId));
        var action = use.Action!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseTargetsFinalized, use.TargetSeats, ProgramCardContinuation.FinalizedTrick, trickContinuation: new(pending.Card.Id, pending.ActionKind, pending.TargetCardId, pending.RequiredCardKind));
    }

    internal CommandResult SubmitRecast(RecastCardCommand command)
    {
        var validation = _coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction);
        if (validation is not null)
            return _kernelCoreMainOperations2.Reject(validation.Code, validation.Message);
        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        if (_coreNativeOwnedCardQueriesOperations.FindOwnedPlayableCard(actor, command.CardId)is null)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.InvalidCard, "The recast card is not in the actor's playable zones.");
        var action = _coreNativeLegalActionsOperations.BuildLegalActions(actor, selectedPhysicalCardId: command.CardId).SingleOrDefault(action => action.Kind == LegalActionKind.Recast && action.CardId == command.CardId && action.ConversionSource == command.ConversionSource);
        if (action is null)
            return _kernelCoreMainOperations2.Reject(CommandErrorCode.IllegalAction, "This card cannot be recast under the current rules.");
        return _kernelCoreMainOperations2.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeActionDispatchOperations.ExecuteAction(actor, action);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
    }

    internal void ResolveRecast(CharacterState actor, Card card, CardKind? playedCardKind = null)
    {
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice = false;
        if (playedCardKind is null && _modeRulesOperations._modeRules.Cards.AllowsNativePhysicalRecast(card.Kind, card.Kind))
        {
            _coreEquipmentRecastsOperations.BeginEquipmentRecast(actor, card, card.Kind);
            return;
        }

        if (playedCardKind is null && (_coreModeCardRecastsOperations.CanModeHandEquipmentRecast(card) || _coreHellDizangProgramsOperations.CanHellDitingMountRecast(actor, card)) && _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card) == CardLocation.Hand(actor.Seat))
        {
            _coreEquipmentRecastsOperations.BeginEquipmentRecast(actor, card, card.Kind);
            return;
        }

        if ((playedCardKind is null && card.Kind != CardKind.IronChain) || (playedCardKind is not null && playedCardKind != CardKind.IronChain))
            throw new InvalidOperationException("Only Iron Chain may be recast.");
        if (_coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card).Zone == CardZoneKind.Equipment)
        {
            _coreEquipmentRecastsOperations.BeginEquipmentRecast(actor, card, playedCardKind ?? card.Kind);
            return;
        }

        var recast = _coreNativeRecastInvocationsOperations.CaptureNativeRecastInvocation(actor.Seat, actor.Seat, [card.Id]);
        _corePhysicalMovementFacadesOperations.MoveCards([card], _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(actor, card), CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(actor, 1, log: false, reason: CardMoveReasons.RecastDraw);
        if (_coreAiBrainsAiBrainsAccess._aiBrains.TryGetValue(actor.Seat, out var brain))
            brain.ObserveRecast(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, card.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(actor.Seat, card.Id, playedCardKind ?? card.Kind, drawn.Count));
        _coreNativeRecastInvocationsOperations.CompleteNativeRecastInvocation(recast, drawn.Count);
        _coreMatchLogOperations.AddLog("Recast", $"{(actor.Name)} 重铸【铁索连环】，摸 {(drawn.Count)} 张牌。", actor.Seat);
    }

    internal NativeOwners.CardAttackHandle? ProgramCardAttack => _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(frame => frame.AttackOwnerFrameId is not null)?.AttackOwnerFrameId is { } id ? _kernelCoreMainOperations.CreateCardAttackHandle(id) : null;

    internal IReadOnlyList<CardAttackState>? PreparedTargetAttacks(long id) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == id).PreparedTargetAttacks;
    internal int GetFangtianEffectiveTarget(NativeOwners.FangtianHalberdHandle pending, int index) => (_coreCardUseLifecycleOperations.LifecycleCardUse(pending.ResolutionId)?.ProgramUseAccepted == true) && PreparedTargetAttacks(pending.ResolutionId)is { } prepared ? prepared[index].TargetSeat : pending.TargetSeats[index];
    internal CardActionContext? CaptureCardUseAction(Card card, int actorSeat, IReadOnlyList<int> targets, CardKind effectiveKind, IReadOnlyList<int> physicalIds, CardConversionSource? explicitConversion = null, IReadOnlyList<CardConversionSource>? additionalConversions = null, IReadOnlyList<int>? designatedTargetSeats = null, bool isTrueZhangbaSlash = false)
    {
        var costs = physicalIds.Select(id => new CardActionCost(id, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(item => item.Id == id).Kind, _coreCardDeclarationsOperations.CapturedDeclarationOrigin(id, _cardZonesOperations._cardZones.GetLocation(id)), _coreActionDiscardColorsOperations.CapturePhysicalCardColor(actorSeat, _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id)))).ToArray();
        var provider = costs.FirstOrDefault()?.From.OwnerSeat ?? actorSeat;
        var selectedFieldConversion = explicitConversion ?? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        if (selectedFieldConversion is { SkillId: ShenMaChaoComposition.Shouli } fieldSource && physicalIds.Count == 1 && physicalIds[0] == card.Id && _coreTableSeatQueriesOperations.IsValidPlayerSeat(fieldSource.OwnerSeat) && _characterMaChaoAnniversaryGodOperations.IsAcceptedShouliMaterial(_coreMatchParticipantsPlayersAccess._players[fieldSource.OwnerSeat], card, fieldSource, false))
            provider = fieldSource.OwnerSeat;
        CardConversionSource? conversion;
        if (explicitConversion is not null)
        {
            _coreLastZoneConversionsOperations.ValidateLastZoneConversionCommit(explicitConversion, _coreMatchParticipantsPlayersAccess._players[provider], physicalIds.Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id)).ToArray(), forResponse: false);
            var selectedConversion = _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion = null;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion = null;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedUseConversionChoice = false;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceHasSelectedUseConversionChoiceSelectedResponseConversionSelectedUseConversionAccess._hasSelectedResponseConversionChoice = false;
            if (selectedConversion is not null && selectedConversion != explicitConversion)
            {
                throw new InvalidOperationException("The explicit card-use conversion no longer matches the selected action.");
            }

            conversion = explicitConversion;
        }
        else
        {
            conversion = physicalIds.Count == 1 ? _coreCardConversionsOperations.GetSelectedUseConversion(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[provider], card, effectiveKind) : null;
        }

        _coreOwnPublicPileConversionsOperations.ValidateOwnPublicPileConversionCommit(conversion, _coreMatchParticipantsPlayersAccess._players[provider], physicalIds.Select(id => _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id)).ToArray(), forResponse: false, effectiveKind: effectiveKind);
        var conversionChain = new List<CardConversionSource>();
        if (conversion is not null)
            conversionChain.Add(conversion);
        if (additionalConversions is not null)
        {
            if (additionalConversions.Any(source => source is null) || additionalConversions.Distinct().Count() != additionalConversions.Count || conversion is not null && additionalConversions.Contains(conversion))
            {
                throw new InvalidOperationException("A card-use conversion chain must contain distinct sources.");
            }

            conversionChain.AddRange(additionalConversions);
        }

        _coreProgramCardLegalityOperations.CardLegality.ValidateSubmittedUse(new(actorSeat, effectiveKind, physicalIds, conversionChain));
        _corePlayCardHistoryOperations.ConsumeProgramViewAsUsage(conversionChain);
        var trackAppearance = _characterGuanYuHappyGodOperations.HasHappyGuanYuHeartSlashCapability && CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveKind) || _characterMaChaoAnniversaryGodOperations.HasHengwuCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.RequiresFact(ProgramTrackedFact.NativeActionAppearance) || _characterLuBuOlMagicOperations.HasMoLuBu || _characterHuangfuSongWumiaoOperations.HasWuHuangFuSongCapability || _characterLuKangWumiaoOperations.HasWuLuKangDixianCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouHuaXiongYaowu) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMouGongsunZanJiaodi) || _characterZhangFeiWumiaoOperations.HasWuZhangFeiUseCapability || _characterZhouYuOlMouOperations.HasMouZuifengCapability || _characterLuXunWumiaoOperations.HasWuLuXunZhangcaiCapability || _characterZouShiLeOperations.HasLeZouShiCapability || _characterGuanYuWumiaoOperations.HasWuyouCapability || _characterGuanYuWumiaoOperations.HasWuGuanYuJuewuCapability || _characterMiHengLeOperations.HasSiruiCapability || _characterLiFengOlOperations.HasLiFengCapability || _characterCuiYanOlOperations.HasCuiYanCapability || conversionChain.Any(source => _corePlayCardHistoryOperations.ViewAsRule(source)?.SourcePileSkillId is not null) || _characterLiuYanOlOperations.HasLiuYanCapability || _characterQuHuangOlOperations.HasQuHuangCapability || _characterRuanYuOlOperations.HasRuanYuCapability || _characterLiuPanOlOperations.HasLiuPanCapability || _characterWuJingOlOperations.HasWuJingLiubing || _characterWuJingOlOperations.HasWuJingHeji || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveKenshangTargetRewrite) || _characterMaMiDiOlOperations.HasMaMiDiCapability || _characterWuAnGuoOlOperations.HasWuAnGuoCapability || _characterFeiYiOlOperations.HasFeiYiHezhong || _characterBianFuRenOlOperations.HasBianFuRenCapability || _coreDamageTargetBudgetOperations.TracksDamageTargetBudgetTargets || _coreAdjacentRedTargetsOperations.HasAdjacentRedTargetCapability || _coreSoleTargetProtectionOperations.HasSoleTargetProtectionCapability || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.RevealRecordedSuitSlash) || effectiveKind is CardKind.UnexpectedAssault or CardKind.WumiaoFlood or CardKind.QizhengXiangsheng || (_coreUniqueLeaderTrickTargetsOperations.TracksUniqueLeaderTrickTargets || _coreRoundGainedSourceUsesOperations.TracksRoundGainedSourceUses || _coreTargetedOrdinaryTrickOperations.HasTargetedOrdinaryTrickAdjustment) && CoreNativeCardClassificationsRuntime.IsOrdinaryTrick(effectiveKind) || _coreSourceCursesOperations.HasSourceCurseActionColors || _coreEndingHistoricalUsesOperations.TracksOwnPlayHistory || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferOriginalTargetAddition) || _coreDyingWindowSkillPoliciesOperations.HasBlackTrickTargetPolicy || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.DrawByDamageCardColor) || _coreCurrentTurnShownCardBenefitsOperations.HasShownCardTurnCapability || _coreTurnRedSlashBenefitsOperations.HasTurnRedSlashCapability || _corePlayCardHistoryOperations.TracksPlayCardHistory || _coreCurrentTurnUseKindsOperations.TracksCurrentTurnUseKinds || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.AlternatingSuitDrawDiscard) || _corePaidColorConversionsOperations.TracksPaidColorUseAppearance(actorSeat, effectiveKind, conversionChain) || _coreSameTypeActualUseAidOperations.HasSameTypeActualUseAid && CoreGameEngineSameTypeActualUseAidRuntime.IsSameTypeAidEffectiveCard(effectiveKind) || (_coreComparedBlackSlashPoliciesOperations.HasComparedBlackSlashPolicies || _coreDirectedCardPolicyQueriesOperations.HasYuanchouCardPolicy) && CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveKind);
        var usedSuit = _characterWuJingOlOperations.WuJingLiubingUsedSuit(actorSeat, card, effectiveKind, physicalIds, conversionChain, trackAppearance ? _corePlayCardHistoryOperations.CaptureUsedCardSuit(actorSeat, physicalIds, card) : null);
        if (_characterCuiYanOlOperations.IsCuiYanNextUseForbidden(actorSeat, physicalIds, usedSuit))
            throw new InvalidOperationException("The native use violates its issued next-use suit restriction.");
        var captured = _characterHuangfuSongWumiaoOperations.CaptureWuHfUseAction(_characterZhangFeiWumiaoOperations.CaptureWuZhangFeiUseRankAction(_coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actorSeat, provider, provider == actorSeat ? null : actorSeat, null, null, effectiveKind, targets, costs, conversionChain, (designatedTargetSeats ?? targets), effectiveSuit: usedSuit, effectiveRank: _characterLuKangWumiaoOperations.CaptureWuLuKangDixianDeclarationRank(provider, physicalIds) ?? _characterGuanYuWumiaoOperations.CaptureJuewuActionRank(provider, physicalIds) ?? _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholActionRank(provider, effectiveKind, physicalIds) ?? (_characterZhouQunOlOperations.HasZhouQunCapability && CoreNativeCardClassificationsRuntime.IsSlashCard(effectiveKind) && physicalIds.Count == 1 ? card.Rank : (int? )null) ?? (_coreFinalTargetSlashOperations.HasRankSlashRange(_coreMatchParticipantsPlayersAccess._players[actorSeat], effectiveKind) || _characterHuangfuSongWumiaoOperations.HasWuHuangFuSongCapability ? physicalIds.Count == 1 && card.Rank > 0 ? card.Rank : isTrueZhangbaSlash && physicalIds.Count == 2 && conversionChain.Count == 0 ? _coreFinalTargetSlashOperations.ZhangbaSpecificSlashRank(_coreMatchParticipantsPlayersAccess._players[actorSeat], physicalIds.Select(id => _coreCardAttackStateOperations.GetAttackCard(id)).ToArray(), _coreMatchParticipantsPlayersAccess._players[provider]) : null : trackAppearance && physicalIds.Count == 1 && card.Rank > 0 ? card.Rank : null), effectiveIsRed: _coreActionDiscardColorsOperations.CaptureActionColor(costs, usedSuit)))));
        return _characterZhaoYunOlGod2016Operations.CaptureHpScaledCardUseAction(captured);
    }

    internal bool TryBeginCardResponsePrograms(NativeOwners.CardAttackHandle attack, CharacterState actor, CharacterState provider, int? requesterSeat, int opponentSeat, CardKind effectiveKind, IReadOnlyList<CardActionCost> costs, ProgramCardContinuation continuation, CardConversionSource? conversionSource = null, Suit? completedResponseUseSuit = null)
    {
        var parent = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Id == attack.ResolutionId);
        var pileMaterial = _coreOwnPublicPileConversionsOperations.OwnPublicPileResponseMaterial(costs, conversionSource);
        completedResponseUseSuit ??= pileMaterial is null ? null : _coreNativeCardAppearanceOperations.EffectiveSuit(provider, pileMaterial);
        if (completedResponseUseSuit is null && _characterMaChaoAnniversaryGodOperations.HasHengwuCapability && costs.Count > 0)
        {
            var suits = costs.Select(cost => _coreNativeCardAppearanceOperations.EffectiveSuit(provider, _coreCardAttackStateOperations.GetAttackCard(cost.CardId))).Distinct().ToArray();
            completedResponseUseSuit = suits.Length == 1 ? suits[0] : null;
        }

        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, parent?.Action?.ActionId, CardActionType.Response, actor.Seat, provider.Seat, requesterSeat, actor.Seat, opponentSeat, effectiveKind, [], costs, conversionSource is null ? [] : [conversionSource], effectiveSuit: completedResponseUseSuit, effectiveRank: _characterGuanYuWumiaoOperations.CaptureJuewuResponseRank(provider, costs) ?? _characterMiHengLeOperations.SiruiResponseRank(conversionSource, costs) ?? _coreAlcoholIdentityPoliciesOperations.CaptureAlcoholResponseRank(provider, effectiveKind, costs) ?? _characterLuXunWumiaoOperations.CaptureWuLuXunResponseRank(provider, effectiveKind, costs, conversionSource) ?? _characterZhangFeiWumiaoOperations.CaptureWuZhangFeiResponseRank(provider, effectiveKind, costs, conversionSource) ?? (pileMaterial is { Rank: > 0 } ? pileMaterial.Rank : null), effectiveIsRed: _coreActionDiscardColorsOperations.CaptureActionColor(costs, completedResponseUseSuit)));
        _characterZhangFeiWumiaoOperations.BindWuZhangFeiResponseRanks(action);
        if (parent is not null)
        {
            _characterMiHengLeOperations.BindSiruiResponseAction(parent, action);
            parent = _coreCardUseLifecycleOperations.LifecycleCardUse(parent.Id);
        }

        if (parent is not null && actor.Seat == provider.Seat && requesterSeat is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, effectiveKind))
            _coreTieredRoundZeroResponsesOperations.TryIssueTieredRoundResponse(parent, action);
        if (actor.Seat == provider.Seat && requesterSeat is null && _coreAlternativeViewAsCostsOperations.IsProgramResponseCardUse(actor, effectiveKind))
        {
            _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, effectiveKind);
            _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        _coreGuanduChariotResponsesOperations.CaptureGuanduChariotResponsePayments(action, attack.ResolutionId, continuation);
        _coreFriendEventsOperations.CaptureFriendMinimumHandUse(action);
        _coreOffTurnUsedCardsOperations.CaptureOffTurnResponseUseOrigin(attack.ResolutionId, action);
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(attack, action, continuation))
            return true;
        if (TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardResponseAccepted, [opponentSeat], continuation))
            return true;
        if (!_coreResponseUseCompletionOperations.HasResponseUseCompletionObserver(action, continuation) && !_coreResponseCompletionOperations.HasCardResponseCompletedObserver(action, continuation) && !_characterHuangZhongAnniversaryGodOperations.HasAnniversaryHzIneffectiveResponse(attack, action, continuation) && !_characterWeiYanHappyGodOperations.HasHappyWeiYanIneffectiveResponse(attack, action, continuation))
            return false;
        _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, action, continuation);
        return true;
    }

    internal bool PrepareProgramSlashTargets(NativeOwners.CardAttackHandle attack)
    {
        if ((_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.ProgramUseAccepted == true))
            return false;
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == attack.ResolutionId);
        if (frame.Action is not { } captured)
            return false;
        if (_coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd is { } multi && multi.ResolutionId == attack.ResolutionId)
        {
            var prepared = (frame.PreparedTargetAttacks ?? []).Append(_coreCardAttackStateOperations.GetCardAttackState(attack.ResolutionId)).ToArray();
            var ownerIndex = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[ownerIndex].Id, frame with { PreparedTargetAttacks = Array.AsReadOnly(prepared) });
            if (prepared.Length < multi.TargetSeats.Count)
            {
                multi.TargetIndex++;
                _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(multi);
                return true;
            }

            multi.TargetIndex = 0;
            _coreCardAttackStateOperations.UpdateCardAttackState(attack.ResolutionId, _ => prepared[0]);
            multi.CurrentAttack = attack;
            _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(multi.ResolutionId, 0);
            frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == attack.ResolutionId);
        }

        var finalized = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(captured.ActionId, captured.ParentActionId, captured.Type, captured.ActorSeat, captured.ProviderSeat, captured.RequesterSeat, captured.ResponderSeat, captured.OpponentSeat, captured.EffectiveKind, frame.TargetSeats, captured.PhysicalCards, captured.ConversionChain, (frame.TargetSeats.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Distinct().ToArray()), captured.EffectiveSuit, captured.EffectiveRank, captured.EffectiveIsRed, captured.FactionOrigin));
        var index = _resolutionStackOperations._resolutionStack.FindLastIndex(item => item.Id == frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeFrame(_resolutionStackOperations._resolutionStack[index].Id, frame with { Action = finalized, ProgramUseAccepted = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(finalized));
        if (TryBeginProgramCardWindow(attack, finalized, SkillProgramTriggerWindow.CardUseTargetsFinalized, finalized.TargetSeats, ProgramCardContinuation.Slash))
            return true;
        if (PreparedTargetAttacks(attack.ResolutionId)is not null)
        {
            _coreNativeSlashUseOperations.ContinueSlashAfterFinalizedTargets(attack);
            return true;
        }

        return false;
    }

    internal bool TryBeginProgramCardWindow(NativeOwners.CardAttackHandle? attack, CardActionContext action, SkillProgramTriggerWindow window, IReadOnlyList<int> opponents, ProgramCardContinuation? continuation, bool? cardUseCausedDamage = null, ProgramTrickContinuation? trickContinuation = null, ProgramSimpleCardContinuation? simpleContinuation = null, ProgramCompletedResponseReturn? completedResponseReturn = null, CaoChunHandUsePayment? caoChunPayment = null, bool caoChunDebtOnly = false)
    {
        if (window == SkillProgramTriggerWindow.CardUseCommitted && attack is not null && _characterSimaYiMouOperations.TryBeginMouSmPrivateWindow(attack, MouSmPrivateCall.CommittedSlash))
            return true;
        if ((continuation is null) != (completedResponseReturn is not null) && !_characterSimaYiMouOperations.IsMouSmPlayedWindow(action, window) && !_characterLuSuMouOperations.IsMouLuSuPlayedWindow(action, window))
            throw new InvalidOperationException("A card trigger window needs exactly one return continuation.");
        _modeRuntimeOperations._modeRuntime.OnCardAction(action, window);
        if (window == SkillProgramTriggerWindow.CardUseCommitted && _characterLiuZanMobileOperations.TracksAlternateActualUseColor)
            _characterLiuZanMobileOperations.RecordAlternateActualUseColor(action);
        if (window is SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardResponseAccepted && action.ConversionChain.Count != 0)
        {
            _coreProgramCardUsePaymentsOperations.CardUsePayments.PayAcceptedConversionCosts(action);
        }

        if (window == SkillProgramTriggerWindow.CardUseCommitted)
            _coreShanheLeizhenOperations.RecordShanheLeizhenCommittedUse(action);
        if ((_characterWangYunOlOperations.HasWangYunPlanCapability || _characterWangYunOlOperations.HasWangYunTrickCapability) && window is SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCommitted)
        {
            for (var nativeIndex = _resolutionStackOperations._resolutionStack.Count - 1; nativeIndex >= 0; nativeIndex--)
                if (_resolutionStackOperations._resolutionStack[nativeIndex] is CardUseFrame { WangYunNativeOrigin: not null } nativeUse && nativeUse.Action?.ActionId == action.ActionId)
                {
                    _characterWangYunOlOperations.CompleteWangYunNativePayment(nativeUse.Id);
                    break;
                }
        }

        if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _coreDragonBoatActivitiesOperations.IsDragonBoatActivity)
        {
            var nativeUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Action?.ActionId == action.ActionId);
            _coreDragonBoatGeneralsOperations.CaptureDragonBoatFinalizedUse(nativeUse.Id, action);
        }

        if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _characterSuFeiOlOperations.HasSuFeiCapability)
        {
            var nativeUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Action?.ActionId == action.ActionId);
            _characterSuFeiOlOperations.CaptureSuFeiFinalizedUse(nativeUse.Id, action);
        }

        if (_characterGuoSiOlOperations.HasGuoSiCapability && action.Type == CardActionType.Use && window is SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCompleted)
        {
            var nativeUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Action?.ActionId == action.ActionId);
            if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized)
                _characterGuoSiOlOperations.CaptureGuoSiFinalizedUse(nativeUse.Id, action);
            else
                _characterGuoSiOlOperations.CaptureGuoSiCompletedUse(nativeUse.Id, action);
        }

        if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _characterYuanTanYuanShangOlOperations.HasNeifaCapability)
        {
            var nativeUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Action?.ActionId == action.ActionId);
            _characterYuanTanYuanShangOlOperations.CaptureNeifaPaidEquipmentUse(nativeUse.Id, action);
        }

        if (window == SkillProgramTriggerWindow.CardUseTargetsFinalized && _characterHuaManOlOperations.HasHuaManCapability && action.Type == CardActionType.Use)
        {
            var nativeUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Action?.ActionId == action.ActionId);
            _characterHuaManOlOperations.CaptureHuaManFinalizedUse(nativeUse.Id, action);
        }

        if (window == SkillProgramTriggerWindow.CardUseCompleted && _characterHuaManOlOperations.HasHuaManCapability && action.Type == CardActionType.Use && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ActionId)is { } huaManCompleted)
            _characterHuaManOlOperations.CaptureHuaManCompletedUse(huaManCompleted.Id, action);
        if (_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveQigongActualDodgedSlashRequest) && window is SkillProgramTriggerWindow.CardUseTargetsFinalized or SkillProgramTriggerWindow.CardUseCommitted && _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ActionId)is { Qigong: not null } qigongUse)
            _characterLvKuangLvXiangOlOperations.CompleteQigongNativePayment(qigongUse.Id);
        _coreQuotaTopProgramsOperations.RegisterFirstTurnTargetActions(action, window);
        _coreUniqueLeaderTrickTargetsOperations.CaptureUniqueLeaderTrickQualification(action, window);
        var candidates = caoChunDebtOnly ? new List<ProgramCardTriggerCandidate>() : CollectSharedCardActionCandidates(action, window, opponents, cardUseCausedDamage, continuation).ToList();
        if (window == SkillProgramTriggerWindow.CardUseBeforeTargetEffects && (caoChunPayment ?? (action.Type == CardActionType.Use ? _characterCaoChunOlOperations.CaoChunPaidUseQualification(action) : null))is { } debt && _characterCaoChunOlOperations.CaoChunDebtCandidate(debt)is { } issued)
            candidates.Add(new(issued.OwnerSeat, -1, issued.SkillId, issued.BindingId, issued.GameplayHash, issued.SkillInstanceId, issued.Priority, _characterCaoChunOlOperations.CaoChunDebtContext(debt, 0)));
        candidates = candidates.OrderByDescending(candidate => candidate.Priority).ThenBy(candidate => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, candidate.OwnerSeat)).ThenBy(candidate => candidate.SkillId, StringComparer.Ordinal).ThenBy(candidate => candidate.SkillInstanceId, StringComparer.Ordinal).ThenBy(candidate => candidate.TriggerId, StringComparer.Ordinal).ToList();
        _coreActorHandLimitTargetProgramsOperations.RecordActorHandLimitTargetAnnouncements(action, window);
        if (candidates.Count == 0 && !_characterHuaTuoAnniversaryGodOperations.HasConvertedSkillInvocations(action, window))
            return false;
        _coreProgramFactionActionRewardOperations.RecordFactionRewardOffers(action, candidates);
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        candidates = candidates.Select(candidate => candidate.FrozenContext is { } frozen ? candidate with { FrozenContext = frozen with { ParentFrameId = frameId, CardUse = frozen.CardUse!with { ParentCardUseFrameId = _resolutionStackOperations._resolutionStack[^1].Id } } } : candidate).ToList();
        var frame = new ProgramCardTriggerWindowFrame(frameId, _resolutionStackOperations._resolutionStack[^1].Id, action, continuation, Array.AsReadOnly(candidates.ToArray()), TrickContinuation: trickContinuation, SimpleContinuation: simpleContinuation, CompletedResponseReturn: completedResponseReturn, AttackOwnerFrameId: attack?.ResolutionId)
        {
            CaoChunDebtBeforeEffects = caoChunDebtOnly ? true : null,
            MouLuSuAnniversaryPlayed = _characterLuSuMouOperations.MouLuSuPlayedWindowReturn(action, window),
            MouSimaYiPlayed = _characterSimaYiMouOperations.MouSmPlayedWindowReturn(action, window)
        };
        _coreRuntimeOperations.PushRuntimeFrame(frame);
        _characterSunQuanOlMagicOperations.CaptureMoQuanyuSingleton(frame, window);
        if (_characterGuoSiOlOperations.HasGuoSiCapability && window == SkillProgramTriggerWindow.CardUseCompleted && completedResponseReturn is not null)
            _characterGuoSiOlOperations.CaptureGuoSiCompletedUse(frame.Id, action);
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramCardTriggerWindowFrame>();
        return true;
    }

    internal bool TryBeginProgramCardUseBeforeTargetEffects(NativeOwners.CardAttackHandle attack)
    {
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == attack.ResolutionId);
        var action = frame.Action ?? throw new InvalidOperationException("A before-target-effects program window requires the frozen card action.");
        return TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseBeforeTargetEffects, [attack.TargetSeat], ProgramCardContinuation.BeforeTargetEffects);
    }

    internal bool TryBeginProgramSlashStage(NativeOwners.CardAttackHandle attack, SkillProgramTriggerWindow window, ProgramCardContinuation continuation)
    {
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack))
            throw new InvalidOperationException("The Slash program stage lost its active attack.");
        // Legacy and virtual Slash flows can reach the Dodge boundary without a
        // frozen card action. They have no card-action facts for program triggers.
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == attack.ResolutionId).Action;
        if (action is null)
            return false;
        return TryBeginProgramCardWindow(attack, action, window, [attack.TargetSeat], continuation);
    }

    internal bool TryBeginProgramCardUseBeforeTargetEffects(NativeOwners.JizhiResolution pending)
    {
        if (_coreActualUseTargetProgramsOperations.TryBeginActualUseTargetPrograms2(pending))
            return true;
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == pending.ResolutionId);
        var action = frame.Action ?? throw new InvalidOperationException("A trick before-target-effects program window requires the frozen card action.");
        return TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardUseBeforeTargetEffects, frame.TargetSeats, ProgramCardContinuation.BeforeTrickTargetEffects, trickContinuation: new(pending.Card.Id, pending.ActionKind, pending.TargetCardId, pending.RequiredCardKind));
    }

    internal IReadOnlyList<ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(CardActionContext action, SkillProgramTriggerWindow window, IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage, ProgramCardContinuation? continuation = null)
    {
        _characterLuXunWumiaoOperations.CaptureWuLuXunZhangcai(action, window);
        _characterZhangFeiWumiaoOperations.CaptureWuZhangFeiUseQualification(action, window);
        _characterCaiYongLeOperations.CaptureLeCaiYongFeibaiQualification(action, window);
        _characterCaoZhiLeOperations.CaptureLeCaoZhiWenlanQualification(action, window);
        _characterLiuChengMouMobileOperations.CaptureMouLiuChengCompletion(action, window);
        _characterLiuChengMouMobileOperations.CaptureMouLiuChengAccrual(action, window);
        _characterYuanShaoMouMobileOperations.CaptureMouYuanShaoDesignation(action, window);
        var targets = eventTargets.Distinct().ToHashSet();
        var result = new List<ProgramCardTriggerCandidate>();
        var capturedOwnerFacts = new SkillProgramTriggerFacts? [_coreMatchParticipantsPlayersAccess._players.Count];
        foreach (var owner in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).OrderBy(player => _coreTableOrderOperations.RelativeTableOrder(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, player.Seat)))
            foreach (var binding in _coreNationalSkillsOperations.GetSkillBindingShard(owner)?.GetInstanceTriggers(window) ?? [])
            {
                var trigger = binding.Trigger;
                // Freeze the applicable branch before any same-window skill can
                // commit the opposite polarity. Acceptance checks it again.
                if (trigger.RequiredPolarity is { } polarity && _coreProgramConversionPolarityOperations.GetProgramConversionPolarity(owner.Seat, binding.SkillId) != polarity)
                    continue;
                if (trigger.Effects.Any(e => e.Op is SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan or SkillProgramEffectOp.SelectMouZhuRongLierenTarget))
                    continue;
                if (trigger.Effects.Any(e => e.Op is SkillProgramEffectOp.CollectJiyunCompletedUseEntities or SkillProgramEffectOp.ResolveLeiluanLastHandUseBenefit or SkillProgramEffectOp.ResolveChenglieRedHolderOutcome or SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse))
                    continue;
                if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ResolveHuiyunCompletedFireAttack) || ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.ResolvePijingNextSoleTargetChoice))
                    continue;
                if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.PayShanjiaHandUseDebt) || trigger.Effects.Any(effect => effect.Op is SkillProgramEffectOp.ResolveBingjieIssuedTargetDiscards or SkillProgramEffectOp.RewardHejiUnconvertedUse))
                    continue;
                if (!_coreFirstSlashDuelBenefitsOperations.CanCollectFirstSlashDuelCandidate(owner.Seat, trigger, action))
                    continue;
                if (trigger.RequireNoCardConversion == true && !_coreUnconvertedCardUseQualificationOperations.IsUnconvertedActualCardUse(action, window, continuation))
                    continue;
                var reward = trigger.Effects.Any(CoreProgramFactionActionRewardRuntime.IsFactionActionReward);
                var rewardChooser = reward ? _coreProgramFactionActionRewardOperations.FactionActionRewardChooser(owner.Seat, trigger, action) : null;
                if (reward && (rewardChooser is null || _coreProgramFactionActionRewardOperations.WasFactionRewardOffered(owner.Seat, binding.SkillId, action) || !_coreLordSkillProjectionsOperations.HasSkillRoleQualification(owner, binding.SkillId, binding.SkillInstanceId, Role.Lord)))
                    continue;
                if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.DiscardNonFinalTargetCardThenDraw) && result.Any(candidate => candidate.OwnerSeat == owner.Seat && candidate.SkillId == binding.SkillId && candidate.TriggerId == trigger.Id))
                    continue;
                if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.FirstCategoryCompletedTop) && (!_coreAlternatingSuitAndCompletedTopOperations.IsFirstCategoryUse(owner.Seat, binding.SkillId, action) || _coreAlternatingSuitAndCompletedTopOperations.AvailableCompletedTopCosts(action, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(f => f.Action?.ActionId == action.ActionId)?.Id ?? _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow?.Id ?? 0).Length == 0))
                    continue;
                if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.IssueCardNoResponseAndPlayUseBan) && (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || action.ActorSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || owner.Seat != action.ActorSeat || !CoreGameEngineIssuedPlayUsePoliciesRuntime.IsEligibleIssuedNoResponseCard(action.EffectiveKind) || _coreIssuedPlayUsePoliciesOperations.HasIssuedPolicyForAction(action.ActionId)))
                    continue;
                if (window is (SkillProgramTriggerWindow.CardUseCompleted or SkillProgramTriggerWindow.CardUseCommitted) && action.Type == CardActionType.Response && !trigger.IncludeResponseUses)
                    continue;
                if (trigger.OwnerRelation is not { } relation || trigger.CardKinds.Count > 0 && !ProgramCardKindMatch.Contains(trigger.CardKinds, action.EffectiveKind) || trigger.CardCategories.Count > 0 && (action.Type != CardActionType.Use && !trigger.IncludeResponseUses || !CoreGameEngineProgramCardPaymentsRuntime.MatchesProgramCardCategory(action.EffectiveKind, trigger.CardCategories)))
                    continue;
                var bindingTargets = trigger.OnlyDesignatedCardTargets ? targets.Intersect(action.EffectiveKind == CardKind.BorrowedSword ? action.TargetSeats.Where((seat, index) => index % 2 == 0) : action.EffectiveDesignatedTargetSeats).ToHashSet() : targets;
                var matchingSources = action.ConversionChain.Where(source => source.OwnerSeat == owner.Seat && source.SkillId == trigger.SourceSkillId && (trigger.SourceViewAsId is null || source.BindingId == trigger.SourceViewAsId)).ToArray();
                if (trigger.SourceSkillId is not null && matchingSources.Length == 0)
                    continue;
                var matches = relation switch
                {
                    SkillProgramCardActionOwnerRelation.Actor => owner.Seat == action.ActorSeat,
                    SkillProgramCardActionOwnerRelation.Target => bindingTargets.Contains(owner.Seat),
                    SkillProgramCardActionOwnerRelation.Observer => true,
                    SkillProgramCardActionOwnerRelation.ConversionSource => matchingSources.Length > 0,
                    _ => false
                };
                if (!matches)
                    continue;
                if (relation == SkillProgramCardActionOwnerRelation.ConversionSource && targets.Count == 0 && !trigger.AllowNoEventTarget)
                    continue;
                var eventTargetsForBinding = window == SkillProgramTriggerWindow.CardUseTargetsFinalized && (CoreGameEngineFinalTargetSlashRuntime.HasFinalTargetSlashEffects(trigger.Effects) || CharacterZhuRongBoundaryMobileRuntime.IsMobileZhuRongTargetTrigger(binding.SkillId, trigger) || CharacterCaoAngMobileRuntime.IsMobileCaoAngTargetTrigger(binding.SkillId, trigger)) ? targets.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Order().ToArray() : CharacterYuanShaoMouMobileRuntime.IsMouYuanShaoTargetTrigger(binding.SkillId, trigger) ? targets.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Order().ToArray() : relation == SkillProgramCardActionOwnerRelation.ConversionSource && targets.Count > 0 && !trigger.SingleActionInstance ? targets.Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Order().ToArray() : [relation == SkillProgramCardActionOwnerRelation.Target ? owner.Seat : targets.Count == 1 ? targets.Single() : -1];
                if (relation == SkillProgramCardActionOwnerRelation.ConversionSource && eventTargetsForBinding.Length == 0 && trigger.AllowNoEventTarget)
                    eventTargetsForBinding = [-1];
                foreach (var eventTarget in eventTargetsForBinding)
                {
                    var capturedFacts = capturedOwnerFacts[owner.Seat] ??= _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(owner, action);
                    var facts = capturedFacts with
                    {
                        // Explicit completion conditions read the response appearance frozen before
                        // payment. Other candidates retain their existing captured facts.
                        CardActionSuit = window == SkillProgramTriggerWindow.CardUseCompleted && action.Type == CardActionType.Response && trigger.IncludeResponseUses && ProgramInstructionResolver.Default.Features(trigger).UsesCondition(SkillProgramTriggerConditionKind.CardActionSuitIs) ? action.EffectiveSuit : capturedFacts.CardActionSuit,
                        CardUseCausedDamage = cardUseCausedDamage,
                        EventTargetRuleWounded = _coreTreatedWoundedOperations.UsesTreatedWounded && eventTarget >= 0 ? _coreTreatedWoundedOperations.IsRuleWounded(_coreMatchParticipantsPlayersAccess._players[eventTarget]) : null,
                        EventTargetHandCount = eventTarget >= 0 ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[eventTarget]).Count : 0,
                        EventTargetHp = window == SkillProgramTriggerWindow.CardUseTargetsFinalized && (CoreGameEngineFinalTargetSlashRuntime.HasFinalTargetSlashEffects(trigger.Effects) || CharacterZhuRongBoundaryMobileRuntime.IsMobileZhuRongTargetTrigger(binding.SkillId, trigger) || CharacterCaoAngMobileRuntime.IsMobileCaoAngTargetTrigger(binding.SkillId, trigger)) && eventTarget >= 0 ? _coreMatchParticipantsPlayersAccess._players[eventTarget].Hp : capturedFacts.EventTargetHp,
                        OwnerEventTargetDistance = eventTarget >= 0 ? _coreNativeRuleQueriesOperations.GetCombatDistance(owner.Seat, eventTarget) : int.MaxValue,
                        CardActionTargetIsOwner = CharacterCaoAngMobileRuntime.IsMobileCaoAngTargetTrigger(binding.SkillId, trigger) ? eventTarget == owner.Seat : bindingTargets.Contains(owner.Seat)
                    };
                    var context = CreateCardActionProgramContext(action, window, parentFrameId: 0, owner.Seat, eventTarget, facts)with
                    {
                        OptionalChooserSeat = rewardChooser
                    };
                    if (trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.GrantActorHandLimitPenalty) && !_coreActorHandLimitTargetProgramsOperations.ShouldCollectActorHandLimitPenalty(action, owner.Seat, window))
                        continue;
                    if (trigger.Effects.Any(effect => CompletedUndamagedTargetRevealComposition.IsOperation(effect.Op)) && !_coreCompletedUndamagedTargetRevealOperations.CanOfferCompletedUndamagedTargetReveal(new(owner, binding.Program, ProgramInstructionResolver.Default.Resolve(binding.Program, ProgramInstructionSourceKind.Trigger, trigger.Id), new(owner.Seat, binding.SkillId, trigger.Id, binding.SkillInstanceId, binding.Program.GameplayHash, trigger.Priority), context)))
                        continue;
                    if (ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.DrawAfterPaidMeiYingConversion) && !_characterZhaoXiangOlOperations.CanOfferMeiYingConversionDraw(ToSharedCandidate(new(owner.Seat, eventTarget, binding.SkillId, trigger.Id, binding.Program.GameplayHash, binding.SkillInstanceId, trigger.Priority, context)), trigger, context))
                        continue;
                    if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DrawForSourceCurseUse))
                    {
                        var curse = _coreSourceCursesOperations.EligibleSourceCurseUse(owner.Seat, binding.SkillId, binding.SkillInstanceId, action, trigger.Effects.Single());
                        if (window != SkillProgramTriggerWindow.CardUseCommitted || curse is null)
                            continue;
                        context = context with
                        {
                            SourceCurseUse = new(curse, action.ActionId, action.ActorSeat, action.EffectiveIsRed)
                        };
                    }

                    if (_characterLiuChengMouMobileOperations.AttachMouLiuChengAccrual(action, binding.SkillId, binding.SkillInstanceId, context)is not { } liuChengContext)
                        continue;
                    context = liuChengContext;
                    result.Add(new(owner.Seat, eventTarget, binding.SkillId, trigger.Id, binding.Program.GameplayHash, binding.SkillInstanceId, trigger.Priority, context));
                }
            }

        _programPipelineHooksOperations._programPipelineHooks.CollectIssuedCardTrigger(action, window, result);
        string GrantIdentity(ProgramCardTriggerCandidate candidate)
        {
            var trigger = _contentRegistryOperations._contentRegistry.GetSkill(candidate.SkillId).Program!.Triggers.Single(t => t.Id == candidate.TriggerId);
            return ProgramInstructionResolver.Default.Features(trigger).HasOperation(SkillProgramEffectOp.GiveOwnedCardToOtherFinalTargetAndDraw) || trigger.Effects.Any(e => e.Op is SkillProgramEffectOp.OfferUniqueLargestHandTrickTargetAddition or SkillProgramEffectOp.JoinUniqueLargestHpTrickTargetAndDrawAfterDamage or SkillProgramEffectOp.DrawAfterJoinedTrickDamage or SkillProgramEffectOp.AdjustOneRoundGainedOrdinaryTrickTarget or SkillProgramEffectOp.DrawForRoundGainedEquipmentUse) || trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.GrantActorHandLimitPenalty) || trigger.Effects.Any(CoreProgramFactionActionRewardRuntime.IsFactionActionReward) ? "" : candidate.SkillInstanceId;
        }

        if (window == SkillProgramTriggerWindow.CardUseCompleted)
            result.AddRange(_characterLiFengOlOperations.CollectJiyunCompletedUseCandidates(action, continuation));
        return _characterRuanYuOlOperations.IncludeRuanYuMiaoxianRedCandidates(result.DistinctBy(candidate => (action.ActionId, candidate.OwnerSeat, candidate.SkillId, GrantIdentity(candidate), candidate.TriggerId, candidate.OpponentSeat)).ToArray(), action, window);
    }

    internal static ProgramTriggerCandidate ToSharedCandidate(ProgramCardTriggerCandidate candidate) => RuntimeSkills.RuntimeRules_Core_CardActions.ToSharedCandidate(candidate);
    internal ProgramSkillWindowContext CreateCardActionProgramContext(ProgramCardTriggerWindowFrame frame, ProgramCardTriggerCandidate candidate)
    {
        var context = candidate.FrozenContext ?? CreateCardActionProgramContext(frame.Action, GetCardActionWindow(frame), frame.Id, candidate.OwnerSeat, candidate.OpponentSeat, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], frame.Action));
        return _characterPangJiOlOperations.CreateBiguoTargetContext(frame, candidate, _characterLiFengOlOperations.CreateJiyunCompletedUseContext(frame, candidate, _characterLiuPanOlOperations.CreatePijingIssuedContext(frame, candidate, _characterMaMiDiOlOperations.CreateMaMiDiIssuedContext(frame, candidate, _characterHuaManOlOperations.CreateHuaManCardUseContext(frame, ToSharedCandidate(candidate), context)))));
    }

    internal ProgramSkillWindowContext CreateCardActionProgramContext(CardActionContext action, SkillProgramTriggerWindow window, long parentFrameId, int ownerSeat, int eventTargetSeat, SkillProgramTriggerFacts facts)
    {
        var suits = action.PhysicalCards.Select(cost =>
        {
            var location = _cardZonesOperations._cardZones.GetLocation(cost.CardId);
            return _cardZonesOperations._cardZones.CardsAt(location).Single(card => card.Id == cost.CardId).Suit;
        }).ToArray();
        var publicSuit = suits.Length == 1 ? suits[0] : (Suit? )null;
        bool? isRed = suits.Length == 0 ? null : suits.All(suit => suit is Suit.Heart or Suit.Diamond);
        var debit = _coreCardUseDebitsOperations.GetCardUseDebit(action.ActionId);
        return new(window, parentFrameId, ownerSeat, SourceSeat: action.ActorSeat, TargetSeat: eventTargetSeat >= 0 ? eventTargetSeat : null, Facts: facts, CardUse: new(action.ActionId, parentFrameId, action.ActorSeat, eventTargetSeat >= 0 ? eventTargetSeat : null, action.EffectiveKind, publicSuit, isRed, debit is not null, debit, action.DesignatedTargetSeats))
        {
            HuaManSouying = _characterHuaManOlOperations.HasHuaManCapability ? _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(use => use.Action?.ActionId == action.ActionId)?.HuaManSouyingQualification : null
        };
    }

    internal static SkillProgramTriggerWindow GetCardActionWindow(ProgramCardTriggerWindowFrame frame) => frame.MouSimaYiPlayed?.Window ?? frame.MouLuSuAnniversaryPlayed?.Window ?? RuntimeSkills.RuntimeRules_Core_CardActions.GetCardActionWindow(frame);
    [NativeFrameRoute(typeof(ProgramCardTriggerWindowFrame), 300)]
    internal void ContinueProgramCardWindowCore() => ProgramTriggerWindowDriver.Run(_kernelCoreGameEngineProgramTriggerWindowsOperations.CreateCardActionTriggerWindowHost());
    internal ProgramTriggerWindowPreparation<ProgramCardTriggerWindowFrame> PrepareProgramCardWindow(ProgramCardTriggerWindowFrame frame)
    {
        if (_characterHuaTuoAnniversaryGodOperations.TryBeginConvertedSkillInvocations(frame))
            return new(ProgramTriggerWindowFlow.Yield, frame);
        if (_characterCaoPiAnniversaryGodOperations.TryCompleteKemingConverted(frame))
            return new(ProgramTriggerWindowFlow.Yield, frame);
        if (_coreCardSupplyCompletionOperations.TryDrainCardSupplyCompletionCosts(frame))
            return new(ProgramTriggerWindowFlow.Yield, frame);
        frame = (ProgramCardTriggerWindowFrame)_resolutionStackOperations._resolutionStack[^1];
        if (_coreResponseCompletionOperations.TryDrainCardResponseCompletionCosts(frame))
            return new(ProgramTriggerWindowFlow.Yield, frame);
        frame = (ProgramCardTriggerWindowFrame)_resolutionStackOperations._resolutionStack[^1];
        if (_coreSoleTargetRedirectOperations.TryBeginSoleTargetRedirect(frame))
            return new(ProgramTriggerWindowFlow.Retry, frame);
        frame = _coreActorHandLimitTargetProgramsOperations.AppendActorHandLimitTargetCandidates(frame);
        frame = _characterLiuChengMouMobileOperations.AppendMouLiuChengAccrual(frame);
        frame = _characterYuanShaoMouMobileOperations.AppendMouYuanShaoTargets(frame);
        frame = _characterPangTongMouMobileOperations.AppendMouPtLateTargets(frame);
        frame = _coreFirstSlashDuelLegacyWindowsOperations.DeferFirstSlashDuelCandidates(frame);
        return new(ProgramTriggerWindowFlow.Ready, frame);
    }

    internal ProgramTriggerWindowFlow CompleteProgramCardWindow(ProgramCardTriggerWindowFrame frame)
    {
        if (_coreSoleTargetRedirectOperations.TryReturnSoleTargetRedirect(frame))
            return ProgramTriggerWindowFlow.Retry;
        if (GetCardActionWindow(frame) == SkillProgramTriggerWindow.CardUseTargetsFinalized)
            _coreDamageTargetBudgetOperations.SettleDamageMarkerBudgetTargets(frame.ParentFrameId);
        var attack = frame.AttackOwnerFrameId is { } attackId ? _kernelCoreMainOperations.CreateCardAttackHandle(attackId) : null;
        _coreRuntimeOperations.PopResolutionFrame(frame.Id, ResolutionFrameKind.ProgramCardTriggerWindow);
        if (_characterZhugeGuoLeOperations.TryStartLeChengyanEffect(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterSimaYiMouOperations.TryReturnMouSmPrivateWindow(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterSimaYiMouOperations.TryReturnMouSmPlayedWindow(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterLuSuMouOperations.TryReturnMouLuSuPlayedWindow(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterLiuYanOlOperations.TryReturnLiuYanLegacyAlcohol(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterCuiYanOlOperations.TryReturnCuiYanLegacyAlcohol(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterZhuJunOlOperations.TryReturnZhuJunLegacyAlcohol(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterHuangYueyingOlMouOperations.TryReturnMouBingcaiLegacyAlcohol(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_coreOverflowTargetCancellationOperations.TryCompleteOverflowCanceledUse(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (_characterTaishiCiMobileGodOperations.TryCompleteShenTaishiCiCancelledSlash(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (GetCardActionWindow(frame) == SkillProgramTriggerWindow.CardUseTargetsFinalized && _characterLiuPanOlOperations.TryCompletePijingZeroTargetUse(frame))
            return ProgramTriggerWindowFlow.Yield;
        if (GetCardActionWindow(frame) == SkillProgramTriggerWindow.CardUseTargetsFinalized && _coreSoleTargetQuarantineOperations.TryCompleteSoleTargetQuarantinedUse(frame.ParentFrameId))
            return ProgramTriggerWindowFlow.Yield;
        if (frame.CardSupplyCompletion is not null)
            _coreCardSupplyCompletionOperations.ContinueCardSupplyAfterCompletion(attack ?? throw new InvalidOperationException("A supplied card lost its native Slash."), frame);
        else if (frame.ResponseCompletion is not null)
            _coreResponseCompletionOperations.ContinueCardResponseAfterCompletion(attack, frame);
        else if (frame.Continuation is ProgramCardContinuation.CommittedSimpleCard or ProgramCardContinuation.FinalizedSimpleCard or ProgramCardContinuation.BeforeSimpleTargetEffects)
            _coreCardUseLifecycleOperations.ContinueSimpleCardUse(frame.ParentFrameId, frame.SimpleContinuation ?? throw new InvalidOperationException("The simple card use lost its continuation."));
        else if (frame.Continuation == ProgramCardContinuation.CommittedTrick)
            _coreCardUseLifecycleOperations.ContinueCommittedTrickUse(frame);
        else if (frame.Continuation == ProgramCardContinuation.CompletedCard)
            _coreNativeCardUseCompletionOperations.PopFinishedCardUse(frame.ParentFrameId);
        else if (frame.CompletedResponseReturn is { IsCommitted: true })
            _coreCommittedResponseUsesOperations.ContinueCommittedResponseUse(attack, frame);
        else if (frame.CompletedResponseReturn is not null)
            _coreResponseUseCompletionOperations.ContinueCompletedResponseUse(attack, frame);
        else if (frame.Continuation == ProgramCardContinuation.CommittedSlash)
            ContinueCommittedSlashAfterPrograms(attack ?? throw new InvalidOperationException("A committed Slash trigger lost its attack continuation."));
        else if (frame.Continuation == ProgramCardContinuation.BeforeTargetEffects)
            _coreNativeSlashUseOperations.ContinueSlashAfterProgramTargetEffects(attack ?? throw new InvalidOperationException("A before-target-effects trigger lost its attack continuation."));
        else if (frame.Continuation == ProgramCardContinuation.SlashTargetRedirecting)
            _coreNativeSlashUseOperations.ContinueSlashAfterRedirectPrograms(attack ?? throw new InvalidOperationException("A Slash redirection trigger lost its attack continuation."));
        else if (frame.Continuation == ProgramCardContinuation.SlashBeforeResponse)
            _coreNativeSlashUseOperations.ContinueSlashAfterResponsePrograms(attack ?? throw new InvalidOperationException("A Slash response trigger lost its attack continuation."));
        else if (frame.Continuation == ProgramCardContinuation.SlashFullyDodged)
            _coreNativeDodgeOperations.ContinueSlashAfterDodgePrograms(attack ?? throw new InvalidOperationException("A dodged Slash trigger lost its attack continuation."));
        else if (frame.Continuation is ProgramCardContinuation.BeforeTrickTargetEffects or ProgramCardContinuation.FinalizedTrick)
            ContinueTrickAfterProgramTargetEffects(frame);
        else if (frame.Continuation == ProgramCardContinuation.Slash)
            _coreNativeSlashUseOperations.ContinueSlashAfterFinalizedTargets(attack ?? throw new InvalidOperationException("A Slash card trigger lost its attack continuation."));
        else if (frame.Continuation == ProgramCardContinuation.CompletedSlash)
            ContinueCompletedSlashAfterPrograms(attack ?? throw new InvalidOperationException("A completed Slash trigger lost its attack continuation."));
        else if (frame.Continuation == ProgramCardContinuation.DelayedCard)
            ContinueAcceptedDelayedCardUse(frame);
        else if (frame.Continuation == ProgramCardContinuation.FinalizedDelayedPlacement)
            _coreDelayedPlacementTargetProgramsOperations.ContinueFinalizedDelayedPlacement(frame);
        else if (frame.Continuation == ProgramCardContinuation.NullificationResponse)
            _coreResponseUseCompletionOperations.ContinueNullificationAfterResponseUse(frame.Action);
        else
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack ?? throw new InvalidOperationException("A response card trigger lost its attack continuation."), frame.Action, frame.Continuation ?? throw new InvalidOperationException("A response card trigger lost its return continuation."));
        return ProgramTriggerWindowFlow.Yield;
    }

    internal void ContinueCompletedSlashAfterPrograms(NativeOwners.CardAttackHandle attack)
    {
        var completion = _coreCardAttackStateOperations.CaptureAttackCompletion(attack);
        _coreNativeCardUseCompletionOperations.PopFinishedCardUse(attack.ResolutionId);
        _coreNativeCardUseCompletionOperations.CompleteFinishedAttackCardUse(attack);
        _coreNativeAttackCompletionOperations.CompleteAttackAfterCardResolution(completion);
    }

    internal bool TryBeginDelayedCardUsePrograms(long resolutionId)
    {
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseAccepted(resolutionId))
            return false;
        _coreCardUseLifecycleOperations.TryMarkFinalizedTrickProgramsStarted(resolutionId);
        var frame = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(item => item.Id == resolutionId);
        var action = frame.Action ?? throw new InvalidOperationException("A direct delayed-card trigger requires a captured card action.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(action));
        return TryBeginProgramCardWindow(attack: null, action, SkillProgramTriggerWindow.CardUseTargetsFinalized, action.TargetSeats, ProgramCardContinuation.DelayedCard);
    }

    internal void ContinueAcceptedDelayedCardUse(ProgramCardTriggerWindowFrame frame)
    {
        var action = frame.Action;
        if (action.EffectiveKind != CardKind.Lightning || action.PhysicalCards.Count != 1 || action.TargetSeats is not [var targetSeat] || targetSeat != action.ActorSeat)
        {
            throw new InvalidOperationException("The delayed-card trigger continuation is not a self-targeted Lightning.");
        }

        var card = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Single(item => item.Id == action.PhysicalCards[0].CardId);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(frame.ParentFrameId, card, action.ActorSeat, action.TargetSeats, LegalActionKind.Lightning, playedCardKind: action.EffectiveKind);
    }

    internal void ContinueTrickAfterProgramTargetEffects(ProgramCardTriggerWindowFrame frame)
    {
        var continuation = frame.TrickContinuation ?? throw new InvalidOperationException("A trick program window lost its frozen continuation.");
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(use => use.Id == frame.ParentFrameId);
        var action = use.Action!;
        if (action.Type != CardActionType.Use || CardCatalog.Get(action.EffectiveKind).CategoryName != "锦囊牌" || action.PhysicalCards.All(cost => cost.CardId != continuation.EffectCardId) && !_coreDamageColorDuelProgramsOperations.MatchesSelectedActorDuelAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterTianChouOlOperations.MatchesTianChouShandaoUseAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterQinLangOlOperations.MatchesQinLangXianyingUseAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_coreVirtualOrdinaryTrickUsesOperations.MatchesVirtualOrdinaryTrickUseAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_coreTieredRoundZeroReturnsOperations.MatchesTieredRoundZeroUseAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterDongChengMouOperations.MatchesMouDcNativeAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterZhugeLiangMouOperations.MatchesMouZhijueUseAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterLiuChengMouMobileOperations.MatchesMouLiuChengNativeAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterXuHuangMouMobileOperations.MatchesMouXuHuangNativeAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_coreOriginalUseLightningOperations.MatchesOriginalUseLightningAction(frame.ParentFrameId, action, continuation.EffectCardId) && !_characterBaoSanNiangOlOperations.MatchesChosenAssaultUseAction(frame.ParentFrameId, action, continuation.EffectCardId))
            throw new InvalidOperationException("The trick trigger continuation does not match its card action.");
        var card = _coreDamageColorDuelProgramsOperations.GetTrickRepresentation(frame.ParentFrameId, continuation.EffectCardId, requireProcessing: true);
        var pending = new NativeOwners.JizhiResolution(action.ActorSeat, frame.ParentFrameId, card, action.EffectiveKind, action.EffectiveKind == CardKind.BorrowedSword ? use.TargetSeats.Skip(use.TargetIndex).Take(2).ToArray() : action.TargetSeats, continuation.ActionKind, continuation.TargetCardId, continuation.RequiredCardKind);
        if (frame.Continuation == ProgramCardContinuation.FinalizedTrick && TryBeginProgramCardUseBeforeTargetEffects(pending))
            return;
        _coreNativeNullificationOperations.ContinueJizhiOrNullificationAfterTargetTriggers(pending);
    }

    internal void ContinueCommittedSlashAfterPrograms(NativeOwners.CardAttackHandle attack)
    {
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None && (_characterHuanShuJunOlOperations.IsHuanpeiUse(attack.ResolutionId) || _characterGuoTuOlOperations.IsWeijieUse(attack.ResolutionId) || (_characterGuanHaiOlOperations.IsYouqueUse(attack.ResolutionId) || _characterLuKangWumiaoOperations.IsWuLuKangKegouUse(attack.ResolutionId) || _characterHuCheErMouOperations.IsMouHuCheErUse(attack.ResolutionId) || _characterZhangXiuMouOperations.IsMouZhangXiuAnniversaryUse(attack.ResolutionId) || _characterHuLieMouOperations.IsMouHuLieUse(attack.ResolutionId)) || _characterTaoQianOlOperations.IsZongluanUse(attack.ResolutionId) || _characterLiuZhangOlOperations.IsZonghuUse(attack.ResolutionId) || _coreRoundOnceCardlessNativeUsesOperations.IsRoundOnceCardlessBasicUse(attack.ResolutionId)))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.SlashTargetsCancelled == true)
        {
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(attack.ResolutionId, use => use with { SlashTargetsCancelled = false });
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void AdvanceProgramCardCandidate(ProgramCardTriggerWindowFrame frame) => _programWindowCursorOperations.Get().AdvanceProgramCardCandidate(frame);
}
