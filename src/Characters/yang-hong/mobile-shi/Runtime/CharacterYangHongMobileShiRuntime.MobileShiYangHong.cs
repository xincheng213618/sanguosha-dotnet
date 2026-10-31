#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangHongMobileShiRuntime
{
    internal const string ShiYhJijian = ShiYangHongComposition.JijianSkill, ShiYhYuanmo = ShiYangHongComposition.YuanmoSkill;
    SkillProgramStepOutcome IShiYangHongHost.ResolveJijian(ProgramSkillFrame frame) => BeginShiYhJijian(frame);
    SkillProgramStepOutcome IShiYangHongHost.ResolveYuanmo(ProgramSkillFrame frame) => BeginShiYhYuanmo(frame);
    internal SkillProgramStepOutcome BeginShiYhJijian(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.YangHong is not null)
            throw new InvalidOperationException("Shi Yang Hong restarted a running operation.");
        if (f.SkillId != ShiYhJijian || f.TriggerId is not null || f.SelectedCardIds is not [var hidden] || f.SelectedTargetSeats is not [var first, var second] || first == second || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[first].IsAlive || !_coreMatchParticipantsPlayersAccess._players[second].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[first]).Count == 0 || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[second]).Count == 0 || _cardZonesOperations._cardZones.GetLocation(hidden)is not { Zone: CardZoneKind.Hand, OwnerSeat: var holder } || holder != f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangHong = new("jijian", ShiYhStage.SwapFirst) { CardId = hidden, FirstSeat = first, SecondSeat = second } });
        PublishShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void LaunchShiYhContest(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiYhJijianStartedEvent(f.Id, f.OwnerSeat, r.CardId, r.FirstSeat, r.SecondSeat, r.FirstSwap, r.SecondSwap));
        if (r.FirstSwap && r.SecondSwap)
        {
            // Both chose the same hidden card, so the ranks tie and nobody wins.
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiYhJijianResolvedEvent(f.Id, f.OwnerSeat, "tied", null, null));
            FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (r.FirstSwap || r.SecondSwap)
        {
            var swapper = r.FirstSwap ? r.FirstSeat : r.SecondSeat;
            var other = r.FirstSwap ? r.SecondSeat : r.FirstSeat;
            if (swapper != f.OwnerSeat)
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), CardLocation.Hand(f.OwnerSeat), CardLocation.Hand(swapper), new CardMoveReason(ShiYangHongComposition.JijianLendReason));
            _corePindianOperations.BeginSharedPindian(f.Id, new SkillPromptPresentation(f.SkillId, skill.Name, $"{(skill.Name)} · 拼点", skill.Description), swapper, other, sourceCardId: r.CardId, programResultBind: ShiYangHongComposition.PindianBind, programResultVisibility: SkillProgramCardSetVisibility.Public);
            return;
        }

        _corePindianOperations.BeginSharedPindian(f.Id, new SkillPromptPresentation(f.SkillId, skill.Name, $"{(skill.Name)} · 拼点", skill.Description), r.FirstSeat, r.SecondSeat, programResultBind: ShiYangHongComposition.PindianBind, programResultVisibility: SkillProgramCardSetVisibility.Public);
    }

    internal void ContinueShiYhContest(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        var binding = f.PindianResultBindings.Single(item => item.Name == ShiYangHongComposition.PindianBind);
        int? winner = binding.SourceRank > binding.OpponentRank ? binding.SourceSeat : binding.OpponentRank > binding.SourceRank ? binding.OpponentSeat : null;
        int? loser = winner == binding.SourceSeat ? binding.OpponentSeat : winner == binding.OpponentSeat ? binding.SourceSeat : null;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiYhJijianResolvedEvent(f.Id, f.OwnerSeat, winner is null ? "tied" : "slash", winner, loser));
        if (winner is not { } winnerSeat || loser is not { } loserSeat)
        {
            BeginShiYhRetaliation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.Slash } });
        BeginShiYhSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), winnerSeat, loserSeat);
    }

    internal void BeginShiYhSlash(ProgramSkillFrame f, int winnerSeat, int loserSeat)
    {
        var id = _coreNativeCardUseOperations.BeginCardUse(new Card(0, CardKind.Slash, Suit.None, 0), winnerSeat, [loserSeat], CardKind.Slash, physicalCardIds: []);
        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, winnerSeat, targets[0], card: null, damageAmount: 1 + _coreMatchParticipantsPlayersAccess._players[winnerSeat].AlcoholDamageBonus, playedCardKind: CardKind.Slash, programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, winnerSeat, targets[0]));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(_coreMatchParticipantsPlayersAccess._players[winnerSeat], _coreMatchParticipantsPlayersAccess._players[targets[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void BeginShiYhRetaliation(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        if (_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind != CardKind.Slash)
        {
            FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.Retaliate } });
        AdvanceShiYhRetaliation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void AdvanceShiYhRetaliation(ProgramSkillFrame f)
    {
        var r = f.YangHong!;
        if (r.FirstSwap && _coreMatchParticipantsPlayersAccess._players[r.FirstSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { FirstSwap = false } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.FirstSeat, 1, explicitSourceSeat: f.OwnerSeat);
            return;
        }

        if (r.SecondSwap && _coreMatchParticipantsPlayersAccess._players[r.SecondSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { SecondSwap = false } });
            _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.SecondSeat, 1, explicitSourceSeat: f.OwnerSeat);
            return;
        }

        FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal SkillProgramStepOutcome BeginShiYhYuanmo(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.YangHong is not null)
            throw new InvalidOperationException("Shi Yang Hong restarted a running operation.");
        if (f.SkillId != ShiYhYuanmo || f.TriggerId is not (ShiYangHongComposition.YuanmoPrepTrigger or ShiYangHongComposition.YuanmoDamageTrigger) || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiYhYuanmoUsedEvent>().Count(entry => entry.OwnerSeat == f.OwnerSeat && entry.Turn == _coreActualTurnProgressTurnNumberAccess._turnNumber) >= 2 || !ShiYhFieldMovesExist())
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { YangHong = new("yuanmo", ShiYhStage.MoveChoice) });
        PublishShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal bool ShiYhFieldMovesExist()
    {
        foreach (var _ in ShiYhFieldMoveChoices(null))
            return true;
        return false;
    }

    internal IEnumerable<PromptChoice> ShiYhFieldMoveChoices(ProgramSkillFrame? f)
    {
        foreach (var source in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive))
            foreach (var zone in new[]
            {
                CardZoneKind.Equipment,
                CardZoneKind.Judgment
            }

            )
                foreach (var card in _cardZonesOperations._cardZones.CardsAt(new CardLocation(zone, source.Seat)))
                    foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != source.Seat))
                    {
                        var legal = zone == CardZoneKind.Equipment ? !card.IsGeneralWeapon && _coreProgramCardPaymentsOperations.CanEnterEquipmentSlot(target.Seat, card) : _coreProgramCardPaymentsOperations.CanMoveProgramCardToCorrespondingZone(card, zone, target.Seat);
                        if (!legal)
                            continue;
                        if (f is null)
                        {
                            yield return new PromptChoice(new("shi-yang-hong.field-probe"), "移动场上牌", [card.Id], [target.Seat], new Dictionary<string, string>());
                            yield break;
                        }

                        yield return MakeShiYhChoice(f, $"move-{(card.Id)}-{(target.Seat)}", $"将 {(source.Name)}{((zone == CardZoneKind.Equipment ? "装备区" : "判定区"))} 的一张牌移至 {(target.Name)}", [card.Id], [target.Seat]);
                    }
    }

    internal void ApplyShiYhMove(ProgramSkillFrame f, PromptChoice choice)
    {
        var r = f.YangHong!;
        var cardId = choice.Cards.Single();
        var targetSeat = choice.Targets.Single();
        var from = _cardZonesOperations._cardZones.GetLocation(cardId);
        var moverSeat = from.OwnerSeat ?? throw new InvalidOperationException("YuanMo moved an unowned card.");
        var rangeBefore = ShiYhRangeCoverage(moverSeat);
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), from, new CardLocation(from.Zone, targetSeat), new CardMoveReason(ShiYangHongComposition.YuanmoMoveReason), disclosure: CardMovementDisclosure.FaceUp);
        var rangeAfter = ShiYhRangeCoverage(moverSeat);
        var drawn = Math.Min(5, rangeBefore.Count(covered => !rangeAfter.Contains(covered)));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiYhYuanmoMovedEvent(f.Id, f.OwnerSeat, cardId, from, new CardLocation(from.Zone, targetSeat), 0));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiYhYuanmoUsedEvent(f.Id, f.OwnerSeat, _coreActualTurnProgressTurnNumberAccess._turnNumber));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的“远谟”移动了场上的一张牌。", f.OwnerSeat, targetSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.DrawChoice, MoverSeat = moverSeat, Drawn = drawn } });
        ContinueShiYh(f.Id);
    }

    internal HashSet<int> ShiYhRangeCoverage(int seat)
    {
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(seat);
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != seat && _coreNativeRuleQueriesOperations.GetCombatDistance(seat, player.Seat) <= range).Select(player => player.Seat).ToHashSet();
    }

    internal IReadOnlyList<PromptChoice> ShiYhChoices(ProgramSkillFrame f)
    {
        var r = f.YangHong;
        if (r is null || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        switch (r.Operation, r.Stage)
        {
            case ("jijian", ShiYhStage.SwapFirst):
            case ("jijian", ShiYhStage.SwapSecond):
                return[MakeShiYhChoice(f, "swap", "改为用此牌进行拼点"), MakeShiYhChoice(f, "keep", "以自己的手牌拼点"), ];
            case ("yuanmo", ShiYhStage.MoveChoice):
                return ShiYhFieldMoveChoices(f).Append(MakeShiYhChoice(f, "pass", "不移动场上牌")).ToArray();
            case ("yuanmo", ShiYhStage.DrawChoice):
            {
                var choices = new List<PromptChoice>();
                if (r.Drawn > 0 && _coreMatchParticipantsPlayersAccess._players[r.MoverSeat].IsAlive)
                    choices.Add(MakeShiYhChoice(f, "draw", $"令 {(_coreMatchParticipantsPlayersAccess._players[r.MoverSeat].Name)} 摸 {(r.Drawn)} 张牌"));
                choices.Add(MakeShiYhChoice(f, "pass", "不发动摸牌"));
                return choices;
            }

            default:
                return[];
        }
    }

    internal PromptChoice MakeShiYhChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null, IReadOnlyList<int>? targets = null) => new(new($"shi-yang-hong.{(f.Id)}.{(branch)}"), label, cards ?? [], targets ?? [], new Dictionary<string, string> { ["program-action"] = ShiYangHongComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal void PublishShiYh(ProgramSkillFrame f)
    {
        var choices = ShiYhChoices(f);
        if (choices.Count == 0)
        {
            FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var r = f.YangHong!;
        var seat = r.Operation == "jijian" && r.Stage == ShiYhStage.SwapFirst ? r.FirstSeat : r.Operation == "jijian" && r.Stage == ShiYhStage.SwapSecond ? r.SecondSeat : f.OwnerSeat;
        if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
        {
            FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, r.Operation == "jijian" ? "间计：请选择。" : "远谟：请选择。", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer(ShiYangHongComposition.ProgramAction)]
    internal void AnswerShiYh(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YangHong: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiYhChoices(f).Any(choice => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], [selected])))
            throw new InvalidOperationException("Shi Yang Hong choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        switch (r.Operation, r.Stage, branch)
        {
            case ("jijian", ShiYhStage.SwapFirst, "swap"):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.SwapSecond, FirstSwap = true } });
                PublishShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("jijian", ShiYhStage.SwapFirst, "keep"):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.SwapSecond } });
                PublishShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("jijian", ShiYhStage.SwapSecond, "swap"):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.Contest, SecondSwap = true } });
                LaunchShiYhContest(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("jijian", ShiYhStage.SwapSecond, "keep"):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = r with { Stage = ShiYhStage.Contest } });
                LaunchShiYhContest(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("yuanmo", ShiYhStage.MoveChoice, var move)when move.StartsWith("move-", StringComparison.Ordinal):
                ApplyShiYhMove(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), selected);
                return;
            case ("yuanmo", ShiYhStage.MoveChoice, "pass"):
                FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("yuanmo", ShiYhStage.DrawChoice, "draw"):
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[r.MoverSeat], r.Drawn, log: true);
                FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            case ("yuanmo", ShiYhStage.DrawChoice, "pass"):
                FinishShiYhOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                return;
            default:
                throw new InvalidOperationException("Shi Yang Hong answered an incompatible operation branch.");
        }
    }

    internal void ContinueShiYh(long id) => PublishShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
    internal void FinishShiYhOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { YangHong = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiYh(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68640)]
    internal bool ResumeShiYh(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { YangHong: { } r } f || f.Id != id)
            return false;
        switch (r.Operation, r.Stage)
        {
            case ("jijian", ShiYhStage.Contest):
                ContinueShiYhContest(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            case ("jijian", ShiYhStage.Slash):
                BeginShiYhRetaliation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            case ("jijian", ShiYhStage.Retaliate):
                AdvanceShiYhRetaliation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                    return true;
                PublishShiYh(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
        }
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68640)]
    internal bool ReturnShiYhMovement(ProgramSkillFrame f) => f.YangHong is not null && ResumeShiYh(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiYhJijian)]
    internal PromptChoice ChooseShiYhJijianAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "keep") ?? decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiYhYuanmo)]
    internal PromptChoice ChooseShiYhYuanmoAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "pass") ?? decision.Choices.LastOrDefault() ?? decision.Choices[0];
}
