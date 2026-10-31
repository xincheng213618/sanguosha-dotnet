#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineAssistedFactionSlashRuntime
{
    internal IReadOnlyList<GameEngine.AssistedFactionSlashCard> GetAssistedFactionSlashCards(NativeOwners.FactionCardRequestHandle pending, CharacterState provider)
    {
        var variants = new List<GameEngine.AssistedFactionSlashCard>();
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(provider).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(provider)).Concat(_coreOwnPublicPileConversionsOperations.GetOwnPublicPileConversionCards(provider)).Concat(_coreLastZoneConversionsOperations.GetLastZoneJudgmentConversionCards(provider, forResponse: false)).DistinctBy(card => card.Id))
        {
            if (_coreCardUseModulesOperations.IsTurnHandCardRestricted(provider, card))
                continue;
            var hasIdentity = _coreCardConversionsOperations.HasProgramCardIdentity(provider, card);
            foreach (var kind in CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds)
            {
                if (!CanSupplyAssistedFactionSlash(pending, kind, [card]))
                    continue;
                if (!hasIdentity && !_coreLastZoneConversionsOperations.IsLastZoneJudgmentCard(provider, card) && !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(provider, card) && card.Kind == kind && _coreCuiYanNativeQueriesOperations.CanSupplyCuiYanFactionSlashVariant(pending, provider, kind, [card]))
                    variants.Add(new(card, kind, null));
                var sources = hasIdentity ? _coreCardConversionsOperations.GetProgramCardIdentitySources(provider, card, kind, forResponse: false) : _coreActionQueriesOperations.GetProgramViewAsConversions(provider, card, kind, forResponse: false);
                variants.AddRange(sources.Where(source => _coreCuiYanNativeQueriesOperations.CanSupplyCuiYanFactionSlashVariant(pending, provider, kind, [card], source)).Select(source => new GameEngine.AssistedFactionSlashCard(card, kind, source)));
            }
        }

        return variants;
    }

    internal IReadOnlyList<PromptChoice> CreateAssistedFactionSlashChoices(CharacterState provider, NativeOwners.FactionCardRequestHandle pending)
    {
        var targets = new[]
        {
            pending.TargetSeat!.Value
        };
        Dictionary<string, string> Parameters(string response, CardKind kind) => new()
        {
            ["response"] = response,
            ["response-card-kind"] = kind.ToString(),
            ["skill"] = _coreNativeFactionSlashOperations.GetFactionRequestSkillId(pending)
        };
        var choices = new List<PromptChoice>();
        foreach (var variant in GetAssistedFactionSlashCards(pending, provider))
        {
            var parameters = Parameters("faction-slash-slash", variant.Kind);
            if (variant.Source is { } source)
                CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
            var sourceId = variant.Source is { } conversion ? $"{(conversion.SkillId)}.{(conversion.BindingId)}.{(conversion.SkillInstanceId)}" : "native";
            choices.Add(new(new ChoiceId($"assisted-faction.{(variant.Kind)}.{(variant.Card.Id)}.{(sourceId)}"), $"打出【{(CardCatalog.Get(variant.Kind).DisplayName)}】，响应【{(_coreNativeFactionSlashOperations.GetFactionRequestDisplayName(pending))}】。", [variant.Card.Id], targets, parameters));
        }

        foreach (var pair in GetFactionRequestZhangbaPairs(pending, provider))
        {
            var parameters = Parameters("zhangba-slash", CardKind.Slash);
            parameters["equipment"] = CardKind.ZhangbaSerpentSpear.ToString();
            choices.Add(new(new ChoiceId($"assisted-faction.zhangba.{(pair[0].Id)}.{(pair[1].Id)}"), $"发动丈八蛇矛，打出两张手牌响应【{(_coreNativeFactionSlashOperations.GetFactionRequestDisplayName(pending))}】。", pair.Select(card => card.Id).ToArray(), targets, parameters));
        }

        foreach (var selection in GetFactionRequestMultiCardSelections(pending, provider))
        {
            var parameters = Parameters("program-view-as-slash", selection.OutputKind);
            CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
            var ids = selection.Cards.Select(card => card.Id).ToArray();
            choices.Add(new(new ChoiceId($"assisted-faction.multi.{(selection.OutputKind)}.{(selection.Source.SkillId)}.{(selection.Source.BindingId)}.{(string.Join('-', ids))}"), $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，打出【{(CardCatalog.Get(selection.OutputKind).DisplayName)}】响应【{(_coreNativeFactionSlashOperations.GetFactionRequestDisplayName(pending))}】。", ids, targets, parameters));
        }

        choices.Add(new(new ChoiceId("faction-slash.decline"), $"不响应【{(_coreNativeFactionSlashOperations.GetFactionRequestDisplayName(pending))}】。", [], targets, new Dictionary<string, string> { ["response"] = "faction-slash-decline", ["skill"] = _coreNativeFactionSlashOperations.GetFactionRequestSkillId(pending) }));
        return choices;
    }

    internal void ResolveAssistedFactionSlashChoice(PromptChoice selected, bool advanceToHumanBoundary)
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest;
        if (pending is not { IsAssistedProgramUse: true, AwaitingProviders: true } || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } prompt || prompt.PlayerSeat != pending.CurrentCandidateSeat)
            throw new InvalidOperationException("The assisted faction response lost its provider continuation.");
        var provider = _coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat];
        var canonical = CreateAssistedFactionSlashChoices(provider, pending).SingleOrDefault(choice => choice.Id == selected.Id);
        if (canonical is null || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], [canonical]))
            throw new InvalidOperationException("The assisted faction response changed its cards, kind or conversion source.");
        var response = canonical.Parameters["response"];
        if (response == "faction-slash-decline")
            _coreNativeFactionSlashOperations.ResolveFactionSlashCandidateResponse(pending, false, null);
        else if (response == "zhangba-slash")
            _coreNativeFactionSlashOperations.ResolveFactionSlashZhangbaCandidateResponse(pending, provider, _coreNativeResponseMaterialQueriesOperations.FindZhangbaSlashPair(provider, canonical.Cards)!);
        else
        {
            var kind = CoreNativeCardClassificationsRuntime.ReadResponseCardKind(canonical) ?? throw new InvalidOperationException("The assisted faction response lost its Slash kind.");
            IReadOnlyList<Card> cards;
            CardConversionSource? conversion;
            if (response == "program-view-as-slash")
            {
                conversion = CoreGameEngineCardConversionsRuntime.RequireConversionSource(canonical);
                cards = GetFactionRequestMultiCardSelections(pending, provider).Single(selection => selection.OutputKind == kind && selection.Source == conversion && selection.Cards.Select(card => card.Id).SequenceEqual(canonical.Cards)).Cards;
            }
            else
            {
                _coreCardConversionsOperations.CaptureSelectedResponseConversion(canonical);
                var variant = GetAssistedFactionSlashCards(pending, provider).Single(item => item.Card.Id == canonical.Cards.Single() && item.Kind == kind && item.Source == (CoreGameEngineCardConversionsRuntime.TryReadConversionSource(canonical.Parameters, out var source) ? source : null));
                cards = [variant.Card];
                if (_coreNativeFactionSlashOperations.TryBeginFactionSlashZhuqueFanChoice(pending, provider, cards, kind))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                    if (advanceToHumanBoundary)
                        _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
                    return;
                }

                conversion = variant.Source;
            }

            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            if (_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, kind, cards, conversion))
                throw new InvalidOperationException("The supplied Slash violates its actual user's current next-use suit restriction.");
            _coreNativeFactionSlashOperations.BeginProvidedFactionSlashSlash(pending, provider, cards, kind, usesZhuqueFan: false, conversionSource: conversion);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal bool CanSupplyAssistedFactionSlash(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) => _coreTurnRedSlashBenefitsOperations.IsRedSlashProviderPaymentLegal(pending, kind, cards, isTrueZhangba) && _characterJiangWeiMouMobileOperations.MouJwFactionSupplyLegal(pending, kind) && _characterMaTieAnniversaryOperations.AnniversaryMaTieFactionSupplyLegal(pending, kind) && (!CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending) || !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(pending.OwnerSeat, cards.Select(c => c.Id).ToArray())) && (!pending.IsAssistedProgramUse || _coreFinalTargetSlashOperations.HasRankSlashRange(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat], kind) || pending.TargetSeat is { } target && (IsAssistedProvidedSlashTarget(pending.OwnerSeat, target, kind, allowPotentialRank: false, physicalCardIds: cards.Select(c => c.Id).ToArray()) || kind == CardKind.Slash && _coreNativeTargetLegalityOperations.HasZhuqueFan(_coreMatchParticipantsPlayersAccess._players[pending.OwnerSeat]) && (cards.Count > 1 || cards is [var primary] && (primary.Kind == CardKind.Slash || _characterGuanYuWumiaoOperations.IsWuyouIdentityKind(_coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat], primary, CardKind.Slash) || _coreOwnPublicPileConversionsOperations.IsOwnPublicPileSlashMaterial(_coreMatchParticipantsPlayersAccess._players[pending.CurrentCandidateSeat], primary))) && IsAssistedProvidedSlashTarget(pending.OwnerSeat, target, CardKind.FireSlash, allowPotentialRank: false, physicalCardIds: cards.Select(c => c.Id).ToArray())));
    internal IReadOnlyList<IReadOnlyList<Card>> GetFactionRequestZhangbaPairs(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(provider).Where(pair => CanSupplyAssistedFactionSlash(pending, CardKind.Slash, pair, isTrueZhangba: true)).ToArray();
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => (pending.IsAssistedProgramUse ? CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.SelectMany(kind => _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, kind, false, ignoreSuitUseProhibition: true).Where(item => _corePlayCardHistoryOperations.ViewAsRule(item.Source)is { RoundDistinctBasicUse: null, LeiluanBasicUse: null } || item.OutputKind == kind)) : _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, pending.RequiredKind, !CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending), ignoreSuitUseProhibition: true)).Where(selection => _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.RoundDistinctBasicUse is null && _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.LeiluanBasicUse is null && CanSupplyAssistedFactionSlash(pending, selection.OutputKind, selection.Cards) && !_coreCuiYanNativeQueriesOperations.IsCuiYanFactionSlashUseForbidden(pending, selection.OutputKind, selection.Cards, selection.Source)).ToArray();
    internal bool CanRequestAssistedProgramFactionSlash(int actorSeat, int targetSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash)is not null && CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind => IsAssistedProvidedSlashTarget(actorSeat, targetSeat, kind));
    internal bool HasQigongFactionDistance(int actorSeat, int targetSeat) => _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsAssistedProgramUse: true, ProgramSkillFrameId: { } frameId } request && request.OwnerSeat == actorSeat && request.TargetSeat == targetSeat && _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == frameId)is ProgramSkillFrame program && _characterLvKuangLvXiangOlOperations.QigongAdapterActor(program, actorSeat, targetSeat, program.InstructionIndex, frameId, request.ResolutionId) && _characterLvKuangLvXiangOlOperations.ValidateQigongFactionSlashParent(request, program);
    internal bool IsAssistedProvidedSlashTarget(int actorSeat, int targetSeat, CardKind kind, int? effectiveRank = null, bool allowPotentialRank = true, IReadOnlyList<int>? physicalCardIds = null) => _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && actorSeat != targetSeat && (_characterHuangfuSongWumiaoOperations.HasWuHfOrWuyouMaterialPrivilege(_coreMatchParticipantsPlayersAccess._players[actorSeat], kind, physicalCardIds) || allowPotentialRank && physicalCardIds is null && _characterGuanYuWumiaoOperations.HasPotentialWuyouFactionSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], kind) || (HasQigongFactionDistance(actorSeat, targetSeat) || _characterWuJingOlOperations.HasWuJingFactionDistance(actorSeat, targetSeat) || _characterJiangWeiMouMobileOperations.HasMouJwFactionDistance(actorSeat, targetSeat) ? true : _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsAssistedProgramUse: true, ProgramSkillFrameId: { } fixedFrameId } && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == fixedFrameId)is { FixedTargetSlash: { } fixedDraft } fixedFrame && fixedFrame.OwnerSeat == actorSeat && fixedDraft.TargetSeat == targetSeat ? _coreFixedSlashAndDeclaredDeckProgramsOperations.IsFixedTargetFactionSlashLegal(fixedFrame, kind, effectiveRank, allowPotentialRank) : _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { IsAssistedProgramUse: true, ProgramSkillFrameId: { } giftFrameId } && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == giftFrameId)is { CompletedCardGiftDraft.RecipientSeat: { } recipient } giftFrame && recipient == actorSeat ? _coreCompletedCardGiftsOperations.CompletedGiftSlashTargets(giftFrame, actorSeat).Contains(targetSeat) : _characterXiaoQiaoLeOperations.HasWeiwanUseDistance(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], kind) || (_characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(_coreMatchParticipantsPlayersAccess._players[actorSeat]) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(_coreMatchParticipantsPlayersAccess._players[actorSeat])) || _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], kind, effectiveRank) || allowPotentialRank && effectiveRank is null && _coreFinalTargetSlashOperations.HasPotentialRankSlashRange(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], kind))) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], ignoresCount: true, kind) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat]) && (!(HasQigongFactionDistance(actorSeat, targetSeat) || _characterWuJingOlOperations.HasWuJingFactionDistance(actorSeat, targetSeat) || _characterJiangWeiMouMobileOperations.HasMouJwFactionDistance(actorSeat, targetSeat)) || !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, targetSeat, kind) && !_coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, kind, CardActionType.Use));
    internal void BeginAssistedProgramFactionSlashRequest(ProgramSkillFrame frame, int actorSeat, int targetSeat, string resultBind)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame current || current.Id != frame.Id || !_characterSunYuanOlOperations.IsSayingFactionOrigin(current, actorSeat, targetSeat, resultBind) && !_characterLiYiOlOperations.IsChanshuangFactionOrigin(current, actorSeat, targetSeat, resultBind) && !_characterPangTongHappyGodOperations.IsHappyPangTongFactionOrigin(current, actorSeat, targetSeat, resultBind) && (current.SelectedTargetSeats is not [var selected] || selected != actorSeat || current.OwnerSeat == actorSeat) || !(current.CompletedCardGiftDraft is { RecipientSeat: { } recipient, RequestTargetSeat: { } giftTarget } && recipient == actorSeat && giftTarget == targetSeat ? _coreCompletedCardGiftsOperations.CompletedGiftSlashTargets(current, actorSeat).Contains(targetSeat) && _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash)is not null : CanRequestAssistedProgramFactionSlash(actorSeat, targetSeat)))
            throw new InvalidOperationException("An assisted faction Slash lost its program parent, actor or target.");
        var instruction = ProgramInstructionResolver.Default.Resolve(current, _contentRegistryOperations._contentRegistry.GetSkill(current.SkillId).Program!).GetPausedInstruction(current.InstructionIndex).Effect;
        if (!(instruction.Op == SkillProgramEffectOp.RequestSlashAgainstChosenTarget && instruction.ResultBind == resultBind || instruction.Op == SkillProgramEffectOp.OfferCompletedCardGift && resultBind == "completed-card-gift-slash" && current.CompletedCardGiftDraft is not null || _coreNearestLegalSlashRequestsOperations.IsNearestLegalFactionOrigin(current, actorSeat, targetSeat, resultBind) || _characterLiYiOlOperations.IsChanshuangFactionOrigin(current, actorSeat, targetSeat, resultBind) || _characterSunYuanOlOperations.IsSayingFactionOrigin(current, actorSeat, targetSeat, resultBind) || _characterPangTongHappyGodOperations.IsHappyPangTongFactionOrigin(current, actorSeat, targetSeat, resultBind)) || current.ChoiceBindings.Any(binding => binding.Name == resultBind))
            throw new InvalidOperationException("An assisted faction Slash must answer its paused card request exactly once.");
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash)!;
        var candidates = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(actorSeat, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(frame.Id, FactionCardRequestPurpose.AssistedProgramUse, actorSeat, candidates, policy.FactionId, targetSeat: targetSeat, programSkillFrameId: frame.Id, policySource: policy, assistedResultBind: resultBind);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(frame.Id, actorSeat, candidates, policy.SkillId, IsActiveUse: true, targetSeat));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal void ValidateAssistedFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame)
    {
        if (_characterPangTongHappyGodOperations.ValidateHappyPangTongFactionParent(pending, frame))
            return;
        if (_characterLvKuangLvXiangOlOperations.ValidateQigongFactionSlashParent(pending, frame))
            return;
        if (_characterCaoChunOlOperations.ValidateCaoChunFactionSlashParent(pending, frame))
            return;
        if (_characterJiangWeiMouMobileOperations.ValidateMouJwFactionParent(pending, frame))
            return;
        if (_characterMaTieAnniversaryOperations.ValidateAnniversaryMaTieFactionParent(pending, frame))
            return;
        if (_characterWuJingOlOperations.ValidateWuJingFactionParent(pending, frame))
            return;
        if (_characterLiYiOlOperations.ValidateChanshuangFactionSlashParent(pending, frame))
            return;
        if (_characterSunYuanOlOperations.ValidateSayingFactionSlashParent(pending, frame))
            return;
        if (_coreFixedSlashAndDeclaredDeckProgramsOperations.ValidateFixedTargetFactionSlashParent(pending, frame))
            return;
        if (_coreNearestLegalSlashRequestsOperations.ValidateNearestLegalFactionParent(pending, frame))
            return;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex).Effect;
        var answered = frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == pending.AssistedResultBind);
        if (!(effect.Op == SkillProgramEffectOp.RequestSlashAgainstChosenTarget && effect.ResultBind == pending.AssistedResultBind || effect.Op == SkillProgramEffectOp.OfferCompletedCardGift && pending.AssistedResultBind == "completed-card-gift-slash" && frame.CompletedCardGiftDraft is { } gift && gift.RecipientSeat == pending.OwnerSeat && gift.RequestTargetSeat == pending.TargetSeat) || frame.SelectedTargetSeats is not [var actor] || actor != pending.OwnerSeat || frame.OwnerSeat == actor || pending.TargetSeat is not { } target || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || pending.PolicySource is null || (pending.ActiveAttack is null ? answered is not null : answered?.OptionId != "used-slash"))
            throw new InvalidOperationException("An assisted faction Slash changed its instruction, participants or result.");
    }
}
