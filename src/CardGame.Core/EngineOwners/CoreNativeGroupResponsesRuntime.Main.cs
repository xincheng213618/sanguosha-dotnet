#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeGroupResponsesRuntime
{
    internal void ResolveGroupResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, CharacterState responder, Card? selectedResponse)
    {
        if (selectedResponse is not null && _characterLiuQiOlOperations.IsWenjiResponseForbidden2(group.ResolutionId, responder.Seat))
            throw new InvalidOperationException("Wenji forbids this exact group response.");
        if (selectedResponse is { } declaredResponse && _coreCardDeclarationsOperations.TryBeginCardDeclaration(responder, declaredResponse, group.RequiredCardKind!.Value, _coreCardDeclarationsOperations.PeekDeclarationConversion(responder, declaredResponse, group.RequiredCardKind.Value, false), _coreCardDeclarationsOperations.DeclarationReturn(CardDeclarationPurpose.Group, responder.Seat), [group.SourceSeat]))
            return;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.ResponseAttack || group.CurrentAttack is not { } attack || responder.Seat != attack.TargetSeat)
        {
            throw new InvalidOperationException("The group attack response does not belong to the current target.");
        }

        var requiredCardKind = group.RequiredCardKind ?? throw new InvalidOperationException("A group response attack must declare a required card kind.");
        if (selectedResponse is { } responseCard)
        {
            var responseCardKind = _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, responseCard, requiredCardKind);
            var responseConversion = _coreCardConversionsOperations.GetSelectedResponseConversion(responder, responseCard, responseCardKind);
            _coreNativePublicPileOriginalCardsOperations.PrepareNativePublicPileResponsePayment(responder, responseCard, responseCardKind, responseConversion);
            var responseFrom = _characterMaChaoAnniversaryGodOperations.FindOwnedCardLocation(responder, responseCard, responseConversion);
            var completedResponseUseSuit = _coreResponseUseCompletionOperations.FreezeCompletedResponseUseSuit(responder, [responseCard], responseCardKind);
            var responseCostIsRed = _coreActionDiscardColorsOperations.CapturePhysicalCardColor(responder.Seat, responseCard);
            _coreAlternativeViewAsCostsOperations.PaySingleCardResponse(responseCard, responder, responseCardKind, responseConversion);
            var incomingName = CardCatalog.Get(group.Card.Kind).DisplayName;
            var responseName = CardCatalog.Get(responseCardKind).DisplayName;
            var responseDescription = _coreCardDeclarationsOperations.PublicDeclarationDescription(responseCard, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsNativeResponseCard(responseCard, requiredCardKind) ? $"打出【{(responseName)}】" : $"将【{(responseCard.DisplayName)}】当作【{(responseName)}】");
            _coreMatchLogOperations.AddLog("CardResponded", $"{(responder.Name)} {(responseDescription)}响应【{(incomingName)}】。", responder.Seat, group.SourceSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(responseCard.Id, responder.Seat, group.SourceSeat, responseCardKind));
            _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, responseCardKind);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, requiredCardKind, responder.Seat, UsedResponse: true, ResponseCardId: responseCard.Id, ResponseCardKind: responseCardKind));
            var responseCost = new CardActionCost(responseCard.Id, responseCard.Kind, responseFrom, responseCostIsRed);
            if (_coreCardActionsOperations.TryBeginCardResponsePrograms(attack, responder, responder, requesterSeat: null, group.SourceSeat, responseCardKind, [responseCost], ProgramCardContinuation.GroupResponse, responseConversion, completedResponseUseSuit))
            {
                return;
            }

            _coreAlternativeViewAsCostsOperations.FinishSingleCardResponse(responseCard, responseConversion);
            if (requiredCardKind == CardKind.Dodge)
                _coreNativeDodgeOperations.CompleteSuccessfulDodgeResponse(attack);
            else
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, requiredCardKind, responder.Seat, UsedResponse: false, ResponseCardId: null));
        if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
        {
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
        }
    }

    internal void ResolveGroupZhangbaResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group, CharacterState responder, IReadOnlyList<Card> pair)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.ResponseAttack || group.RequiredCardKind != CardKind.Slash || group.CurrentAttack is not { } attack || responder.Seat != attack.TargetSeat)
        {
            throw new InvalidOperationException("The Zhangba group response is not current.");
        }

        var responseCosts = _coreResponseCompletionZhangbaOperations.CaptureZhangbaResponseCompletionCosts(responder, pair);
        MoveZhangbaResponseCards(responder, pair, group.ResolutionId, group.SourceSeat);
        _coreMatchLogOperations.AddLog("CardResponded", $"{(responder.Name)} 发动【丈八蛇矛】，将两张手牌当【杀】响应【{(group.Card.DisplayName)}】。", responder.Seat, group.SourceSeat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(responder.Seat, CardKind.Slash);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GroupResponseEvent(group.ResolutionId, group.Card.Kind, CardKind.Slash, responder.Seat, UsedResponse: true, ResponseCardId: pair[0].Id, ResponseCardKind: CardKind.Slash));
        if (responseCosts is not null && _coreCardActionsOperations.TryBeginCardResponsePrograms(attack, responder, responder, null, group.SourceSeat, CardKind.Slash, responseCosts, ProgramCardContinuation.GroupResponse))
            return;
        FinishZhangbaResponseCards(pair);
        _coreNativeAttackCompletionOperations.CompleteAttack(attack);
    }

    internal void MoveZhangbaResponseCards(CharacterState responder, IReadOnlyList<Card> pair, long resolutionId, int responseTargetSeat)
    {
        if (_coreNativeResponseMaterialQueriesOperations.FindZhangbaSlashPair(responder, pair.Select(card => card.Id).ToArray())is null)
        {
            throw new InvalidOperationException("The Zhangba Slash pair is no longer legal.");
        }

        _characterCaoZhiLeOperations.PrepareLeCaoZhiWenlanResponse(responder, CardKind.Slash, pair);
        foreach (var card in pair)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(responder, card), CardLocation.Processing, CardMoveReasons.Respond, disclosure: CardMovementDisclosure.FaceUp);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRespondedEvent(card.Id, responder.Seat, responseTargetSeat, CardKind.Slash));
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangbaSerpentSpearConvertedEvent(resolutionId, responder.Seat, Array.AsReadOnly(pair.Select(card => card.Id).ToArray()), IsUse: false, responseTargetSeat));
    }

    internal void FinishZhangbaResponseCards(IReadOnlyList<Card> pair)
    {
        foreach (var card in pair)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Processing, CardLocation.DiscardPile, CardMoveReasons.ResponseFinished);
        }
    }
}
