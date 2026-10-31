#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeNullificationRuntime
{
    internal void BeginJizhiOrNullificationWindow(long resolutionId, Card effectCard, int sourceSeat, IReadOnlyList<int> targetSeats, LegalActionKind actionKind, int? targetCardId = null, CardKind? requiredCardKind = null, CardKind? playedCardKind = null)
    {
        if (_coreRecoveryReplacementOperations.TryPauseRecoveryPaidCardUse(resolutionId, new(RecoveryPaidCardUseKind.Trick, sourceSeat, effectCard.Id, targetSeats, actionKind, targetCardId, requiredCardKind, playedCardKind)))
            return;
        _characterFeiYiOlOperations.BindFeiYiRankEffects(resolutionId, new(effectCard.Id, actionKind, targetCardId, requiredCardKind));
        var effectiveCardKind = playedCardKind ?? effectCard.Kind;
        if ((_coreCardUseLifecycleOperations.LifecycleCardUse(resolutionId)?.TargetsAdjusted == true))
        {
            var adjustedUse = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(frame => frame.Id == resolutionId);
            targetSeats = actionKind == LegalActionKind.BorrowedSword ? adjustedUse.TargetSeats.Skip(adjustedUse.TargetIndex).Take(2).ToArray() : adjustedUse.TargetSeats;
        }

        if (_coreCardUseLifecycleOperations.TryBeginCommittedCardUse(resolutionId, ProgramCardContinuation.CommittedTrick, trick: new(effectCard.Id, actionKind, targetCardId, requiredCardKind)))
            return;
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(effectiveCardKind) && _coreDelayedPlacementTargetProgramsOperations.TryBeginFinalizedDelayedPlacementPrograms(resolutionId, effectCard, actionKind))
            return;
        var pending = new global::CardGame.Core.NativeOwners.JizhiResolution(sourceSeat, resolutionId, effectCard, effectiveCardKind, targetSeats, actionKind, targetCardId, requiredCardKind);
        if (_coreCardActionsOperations.TryBeginFinalizedTrickPrograms(pending))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardUseBeforeTargetEffects(pending))
        {
            return;
        }

        ContinueJizhiOrNullificationAfterTargetTriggers(pending);
    }

    internal void ContinueJizhiOrNullificationAfterTargetTriggers(global::CardGame.Core.NativeOwners.JizhiResolution pending)
    {
        // Placement is the completed use. The delayed effect has its own
        // counterspell window when its holder reaches the Judgment phase.
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsDelayedCard(pending.EffectiveCardKind))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            ResolveNullifiableEffect(new NullificationWindowFrame(0, pending.ResolutionId, pending.PlayerSeat, _characterCuiLieAnniversaryOperations.AnniversaryCuiLieDelayedPlacementTargets(pending, _coreOriginalUseLightningOperations.OriginalUseLightningPlacementTargets(pending)), pending.Card.Id, pending.EffectiveCardKind, pending.ActionKind, pending.TargetCardId, pending.RequiredCardKind, []));
            return;
        }

        BeginNullificationWindow(pending.ResolutionId, pending.Card, pending.PlayerSeat, pending.TargetSeats, pending.ActionKind, pending.TargetCardId, pending.RequiredCardKind, pending.EffectiveCardKind);
    }

    internal void BeginNullificationWindow(long resolutionId, Card effectCard, int sourceSeat, IReadOnlyList<int> targetSeats, LegalActionKind actionKind, int? targetCardId = null, CardKind? requiredCardKind = null, CardKind? playedCardKind = null)
    {
        if (actionKind == LegalActionKind.MantianGuohai)
            _coreMantianGuohaiOperations.PrepareMantianGuohaiEffectRound(resolutionId);
        targetSeats = _coreSequentialTrickTargetsOperations.FreezeSequentialTrickTarget(resolutionId, targetSeats, actionKind, targetCardId, requiredCardKind);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineGroupTargetNullificationRuntime.UsesGroupTargetNullification(actionKind))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            ResolveNullifiableEffect(new NullificationWindowFrame(0, resolutionId, sourceSeat, Array.AsReadOnly(targetSeats.ToArray()), effectCard.Id, playedCardKind ?? effectCard.Kind, actionKind, targetCardId, requiredCardKind, []));
            return;
        }

        if (_characterZhugeGuoLeOperations.TryBypassLeChengyanNullification(resolutionId, effectCard, targetSeats, actionKind, targetCardId, requiredCardKind))
            return;
        if (_coreUnnullifiableOrdinaryTricksOperations.IsOriginalOrdinaryTrickCannotNullify(resolutionId) || _coreOrderedSelectedActorDuelsOperations.IsOrderedSelectedActorDuelCannotNullify(resolutionId) || _characterLuMengMouOperations.IsYingboUnrespondable(resolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(resolutionId))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.ResolvingEffect);
            ResolveNullifiableEffect(new NullificationWindowFrame(0, resolutionId, sourceSeat, Array.AsReadOnly(targetSeats.ToArray()), effectCard.Id, playedCardKind ?? effectCard.Kind, actionKind, targetCardId, requiredCardKind, []));
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow is not null)
        {
            throw new InvalidOperationException("The engine cannot open two Nullification windows at once.");
        }

        var candidates = BuildNullificationCandidateSeats(sourceSeat);
        _coreNativeCardUseFramesOperations.SetCardUseStep(resolutionId, ResolutionFrameStep.AwaitingResponse);
        var pending = new NullificationWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, resolutionId, sourceSeat, Array.AsReadOnly(targetSeats.ToArray()), effectCard.Id, playedCardKind ?? effectCard.Kind, actionKind, targetCardId, requiredCardKind, candidates);
        _coreRuntimeOperations.PushRuntimeFrame(pending);
        ContinueNullificationWindow(pending);
    }

    internal IReadOnlyList<int> BuildNullificationCandidateSeats(int startAfterSeat)
    {
        return Array.AsReadOnly(Enumerable.Range(1, _coreMatchDefinitionPlayerCountAccess._playerCount).Select(offset => _coreMatchParticipantsPlayersAccess._players[_coreTableOrderOperations.TableStepSeat(startAfterSeat, offset)]).Where(player => player.IsAlive).Select(player => player.Seat).ToArray());
    }

    internal void ContinueNullificationWindow(NullificationWindowFrame pending)
    {
        pending = _coreNamedAmbushInvalidationOperations.CompleteNamedAmbushCounterspell(pending);
        if (_coreUnrespondableCounterspellsOperations.TryFinishUnrespondableCounterspell(pending))
            return;
        if (pending.IssuedNoResponseNode is { } issuedNode)
        {
            if (issuedNode.ChainDepth != pending.ChainDepth || issuedNode.Policy.ParentFrameId != pending.Id || issuedNode.Policy.CardActionId != issuedNode.ActionId)
                throw new InvalidOperationException("Issued counterspell node lost its exact typed parent.");
            FinishNullificationWindow(pending);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not NullificationWindowFrame frame || frame.Id != pending.Id)
        {
            throw new InvalidOperationException("The Nullification window is not the current resolution frame.");
        }

        pending = frame;
        while (pending.CandidateIndex < pending.CandidateSeats.Count)
        {
            var responder = _coreMatchParticipantsPlayersAccess._players[pending.CandidateSeats[pending.CandidateIndex]];
            if (_characterLiuQiOlOperations.IsWenjiNullificationResponseForbidden(pending, responder.Seat) || _coreWushuangSkillsBAuditOperations.WushuangFuqiNullificationBanned(responder.Seat) || _coreShanheS5QuyiOperations.ShanheS5FuqiNullificationBanned(responder.Seat) || _coreShanheTrickResponseOperations.ShanheTrickNullificationBanned(responder.Seat))
            {
                pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { CandidateIndex = pending.CandidateIndex + 1, Step = ResolutionFrameStep.AwaitingResponse });
                continue;
            }

            var cards = pending.ChainDepth == 0 && _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(pending.SourceSeat, responder.Seat, pending.EffectCardKind, pending.TargetSeats) ? [] : GetNullificationCards(responder);
            if (cards.Count == 0 && _coreTieredRoundZeroResponsesOperations.TieredRoundZeroResponseChoices(responder, CardKind.Nullification).Count == 0 && _characterXiaoQiaoMouOlOperations.MouXqJueyanResponseChoices(responder, CardKind.Nullification).Count == 0)
            {
                if (!(pending.ChainDepth == 0 && _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(pending.SourceSeat, responder.Seat, pending.EffectCardKind, pending.TargetSeats)) && _characterSimaYiMobileGodOperations.TryBeginMobileSimaRenjieNullification(pending, explicitPass: false))
                    return;
                pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { CandidateIndex = pending.CandidateIndex + 1, Step = ResolutionFrameStep.AwaitingResponse });
                continue;
            }

            var effectName = CardCatalog.Get(pending.EffectCardKind).DisplayName;
            var effectDescription = _coreGroupTargetNullificationOperations.NullificationEffectDescription(pending);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.Nullification, responder.Seat, pending.EffectNullified ? $"上一张【无懈可击】使{(effectDescription)}暂时失效，是否继续使用【无懈可击】？" : pending.DelayedReturn is not null ? $"{(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].Name)} 的【{(effectName)}】即将判定，是否使用【无懈可击】？" : pending.GroupTargetReturn is not null ? $"{(effectDescription)}即将结算，是否使用【无懈可击】？" : $"{(_coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].Name)} 使用了【{(effectName)}】，是否使用【无懈可击】？", cards.Select(card => card.Id).ToArray(), [], pending.SourceSeat, pending.EffectCardKind)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                Choices = CreateNullificationChoices(pending, responder, cards),
                TargetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending),
                RequiredCardKind = CardKind.Nullification
            };
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationRequestedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, pending.SourceSeat, responder.Seat, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
            _coreMatchLogOperations.AddLog("NullificationRequested", pending.EffectNullified ? $"{(responder.Name)} 可以继续使用【无懈可击】恢复{(effectDescription)}。" : $"{(responder.Name)} 可以使用【无懈可击】响应{(effectDescription)}。", pending.SourceSeat, responder.Seat);
            pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { Step = ResolutionFrameStep.AwaitingResponse });
            _coreMatchOutcomeStatusAccess._status = responder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return;
        }

        FinishNullificationWindow(pending);
    }

    internal IReadOnlyList<PromptChoice> CreateNullificationChoices(NullificationWindowFrame pending, CharacterState responder, IReadOnlyList<Card> cards)
    {
        var effectDescription = _coreGroupTargetNullificationOperations.NullificationEffectDescription(pending);
        var choices = cards.Where(card => _characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Nullification) || card.Kind == CardKind.Nullification || _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, CardKind.Nullification, true).Count > 0).SelectMany(card => _coreCardConversionsOperations.CreateConversionChoiceVariants(responder, card, CardKind.Nullification, forResponse: true, $"nullification.card-{(card.Id)}.depth-{(pending.ChainDepth)}", pending.EffectNullified ? $"使用【无懈可击】恢复{(effectDescription)}。" : $"使用【无懈可击】使{(effectDescription)}失效。", [card.Id], [], new Dictionary<string, string> { ["response"] = "nullification", ["effect-card-id"] = pending.EffectCardId.ToString(System.Globalization.CultureInfo.InvariantCulture), ["chain-depth"] = pending.ChainDepth.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList();
        choices.AddRange(_coreExtendedViewAsOperations.ExtendedViewAsResponseChoices(responder, CardKind.Nullification));
        choices.AddRange(_coreTieredRoundZeroResponsesOperations.TieredRoundZeroResponseChoices(responder, CardKind.Nullification));
        choices.AddRange(_characterXiaoQiaoMouOlOperations.MouXqJueyanResponseChoices(responder, CardKind.Nullification));
        choices.Add(new PromptChoice(new ChoiceId($"nullification.pass.depth-{(pending.ChainDepth)}"), "不使用【无懈可击】。", [], [], new Dictionary<string, string> { ["response"] = "pass" }));
        return choices;
    }

    internal IReadOnlyList<Card> GetNullificationCards(CharacterState responder) => _corePhysicalOwnedCardQueriesOperations.GetPlayableCards(responder).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(responder)).Concat(_coreNativePublicPileOriginalCardsOperations.NativePublicPileResponseCards(responder, CardKind.Nullification)).Where(card => !_coreProgramCardLegalityOperations.IsProgramMaterialUseForbidden(responder.Seat, CardKind.Nullification, [card.Id]) && _coreProgramCardLegalityOperations.IsProgramNativeResponseAllowed(responder, CardKind.Nullification, [card.Id]) && !_coreCardUseModulesOperations.IsCardUseForbidden(responder.Seat, CardKind.Nullification, CardActionType.Use) && !_characterCuiYanOlOperations.IsCuiYanNativeResponseForbidden(responder, CardKind.Nullification, [card]) && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(responder, card) && (!_coreCardConversionsOperations.HasProgramCardIdentity(responder, card) || _characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Nullification)) && (_characterGuanYuWumiaoOperations.IsWuyouIdentityKind(responder, card, CardKind.Nullification) || card.Kind == CardKind.Nullification || _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, CardKind.Nullification, forResponse: true).Count != 0)).Concat(_coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Nullification, true).Select(selection => selection.Cards[0])).DistinctBy(card => card.Id).ToArray();
    internal static CardConversionSource? ReadPublishedNullificationSource(PromptChoice choice)
    {
        if (choice.Parameters.ContainsKey("conversion-skill-id") && !global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var source))
        {
            throw new InvalidOperationException("The Nullification choice has an invalid conversion source.");
        }

        _ = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.TryReadConversionSource(choice.Parameters, out var selected);
        return selected;
    }

    internal void ValidateNullificationConversion(CharacterState responder, Card card, CardConversionSource? selectedSource)
    {
        if (_characterGuanYuWumiaoOperations.IsWuyouIdentityMaterial(responder, card, CardKind.Nullification, selectedSource))
            return;
        var candidates = _coreActionQueriesOperations.GetProgramViewAsConversions(responder, card, CardKind.Nullification, forResponse: true);
        if (selectedSource is not null && candidates.Contains(selectedSource))
            return;
        if (selectedSource is null && card.Kind == CardKind.Nullification && !_coreCardConversionsOperations.HasProgramCardIdentity(responder, card) && _characterHanDangMouMobileOperations.IsMouHanDangNativeResponseAllowed(responder, card))
            return;
        throw new InvalidOperationException("The selected Nullification conversion is no longer legal.");
    }

    internal void ResolveNullificationChoice(NullificationWindowFrame pending, CharacterState responder, Card? selectedCard, CardConversionSource? selectedConversionSource = null)
    {
        if (selectedCard is not null)
        {
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess._selectedResponseConversion = selectedConversionSource;
            _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess._hasSelectedResponseConversionChoice = true;
        }

        if (selectedCard is not null && _characterLiuQiOlOperations.IsWenjiNullificationResponseForbidden(pending, responder.Seat))
            throw new InvalidOperationException("Wenji forbids this exact actual counterspell responder.");
        if (selectedCard is { } declaredCounter && _coreCardDeclarationsOperations.TryBeginCardDeclaration(responder, declaredCounter, CardKind.Nullification, _coreCardDeclarationsOperations.PeekDeclarationConversion(responder, declaredCounter, CardKind.Nullification, false, selectedConversionSource), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.Counterspell, responder.Seat), pending.TargetSeats))
            return;
        if (!ReferenceEquals(_resolutionStackOperations._resolutionStack.LastOrDefault(), pending) || pending.CandidateIndex >= pending.CandidateSeats.Count || pending.CandidateSeats[pending.CandidateIndex] != responder.Seat)
        {
            throw new InvalidOperationException("The Nullification response is not for the current responder.");
        }

        if (selectedCard is null)
        {
            if (_characterSimaYiMobileGodOperations.TryBeginMobileSimaRenjieNullification(pending, explicitPass: true))
                return;
            pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { CandidateIndex = pending.CandidateIndex + 1, Step = ResolutionFrameStep.AwaitingResponse });
            ContinueNullificationWindow(pending);
            return;
        }

        if (selectedConversionSource is null && selectedCard.Kind != CardKind.Nullification && _coreActionQueriesOperations.GetProgramViewAsConversions(responder, selectedCard, CardKind.Nullification, true).Count == 0 && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Nullification, true).FirstOrDefault(selection => selection.Cards[0].Id == selectedCard.Id)is { } extended)
        {
            _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse(responder, extended);
            return;
        }

        var card = GetNullificationCards(responder).SingleOrDefault(candidate => candidate.Id == selectedCard.Id);
        if (card is null)
        {
            throw new InvalidOperationException("The selected Nullification card is not in the responder's hand.");
        }

        ValidateNullificationConversion(responder, card, selectedConversionSource);
        // This native path captures its response action explicitly below. Its
        // selected source has now been validated, so consume this transient
        // choice just as GetSelectedResponseConversion does for other replies.
        // A declaration child returns above before reaching this payment path.
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess._selectedResponseConversion = null;
        _coreGameEngineCardConversionsHasSelectedResponseConversionChoiceSelectedResponseConversionAccess._hasSelectedResponseConversionChoice = false;
        if (selectedConversionSource is not null && _corePlayCardHistoryOperations.ViewAsRule(selectedConversionSource)is { } usageRule && (usageRule.TieredRoundConversion is not null || usageRule.UnusedOutputNameThisGame || usageRule.ConversionStateId is not null && usageRule.UsesPerPhase is not null))
            _corePlayCardHistoryOperations.ConsumeProgramViewAsUsage([selectedConversionSource]);
        _characterZhangFeiWumiaoOperations.PrepareWuZhangFeiResponseRanks(responder, CardKind.Nullification, [card], selectedConversionSource);
        _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(responder, CardKind.Nullification, [card]);
        _characterMiHengLeOperations.PrepareSiruiResponsePayment(responder, card, CardKind.Nullification, selectedConversionSource);
        var unrespondableSource = _characterGuoJiaMouOperations.MouGjLunshiSource(responder, [card.Id], selectedConversionSource) ?? _coreUnrespondableCounterspellsOperations.FreezeUnrespondableCounterspellSource(responder, [card.Id]);
        _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(responder, card, CardKind.Nullification, selectedConversionSource);
        var responseFrom = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card);
        var responseCostIsRed = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, card);
        var completedResponseUseSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(responder, [card], CardKind.Nullification);
        var responsePaymentBefore = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
        _corePhysicalMovementFacadesOperations.MoveCard(card, responseFrom, CardLocation.Processing, CardMoveReasons.Nullification, disclosure: CardMovementDisclosure.FaceUp);
        if (!_characterMiHengLeOperations.IsSiruiOutsideResponseMaterial(pending.Id, card.Id) && !_coreWushuangShopCardsOperations.IsWushuangShopDestroyedNullificationPayment(pending.Id, responder.Seat, new(card.Id, card.Kind, responseFrom, responseCostIsRed), responsePaymentBefore))
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.NullificationFinished);
        pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(frame => frame.Id == pending.Id);
        pending = (responseFrom == CardLocation.Hand(responder.Seat) && _coreNamedAmbushInvalidationOperations.HasEnabledMatchingNamedAmbush(responder.Seat, CardKind.Nullification)) || _characterChengYuMouOperations.HasMouCyMatchingName(responder.Seat, CardKind.Nullification) || _characterHuangZhongAnniversaryGodOperations.HasAnniversaryHzPendingInvalidation(responder.Seat) || _coreFieldEquipmentOperations.HasHappyWeiYanPotentialCancellation(responder.Seat) ? _coreNamedAmbushInvalidationOperations.PrepareNamedAmbushCounterspell(pending, responder.Seat, card.Id) : _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { EffectNullified = !pending.EffectNullified, ChainDepth = pending.ChainDepth + 1, CandidateSeats = BuildNullificationCandidateSeats(responder.Seat), CandidateIndex = 0, Step = ResolutionFrameStep.AwaitingResponse });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(card.Id, responder.Seat, pending.SourceSeat, CardKind.Nullification));
        if (pending.NamedAmbushResponse is null)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationRespondedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, responder.Seat, card.Id, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
            _coreMatchLogOperations.AddLog("NullificationResponded", pending.EffectNullified ? $"{(responder.Name)} 使用【无懈可击】，使{(_coreGroupTargetNullificationOperations.NullificationEffectDescription(pending))}失效。" : $"{(responder.Name)} 使用【无懈可击】，使{(_coreGroupTargetNullificationOperations.NullificationEffectDescription(pending))}恢复。", responder.Seat, pending.SourceSeat);
        }

        var parentActionId = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Id == pending.ParentFrameId)?.Action?.ActionId;
        var responseAction = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, parentActionId, CardActionType.Response, responder.Seat, responder.Seat, null, responder.Seat, pending.SourceSeat, CardKind.Nullification, [], [new CardActionCost(card.Id, card.Kind, responseFrom, responseCostIsRed)], selectedConversionSource is null ? [] : [selectedConversionSource], effectiveSuit: completedResponseUseSuit, effectiveRank: _characterGuanYuWumiaoOperations.CaptureJuewuResponseRank(responder, [new CardActionCost(card.Id, card.Kind, responseFrom, responseCostIsRed)]) ?? _characterLuXunWumiaoOperations.CaptureWuLuXunResponseRank(responder, CardKind.Nullification, [new CardActionCost(card.Id, card.Kind, responseFrom, responseCostIsRed)], selectedConversionSource) ?? _characterZhangFeiWumiaoOperations.CaptureWuZhangFeiResponseRank(responder, CardKind.Nullification, [new CardActionCost(card.Id, card.Kind, responseFrom, responseCostIsRed)], selectedConversionSource) ?? (_characterMiHengLeOperations.IsSiruiSource(selectedConversionSource) || _characterLiFengOlOperations.HasLiFengCapability && responseFrom.Zone == CardZoneKind.PublicPersistentPile ? card.Rank : (int? )null), effectiveIsRed: (_characterRuanYuOlOperations.HasRuanYuCapability || _characterMaMiDiOlOperations.HasMaMiDiCapability || _coreActionDiscardColorsOperations.TracksActionDiscardColor || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.OfferPairedColorCardDisposition)) ? global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(completedResponseUseSuit) ?? responseCostIsRed : null));
        _characterZhangFeiWumiaoOperations.BindWuZhangFeiResponseRanks(responseAction);
        _characterMiHengLeOperations.BindSiruiResponseAction(pending, responseAction);
        pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == pending.Id);
        if (pending.NamedAmbushResponse is not null)
            _coreNamedAmbushInvalidationOperations.BindNamedAmbushCounterspell(pending.Id, responseAction);
        _coreTieredRoundZeroResponsesOperations.TryIssueTieredRoundResponse(pending, responseAction);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(responseAction);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardActionAcceptedEvent(responseAction));
        if (pending.NamedAmbushResponse is null)
            _characterCaoXiOlOperations.CaptureCaoXiNativeCounterspellOffset(pending, responseAction, invalidated: false);
        _coreFriendEventsOperations.CaptureFriendMinimumHandUse(responseAction);
        _coreOffTurnUsedCardsOperations.CaptureOffTurnResponseUseOrigin(pending.ParentFrameId, responseAction);
        _coreOffTurnUsedCardsOperations.CaptureOffTurnResponseUsedCards(responseAction);
        if (_coreUnrespondableCounterspellsOperations.TryBeginPolicyCounterspellPayment(pending, responseAction, unrespondableSource, [card.Id]))
            return;
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(null, responseAction, ProgramCardContinuation.NullificationResponse))
            return;
        if (_coreCardActionsOperations.TryBeginProgramCardWindow(null, responseAction, SkillProgramTriggerWindow.CardResponseAccepted, [], ProgramCardContinuation.NullificationResponse))
            return;
        _coreResponseUseCompletionOperations.ContinueNullificationAfterResponseUse(responseAction);
    }

    internal void FinishNullificationWindow(NullificationWindowFrame pending)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not NullificationWindowFrame frame || frame.Id != pending.Id)
        {
            throw new InvalidOperationException("The Nullification window cannot finish from its current frame.");
        }

        if (_programPipelineHooksOperations._programPipelineHooks.BeforeNullificationFinished(frame))
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == frame.Id)
                throw new InvalidOperationException("A deferred counterspell completion must push a child on its original live window.");
            return;
        }

        frame = _resolutionStackOperations._resolutionStack.OfType<NullificationWindowFrame>().Single(item => item.Id == frame.Id);
        pending = _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(frame with { Step = ResolutionFrameStep.Completed });
        _coreRuntimeOperations.PopResolutionFrame(pending.Id, ResolutionFrameKind.NullificationWindow);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationResolvedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (pending.DelayedReturn is { } delayed)
        {
            _coreDelayedNullificationOperations.CompleteDelayedNullification(pending, delayed);
            return;
        }

        _coreNativeCardUseFramesOperations.SetCardUseStep(pending.ParentFrameId, ResolutionFrameStep.ResolvingEffect);
        if (pending.GroupTargetReturn is not null)
        {
            _coreGroupTargetNullificationOperations.CompleteGroupTargetNullification(pending);
            return;
        }

        if (pending.EffectNullified && !_coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(pending.ParentFrameId, CurrentCardEnhancement.Uncancelable))
        {
            if (_coreNativeSlashTargetDuelOperations.TryCompleteCanceledNativeSlashTargetDuel(pending))
                return;
            var finishedPhysicalCards = _coreNativeTrickUseOperations.GetCardUsePhysicalCards(pending.ParentFrameId);
            var beforeFinishMovement = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            foreach (var physicalCard in finishedPhysicalCards)
            {
                _coreSequentialTrickTargetsOperations.MoveFinishedTrickCard(pending.ParentFrameId, physicalCard);
            }

            _coreWushuangTargetCardsOperations.WushuangClaimNullifiedTrick(pending, finishedPhysicalCards, beforeFinishMovement);
            _coreMatchLogOperations.AddLog("CardEffect", $"【{(CardCatalog.Get(pending.EffectCardKind).DisplayName)}】的效果被无懈。", pending.SourceSeat);
            _coreNativeCardUseCompletionOperations.FinishCardUse(pending.ParentFrameId, _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending), pending.EffectCardKind);
            return;
        }

        ResolveNullifiableEffect(pending);
    }

    internal void ResolveNullifiableEffect(NullificationWindowFrame pending)
    {
        var effectUse = _coreCardUseLifecycleOperations.LifecycleCardUse(pending.ParentFrameId) ?? throw new InvalidOperationException("A trick effect lost its owning use.");
        if (_coreProgramCardUseNullificationOperations.IsProgramUseIneffective(effectUse) || _coreNamedAmbushInvalidationOperations.IsNamedAmbushUseIneffective(effectUse))
        {
            _coreProgramCardTargetEffectsOperations.CompleteIneffectiveTrickTarget(pending, pending.TargetSeats.FirstOrDefault(pending.SourceSeat));
            return;
        }

        if (pending.TargetSeats.Count == 1 && _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(pending.ParentFrameId, pending.TargetSeats[0]))
        {
            _coreProgramCardTargetEffectsOperations.CompleteIneffectiveTrickTarget(pending, pending.TargetSeats[0]);
            return;
        }

        if (_coreCardEffectProgramsOperations.TryBeginOrdinaryCardEffectPrograms(pending))
            return;
        ResolveNullifiableEffectCore(pending);
    }

    internal void ResolveNullifiableEffectCore(NullificationWindowFrame pending)
    {
        if (_characterZhugeGuoLeOperations.TryResolveLeChengyanNativeEffect(pending))
            return;
        switch (pending.ActionKind)
        {
            case LegalActionKind.WumiaoFlood:
                _characterGuanYuWumiaoOperations.ResolveWumiaoFloodEffect(pending);
                break;
            case LegalActionKind.QizhengXiangsheng:
                _characterXunYuMobileGodOperations.ResolveQizhengXiangshengEffect(pending);
                break;
            case LegalActionKind.UnexpectedAssault:
                _coreUnexpectedAssaultOperations.ResolveUnexpectedAssaultEffect(pending);
                break;
            case LegalActionKind.DrawTwo:
                _coreNativeTrickEffectsOperations.ResolveDrawTwoEffect(pending);
                break;
            case LegalActionKind.AllianceFeast:
                _coreHulaoCardsOperations.ResolveAllianceFeastEffect(pending);
                break;
            case LegalActionKind.SowBeans:
                _coreShenwuCardsOperations.ResolveShenwuSowBeansEffect(pending);
                break;
            case LegalActionKind.ReturnSoul:
                _coreHellSoulCardsOperations.ResolveHellReturnSoulEffect(pending);
                break;
            case LegalActionKind.TransplantWood:
                _coreShenwuCardsOperations.ResolveShenwuTransplantWoodEffect(pending);
                break;
            case LegalActionKind.MantianGuohai:
                _coreMantianGuohaiOperations.ResolveMantianGuohaiEffect(pending);
                break;
            case LegalActionKind.StarShift:
            case LegalActionKind.BeamSwap:
            case LegalActionKind.HandRedistribution:
            case LegalActionKind.WenheChaos:
                _coreWenheCardsOperations.ResolveWenheCardEffect(pending);
                break;
            case LegalActionKind.StoreGrain:
            case LegalActionKind.Reinforcements:
            case LegalActionKind.UnitedEffort:
                _coreGuanduCardsOperations.ResolveGuanduTrickEffect(pending);
                break;
            case LegalActionKind.SameBoat:
            case LegalActionKind.StriveUpstream:
            case LegalActionKind.AgainstCurrent:
                _coreDragonBoatCardsOperations.ResolveDragonBoatTrickEffect(pending);
                break;
            case LegalActionKind.BarbarianAssault:
            case LegalActionKind.ArrowBarrage:
                _coreNativeTrickEffectsOperations.ResolveGroupCardEffect(pending);
                break;
            case LegalActionKind.PeachGarden:
                _coreNativeTrickEffectsOperations.ResolvePeachGardenEffect(pending);
                break;
            case LegalActionKind.FiveGrains:
                _coreNativeTrickEffectsOperations.ResolveFiveGrainsEffect(pending);
                break;
            case LegalActionKind.Dismantlement:
            case LegalActionKind.Snatch:
                _coreNativeTrickEffectsOperations.ResolveTargetCardEffect(pending);
                break;
            case LegalActionKind.FireAttack:
                _coreNativeTrickEffectsOperations.ResolveFireAttackEffect(pending);
                break;
            case LegalActionKind.BorrowedSword:
                _coreNativeTrickEffectsOperations.ResolveBorrowedSwordEffect(pending);
                break;
            case LegalActionKind.Duel:
                _coreNativeTrickEffectsOperations.ResolveDuelEffect(pending);
                break;
            case LegalActionKind.IronChain:
                _coreNativeTrickEffectsOperations.ResolveIronChainEffect(pending);
                break;
            case LegalActionKind.Indulgence:
            case LegalActionKind.SupplyShortage:
            case LegalActionKind.Lightning:
                _coreNativeTrickUseOperations.ResolveDelayedCardEffect(pending);
                break;
            default:
                throw new InvalidOperationException($"Card kind {(pending.EffectCardKind)} cannot continue after Nullification.");
        }
    }
}
