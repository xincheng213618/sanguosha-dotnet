#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXinXianYingMobileShiRuntime
{
    internal const string ShiXxyJiejie = "mobile:shi-jiejie", ShiXxyQingshi = "mobile:shi-qingshi";
    internal static int ShiXxyMask(IEnumerable<Suit> suits) => suits.Aggregate(0, (n, s) => n | (s switch
    {
        Suit.Spade => 1,
        Suit.Heart => 2,
        Suit.Club => 4,
        Suit.Diamond => 8,
        _ => 0
    }));
    internal static int ShiXxySuitCount(int mask) => System.Numerics.BitOperations.PopCount((uint)mask);
    internal static CardConversionSource ShiXxySource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal int ShiXxyRoundMask(int owner, int actor) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingViewedEvent>().Where(e => e.Source.OwnerSeat == owner && e.Actor == actor && e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber).Aggregate(0, (n, e) => n | e.SuitMask);
    internal int ShiXxyRoundUsed(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<ShiXinXianYingQingshiIssuedEvent>().Count(e => e.Source.OwnerSeat == owner && e.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber && e.FromJiejie);
    internal bool ShiXxySameCamp(int owner, int target)
    {
        if (owner == target)
            return true;
        var a = _coreMatchParticipantsPlayersAccess._players[owner];
        var b = _coreMatchParticipantsPlayersAccess._players[target];
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.Team)
            return a.TeamId is { } team && team == b.TeamId;
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.NationalWarLite)
            return _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(a) == _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(b);
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.Survival)
            return false;
        return a.Role is Role.Lord or Role.Loyalist && b.Role is Role.Lord or Role.Loyalist || a.Role == Role.Rebel && b.Role == Role.Rebel;
    }

    SkillProgramStepOutcome global::CardGame.Core.IShiXinXianYingHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op, int actor) => BeginShiXxy(f, op, actor);
    internal SkillProgramStepOutcome BeginShiXxy(ProgramSkillFrame supplied, SkillProgramEffectOp op, int actor)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.ShiXinXianYing is not null || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("Shi Xin Xian Ying requires her original unpaid instruction.");
        var jiejie = op == SkillProgramEffectOp.ResolveMobileShiXinXianYingJiejie;
        if (jiejie ? f.SkillId != ShiXxyJiejie || f.TriggerId is not null || actor != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || actor != (f.ActivatingSeat ?? f.OwnerSeat) : op != SkillProgramEffectOp.ResolveMobileShiXinXianYingQingshi || f.SkillId != ShiXxyQingshi || f.TriggerId != "damage-qingshi" || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, Amount: > 0 } c || c.TargetSeat != f.OwnerSeat)
            throw new InvalidOperationException("Jiejie needs its real current-phase activator; Qingshi needs its actual damage victim.");
        var hand = jiejie ? _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[actor]).Select(c => new ShiXinXianYingViewedCard(c.Id, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[actor], c))).ToArray() : [];
        var receipt = new ShiXinXianYingReceipt(ShiXxySource(f), f.GameplayHash, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, actor, jiejie ? ShiXinXianYingStage.Suit : ShiXinXianYingStage.Target)
        {
            Jiejie = jiejie,
            Viewed = hand
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = receipt });
        if (jiejie)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramPrivateHandViewedEvent(f.Id, f.SkillId, f.OwnerSeat, actor, hand.Length));
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXinXianYingViewedEvent(f.Id, receipt.Source, actor, _coreActualRoundProgressRoundNumberAccess._roundNumber, ShiXxyMask(hand.Select(c => c.Suit)), hand.Length));
        }

        PublishShiXxy(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShiXxyChoices(ProgramSkillFrame f)
    {
        var r = f.ShiXinXianYing!;
        PromptChoice C(string branch, string label, int? target = null, int? card = null, CardZoneKind? zone = null, int? index = null) => new(new($"shi-xin-xian-ying.{(f.Id)}.{(r.Stage)}.{(branch)}.{(target ?? -1)}.{(zone)}.{(index ?? card ?? -1)}"), label, card is { } id ? [id] : [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "shi-xin-xian-ying", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["zone"] = zone?.ToString() ?? "", ["index"] = (index ?? -1).ToString(CultureInfo.InvariantCulture) });
        if (r.Stage == ShiXinXianYingStage.Suit)
            return global::CardGame.Core.EngineOwners.CharacterXinXianYingMobileShiDefinitions.ShiXxySuits.Select(s => C(s.ToString(), s switch
            {
                Suit.Spade => "选择黑桃",
                Suit.Heart => "选择红桃",
                Suit.Club => "选择梅花",
                _ => "选择方块"
            })).Append(C("cancel", "不选择花色，结束诫节")).ToArray();
        if (r.Stage == ShiXinXianYingStage.Target)
            return _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => C("target", "对 " + p.Name + " 发动清识", p.Seat)).ToArray();
        if (r.Stage is not (ShiXinXianYingStage.OwnerDiscard or ShiXinXianYingStage.OtherDiscard))
            return[];
        var holder = r.Stage == ShiXinXianYingStage.OwnerDiscard ? f.OwnerSeat : r.Target;
        var result = new List<PromptChoice>();
        foreach (var zone in new[]
        {
            CardZoneKind.Hand,
            CardZoneKind.Equipment
        }

        )
        {
            var from = new CardLocation(zone, holder);
            var cards = _cardZonesOperations._cardZones.CardsAt(from);
            for (var index = 0; index < cards.Count; index++)
            {
                var card = cards[index];
                if (_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
                    continue;
                var hidden = holder != f.OwnerSeat && zone == CardZoneKind.Hand;
                result.Add(C("discard", hidden ? $"弃置 {(_coreMatchParticipantsPlayersAccess._players[holder].Name)} 的第{(index + 1)}张暗手牌" : "弃置【" + card.DisplayName + "】", holder, hidden ? null : card.Id, zone, index));
            }
        }

        return result;
    }

    internal void PublishShiXxy(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var r = f.ShiXinXianYing!;
        var choices = ShiXxyChoices(f);
        if (choices.Count == 0)
        {
            ContinueShiXxy(f.Id);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.AwaitingResponse });
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, r.Stage switch
        {
            ShiXinXianYingStage.Suit => "诫节：私下观看当前角色的全部手牌，选择一种花色（也可以不选）。",
            ShiXinXianYingStage.Target => "清识：选择一名角色，按双方实际阵营结算。可以选择自己。",
            ShiXinXianYingStage.OwnerDiscard => "清识：选择弃置你的一张手牌或装备牌。",
            _ => "清识：选择弃置对方的一张手牌或装备牌。"
        }, r.Stage == ShiXinXianYingStage.Suit ? r.Viewed.Select(c => c.CardId).ToArray() : choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            TargetSeat = r.Stage == ShiXinXianYingStage.Suit ? r.Actor : r.Target < 0 ? null : r.Target,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("shi-xin-xian-ying")]
    internal void AnswerShiXxy(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiXinXianYing: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShiXxyChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Shi Xin Xian Ying rejects a stale choice or an invented foreign hand entity.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = choice.Parameters["branch"];
        if (r.Stage == ShiXinXianYingStage.Suit)
        {
            if (branch == "cancel")
            {
                CompleteShiXxyJiejie(f);
                return;
            }

            var suit = Enum.Parse<Suit>(branch);
            var included = r.Viewed.Any(c => c.Suit == suit);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiXinXianYingSuitChosenEvent(f.Id, r.Actor, suit, included));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = r = r with { Stage = ShiXinXianYingStage.JiejieChildren, ChosenSuit = suit, Before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(r.Actor, 0, null) });
            if (included)
            {
                var ids = _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(_coreMatchParticipantsPlayersAccess._players[r.Actor]).Where(c => _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[r.Actor], c) != suit).Select(c => c.Id).ToArray();
                if (ids.Length > 0)
                    _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Actor, ids, new("skill-program.mobile-shi-jiejie.discard"));
            }
            else
            {
                var pool = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Concat(_cardZonesOperations._cardZones.CardsAt(CardLocation.DiscardPile)).Where(c => c.Suit == suit).ToArray();
                if (pool.Length > 0)
                {
                    var card = pool[_randomOperations._random.Next(pool.Length)];
                    _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.Hand(r.Actor), new("skill-program.mobile-shi-jiejie.obtain"));
                }
            }

            SealShiXxy(f.Id);
            ContinueShiXxy(f.Id);
            return;
        }

        if (r.Stage == ShiXinXianYingStage.Target)
        {
            IssueShiXxyQingshi(f, choice.Targets.Single());
            return;
        }

        if (r.Stage is ShiXinXianYingStage.OwnerDiscard or ShiXinXianYingStage.OtherDiscard)
        {
            var holder = r.Stage == ShiXinXianYingStage.OwnerDiscard ? f.OwnerSeat : r.Target;
            var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["zone"]), holder);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["index"], CultureInfo.InvariantCulture)];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiXinXianYing = r with { Selected = r.Selected.Append(new(card.Id, from)).ToArray(), Stage = r.Stage == ShiXinXianYingStage.OwnerDiscard ? ShiXinXianYingStage.OtherDiscard : ShiXinXianYingStage.QingDiscardChildren } });
            ContinueShiXxy(f.Id);
        }
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 48200)]
    internal IEnumerable<CardSnapshot> ShiXxyPrivateCards(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.ShiXinXianYing is { Stage: ShiXinXianYingStage.Suit }).SelectMany(f => f.ShiXinXianYing!.Viewed).Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId).Zone == CardZoneKind.Hand).DistinctBy(c => c.CardId).Select(c => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId))).ToArray();
}
