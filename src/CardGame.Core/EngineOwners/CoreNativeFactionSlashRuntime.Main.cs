#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeFactionSlashRuntime
{
    internal void BeginProgramFactionCardRequest(long frameId, int ownerSeat, int targetSeat, string providerFactionId, CardKind requiredKind)
    {
        if (requiredKind != CardKind.Slash || string.IsNullOrWhiteSpace(providerFactionId))
            throw new InvalidOperationException("This faction-card request supports a Slash and a named faction.");
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId || frame.OwnerSeat != ownerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(_coreMatchParticipantsPlayersAccess._players[ownerSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash))
            throw new InvalidOperationException("The program faction-card request has no valid parent or target.");
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(frameId, FactionCardRequestPurpose.ProgramSkillUse, ownerSeat, _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(ownerSeat, providerFactionId), targetSeat: targetSeat, programSkillFrameId: frameId, providerFactionId: providerFactionId, requiredKind: requiredKind, policySource: _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[ownerSeat], requiredKind));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        AdvanceFactionSlashCandidate();
    }

    internal void BeginFactionSlashResponseRequest(NativeOwners.CardAttackHandle attack)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null)
        {
            throw new InvalidOperationException("Another FactionSlash request is already active.");
        }

        FactionCardRequestPurpose purpose;
        CharacterState owner;
        if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is { } duel && CoreNativeDamageHandlesRuntime.SameAttackOwner(duel.Attack, attack))
        {
            purpose = FactionCardRequestPurpose.DuelResponse;
            owner = _coreMatchParticipantsPlayersAccess._players[duel.ResponderSeat];
            if (!_coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionSlashResponse(owner, attack))
            {
                throw new InvalidOperationException("FactionSlash is not available for this Duel response.");
            }

            duel.FactionSlashAttempted = true;
        }
        else if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack, RequiredCardKind: CardKind.Slash } group && CoreNativeDamageHandlesRuntime.SameAttackOwner(group.CurrentAttack, attack))
        {
            purpose = FactionCardRequestPurpose.GroupResponse;
            owner = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
            if (!_coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionSlashResponse(owner, attack))
            {
                throw new InvalidOperationException("FactionSlash is not available for this group response.");
            }

            attack.MarkFactionSlashAttempted();
        }
        else
        {
            throw new InvalidOperationException("FactionSlash requires a Duel or Barbarian Assault Slash response.");
        }

        var requestPolicy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Slash)!;
        var candidateSeats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(owner.Seat, requestPolicy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(attack.ResolutionId, purpose, owner.Seat, candidateSeats, responseAttack: attack, providerFactionId: requestPolicy.FactionId, policySource: requestPolicy);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(attack.ResolutionId, owner.Seat, candidateSeats, SkillId: requestPolicy.SkillId, IsActiveUse: false));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动主公技【激将】，依次询问其他蜀势力角色是否替其打出【杀】。", owner.Seat, attack.SourceSeat);
        AdvanceFactionSlashCandidate();
    }

    internal void BeginBorrowedSwordFactionSlashRequest(NativeOwners.BorrowedSwordHandle borrowedSword)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, borrowedSword) || !borrowedSword.AwaitingSlashChoice || !_coreNativeBorrowedSwordOperations.CanRequestBorrowedSwordFactionSlash(borrowedSword))
        {
            throw new InvalidOperationException("FactionSlash is not available for this Borrowed Sword use.");
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[borrowedSword.WeaponOwnerSeat];
        var requestPolicy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Slash)!;
        var candidateSeats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(owner.Seat, requestPolicy.FactionId);
        borrowedSword.FactionSlashAttempted = true;
        borrowedSword.AwaitingSlashChoice = false;
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(borrowedSword.ResolutionId, FactionCardRequestPurpose.BorrowedSwordUse, owner.Seat, candidateSeats, targetSeat: borrowedSword.SlashTargetSeat, borrowedSword: borrowedSword, providerFactionId: requestPolicy.FactionId, policySource: requestPolicy);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(borrowedSword.ResolutionId, owner.Seat, candidateSeats, SkillId: requestPolicy.SkillId, IsActiveUse: true, borrowedSword.SlashTargetSeat));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 在【借刀杀人】结算中发动主公技【激将】，请求其他蜀势力角色为其对 {(_coreMatchParticipantsPlayersAccess._players[borrowedSword.SlashTargetSeat].Name)} 提供【杀】。", owner.Seat, borrowedSword.SlashTargetSeat);
        AdvanceFactionSlashCandidate();
    }

    internal void BeginQinglongCrescentBladeFactionSlashRequest(NativeOwners.QinglongCrescentBladeHandle qinglong)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || !CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, qinglong) || !_coreNativeQinglongOperations.CanRequestQinglongCrescentBladeFactionSlash(qinglong))
        {
            throw new InvalidOperationException("FactionSlash is not available for this Qinglong Crescent Blade use.");
        }

        var attack = qinglong.Attack;
        var owner = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var requestPolicy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(owner, CardKind.Slash)!;
        var candidateSeats = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(owner.Seat, requestPolicy.FactionId);
        qinglong.FactionSlashAttempted = true;
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(attack.ResolutionId, FactionCardRequestPurpose.QinglongCrescentBladeUse, owner.Seat, candidateSeats, targetSeat: attack.TargetSeat, qinglongCrescentBlade: qinglong, providerFactionId: requestPolicy.FactionId, policySource: requestPolicy);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(attack.ResolutionId, owner.Seat, candidateSeats, SkillId: requestPolicy.SkillId, IsActiveUse: true, attack.TargetSeat));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 在【青龙偃月刀】结算中发动主公技【激将】，请求其他蜀势力角色为其对 {(_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].Name)} 提供【杀】。", owner.Seat, attack.TargetSeat);
        AdvanceFactionSlashCandidate();
    }

    internal void AdvanceFactionSlashCandidate(bool keepCurrentRequest = false)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("There is no active FactionSlash request.");
        if (!pending.AwaitingProviders)
        {
            throw new InvalidOperationException("The FactionSlash provider cursor has already completed.");
        }

        if (!pending.IsProgramSkillUse && pending.PolicySource is { } invocationSource && _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _characterHuaTuoAnniversaryGodOperations.TryBeginSkillInvocation(_resolutionStackOperations._resolutionStack[^1].Id, pending.OwnerSeat, invocationSource.SkillId, pending.InvocationOccurrenceKey, invocationSource.SkillInstanceId, SkillInvocationReturnKind.FactionSlash))
            return;
        if (_coreFactionRequestPaymentsOperations.TryPublishFactionRequestCost(pending))
            return;
        var retainedCandidateIndex = keepCurrentRequest ? pending.CandidateIndex : -1;
        while (pending.CandidateIndex < pending.CandidateSeats.Count)
        {
            var provider = _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat];
            var slashes = provider.IsAlive && string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider), pending.ProviderFactionId, StringComparison.Ordinal) ? _coreCardConversionsOperations.GetFactionSlashSlashCards(pending, provider) : [];
            var zhangbaPairs = provider.IsAlive && string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider), pending.ProviderFactionId, StringComparison.Ordinal) ? _coreAssistedFactionSlashOperations.GetFactionRequestZhangbaPairs(pending, provider) : [];
            var isResponse = !pending.IsProgramSkillUse && !pending.IsBorrowedSwordUse && !pending.IsQinglongCrescentBladeUse;
            var programMultiCardConversions = provider.IsAlive && string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider), pending.ProviderFactionId, StringComparison.Ordinal) ? _coreAssistedFactionSlashOperations.GetFactionRequestMultiCardSelections(pending, provider) : [];
            var retainsProvider = pending.CandidateIndex == retainedCandidateIndex && provider.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) == pending.ProviderFactionId;
            if (!retainsProvider && slashes.Count == 0 && zhangbaPairs.Count == 0 && programMultiCardConversions.Count == 0 && !(provider.IsAlive && _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(provider) == pending.ProviderFactionId && (_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(provider, CardKind.Slash) || _characterGuoTuOlOperations.HasWeijieRequiredBasic(provider, CardKind.Slash))))
            {
                pending.CandidateIndex++;
                continue;
            }

            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondSlash, provider.Seat, $"{(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 发动了【{(GetFactionRequestDisplayName(pending))}】，是否替其打出【杀】？", slashes.Select(card => card.Id).Concat(zhangbaPairs.SelectMany(pair => pair.Select(card => card.Id))).Concat(programMultiCardConversions.SelectMany(item => item.Cards.Select(card => card.Id))).Distinct().ToArray(), [], SourceSeat: pending.OwnerSeat, IncomingCard: CardKind.Slash)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                TargetSeat = pending.OwnerSeat,
                RequiredCardKind = CardKind.Slash,
                Choices = CreateFactionSlashResponseChoices(provider, pending, slashes)
            };
            _coreMatchOutcomeStatusAccess._status = provider.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        CompleteFactionSlashFailure(pending);
    }

    internal string GetFactionRequestSkillId(NativeOwners.FactionCardRequestHandle pending) => pending.IsProgramSkillUse && !pending.IsAssistedProgramUse && pending.ProgramSkillFrameId is { } frameId ? _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.Id == frameId).SkillId : pending.PolicySource!.SkillId;
    internal string GetFactionRequestDisplayName(NativeOwners.FactionCardRequestHandle pending) => _contentRegistryOperations._contentRegistry!.GetSkill(GetFactionRequestSkillId(pending)).Name;
    internal IReadOnlyList<PromptChoice> CreateFactionSlashResponseChoices(CharacterState provider, NativeOwners.FactionCardRequestHandle pending, IReadOnlyList<Card> slashes)
    {
        if (pending.IsAssistedProgramUse)
            return _coreAssistedFactionSlashOperations.CreateAssistedFactionSlashChoices(provider, pending);
        var responseTargets = pending.TargetSeat is { } responseTargetSeat ? new[]
        {
            responseTargetSeat
        }

        : new[]
        {
            pending.OwnerSeat
        };
        var isResponse = !pending.IsProgramSkillUse && !pending.IsBorrowedSwordUse && !pending.IsQinglongCrescentBladeUse;
        var choices = slashes.SelectMany(card =>
        {
            var effectiveKind = _coreCardConversionsOperations.GetFactionSlashEffectiveSlashKind(pending, provider, card);
            var description = CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, CardKind.Slash) ? $"打出【{(CardCatalog.Get(effectiveKind).DisplayName)}】" : $"将【{(card.DisplayName)}】当作【杀】";
            return _coreCardConversionsOperations.CreateConversionChoiceVariants(provider, card, effectiveKind, isResponse, $"faction-slash.slash.card-{(card.Id)}", $"{(description)}，替 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 响应【{(GetFactionRequestDisplayName(pending))}】。", [card.Id], responseTargets, new Dictionary<string, string> { ["response"] = "faction-slash-slash", ["response-card-kind"] = effectiveKind.ToString(), ["skill"] = GetFactionRequestSkillId(pending) });
        }).Where(choice => !_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashChoiceForbidden(pending, choice)).ToList();
        foreach (var pair in _coreAssistedFactionSlashOperations.GetFactionRequestZhangbaPairs(pending, provider))
        {
            choices.Add(new PromptChoice(new ChoiceId($"faction-slash.zhangba-slash.cards-{(pair[0].Id)}-{(pair[1].Id)}"), $"发动【丈八蛇矛】，将两张手牌当【杀】替 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 响应【{(GetFactionRequestDisplayName(pending))}】。", [pair[0].Id, pair[1].Id], responseTargets, new Dictionary<string, string> { ["response"] = "zhangba-slash", ["response-card-kind"] = CardKind.Slash.ToString(), ["skill"] = GetFactionRequestSkillId(pending), ["equipment"] = CardKind.ZhangbaSerpentSpear.ToString() }));
        }

        foreach (var selection in _coreAssistedFactionSlashOperations.GetFactionRequestMultiCardSelections(pending, provider))
        {
            var cardIds = selection.Cards.Select(card => card.Id).ToArray();
            var parameters = new Dictionary<string, string>
            {
                ["response"] = "program-view-as-slash",
                ["response-card-kind"] = CardKind.Slash.ToString(),
                ["skill"] = GetFactionRequestSkillId(pending)
            };
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
            choices.Add(new PromptChoice(new ChoiceId($"faction-slash.program-view-as-slash.{(selection.Source.SkillId.Length)}:" + $"{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.cards-{(string.Join('-', cardIds))}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，将 {(cardIds.Length)} 张手牌当【杀】替 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 响应【{(GetFactionRequestDisplayName(pending))}】。", cardIds, responseTargets, parameters));
        }

        choices.Add(new PromptChoice(new ChoiceId("faction-slash.decline"), $"不响应【{(GetFactionRequestDisplayName(pending))}】。", [], responseTargets, new Dictionary<string, string> { ["response"] = "faction-slash-decline", ["skill"] = GetFactionRequestSkillId(pending) }));
        return choices;
    }

    internal void ResolveFactionSlashCandidateResponse(NativeOwners.FactionCardRequestHandle pending, bool useSlash, int? requestedCardId, CardKind? requestedEffectiveKind = null)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.AwaitingProviders || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } decision || decision.PlayerSeat != pending.CurrentCandidateSeat)
        {
            throw new InvalidOperationException("The FactionSlash response is not the current continuation.");
        }

        var provider = _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat];
        var selectedSlash = useSlash ? _coreCardConversionsOperations.GetFactionSlashSlashCards(pending, provider).FirstOrDefault(card => card.Id == requestedCardId) : null;
        if (useSlash && selectedSlash is null)
        {
            throw new InvalidOperationException("The FactionSlash responder has no matching Slash response card.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        if (selectedSlash is not null)
        {
            var baseEffectiveKind = _coreCardConversionsOperations.GetFactionSlashEffectiveSlashKind(pending, provider, selectedSlash);
            var effectiveKind = requestedEffectiveKind ?? baseEffectiveKind;
            if (effectiveKind != baseEffectiveKind)
            {
                throw new InvalidOperationException("A FactionSlash provider cannot choose the owner's weapon conversion.");
            }

            if (TryBeginFactionSlashZhuqueFanChoice(pending, provider, [selectedSlash], baseEffectiveKind))
            {
                return;
            }

            if (pending.IsBorrowedSwordUse)
            {
                BeginBorrowedSwordFactionSlashSlash(pending, provider, [selectedSlash], effectiveKind, usesZhuqueFan: false);
            }
            else if (pending.IsQinglongCrescentBladeUse)
            {
                BeginQinglongCrescentBladeFactionSlashSlash(pending, provider, [selectedSlash], effectiveKind, usesZhuqueFan: false);
            }
            else if (pending.IsProgramSkillUse)
            {
                BeginProvidedFactionSlashSlash(pending, provider, [selectedSlash], effectiveKind, usesZhuqueFan: false);
            }
            else
            {
                CompleteFactionSlashResponse(pending, provider, selectedSlash);
            }

            return;
        }

        _coreMatchLogOperations.AddLog("SkillSkipped", $"{(provider.Name)} 选择不响应 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 的【激将】。", provider.Seat, pending.OwnerSeat);
        pending.CandidateIndex++;
        AdvanceFactionSlashCandidate();
    }

    internal void ResolveFactionSlashZhangbaCandidateResponse(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> pair)
    {
        var providerReward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(pending);
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.AwaitingProviders || provider.Seat != pending.CurrentCandidateSeat || _coreNativeResponseMaterialQueriesOperations.FindZhangbaSlashPair(provider, pair.Select(card => card.Id).ToArray())is null)
        {
            throw new InvalidOperationException("The Zhangba FactionSlash response is not current.");
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        if (TryBeginFactionSlashZhuqueFanChoice(pending, provider, pair, CardKind.Slash))
        {
            return;
        }

        if (pending.IsBorrowedSwordUse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            BeginBorrowedSwordFactionSlashSlash(pending, provider, pair, CardKind.Slash, usesZhuqueFan: false);
            return;
        }

        if (pending.IsQinglongCrescentBladeUse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            BeginQinglongCrescentBladeFactionSlashSlash(pending, provider, pair, CardKind.Slash, usesZhuqueFan: false);
            return;
        }

        if (pending.IsProgramSkillUse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            BeginProvidedFactionSlashSlash(pending, provider, pair, CardKind.Slash, usesZhuqueFan: false);
            return;
        }

        var attack = pending.ResponseAttack ?? throw new InvalidOperationException("A response FactionSlash has no attack continuation.");
        var duel = pending.Purpose == FactionCardRequestPurpose.DuelResponse ? _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("A Duel FactionSlash response has no Duel continuation.") : null;
        var responseOpponentSeat = duel?.OpponentSeat ?? attack.SourceSeat;
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var responseCosts = _coreResponseCompletionZhangbaOperations.Value.CaptureZhangbaResponseCompletionCosts(provider, pair);
        _coreNativeGroupResponsesOperations.MoveZhangbaResponseCards(provider, pair, pending.ResolutionId, responseOpponentSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(pending.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, pair[0].Id, CardKind.Slash, IsActiveUse: false));
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(owner.Seat, CardKind.Slash);
        if (responseCosts is null)
        {
            _coreNativeGroupResponsesOperations.FinishZhangbaResponseCards(pair);
            _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
        }

        if (pending.Purpose == FactionCardRequestPurpose.DuelResponse)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel!.ResolutionId, owner.Seat, UsedSlash: true, SlashCardId: pair[0].Id, ResponseCardKind: CardKind.Slash));
            if (responseCosts is not null)
            {
                _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
                if (!_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, provider, owner.Seat, responseOpponentSeat, CardKind.Slash, responseCosts, ProgramCardContinuation.FactionSlashDuelSlash))
                    throw new InvalidOperationException("A completed faction Zhangba response lost its opted-in completion window.");
                return;
            }

            _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(duel, owner.Seat);
            return;
        }

        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("A group FactionSlash response has no group continuation.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, CardKind.Slash, owner.Seat, UsedResponse: true, ResponseCardId: pair[0].Id, ResponseCardKind: CardKind.Slash));
        if (responseCosts is not null)
        {
            _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
            if (!_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, provider, owner.Seat, responseOpponentSeat, CardKind.Slash, responseCosts, ProgramCardContinuation.FactionSlashGroupResponse))
                throw new InvalidOperationException("A completed faction Zhangba response lost its opted-in completion window.");
            return;
        }

        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void ResolveFactionSlashProgramMultiCardCandidateResponse(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, ProgramMultiCardViewAsSelection selection)
    {
        var providerReward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(pending);
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.AwaitingProviders || provider.Seat != pending.CurrentCandidateSeat || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.RoundDistinctBasicUse is not null || _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(provider, selection.Cards.Select(card => card.Id).ToArray(), CardKind.Slash, forResponse: !pending.IsProgramSkillUse && !pending.IsBorrowedSwordUse && !pending.IsQinglongCrescentBladeUse, selection.Source)is null)
        {
            throw new InvalidOperationException("The configured FactionSlash conversion is not current.");
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var conversion = selection.Source;
        if (pending.IsBorrowedSwordUse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            BeginBorrowedSwordFactionSlashSlash(pending, provider, selection.Cards, CardKind.Slash, usesZhuqueFan: false, conversionSource: conversion);
            return;
        }

        if (pending.IsQinglongCrescentBladeUse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            BeginQinglongCrescentBladeFactionSlashSlash(pending, provider, selection.Cards, CardKind.Slash, usesZhuqueFan: false, conversionSource: conversion);
            return;
        }

        if (pending.IsProgramSkillUse)
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            BeginProvidedFactionSlashSlash(pending, provider, selection.Cards, CardKind.Slash, usesZhuqueFan: false, conversionSource: conversion);
            return;
        }

        var attack = pending.ResponseAttack ?? throw new InvalidOperationException("A response FactionSlash has no attack continuation.");
        var duel = pending.Purpose == FactionCardRequestPurpose.DuelResponse ? _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("A Duel FactionSlash response has no Duel continuation.") : null;
        var responseOpponentSeat = duel?.OpponentSeat ?? attack.SourceSeat;
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var responseAction = _coreCardConversionsOperations.MoveProgramMultiCardResponse(provider, selection, pending.ResolutionId, responseOpponentSeat, owner.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(pending.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, selection.Cards[0].Id, CardKind.Slash, IsActiveUse: false));
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(owner.Seat, CardKind.Slash);
        if (!(_coreResponseCompletionOperations.HasCardResponseCompletedObserver(responseAction, ProgramCardContinuation.FactionSlashDuelSlash)))
        {
            _coreCardConversionsOperations.FinishProgramMultiCardResponse(selection);
            _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
        }

        if (pending.Purpose == FactionCardRequestPurpose.DuelResponse)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel!.ResolutionId, owner.Seat, UsedSlash: true, SlashCardId: selection.Cards[0].Id, ResponseCardKind: CardKind.Slash));
            if (_coreResponseCompletionOperations.HasCardResponseCompletedObserver(responseAction, ProgramCardContinuation.FactionSlashDuelSlash))
            {
                _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
                _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, responseAction, ProgramCardContinuation.FactionSlashDuelSlash);
                return;
            }

            _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(duel, owner.Seat);
            return;
        }

        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("A group FactionSlash response has no group continuation.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, CardKind.Slash, owner.Seat, UsedResponse: true, ResponseCardId: selection.Cards[0].Id, ResponseCardKind: CardKind.Slash));
        if (_coreResponseCompletionOperations.HasCardResponseCompletedObserver(responseAction, ProgramCardContinuation.FactionSlashGroupResponse))
        {
            _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, responseAction, ProgramCardContinuation.FactionSlashGroupResponse);
            return;
        }

        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void CompleteFactionSlashFailure(NativeOwners.FactionCardRequestHandle pending)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending))
        {
            throw new InvalidOperationException("The failed FactionSlash request is not current.");
        }

        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        if (!pending.IsProgramSkillUse || pending.IsAssistedProgramUse)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(pending.ResolutionId, pending.OwnerSeat, Succeeded: false, ProviderSeat: null, SlashCardId: null, EffectiveSlashKind: null, pending.IsBorrowedSwordUse || pending.IsQinglongCrescentBladeUse || pending.IsAssistedProgramUse, pending.TargetSeat));
        _coreMatchLogOperations.AddLog("SkillResolved", $"没有{(CoreMatchPresentationRuntime.GetFactionName(pending.ProviderFactionId))}势力角色响应 {(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat].Name)} 的【{(GetFactionRequestDisplayName(pending))}】。", pending.OwnerSeat, pending.TargetSeat);
        if (pending.IsBorrowedSwordUse)
        {
            var borrowedSword = pending.BorrowedSword ?? throw new InvalidOperationException("A Borrowed Sword FactionSlash failure has no parent resolution.");
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is ResponseWindowFrame response && response.ParentFrameId == borrowedSword.ResolutionId)
            {
                _coreNativeResponseWindowsOperations.PopResponseWindow(borrowedSword.ResolutionId);
            }

            _coreNativeBorrowedSwordOperations.BeginBorrowedSwordSlashChoice(borrowedSword, includeFactionSlash: false);
            return;
        }

        if (pending.IsQinglongCrescentBladeUse)
        {
            var qinglong = pending.QinglongCrescentBlade ?? throw new InvalidOperationException("A Qinglong Crescent Blade FactionSlash failure has no parent resolution.");
            _coreNativeQinglongOperations.PublishQinglongCrescentBladeChoice(qinglong, includeFactionSlash: false);
            return;
        }

        if (!pending.IsProgramSkillUse)
        {
            PublishOwnerSlashDecision(pending);
            return;
        }

        var frameId = pending.ProgramSkillFrameId ?? throw new InvalidOperationException("A faction-card request has no program-skill frame.");
        if (pending.IsAssistedProgramUse)
        {
            if (_characterLvKuangLvXiangOlOperations.FinishQigongFactionDeclined(frameId, pending))
                return;
            if (_characterCaoChunOlOperations.FinishCaoChunFactionDeclined(frameId, pending))
                return;
            if (_characterJiangWeiMouMobileOperations.FinishMouJwFactionDeclined(frameId, pending))
                return;
            if (_characterMaTieAnniversaryOperations.FinishAnniversaryMaTieFactionDeclined(frameId, pending))
                return;
            if (_characterWuJingOlOperations.FinishWuJingFactionDeclined(frameId, pending))
                return;
            _coreProgramSlashRequestOperations.CommitProgramChoiceResult(frameId, pending.AssistedResultBind!, "declined", pending.OwnerSeat, "没有角色提供【杀】。");
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frameId);
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
    }

    internal void PublishOwnerSlashDecision(NativeOwners.FactionCardRequestHandle failedRequest)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[failedRequest.OwnerSeat];
        var slashes = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(owner, CardKind.Slash);
        var incomingCard = failedRequest.Purpose switch
        {
            FactionCardRequestPurpose.DuelResponse => CardKind.Duel,
            FactionCardRequestPurpose.GroupResponse => _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.Card.Kind ?? CardKind.BarbarianAssault,
            _ => throw new InvalidOperationException("Only response FactionSlash can restore an owner response prompt.")};
        var sourceSeat = failedRequest.Purpose == FactionCardRequestPurpose.DuelResponse ? _coreNativeCardContinuationCursorsOperations.ActiveDuel?.OpponentSeat : _coreNativeCardContinuationCursorsOperations.ActiveGroupCard?.SourceSeat;
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondSlash, owner.Seat, "【激将】无人响应，是否由你自己打出【杀】？", slashes.Select(card => card.Id).ToArray(), [], sourceSeat, incomingCard)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            RequiredCardKind = CardKind.Slash,
            Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(slashes, CardKind.Slash, incomingCard, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(owner, card, CardKind.Slash), includeFactionSlash: false, responder: owner)
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void CompleteFactionSlashResponse(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, Card selectedSlash)
    {
        if (_coreCardDeclarationsOperations.TryBeginCardDeclaration(provider, selectedSlash, CardKind.Slash, _coreCardDeclarationsOperations.PeekDeclarationConversion(provider, selectedSlash, CardKind.Slash, false), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.FactionSlash, pending.OwnerSeat), [pending.ResponseAttack!.SourceSeat]))
            return;
        var providerReward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(pending);
        var attack = pending.ResponseAttack ?? throw new InvalidOperationException("A response FactionSlash has no attack continuation.");
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || provider.Seat != pending.CurrentCandidateSeat)
        {
            throw new InvalidOperationException("The completed FactionSlash response is not current.");
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var effectiveKind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(provider, selectedSlash, CardKind.Slash);
        var responseConversion = _coreCardConversionsOperations.GetSelectedResponseConversion(provider, selectedSlash, effectiveKind);
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(provider, selectedSlash, effectiveKind, responseConversion);
        var responseFrom = _characterMaChaoAnniversaryGodOperations.FindOwnedCardLocation(provider, selectedSlash, responseConversion);
        var completedResponseUseSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(provider, [selectedSlash], effectiveKind);
        var responseCostIsRed = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(provider.Seat, selectedSlash);
        var duel = pending.Purpose == FactionCardRequestPurpose.DuelResponse ? _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("A Duel FactionSlash response has no Duel continuation.") : null;
        var responseOpponentSeat = duel?.OpponentSeat ?? attack.SourceSeat;
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(provider, effectiveKind, [selectedSlash]);
        _corePhysicalMovementFacadesOperations.MoveCard(selectedSlash, responseFrom, CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
        _coreMatchLogOperations.AddLog("CardResponded", $"{(provider.Name)} 响应【激将】，替 {(owner.Name)} 打出【{(CardCatalog.Get(effectiveKind).DisplayName)}】。", provider.Seat, owner.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(pending.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, selectedSlash.Id, effectiveKind, IsActiveUse: false));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(selectedSlash.Id, owner.Seat, responseOpponentSeat, effectiveKind));
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(owner.Seat, effectiveKind);
        _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
        if (pending.Purpose == FactionCardRequestPurpose.DuelResponse)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel!.ResolutionId, owner.Seat, UsedSlash: true, SlashCardId: selectedSlash.Id, ResponseCardKind: effectiveKind));
            var responseCost = new CardActionCost(selectedSlash.Id, selectedSlash.Kind, responseFrom, responseCostIsRed);
            if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, provider, requesterSeat: owner.Seat, responseOpponentSeat, effectiveKind, [responseCost], ProgramCardContinuation.FactionSlashDuelSlash, responseConversion, completedResponseUseSuit))
            {
                return;
            }

            _corePhysicalMovementFacadesOperations.MoveCard(selectedSlash, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
            _coreNativeDuelOperations.ContinueDuelAfterSuccessfulSlash(duel, owner.Seat);
            return;
        }

        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("A group FactionSlash response has no group continuation.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, CardKind.Slash, owner.Seat, UsedResponse: true, ResponseCardId: selectedSlash.Id, ResponseCardKind: effectiveKind));
        var groupResponseCost = new CardActionCost(selectedSlash.Id, selectedSlash.Kind, responseFrom, responseCostIsRed);
        if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, owner, provider, requesterSeat: owner.Seat, responseOpponentSeat, effectiveKind, [groupResponseCost], ProgramCardContinuation.FactionSlashGroupResponse, responseConversion))
        {
            return;
        }

        _corePhysicalMovementFacadesOperations.MoveCard(selectedSlash, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void BeginProvidedFactionSlashSlash(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind effectiveKind, bool usesZhuqueFan, CardConversionSource? conversionSource = null)
    {
        var actualConversion = conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        if (_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, effectiveKind, physicalCards, actualConversion))
            throw new InvalidOperationException("The supplied Slash violates its actual user's current next-use suit restriction.");
        if (physicalCards.Count == 1 && _coreCardDeclarationsOperations.TryBeginCardDeclaration(provider, physicalCards[0], effectiveKind, _coreCardDeclarationsOperations.PeekDeclarationConversion(provider, physicalCards[0], effectiveKind, true, conversionSource), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.ProvidedSlash, pending.OwnerSeat, fan: usesZhuqueFan, finalKind: effectiveKind), [pending.TargetSeat!.Value]))
            return;
        var providerReward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(pending);
        var selectedSlash = physicalCards.FirstOrDefault() ?? throw new InvalidOperationException("An active FactionSlash Slash must retain a physical card.");
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat ?? throw new InvalidOperationException("An active FactionSlash use has no target.")];
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.IsProgramSkillUse || (pending.IsAssistedProgramUse ? !_coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(owner.Seat, target.Seat, effectiveKind, _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, conversionSource is null && physicalCards.Count == 2, effectiveKind), allowPotentialRank: false, physicalCardIds: physicalCards.Select(c => c.Id).ToArray()) : !_coreNativeEquipmentAndFactionQueriesOperations.CanUseProvidedSlashTarget(owner, target, effectiveKind, physicalSuit: _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, physicalCards), allowAnyPhysicalSuit: false, effectiveColor: _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, physicalCards), effectiveRank: _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, conversionSource is null && physicalCards.Count == 2, effectiveKind), physicalCardIds: physicalCards.Select(c => c.Id).ToArray())))
        {
            throw new InvalidOperationException("The active FactionSlash target is no longer legal.");
        }

        if (!_coreRuleQueriesOperations.CanSpendSlashUse(owner, target, ignoresCount: pending.IsAssistedProgramUse, effectiveKind, effectiveRank: _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, conversionSource is null && physicalCards.Count == 2, effectiveKind), physicalCardIds: physicalCards.Select(c => c.Id).ToArray()))
        {
            throw new InvalidOperationException("The FactionSlash Slash limit has already been reached.");
        }

        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileProvidedSlash(pending, provider, physicalCards, actualConversion);
        pending.AwaitingProviders = false;
        var frameId = pending.ProgramSkillFrameId ?? throw new InvalidOperationException("A faction-card request has no program-skill frame.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId)
            throw new InvalidOperationException("A faction-card request lost its program-skill parent.");
        if (pending.IsAssistedProgramUse)
        {
            _coreAssistedFactionSlashOperations.ValidateAssistedFactionSlashParent(pending, frame);
            if (_characterSunYuanOlOperations.IsSayingSlashRequest(frame))
                _characterSunYuanOlOperations.PrepareSayingSuppliedSlash(frame, owner.Seat, provider.Seat, target.Seat, effectiveKind, physicalCards, actualConversion);
            else if (_characterLiYiOlOperations.IsChanshuangSlashRequest(frame))
                _characterLiYiOlOperations.PrepareChanshuangSuppliedSlash(frame, owner.Seat, provider.Seat, target.Seat, effectiveKind, physicalCards, conversionSource);
            else if (!_characterJiangWeiMouMobileOperations.PrepareMouJwFactionNative(frame, pending, provider, physicalCards, effectiveKind, actualConversion) && !_characterMaTieAnniversaryOperations.PrepareAnniversaryMaTieFactionNative(frame, pending, provider, physicalCards, effectiveKind, actualConversion) && !_characterLvKuangLvXiangOlOperations.PrepareQigongFactionNative(frame, pending, provider, physicalCards, effectiveKind) && !_characterCaoChunOlOperations.PrepareCaoChunFactionNative(frame, pending, provider, physicalCards, effectiveKind) && !_characterWuJingOlOperations.PrepareWuJingFactionNative(frame, pending, provider, physicalCards, effectiveKind))
                _coreProgramSlashRequestOperations.CommitProgramChoiceResult(frameId, pending.AssistedResultBind!, "used-slash", owner.Seat, "通过势力响应使用【杀】。");
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(pending.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, selectedSlash.Id, effectiveKind, IsActiveUse: true, target.Seat));
        }

        _coreMatchLogOperations.AddLog("CardUsed", $"{(provider.Name)} 响应【{(GetFactionRequestDisplayName(pending))}】，由 {(owner.Name)} 对 {(target.Name)} 使用【{(CardCatalog.Get(effectiveKind).DisplayName)}】。", owner.Seat, target.Seat);
        _coreNativeSlashUseOperations.ResolveSlashCore(owner, target, _characterCuiYanOlOperations.HasCuiYanCapability && physicalCards.Count == 1 ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, selectedSlash, actualConversion) : selectedSlash, effectiveKind, provider.Seat, pending, physicalCards: physicalCards, countsTowardSlashLimit: !pending.IsAssistedProgramUse, usesZhuqueFan: usesZhuqueFan, conversionSource: conversionSource);
        _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
    }

    internal void BeginBorrowedSwordFactionSlashSlash(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind effectiveKind, bool usesZhuqueFan, CardConversionSource? conversionSource = null)
    {
        var providerReward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(pending);
        var selectedSlash = physicalCards.FirstOrDefault() ?? throw new InvalidOperationException("A Borrowed Sword FactionSlash Slash must retain a physical card.");
        var actualConversion = conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        var borrowedSword = pending.BorrowedSword ?? throw new InvalidOperationException("A Borrowed Sword FactionSlash use has no parent resolution.");
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat ?? throw new InvalidOperationException("A Borrowed Sword FactionSlash use has no target.")];
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword, borrowedSword) || !pending.IsBorrowedSwordUse || !_coreNativeBorrowedSwordOperations.IsLegalBorrowedSwordSlashTarget(owner, target, effectiveKind, physicalSuit: _coreCurrentTurnShownCardBenefitsOperations.HasSlashUseDistanceBySuit(owner, effectiveKind, Suit.Diamond) ? _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, physicalCards) : null, allowAnyPhysicalSuit: false, effectiveColor: _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, physicalCards), effectiveRank: _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, conversionSource is null && physicalCards.Count == 2, effectiveKind), physicalCardIds: _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.RoundGainedOtherSourceUse) ? physicalCards.Select(c => c.Id).ToArray() : null))
        {
            throw new InvalidOperationException("The Borrowed Sword FactionSlash target is no longer legal.");
        }

        if (physicalCards.Count == 1 && _coreCardDeclarationsOperations.TryBeginCardDeclaration(provider, selectedSlash, effectiveKind, _coreCardDeclarationsOperations.PeekDeclarationConversion(provider, selectedSlash, effectiveKind, true, conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.BorrowedSwordProvidedSlash, pending.OwnerSeat, fan: usesZhuqueFan, finalKind: effectiveKind), [target.Seat]))
            return;
        if (_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, effectiveKind, physicalCards, actualConversion))
            throw new InvalidOperationException("The supplied Borrowed Sword Slash violates its actual user's current next-use suit restriction.");
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileProvidedSlash(pending, provider, physicalCards, actualConversion);
        pending.AwaitingProviders = false;
        borrowedSword.AwaitingSlashChoice = false;
        borrowedSword.SlashCardId = selectedSlash.Id;
        borrowedSword.EffectiveSlashKind = effectiveKind;
        _coreNativeResponseWindowsOperations.PopResponseWindow(borrowedSword.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(borrowedSword.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(borrowedSword.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, selectedSlash.Id, effectiveKind, IsActiveUse: true, target.Seat));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(provider.Name)} 响应【激将】，由 {(owner.Name)} 按【借刀杀人】要求对 {(target.Name)} 使用【{(CardCatalog.Get(effectiveKind).DisplayName)}】。", owner.Seat, target.Seat);
        _coreNativeSlashUseOperations.ResolveSlashCore(owner, target, _characterCuiYanOlOperations.HasCuiYanCapability && physicalCards.Count == 1 ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, selectedSlash, actualConversion) : selectedSlash, effectiveKind, provider.Seat, factionRequest: pending, borrowedSword: borrowedSword, physicalCards: physicalCards, usesZhuqueFan: usesZhuqueFan, conversionSource: conversionSource);
        _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
    }

    internal void BeginQinglongCrescentBladeFactionSlashSlash(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind effectiveKind, bool usesZhuqueFan, CardConversionSource? conversionSource = null)
    {
        var providerReward = _coreFactionRequestPaymentsOperations.CaptureFactionProviderReward(pending);
        var selectedSlash = physicalCards.FirstOrDefault() ?? throw new InvalidOperationException("A Qinglong Crescent Blade FactionSlash Slash must retain a physical card.");
        var actualConversion = conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
        var qinglong = pending.QinglongCrescentBlade ?? throw new InvalidOperationException("A Qinglong Crescent Blade FactionSlash use has no parent resolution.");
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat ?? throw new InvalidOperationException("A Qinglong Crescent Blade FactionSlash use has no target.")];
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade, qinglong) || !pending.IsQinglongCrescentBladeUse || !_coreNativeQinglongOperations.CanUseQinglongCrescentBladeTarget(owner, target, effectiveKind, _coreTurnRedSlashBenefitsOperations.PhysicalGroupColor(owner, physicalCards), allowAnyColor: false, physicalSuit: _coreAlternatingSuitAndCompletedTopOperations.PhysicalGroupSuit(owner, physicalCards), effectiveRank: _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, conversionSource is null && physicalCards.Count == 2, effectiveKind), physicalCardIds: physicalCards.Select(c => c.Id).ToArray()))
        {
            throw new InvalidOperationException("The Qinglong Crescent Blade FactionSlash target is no longer legal.");
        }

        if (usesZhuqueFan)
        {
            throw new InvalidOperationException("Qinglong Crescent Blade cannot simultaneously provide Zhuque Fan conversion.");
        }

        if (physicalCards.Count == 1 && _coreCardDeclarationsOperations.TryBeginCardDeclaration(provider, selectedSlash, effectiveKind, _coreCardDeclarationsOperations.PeekDeclarationConversion(provider, selectedSlash, effectiveKind, true, conversionSource ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.QinglongProvidedSlash, pending.OwnerSeat, fan: usesZhuqueFan, finalKind: effectiveKind), [target.Seat]))
            return;
        if (_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, effectiveKind, physicalCards, actualConversion))
            throw new InvalidOperationException("The supplied Qinglong Slash violates its actual user's current next-use suit restriction.");
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileProvidedSlash(pending, provider, physicalCards, actualConversion);
        pending.AwaitingProviders = false;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashResolvedEvent(pending.ResolutionId, owner.Seat, Succeeded: true, provider.Seat, selectedSlash.Id, effectiveKind, IsActiveUse: true, target.Seat));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(provider.Name)} 响应【激将】，由 {(owner.Name)} 发动【青龙偃月刀】对 {(target.Name)} 使用【{(CardCatalog.Get(effectiveKind).DisplayName)}】。", owner.Seat, target.Seat);
        var deferEquipmentReward = _characterHuaTuoAnniversaryGodOperations.HasSkillInvocationObservers;
        _coreNativeQinglongOperations.BeginQinglongCrescentBladeFollowup(qinglong, _characterCuiYanOlOperations.HasCuiYanCapability && physicalCards.Count == 1 ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, selectedSlash, actualConversion) : selectedSlash, effectiveKind, provider.Seat, physicalCards, pending, conversionSource);
        if (deferEquipmentReward)
            return;
        _coreFactionRequestPaymentsOperations.RewardFactionRequestProvider(providerReward, provider.Seat);
    }

    internal bool TryBeginFactionSlashZhuqueFanChoice(NativeOwners.FactionCardRequestHandle pending, CharacterState provider, IReadOnlyList<Card> physicalCards, CardKind baseEffectiveKind)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        if ((!pending.IsProgramSkillUse && !pending.IsBorrowedSwordUse) || baseEffectiveKind != CardKind.Slash || (physicalCards.Count == 1 && physicalCards[0].Kind != CardKind.Slash && !_characterGuanYuWumiaoOperations.IsWuyouIdentityKind(provider, physicalCards[0], CardKind.Slash) && !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(provider, physicalCards[0])) || !_coreNativeTargetLegalityOperations.HasZhuqueFan(owner))
        {
            return false;
        }

        if (_characterCuiYanOlOperations.HasCuiYanCapability && physicalCards.Count == 1)
        {
            var conversion = _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedUseConversion ?? _coreGameEngineCardConversionsSelectedResponseConversionSelectedUseConversionAccess._selectedResponseConversion;
            var normalAllowed = !_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, CardKind.Slash, physicalCards, conversion);
            var fireAllowed = !_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, CardKind.FireSlash, physicalCards, conversion);
            if (pending.IsAssistedProgramUse && pending.TargetSeat is { } exactTarget)
            {
                normalAllowed &= _coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(owner.Seat, exactTarget, CardKind.Slash, _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, false), allowPotentialRank: false, physicalCardIds: physicalCards.Select(c => c.Id).ToArray());
                fireAllowed &= _coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(owner.Seat, exactTarget, CardKind.FireSlash, _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, false, CardKind.FireSlash), allowPotentialRank: false, physicalCardIds: physicalCards.Select(c => c.Id).ToArray());
            }

            if (!normalAllowed || !fireAllowed)
            {
                if (!normalAllowed && !fireAllowed)
                    throw new InvalidOperationException("The supplied Slash has no legal native suit after the requester's fan conversion.");
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                var kind = fireAllowed ? CardKind.FireSlash : CardKind.Slash;
                if (pending.IsBorrowedSwordUse)
                    BeginBorrowedSwordFactionSlashSlash(pending, provider, physicalCards, kind, usesZhuqueFan: fireAllowed);
                else
                    BeginProvidedFactionSlashSlash(pending, provider, physicalCards, kind, usesZhuqueFan: fireAllowed);
                return true;
            }
        }

        if (pending.IsAssistedProgramUse && pending.TargetSeat is { } assistedTarget)
        {
            var normalAllowed = _coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(owner.Seat, assistedTarget, CardKind.Slash, _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, physicalCards.Count == 2), allowPotentialRank: false, physicalCardIds: physicalCards.Select(c => c.Id).ToArray());
            var fireAllowed = _coreAssistedFactionSlashOperations.IsAssistedProvidedSlashTarget(owner.Seat, assistedTarget, CardKind.FireSlash, _coreFinalTargetSlashOperations.ProvidedSpecificSlashRank(owner, provider, physicalCards, physicalCards.Count == 2, CardKind.FireSlash), allowPotentialRank: false, physicalCardIds: physicalCards.Select(c => c.Id).ToArray());
            if (normalAllowed != fireAllowed)
            {
                _coreNativePromptBoundaryOperations.ClearPendingDecision();
                BeginProvidedFactionSlashSlash(pending, provider, physicalCards, fireAllowed ? CardKind.FireSlash : CardKind.Slash, usesZhuqueFan: fireAllowed);
                return true;
            }
        }

        if (!pending.AwaitingProviders || pending.AwaitingZhuqueFanChoice || physicalCards.Count == 0 || provider.Seat != pending.CurrentCandidateSeat)
        {
            throw new InvalidOperationException("The FactionSlash Zhuque Fan choice cannot start from this continuation.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        pending.AwaitingProviders = false;
        pending.AwaitingZhuqueFanChoice = true;
        pending.ZhuqueFanProviderSeat = provider.Seat;
        pending.ZhuqueFanPhysicalCards = Array.AsReadOnly(physicalCards.ToArray());
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ZhuqueFan, owner.Seat, $"{(provider.Name)} 已为【激将】提供普通【杀】，是否发动【朱雀羽扇】改为【火杀】？", [], [], SourceSeat: provider.Seat, IncomingCard: CardKind.Slash)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = pending.TargetSeat,
            Choices = [new PromptChoice(new ChoiceId("zhuque-fan.normal"), "保持为普通【杀】。", [], [], new Dictionary<string, string> { ["action"] = "zhuque-fan-normal", ["equipment"] = CardKind.ZhuqueFan.ToString() }), new PromptChoice(new ChoiceId("zhuque-fan.fire"), "发动【朱雀羽扇】，将此【杀】改为【火杀】。", [], [], new Dictionary<string, string> { ["action"] = "zhuque-fan-fire", ["equipment"] = CardKind.ZhuqueFan.ToString() })]
        };
        _coreMatchOutcomeStatusAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return true;
    }

    internal void ResolveFactionSlashZhuqueFanChoice(NativeOwners.FactionCardRequestHandle pending, bool convertToFireSlash)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.AwaitingZhuqueFanChoice || pending.AwaitingProviders || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ZhuqueFan } decision || decision.PlayerSeat != pending.OwnerSeat || pending.ZhuqueFanProviderSeat is not { } providerSeat || pending.ZhuqueFanPhysicalCards.Count == 0)
        {
            throw new InvalidOperationException("The FactionSlash Zhuque Fan choice is not current.");
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat];
        if (convertToFireSlash && !_coreNativeTargetLegalityOperations.HasZhuqueFan(owner))
        {
            throw new InvalidOperationException("Zhuque Fan is no longer available for this conversion.");
        }

        var provider = _coreMatchParticipantsPlayersAccess._players[providerSeat];
        var physicalCards = pending.ZhuqueFanPhysicalCards;
        pending.AwaitingZhuqueFanChoice = false;
        pending.ZhuqueFanProviderSeat = null;
        pending.ZhuqueFanPhysicalCards = [];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        var effectiveKind = convertToFireSlash ? CardKind.FireSlash : CardKind.Slash;
        if (pending.IsBorrowedSwordUse)
        {
            BeginBorrowedSwordFactionSlashSlash(pending, provider, physicalCards, effectiveKind, convertToFireSlash);
            return;
        }

        if (!pending.IsProgramSkillUse)
        {
            throw new InvalidOperationException("Only an active FactionSlash Slash use can resume from Zhuque Fan.");
        }

        BeginProvidedFactionSlashSlash(pending, provider, physicalCards, effectiveKind, convertToFireSlash);
    }

    internal void CompleteQinglongCrescentBladeFactionSlash(NativeOwners.FactionCardRequestHandle pending)
    {
        if (!CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest, pending) || !pending.IsQinglongCrescentBladeUse || pending.AwaitingProviders)
        {
            throw new InvalidOperationException("The completed Qinglong Crescent Blade FactionSlash continuation is invalid.");
        }

        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = null;
    }
}
