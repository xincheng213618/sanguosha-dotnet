using System.Globalization;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ShiZrFuan = ShiZhangRenComposition.FuanSkill, ShiZrYinXian = ShiZhangRenComposition.YinXianSkill;

    private static CardConversionSource ShiZrSource(ProgramSkillFrame f) =>
        new(f.SkillId, GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);

    [ProgramCapability]
    private sealed class ShiZhangRenCapability(GameEngine engine) : IShiZhangRenHost
    {
        public SkillProgramStepOutcome ResolveFuan(ProgramSkillFrame frame) => engine.BeginShiZrFuan(frame);
        public SkillProgramStepOutcome ResolveFuanStrike(ProgramSkillFrame frame) => engine.BeginShiZrFuanStrike(frame);
        public SkillProgramStepOutcome ResolveYinXian(ProgramSkillFrame frame) => engine.ResolveShiZrYinXian(frame);
    }

    // ---- 伏暗① own turn ending: zero the range, draw X and record a hand suit.

    private SkillProgramStepOutcome BeginShiZrFuan(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ShiZhangRen is not null)
            throw new InvalidOperationException("Shi Zhang Ren restarted a running operation.");
        if (f.SkillId != ShiZrFuan || f.TriggerId != ShiZhangRenComposition.FuanArmTrigger ||
            _winner != Winner.None || !_players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { ShiZhangRen = new("fuan", _turnNumber, ShiZrStage.FuanAsk) { Round = _roundNumber } });
        PublishShiZr(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private void ArmShiZrFuan(ProgramSkillFrame f, string drawReason)
    {
        var r = f.ShiZhangRen!;
        var draw = Math.Clamp(GetAttackRange(f.OwnerSeat), 0, ShiZhangRenComposition.MaximumDraw);
        AdvanceEventRulesAndQueueFact(new ShiZrFuanArmedEvent(f.Id, f.OwnerSeat, _turnNumber, _roundNumber, draw));
        if (draw > 0)
            DrawProgramCards(f.Id, f.OwnerSeat, draw, null, null, SkillProgramCardSetVisibility.Private, new(drawReason));
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
        { ShiZhangRen = r with { Stage = ShiZrStage.FuanRecord, DrawCount = draw } });
        ContinueShiZr(f.Id);
    }

    private IReadOnlyList<Suit> ShiZrFuanRecordableSuits(int seat)
    {
        var used = ProgramEventHistory<ShiZrFuanRecordedEvent>()
            .Where(entry => entry.OwnerSeat == seat && entry.Round == _roundNumber)
            .Select(entry => entry.Suit).ToHashSet();
        return GetHand(_players[seat]).Select(card => card.Suit).Distinct()
            .Where(suit => !used.Contains(suit)).Order().ToArray();
    }

    private void RecordShiZrFuanSuit(ProgramSkillFrame f, Suit suit)
    {
        var r = f.ShiZhangRen!;
        if (!ShiZrFuanRecordableSuits(f.OwnerSeat).Contains(suit))
            throw new InvalidOperationException("FuAn recorded a suit outside the owner's current hand quota.");
        var viaRelaunch = r.Stage == ShiZrStage.RelaunchRecord;
        AdvanceEventRulesAndQueueFact(new ShiZrFuanRecordedEvent(f.Id, f.OwnerSeat, _turnNumber, _roundNumber, suit, viaRelaunch));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 记录了花色 {DescribeSuit(suit)}（伏暗）。", f.OwnerSeat);
        FinishShiZrOperation(GetActiveProgramFrame(f.Id));
    }

    private static string DescribeSuit(Suit suit) => suit switch
    {
        Suit.Spade => "黑桃", Suit.Heart => "红桃", Suit.Club => "梅花", Suit.Diamond => "方块", _ => suit.ToString(),
    };

    // ---- 伏暗② other turns' ending: recorded suit hit the discard pile.

    private bool ShiZrFuanStrikeQualified(int seat)
    {
        if (_winner != Winner.None || !_players[seat].IsAlive || _currentSeat == seat ||
            ShiZrFuanRecord(seat) is not { } record)
            return false;
        return ProgramEventHistory<ShiZrFuanSuitDiscardedEvent>()
            .Any(entry => entry.ActualTurn == _turnNumber && entry.OwnerSeat == seat && entry.Suit == record.Suit);
    }

    private SkillProgramStepOutcome BeginShiZrFuanStrike(ProgramSkillFrame input)
    {
        var f = GetActiveProgramFrame(input.Id);
        if (f.ShiZhangRen is not null)
            throw new InvalidOperationException("Shi Zhang Ren restarted a running operation.");
        if (f.SkillId != ShiZrFuan || f.TriggerId != ShiZhangRenComposition.FuanStrikeTrigger ||
            !ShiZrFuanStrikeQualified(f.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(f = f with { ShiZhangRen = new("strike", _turnNumber, ShiZrStage.StrikeAsk) { Round = _roundNumber } });
        PublishShiZr(GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    private bool CanIssueShiZrFuanStrike(int actor, int target) =>
        actor != target && _players[actor].IsAlive && _players[target].IsAlive &&
        !HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) &&
        !IsCardUseForbidden(actor, CardKind.Slash, CardActionType.Use) &&
        !IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) &&
        !IsCardTargetProhibited(_players[target], CardKind.Slash, Suit.None, null) &&
        !IsSlashProhibited(_players[target]) &&
        CanSpendSlashUse(_players[actor], _players[target], ignoresCount: true);

    private void IssueShiZrFuanStrike(ProgramSkillFrame f, int targetSeat)
    {
        var r = f.ShiZhangRen!;
        if (r.Stage != ShiZrStage.StrikeTarget || !CanIssueShiZrFuanStrike(f.OwnerSeat, targetSeat))
            throw new InvalidOperationException("FuAn lost its legal strike target.");
        var record = ShiZrFuanRecord(f.OwnerSeat) ??
            throw new InvalidOperationException("FuAn lost its recorded suit before the strike.");
        var actor = _players[f.OwnerSeat];
        AdvanceEventRulesAndQueueFact(new ShiZrFuanClearedEvent(f.Id, f.OwnerSeat, _turnNumber, _roundNumber, targetSeat));
        var damageBefore = ProgramEventHistory<DamageAppliedEvent>()
            .Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == targetSeat);
        var useId = ++_resolutionSequence;
        var action = CaptureFactionAction(new CardActionContext(++_cardActionSequence,
            _resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use,
            actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [],
            Array.AsReadOnly(new[] { ShiZrSource(f) }),
            effectiveSuit: Suit.None, effectiveRank: 0));
        ReplaceRuntimeTop(f = f with { ShiZhangRen = r with
        { Stage = ShiZrStage.StrikeIssued, TargetSeat = targetSeat, UseId = useId, DamageCountBefore = damageBefore } });
        PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash,
            Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>()))
        { Action = action });
        if (TracksPlayCardHistory) AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, Array.AsReadOnly(new[] { targetSeat })));
        RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        RecordActualPlayPhaseUse(action);
        var attack = new CardAttackHandle(this, useId, actor.Seat, targetSeat, card: null,
            damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash,
            ignoresArmor: HasCardArmorBypass(actor, _players[targetSeat], CardKind.Slash),
            programSkillCardUseFrameId: f.Id);
        CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        ActiveCardAttack = attack;
        AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeIssuedEvent(f.Id, f.OwnerSeat, _turnNumber, useId,
            action.ActionId, targetSeat, damageBefore));
        AddLog("CardUsed", $"{actor.Name} 的“伏暗”视为对 {_players[targetSeat].Name} 使用了一张无距离限制的【杀】。", actor.Seat, targetSeat);
        TryMarkProgramUseCommitted(useId);
        if (!TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats,
                ProgramCardContinuation.CommittedSlash))
            BeginSlashTargetResolution(attack);
    }

    private void SettleShiZrFuanStrike(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        var damaged = r.UseId != 0 && _players[f.OwnerSeat].IsAlive && _winner == Winner.None &&
            r.TargetSeat >= 0 && _players[r.TargetSeat].IsAlive &&
            ProgramEventHistory<DamageAppliedEvent>()
                .Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == r.TargetSeat) > r.DamageCountBefore;
        if (!damaged)
        {
            AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, false, "none"));
            FinishShiZrOperation(GetActiveProgramFrame(f.Id));
            return;
        }
        ReplaceRuntimeTop(f = f with { ShiZhangRen = r with { Stage = ShiZrStage.StrikeChoose } });
        ContinueShiZr(GetActiveProgramFrame(f.Id).Id);
    }

    private void SealShiZrFuanTarget(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        if (r.TargetSeat < 0 || !_players[r.TargetSeat].IsAlive)
            throw new InvalidOperationException("FuAn sealed a lost strike target.");
        AdvanceEventRulesAndQueueFact(new ShiZrFuanSealedEvent(f.Id, f.OwnerSeat, r.TargetSeat, _turnNumber));
        _currentTurnSkillSuppressionRevision++;
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 令 {_players[r.TargetSeat].Name} 的技能失效直至其下个回合结束前（伏暗）。", f.OwnerSeat, r.TargetSeat);
    }

    // ---- 引弦② out-of-turn uses widen the range.

    private SkillProgramStepOutcome ResolveShiZrYinXian(ProgramSkillFrame supplied)
    {
        var f = GetActiveProgramFrame(supplied.Id);
        var use = f.WindowContext?.CardUse ??
            throw new InvalidOperationException("YinXian lost its completed-use window.");
        if (f.SkillId != ShiZrYinXian || f.TriggerId != ShiZhangRenComposition.YinXianUseTrigger ||
            _winner != Winner.None || !_players[f.OwnerSeat].IsAlive ||
            _turnProgression.OwnerSeat != _currentSeat || _currentSeat == f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        var bonus = ShiZrYinXianBonus(f.OwnerSeat) + 1;
        AdvanceEventRulesAndQueueFact(new ShiZrYinXianBonusGainedEvent(f.OwnerSeat, _turnNumber, use.CardActionId, bonus));
        AddLog("SkillTriggered", $"{_players[f.OwnerSeat].Name} 于回合外使用牌，攻击范围+1（引弦 {bonus}）。", f.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }

    // ---- choice publication --------------------------------------------------

    private IReadOnlyList<PromptChoice> ShiZrChoices(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        if (_winner != Winner.None || !_players[f.OwnerSeat].IsAlive) return [];
        switch (r.Operation, r.Stage)
        {
            case ("fuan", ShiZrStage.FuanAsk):
                return
                [
                    MakeShiZrChoice(f, "arm", null, "发动“伏暗”：攻击范围调整至0，摸X张牌并记录一个手牌花色"),
                    MakeShiZrChoice(f, "pass", null, "不发动“伏暗”"),
                ];
            case ("fuan", ShiZrStage.FuanRecord) when r.DrawCount >= 0:
                return ShiZrFuanRecordableSuits(f.OwnerSeat)
                    .Select(suit => MakeShiZrChoice(f, "suit", ((int)suit).ToString(CultureInfo.InvariantCulture),
                        $"记录花色{DescribeSuit(suit)}")).ToArray();
            case ("strike", ShiZrStage.StrikeAsk) when ShiZrFuanStrikeQualified(f.OwnerSeat):
                return
                [
                    MakeShiZrChoice(f, "arm", null, "发动“伏暗”：清除记录并对一名角色视为使用无距离限制的【杀】"),
                    MakeShiZrChoice(f, "pass", null, "不发动“伏暗”"),
                ];
            case ("strike", ShiZrStage.StrikeTarget):
                return Enumerable.Range(0, _players.Count).Where(seat => CanIssueShiZrFuanStrike(f.OwnerSeat, seat))
                    .Select(seat => new PromptChoice(
                        new($"shi-zhang-ren.{f.Id}.target.{seat}"),
                        $"对 {_players[seat].Name} 视为使用无距离限制的【杀】", [], [seat],
                        new Dictionary<string, string>
                        {
                            ["program-action"] = ShiZhangRenComposition.ProgramAction,
                            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                            ["branch"] = "target", ["seat"] = seat.ToString(CultureInfo.InvariantCulture),
                        })).ToArray();
            case ("strike", ShiZrStage.StrikeChoose):
                return ShiZrStrikeFollowUps(f);
            default:
                return [];
        }
    }

    private IReadOnlyList<PromptChoice> ShiZrStrikeFollowUps(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        var choices = new List<PromptChoice>();
        if (ShiZrFuanRecord(f.OwnerSeat) is { ViaRelaunch: false })
            choices.Add(MakeShiZrChoice(f, "relaunch", null, "再次发动“伏暗”"));
        if (r.TargetSeat >= 0 && _players[r.TargetSeat].IsAlive)
            choices.Add(MakeShiZrChoice(f, "seal", null, $"令 {_players[r.TargetSeat].Name} 的技能失效直至其下个回合结束前"));
        choices.Add(MakeShiZrChoice(f, "decline", null, "放弃"));
        return choices;
    }

    private PromptChoice MakeShiZrChoice(ProgramSkillFrame f, string branch, string? value, string label) =>
        new(new($"shi-zhang-ren.{f.Id}.{branch}.{value ?? "-"}"), label, [], [],
            new Dictionary<string, string>
            {
                ["program-action"] = ShiZhangRenComposition.ProgramAction,
                ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
                ["branch"] = branch, ["value"] = value ?? "",
            });

    private void PublishShiZr(ProgramSkillFrame f)
    {
        var choices = ShiZrChoices(f);
        if (choices.Count == 0)
        { FinishShiZrOperation(GetActiveProgramFrame(f.Id)); return; }
        var skill = _contentRegistry.GetSkill(f.SkillId);
        ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat,
            f.ShiZhangRen!.Operation == "strike" ? "伏暗：选择是否发动或指定目标。" : "伏暗：选择是否发动。",
            choices.SelectMany(choice => choice.Cards).Distinct().ToArray(),
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = f.OwnerSeat, Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _status = _players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiZhangRenComposition.ProgramAction)]
    private void AnswerShiZr(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiZhangRen: { } r } f ||
            _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending ||
            !ShiZrChoices(f).Any(choice => AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Zhang Ren choice lost its current frame or legal branch.");
        ClearPendingDecision();
        ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("fuan", "arm"):
                ArmShiZrFuan(GetActiveProgramFrame(f.Id), ShiZhangRenComposition.FuanDrawReason);
                return;
            case ("fuan", "pass"):
                FinishShiZrOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("fuan", "suit") when Enum.TryParse(selected.Parameters["value"], out Suit suit):
                RecordShiZrFuanSuit(GetActiveProgramFrame(f.Id), suit);
                return;
            case ("strike", "arm"):
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
                { ShiZhangRen = r with { Stage = ShiZrStage.StrikeTarget } });
                ContinueShiZr(f.Id);
                return;
            case ("strike", "pass"):
                FinishShiZrOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("strike", "target") when int.TryParse(selected.Parameters["seat"],
                NumberStyles.Integer, CultureInfo.InvariantCulture, out var seat):
                IssueShiZrFuanStrike(GetActiveProgramFrame(f.Id), seat);
                return;
            case ("strike", "relaunch"):
                AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, true, "relaunch"));
                ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with
                { ShiZhangRen = r with { Stage = ShiZrStage.RelaunchRecord, UseId = 0, TargetSeat = -1 } });
                ArmShiZrFuan(GetActiveProgramFrame(f.Id), ShiZhangRenComposition.FuanRelaunchDrawReason);
                return;
            case ("strike", "seal"):
                AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, true, "seal"));
                SealShiZrFuanTarget(GetActiveProgramFrame(f.Id));
                FinishShiZrOperation(GetActiveProgramFrame(f.Id));
                return;
            case ("strike", "decline"):
                AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, true, "none"));
                FinishShiZrOperation(GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Zhang Ren answered an incompatible operation branch.");
        }
    }

    // ---- continuations --------------------------------------------------------

    private void ContinueShiZr(long id)
    {
        if (_pendingDecision is not null ||
            _resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiZhangRen: { } r } f || f.Id != id) return;
        if (r.Stage is ShiZrStage.StrikeIssued) return;
        PublishShiZr(GetActiveProgramFrame(id));
    }

    private void FinishShiZrOperation(ProgramSkillFrame f)
    {
        ReplaceRuntimeTop(GetActiveProgramFrame(f.Id) with { ShiZhangRen = null });
        FinishProgramSkill(GetActiveProgramFrame(f.Id), true);
    }

    private bool DrainShiZr(ProgramSkillFrame f) => TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);

    [ProgramContinuation(ProgramContinuationKind.Resume, 68200)]
    private bool ResumeShiZr(long id)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShiZhangRen: { } r } f || f.Id != id) return false;
        switch (r.Stage)
        {
            case ShiZrStage.StrikeIssued:
                SettleShiZrFuanStrike(GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiZr(GetActiveProgramFrame(id))) return true;
                ContinueShiZr(id);
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68200)]
    private bool ReturnShiZrMovement(ProgramSkillFrame f) => f.ShiZhangRen is not null && ResumeShiZr(f.Id);

    // ---- AI -------------------------------------------------------------------

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZrFuan)]
    private PromptChoice ChooseShiZrFuanAi(PendingDecision decision, ProgramSkillFrame f) =>
        decision.Choices[0];

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZrFuanStrike)]
    private PromptChoice ChooseShiZrStrikeAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        return r.Stage switch
        {
            ShiZrStage.StrikeTarget => decision.Choices
                .OrderBy(choice => GetHand(_players[choice.Targets.Single()]).Count).First(),
            ShiZrStage.StrikeChoose => decision.Choices.FirstOrDefault(
                choice => choice.Parameters.GetValueOrDefault("branch") == "seal") ?? decision.Choices[^1],
            _ => decision.Choices[0],
        };
    }
}
