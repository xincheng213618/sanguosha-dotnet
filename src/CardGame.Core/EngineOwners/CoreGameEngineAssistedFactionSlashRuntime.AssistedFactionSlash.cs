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
                if (!hasIdentity && !_coreLastZoneConversionsOperations.IsLastZoneJudgmentCard(provider, card) && !_coreOwnPublicPileConversionsOperations.IsOwnPublicPileCard(provider, card) && card.Kind == kind && _coreNativeFactionSlashOperations.GetPermittedFactionSlashUseKinds(pending, provider, kind, [card]).Count != 0)
                    variants.Add(new(card, kind, null));
                var sources = hasIdentity ? _coreCardConversionsOperations.GetProgramCardIdentitySources(provider, card, kind, forResponse: false) : _coreActionQueriesOperations.GetProgramViewAsConversions(provider, card, kind, forResponse: false);
                variants.AddRange(sources.Where(source => _coreNativeFactionSlashOperations.GetPermittedFactionSlashUseKinds(pending, provider, kind, [card], source).Count != 0).Select(source => new GameEngine.AssistedFactionSlashCard(card, kind, source)));
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
            if (!_coreNativeFactionSlashOperations.GetPermittedFactionSlashUseKinds(pending, provider, kind, cards, conversion).Contains(kind))
                throw new InvalidOperationException("The supplied Slash violates its actual user's current next-use suit restriction.");
            _coreNativeFactionSlashOperations.BeginProvidedFactionSlashSlash(pending, provider, cards, kind, usesZhuqueFan: false, conversionSource: conversion);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (advanceToHumanBoundary)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal bool CanSupplyAssistedFactionSlash(NativeOwners.FactionCardRequestHandle pending, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) =>
        (!CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending) || !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(pending.OwnerSeat, cards.Select(c => c.Id).ToArray())) &&
        _providedSlashPaymentTargets.CanSupply(pending, kind, cards, isTrueZhangba);
    internal IReadOnlyList<IReadOnlyList<Card>> GetFactionRequestZhangbaPairs(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(provider).Where(pair => CanSupplyAssistedFactionSlash(pending, CardKind.Slash, pair, isTrueZhangba: true)).ToArray();
    internal IReadOnlyList<ProgramMultiCardViewAsSelection> GetFactionRequestMultiCardSelections(NativeOwners.FactionCardRequestHandle pending, CharacterState provider) => (pending.IsAssistedProgramUse ? CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.SelectMany(kind => _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, kind, false, ignoreSuitUseProhibition: true).Where(item => _corePlayCardHistoryOperations.ViewAsRule(item.Source)is { RoundDistinctBasicUse: null, LeiluanBasicUse: null } || item.OutputKind == kind)) : _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, pending.RequiredKind, !CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(pending), ignoreSuitUseProhibition: true)).Where(selection => _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.RoundDistinctBasicUse is null && _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.LeiluanBasicUse is null && CanSupplyAssistedFactionSlash(pending, selection.OutputKind, selection.Cards) && _coreNativeFactionSlashOperations.GetPermittedFactionSlashUseKinds(pending, provider, selection.OutputKind, selection.Cards, selection.Source).Contains(selection.OutputKind)).ToArray();
    internal bool CanRequestAssistedProgramFactionSlash(int actorSeat, int targetSeat, bool ignoreDistance = false, int? rangeOwnerSeat = null) =>
        _coreTableSeatQueriesOperations.IsValidPlayerSeat(actorSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) &&
        _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash) is not null &&
        CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds.Any(kind =>
            IsAssistedSlashTarget(actorSeat, targetSeat, kind, null, true, null, null, null, ignoreDistance, rangeOwnerSeat));

    internal bool IsAssistedProvidedSlashTarget(int actorSeat, int targetSeat, CardKind kind, int? effectiveRank = null, bool allowPotentialRank = true, IReadOnlyList<int>? physicalCardIds = null, Suit? physicalSuit = null, bool? effectiveColor = null, bool ignoreDistance = false, int? rangeOwnerSeat = null) =>
        IsAssistedSlashTarget(actorSeat, targetSeat, kind, effectiveRank, allowPotentialRank, physicalCardIds, physicalSuit, effectiveColor, ignoreDistance, rangeOwnerSeat);

    internal bool IsAssistedProvidedSlashTarget(NativeOwners.FactionCardRequestHandle request, CardKind kind, int? effectiveRank, IReadOnlyList<int> physicalCardIds, Suit? physicalSuit, bool? effectiveColor)
    {
        if (request.TargetSeat is not { } targetSeat || request.AssistedAuthorization is not { } authorization ||
            _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == request.OwnerFrameId) is not { } owner ||
            owner.InstructionIndex != authorization.InstructionIndex)
            return false;
        return IsAssistedSlashTarget(request.OwnerSeat, targetSeat, kind, effectiveRank, false, physicalCardIds, physicalSuit, effectiveColor, authorization.IgnoreDistance, authorization.RangeOwnerSeat);
    }

    private bool IsAssistedSlashTarget(int actorSeat, int targetSeat, CardKind kind, int? effectiveRank, bool allowPotentialRank, IReadOnlyList<int>? physicalCardIds, Suit? physicalSuit, bool? effectiveColor, bool ignoreDistance, int? rangeOwnerSeat)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[actorSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!actor.IsAlive || !target.IsAlive || actorSeat == targetSeat ||
            _coreCardUseModulesOperations.HasTurnCardTargetRestriction(actorSeat, SkillProgramCardTargetRestriction.SelfOnly) ||
            _coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, kind, CardActionType.Use) ||
            _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, targetSeat, kind) ||
            _coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind, physicalSuit, effectiveColor) ||
            !_coreRuleQueriesOperations.CanSpendSlashUse(actor, target, ignoresCount: true, kind))
            return false;

        if (rangeOwnerSeat is { } rangeOwner)
            return _coreMatchParticipantsPlayersAccess._players[rangeOwner].IsAlive && rangeOwner != actorSeat &&
                rangeOwner != targetSeat && _attackRanges.IsWithinAttackRange(rangeOwner, targetSeat);

        return ignoreDistance || _characterHuangfuSongWumiaoOperations.HasWuHfOrWuyouMaterialPrivilege(actor, kind, physicalCardIds) ||
            allowPotentialRank && physicalCardIds is null && _characterGuanYuWumiaoOperations.HasPotentialWuyouFactionSlash(actor, kind) ||
            _coreDirectedTurnCardPoliciesOperations.HasDirectedTurnCardPolicy(actor.Seat, target.Seat, kind, DirectedTurnCardPolicyEffect.IgnoreDistance) ||
            _characterCaiWenJiLeOperations.HasBeifenPreUsePrivilege(actor) || _characterHuangGaiMouMobileOperations.HasMouZhaJiangPreUsePrivilege(actor) ||
            _coreFinalTargetSlashOperations.IsWithinSpecificSlashRange(actor, target, kind, effectiveRank) ||
            allowPotentialRank && effectiveRank is null && _coreFinalTargetSlashOperations.HasPotentialRankSlashRange(actor, target, kind);
    }

    internal void BeginAssistedProgramFactionSlashRequest(ProgramSkillFrame frame, int actorSeat, int targetSeat, string resultBind, bool ignoreDistance = false, int? rangeOwnerSeat = null)
    {
        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is not null ||
            _resolutionStackOperations._resolutionStack.LastOrDefault() is not ProgramSkillFrame current || current.Id != frame.Id ||
            current.ChoiceBindings.Any(binding => binding.Name == resultBind) ||
            !CanRequestAssistedProgramFactionSlash(actorSeat, targetSeat, ignoreDistance, rangeOwnerSeat))
            throw new InvalidOperationException("An assisted faction Slash requires its unanswered original program request and legal participants.");
        var policy = _coreProgramFactionPoliciesOperations.GetFactionResponsePolicy(_coreMatchParticipantsPlayersAccess._players[actorSeat], CardKind.Slash)!;
        var candidates = _coreNativeEquipmentAndFactionQueriesOperations.GetFactionProviderSeats(actorSeat, policy.FactionId);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest = _kernelCoreMainOperations.CreateFactionCardRequestHandle(frame.Id, FactionCardRequestPurpose.AssistedProgramUse, actorSeat, candidates, policy.FactionId, targetSeat: targetSeat, programSkillFrameId: frame.Id, policySource: policy, assistedResultBind: resultBind);
        _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest.AuthorizeProgramRequest(current.InstructionIndex, ignoreDistance, rangeOwnerSeat, usesChoiceResult: true);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreMatchOutcomeStatusAccess._status = EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FactionSlashRequestedEvent(frame.Id, actorSeat, candidates, policy.SkillId, IsActiveUse: true, targetSeat));
        _coreNativeFactionSlashOperations.AdvanceFactionSlashCandidate();
    }

    internal void ValidateAssistedFactionSlashParent(NativeOwners.FactionCardRequestHandle pending, ProgramSkillFrame frame)
    {
        if (pending.AssistedAuthorization is not { } authorization || pending.OwnerFrameId != frame.Id ||
            authorization.InstructionIndex != frame.InstructionIndex || pending.PolicySource is null || pending.AssistedResultBind is null)
            throw new InvalidOperationException("An assisted faction Slash lost its original issued program request.");
        if (authorization.UsesChoiceResult)
        {
            var answered = frame.ChoiceBindings.SingleOrDefault(binding => binding.Name == pending.AssistedResultBind);
            if (pending.ActiveAttack is null ? answered is not null : answered?.OptionId != "used-slash")
                throw new InvalidOperationException("An assisted faction Slash changed its original choice result.");
        }
    }
}
