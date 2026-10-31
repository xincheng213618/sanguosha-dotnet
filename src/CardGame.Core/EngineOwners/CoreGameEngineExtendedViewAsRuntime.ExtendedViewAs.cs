#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtendedViewAsRuntime
{
    internal bool IsArmorIneffectiveForTurn(CharacterState target) => _characterLuBuMobileGodOperations.HasMobileWuqianArmorDisabled(target.Seat) || _coreShenwuExpandedPoliciesOperations.ShenwuZhaohuoDisablesArmor(target.Seat) || _turnCardUseEffectsOperations._turnCardUseEffects.TargetRestrictions.Any(restriction => restriction.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && restriction.Restriction == SkillProgramCardTargetRestriction.ArmorIneffectiveForTurn && restriction.TargetSeat == target.Seat);
    internal bool TryResolveExtendedAiResponse(CharacterState owner, Card card, CardKind kind)
    {
        if (global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsNativeResponseCard(card, kind) || _coreActionQueriesOperations.GetProgramViewAsConversions(owner, card, kind, true).Count > 0)
            return false;
        var selection = _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, kind, true).FirstOrDefault(item => item.Cards.Any(cost => cost.Id == card.Id));
        if (selection is null)
            return false;
        ResolveExtendedViewAsResponse(owner, selection);
        return true;
    }

    internal IReadOnlyList<PromptChoice> ExtendedViewAsResponseChoices(CharacterState owner, CardKind kind)
    {
        if (kind == CardKind.Peach && _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(owner.Seat, dying.VictimSeat))
            return[];
        return _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, kind, true).Where(selection => (kind != CardKind.Peach || !_coreDeckEquipmentProgramsOperations.IsShieldedRescueSelection(owner, selection)) && (kind != CardKind.Alcohol || _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) || _characterZhaoTongZhaoGuangAnniversaryOperations.IsAnniversaryYizanSelection(selection) || _characterXunXuAnniversaryOperations.IsAnniversaryXunXuBoqiaSource(selection.Source) || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.RoundDistinctBasicUse is not null || _corePlayCardHistoryOperations.ViewAsRule(selection.Source)?.LeiluanBasicUse is not null)).Select(selection =>
        {
            var parameters = new Dictionary<string, string>
            {
                ["response"] = "extended-view-as",
                ["output-kind"] = kind.ToString()
            };
            global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.AddConversionParameters(parameters, selection.Source);
            return new PromptChoice(new ChoiceId("extended-view-as." + kind + "." + selection.Source.SkillId + "." + string.Join('-', selection.Cards.Select(card => card.Id))), _characterGuanHaiOlOperations.IsXieweiSelection(selection) ? $"发动【卸尾】，将两张手牌置为饵，视为使用【{(CardCatalog.Get(kind).DisplayName)}】。" : $"发动【{(_coreCardConversionsOperations.ProgramConversionName(selection.Source))}】，将 {(selection.Cards.Count)} 张牌当【{(CardCatalog.Get(kind).DisplayName)}】。", selection.Cards.Select(card => card.Id).ToArray(), [], parameters);
        }).ToArray();
    }

    internal void ResolveExtendedViewAsResponse(PromptChoice choice)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[_corePendingDecisionPolicyPipelineOperations._pendingDecision!.PlayerSeat];
        var kind = Enum.Parse<CardKind>(choice.Parameters["output-kind"]);
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardConversionsRuntime.RequireConversionSource(choice);
        var selection = _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(owner, choice.Cards, kind, true, source) ?? throw new InvalidOperationException("The extended conversion lost its physical costs.");
        ResolveExtendedViewAsResponse(owner, selection);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
            _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
    }

    internal void ResolveExtendedViewAsResponse(CharacterState owner, ProgramMultiCardViewAsSelection selection)
    {
        var kind = selection.OutputKind;
        if (kind == CardKind.Dodge && _characterGuanHaiOlOperations.IsXieweiSelection(selection) && _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is { } defense && defense.CurrentCandidateSeat == owner.Seat && _characterGuanHaiOlOperations.TryCompleteXieweiFactionDefense(defense, owner, selection))
            return;
        if (_characterKongShuOlOperations.TryResolveLeiluanDyingResponse(owner, selection))
            return;
        if (_coreWushuangJinzhiOperations.TryResolveWushuangJinzhiDyingAlcohol(owner, selection))
            return;
        if (_characterZhaoTongZhaoGuangAnniversaryOperations.TryResolveAnniversaryYizanDyingAlcohol(owner, selection))
            return;
        if (_characterXunXuAnniversaryOperations.TryResolveAnniversaryXunXuDyingAlcohol(owner, selection))
            return;
        if (_coreRoundDistinctBasicPlayOperations.TryResolveRoundDistinctBasicDyingResponse(owner, selection))
            return;
        if (kind == CardKind.Peach)
        {
            var dying = _coreDyingCursorRuntimeOperations.ActiveDying ?? throw new InvalidOperationException("Missing dying response.");
            if (dying.ResponderSeat != owner.Seat || !_coreProgramAllHandBonusOperationOperations.CanUsePeachToRescue(owner.Seat, dying.VictimSeat) || _coreDeckEquipmentProgramsOperations.IsShieldedRescueSelection(owner, selection))
                throw new InvalidOperationException("The rescue responder is no longer legal.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            var rule = _coreSkillProgramsOperations.GetEnabledSkillProgram(owner, selection.Source.SkillId).ViewAs.Single(item => item.Id == selection.Source.BindingId);
            var victim = _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat];
            var recoveryPolicies = owner.Seat != victim.Seat ? _coreProgramCardPoliciesOperations.CardPolicies(victim, SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(item => string.Equals(_coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(owner), item.Policy.FactionId, StringComparison.Ordinal)).ToArray() : [];
            _coreNativeRecoveryCardsOperations.ResolveRecoveryCard(owner, _coreMatchParticipantsPlayersAccess._players[dying.VictimSeat], _coreWushuangJinzhiOperations.IsWushuangJinzhiSelection(selection) ? _corePlayCardHistoryOperations.ApplyProgramUseAppearance(owner, selection.Cards[0], selection.Source) : selection.Cards[0], "桃", CardKind.Peach, 1 + rule.RecoveryBonus + recoveryPolicies.Sum(item => item.Policy.Value), recoveryPolicySources: recoveryPolicies.Select(item => (item.Source.SkillId, item.Policy.Id)).ToArray(), conversionSource: selection.Source, dyingResponse: new(dying.FrameId, owner.Seat, true, selection.Cards[0].Id, false, null), physicalCards: selection.Cards);
            return;
        }

        if (kind == CardKind.Nullification)
        {
            var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("Missing counterspell window.");
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            var unrespondableSource = _coreUnrespondableCounterspellsOperations.FreezeUnrespondableCounterspellSource(owner, selection.Cards.Select(card => card.Id).ToArray());
            var action = _coreCardConversionsOperations.MoveProgramMultiCardResponse(owner, selection, pending.ParentFrameId, pending.SourceSeat);
            _coreCardConversionsOperations.FinishProgramMultiCardResponse(selection);
            pending = (NullificationWindowFrame)_resolutionStackOperations._resolutionStack.Single(f => f.Id == pending.Id);
            pending = (action.PhysicalCards.Count > 0 && action.ActorSeat == action.ProviderSeat && action.PhysicalCards.All(c => c.From == CardLocation.Hand(owner.Seat)) && _coreNamedAmbushInvalidationOperations.HasEnabledMatchingNamedAmbush(owner.Seat, CardKind.Nullification)) || _characterChengYuMouOperations.HasMouCyMatchingName(owner.Seat, CardKind.Nullification) || _characterHuangZhongAnniversaryGodOperations.HasAnniversaryHzPendingInvalidation(owner.Seat) || _coreFieldEquipmentOperations.HasHappyWeiYanPotentialCancellation(owner.Seat) ? _coreNamedAmbushInvalidationOperations.PrepareNamedAmbushCounterspell(pending, owner.Seat, selection.Cards[0].Id) : _coreNativeNullificationFramesOperations.ReplaceNullificationWindowFrame(pending with { EffectNullified = !pending.EffectNullified, ChainDepth = pending.ChainDepth + 1, CandidateSeats = _coreNativeNullificationOperations.BuildNullificationCandidateSeats(owner.Seat), CandidateIndex = 0, Step = ResolutionFrameStep.AwaitingResponse });
            if (pending.NamedAmbushResponse is not null)
                _coreNamedAmbushInvalidationOperations.BindNamedAmbushCounterspell(pending.Id, action);
            else
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new NullificationRespondedEvent(pending.ParentFrameId, pending.EffectCardId, pending.EffectCardKind, owner.Seat, selection.Cards[0].Id, pending.EffectNullified, pending.ChainDepth) { WindowFrameId = pending.Id, TargetSeat = global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending) });
            if (pending.NamedAmbushResponse is null)
                _characterCaoXiOlOperations.CaptureCaoXiNativeCounterspellOffset(pending, action, invalidated: false);
            if (_coreUnrespondableCounterspellsOperations.TryBeginPolicyCounterspellPayment(pending, action, unrespondableSource, selection.Cards.Select(card => card.Id).ToArray()))
                return;
            if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(null, action, ProgramCardContinuation.NullificationResponse))
                return;
            if (!_coreCardActionsOperations.TryBeginProgramCardWindow(null, action, SkillProgramTriggerWindow.CardResponseAccepted, [], ProgramCardContinuation.NullificationResponse))
                _coreResponseUseCompletionOperations.ContinueNullificationAfterResponseUse(action);
            return;
        }

        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("Missing attack response.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault() is ResponseWindowFrame)
            _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        var responseAction = _coreCardConversionsOperations.MoveProgramMultiCardResponse(owner, selection, attack.ResolutionId, attack.SourceSeat);
        var continuation = _corePlayCardHistoryOperations.ViewAsRule(selection.Source)is { RoundDistinctBasicUse: not null } or { LeiluanBasicUse: not null } ? ProgramCardContinuation.Dodge : _coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { } ? ProgramCardContinuation.GroupResponse : ProgramCardContinuation.Dodge;
        if (_coreCommittedResponseUsesOperations.TryBeginCommittedResponseUsePrograms(attack, responseAction, continuation))
            return;
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, responseAction, SkillProgramTriggerWindow.CardResponseAccepted, [], continuation))
            _coreNativeDodgeOperations.ContinueAcceptedCardResponse(attack, responseAction, continuation);
    }

    internal int ProgramConversionDamageBonus(global::CardGame.Core.NativeOwners.CardAttackHandle attack)
    {
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Id == attack.ResolutionId)?.Action;
        if (action is null || attack.IsChainPropagation)
            return 0;
        return action.ConversionChain.Sum(source => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(source.SkillId)?.Program?.ViewAs.SingleOrDefault(rule => rule.Id == source.BindingId)?.DamageBonus ?? 0);
    }
}
