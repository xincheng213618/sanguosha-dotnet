#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShaoMobileShiRuntime
{
    internal const string ShiSsGanjue = ShiSunShaoComposition.GanjueSkill, ShiSsZhuji = ShiSunShaoComposition.ZhujiSkill;
    internal static CardConversionSource ShiSsSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome global::CardGame.Core.IShiSunShaoHost.ResolveGanjue(ProgramSkillFrame frame) => BeginShiSsGanjue(frame);
    SkillProgramStepOutcome global::CardGame.Core.IShiSunShaoHost.ResolveZhuji(ProgramSkillFrame frame) => BeginShiSsZhuji(frame);
    internal SkillProgramStepOutcome BeginShiSsGanjue(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SunShao is not null)
            throw new InvalidOperationException("Shi Sun Shao restarted a running operation.");
        if (f.SkillId != ShiSsGanjue || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunShao = new("ganjue", _coreActualTurnProgressTurnNumberAccess._turnNumber, ShiSsStage.GanjueMaterial) });
        PublishShiSs(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void IssueShiSsGanjueStrike(ProgramSkillFrame f, int equipmentCardId, int targetSeat)
    {
        var r = f.SunShao!;
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).SingleOrDefault(card => card.Id == equipmentCardId) ?? throw new InvalidOperationException("GanJue lost its equipped strike material.");
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var damageBefore = _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == targetSeat);
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [], Array.AsReadOnly(new[] { ShiSsSource(f) }), effectiveSuit: equipment.Suit, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunShao = r with { Stage = ShiSsStage.GanjueIssued, EquipmentCardId = equipmentCardId, StrikeSuit = equipment.Suit, UseId = useId, DamageCountBefore = damageBefore } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash, Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>())) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, Array.AsReadOnly(new[] { targetSeat })));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targetSeat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsGanjueIssuedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, useId, action.ActionId, equipmentCardId, equipment.Suit, targetSeat));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(actor.Name)} 的“敢决”将装备区里的【{(equipment.DisplayName)}】当作无距离和次数限制的【杀】对 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 使用。", actor.Seat, targetSeat);
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void SettleShiSsGanjueStrike(ProgramSkillFrame f)
    {
        var r = f.SunShao!;
        var target = f.SelectedTargetSeats is [var seat] ? seat : -1;
        var damaged = _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == target) > r.DamageCountBefore;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsGanjueSettledEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, r.UseId, target, damaged));
        FinishShiSsOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal SkillProgramStepOutcome BeginShiSsZhuji(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SunShao is not null)
            throw new InvalidOperationException("Shi Sun Shao restarted a running operation.");
        if (f.SkillId != ShiSsZhuji || f.TriggerId != ShiSunShaoComposition.ZhujiTrigger || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { SunShao = new("zhuji", _coreActualTurnProgressTurnNumberAccess._turnNumber, ShiSsStage.ZhujiAsk) });
        PublishShiSs(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ApplyShiSsZhujiDiscard(ProgramSkillFrame f, int chosenCardId)
    {
        var r = f.SunShao!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var chosen = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).SingleOrDefault(card => card.Id == chosenCardId) ?? throw new InvalidOperationException("ZhuJi lost its chosen hand card.");
        var sameSuit = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(card => card.Suit == chosen.Suit).ToArray();
        foreach (var card in sameSuit)
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.DiscardPile, new CardMoveReason(ShiSunShaoComposition.ZhujiDiscardReason));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsZhujiDiscardedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, chosenCardId, chosen.Suit, sameSuit.Length));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 的“筑墼”弃置了 {(sameSuit.Length)} 张 {(global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime.DescribeSuit(chosen.Suit))} 手牌。", f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SunShao = r with { ChosenCardId = chosenCardId, StrikeSuit = chosen.Suit, DiscardedCount = sameSuit.Length } });
        ApplyShiSsZhujiEquip(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ApplyShiSsZhujiEquip(ProgramSkillFrame f)
    {
        var r = f.SunShao!;
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var candidates = _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Where(card => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && card.Suit == r.StrikeSuit).ToArray();
        if (candidates.Length > 0)
        {
            var card = candidates[_randomOperations._random.Next(candidates.Length)];
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat), new CardMoveReason(ShiSunShaoComposition.ZhujiGainReason));
            EquipShiSsZhujiGain(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), card.Id);
        }

        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        if (ShiSsZhujiBonusQualified(f))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SunShao = f.SunShao!with { Stage = ShiSsStage.ZhujiBonus } });
            ContinueShiSs(f.Id);
            return;
        }

        FinishShiSsOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void EquipShiSsZhujiGain(ProgramSkillFrame f, int cardId)
    {
        var r = f.SunShao!;
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        var slot = _coreFieldEquipmentOperations.FieldEquipmentSlot(card);
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        var equipped = _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Where(item => _coreFieldEquipmentOperations.FieldEquipmentSlot(item) == slot).ToArray();
        var replaced = equipped.Length >= owner.EquipmentSlotCapacity(slot) ? equipped[0] : null;
        if (replaced is not null)
            _corePhysicalMovementFacadesOperations.MoveCard(replaced, CardLocation.Equipment(f.OwnerSeat), CardLocation.DiscardPile, CardMoveReasons.EquipmentReplace);
        _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(cardId), CardLocation.Equipment(f.OwnerSeat), CardMoveReasons.EquipmentEnter);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, f.OwnerSeat, slot, cardId, card.Kind, replaced?.Id));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsZhujiEquippedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, cardId, replaced?.Id));
        _coreMatchLogOperations.AddLog("EquipmentChanged", $"{(owner.Name)} 的“筑墼”获得并使用了【{(card.DisplayName)}】。", f.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SunShao = r with { EquipmentCardId = cardId } });
    }

    internal bool ShiSsZhujiBonusQualified(ProgramSkillFrame f) => f.SunShao is { DiscardedCount: var discarded } && discarded > 0 && discarded >= _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Count;
    internal void ApplyShiSsZhujiBonus(ProgramSkillFrame f, string choice)
    {
        var r = f.SunShao!;
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        switch (choice)
        {
            case "draw":
                _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private, new CardMoveReason("skill-program.mobile:shi-zhuji.bonus-draw"));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsZhujiBonusEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, "draw", 2));
                break;
            case "recover":
            {
                var target = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                var recovered = target.IsAlive ? Math.Min(1, Math.Max(0, target.MaxHp - target.Hp)) : 0;
                if (recovered > 0)
                {
                    var recoveryId = _coreNativeRecoveryFramesOperations.BeginRecovery(active.Id, active.OwnerSeat, active.OwnerSeat, recovered);
                    target.Hp += recovered;
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(active.OwnerSeat, active.OwnerSeat, recovered, target.Hp));
                    _coreRuntimeOperations.PopResolutionFrame(recoveryId, ResolutionFrameKind.Recovery);
                }

                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsZhujiBonusEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, "recover", recovered));
                break;
            }

            case "armor":
            {
                var target = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
                var before = target.Armor;
                target.Armor = checked(before + 1);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ArmorGrantedEvent(active.Id, active.InstructionIndex - 1, global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(active), active.OwnerSeat, 1, before, before + 1));
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiSsZhujiBonusEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber, "armor", 1));
                _coreMatchLogOperations.AddLog("ArmorGranted", $"{(target.Name)} 获得 1 点护甲，现有 {(target.Armor)} 点。", f.OwnerSeat);
                break;
            }

            default:
                throw new InvalidOperationException("ZhuJi answered an unknown bonus branch.");
        }

        FinishShiSsOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal IReadOnlyList<PromptChoice> ShiSsChoices(ProgramSkillFrame f)
    {
        var r = f.SunShao!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        switch (r.Operation, r.Stage)
        {
            case ("ganjue", ShiSsStage.GanjueMaterial):
                return _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(card => new PromptChoice(new($"shi-sun-shao.{(f.Id)}.material.{(card.Id)}"), $"将装备区里的【{(CardCatalog.Get(card.Kind).DisplayName)}】（{(global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime.DescribeSuit(card.Suit))}）当作【杀】使用", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ShiSunShaoComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "material", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture), })).ToArray();
            case ("zhuji", ShiSsStage.ZhujiAsk):
                return[MakeShiSsChoice(f, "arm", "发动“筑墼”：弃置一种花色的所有手牌并获得同花色装备"), MakeShiSsChoice(f, "pass", "不发动“筑墼”"), ];
            case ("zhuji", ShiSsStage.ZhujiHand):
                return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Select(card => new PromptChoice(new($"shi-sun-shao.{(f.Id)}.hand.{(card.Id)}"), $"选择这张手牌，弃置所有{(global::CardGame.Core.EngineOwners.CharacterZhangRenMobileShiRuntime.DescribeSuit(card.Suit))}手牌（{(DescribeShiSsCard(card))}）", [card.Id], [], new Dictionary<string, string> { ["program-action"] = ShiSunShaoComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "hand", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture), })).ToArray();
            case ("zhuji", ShiSsStage.ZhujiBonus):
                return[MakeShiSsChoice(f, "draw", "摸两张牌"), MakeShiSsChoice(f, "recover", "回复1点体力"), MakeShiSsChoice(f, "armor", "获得1点护甲"), ];
            default:
                return[];
        }
    }

    internal PromptChoice MakeShiSsChoice(ProgramSkillFrame f, string branch, string label) => new(new($"shi-sun-shao.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiSunShaoComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal static string DescribeShiSsCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{(card.Suit)}{(card.Rank)}";
    internal void PublishShiSs(ProgramSkillFrame f)
    {
        var choices = ShiSsChoices(f);
        if (choices.Count == 0)
        {
            FinishShiSsOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.SunShao!.Operation == "ganjue" ? "敢决：选择装备区里当作【杀】的牌。" : "筑墼：请选择。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiSunShaoComposition.ProgramAction)]
    internal void AnswerShiSs(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SunShao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiSsChoices(f).Any(choice => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Sun Shao choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("ganjue", "material")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId) && f.SelectedTargetSeats is [var target]:
                IssueShiSsGanjueStrike(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), cardId, target);
                return;
            case ("zhuji", "arm"):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SunShao = r with { Stage = ShiSsStage.ZhujiHand } });
                ContinueShiSs(f.Id);
                return;
            case ("zhuji", "pass"):
                FinishShiSsOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("zhuji", "hand")when int.TryParse(selected.Parameters["card-id"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var handCardId):
                ApplyShiSsZhujiDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), handCardId);
                return;
            case ("zhuji", "draw") or ("zhuji", "recover") or ("zhuji", "armor"):
                ApplyShiSsZhujiBonus(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), selected.Parameters["branch"]);
                return;
            default:
                throw new InvalidOperationException("Shi Sun Shao answered an incompatible operation branch.");
        }
    }

    internal void ContinueShiSs(long id) => PublishShiSs(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    internal void FinishShiSsOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { SunShao = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiSs(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68400)]
    internal bool ResumeShiSs(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SunShao: { } r } f || f.Id != id)
            return false;
        if (r.Stage == ShiSsStage.GanjueIssued)
        {
            SettleShiSsGanjueStrike(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
            return true;
        }

        if (DrainShiSs(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
            return true;
        PublishShiSs(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68400)]
    internal bool ReturnShiSsMovement(ProgramSkillFrame f) => f.SunShao is not null && ResumeShiSs(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiSsGanjue)]
    internal PromptChoice ChooseShiSsGanjueAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiSsZhuji)]
    internal PromptChoice ChooseShiSsZhujiAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}
