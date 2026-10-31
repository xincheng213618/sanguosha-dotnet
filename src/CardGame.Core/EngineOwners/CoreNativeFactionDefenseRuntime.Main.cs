#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFactionDefenseRuntime
{
    internal void BeginFactionDefenseRequest(NativeOwners.CardAttackHandle attack)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (!_coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionDefense(owner, attack) || _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null)
        {
            throw new InvalidOperationException("FactionDefense is not available for the current Dodge response.");
        }

        var policySource = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Dodge)!;
        var candidateSeats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(owner.Seat, policySource.FactionId);
        attack.MarkFactionDefenseAttempted();
        _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense = _kernelCoreMainOperations.CreateFactionDefenseHandle(attack, owner.Seat, candidateSeats, policySource);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionDefenseRequestedEvent(attack.ResolutionId, owner.Seat, candidateSeats, policySource.SkillId));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(policySource))}】，依次询问{(_coreProgramFactionPoliciesOperations.FactionProviderDescription(policySource))}是否替其打出【闪】。", owner.Seat, attack.SourceSeat);
        AdvanceFactionDefenseCandidate();
    }

    internal void AdvanceFactionDefenseCandidate(bool keepCurrentRequest = false)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense ?? throw new InvalidOperationException("There is no active FactionDefense request.");
        if (!CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack))
        {
            throw new InvalidOperationException("The FactionDefense request no longer belongs to the active attack.");
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(_resolutionStackOperations._resolutionStack[^1].Id, pending.OwnerSeat, pending.Source.SkillId, pending.InvocationOccurrenceKey, pending.Source.SkillInstanceId, SkillInvocationReturnKind.FactionDefense))
            return;
        var retainedCandidateIndex = keepCurrentRequest ? pending.CandidateIndex : -1;
        while (pending.CandidateIndex < pending.CandidateSeats.Count)
        {
            var provider = _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat];
            var responseCards = provider.IsAlive && (pending.Source.FactionId == "magic-any" || string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider), pending.Source.FactionId, StringComparison.Ordinal)) ? _coreNativeResponseMaterialQueriesOperations.GetResponseCards(provider, CardKind.Dodge) : [];
            var hasBagua = provider.IsAlive && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(provider);
            var retainsProvider = pending.CandidateIndex == retainedCandidateIndex && provider.IsAlive && (pending.Source.FactionId == "magic-any" || _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) == pending.Source.FactionId);
            if (!retainsProvider && responseCards.Count == 0 && !hasBagua && !_coreProgramDodgeResponsesOperations.ProgramDodgeResponseChoices(provider).Any() && !(provider.IsAlive && (pending.Source.FactionId == "magic-any" || _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) == pending.Source.FactionId) && (_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(provider, CardKind.Dodge) || _characterGuoTuOlOperations.HasWeijieRequiredBasic(provider, CardKind.Dodge))))
            {
                pending.CandidateIndex++;
                continue;
            }

            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondDodge, provider.Seat, $"{(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 发动了【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】，是否替其打出【闪】？", responseCards.Select(card => card.Id).ToArray(), [], pending.Attack.SourceSeat, pending.Attack.EffectiveCardKind)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                TargetSeat = pending.OwnerSeat,
                RequiredCardKind = CardKind.Dodge,
                Choices = CreateFactionDefenseResponseChoices(provider, pending, responseCards, hasBagua)
            };
            _coreMatchOutcomeStatusAccess._status = provider.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        var attack = pending.Attack;
        var ownerSeat = pending.OwnerSeat;
        _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense = null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionDefenseResolvedEvent(attack.ResolutionId, ownerSeat, Succeeded: false, ProviderSeat: null, ResponseCardId: null));
        _coreMatchLogOperations.AddLog("SkillResolved", $"没有角色响应 {(_coreMatchParticipantsPlayersAccess._players[ownerSeat].Name)} 的【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】。", ownerSeat, attack.SourceSeat);
        _coreNativeDodgeOperations.PublishOwnerDodgeDecision(attack);
    }

    internal IReadOnlyList<PromptChoice> CreateFactionDefenseResponseChoices(CharacterState provider, NativeOwners.FactionDefenseHandle pending, IReadOnlyList<Card> responseCards, bool includeBagua)
    {
        var baitOnlyFirstIds = _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, CardKind.Dodge, true).Where(_characterGuanHaiOlOperations.IsXieweiSelection).Select(selection => selection.Cards[0].Id).ToHashSet();
        var choices = responseCards.Where(card => !baitOnlyFirstIds.Contains(card.Id) || _coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(provider, card, CardKind.Dodge)).SelectMany(card =>
        {
            var responseCardKind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(provider, card, CardKind.Dodge);
            var responseDescription = CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, CardKind.Dodge) ? "打出【闪】" : $"将【{(card.DisplayName)}】当作【闪】";
            return _coreCardConversionsOperations.CreateConversionChoiceVariants(provider, card, responseCardKind, forResponse: true, $"faction-defense.dodge.card-{(card.Id)}", $"{(responseDescription)}，替 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 响应【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】。", [card.Id], [pending.OwnerSeat], new Dictionary<string, string> { ["response"] = "faction-defense-dodge", ["response-card-kind"] = responseCardKind.ToString(), ["skill"] = pending.Source.SkillId });
        }).ToList();
        choices.AddRange(_coreProgramDodgeResponsesOperations.ProgramDodgeResponseChoices(provider));
        choices.AddRange(_coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(provider, CardKind.Dodge).Where(choice => CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source) && _characterGuanHaiOlOperations.IsXieweiSource(source)));
        if (includeBagua)
        {
            choices.Add(new PromptChoice(new ChoiceId("faction-defense.bagua"), "发动【八卦阵】判定；若为红色，视为替求闪者打出【闪】。", [], [pending.OwnerSeat], new Dictionary<string, string> { ["response"] = "faction-defense-bagua", ["skill"] = pending.Source.SkillId, ["equipment"] = CardKind.BaguaFormation.ToString() }));
            if (provider.IsHuman)
                choices.Add(CoreGameEngineBaguaResponsesRuntime.BaguaSkipChoice(factionDefense: true));
        }

        choices.Add(new PromptChoice(new ChoiceId("faction-defense.decline"), $"不响应【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】。", [], [pending.OwnerSeat], new Dictionary<string, string> { ["response"] = "faction-defense-decline", ["skill"] = pending.Source.SkillId }));
        return choices;
    }

    internal void ResolveFactionDefenseCandidateResponse(NativeOwners.FactionDefenseHandle pending, bool useDodge, bool useBagua, int? requestedCardId)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, pending.Attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondDodge } decision || decision.PlayerSeat != pending.CurrentCandidateSeat || useDodge && useBagua)
        {
            throw new InvalidOperationException("The FactionDefense response is not the current continuation.");
        }

        var provider = _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat];
        var selectedDodge = useDodge ? _coreNativeResponseMaterialQueriesOperations.GetResponseCards(provider, CardKind.Dodge).FirstOrDefault(card => card.Id == requestedCardId) : null;
        if (useDodge && selectedDodge is null)
        {
            throw new InvalidOperationException("The FactionDefense responder has no matching Dodge response card.");
        }

        if (useBagua && (!_coreNativeEquipmentAndFactionQueriesOperations.HasBagua(provider)))
        {
            throw new InvalidOperationException("The FactionDefense responder cannot use Bagua in this response window.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        if (selectedDodge is not null)
        {
            CompleteFactionDefenseResponse(pending, provider, selectedDodge, usedBagua: false);
            return;
        }

        if (useBagua)
        {
            if (_coreNativeEquipmentInvocationsOperations.TryBeginBaguaEquipmentInvocation(pending.Attack, provider, factionDefense: true))
                return;
            var judgmentResult = _coreNativeJudgmentOperations.BeginJudgment(pending.Attack, provider.Seat, JudgmentReasons.BaguaDefense, pending.Attack.ResolutionId, pending.Attack.EffectiveCardKind, JudgmentContinuationKind.FactionDefenseBagua);
            if (judgmentResult is { } succeeded)
            {
                _coreNativeDodgeOperations.CompleteFactionDefenseBaguaResponse(pending.Attack, succeeded);
            }

            return;
        }

        _coreMatchLogOperations.AddLog("SkillSkipped", $"{(provider.Name)} 选择不响应 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 的【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】。", provider.Seat, pending.OwnerSeat);
        pending.CandidateIndex++;
        AdvanceFactionDefenseCandidate();
    }

    internal void CompleteFactionDefenseResponse(NativeOwners.FactionDefenseHandle pending, CharacterState provider, Card? selectedDodge, bool usedBagua, CardConversionSource? virtualResponseSource = null)
    {
        if (selectedDodge is not null && !_coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(provider, selectedDodge, CardKind.Dodge) && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, CardKind.Dodge, true).FirstOrDefault(selection => _characterGuanHaiOlOperations.IsXieweiSelection(selection) && selection.Cards[0].Id == selectedDodge.Id)is { } baitResponse && _characterGuanHaiOlOperations.TryCompleteXieweiFactionDefense(pending, provider, baitResponse))
            return;
        if (selectedDodge is { } declaredDodge && _coreCardDeclarationsOperations.TryBeginCardDeclaration(provider, declaredDodge, CardKind.Dodge, _coreCardDeclarationsOperations.PeekDeclarationConversion(provider, declaredDodge, CardKind.Dodge, false), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.FactionDefense, pending.OwnerSeat), [pending.Attack.SourceSeat]))
            return;
        var attack = pending.Attack;
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || provider.Seat != pending.CurrentCandidateSeat)
        {
            throw new InvalidOperationException("The completed FactionDefense response is not current.");
        }

        if (selectedDodge is not null)
            _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(provider, selectedDodge, CardKind.Dodge, _coreGameEngineCardConversionsSelectedResponseConversionAccess._selectedResponseConversion);
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        CardKind? responseCardKind = null;
        CardConversionSource? responseConversion = null;
        CardLocation? responseFrom = null;
        bool? responseCostIsRed = null;
        if (selectedDodge is not null)
        {
            responseCardKind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(provider, selectedDodge, CardKind.Dodge);
            responseConversion = _coreCardConversionsOperations.GetSelectedResponseConversion(provider, selectedDodge, responseCardKind.Value);
            responseFrom = _characterMaChaoAnniversaryGodOperations.FindOwnedCardLocation(provider, selectedDodge, responseConversion);
            responseCostIsRed = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(provider.Seat, selectedDodge);
            _coreAlternativeViewAsCostsOperations.PaySingleCardResponse(selectedDodge, provider, responseCardKind.Value, responseConversion);
        }

        _coreMatchLogOperations.AddLog("CardResponded", usedBagua ? $"{(provider.Name)} 发动【八卦阵】响应【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】，视为 {(owner.Name)} 打出【闪】。" : $"{(provider.Name)} 响应【{(_coreProgramFactionPoliciesOperations.FactionPolicyName(pending.Source))}】，替 {(owner.Name)} 打出【闪】。", provider.Seat, owner.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionDefenseResolvedEvent(attack.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, selectedDodge?.Id, usedBagua));
        if (selectedDodge is not null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(selectedDodge.Id, owner.Seat, attack.SourceSeat, responseCardKind ?? CardKind.Dodge));
        }
        else if (virtualResponseSource is not null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(-1, owner.Seat, attack.SourceSeat, CardKind.Dodge));
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack } group && CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, attack))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, CardKind.Dodge, owner.Seat, UsedResponse: true, ResponseCardId: selectedDodge?.Id, ResponseCardKind: CardKind.Dodge));
        }

        if (selectedDodge is not null)
        {
            var responseCost = new CardActionCost(selectedDodge.Id, selectedDodge.Kind, responseFrom!.Value, responseCostIsRed);
            if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, provider, requesterSeat: owner.Seat, attack.SourceSeat, responseCardKind ?? CardKind.Dodge, [responseCost], ProgramCardContinuation.FactionDefenseDodge, responseConversion))
            {
                return;
            }

            _coreAlternativeViewAsCostsOperations.FinishSingleCardResponse(selectedDodge, responseConversion);
        }
        else if ((usedBagua || virtualResponseSource is not null) && _coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, provider, requesterSeat: owner.Seat, attack.SourceSeat, CardKind.Dodge, [], ProgramCardContinuation.FactionDefenseDodge, virtualResponseSource))
        {
            return;
        }

        _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
    }
}
