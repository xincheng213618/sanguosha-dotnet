#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangRenMobileShiRuntime
{
    internal const string ShiZrFuan = ShiZhangRenComposition.FuanSkill, ShiZrYinXian = ShiZhangRenComposition.YinXianSkill;
    internal static CardConversionSource ShiZrSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    SkillProgramStepOutcome IShiZhangRenHost.ResolveFuan(ProgramSkillFrame frame) => BeginShiZrFuan(frame);
    SkillProgramStepOutcome IShiZhangRenHost.ResolveFuanStrike(ProgramSkillFrame frame) => BeginShiZrFuanStrike(frame);
    SkillProgramStepOutcome IShiZhangRenHost.ResolveYinXian(ProgramSkillFrame frame) => ResolveShiZrYinXian(frame);
    internal SkillProgramStepOutcome BeginShiZrFuan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiZhangRen is not null)
            throw new InvalidOperationException("Shi Zhang Ren restarted a running operation.");
        if (f.SkillId != ShiZrFuan || f.TriggerId != ShiZhangRenComposition.FuanArmTrigger || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhangRen = new("fuan", _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, ShiZrStage.FuanAsk) { Round = _coreActualRoundProgressRoundNumberAccess._roundNumber } });
        PublishShiZr(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ArmShiZrFuan(ProgramSkillFrame f, string drawReason)
    {
        var r = f.ShiZhangRen!;
        var draw = Math.Clamp(_coreNativeRuleQueriesOperations.GetAttackRange(f.OwnerSeat), 0, ShiZhangRenComposition.MaximumDraw);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanArmedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, draw));
        if (draw > 0)
            _coreProgramLifecycleOperations.DrawProgramCards(f.Id, f.OwnerSeat, draw, null, null, SkillProgramCardSetVisibility.Private, new(drawReason));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiZhangRen = r with { Stage = ShiZrStage.FuanRecord, DrawCount = draw } });
        ContinueShiZr(f.Id);
    }

    internal IReadOnlyList<Suit> ShiZrFuanRecordableSuits(int seat)
    {
        var used = _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanRecordedEvent>().Where(entry => entry.OwnerSeat == seat && entry.Round == _coreActualRoundProgressRoundNumberAccess._roundNumber).Select(entry => entry.Suit).ToHashSet();
        return _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(card => card.Suit).Distinct().Where(suit => !used.Contains(suit)).Order().ToArray();
    }

    internal void RecordShiZrFuanSuit(ProgramSkillFrame f, Suit suit)
    {
        var r = f.ShiZhangRen!;
        if (!ShiZrFuanRecordableSuits(f.OwnerSeat).Contains(suit))
            throw new InvalidOperationException("FuAn recorded a suit outside the owner's current hand quota.");
        var viaRelaunch = r.Stage == ShiZrStage.RelaunchRecord;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanRecordedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, suit, viaRelaunch));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 记录了花色 {(DescribeSuit(suit))}（伏暗）。", f.OwnerSeat);
        FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal static string DescribeSuit(Suit suit) => suit switch
    {
        Suit.Spade => "黑桃",
        Suit.Heart => "红桃",
        Suit.Club => "梅花",
        Suit.Diamond => "方块",
        _ => suit.ToString(),
    };
    internal bool ShiZrFuanStrikeQualified(int seat)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat == seat || ShiZrFuanRecord(seat)is not { } record)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<ShiZrFuanSuitDiscardedEvent>().Any(entry => entry.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber && entry.OwnerSeat == seat && entry.Suit == record.Suit);
    }

    internal SkillProgramStepOutcome BeginShiZrFuanStrike(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.ShiZhangRen is not null)
            throw new InvalidOperationException("Shi Zhang Ren restarted a running operation.");
        if (f.SkillId != ShiZrFuan || f.TriggerId != ShiZhangRenComposition.FuanStrikeTrigger || !ShiZrFuanStrikeQualified(f.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhangRen = new("strike", _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, ShiZrStage.StrikeAsk) { Round = _coreActualRoundProgressRoundNumberAccess._roundNumber } });
        PublishShiZr(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool CanIssueShiZrFuanStrike(int actor, int target) => actor != target && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && _coreMatchParticipantsPlayersAccess._players[target].IsAlive && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actor, SkillProgramCardTargetRestriction.SelfOnly) && !_coreCardUseModulesOperations.IsCardUseForbidden(actor, CardKind.Slash, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor, target, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[target], CardKind.Slash, Suit.None, null) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[target]) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[actor], _coreMatchParticipantsPlayersAccess._players[target], ignoresCount: true);
    internal void IssueShiZrFuanStrike(ProgramSkillFrame f, int targetSeat)
    {
        var r = f.ShiZhangRen!;
        if (r.Stage != ShiZrStage.StrikeTarget || !CanIssueShiZrFuanStrike(f.OwnerSeat, targetSeat))
            throw new InvalidOperationException("FuAn lost its legal strike target.");
        var record = ShiZrFuanRecord(f.OwnerSeat) ?? throw new InvalidOperationException("FuAn lost its recorded suit before the strike.");
        var actor = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanClearedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, _coreActualRoundProgressRoundNumberAccess._roundNumber, targetSeat));
        var damageBefore = _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == targetSeat);
        var useId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, actor.Seat, actor.Seat, null, null, null, CardKind.Slash, [targetSeat], [], Array.AsReadOnly(new[] { ShiZrSource(f) }), effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhangRen = r with { Stage = ShiZrStage.StrikeIssued, TargetSeat = targetSeat, UseId = useId, DamageCountBefore = damageBefore } });
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(useId, actor.Seat, 0, CardKind.Slash, Array.AsReadOnly(new[] { targetSeat }), PhysicalCardIds: Array.AsReadOnly(Array.Empty<int>())) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(useId, 0, CardKind.Slash, actor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(useId, Array.AsReadOnly(new[] { targetSeat })));
        _characterLuMengMouOperations.RecordYingboCardUse(useId, actor.Seat, CardKind.Slash);
        _coreAlternativeViewAsCostsOperations.RecordProgramUsedBasicCard(actor.Seat, CardKind.Slash);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        _coreActualPlayPhaseCardUsesOperations.RecordActualPlayPhaseUse(action);
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(useId, actor.Seat, targetSeat, card: null, damageAmount: 1 + actor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(actor, _coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash), programSkillCardUseFrameId: f.Id);
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(useId, actor);
        actor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actor.Seat, targetSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeIssuedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, useId, action.ActionId, targetSeat, damageBefore));
        _coreMatchLogOperations.AddLog("CardUsed", $"{(actor.Name)} 的“伏暗”视为对 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 使用了一张无距离限制的【杀】。", actor.Seat, targetSeat);
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(useId);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void SettleShiZrFuanStrike(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        var damaged = r.UseId != 0 && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None && r.TargetSeat >= 0 && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive && _coreProgramEventHistoryOperations.ProgramEventHistory<DamageAppliedEvent>().Count(entry => entry.SourceSeat == f.OwnerSeat && entry.TargetSeat == r.TargetSeat) > r.DamageCountBefore;
        if (!damaged)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, false, "none"));
            FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShiZhangRen = r with { Stage = ShiZrStage.StrikeChoose } });
        ContinueShiZr(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id).Id);
    }

    internal void SealShiZrFuanTarget(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        if (r.TargetSeat < 0 || !_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            throw new InvalidOperationException("FuAn sealed a lost strike target.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanSealedEvent(f.Id, f.OwnerSeat, r.TargetSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber));
        _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 令 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 的技能失效直至其下个回合结束前（伏暗）。", f.OwnerSeat, r.TargetSeat);
    }

    internal SkillProgramStepOutcome ResolveShiZrYinXian(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var use = f.WindowContext?.CardUse ?? throw new InvalidOperationException("YinXian lost its completed-use window.");
        if (f.SkillId != ShiZrYinXian || f.TriggerId != ShiZhangRenComposition.YinXianUseTrigger || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat || _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._currentSeat == f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        var bonus = ShiZrYinXianBonus(f.OwnerSeat) + 1;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrYinXianBonusGainedEvent(f.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberTurnProgressionAccess._turnNumber, use.CardActionId, bonus));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 于回合外使用牌，攻击范围+1（引弦 {(bonus)}）。", f.OwnerSeat);
        return SkillProgramStepOutcome.Continue;
    }

    internal IReadOnlyList<PromptChoice> ShiZrChoices(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            return[];
        switch (r.Operation, r.Stage)
        {
            case ("fuan", ShiZrStage.FuanAsk):
                return[MakeShiZrChoice(f, "arm", null, "发动“伏暗”：攻击范围调整至0，摸X张牌并记录一个手牌花色"), MakeShiZrChoice(f, "pass", null, "不发动“伏暗”"), ];
            case ("fuan", ShiZrStage.FuanRecord)when r.DrawCount >= 0:
                return ShiZrFuanRecordableSuits(f.OwnerSeat).Select(suit => MakeShiZrChoice(f, "suit", ((int)suit).ToString(CultureInfo.InvariantCulture), $"记录花色{(DescribeSuit(suit))}")).ToArray();
            case ("strike", ShiZrStage.StrikeAsk)when ShiZrFuanStrikeQualified(f.OwnerSeat):
                return[MakeShiZrChoice(f, "arm", null, "发动“伏暗”：清除记录并对一名角色视为使用无距离限制的【杀】"), MakeShiZrChoice(f, "pass", null, "不发动“伏暗”"), ];
            case ("strike", ShiZrStage.StrikeTarget):
                return Enumerable.Range(0, _coreMatchParticipantsPlayersAccess._players.Count).Where(seat => CanIssueShiZrFuanStrike(f.OwnerSeat, seat)).Select(seat => new PromptChoice(new($"shi-zhang-ren.{(f.Id)}.target.{(seat)}"), $"对 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 视为使用无距离限制的【杀】", [], [seat], new Dictionary<string, string> { ["program-action"] = ShiZhangRenComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = "target", ["seat"] = seat.ToString(CultureInfo.InvariantCulture), })).ToArray();
            case ("strike", ShiZrStage.StrikeChoose):
                return ShiZrStrikeFollowUps(f);
            default:
                return[];
        }
    }

    internal IReadOnlyList<PromptChoice> ShiZrStrikeFollowUps(ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        var choices = new List<PromptChoice>();
        if (ShiZrFuanRecord(f.OwnerSeat)is { ViaRelaunch: false })
            choices.Add(MakeShiZrChoice(f, "relaunch", null, "再次发动“伏暗”"));
        if (r.TargetSeat >= 0 && _coreMatchParticipantsPlayersAccess._players[r.TargetSeat].IsAlive)
            choices.Add(MakeShiZrChoice(f, "seal", null, $"令 {(_coreMatchParticipantsPlayersAccess._players[r.TargetSeat].Name)} 的技能失效直至其下个回合结束前"));
        choices.Add(MakeShiZrChoice(f, "decline", null, "放弃"));
        return choices;
    }

    internal PromptChoice MakeShiZrChoice(ProgramSkillFrame f, string branch, string? value, string label) => new(new($"shi-zhang-ren.{(f.Id)}.{(branch)}.{(value ?? "-")}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiZhangRenComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, ["value"] = value ?? "", });
    internal void PublishShiZr(ProgramSkillFrame f)
    {
        var choices = ShiZrChoices(f);
        if (choices.Count == 0)
        {
            FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, f.ShiZhangRen!.Operation == "strike" ? "伏暗：选择是否发动或指定目标。" : "伏暗：选择是否发动。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiZhangRenComposition.ProgramAction)]
    internal void AnswerShiZr(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhangRen: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiZrChoices(f).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Zhang Ren choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, selected.Parameters["branch"])
        {
            case ("fuan", "arm"):
                ArmShiZrFuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZhangRenComposition.FuanDrawReason);
                return;
            case ("fuan", "pass"):
                FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("fuan", "suit")when Enum.TryParse(selected.Parameters["value"], out Suit suit):
                RecordShiZrFuanSuit(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), suit);
                return;
            case ("strike", "arm"):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiZhangRen = r with { Stage = ShiZrStage.StrikeTarget } });
                ContinueShiZr(f.Id);
                return;
            case ("strike", "pass"):
                FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("strike", "target")when int.TryParse(selected.Parameters["seat"], NumberStyles.Integer, CultureInfo.InvariantCulture, out var seat):
                IssueShiZrFuanStrike(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), seat);
                return;
            case ("strike", "relaunch"):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, true, "relaunch"));
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiZhangRen = r with { Stage = ShiZrStage.RelaunchRecord, UseId = 0, TargetSeat = -1 } });
                ArmShiZrFuan(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZhangRenComposition.FuanRelaunchDrawReason);
                return;
            case ("strike", "seal"):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, true, "seal"));
                SealShiZrFuanTarget(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("strike", "decline"):
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZrFuanStrikeSettledEvent(f.Id, r.UseId, r.TargetSeat, true, "none"));
                FinishShiZrOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Zhang Ren answered an incompatible operation branch.");
        }
    }

    internal void ContinueShiZr(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhangRen: { } r } f || f.Id != id)
            return;
        if (r.Stage is ShiZrStage.StrikeIssued)
            return;
        PublishShiZr(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    }

    internal void FinishShiZrOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { ShiZhangRen = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiZr(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68200)]
    internal bool ResumeShiZr(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShiZhangRen: { } r } f || f.Id != id)
            return false;
        switch (r.Stage)
        {
            case ShiZrStage.StrikeIssued:
                SettleShiZrFuanStrike(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiZr(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                    return true;
                ContinueShiZr(id);
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68200)]
    internal bool ReturnShiZrMovement(ProgramSkillFrame f) => f.ShiZhangRen is not null && ResumeShiZr(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZrFuan)]
    internal PromptChoice ChooseShiZrFuanAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZrFuanStrike)]
    internal PromptChoice ChooseShiZrStrikeAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var r = f.ShiZhangRen!;
        return r.Stage switch
        {
            ShiZrStage.StrikeTarget => decision.Choices.OrderBy(choice => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[choice.Targets.Single()]).Count).First(),
            ShiZrStage.StrikeChoose => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "seal") ?? decision.Choices[^1],
            _ => decision.Choices[0],
        };
    }
}
