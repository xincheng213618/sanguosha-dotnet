#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeDuelRuntime
{
    internal void ResolveDuel(CharacterState source, CharacterState target, Card duel, CardKind? playedCardKind = null)
    {
        var stillLegal = _coreNativeLegalActionsOperations.BuildLegalActions(source, selectedPhysicalCardId: duel.Id).Any(action => action.Kind == LegalActionKind.Duel && action.CardId == duel.Id && action.TargetSeat == target.Seat && action.PlayedCardKind == playedCardKind);
        if (!stillLegal)
        {
            throw new InvalidOperationException("Duel became illegal before resolution.");
        }

        var resolutionId = _coreNativeCardUseOperations.BeginCardUse(duel, source.Seat, [target.Seat], playedCardKind);
        _corePhysicalMovementFacadesOperations.MoveCard(duel, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(source, duel), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(resolutionId, duel, source.Seat, [target.Seat], LegalActionKind.Duel, playedCardKind: playedCardKind);
    }

    internal void BeginDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, bool keepCurrentRequest = false)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveDuel, duel))
        {
            throw new InvalidOperationException("The Duel response is not the current card resolution.");
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[duel.ResponderSeat];
        var opponent = _coreMatchParticipantsPlayersAccess._players[duel.OpponentSeat];
        if (_characterLuMengMouOperations.IsYingboUnrespondable(duel.ResolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(duel.ResolutionId) || _characterLiuQiOlOperations.IsWenjiResponseForbidden(duel.ResolutionId, responder.Seat) || _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(duel.Attack.CardUserSeat, responder.Seat, CardKind.Duel, [duel.TargetSeat]))
        {
            _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
            ResolveDuelResponse(duel, responder, selectedSlash: null);
            return;
        }

        _characterSimaYiMobileGodOperations.MarkMobileSimaRenjieResponse(duel.Attack, responder.Seat, CardKind.Slash);
        var requiredSlashResponses = _coreNativeEquipmentAndFactionQueriesOperations.GetRequiredResponseCount(opponent, opponent.Seat, responder.Seat, CardKind.Duel, CardKind.Slash);
        var responseOrdinal = duel.SuccessfulSlashResponses + 1;
        var slashes = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, CardKind.Slash);
        var zhangbaPairs = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(responder);
        var programMultiCardResponses = _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Slash, forResponse: true);
        var canRequestFactionSlash = _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionSlashResponse(responder, duel.Attack);
        var friendResponses = _coreFriendDuelResponsesOperations.FriendDuelResponseChoices(duel, responder);
        if (!keepCurrentRequest && slashes.Count == 0 && zhangbaPairs.Count == 0 && programMultiCardResponses.Count == 0 && !canRequestFactionSlash && !_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(responder, CardKind.Slash) && !_characterGuoTuOlOperations.HasWeijieRequiredBasic(responder, CardKind.Slash) && friendResponses.Count == 0)
        {
            if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is null)
            {
                throw new InvalidOperationException("A Duel response has no active damage source.");
            }

            _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
            ResolveDuelResponse(duel, responder, selectedSlash: null);
            return;
        }

        _coreNativeResponseWindowsOperations.PushResponseWindow(duel.ResolutionId, duel.OpponentSeat, responder.Seat, CardKind.Duel, CardKind.Slash);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.RespondSlash, responder.Seat, requiredSlashResponses > 1 ? $"{(opponent.Name)} 的【无双】要求你打出第 {(responseOrdinal)} 张【杀】响应【决斗】，是否响应？" : $"{(opponent.Name)} 对你使用了【决斗】，是否打出【杀】？", slashes.Select(card => card.Id).Concat(zhangbaPairs.SelectMany(pair => pair.Select(card => card.Id))).Concat(programMultiCardResponses.SelectMany(item => item.Cards.Select(card => card.Id))).Concat(friendResponses.SelectMany(choice => choice.Cards)).Distinct().ToArray(), [], duel.OpponentSeat, CardKind.Duel)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(slashes, CardKind.Slash, effectiveCardKindSelector: card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, CardKind.Slash), includeFactionSlash: canRequestFactionSlash, responder: responder).Concat(friendResponses).ToArray(),
            RequiredCardKind = CardKind.Slash
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ResponseRequestedEvent(duel.OpponentSeat, responder.Seat, CardKind.Duel, CardKind.Slash));
        _coreMatchOutcomeStatusAccess._status = responder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveDuelResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, CharacterState responder, Card? selectedSlash)
    {
        if (selectedSlash is not null && _characterLiuQiOlOperations.IsWenjiResponseForbidden(duel.ResolutionId, responder.Seat))
            throw new InvalidOperationException("Wenji forbids this exact Duel response.");
        if (selectedSlash is { } declaredSlash && _coreCardDeclarationsOperations.TryBeginCardDeclaration(responder, declaredSlash, CardKind.Slash, _coreCardDeclarationsOperations.PeekDeclarationConversion(responder, declaredSlash, CardKind.Slash, false), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.Duel, responder.Seat), [duel.OpponentSeat]))
            return;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveDuel, duel) || responder.Seat != duel.ResponderSeat)
        {
            throw new InvalidOperationException("The Duel response does not belong to the current responder.");
        }

        if (selectedSlash is { } slash)
        {
            var responseCardKind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, slash, CardKind.Slash);
            var responseConversion = _coreCardConversionsOperations.GetSelectedResponseConversion(responder, slash, responseCardKind);
            _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(responder, slash, responseCardKind, responseConversion);
            _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(responder, responseCardKind, [slash]);
            var responseFrom = _characterMaChaoAnniversaryGodOperations.FindOwnedCardLocation(responder, slash, responseConversion);
            var completedResponseUseSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(responder, [slash], responseCardKind);
            var responseCostIsRed = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, slash);
            _corePhysicalMovementFacadesOperations.MoveCard(slash, responseFrom, CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
            var responseName = CardCatalog.Get(responseCardKind).DisplayName;
            var responseDescription = _coreCardDeclarationsOperations.PublicDeclarationDescription(slash, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsNativeResponseCard(slash, CardKind.Slash) ? $"打出【{(responseName)}】" : $"将【{(slash.DisplayName)}】当作【{(responseName)}】");
            _coreMatchLogOperations.AddLog("CardResponded", $"{(responder.Name)} {(responseDescription)}应战【决斗】。", responder.Seat, duel.OpponentSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(slash.Id, responder.Seat, duel.OpponentSeat, responseCardKind));
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, responseCardKind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel.ResolutionId, responder.Seat, UsedSlash: true, SlashCardId: slash.Id, ResponseCardKind: responseCardKind));
            var responseCost = new CardActionCost(slash.Id, slash.Kind, responseFrom, responseCostIsRed);
            if (_coreCardActionsOperations.TryBeginCardResponsePrograms(duel.Attack, responder, responder, requesterSeat: null, duel.OpponentSeat, responseCardKind, [responseCost], ProgramCardContinuation.DuelSlash, responseConversion, completedResponseUseSuit))
            {
                return;
            }

            _corePhysicalMovementFacadesOperations.MoveCard(slash, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
            ContinueDuelAfterSuccessfulSlash(duel, responder.Seat);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel.ResolutionId, responder.Seat, UsedSlash: false, SlashCardId: null));
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("A failed Duel response has no active damage source.");
        {
            attack.SetDamageParticipants(sourceSeat: duel.OpponentSeat, targetSeat: responder.Seat);
        }

        if (_characterZhangLiaoMobileGodOperations.TryBeginMobileZhitiDuelPrograms(attack))
            return;
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }

    internal void ResolveDuelZhangbaResponse(global::CardGame.Core.NativeOwners.DuelHandle duel, CharacterState responder, IReadOnlyList<Card> pair)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveDuel, duel) || responder.Seat != duel.ResponderSeat)
        {
            throw new InvalidOperationException("The Zhangba Duel response does not belong to the current responder.");
        }

        var responseCosts = _coreResponseCompletionZhangbaOperations.CaptureZhangbaResponseCompletionCosts(responder, pair);
        _coreNativeGroupResponsesOperations.MoveZhangbaResponseCards(responder, pair, duel.ResolutionId, duel.OpponentSeat);
        _coreMatchLogOperations.AddLog("CardResponded", $"{(responder.Name)} 发动【丈八蛇矛】，将两张手牌当【杀】应战【决斗】。", responder.Seat, duel.OpponentSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new DuelResponseEvent(duel.ResolutionId, responder.Seat, UsedSlash: true, SlashCardId: pair[0].Id, ResponseCardKind: CardKind.Slash));
        if (responseCosts is not null && _coreCardActionsOperations.TryBeginCardResponsePrograms(duel.Attack, responder, responder, null, duel.OpponentSeat, CardKind.Slash, responseCosts, ProgramCardContinuation.DuelSlash))
            return;
        _coreNativeGroupResponsesOperations.FinishZhangbaResponseCards(pair);
        ContinueDuelAfterSuccessfulSlash(duel, responder.Seat);
    }

    internal void ContinueDuelAfterSuccessfulSlash(global::CardGame.Core.NativeOwners.DuelHandle duel, int responderSeat)
    {
        _characterSimaYiMobileGodOperations.ClearMobileSimaRenjieResponse(duel.Attack);
        var skillOwner = _coreMatchParticipantsPlayersAccess._players[duel.OpponentSeat];
        var requiredSlashResponses = _coreNativeEquipmentAndFactionQueriesOperations.GetRequiredResponseCount(skillOwner, skillOwner.Seat, responderSeat, CardKind.Duel, CardKind.Slash);
        var completedResponseSet = duel.RegisterSlashResponse(requiredSlashResponses);
        if (requiredSlashResponses > 1)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RequiredResponseProgressEvent(duel.ResolutionId, skillOwner.Seat, responderSeat, CardKind.Duel, CardKind.Slash, completedResponseSet ? requiredSlashResponses : duel.SuccessfulSlashResponses, requiredSlashResponses));
        }

        BeginDuelResponse(duel);
    }
}
