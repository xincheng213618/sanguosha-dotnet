using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiSsGanjue = ShiSunShaoComposition.GanjueSkill, ShiSsZhuji = ShiSunShaoComposition.ZhujiSkill;

    private static CardConversionSource ShiSsSource(ProgramSkillFrame f) =>
        new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    [ProgramCapability]
    private sealed class ShiSunShaoCapability(GameEngine engine) : IShiSunShaoHost
    {
        public SkillProgramStepOutcome ResolveGanjue(ProgramSkillFrame frame) => engine.BeginShiSsGanjue(frame);
        public SkillProgramStepOutcome ResolveZhuji(ProgramSkillFrame frame) => engine.BeginShiSsZhuji(frame);
    }

    // ---- 敢决: once per play phase, an equipment card becomes a distance- and
    // count-unlimited normal Slash against the activation's chosen target.

    private SkillProgramStepOutcome BeginShiSsGanjue(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.SunShao is not null)
            throw new InvalidOperationException("Shi Sun Shao restarted a running operation.");
        if (f.SkillId != ShiSsGanjue || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] ||
            !_players[target].IsAlive || _winner != Winner.None || !_players[f.OwnerSeat].IsAlive ||
            GetEquipment(_players[f.OwnerSeat]).Count == 0)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { SunShao = new("ganjue", _turnNumber, ShiSsStage.GanjueMaterial) });
        PublishShiSs(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void IssueShiSsGanjueStrike(ProgramSkillFrame f, int equipmentCardId, int targetSeat)
    {
        var r = f.SunShao!;
        var equipment = GetEquipment(_players[f.OwnerSeat]).SingleOrDefault(card => card.Id == equipmentCardId) ??
            throw new InvalidOperationException("GanJue lost its equipped strike material.");
        var actor = _players[f.OwnerSeat];
        var damageBefore = ProgramEventHistory<DamageAppliedEvent>()
            .Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == targetSeat);
        var useId = ++_resolutionSequence;
        var action = CaptureFactionAction(new CardActionContext(++_cardActionSequence,
            _resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use,
            actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [],
            Array.AsReadOnly(new[] { ShiSsSource(f) }),
            effectiveSuit: equipment.Suit, effectiveRank: 0));
        ReplaceRuntimeTop(f = f with { SunShao = r with
        { Stage = ShiSsStage.GanjueIssued, EquipmentCardId = equipmentCardId, StrikeSuit = equipment.Suit,
            UseId = useId, DamageCountBefore = damageBefore } });
        PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash,
            Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>()))
        { Action = action });
        if (TracksPlayCardHistory) AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, Array.AsReadOnly(new[] { targetSeat })));
        RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        RecordActualPlayPhaseUse(action);
        var attack = new CardAttackHandle(this, useId, actor.Seat, targetSeat, card: null,
            damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash,
            ignoresArmor: HasCardArmorBypass(actor, _players[targetSeat], CardKind.Slash),
            programSkillCardUseFrameId: f.Id);
        CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        ActiveCardAttack = attack;
        AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        AdvanceEventRulesAndQueueFact(new ShiSsGanjueIssuedEvent(f.Id, f.OwnerSeat, _turnNumber, useId,
            action.ActionId, equipmentCardId, equipment.Suit, targetSeat));
        AddLog("CardUsed", $"{actor.Name} 的“敢决”将装备区里的【{equipment.DisplayName}】当作无距离和次数限制的【杀】对 {_players[targetSeat].Name} 使用。",
            actor.Seat, targetSeat);
        TryMarkProgramUseCommitted(useId);
        if (!TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats,
                ProgramCardContinuation.CommittedSlash))
            BeginSlashTargetResolution(attack);
    }

    private void SettleShiSsGanjueStrike(ProgramSkillFrame f)
    {
        var r = f.SunShao!;
        var target = f.SelectedTargetSeats is [var seat] ? seat : -1;
        var damaged = ProgramEventHistory<DamageAppliedEvent>()
            .Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == target) > r.DamageCountBefore;
        AdvanceEventRulesAndQueueFact(new ShiSsGanjueSettledEvent(f.Id, f.OwnerSeat, _turnNumber, r.UseId,
            target, damaged));
        FinishShiSsOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- 筑墼: own play ending, discard one suit from the hand, gain and use
    // a same-suit equipment, and score a bonus when the batch met the zone.

    private SkillProgramStepOutcome BeginShiSsZhuji(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.SunShao is not null)
            throw new InvalidOperationException("Shi Sun Shao restarted a running operation.");
        if (f.SkillId != ShiSsZhuji || f.TriggerId != ShiSunShaoComposition.ZhujiTrigger ||
            _winner != Winner.None || !_players[f.OwnerSeat].IsAlive || GetHand(_players[f.OwnerSeat]).Count == 0)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { SunShao = new("zhuji", _turnNumber, ShiSsStage.ZhujiAsk) });
        PublishShiSs(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ApplyShiSsZhujiDiscard(ProgramSkillFrame f, int chosenCardId)
    {
        var r = f.SunShao!;
        var owner = _players[f.OwnerSeat];
        var chosen = GetHand(owner).SingleOrDefault(card => card.Id == chosenCardId) ??
            throw new InvalidOperationException("ZhuJi lost its chosen hand card.");
        var sameSuit = GetHand(owner).Where(card => card.Suit == chosen.Suit).ToArray();
        foreach (var card in sameSuit)
            MoveCard(card, _cardZones.GetLocation(card.Id), CardLocation.DiscardPile,
                new CardMoveReason(ShiSunShaoComposition.ZhujiDiscardReason));
        AdvanceEventRulesAndQueueFact(new ShiSsZhujiDiscardedEvent(f.Id, f.OwnerSeat, _turnNumber,
            chosenCardId, chosen.Suit, sameSuit.Length));
        AddLog("SkillTriggered", $"{owner.Name} 的“筑墼”弃置了 {sameSuit.Length} 张 {DescribeSuit(chosen.Suit)} 手牌。",
            f.OwnerSeat);
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
        { SunShao = r with { ChosenCardId = chosenCardId, StrikeSuit = chosen.Suit, DiscardedCount = sameSuit.Length } });
        ApplyShiSsZhujiEquip(GetActiveProgramFrame(f.Id));
    }

    private void ApplyShiSsZhujiEquip(ProgramSkillFrame f)
    {
        var r = f.SunShao!;
        var owner = _players[f.OwnerSeat];
        var candidates = _cardZones.CardsAt(CardLocation.DrawPile)
            .Where(card => EquipmentRules.IsEquipment(card.Kind) && card.Suit == r.StrikeSuit).ToArray();
        if (candidates.Length > 0)
        {
            var card = candidates[_random.Next(candidates.Length)];
            MoveCard(card, CardLocation.DrawPile, CardLocation.Hand(f.OwnerSeat),
                new CardMoveReason(ShiSunShaoComposition.ZhujiGainReason));
            EquipShiSsZhujiGain(GetActiveProgramFrame(f.Id), card.Id);
        }
        f = GetActiveProgramFrame(f.Id);
        if (ShiSsZhujiBonusQualified(f))
        {
            ReplaceRuntimeTop(f with { SunShao = f.SunShao! with { Stage = ShiSsStage.ZhujiBonus } });
            ContinueShiSs(f.Id);
            return;
        }
        FinishShiSsOperation(GetActiveProgramFrame(f.Id));
    }

    private void EquipShiSsZhujiGain(ProgramSkillFrame f, int cardId)
    {
        var r = f.SunShao!;
        var card = GetAdvancedCard(cardId);
        var slot = FieldEquipmentSlot(card);
        var owner = _players[f.OwnerSeat];
        var equipped = GetEquipment(owner).Where(item => FieldEquipmentSlot(item) == slot).ToArray();
        var replaced = equipped.Length >= owner.EquipmentSlotCapacity(slot) ? equipped[0] : null;
        if (replaced is not null)
            MoveCard(replaced, CardLocation.Equipment(f.OwnerSeat), CardLocation.DiscardPile,
                CardMoveReasons.EquipmentReplace);
        MoveCard(card, _cardZones.GetLocation(cardId), CardLocation.Equipment(f.OwnerSeat),
            CardMoveReasons.EquipmentEnter);
        AdvanceEventRulesAndQueueFact(new EquipmentChangedEvent(f.Id, f.OwnerSeat, slot, cardId, card.Kind,
            replaced?.Id));
        AdvanceEventRulesAndQueueFact(new ShiSsZhujiEquippedEvent(f.Id, f.OwnerSeat, _turnNumber, cardId,
            replaced?.Id));
        AddLog("EquipmentChanged",
            $"{owner.Name} 的“筑墼”获得并使用了【{card.DisplayName}】。", f.OwnerSeat);
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { SunShao = r with { EquipmentCardId = cardId } });
    }

    private bool ShiSsZhujiBonusQualified(ProgramSkillFrame f) =>
        f.SunShao is { DiscardedCount: var discarded } && discarded > 0 &&
        discarded >= GetEquipment(_players[f.OwnerSeat]).Count;

    private void ApplyShiSsZhujiBonus(ProgramSkillFrame f, string choice)
    {
        var r = f.SunShao!;
        var active = GetActiveProgramFrame(f.Id);
        switch (choice)
        {
            case "draw":
                DrawProgramCards(f.Id, f.OwnerSeat, 2, null, null, SkillProgramCardSetVisibility.Private,
                    new CardMoveReason("skill-program.mobile:shi-zhuji.bonus-draw"));
                AdvanceEventRulesAndQueueFact(new ShiSsZhujiBonusEvent(f.Id, f.OwnerSeat, _turnNumber, "draw", 2));
                break;
            case "recover":
            {
                var target = _players[f.OwnerSeat];
                var recovered = target.IsAlive ? Math.Min(1, Math.Max(0, target.MaxHp - target.Hp)) : 0;
                if (recovered > 0)
                {
                    var recoveryId = BeginRecovery(active.Id, active.OwnerSeat, active.OwnerSeat, recovered);
                    target.Hp += recovered;
                    AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(active.OwnerSeat, active.OwnerSeat,
                        recovered, target.Hp));
                    PopResolutionFrame(recoveryId, ResolutionFrameKind.Recovery);
                }
                AdvanceEventRulesAndQueueFact(new ShiSsZhujiBonusEvent(f.Id, f.OwnerSeat, _turnNumber, "recover", recovered));
                break;
            }
            case "armor":
            {
                var target = _players[f.OwnerSeat];
                var before = target.Armor;
                target.Armor = checked(before + 1);
                AdvanceEventRulesAndQueueFact(new ArmorGrantedEvent(active.Id, active.InstructionIndex - 1,
                    CreateProgramTurnEffectSource(active), active.OwnerSeat, 1, before, before + 1));
                AdvanceEventRulesAndQueueFact(new ShiSsZhujiBonusEvent(f.Id, f.OwnerSeat, _turnNumber, "armor", 1));
                AddLog("ArmorGranted", $"{target.Name} 获得 1 点护甲，现有 {target.Armor} 点。", f.OwnerSeat);
                break;
            }
            default:
                throw new InvalidOperationException("ZhuJi answered an unknown bonus branch.");
        }
        FinishShiSsOperation(GetActiveProgramFrame(f.Id));
    }

    // ---- prompts --------------------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiSsChoices(ProgramSkillFrame f)
    {
        var r = f.SunShao!;
        if (_winner != Winner.None || !_players[f.OwnerSeat].IsAlive) return [];
        switch (r.Operation, r.Stage)
        {
            case ("ganjue", ShiSsStage.GanjueMaterial):
                return GetEquipment(_players[f.OwnerSeat]).Select(card => new PromptChoice(
                    new($"shi-sun-shao.{f.Id}.material.{card.Id}"),
                    $"将装备区里的【{CardCatalog.Get(card.Kind).DisplayName}】（{DescribeSuit(card.Suit)}）当作【杀】使用",
                    [card.Id], [],
                    new Dictionary<string, string>
                    {
                        ["program-action"] = ShiSunShaoComposition.ProgramAction,
                        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                        ["branch"] = "material", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture),
                    })).ToArray();
            case ("zhuji", ShiSsStage.ZhujiAsk):
                return
                [
                    MakeShiSsChoice(f, "arm", "发动“筑墼”：弃置一种花色的所有手牌并获得同花色装备"),
                    MakeShiSsChoice(f, "pass", "不发动“筑墼”"),
                ];
            case ("zhuji", ShiSsStage.ZhujiHand):
                return GetHand(_players[f.OwnerSeat]).Select(card => new PromptChoice(
                    new($"shi-sun-shao.{f.Id}.hand.{card.Id}"),
                    $"选择这张手牌，弃置所有{DescribeSuit(card.Suit)}手牌（{DescribeShiSsCard(card)}）", [card.Id], [],
                    new Dictionary<string, string>
                    {
                        ["program-action"] = ShiSunShaoComposition.ProgramAction,
                        ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                        ["branch"] = "hand", ["card-id"] = card.Id.ToString(CultureInfo.InvariantCulture),
                    })).ToArray();
            case ("zhuji", ShiSsStage.ZhujiBonus):
                return
                [
                    MakeShiSsChoice(f, "draw", "摸两张牌"),
                    MakeShiSsChoice(f, "recover", "回复1点体力"),
                    MakeShiSsChoice(f, "armor", "获得1点护甲"),
                ];
            default:
                return [];
        }
    }

    private PromptChoice MakeShiSsChoice(ProgramSkillFrame f, string branch, string label) =>
        new(new($"shi-sun-shao.{f.Id}.{branch}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiSunShaoComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch,
            });

    private static string DescribeShiSsCard(Card card) => CardCatalog.Get(card.Kind).DisplayName + $"{card.Suit}{card.Rank}";

    private void PublishShiSs(ProgramSkillFrame f)
    {
        var choices = ShiSsChoices(f);
        if (choices.Count == 0)
        { FinishShiSsOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            f.SunShao!.Operation == "ganjue" ? "敢决：选择装备区里当作【杀】的牌。" : "筑墼：请选择。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiSunShaoComposition.ProgramAction)]
    private void AnswerShiSs(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SunShao: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiSsChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Sun Shao choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("ganjue", "material") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var cardId) &&
                f.SelectedTargetSeats is [var target]:
                IssueShiSsGanjueStrike(GetActiveProgramFrame(f.Id), cardId, target);
                return;
            case ("zhuji", "arm"):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
                { SunShao = r with { Stage = ShiSsStage.ZhujiHand } });
                ContinueShiSs(f.Id);
                return;
            case ("zhuji", "pass"):
                FinishShiSsOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("zhuji", "hand") when int.TryParse(selected.Parameters["card-id"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var handCardId):
                ApplyShiSsZhujiDiscard(GetActiveProgramFrame(f.Id), handCardId);
                return;
            case ("zhuji", "draw") or ("zhuji", "recover") or ("zhuji", "armor"):
                ApplyShiSsZhujiBonus(GetActiveProgramFrame(f.Id), selected.Parameters["branch"]);
                return;
            default:
                throw new InvalidOperationException("Shi Sun Shao answered an incompatible operation branch.");
        }
    }

    private void ContinueShiSs(long id) => PublishShiSs(GetActiveProgramFrame(id));

    private void FinishShiSsOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { SunShao = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiSs(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68400)]
    private bool ResumeShiSs(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { SunShao: { } r } f || f.Id != id) return false;
        if (r.Stage == ShiSsStage.GanjueIssued)
        { SettleShiSsGanjueStrike(GetActiveProgramFrame(id)); return true; }
        if (DrainShiSs(GetActiveProgramFrame(id))) return true;
        PublishShiSs(GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68400)]
    private bool ReturnShiSsMovement(ProgramSkillFrame f) => f.SunShao is not null && ResumeShiSs(f.Id);

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiSsGanjue)]
    private PromptChoice ChooseShiSsGanjueAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiSsZhuji)]
    private PromptChoice ChooseShiSsZhujiAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
}
