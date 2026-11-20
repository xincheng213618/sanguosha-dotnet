#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDodgeRuntime
{
    internal void CompleteSuccessfulDodgeResponse(NativeOwners.CardAttackHandle attack)
    {
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(attack.ResolutionId, new(RecoveryPaidCardUseKind.DodgeCompletion, attack.SourceSeat)))
            return;
        _characterHuanShuJunOlOperations.ClearHuanpeiDodgeResponse(attack);
        _coreRoundOnceCardlessNativeResponsesOperations.ClearRoundOnceCardlessDodgeResponse(attack);
        _coreTieredRoundZeroResponsesOperations.ClearTieredRoundDodgeResponse(attack);
        _coreChainedStateBasicReturnsOperations.ClearChainedStateBasicDodgeResponse(attack);
        _coreDrawFundedDistinctBasicResponsesOperations.Value.ClearDrawFundedDistinctBasicDodgeResponse(attack);
        FinishSuccessfulDodgeResponse(attack);
    }

    internal void FinishSuccessfulDodgeResponse(NativeOwners.CardAttackHandle attack)
    {
        if (_coreRequiredGroupDodgesOperations.Value.TryFinishRequiredGroupDodge(attack))
            return;
        var completed = attack.RegisterDodgeResponse();
        if (attack.RequiredDodgeResponses > 1)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RequiredResponseProgressEvent(attack.ResolutionId, attack.SourceSeat, attack.TargetSeat, CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack), CardKind.Dodge, attack.SuccessfulDodgeResponses, attack.RequiredDodgeResponses));
        }

        if (completed)
        {
            _characterCaoXiOlOperations.CaptureCaoXiGangshuSlashOffset(attack);
            if (_coreCardActionsOperations.TryBeginProgramSlashStage(attack, SkillProgramTriggerWindow.SlashFullyDodged, ProgramCardContinuation.SlashFullyDodged))
            {
                return;
            }

            ContinueSlashAfterDodgePrograms(attack);
            return;
        }

        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.AwaitingResponse);
        _coreNativeSlashUseOperations.ContinueSlashDefense(attack);
    }

    internal void ContinueSlashAfterDodgePrograms(NativeOwners.CardAttackHandle attack)
    {
        if (_coreSlashSuitDiscardOperations.Value.IsSlashDodgeCancellationPrevented(attack))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreSlashTargetBenefitWindowsOperations.TryBeginDodgeCancelledSlashBenefits(attack))
            return;
        ContinueSlashAfterDodgeCancellation(attack);
    }

    internal void ContinueSlashAfterDodgeCancellation(NativeOwners.CardAttackHandle attack)
    {
        if (_coreNativeQinglongOperations.TryBeginQinglongCrescentBladeChoice(attack))
        {
            return;
        }

        if (_coreNativeStoneAxeOperations.Value.TryBeginStoneAxeChoice(attack))
        {
            return;
        }

        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void CompleteFactionDefenseBaguaResponse(NativeOwners.CardAttackHandle attack, bool succeeded)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense ?? throw new InvalidOperationException("A FactionDefense Bagua judgment has no FactionDefense continuation.");
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(pending.Attack, attack))
        {
            throw new InvalidOperationException("The FactionDefense Bagua judgment belongs to another attack.");
        }

        var provider = _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat];
        succeeded &= _characterHuangZuMobileShiOperations.ShiHzResponseColorAllowed(provider.Seat, effectiveIsRed: null);
        if (succeeded)
        {
            _coreNativeFactionDefenseOperations.CompleteFactionDefenseResponse(pending, provider, selectedDodge: null, usedBagua: true);
            return;
        }

        _coreMatchLogOperations.AddLog("SkillResolved", $"{(provider.Name)} 以【八卦阵】响应【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】，但判定失败。", provider.Seat, pending.OwnerSeat);
        _coreBaguaResponsesOperations.PublishFailedBaguaResponse(attack);
    }

    internal void PublishOwnerDodgeDecision(NativeOwners.CardAttackHandle attack)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        var dodges = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(owner, CardKind.Dodge);
        var hasBagua = !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(owner);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondDodge, owner.Seat, "无人响应求闪，是否由你自己打出【闪】？", dodges.Select(card => card.Id).ToArray(), [], attack.SourceSeat, attack.EffectiveCardKind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            RequiredCardKind = CardKind.Dodge,
            Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(dodges, CardKind.Dodge, attack.EffectiveCardKind, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(owner, card, CardKind.Dodge), includeBagua: hasBagua, includeFactionDefense: false, responder: owner)
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveDodgeResponse(NativeOwners.CardAttackHandle attack, CharacterState defender, Card responseCard)
    {
        if (_responseRules.IsResponseForbidden(attack.ResolutionId, defender.Seat, CardKind.Dodge, attack.SourceSeat))
            throw new InvalidOperationException("The incoming Slash forbids this Dodge response.");
        if (_coreCardDeclarationsOperations.TryBeginCardDeclaration(defender, responseCard, CardKind.Dodge, _coreCardDeclarationsOperations.PeekDeclarationConversion(defender, responseCard, CardKind.Dodge, false), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.Dodge, defender.Seat), [attack.SourceSeat]))
            return;
        if (!_coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(defender, responseCard, CardKind.Dodge) && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(defender, CardKind.Dodge, true).FirstOrDefault(selection => selection.Cards[0].Id == responseCard.Id)is { } extended)
        {
            _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse(defender, extended);
            return;
        }

        var responseCardKind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(defender, responseCard, CardKind.Dodge);
        var responseConversion = _coreCardConversionsOperations.GetSelectedResponseConversion(defender, responseCard, responseCardKind);
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(defender, responseCard, responseCardKind, responseConversion);
        var responseFrom = _characterMaChaoAnniversaryGodOperations.FindOwnedCardLocation(defender, responseCard, responseConversion);
        var responseCostIsRed = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(defender.Seat, responseCard);
        var completedResponseUseSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(defender, [responseCard], responseCardKind);
        _coreAlternativeViewAsCostsOperations.PaySingleCardResponse(responseCard, defender, responseCardKind, responseConversion);
        var incomingName = CardCatalog.Get(CoreNativeDamagePoliciesRuntime.RequireAttackCardKind(attack)).DisplayName;
        var responseName = CardCatalog.Get(responseCardKind).DisplayName;
        var responseDescription = _coreCardDeclarationsOperations.PublicDeclarationDescription(responseCard, CoreNativeCardClassificationsRuntime.IsNativeResponseCard(responseCard, CardKind.Dodge) ? $"打出【{(responseName)}】" : $"将【{(responseCard.DisplayName)}】当作【{(responseName)}】");
        var completesResponse = attack.SuccessfulDodgeResponses + 1 >= attack.RequiredDodgeResponses;
        _coreMatchLogOperations.AddLog("CardResponded", completesResponse ? _coreSlashSuitDiscardOperations.Value.IsSlashDodgeCancellationPrevented(attack) ? $"{(defender.Name)} {(responseDescription)}，响应了【{(incomingName)}】，但此杀不能被抵消。" : $"{(defender.Name)} {(responseDescription)}，抵消了【{(incomingName)}】。" : $"{(defender.Name)} {(responseDescription)}响应【{(incomingName)}】；【无双】仍要求下一张【闪】。", defender.Seat, attack.SourceSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(responseCard.Id, defender.Seat, attack.SourceSeat, responseCardKind));
        var responseCost = new CardActionCost(responseCard.Id, responseCard.Kind, responseFrom, responseCostIsRed);
        if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, defender, defender, requesterSeat: null, attack.SourceSeat, responseCardKind, [responseCost], ProgramCardContinuation.Dodge, responseConversion, completedResponseUseSuit))
        {
            return;
        }

        _coreAlternativeViewAsCostsOperations.FinishSingleCardResponse(responseCard, responseConversion);
        CompleteSuccessfulDodgeResponse(attack);
    }

    internal static bool RecordsNativeDodgeSettlement(ContentRegistry registry, CardUseFrame? use, CardActionContext action, ProgramCardContinuation continuation) =>
        continuation is ProgramCardContinuation.Dodge or ProgramCardContinuation.FactionDefenseDodge &&
        action is { Type: CardActionType.Response, EffectiveKind: CardKind.Dodge, PhysicalCards.Count: > 0 } &&
        use?.Action is { Type: CardActionType.Use } original && CoreNativeCardClassificationsRuntime.IsSlashCard(original.EffectiveKind) &&
        action.ParentActionId == original.ActionId && registry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferCompletedCardGift);

    internal void ContinueAcceptedCardResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        var processing = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId);
        var entries = RecordsNativeDodgeSettlement(_contentRegistryOperations._contentRegistry, use, action, continuation)
            ? new List<NativeDodgeDiscardEntry>() : null;
        void RecordDiscard(CardMovementRecord move)
        {
            if (move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile)
                entries!.Add(new(move.CardId, move.Sequence));
        }

        var finishedBatch = _characterXunXuAnniversaryOperations.TryFinishAnniversaryXunXuAcceptedResponse(action);
        if (finishedBatch && entries is not null &&
            action.PhysicalCards.All(cost => _cardZonesOperations._cardZones.GetLocation(cost.CardId) == CardLocation.DiscardPile))
            foreach (var movement in _coreResponseCompletionOperations.CaptureResponseCompletionCosts(action, continuation, attack.ResolutionId))
                RecordDiscard(movement);
        var responseCosts = finishedBatch ? [] : action.PhysicalCards;
        foreach (var cost in responseCosts)
        {
            if (_characterGuanHaiOlOperations.TryGetXieweiResponseCostMovement(action, cost.CardId, out _))
            {
                if (processing.Any(card => card.Id == cost.CardId))
                    throw new InvalidOperationException("A paid bait response cost cannot enter Processing.");
                continue;
            }

            if (_coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, cost.CardId))
            {
                if (processing.Any(card => card.Id == cost.CardId))
                    throw new InvalidOperationException("An alternative response cost must not remain in Processing.");
                if (entries is not null && _coreWushuangJinzhiOperations.TryGetWushuangJinzhiResponseMovements(action, cost.CardId, out var paid))
                    foreach (var movement in paid)
                        RecordDiscard(movement);
                continue;
            }

            if (_coreResponseEntityExchangeOperations.IsExchangedCardClaim(action.ActionId, cost.CardId))
                continue;
            if (_coreGuanduChariotResponsesOperations.GuanduChariotResponsePayment(action, attack.ResolutionId, cost, continuation)is not null)
                continue;
            var card = processing.Single(item => item.Id == cost.CardId);
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished,
                beforeFact: entries is null ? null : RecordDiscard);
        }

        if (entries is { Count: > 0 })
        {
            var settlement = new NativeDodgeResponseSettlement(action, Array.AsReadOnly(entries.ToArray()));
            _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(attack.ResolutionId, original => original with
            {
                NativeDodgeResponses = Array.AsReadOnly((original.NativeDodgeResponses ?? []).Append(settlement).ToArray())
            });
        }

        if (_coreResponseCompletionOperations.TryBeginCardResponseCompleted(attack, action, continuation))
            return;
        if (_coreResponseUseCompletionOperations.TryBeginCompletedResponseUsePrograms(attack, action, continuation))
            return;
        ContinueFinishedCardResponse(attack, action, continuation);
    }

    internal void ContinueFinishedCardResponse(NativeOwners.CardAttackHandle attack, CardActionContext action, ProgramCardContinuation continuation)
    {
        if (_characterXunYuMobileGodOperations.TryContinueQizhengResponse(attack, action, continuation))
            return;
        if (_invalidationExecution.HasIneffectiveDodge(attack, action, continuation))
            _characterHuanShuJunOlOperations.ClearHuanpeiDodgeResponse(attack);
        if (_invalidationExecution.ContinueIneffectiveDodge(attack, action, continuation))
            return;
        switch (continuation)
        {
            case ProgramCardContinuation.Dodge:
            case ProgramCardContinuation.FactionDefenseDodge:
                CompleteSuccessfulDodgeResponse(attack);
                return;
            case ProgramCardContinuation.DuelSlash:
            case ProgramCardContinuation.FactionSlashDuelSlash:
            {
                var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("An accepted Duel response has no Duel continuation.");
                _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(duel, action.ActorSeat);
                return;
            }

            case ProgramCardContinuation.GroupResponse:
                if (action.EffectiveKind == CardKind.Dodge)
                    CompleteSuccessfulDodgeResponse(attack);
                else
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                return;
            case ProgramCardContinuation.FactionSlashGroupResponse:
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                return;
            default:
                throw new InvalidOperationException($"Continuation {(continuation)} is not a card-response continuation.");
        }
    }
}
