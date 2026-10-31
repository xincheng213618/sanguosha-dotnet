#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFriendEventsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IFriendEventProgramHost.ExecuteFriendEvent(ProgramSkillFrame frame, SkillProgramEffectOp operation) => BeginFriendEvent(frame, operation);
    internal Card? UniqueMaximumHandCard(int seat)
    {
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
        if (hand.Count == 0)
            return null;
        var rank = hand.Max(c => _characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[seat], c) ?? c.Rank);
        return hand.Where(c => (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[seat], c) ?? c.Rank) == rank).ToArray()is [var card] ? card : null;
    }

    internal bool FriendHandDiscardAllowed(int seat, Card card) => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(seat, card, CardLocation.Hand(seat), OwnedCardMoveIntent.Discard);
    internal int AscendingHandRank(int seat, string skillId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<AscendingHandRankPaidEvent>().Where(e => e.OwnerSeat == seat && e.SkillId == skillId).Select(e => e.Rank).DefaultIfEmpty(0).Max();
    internal int FriendSuitUseCount(int seat, string skillId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<UsedSuitHandHeldEvent>().Count(e => e.OwnerSeat == seat && e.SkillId == skillId && e.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber);
    internal bool FriendSuitAllowed(int seat, Suit suit) => suit == Suit.Heart || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FriendEventConfiguredEvent>().Any(e => e.OwnerSeat == seat && e.SkillId == "friend:cui-jun-gongli" && e.Option == suit.ToString());
    internal bool FriendDuelCategoryAllowed(int seat, CardKind kind) => !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FriendEventConfiguredEvent>().Any(e => e.OwnerSeat == seat && e.SkillId == "friend:shi-tao-gongli" && e.Option == global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind).ToString());
    internal void CaptureFriendMinimumHandUse(CardActionContext? action)
    {
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.HoldUsedSuitHandAndDraw) || action is not { PhysicalCards.Count: 1 } || !_coreFriendTrioOperations.IsFriendActualCardUse(action) || action.PhysicalCards[0].From != CardLocation.Hand(action.ActorSeat) || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], "friend:shunyi"))
            return;
        var card = _coreCardAttackStateOperations.GetAttackCard(action.PhysicalCards[0].CardId);
        var rank = action.EffectiveRank ?? _characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], card) ?? card.Rank;
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).Where(c => c.Id != card.Id).All(c => (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], c) ?? c.Rank) > rank))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FriendMinimumHandUseEvent(action.ActionId, action.ActorSeat, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], "friend:shunyi"), rank, action.EffectiveSuit ?? _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], card)));
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 900)]
    internal bool CanOfferFriendEvent(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.DiscardAscendingMaximumAndDraw))
            return UniqueMaximumHandCard(candidate.OwnerSeat)is { } highest && (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat], highest) ?? highest.Rank) > AscendingHandRank(candidate.OwnerSeat, candidate.SkillId) && FriendHandDiscardAllowed(candidate.OwnerSeat, highest);
        if (!trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.HoldUsedSuitHandAndDraw))
            return true;
        var action = _coreFriendTrioOperations.FriendCardAction(context);
        if (action is not { PhysicalCards.Count: 1 } || !_coreFriendTrioOperations.IsFriendActualCardUse(action) || action.ActorSeat != candidate.OwnerSeat || action.PhysicalCards[0].From != CardLocation.Hand(candidate.OwnerSeat))
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FriendMinimumHandUseEvent>().Any(e => e.ActionId == action.ActionId && e.OwnerSeat == candidate.OwnerSeat && e.SkillInstanceId == candidate.SkillInstanceId && e.Rank > FriendSuitUseCount(candidate.OwnerSeat, candidate.SkillId) && FriendSuitAllowed(candidate.OwnerSeat, e.Suit));
    }

    internal SkillProgramStepOutcome BeginFriendEvent(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var plan = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!);
        if (f.FriendEventFlow is not null || f.InstructionIndex != 1 || plan.Instructions is not [{ } instruction] || instruction.Op != operation)
            throw new InvalidOperationException("A friend event requires its exact once-only instruction.");
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (operation is SkillProgramEffectOp.ConfigureFriendDuelCategories or SkillProgramEffectOp.ConfigureFriendUseSuits)
        {
            var count = Math.Min(3, _coreMatchParticipantsPlayersAccess._players.Sum(p => (p.General.Id.StartsWith("friend:", StringComparison.Ordinal) ? 1 : 0) + (p.SecondaryGeneral?.Id.StartsWith("friend:", StringComparison.Ordinal) == true ? 1 : 0)));
            if (count == 0)
                return SkillProgramStepOutcome.Continue;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = new(f.InstructionIndex, operation, FriendEventFlowStage.Configuration, Count: count) });
            PublishFriendConfiguration(f);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (operation == SkillProgramEffectOp.RecastOwnedCardsAndDuel)
        {
            if (f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || target == f.OwnerSeat || !_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(target, f.OwnerSeat) || f.SelectedCardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id).OwnerSeat != f.OwnerSeat || _cardZonesOperations._cardZones.GetLocation(id).Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
                throw new InvalidOperationException("Recast Duel requires exact owned HE cards and a legal other target.");
            f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, f.OwnerSeat, f.SelectedCardIds);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = new(1, operation, FriendEventFlowStage.RecastCost, Count: f.SelectedCardIds.Count, TargetSeat: target), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(f.SelectedCardIds, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
        }
        else if (operation == SkillProgramEffectOp.DiscardMaximumAndPunishHigherHand)
        {
            if (f.TriggerId is not null || f.SelectedCardIds is not [var selected] || f.SelectedTargetSeats is not [var target] || UniqueMaximumHandCard(f.OwnerSeat)is not { } card || card.Id != selected || !FriendHandDiscardAllowed(f.OwnerSeat, card))
                throw new InvalidOperationException("Maximum hand payment must be the unique maximum physical hand card.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = new(1, operation, FriendEventFlowStage.MaximumCost, Rank: _characterGuanYuWumiaoOperations.JuewuForeignHandRank(owner, card) ?? card.Rank, TargetSeat: target), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, CardLocation.Hand(f.OwnerSeat), new("skill.friend-biwei.cost"));
        }
        else if (operation == SkillProgramEffectOp.DiscardAscendingMaximumAndDraw)
        {
            if (UniqueMaximumHandCard(f.OwnerSeat)is not { } card || !FriendHandDiscardAllowed(f.OwnerSeat, card))
                return SkillProgramStepOutcome.Continue;
            var paidRank = _characterGuanYuWumiaoOperations.JuewuForeignHandRank(owner, card) ?? card.Rank;
            if (paidRank <= AscendingHandRank(f.OwnerSeat, f.SkillId))
                return SkillProgramStepOutcome.Continue;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = new(1, operation, FriendEventFlowStage.AscendingCost, Rank: paidRank), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, CardLocation.Hand(f.OwnerSeat), new("skill.friend-lunxiong.cost"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AscendingHandRankPaidEvent(f.OwnerSeat, f.SkillId, card.Id, paidRank));
        }
        else if (operation == SkillProgramEffectOp.HoldUsedSuitHandAndDraw)
        {
            var action = _coreFriendTrioOperations.FriendCardAction(f.WindowContext!) ?? throw new InvalidOperationException("Suit hand hold lost its Use owner.");
            var suit = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FriendMinimumHandUseEvent>().Single(e => e.ActionId == action.ActionId && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId).Suit;
            var grant = owner.SkillGrants.Grants.Single(g => g.SkillId == f.SkillId && g.SkillInstanceId == f.SkillInstanceId);
            var location = new CardLocation(CardZoneKind.PrivateTurnHold, f.OwnerSeat, privateTurnHold: new(f.Id, f.SkillId, f.SkillInstanceId, grant.SourceId, _coreActualTurnProgressTurnNumberAccess._turnNumber));
            _cardZonesOperations._cardZones.EnsurePrivateTurnHold(location);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = new(1, operation, FriendEventFlowStage.SuitHold, Suit: suit), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCards(_corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => _coreNativeCardAppearanceOperations.EffectiveSuit(owner, c) == suit).ToArray(), CardLocation.Hand(f.OwnerSeat), location, new("skill.friend-shunyi.hold"));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new UsedSuitHandHeldEvent(f.OwnerSeat, f.SkillId, action.ActionId, _coreActualTurnProgressTurnNumberAccess._turnNumber, suit));
        }
        else
            throw new InvalidOperationException("Unsupported friend event operation.");
        ContinueFriendEventMovement(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void ContinueFriendEventMovement(long id)
    {
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30)]
    internal bool ReturnFriendEventMovement(ProgramSkillFrame f)
    {
        if (f.FriendEventFlow is null || f.PendingMovementContinuation is null)
            return false;
        ContinueFriendEventMovement(f.Id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 30)]
    internal bool ResumeFriendEvent(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (f.FriendEventFlow is not { } flow)
            return false;
        if (flow.InstructionIndex != f.InstructionIndex || flow.InstructionIndex != 1 || ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect.Op != flow.Operation)
            throw new InvalidOperationException("Friend event lost its paid owning instruction.");
        if (flow.Stage == FriendEventFlowStage.Configuration)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishFriendConfiguration(f);
            return true;
        }

        if (flow.Stage == FriendEventFlowStage.DuelIssued)
            throw new InvalidOperationException("Recast Duel requires its typed Use return.");
        if (f.PendingMovementContinuation is not null)
        {
            ContinueFriendEventMovement(id);
            return true;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
        {
            if (flow.Stage == FriendEventFlowStage.RecastCost)
                f = _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(f, 0);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { FriendEventFlow = null });
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), "已付成本保留，武将技能失效。");
            return true;
        }

        if (flow.Stage is FriendEventFlowStage.RecastCost or FriendEventFlowStage.AscendingCost or FriendEventFlowStage.SuitHold)
        {
            var stage = flow.Stage == FriendEventFlowStage.RecastCost ? FriendEventFlowStage.RecastDraw : flow.Stage == FriendEventFlowStage.AscendingCost ? FriendEventFlowStage.AscendingDraw : FriendEventFlowStage.SuitDraw;
            var amount = flow.Stage == FriendEventFlowStage.RecastCost ? flow.Count : flow.Stage == FriendEventFlowStage.AscendingCost ? 3 : 1;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = flow with { Stage = stage }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            var drawn = _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], amount, true, stage == FriendEventFlowStage.RecastDraw ? CardMoveReasons.RecastDraw : CardMoveReasons.Draw);
            if (stage == FriendEventFlowStage.RecastDraw)
            {
                for (var i = 0; i < f.SelectedCardIds.Count; i++)
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(f.OwnerSeat, f.SelectedCardIds[i], _coreCardAttackStateOperations.GetAttackCard(f.SelectedCardIds[i]).Kind, i < drawn.Count ? 1 : 0));
                _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), drawn.Count);
            }

            ContinueFriendEventMovement(id);
            return true;
        }

        if (flow.Stage == FriendEventFlowStage.RecastDraw)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[flow.TargetSeat].IsAlive || !_coreDamageColorDuelProgramsOperations.CanIssueSelectedActorDuel(flow.TargetSeat, f.OwnerSeat))
            {
                FinishFriendEvent(f);
                return true;
            }

            var useId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
            var origin = new FriendRecastDuelOrigin(f.Id, f.InstructionIndex, f.SkillId, f.ActivationId, f.SkillInstanceId, f.GameplayHash, f.OwnerSeat, flow.TargetSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, flow.Count);
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { FriendEventFlow = flow with { Stage = FriendEventFlowStage.DuelIssued, UseFrameId = useId } });
            var card = new Card(0, CardKind.Duel, Suit.None, 0);
            var actual = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [flow.TargetSeat], CardKind.Duel, physicalCardIds: [], friendRecastDuel: origin);
            if (actual != useId)
                throw new InvalidOperationException("Recast Duel lost its reserved Use.");
            _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(useId, card, f.OwnerSeat, [flow.TargetSeat], LegalActionKind.Duel, playedCardKind: CardKind.Duel);
            return true;
        }

        if (flow.Stage == FriendEventFlowStage.MaximumCost)
        {
            var cards = _coreMatchParticipantsPlayersAccess._players[flow.TargetSeat].IsAlive ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[flow.TargetSeat]).Where(c => (_characterGuanYuWumiaoOperations.JuewuForeignHandRank(_coreMatchParticipantsPlayersAccess._players[flow.TargetSeat], c) ?? c.Rank) >= flow.Rank && FriendHandDiscardAllowed(flow.TargetSeat, c)).ToArray() : [];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = flow with { Stage = FriendEventFlowStage.TargetDiscard, Count = cards.Length }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscardCards(f.OwnerSeat, flow.TargetSeat, cards, CardLocation.Hand(flow.TargetSeat), new("skill.friend-biwei.target"));
            if (cards.Length == 0)
            {
                var activation = ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Activation!;
                var key = (f.OwnerSeat, f.SkillId, activation.UsageGroup);
                _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = Math.Max(0, _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key) - 1);
            }

            ContinueFriendEventMovement(id);
            return true;
        }

        FinishFriendEvent(f);
        return true;
    }

    internal void FinishFriendEvent(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { FriendEventFlow = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal IReadOnlyList<PromptChoice> FriendConfigurationChoices(ProgramSkillFrame f)
    {
        var options = f.FriendEventFlow!.Operation == SkillProgramEffectOp.ConfigureFriendDuelCategories ? new[]
        {
            "Basic",
            "Trick",
            "Equipment"
        }

        : new[]
        {
            "Spade",
            "Club",
            "Diamond"
        };
        var chosen = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<FriendEventConfiguredEvent>().Where(e => e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId).Select(e => e.Option).ToHashSet();
        return options.Where(o => !chosen.Contains(o)).Select(option => new PromptChoice(new($"friend-config.{(f.Id)}.{(option)}"), option switch
        {
            "Basic" => "基本牌",
            "Trick" => "锦囊牌",
            "Equipment" => "装备牌",
            "Spade" => "黑桃",
            "Club" => "梅花",
            _ => "方块"
        }, [], [], new Dictionary<string, string> { ["program-action"] = "friend-configuration", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option })).ToArray();
    }

    internal void PublishFriendConfiguration(ProgramSkillFrame f)
    {
        var choices = FriendConfigurationChoices(f);
        if (choices.Count == 0 || f.FriendEventFlow!.Count <= 0)
        {
            FinishFriendEvent(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.FriendEventFlow.Operation == SkillProgramEffectOp.ConfigureFriendDuelCategories ? "共砺：选择钦英本局禁止弃置的类别。" : "共砺：选择顺逸本局额外可触发的花色。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("friend-configuration")]
    internal void ResolveFriendConfiguration(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FriendEventFlow.Stage: FriendEventFlowStage.Configuration } f || !FriendConfigurationChoices(f).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Friend configuration requires its exact current option.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new FriendEventConfiguredEvent(f.OwnerSeat, f.SkillId, f.SkillInstanceId, selected.Parameters["option"]));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { FriendEventFlow = f.FriendEventFlow!with { Count = f.FriendEventFlow.Count - 1 } });
        PublishFriendConfiguration(f);
    }

    internal bool IsFriendRecastDuelUse(long id)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(id)is not { CardId: 0, CardKind: CardKind.Duel, PhysicalCardIds.Count: 0, FriendRecastDuel: { } o, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Duel, PhysicalCards.Count: 0 } } use)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramSkillFrame f && f.Id == o.ProgramFrameId && f.OwnerSeat == o.OwnerSeat && f.SkillId == o.SkillId && f.ActivationId == o.BindingId && f.SkillInstanceId == o.SkillInstanceId && f.GameplayHash == o.GameplayHash && f.InstructionIndex == o.InstructionIndex && f.SelectedCardIds.Count == o.RecastCount && f.SelectedTargetSeats.SequenceEqual([o.OriginalTargetSeat]) && o.TurnNumber == _coreActualTurnProgressTurnNumberAccess._turnNumber && f.FriendEventFlow is { Stage: FriendEventFlowStage.DuelIssued } flow && flow.UseFrameId == use.Id && flow.Count == o.RecastCount && ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).Instructions is [{ Op: SkillProgramEffectOp.RecastOwnedCardsAndDuel }];
    }

    [NativeUseReturn(260)]
    internal void ReturnFriendRecastDuel(CardUseFrame use)
    {
        if (use.FriendRecastDuel is not { AttackStarted: false } o)
            return;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FriendEventFlow.Stage: FriendEventFlowStage.DuelIssued } f || f.Id != o.ProgramFrameId || f.InstructionIndex != o.InstructionIndex || f.FriendEventFlow.UseFrameId != use.Id || f.SkillInstanceId != o.SkillInstanceId || f.GameplayHash != o.GameplayHash)
            throw new InvalidOperationException("Recast Duel lost its typed Program return.");
        FinishFriendEvent(f);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 60)]
    internal bool ReturnRegisteredCompleteFriendRecastDuelAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.FriendRecastDuelReturn is null)
            return false;
        CompleteFriendRecastDuelAttackReturn(completion);
        return true;
    }

    internal void CompleteFriendRecastDuelAttackReturn(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (completion.FriendRecastDuelReturn is not { AttackStarted: true } o || completion.ProgramFrameId != o.ProgramFrameId || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { FriendEventFlow.Stage: FriendEventFlowStage.DuelIssued } f || f.Id != o.ProgramFrameId || f.InstructionIndex != o.InstructionIndex || f.FriendEventFlow.UseFrameId != completion.ResolutionId || f.SkillId != o.SkillId || f.ActivationId != o.BindingId || f.SkillInstanceId != o.SkillInstanceId || f.GameplayHash != o.GameplayHash)
            throw new InvalidOperationException("Recast Duel lost its exact completed attack return.");
        FinishFriendEvent(f);
    }
}
