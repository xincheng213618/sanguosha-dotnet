#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDyingRuntime
{
    internal void BeginDying(global::CardGame.Core.GameEngine.IDamageAttempt attack, long damageFrameId, CharacterState victim, CharacterState killer)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying is not null)
        {
            throw new InvalidOperationException("The engine cannot resolve two dying players at once.");
        }

        var responderSeats = Array.AsReadOnly(BuildDyingResponderSeats(victim.Seat).ToArray());
        var frameId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        _coreRuntimeOperations.PushRuntimeFrame(new DyingFrame(frameId, damageFrameId, victim.Seat, attack.IsSourceLess ? null : killer.Seat, responderSeats, ResponderIndex: 0, DyingContinuationKind.Damage) { HappyQiaoPool = _characterDaQiaoXiaoQiaoHappyGodOperations.CurrentHappyQiaoPool(victim.Seat, damageFrameId) });
        _coreDynamicDiscardDamagePaymentsOperations.CaptureDynamicDiscardDamageDying(frameId, damageFrameId, victim.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PlayerDyingEvent(frameId, victim.Seat, attack.IsSourceLess ? null : killer.Seat));
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        if (_coreProgramLifecycleOperations.TryBeginMandatorySelfDyingProgram(_coreDyingCursorRuntimeOperations.ActiveDying!) || _coreDyingEntryProgramsOperations.TryBeginDyingEntryProgramWindow(_coreDyingCursorRuntimeOperations.ActiveDying!))
        {
            return;
        }

        ExposeHumanDyingPrompt();
    }

    internal IReadOnlyList<int> BuildDyingResponderSeats(int victimSeat)
    {
        var seats = new List<int>(_coreMatchDefinitionPlayerCountAccess._playerCount);
        for (var offset = 0; offset < _coreMatchDefinitionPlayerCountAccess._playerCount; offset++)
        {
            var seat = _coreTableOrderOperations.TableStepSeat(victimSeat, offset);
            if (_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            {
                seats.Add(seat);
            }
        }

        return seats;
    }

    internal Card[] GetDyingPeaches(CharacterState responder)
    {
        if (_characterPangJiOlOperations.IsBiguoCategoryForbidden(responder.Seat, CardKind.Peach))
            return[];
        if (_coreDyingCursorRuntimeOperations.ActiveDying is { } dying && !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(responder.Seat, dying.VictimSeat))
            return[];
        var handCandidates = _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(responder).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileResponseCards(responder, CardKind.Peach)).Where(card => !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(responder.Seat, CardKind.Peach, [card.Id])).Where(card => _coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(responder, CardKind.Peach, [card.Id], dyingUse: true)).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card)).Where(card => !_coreCardConversionsOperations.HasProgramCardIdentity(responder, card) || _characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Peach)).Where(card => card.Kind == CardKind.Peach && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(responder, card) && !_coreCuiYanNativeQueriesOperations.IsCuiYanSingleUseForbidden(responder, card) && (_coreDyingCursorRuntimeOperations.ActiveDying is not { } victim || !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(responder.Seat, victim.VictimSeat, _coreNativeCardAppearanceOperations.EffectiveSuit(responder, card))) || GetDyingPeachConversionSources(responder, card).Count != 0).ToArray();
        return handCandidates.Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(responder).Where(card => GetDyingPeachConversionSources(responder, card).Count != 0)).ToArray();
    }

    internal IReadOnlyList<CardConversionSource> GetDyingPeachConversionSources(CharacterState responder, Card card) => (_characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Peach) ? _characterGuanYuWumiaoOperations.WuyouIdentitySources(responder, card, CardKind.Peach) : card.Kind == CardKind.Peach && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(responder, card) && !_coreCardConversionsOperations.HasProgramCardIdentity(responder, card) ? [] : _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, CardKind.Peach, forResponse: true, dyingUse: true)).Where(source => _coreDyingCursorRuntimeOperations.ActiveDying is not { } dying || !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(responder.Seat, dying.VictimSeat, _coreNativeCardAppearanceOperations.EffectiveSuit(responder, _corePlayCardHistoryOperations.ApplyProgramUseAppearance(responder, card, source)))).ToArray();
    internal Card[] GetDyingAlcohols(CharacterState responder, int victimSeat) => responder.Seat == victimSeat && !_coreHandComparisonOperations.HasSelfCardTargetProhibition(responder.Seat) ? _corePhysicalOwnedCardQueriesOperations.GetHand(responder).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(responder)).Concat(_characterCaoZhiLeOperations.FuleDyingAlcoholGrainCards(responder)).Concat(_coreRequestedDeckBasicsOperations.SelectedRequestedDeckBasicCards(responder)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileResponseCards(responder, CardKind.Alcohol)).Where(card => !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card) && !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(responder.Seat, CardKind.Alcohol, [card.Id]) && _coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(responder, CardKind.Alcohol, [card.Id], dyingUse: true) && !_coreCardUseModulesOperations.IsCardUseForbidden(responder.Seat, CardKind.Alcohol, CardActionType.Use) && (!_coreCardConversionsOperations.HasProgramCardIdentity(responder, card) || _characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Alcohol) || _characterHouChengAnniversaryOperations.IsAnniversaryHouChengWineCard(card.Id)) && (_characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Alcohol) || (card.Kind == CardKind.Alcohol ? !_coreCuiYanNativeQueriesOperations.IsCuiYanSingleUseForbidden(responder, card) : _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, CardKind.Alcohol, forResponse: true, dyingUse: true).Count > 0))).ToArray() : [];
    internal void RunOneDyingStep(bool skipRequestedDeckBasic = false)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("A dying step requires a pending dying resolution.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
        {
            return;
        }

        if (dying.ResponderIndex >= dying.ResponderSeats.Count)
        {
            CompleteDying(dying, survived: false);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat];
        var peaches = GetDyingPeaches(responder);
        var alcohols = GetDyingAlcohols(responder, dying.VictimSeat);
        var programCandidates = _coreProgramLifecycleOperations.GetDyingProgramCandidates(responder, dying);
        if (responder.IsHuman)
        {
            if (peaches.Length > 0 || alcohols.Length > 0 || programCandidates.Count > 0 || _coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Peach).Count > 0 || _coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Alcohol).Count > 0 || (_coreTieredRoundZeroResponsesOperations.HasTieredRoundZeroDyingResponse(responder) || _characterXiaoQiaoMouOlOperations.HasMouXqJueyanDyingResponse(responder) || _characterHuanShuJunOlOperations.HasHuanpeiDyingResponse(responder)) || _coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicDyingResponse(responder) || _coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(responder, CardKind.Peach) || _characterGuoTuOlOperations.HasWeijieRequiredBasic(responder, CardKind.Peach))
            {
                RequestHumanDyingResponse(responder, peaches, alcohols);
                return;
            }

            ApplyDyingResponse(responder, usePeach: false, peachCardId: null, useAlcohol: false, alcoholCardId: null);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (programCandidates.FirstOrDefault(item => _coreProgramLifecycleOperations.GetProgramTrigger2(item).Window == SkillProgramTriggerWindow.SelfDyingResponse)is { } programCandidate)
        {
            _coreProgramLifecycleOperations.BeginDyingProgramBinding(programCandidate, dying);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        foreach (var rescueCandidate in programCandidates.Where(item => _coreProgramLifecycleOperations.GetProgramTrigger2(item).Window == SkillProgramTriggerWindow.DyingResponse))
        {
            var(useRescue, rescueThought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseProgramDyingRescue(_corePlayerViewProjectionOperations.CreateSnapshot(responder.Seat), dying.VictimSeat, _contentRegistryOperations._contentRegistry!.GetSkill(rescueCandidate.SkillId).Name, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
            _coreNativeAiDecisionsOperations.AddThought(rescueThought);
            if (!useRescue)
                continue;
            _coreProgramLifecycleOperations.BeginDyingProgramBinding(rescueCandidate, dying);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_characterGuoTuOlOperations.HasWeijieRequiredBasic(responder, CardKind.Peach))
        {
            RequestHumanDyingResponse(responder, peaches, alcohols);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (!skipRequestedDeckBasic && _coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(responder, CardKind.Peach))
        {
            RequestHumanDyingResponse(responder, peaches, alcohols);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_coreDrawFundedDistinctBasicResponsesOperations.TryResolveDrawFundedDistinctBasicAiDying())
            return;
        if (_characterXiaoQiaoMouOlOperations.TryMouXqJueyanAiResponse(CardKind.Peach, true) || _characterXiaoQiaoMouOlOperations.TryMouXqJueyanAiResponse(CardKind.Alcohol, true))
            return;
        if (_coreTieredRoundZeroResponsesOperations.TryResolveTieredRoundZeroAiDying())
            return;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(responder.Seat);
        if (peaches.Length == 0 && _coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(responder.Seat, dying.VictimSeat) && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Peach, true).FirstOrDefault(selection => !_coreDeckEquipmentProgramsOperations.IsShieldedRescueSelection(responder, selection))is { } multiPeach)
        {
            var(useMulti, multiThought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseProgramDyingRescue(view, dying.VictimSeat, _coreCardConversionsOperations.ProgramConversionName(multiPeach.Source), ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
            _coreNativeAiDecisionsOperations.AddThought(multiThought);
            if (useMulti)
            {
                _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse(responder, multiPeach);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return;
            }
        }

        if (peaches.Length == 0 && alcohols.Length == 0 && dying.VictimSeat == responder.Seat && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Alcohol, true).FirstOrDefault(selection => _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) || _characterZhaoTongZhaoGuangAnniversaryOperations.IsAnniversaryYizanSelection(selection) || _characterXunXuAnniversaryOperations.IsAnniversaryXunXuBoqiaSource(selection.Source) || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)is { RoundDistinctBasicUse: not null } or { LeiluanBasicUse: not null })is { } multiAlcohol)
        {
            var(useMultiAlcohol, multiAlcoholThought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseProgramDyingRescue(view, dying.VictimSeat, _coreCardConversionsOperations.ProgramConversionName(multiAlcohol.Source), ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence);
            _coreNativeAiDecisionsOperations.AddThought(multiAlcoholThought);
            if (useMultiAlcohol)
            {
                _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse(responder, multiAlcohol);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                return;
            }
        }

        var(usePeach, peachCardId, useAlcohol, alcoholCardId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseDyingResponseWithAlcohol(view, dying.VictimSeat, peaches, alcohols, ++_coreAiDiagnosticsThoughtSequenceAccess._thoughtSequence, false);
        _coreNativeAiDecisionsOperations.AddThought(thought);
        var aiPeachConversion = usePeach && peachCardId is { } selectedPeachId ? peaches.Where(card => card.Id == selectedPeachId && (card.Kind != CardKind.Peach || _coreWenhePoisonOperations.IsWenhePhysicalPoison(responder, card) || _coreCardConversionsOperations.HasProgramCardIdentity(responder, card))).SelectMany(card => GetDyingPeachConversionSources(responder, card)).FirstOrDefault() : null;
        var aiAlcoholConversion = useAlcohol && alcoholCardId is { } selectedAlcoholId ? alcohols.Where(card => card.Id == selectedAlcoholId && (card.Kind != CardKind.Alcohol || _coreCardConversionsOperations.HasProgramCardIdentity(responder, card))).SelectMany(card => _characterGuanYuWumiaoOperations.WuyouIdentitySources(responder, card, CardKind.Alcohol).Concat(_coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, CardKind.Alcohol, forResponse: true, dyingUse: true))).FirstOrDefault() : null;
        ApplyDyingResponse(responder, usePeach, peachCardId, useAlcohol, alcoholCardId, aiPeachConversion, aiAlcoholConversion);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ExposeHumanDyingPrompt()
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying;
        if (dying is null || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || dying.ResponderIndex >= dying.ResponderSeats.Count)
        {
            return;
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[dying.ResponderSeat];
        if (!responder.IsHuman)
        {
            return;
        }

        var peaches = GetDyingPeaches(responder);
        var alcohols = GetDyingAlcohols(responder, dying.VictimSeat);
        var programCandidates = _coreProgramLifecycleOperations.GetDyingProgramCandidates(responder, dying);
        if (peaches.Length > 0 || alcohols.Length > 0 || programCandidates.Count > 0 || _coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Peach).Count > 0 || _coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Alcohol).Count > 0 || (_coreTieredRoundZeroResponsesOperations.HasTieredRoundZeroDyingResponse(responder) || _characterXiaoQiaoMouOlOperations.HasMouXqJueyanDyingResponse(responder) || _characterHuanShuJunOlOperations.HasHuanpeiDyingResponse(responder)) || _coreDrawFundedDistinctBasicOffersOperations.HasDrawFundedDistinctBasicDyingResponse(responder) || _coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(responder, CardKind.Peach) || _characterGuoTuOlOperations.HasWeijieRequiredBasic(responder, CardKind.Peach))
        {
            RequestHumanDyingResponse(responder, peaches, alcohols);
        }
    }

    internal void RequestHumanDyingResponse(CharacterState responder, IReadOnlyList<Card> peaches, IReadOnlyList<Card> alcohols)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("There is no dying resolution for the prompt.");
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        var choices = peaches.SelectMany(peach =>
        {
            var sources = peach.Kind == CardKind.Peach && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(responder, peach) && !_coreCardConversionsOperations.HasProgramCardIdentity(responder, peach) ? new CardConversionSource? []
            {
                null
            }

            : GetDyingPeachConversionSources(responder, peach).Select(source => (CardConversionSource? )source).ToArray();
            return sources.Select((source, index) =>
            {
                var parameters = new Dictionary<string, string>
                {
                    ["response"] = "peach",
                    ["physical-card-kind"] = peach.Kind.ToString(),
                    ["target-seat"] = victim.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture)
                };
                if (source is not null)
                    global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, source);
                if (source is null && _coreNativePublicPileOriginalCardsOperations.IsNativePublicPileOriginalChoice(responder, peach, CardKind.Peach, true))
                    parameters["native-original-public-pile"] = "true";
                return new PromptChoice(new ChoiceId($"dying.peach.card-{(peach.Id)}.source-{(index)}"), source is null ? $"使用【桃】救援 {(victim.Name)}。" : $"发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(source!.SkillId).Name)}】，将【{(peach.DisplayName)}】当作【桃】救援 {(victim.Name)}。", [peach.Id], [], parameters);
            });
        }).ToList();
        choices.AddRange(alcohols.SelectMany(alcohol => _coreCardConversionsOperations.CreateConversionChoiceVariants(responder, alcohol, CardKind.Alcohol, forResponse: true, $"dying.alcohol.card-{(alcohol.Id)}", $"将【{(alcohol.DisplayName)}】作为【酒】自救，回复 1 点体力。", [alcohol.Id], [], new Dictionary<string, string> { ["response"] = "alcohol", ["target-seat"] = victim.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture) }, dyingUse: true)));
        var programCandidates = _coreProgramLifecycleOperations.GetDyingProgramCandidates(responder, dying);
        choices.AddRange(_coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Peach));
        choices.AddRange(_coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Alcohol));
        choices.AddRange(_coreTieredRoundZeroResponsesOperations.TieredRoundZeroResponseChoices(responder, CardKind.Peach, true));
        choices.AddRange(_characterXiaoQiaoMouOlOperations.MouXqJueyanResponseChoices(responder, CardKind.Peach, true));
        choices.AddRange(_characterXiaoQiaoMouOlOperations.MouXqJueyanResponseChoices(responder, CardKind.Alcohol, true));
        choices.AddRange(_coreTieredRoundZeroResponsesOperations.TieredRoundZeroResponseChoices(responder, CardKind.Alcohol, true));
        foreach (var candidate in programCandidates)
        {
            var skill = _contentRegistryOperations._contentRegistry!.GetSkill(candidate.SkillId);
            choices.Add(new PromptChoice(new ChoiceId($"dying.program.{(candidate.SkillId)}.{(candidate.BindingId)}.{(candidate.SkillInstanceId)}"), _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Window == SkillProgramTriggerWindow.DyingResponse ? $"发动【{(skill.Name)}】救援 {(victim.Name)}。" : $"发动【{(skill.Name)}】：{(skill.Description)}", [], [], new Dictionary<string, string> { ["response"] = "program-trigger", ["skill-id"] = candidate.SkillId, ["binding-id"] = candidate.BindingId, ["skill-instance-id"] = candidate.SkillInstanceId, ["target-seat"] = victim.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture), }));
        }

        var rescueCardIds = peaches.Select(card => card.Id).Concat(alcohols.Select(card => card.Id)).ToArray();
        var rescueNameParts = new List<string>();
        if (peaches.Count > 0)
            rescueNameParts.Add("桃");
        if (alcohols.Count > 0)
            rescueNameParts.Add("酒");
        rescueNameParts.AddRange(programCandidates.Select(candidate => _contentRegistryOperations._contentRegistry!.GetSkill(candidate.SkillId).Name));
        var rescueNames = string.Join("】或【", rescueNameParts);
        choices.Add(new PromptChoice(new ChoiceId("dying.let-die"), $"不使用【{(rescueNames)}】，让 {(victim.Name)} 阵亡。", [], [], new Dictionary<string, string> { ["response"] = "let-die", ["target-seat"] = victim.Seat.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RescueDying, responder.Seat, $"{(victim.Name)} 进入濒死状态，是否使用【{(rescueNames)}】救援？", rescueCardIds, [], SourceSeat: victim.Seat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            TargetSeat = victim.Seat
        };
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.AwaitingHumanDying;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ApplyDyingResponse(CharacterState responder, bool usePeach, int? peachCardId, bool useAlcohol, int? alcoholCardId, CardConversionSource? peachConversionSource = null, CardConversionSource? alcoholConversionSource = null)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("There is no dying response to apply.");
        if (responder.Seat != dying.ResponderSeat)
        {
            throw new InvalidOperationException("The response does not belong to the current dying responder.");
        }

        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.ResolvingEffect);
        int? usedPeachCardId = null;
        CardKind? usedPeachPhysicalCardKind = null;
        int? usedAlcoholCardId = null;
        if (usePeach && useAlcohol)
        {
            throw new InvalidOperationException("A dying response can use only one rescue source.");
        }

        if (usePeach)
        {
            var peach = GetDyingPeaches(responder).FirstOrDefault(card => (!peachCardId.HasValue || card.Id == peachCardId.Value));
            if (peach is null)
            {
                throw new InvalidOperationException("The requested Peach is not in the responder's playable zones.");
            }

            var sources = GetDyingPeachConversionSources(responder, peach);
            if (peach.Kind == CardKind.Peach && !_coreWenhePoisonOperations.IsWenhePhysicalPoison(responder, peach) && !_coreCardConversionsOperations.HasProgramCardIdentity(responder, peach) ? peachConversionSource is not null : peachConversionSource is null || !sources.Contains(peachConversionSource))
                throw new InvalidOperationException("The requested Peach conversion is no longer legal.");
            usedPeachCardId = peach.Id;
            usedPeachPhysicalCardKind = peachConversionSource is null ? null : peach.Kind;
            _coreNativeRecoveryCardsOperations.ResolvePeach2(responder, victim, peach, allowDying: true, peachConversionSource, new DyingResponseEvent(dying.FrameId, responder.Seat, true, peach.Id, false, null) { UsedPeachPhysicalCardKind = usedPeachPhysicalCardKind });
            return;
        }

        if (useAlcohol)
        {
            if (responder.Seat != victim.Seat)
            {
                throw new InvalidOperationException("Alcohol can only rescue its dying holder.");
            }

            var alcohol = GetDyingAlcohols(responder, victim.Seat).FirstOrDefault(card => (!alcoholCardId.HasValue || card.Id == alcoholCardId.Value));
            if (alcohol is null)
            {
                throw new InvalidOperationException("The requested Alcohol is not in the responder's hand.");
            }

            _coreProgramAlcoholConversionsOperations.ValidateAlcoholConversion(responder, alcohol, alcoholConversionSource, forResponse: true);
            usedAlcoholCardId = alcohol.Id;
            _coreNativeRecoveryCardsOperations.ResolveDyingAlcohol(responder, victim, alcohol, alcoholConversionSource, new DyingResponseEvent(dying.FrameId, responder.Seat, false, null, true, alcohol.Id));
            return;
        }

        CompleteDyingCardResponse(new DyingResponseEvent(dying.FrameId, responder.Seat, usePeach, usedPeachCardId, useAlcohol, usedAlcoholCardId) { UsedPeachPhysicalCardKind = usedPeachPhysicalCardKind });
    }

    internal void CompleteDyingCardResponse(DyingResponseEvent response)
    {
        var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("The rescue use lost its dying parent.");
        if (dying.FrameId != response.ResolutionId)
            throw new InvalidOperationException("The rescue use returned to a different dying parent.");
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(response);
        // A paid rescue use may leave a negative-HP victim still dying. Keep
        // offering this responder their remaining cards before moving on.
        if (!(response.UsedPeach || response.UsedAlcohol) || !_coreMatchParticipantsPlayersAccess._players[response.ResponderSeat].IsAlive)
            dying = _coreNativeDyingFramesOperations.UpdateDyingFrame(dying.Id, current => current with { ResponderIndex = current.ResponderIndex + 1 });
        if (victim.Hp > 0)
        {
            CompleteDying(dying, survived: true);
            return;
        }

        if (dying.ResponderIndex >= dying.ResponderSeats.Count)
        {
            CompleteDying(dying, survived: false);
            return;
        }

        _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.AwaitingResponse);
        _coreMatchOutcomeStatusWinnerAccess._status = EngineStatus.Running;
        ExposeHumanDyingPrompt();
    }

    internal void CompleteDying(DyingFrame dying, bool survived)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying?.Id != dying.Id)
        {
            throw new InvalidOperationException("The completed dying resolution is not current.");
        }

        _coreNativeDyingFramesOperations.SetDyingFrameStep(dying.FrameId, ResolutionFrameStep.ResolvingEffect);
        if (!survived)
        {
            _coreNativePlayerDeathOperations.FinalizePlayerDeath(dying);
            return;
        }

        CompleteDyingAfterDeath(dying, survived);
    }

    internal void CompleteDyingAfterDeath(DyingFrame dying, bool survived)
    {
        if (_coreDyingCursorRuntimeOperations.ActiveDying?.Id != dying.Id)
        {
            throw new InvalidOperationException("The completed dying continuation is not current.");
        }

        var receipt = new global::CardGame.Core.GameEngine.DyingCompletionReceipt(dying.Id, dying.ParentFrameId, dying.VictimSeat, dying.Continuation);
        _coreDynamicDiscardDamagePaymentsOperations.CaptureDynamicDiscardDamageSurvival(receipt, survived);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DyingResolvedEvent(dying.FrameId, dying.VictimSeat, survived));
        _coreRuntimeOperations.PopResolutionFrame(dying.FrameId, ResolutionFrameKind.Dying);
        if (_characterYanPuOlOperations.TryBeginYanPuDyingExit(receipt, survived))
            return;
        if (_coreDeferredBenefitsProgramsOperations.TryBeginFactionRecoveryDebts(receipt, survived))
            return;
        ContinueDyingAfterFactionRecoveryDebts(receipt, survived);
    }

    internal void ContinueDyingAfterFactionRecoveryDebts(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying, bool survived)
    {
        if (dying.Continuation == DyingContinuationKind.SkillInvocation)
        {
            _characterHuaTuoAnniversaryGodOperations.ReturnSkillInvocationDying(dying);
            return;
        }

        if (dying.Continuation == DyingContinuationKind.WenhePoison)
        {
            _coreWenhePoisonOperations.ReturnWenhePoisonDying(dying);
            return;
        }

        if (dying.Continuation == DyingContinuationKind.AttackHpLoss)
        {
            _coreNativeDamageCompletionOperations.CompleteDamageAttack(_coreNativeDamageHandlesOperations.GetCompletedDyingAttack(dying)!);
            return;
        }

        if (dying.Continuation == DyingContinuationKind.ProgramSkill)
        {
            _coreSkillProgramsOperations.CompleteProgramSkillAfterDying(dying);
            return;
        }

        CompleteDamageAfterDying(dying);
    }

    internal void CompleteDamageAfterDying(global::CardGame.Core.GameEngine.DyingCompletionReceipt dying)
    {
        var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
        var attack = _coreNativeDamageHandlesOperations.GetCompletedDyingAttack(dying) ?? throw new InvalidOperationException("A damage dying continuation has no attack.");
        if (dying.Continuation != DyingContinuationKind.Damage)
            throw new InvalidOperationException("A damage dying continuation has no damage frame.");
        var damageFrameId = dying.ParentFrameId;
        attack.MarkDyingResolvedForDamage(damageFrameId);
        if (_coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && _coreNativeDamageWindowsOperations.TryBeginDamageTriggerWindow(attack, damageFrameId, SkillProgramTriggerWindow.AfterDamageApplied))
        {
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AfterDamageEvent(damageFrameId, attack.SourceSeat, dying.VictimSeat, attack.DamageAmount, victim.Hp, _coreNativeCardClassificationsOperations.GetDamageNature(attack)) { SourceLess = attack.IsSourceLess });
        _coreRuntimeOperations.PopResolutionFrame(damageFrameId, ResolutionFrameKind.Damage);
        _coreNativeDamageCompletionOperations.CompleteDamageAttack(attack);
    }
}
