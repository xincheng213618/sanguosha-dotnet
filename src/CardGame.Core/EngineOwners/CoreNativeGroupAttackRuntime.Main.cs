#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeGroupAttackRuntime
{
    internal void BeginGroupAttackResponse(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameContinuationOwner(_coreNativeCardContinuationCursorsOperations.ActiveGroupCard, group) || group.Effect != GroupCardEffect.ResponseAttack)
        {
            throw new InvalidOperationException("The group attack is not the current card resolution.");
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativeGroupCompletionOperations.FinishGroupAttack(group);
            return;
        }

        if (group.TargetIndex >= group.TargetSeats.Count)
        {
            _coreNativeGroupCompletionOperations.FinishGroupAttack(group);
            return;
        }

        var targetSeat = group.TargetSeats[group.TargetIndex];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || _coreProgramCardTargetEffectsOperations.IsCardEffectIneffective(group.ResolutionId, targetSeat))
        {
            group.TargetIndex++;
            _coreNativeNullificationFramesOperations.SetCardUseTargetIndex(group.ResolutionId, group.TargetIndex);
            BeginGroupAttackResponse(group);
            return;
        }

        if (_coreGroupTargetNullificationOperations.TryBeginGroupTargetNullification(group))
            return;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(group.ResolutionId, group.DamageSourceSeat, target.Seat, (_characterZhugeLiangMouOperations.IsZeroEntityUse(group.ResolutionId) || _characterZhugeLiangMouOperations.IsMouZhijueZeroUse(group.ResolutionId) || _coreVirtualOrdinaryTrickUsesOperations.IsProgramVirtualOrdinaryTrickUse(group.ResolutionId)) ? null : group.Card, playedCardKind: group.Card.Kind, physicalCards: group.PhysicalCards, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(group.ResolutionId, target.Seat));
        group.CurrentAttack = attack;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreRequiredGroupDodgesOperations.FreezeRequiredGroupDodgeResponses(group, attack);
        if (_coreHellActivitiesOperations.HasHellRemenImmunity(target, group.Card.Kind))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_coreNativeDamageHandlesOperations.UsesFormalTengjia && !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasTengjia(target) && group.Card.Kind is CardKind.BarbarianAssault or CardKind.ArrowBarrage)
        {
            _coreMatchLogOperations.AddLog("ArmorEffect", $"{(target.Name)} 的【藤甲】令【{(CardCatalog.Get(group.Card.Kind).DisplayName)}】对其无效。", target.Seat, group.SourceSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorEffectAppliedEvent(group.ResolutionId, CardKind.Tengjia, group.SourceSeat, target.Seat, group.Card.Kind));
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_characterLuXunMouMobileOperations.TryBeginMouLuXunGroupEffect(group))
            return;
        ContinueGroupAttackResponseAfterEffect(group);
    }

    internal void ContinueGroupAttackResponseAfterEffect(global::CardGame.Core.NativeOwners.GroupCardHandle group)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[group.TargetSeats[group.TargetIndex]];
        var attack = group.CurrentAttack ?? throw new InvalidOperationException("A resumed group effect lost its exact target attack.");
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            _coreNativeGroupCompletionOperations.FinishGroupAttack(group);
            return;
        }

        if (!target.IsAlive)
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            return;
        }

        if (_characterLuMengMouOperations.IsYingboUnrespondable(group.ResolutionId) || _coreIssuedPlayUsePoliciesOperations.IsIssuedCardUnrespondable(group.ResolutionId) || _characterLiuQiOlOperations.IsWenjiResponseForbidden2(group.ResolutionId, target.Seat) || _coreProgramCardPoliciesOperations.IsNearbyTargetResponseProhibited(group.SourceSeat, target.Seat, group.Card.Kind, group.TargetSeats))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, target, selectedResponse: null);
            return;
        }

        var requiredCardKind = group.RequiredCardKind ?? throw new InvalidOperationException("A group response attack must declare a required card kind.");
        _characterSimaYiMobileGodOperations.MarkMobileSimaRenjieResponse(attack, target.Seat, requiredCardKind);
        var responseCards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(target, requiredCardKind);
        var zhangbaPairs = requiredCardKind == CardKind.Slash ? _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(target) : [];
        var programMultiCardResponses = requiredCardKind == CardKind.Slash ? _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(target, CardKind.Slash, forResponse: true) : [];
        var hasBagua = requiredCardKind == CardKind.Dodge && !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(target);
        var canRequestFactionDefense = requiredCardKind == CardKind.Dodge && _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionDefense(target, attack);
        var canRequestFactionSlash = requiredCardKind == CardKind.Slash && _coreNativeEquipmentAndFactionQueriesOperations.CanRequestFactionSlashResponse(target, attack);
        if (responseCards.Count == 0 && zhangbaPairs.Count == 0 && programMultiCardResponses.Count == 0 && !hasBagua && !canRequestFactionDefense && !canRequestFactionSlash && !(requiredCardKind == CardKind.Dodge && _coreProgramDodgeResponsesOperations.ProgramDodgeResponseChoices(target).Any()) && !_coreRequestedDeckBasicsOperations.HasRequestedDeckBasicSource(target, requiredCardKind) && !_characterGuoTuOlOperations.HasWeijieRequiredBasic(target, requiredCardKind))
        {
            _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
            _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, target, selectedResponse: null);
            return;
        }

        _coreNativeResponseWindowsOperations.PushResponseWindow(group.ResolutionId, group.SourceSeat, target.Seat, group.Card.Kind, requiredCardKind);
        var incomingName = CardCatalog.Get(group.Card.Kind).DisplayName;
        var requiredName = CardCatalog.Get(requiredCardKind).DisplayName;
        var decisionKind = requiredCardKind == CardKind.Dodge ? DecisionKind.RespondDodge : DecisionKind.RespondSlash;
        var responseQuestion = requiredCardKind == CardKind.Dodge && attack.RequiredDodgeResponses > 1 ? $"请打出第{(attack.SuccessfulDodgeResponses + 1)}/{(attack.RequiredDodgeResponses)}张【{(requiredName)}】。" : $"是否打出【{(requiredName)}】？";
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(decisionKind, target.Seat, $"{(_coreMatchParticipantsPlayersAccess._players[group.SourceSeat].Name)} 使用了【{(incomingName)}】，{(responseQuestion)}", responseCards.Select(card => card.Id).Concat(zhangbaPairs.SelectMany(pair => pair.Select(card => card.Id))).Concat(programMultiCardResponses.SelectMany(item => item.Cards.Select(card => card.Id))).Distinct().ToArray(), [], group.SourceSeat, group.Card.Kind)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = _coreNativePromptBoundaryOperations.CreateResponseChoices(responseCards, requiredCardKind, group.Card.Kind, card => _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(target, card, requiredCardKind), includeBagua: hasBagua, includeFactionDefense: canRequestFactionDefense, includeFactionSlash: canRequestFactionSlash, responder: target),
            RequiredCardKind = requiredCardKind
        };
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ResponseRequestedEvent(group.SourceSeat, target.Seat, group.Card.Kind, requiredCardKind));
        _coreMatchOutcomeStatusWinnerAccess._status = target.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }
}
