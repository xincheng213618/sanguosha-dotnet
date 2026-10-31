#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreNativeAiDecisionsRuntime
{
    internal void RunOneAiPlayDecision(CharacterState player)
    {
        _programPipelineHooksOperations._programPipelineHooks.PreparePlayPhaseDecision(player);
        var legalActions = _coreNativeLegalActionsOperations.BuildLegalActions(player);
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(player.Seat);
        var(action, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[player.Seat].ChoosePlay(view, legalActions, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, damagePrediction: (candidate, target) => _corePublicDamagePredictionOperations.PredictPublicActionDamage(view, player.Seat, candidate, target));
        AddThought(thought);
        if (action.Kind == LegalActionKind.EndPlay)
        {
            _coreProgramLifecycleOperations.CompleteCurrentPlayPhase();
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var activeSkillCards = action.Kind is LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill ? _coreAiBrainsAiBrainsAccess._aiBrains[player.Seat].ChooseActiveSkillCards(view, action) : [];
        var activeSkillTargets = action.Kind is LegalActionKind.UseEquipmentEffect or LegalActionKind.UseProgramSkill ? _coreAiBrainsAiBrainsAccess._aiBrains[player.Seat].ChooseActiveSkillTargets(view, action, activeSkillCards) : [];
        _coreNativeActionDispatchOperations.ExecuteAction(player, action, activeSkillCards, activeSkillTargets);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiResponse()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.RespondDodge, ValidCardIds.Count: 0 } dodgePrompt && dodgePrompt.Choices.FirstOrDefault(item => item.Parameters.GetValueOrDefault("response") == "program-dodge")is { } programDodge)
        {
            _coreProgramDodgeResponsesOperations.BeginProgramDodgeResponse(programDodge, false);
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQilinBow is not null)
        {
            ResolvePendingAiQilinBow();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveIceSword is not null)
        {
            ResolvePendingAiIceSword();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade is not null && _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind == DecisionKind.QinglongCrescentBlade)
        {
            ResolvePendingAiQinglongCrescentBlade();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords is not null)
        {
            ResolvePendingAiCixiongDoubleSwords();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveStoneAxe is not null)
        {
            ResolvePendingAiStoneAxe();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingZhuqueFanChoice: true })
        {
            ResolvePendingAiZhuqueFan();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest is { AwaitingProviders: true })
        {
            ResolvePendingAiFactionSlash();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveFactionDefense is not null)
        {
            ResolvePendingAiFactionDefense();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword is { AwaitingSlashChoice: true })
        {
            ResolvePendingAiBorrowedSword();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
        {
            ResolvePendingAiDuel();
            return;
        }

        if (_coreNativeCardContinuationCursorsOperations.ActiveGroupCard is { Effect: GroupCardEffect.ResponseAttack })
        {
            ResolvePendingAiGroupAttack();
            return;
        }

        ResolvePendingAiDodge();
    }

    internal void ResolvePendingAiQinglongCrescentBlade()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveQinglongCrescentBlade ?? throw new InvalidOperationException("AI Qinglong Crescent Blade response has no active resolution.");
        var attack = pending.Attack;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QinglongCrescentBlade } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The pending AI Qinglong Crescent Blade prompt is inconsistent.");
        }

        var(choiceId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[attack.SourceSeat].ChooseQinglongCrescentBladeChoice(_corePlayerViewProjectionOperations.CreateSnapshot(attack.SourceSeat), attack.TargetSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        var selected = decision.Choices.Single(choice => choice.Id == choiceId);
        _coreNativeQinglongOperations.ResolveQinglongCrescentBladeChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiIceSword()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveIceSword ?? throw new InvalidOperationException("AI Ice Sword response has no active resolution.");
        var attack = pending.Attack;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.IceSword } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The pending AI Ice Sword prompt is inconsistent.");
        }

        var(choiceId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[attack.SourceSeat].ChooseIceSwordChoice(_corePlayerViewProjectionOperations.CreateSnapshot(attack.SourceSeat), attack.TargetSeat, pending.PreventedDamageAmount, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, incomingPrediction: _corePublicDamagePredictionOperations.PredictPublicIncomingDamage(attack, attack.SourceSeat, damageAmount: pending.PreventedDamageAmount));
        AddThought(thought);
        var selected = decision.Choices.Single(choice => choice.Id == choiceId);
        _coreNativeIceSwordOperations.ResolveIceSwordChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiQilinBow()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveQilinBow ?? throw new InvalidOperationException("AI Qilin Bow response has no active resolution.");
        var attack = pending.Attack;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.QilinBow } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The pending AI Qilin Bow prompt is inconsistent.");
        }

        var(choiceId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[attack.SourceSeat].ChooseQilinBowChoice(_corePlayerViewProjectionOperations.CreateSnapshot(attack.SourceSeat), attack.TargetSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        var selected = decision.Choices.Single(choice => choice.Id == choiceId);
        _coreNativeQilinBowOperations.ResolveQilinBowChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiCixiongDoubleSwords()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveCixiongDoubleSwords ?? throw new InvalidOperationException("AI Cixiong Double Swords response has no active resolution.");
        var attack = pending.Attack;
        var responderSeat = pending.Stage == CixiongDoubleSwordsStage.SourceActivation ? attack.SourceSeat : attack.TargetSeat;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.CixiongDoubleSwords } decision || decision.PlayerSeat != responderSeat)
        {
            throw new InvalidOperationException("The pending AI Cixiong Double Swords prompt is inconsistent.");
        }

        var(choiceId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responderSeat].ChooseCixiongDoubleSwordsChoice(_corePlayerViewProjectionOperations.CreateSnapshot(responderSeat), attack.SourceSeat, attack.TargetSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        var selected = decision.Choices.Single(choice => choice.Id == choiceId);
        _coreNativeWeaponResponsesOperations.ResolveCixiongDoubleSwordsChoice(selected);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiStoneAxe()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveStoneAxe ?? throw new InvalidOperationException("AI Stone Axe response has no active resolution.");
        var attack = pending.Attack;
        if (!global::CardGame.Core.EngineOwners.CoreNativeDamageHandlesRuntime.SameAttackOwner(_coreActiveNativeAttackQueriesOperations.ActiveCardAttack, attack) || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.StoneAxe } decision || decision.PlayerSeat != attack.SourceSeat)
        {
            throw new InvalidOperationException("The pending AI Stone Axe prompt is inconsistent.");
        }

        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var(discardedCardIds, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[source.Seat].ChooseStoneAxeResponse(_corePlayerViewProjectionOperations.CreateSnapshot(source.Seat), attack.TargetSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        _coreNativeStoneAxeOperations.ResolveStoneAxeChoice(discardedCardIds);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiZhuqueFan()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("AI Zhuque Fan response has no FactionSlash continuation.");
        if (!pending.AwaitingZhuqueFanChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ZhuqueFan } decision || decision.PlayerSeat != pending.OwnerSeat || pending.TargetSeat is not { } targetSeat)
        {
            throw new InvalidOperationException("The pending AI Zhuque Fan prompt is inconsistent.");
        }

        var(choiceId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[pending.OwnerSeat].ChooseZhuqueFanChoice(_corePlayerViewProjectionOperations.CreateSnapshot(pending.OwnerSeat), targetSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        var selected = decision.Choices.Single(choice => choice.Id == choiceId);
        _coreNativeFactionSlashOperations.ResolveFactionSlashZhuqueFanChoice(pending, selected.Parameters.GetValueOrDefault("action") == "zhuque-fan-fire");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiBorrowedSword()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveBorrowedSword ?? throw new InvalidOperationException("AI Borrowed Sword response has no active resolution.");
        if (!pending.AwaitingSlashChoice || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } decision || decision.PlayerSeat != pending.WeaponOwnerSeat)
        {
            throw new InvalidOperationException("The pending AI Borrowed Sword prompt is inconsistent.");
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[pending.WeaponOwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[pending.SlashTargetSeat];
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(owner.Seat);
        var(useSlash, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[owner.Seat].ChooseBorrowedSwordResponse(view, pending.SourceSeat, target.Seat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        var canRequestFactionSlash = decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "faction-slash-request");
        if (useSlash && canRequestFactionSlash)
        {
            _coreNativeFactionSlashOperations.BeginBorrowedSwordFactionSlashRequest(pending);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (useSlash && decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") == "mou-xiao-qiao-forced-slash")is { } jueyanForced)
        {
            _characterXiaoQiaoMouOlOperations.BeginMouXqForcedSlash(jueyanForced);
            return;
        }

        if (useSlash && decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") == "round-once-cardless-forced-slash")is { } roundForced)
        {
            _coreRoundOnceCardlessNativeUsesOperations.TryResolveRoundOnceCardlessForcedSlash(roundForced, false);
            return;
        }

        if (useSlash && decision.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("response") == "tiered-round-zero-forced-slash")is { } zeroForced)
        {
            _coreTieredRoundZeroUsesOperations.TryResolveTieredRoundZeroForcedSlash(zeroForced, false);
            return;
        }

        var selectedSlash = useSlash ? _coreNativeBorrowedSwordOperations.GetBorrowedSwordSlashCards(owner, target).FirstOrDefault() : null;
        if (selectedSlash is not null)
        {
            _coreCardConversionsOperations.CaptureAiCardResponseChoice(decision, selectedSlash, _coreProgramCardLegalityOperations.CardLegality.SelectAiNativeSlashKind(new(decision, selectedSlash, _characterCaoZhiLeOperations.FuleForcedAiSlashKind(decision, selectedSlash, _coreOwnPublicPileConversionsOperations.PhysicalSlashUseBaseKind(owner, selectedSlash)))));
            _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordSlashChoice(pending, selectedSlash);
        }
        else if (useSlash && _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(owner).FirstOrDefault()is { } pair)
        {
            _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordZhangbaSlashChoice(pending, pair);
        }
        else if (useSlash && _characterChengJiChengCuiOlOperations.TousuiBorrowedSlashChoices(pending).FirstOrDefault()is { } tousui)
        {
            _characterChengJiChengCuiOlOperations.TryBeginTousuiBorrowedSlash(pending, tousui);
        }
        else if (useSlash && _characterMaXiuMaTieOlOperations.KenshangBorrowedSlashChoices(pending).FirstOrDefault()is { } kenshang)
        {
            _characterMaXiuMaTieOlOperations.TryBeginKenshangBorrowedSlash(pending, kenshang);
        }
        else if (useSlash && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, CardKind.Slash, forResponse: false).FirstOrDefault()is { } programSelection)
        {
            _coreNativeBorrowedSwordOperations.ResolveBorrowedSwordProgramMultiCardSlashChoice(pending, programSelection);
        }
        else
        {
            _coreNativeBorrowedSwordOperations.CompleteBorrowedSwordWithoutSlash(pending, transferWeapon: true);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiFactionSlash()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionCardRequest ?? throw new InvalidOperationException("AI FactionSlash response has no active request.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.RespondSlash } cost && cost.PlayerSeat == pending.OwnerSeat && cost.Choices.FirstOrDefault()?.Parameters.GetValueOrDefault("response") == "faction-request-cost")
        {
            _coreFactionRequestPaymentsOperations.ResolveFactionRequestCostChoice(cost.Choices.First(), false);
            return;
        }

        if (!pending.AwaitingProviders || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondSlash } decision || decision.PlayerSeat != pending.CurrentCandidateSeat)
        {
            throw new InvalidOperationException("The pending AI FactionSlash prompt is inconsistent.");
        }

        var provider = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var slashes = _coreCardConversionsOperations.GetFactionSlashSlashCards(pending, provider);
        var zhangbaPair = _coreAssistedFactionSlashOperations.GetFactionRequestZhangbaPairs(pending, provider).FirstOrDefault();
        var isResponse = !pending.IsProgramSkillUse && !pending.IsBorrowedSwordUse && !pending.IsQinglongCrescentBladeUse;
        var programSelection = _coreAssistedFactionSlashOperations.GetFactionRequestMultiCardSelections(pending, provider).FirstOrDefault();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(provider.Seat);
        var(useSlash, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[provider.Seat].ChooseFactionSlashResponse(view, pending.OwnerSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        if (pending.IsAssistedProgramUse)
        {
            var selected = useSlash ? decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("response") != "faction-slash-decline") : null;
            _coreAssistedFactionSlashOperations.ResolveAssistedFactionSlashChoice(selected ?? decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("response") == "faction-slash-decline"), false);
            return;
        }

        var selectedSlash = useSlash ? slashes.FirstOrDefault() : null;
        if (selectedSlash is not null)
            _coreCardConversionsOperations.CaptureAiCardResponseChoice(decision, selectedSlash, _coreCardConversionsOperations.GetFactionSlashEffectiveSlashKind(pending, provider, selectedSlash));
        if (useSlash && selectedSlash is null && zhangbaPair is not null)
        {
            _coreNativeFactionSlashOperations.ResolveFactionSlashZhangbaCandidateResponse(pending, provider, zhangbaPair);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (useSlash && selectedSlash is null && programSelection is not null)
        {
            _coreNativeFactionSlashOperations.ResolveFactionSlashProgramMultiCardCandidateResponse(pending, provider, programSelection);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        _coreNativeFactionSlashOperations.ResolveFactionSlashCandidateResponse(pending, useSlash: selectedSlash is not null, requestedCardId: selectedSlash?.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiFactionDefense()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFactionDefense ?? throw new InvalidOperationException("AI FactionDefense response has no active request.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.RespondDodge } decision || decision.PlayerSeat != pending.CurrentCandidateSeat)
        {
            throw new InvalidOperationException("The pending AI FactionDefense prompt is inconsistent.");
        }

        var provider = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var responseCards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(provider, CardKind.Dodge);
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(provider.Seat);
        var(useDodge, useBagua, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[provider.Seat].ChooseFactionDefenseResponse(view, pending.OwnerSeat, responseCards.Count > 0, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, legalConditionalBaguaAvailable: _coreNativeEquipmentAndFactionQueriesOperations.HasConditionalVirtualBaguaForAi(provider), legalBaguaAvailable: decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "faction-defense-bagua"));
        AddThought(thought);
        var selectedDodge = useDodge ? responseCards.FirstOrDefault() : null;
        if (selectedDodge is not null && !_coreNativeResponseMaterialQueriesOperations.CanUseCardAsResponse(provider, selectedDodge, CardKind.Dodge) && _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(provider, CardKind.Dodge, true).FirstOrDefault(selection => _characterGuanHaiOlOperations.IsXieweiSelection(selection) && selection.Cards[0].Id == selectedDodge.Id)is { } baitResponse && _characterGuanHaiOlOperations.TryCompleteXieweiFactionDefense(pending, provider, baitResponse))
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (selectedDodge is not null)
            _coreCardConversionsOperations.CaptureAiCardResponseChoice(decision, selectedDodge, _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(provider, selectedDodge, CardKind.Dodge));
        _coreNativeFactionDefenseOperations.ResolveFactionDefenseCandidateResponse(pending, useDodge: selectedDodge is not null, useBagua: useBagua && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(provider), requestedCardId: selectedDodge?.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiNullification()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveNullificationWindow ?? throw new InvalidOperationException("AI Nullification continuation is missing.");
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI Nullification continuation has no prompt.");
        if (decision.Kind != DecisionKind.Nullification || pending.CandidateIndex >= pending.CandidateSeats.Count || decision.PlayerSeat != pending.CandidateSeats[pending.CandidateIndex])
        {
            throw new InvalidOperationException("The pending AI Nullification prompt is inconsistent.");
        }

        var responder = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        if (responder.IsHuman)
        {
            throw new InvalidOperationException("A human Nullification prompt cannot be resolved as AI.");
        }

        if (_characterXiaoQiaoMouOlOperations.TryMouXqJueyanAiResponse(CardKind.Nullification))
            return;
        if (_coreTieredRoundZeroResponsesOperations.TryResolveTieredRoundZeroAiCounterspell())
            return;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(responder.Seat);
        var(cardId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseNullification(view, pending.DelayedReturn is not null ? pending.EffectCardKind : _coreNativeNullificationFramesOperations.GetNullificationEffectCard(pending).Kind, pending.SourceSeat, global::CardGame.Core.EngineOwners.CoreNativeNullificationFramesRuntime.GetNullificationTargetSeat(pending), pending.EffectNullified, pending.ChainDepth, decision.ValidCardIds, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, pending.TargetSeats, includeEquipment: _coreProgramCardPoliciesOperations.HasCardPolicy(responder, SkillProgramCardPolicyKind.UnrespondableNullification, CardKind.Nullification) || _coreProvenanceCardPoliciesOperations.HasProvenanceCounterspellMaterials(responder, decision.ValidCardIds), includeGrain: _coreProvenanceCardPoliciesOperations.HasProvenanceGrainCounterspellMaterials(responder, decision.ValidCardIds), originalPublicPileCardIds: view.PendingDecision?.Choices.Where(choice => choice.Parameters.GetValueOrDefault("native-original-public-pile") == "true").SelectMany(choice => choice.Cards).Distinct().ToArray());
        AddThought(thought);
        var selected = cardId is { } selectedCardId ? _coreNativeNullificationOperations.GetNullificationCards(responder).Single(card => card.Id == selectedCardId) : null;
        if (selected is not null && decision.Choices.FirstOrDefault(candidate => candidate.Cards.Contains(selected.Id) && candidate.Parameters.GetValueOrDefault("response") == "extended-view-as")is { } extendedChoice && !decision.Choices.Any(candidate => candidate.Cards is [var singleId] && singleId == selected.Id))
        {
            _coreExtendedViewAsOperations.ResolveExtendedViewAsResponse(extendedChoice);
            return;
        }

        var choice = selected is null ? null : decision.Choices.FirstOrDefault(candidate => candidate.Cards is [var publishedCardId] && publishedCardId == selected.Id && candidate.Parameters.GetValueOrDefault("response") == "nullification") ?? throw new InvalidOperationException("The AI Nullification response has no published source choice.");
        var selectedConversionSource = choice is null ? null : global::CardGame.Core.EngineOwners.CoreNativeNullificationRuntime.ReadPublishedNullificationSource(choice);
        if (selected is not null)
            _coreNativeNullificationOperations.ValidateNullificationConversion(responder, selected, selectedConversionSource);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeNullificationOperations.ResolveNullificationChoice(pending, responder, selected, selectedConversionSource);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiHarvest()
    {
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("AI FiveGrains selection has no pending draft.");
        if (group.Effect != GroupCardEffect.PublicDraft || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.Kind != DecisionKind.SelectHarvestCard)
        {
            throw new InvalidOperationException("The pending AI decision is not a FiveGrains selection.");
        }

        var picker = _coreMatchParticipantsPlayersAccess._players[group.TargetSeats[group.TargetIndex]];
        if (picker.IsHuman)
        {
            throw new InvalidOperationException("A human FiveGrains picker cannot be resolved as AI.");
        }

        var processing = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing);
        var options = group.RevealedCardIds.Select(cardId => processing.Single(card => card.Id == cardId)).Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(picker.Seat);
        var(cardId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[picker.Seat].ChooseHarvestCard(view, options, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeHarvestOperations.ResolveHarvestSelection(group, picker, cardId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiTargetCardSelection()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveTargetCardSelection ?? throw new InvalidOperationException("AI hidden target-card selection has no pending resolution.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.SelectTargetCard } decision || decision.PlayerSeat != pending.SourceSeat || _coreMatchParticipantsPlayersAccess._players[pending.SourceSeat].IsHuman)
        {
            throw new InvalidOperationException("The pending AI hidden target-card choice is inconsistent.");
        }

        var sourceView = _corePlayerViewProjectionOperations.CreateSnapshot(pending.SourceSeat);
        var(choiceId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[pending.SourceSeat].ChooseTargetCardSlot(sourceView, pending.TargetSeat, pending.ActionKind, sourceView.PendingDecision is { } projected && projected.PromptId == decision.PromptId ? projected.Choices : decision.Choices, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
        AddThought(thought);
        var choice = decision.Choices.SingleOrDefault(candidate => candidate.Id == choiceId) ?? throw new InvalidOperationException("The AI selected a hidden target-card choice that was not published.");
        if (!choice.Parameters.TryGetValue("slot-index", out var slotText) || !int.TryParse(slotText, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out var slot))
        {
            throw new InvalidOperationException("The AI selected a malformed hidden target-card slot.");
        }

        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreNativeTrickEffectsOperations.ResolveTargetCardSelection(pending, slot);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiFireAttack()
    {
        var pending = _coreNativeCardContinuationCursorsOperations.ActiveFireAttack ?? throw new InvalidOperationException("AI FireAttack selection has no pending resolution.");
        var decision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("AI FireAttack selection has no pending prompt.");
        if (decision.Kind is not (DecisionKind.FireAttackReveal or DecisionKind.FireAttackDiscard) || _coreMultiplayerOperations.IsHumanControlledSeat(decision.PlayerSeat))
        {
            throw new InvalidOperationException("The pending AI decision is not a FireAttack selection.");
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        if (actor.IsHuman)
        {
            throw new InvalidOperationException("A human FireAttack selection cannot be resolved as AI.");
        }

        if (_coreColorFireAttacksOperations.TryResolveAiColorFireAttack(pending, decision))
            return;
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(actor.Seat);
        if (decision.Kind == DecisionKind.FireAttackReveal)
        {
            var(cardId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseFireAttackReveal(view, decision.ValidCardIds, pending.SourceSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
            AddThought(thought);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeFireAttackOperations.ResolveFireAttackReveal(pending, cardId);
        }
        else
        {
            var revealed = _coreNativeFireAttackOperations.GetFireAttackRevealedCard(pending);
            var(cardId, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[actor.Seat].ChooseFireAttackDiscard(view, global::CardGame.Core.EngineOwners.CoreNativeFireAttackRuntime.GetFireAttackTargetSeat(pending), revealed.Suit, decision.ValidCardIds, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence);
            AddThought(thought);
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            _coreNativeFireAttackOperations.ResolveFireAttackDiscard(pending, cardId);
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiDuel()
    {
        var duel = _coreNativeCardContinuationCursorsOperations.ActiveDuel ?? throw new InvalidOperationException("AI Duel continuation has no pending Duel.");
        var responder = _coreMatchParticipantsPlayersAccess._players[duel.ResponderSeat];
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "faction-slash-request") == true)
        {
            _coreNativeFactionSlashOperations.BeginFactionSlashResponseRequest(duel.Attack);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var slashes = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, CardKind.Slash);
        var zhangbaPair = _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(responder).FirstOrDefault();
        var programSelection = _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Slash, forResponse: true).FirstOrDefault();
        var responseDecision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("The AI Duel response has no published choice.");
        if (_coreFriendDuelResponsesOperations.FriendDuelResponseChoices(duel, responder).FirstOrDefault()is { } friendResponse && slashes.Count == 0 && zhangbaPair is null && programSelection is null)
        {
            _coreFriendDuelResponsesOperations.ResolveFriendDuelDiscard(friendResponse);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        _coreNativeResponseWindowsOperations.PopResponseWindow(duel.ResolutionId);
        _coreNativeResponseWindowsOperations.SetResponseParentStep(duel.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        if (slashes.Count == 0 && zhangbaPair is null && programSelection is null)
        {
            _coreNativeDuelOperations.ResolveDuelResponse(duel, responder, selectedSlash: null);
        }
        else
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(responder.Seat);
            var(useSlash, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseDuelResponse(view, duel.OpponentSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, incomingPrediction: _corePublicDamagePredictionOperations.PredictPublicIncomingDamage(duel.Attack, responder.Seat, sourceSeat: duel.OpponentSeat, targetSeat: responder.Seat));
            AddThought(thought);
            if (useSlash && slashes.Count > 0)
            {
                _coreCardConversionsOperations.CaptureAiCardResponseChoice(responseDecision, slashes[0], _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, slashes[0], CardKind.Slash));
                _coreNativeDuelOperations.ResolveDuelResponse(duel, responder, slashes[0]);
            }
            else if (useSlash && zhangbaPair is not null)
            {
                _coreNativeDuelOperations.ResolveDuelZhangbaResponse(duel, responder, zhangbaPair);
            }
            else if (useSlash && programSelection is not null)
            {
                _coreCardConversionsOperations.ResolveDuelProgramMultiCardResponse(duel, responder, programSelection);
            }
            else
            {
                _coreNativeDuelOperations.ResolveDuelResponse(duel, responder, selectedSlash: null);
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiGroupAttack()
    {
        var group = _coreNativeCardContinuationCursorsOperations.ActiveGroupCard ?? throw new InvalidOperationException("AI group response has no pending group attack.");
        var attack = group.CurrentAttack ?? throw new InvalidOperationException("AI group response has no current target.");
        var responder = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "faction-slash-request") == true)
        {
            _coreNativeFactionSlashOperations.BeginFactionSlashResponseRequest(attack);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "faction-defense-request") == true)
        {
            _coreNativeFactionDefenseOperations.BeginFactionDefenseRequest(attack);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var requiredCardKind = group.RequiredCardKind ?? throw new InvalidOperationException("A group response attack must declare a required card kind.");
        var responseCards = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(responder, requiredCardKind);
        var zhangbaPair = requiredCardKind == CardKind.Slash ? _coreNativeResponseMaterialQueriesOperations.GetZhangbaSlashPairs(responder).FirstOrDefault() : null;
        var programSelection = requiredCardKind == CardKind.Slash ? _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(responder, CardKind.Slash, forResponse: true).FirstOrDefault() : null;
        var hasBagua = requiredCardKind == CardKind.Dodge && !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(responder) && _corePendingDecisionPolicyPipelineOperations._pendingDecision!.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "bagua");
        var responseDecision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("The AI group response has no published choice.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(group.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(group.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        if (responseCards.Count == 0 && zhangbaPair is null && programSelection is null && !hasBagua)
        {
            _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, responder, selectedResponse: null);
        }
        else if (requiredCardKind == CardKind.Dodge && hasBagua)
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(responder.Seat);
            var(useDodge, useBagua, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseDodgeResponse(view, group.SourceSeat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, legalDodgeAvailable: responseCards.Count > 0, legalConditionalBaguaAvailable: _coreNativeEquipmentAndFactionQueriesOperations.HasConditionalVirtualBaguaForAi(responder), incomingPrediction: _corePublicDamagePredictionOperations.PredictPublicIncomingDamage(attack, responder.Seat), legalBaguaAvailable: hasBagua);
            AddThought(thought);
            if (useDodge)
            {
                var card = responseCards[0];
                if (_coreExtendedViewAsOperations.TryResolveExtendedAiResponse(responder, card, requiredCardKind))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                    return;
                }

                _coreCardConversionsOperations.CaptureAiCardResponseChoice(responseDecision, card, _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, card, requiredCardKind));
                _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, responder, card);
            }
            else if (useBagua)
            {
                var judgmentResult = _coreNativeJudgmentOperations.ResolveBaguaJudgment(attack, responder);
                if (judgmentResult is { } succeeded)
                {
                    _coreNativeJudgmentOperations.CompleteBaguaResponse(attack, succeeded);
                }
            }
            else
            {
                _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, responder, selectedResponse: null);
            }
        }
        else
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(responder.Seat);
            var(useResponse, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[responder.Seat].ChooseGroupResponse(view, group.SourceSeat, group.Card.Kind, requiredCardKind, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, incomingPrediction: _corePublicDamagePredictionOperations.PredictPublicIncomingDamage(attack, responder.Seat));
            AddThought(thought);
            if (useResponse && responseCards.Count > 0)
            {
                if (_coreExtendedViewAsOperations.TryResolveExtendedAiResponse(responder, responseCards[0], requiredCardKind))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                    return;
                }

                _coreCardConversionsOperations.CaptureAiCardResponseChoice(responseDecision, responseCards[0], _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(responder, responseCards[0], requiredCardKind));
                _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, responder, responseCards[0]);
            }
            else if (useResponse && zhangbaPair is not null)
            {
                _coreNativeGroupResponsesOperations.ResolveGroupZhangbaResponse(group, responder, zhangbaPair);
            }
            else if (useResponse && programSelection is not null)
            {
                _coreCardConversionsOperations.ResolveGroupProgramMultiCardResponse(group, responder, programSelection);
            }
            else
            {
                _coreNativeGroupResponsesOperations.ResolveGroupResponse(group, responder, selectedResponse: null);
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void ResolvePendingAiDodge()
    {
        if (_coreRoundOnceCardlessReturnsOperations.TryResolveRoundOnceCardlessAiDodge())
            return;
        if (_characterXiaoQiaoMouOlOperations.TryMouXqJueyanAiResponse(CardKind.Dodge))
            return;
        if (_coreTieredRoundZeroResponsesOperations.TryResolveTieredRoundZeroAiDodge())
            return;
        var attack = _coreActiveNativeAttackQueriesOperations.ActiveCardAttack ?? throw new InvalidOperationException("AI Dodge continuation has no pending Slash.");
        var source = _coreMatchParticipantsPlayersAccess._players[attack.SourceSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[attack.TargetSeat];
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "faction-defense-request") == true)
        {
            _coreNativeFactionDefenseOperations.BeginFactionDefenseRequest(attack);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            return;
        }

        var responseDecision = _corePendingDecisionPolicyPipelineOperations._pendingDecision ?? throw new InvalidOperationException("The AI Dodge response has no published choice.");
        _coreNativeResponseWindowsOperations.PopResponseWindow(attack.ResolutionId);
        _coreNativeCardUseFramesOperations.SetCardUseStep(attack.ResolutionId, ResolutionFrameStep.ResolvingEffect);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = null;
        var dodges = _coreNativeResponseMaterialQueriesOperations.GetResponseCards(target, CardKind.Dodge);
        if (_coreConvertedSlashResponseOperations.ConvertedSlashSameColorResponseColor(attack)is { } requireRed)
            dodges = dodges.Where(card => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsRedSuit(_coreNativeCardAppearanceOperations.EffectiveSuit(target, card)) == requireRed).ToList();
        var dodge = dodges.FirstOrDefault();
        var hasBagua = !attack.IgnoresArmor && _coreNativeEquipmentAndFactionQueriesOperations.HasBagua(target) && responseDecision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("response") == "bagua");
        if (!target.IsAlive || (dodge is null && !hasBagua))
        {
            if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
            {
                _coreNativeAttackCompletionOperations.CompleteAttack(attack);
            }
        }
        else
        {
            var view = _corePlayerViewProjectionOperations.CreateSnapshot(target.Seat);
            var(useDodge, useBagua, thought) = _coreAiBrainsAiBrainsAccess._aiBrains[target.Seat].ChooseDodgeResponse(view, source.Seat, ++_coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._thoughtSequence, attack.IgnoresArmor, legalDodgeAvailable: dodge is not null, legalConditionalBaguaAvailable: _coreNativeEquipmentAndFactionQueriesOperations.HasConditionalVirtualBaguaForAi(target), incomingPrediction: _corePublicDamagePredictionOperations.PredictPublicIncomingDamage(attack, target.Seat), legalBaguaAvailable: hasBagua);
            AddThought(thought);
            if (useDodge)
            {
                if (_coreExtendedViewAsOperations.TryResolveExtendedAiResponse(target, dodge!, CardKind.Dodge))
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
                    return;
                }

                _coreCardConversionsOperations.CaptureAiCardResponseChoice(responseDecision, dodge!, _coreNativeResponseMaterialQueriesOperations.GetEffectiveResponseKind(target, dodge!, CardKind.Dodge));
                _coreNativeDodgeOperations.ResolveDodgeResponse(attack, target, dodge!);
            }
            else if (useBagua)
            {
                var judgmentResult = _coreNativeJudgmentOperations.ResolveBaguaJudgment(attack, target);
                if (judgmentResult is { } succeeded)
                {
                    _coreNativeJudgmentOperations.CompleteBaguaResponse(attack, succeeded);
                }
            }
            else
            {
                if (!_coreNativeDamageApplicationOperations.ApplyAttackDamage(attack))
                {
                    _coreNativeAttackCompletionOperations.CompleteAttack(attack);
                }
            }
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal static int GetKeepValue(Card card, CharacterState owner)
    {
        var value = CardCatalog.Get(card.Kind).HandKeepValue;
        return card.Kind == CardKind.Peach && owner.Hp < owner.MaxHp ? value + 50 : value;
    }

    internal void NotifyAiOfSlash(CharacterState source, CharacterState target)
    {
        if (_characterSimaYiMouOperations.HoldMouSmAiAttack(source.Seat, target.Seat))
            return;
        NotifyAiOfPublicAttack(source, target, PublicAttackKind.Slash);
    }

    internal void NotifyAiOfDuel(CharacterState source, CharacterState target)
    {
        NotifyAiOfPublicAttack(source, target, PublicAttackKind.Duel);
    }

    internal void NotifyAiOfFireAttack(CharacterState source, CharacterState target)
    {
        NotifyAiOfPublicAttack(source, target, PublicAttackKind.FireAttack);
    }

    internal void NotifyAiOfPublicAttack(CharacterState source, CharacterState target, PublicAttackKind kind)
    {
        if (_coreNativeDamageHandlesOperations.IsSurvivalMode)
            return;
        if (_coreNativeDamageHandlesOperations.IsNationalWarMode)
        {
            NotifyAiOfNationalAttack(source, target);
            return;
        }

        var lordSeat = _coreNativeDamageHandlesOperations.IsTeamMode ? -1 : _coreMatchParticipantsPlayersAccess._players.Single(player => player.Role == Role.Lord).Seat;
        var visibleTargetRole = _coreNativeDamageHandlesOperations.IsTeamMode ? null : target.RoleRevealed || target.Role == Role.Lord ? target.Role : (Role? )null;
        var evidence = new PublicAttackEvidence(kind, source.Seat, target.Seat, lordSeat, visibleTargetRole);
        foreach (var brain in _coreAiBrainsAiBrainsAccess._aiBrains.Values)
        {
            if (kind == PublicAttackKind.FireAttack)
            {
                brain.ObserveFireAttack(evidence);
            }
            else
            {
                brain.ObservePublicAttack(evidence);
            }
        }
    }

    internal void NotifyAiOfGroupAttack(CharacterState source, IReadOnlyList<int> targets)
    {
        if (_coreNativeDamageHandlesOperations.IsNationalWarMode)
        {
            foreach (var targetSeat in targets)
            {
                NotifyAiOfPublicAttack(source, _coreMatchParticipantsPlayersAccess._players[targetSeat], PublicAttackKind.GroupAttack);
            }

            return;
        }

        foreach (var targetSeat in targets)
        {
            NotifyAiOfPublicAttack(source, _coreMatchParticipantsPlayersAccess._players[targetSeat], PublicAttackKind.GroupAttack);
        }
    }

    internal void NotifyAiOfNationalAttack(CharacterState source, CharacterState target)
    {
        var visibleSourceFaction = source.FactionRevealed ? source.NationalFactionId : null;
        var visibleTargetFaction = target.FactionRevealed ? target.NationalFactionId : null;
        foreach (var brain in _coreAiBrainsAiBrainsAccess._aiBrains.Values)
        {
            brain.ObserveNationalAttack(source.Seat, target.Seat, _coreMatchParticipantsPlayersAccess._players[brain.Seat].NationalFactionId, visibleSourceFaction, visibleTargetFaction);
        }
    }

    internal void NotifyAiOfDeath(CharacterState? killer, CharacterState victim)
    {
        foreach (var brain in _coreAiBrainsAiBrainsAccess._aiBrains.Values)
        {
            brain.ObserveDeath(killer?.Seat, victim.Role);
        }
    }

    internal void ResolvePendingAiSkipDiscardPolicy()
    {
        if (!_coreNativePromptBoundaryOperations.IsAiSkipDiscardPolicyPending())
        {
            throw new InvalidOperationException("There is no AI discard-policy choice to resolve.");
        }

        _coreActualDiscardPhaseOperations.ResolveSkipDiscardPolicyChoice(useSkill: true);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    internal void AddThought(AiThoughtRecord thought)
    {
        _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._aiThoughts.Add(thought);
        _coreCommandJournalStoragePendingNotificationsAccess._pendingNotifications.Enqueue(new AiThoughtNotification(thought));
    }

    internal void AddGeneralThought(AiGeneralThought thought)
    {
        _coreAiDiagnosticsAiGeneralThoughtsAiThoughtsThoughtSequenceAccess._aiGeneralThoughts.Add(thought);
        _coreCommandJournalStoragePendingNotificationsAccess._pendingNotifications.Enqueue(new AiGeneralThoughtNotification(thought));
    }
}
