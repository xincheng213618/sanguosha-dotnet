#nullable enable

using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGaoXiangMobileShiRuntime
{
    internal const string ShiZxChiyuan = ShiGaoXiangComposition.ChiyuanSkill, ShiZxErdi = ShiGaoXiangComposition.ErdiSkill;
    SkillProgramStepOutcome IShiGaoXiangHost.ResolveChiyuan(ProgramSkillFrame frame) => BeginShiZxChiyuan(frame);
    SkillProgramStepOutcome IShiGaoXiangHost.ResolveErdi(ProgramSkillFrame frame) => BeginShiZxErdi(frame);
    // ---- 驰援: once per play phase, hand one card to a wounded-less character
    // and virtually use a normal Slash at a character whose attack range covers
    // the receiver. If that Slash drew blood the receiver recovers one; if it
    // did not, the target uses a virtual normal Slash back at the owner.
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeCondition, 68730, SkillProgramEffectOp.ResolveShiZxChiyuan)]
    internal bool CanOfferShiZxChiyuanActivation(CharacterState owner, string skill) => skill != ShiZxChiyuan || _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count > 0 && ShiZxReceiverSeats(owner.Seat).Any(receiver => ShiZxSlashTargetSeats(owner.Seat, receiver).Any());
    internal SkillProgramStepOutcome BeginShiZxChiyuan(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.GaoXiang is not null)
            throw new InvalidOperationException("Shi Gao Xiang restarted a running operation.");
        if (f.SkillId != ShiZxChiyuan || f.TriggerId is not null || f.SelectedCardIds is not [var gift] || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || _cardZonesOperations._cardZones.GetLocation(gift)is not { Zone: CardZoneKind.Hand, OwnerSeat: var holder } || holder != f.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        var receivers = ShiZxReceiverSeats(f.OwnerSeat).ToArray();
        if (receivers.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GaoXiang = new("chiyuan", ShiZxStage.PickReceiver) { CardId = gift } });
        PublishShiZxChoices(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZxStage.PickReceiver, receivers.Select(seat => (seat, default(int? ))).ToArray());
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome BeginShiZxErdi(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.GaoXiang is not null)
            throw new InvalidOperationException("Shi Gao Xiang restarted a running operation.");
        if (f.SkillId != ShiZxErdi || f.TriggerId != ShiGaoXiangComposition.ErdiTrigger || f.SelectedCardIds is { Count: > 0 } || f.SelectedTargetSeats.Count != 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        var targets = ShiZxErdiTargetSeats(f.OwnerSeat).ToArray();
        if (targets.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { GaoXiang = new("erdi", ShiZxStage.ErdiTarget) });
        PublishShiZxChoices(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZxStage.ErdiTarget, targets.Select(seat => (seat, default(int? ))).ToArray());
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IEnumerable<int> ShiZxErdiTargetSeats(int ownerSeat) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != ownerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Count).Select(player => player.Seat);
    internal IEnumerable<int> ShiZxReceiverSeats(int ownerSeat)
    {
        var hp = _coreMatchParticipantsPlayersAccess._players[ownerSeat].HasHpValue ? _coreMatchParticipantsPlayersAccess._players[ownerSeat].Hp : int.MaxValue;
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != ownerSeat && player.HasHpValue && player.Hp < hp).Select(player => player.Seat);
    }

    internal IEnumerable<int> ShiZxSlashTargetSeats(int ownerSeat, int receiverSeat) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != ownerSeat && player.Seat != receiverSeat && ShiZxRangeCoverage(player.Seat).Contains(receiverSeat)).Select(player => player.Seat);
    internal HashSet<int> ShiZxRangeCoverage(int seat)
    {
        var range = _coreNativeRuleQueriesOperations.GetAttackRange(seat);
        return _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != seat && _coreNativeRuleQueriesOperations.GetCombatDistance(seat, player.Seat) <= range).Select(player => player.Seat).ToHashSet();
    }

    internal void PublishShiZx(ProgramSkillFrame f, ShiZxStage stage, (int Seat, int? Card)[] options, IReadOnlyList<int> selected)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = new List<PromptChoice>();
        string label = stage switch
        {
            ShiZxStage.PickReceiver => "选择交牌的角色。",
            ShiZxStage.PickSlashTarget => "选择视为使用【杀】的目标。",
            ShiZxStage.ErdiTarget => "选择一名手牌数大于你的角色。",
            _ => "请选择。",
        };
        foreach (var(seat, card)in options)
            choices.Add(new PromptChoice(new ChoiceId($"shi-gao-xiang.{(f.Id)}.seat-{(seat)}{((card is { } id ? $"-{(id)}" : string.Empty))}"), $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", card is { } cardId ? [cardId] : [], [seat], new Dictionary<string, string> { ["program-action"] = ShiGaoXiangComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"seat-{(seat)}", }));
        if (stage is ShiZxStage.OwnerDiscarding or ShiZxStage.TheirDiscarding)
        {
            var discarder = stage == ShiZxStage.OwnerDiscarding ? f.OwnerSeat : f.GaoXiang!.ErdiTargetSeat;
            label = stage == ShiZxStage.OwnerDiscarding ? "【饵敌】：选择任意张手牌弃置，选完后确认。" : $"【饵敌】：{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 对你发动饵敌，请同时选择任意张手牌弃置。";
            choices.AddRange(ShiZxDiscardChoices(f, f.GaoXiang!, discarder));
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, $"【{(skill.Name)}】：{(label)}", choices.SelectMany(choice => choice.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = f.OwnerSeat,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal PromptChoice MakeShiZxChoice(ProgramSkillFrame f, string branch, string label) => new(new($"shi-gao-xiang.{(f.Id)}.{(branch)}"), label, [], [], new Dictionary<string, string> { ["program-action"] = ShiGaoXiangComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = branch, });
    internal PromptChoice MakeShiZxDiscardChoice(ProgramSkillFrame f, string branch, int cardId, IReadOnlyList<int> selected) => new(new($"shi-gao-xiang.{(f.Id)}.{(branch)}-{(cardId)}.{(string.Join('-', selected))}"), branch == "pick" ? $"选择弃置【{(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(card => card.Id == cardId).DisplayName)}】" : $"取消选择【{(_cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(card => card.Id == cardId).DisplayName)}】", [cardId], [], new Dictionary<string, string> { ["program-action"] = ShiGaoXiangComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"{(branch)}-{(cardId)}", });
    internal void AnswerShiZxSeat(ProgramSkillFrame f, PromptChoice selected)
    {
        var r = f.GaoXiang!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        switch (r.Operation, r.Stage, selected.Parameters["branch"])
        {
            case ("chiyuan", ShiZxStage.PickReceiver, var branch):
                var receiver = int.Parse(branch["seat-".Length..], CultureInfo.InvariantCulture);
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Stage = ShiZxStage.PickSlashTarget, ReceiverSeat = receiver } });
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), CardLocation.Hand(f.OwnerSeat), CardLocation.Hand(receiver), new CardMoveReason(ShiGaoXiangComposition.ChiyuanGiveReason), disclosure: CardMovementDisclosure.FaceUp);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZxChiyuanGivenEvent(f.Id, f.OwnerSeat, receiver, r.CardId));
                if (DrainShiZx(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                    return;
                var targets = ShiZxSlashTargetSeats(f.OwnerSeat, receiver).ToArray();
                if (targets.Length == 0)
                {
                    FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
                    return;
                }

                PublishShiZxChoices(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZxStage.PickSlashTarget, targets.Select(seat => (seat, default(int? ))).ToArray());
                return;
            case ("chiyuan", ShiZxStage.PickSlashTarget, var branch):
                var slashTarget = int.Parse(branch["seat-".Length..], CultureInfo.InvariantCulture);
                IssueShiZxSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), slashTarget, revenge: false);
                return;
            case ("erdi", ShiZxStage.ErdiTarget, var branch):
                var erdiTarget = int.Parse(branch["seat-".Length..], CultureInfo.InvariantCulture);
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Stage = ShiZxStage.OwnerDiscarding, ErdiTargetSeat = erdiTarget } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZxErdiStartedEvent(f.Id, f.OwnerSeat, erdiTarget, _coreActualTurnProgressTurnNumberAccess._turnNumber));
                _corePrepPoisonOutcomesOperations.ApplyProgramHpLoss(f.Id, f.SkillId, f.OwnerSeat, 1);
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], 1, log: true);
                if (DrainShiZx(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                    return;
                PublishShiZxDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZxStage.OwnerDiscarding);
                return;
            default:
                throw new InvalidOperationException("Shi Gao Xiang answered an incompatible seat branch.");
        }
    }

    internal void PublishShiZxChoices(ProgramSkillFrame f, ShiZxStage stage, (int Seat, int? Card)[] options) => PublishShiZx(f, stage, options, []);
    internal void PublishShiZxDiscard(ProgramSkillFrame f, ShiZxStage stage)
    {
        var r = f.GaoXiang!;
        var seat = stage == ShiZxStage.OwnerDiscarding ? f.OwnerSeat : r.ErdiTargetSeat;
        PublishShiZxChoices(f, stage, []);
        var prompt = _corePendingDecisionPolicyPipelineOperations._pendingDecision!;
        if (seat != f.OwnerSeat)
        {
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(prompt.Kind, seat, prompt.Prompt, prompt.ValidCardIds, prompt.ValidTargetSeats, f.OwnerSeat)
            {
                PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
                IsPrivate = true,
                TargetSeat = seat,
                SkillPrompt = prompt.SkillPrompt,
                Choices = prompt.Choices
            };
            _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        }
    }

    internal void IssueShiZxSlash(ProgramSkillFrame f, int targetSeat, bool revenge)
    {
        var r = f.GaoXiang!;
        var actorSeat = revenge ? _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiZxChiyuanSlashIssuedEvent>().First(issue => issue.FrameId == f.Id).TargetSeat : f.OwnerSeat;
        var target = revenge ? f.OwnerSeat : targetSeat;
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { GaoXiang = r with { Stage = revenge ? ShiZxStage.RevengeSettling : ShiZxStage.SlashSettling, DamageEventsBefore = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Count(), Revenge = revenge } });
        var id = _coreNativeCardUseOperations.BeginCardUse(new Card(0, CardKind.Slash, Suit.None, 0), actorSeat, [target], CardKind.Slash, physicalCardIds: []);
        var targets = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.TargetSeats;
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(id, actorSeat, targets[0], card: null, damageAmount: 1 + _coreMatchParticipantsPlayersAccess._players[actorSeat].AlcoholDamageBonus, playedCardKind: CardKind.Slash, programSkillCardUseFrameId: f.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreCurrentCardEnhancementsOperations.CaptureProgramAdjustedSlashBaseDamage(attack);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, actorSeat, targets[0]));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZxChiyuanSlashIssuedEvent(f.Id, f.OwnerSeat, id, r.ReceiverSeat, targets[0]));
        _coreNativeAiDecisionsOperations.NotifyAiOfSlash(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[targets[0]]);
        if (!_coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(id) || !_coreCardActionsOperations.TryBeginProgramCardWindow(attack, _coreCardUseLifecycleOperations.LifecycleCardUse(id)!.Action!, SkillProgramTriggerWindow.CardUseCommitted, targets, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    [ProgramAnswer(ShiGaoXiangComposition.ProgramAction)]
    internal void AnswerShiZx(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GaoXiang: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } pending || !ShiZxCurrentChoices(f).Any(choice => choice.Id == selected.Id && choice.Cards.SequenceEqual(selected.Cards) && choice.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Shi Gao Xiang choice lost its current frame or legal branch.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        var branch = selected.Parameters["branch"];
        if (branch.StartsWith("seat-", StringComparison.Ordinal))
        {
            AnswerShiZxSeat(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), selected);
            return;
        }

        switch (r.Operation, r.Stage, branch)
        {
            case ("erdi", ShiZxStage.OwnerDiscarding or ShiZxStage.TheirDiscarding, var b)when b.StartsWith("pick-", StringComparison.Ordinal):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Selected = [..r.Selected, int.Parse(b["pick-".Length..], CultureInfo.InvariantCulture)] } });
                PublishShiZxDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.Stage);
                return;
            case ("erdi", ShiZxStage.OwnerDiscarding or ShiZxStage.TheirDiscarding, var b)when b.StartsWith("unpick-", StringComparison.Ordinal):
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Selected = r.Selected.Where(id => id != int.Parse(b["unpick-".Length..], CultureInfo.InvariantCulture)).ToArray() } });
                PublishShiZxDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.Stage);
                return;
            case ("erdi", ShiZxStage.OwnerDiscarding, "confirm"):
                var ownerCount = r.Selected.Length;
                var ownerSelection = r.Selected;
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Stage = ShiZxStage.TheirDiscarding, OwnerDiscardCount = ownerCount, Selected = [] } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZxErdiDiscardedEvent(f.Id, f.OwnerSeat, ownerCount));
                DiscardShiZxSelected(f.OwnerSeat, ownerSelection);
                if (DrainShiZx(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                    return;
                PublishShiZxDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), ShiZxStage.TheirDiscarding);
                return;
            case ("erdi", ShiZxStage.TheirDiscarding, "confirm"):
                var theirCount = r.Selected.Length;
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Stage = ShiZxStage.ErdiSettling } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiZxErdiDiscardedEvent(f.Id, r.ErdiTargetSeat, theirCount));
                DiscardShiZxSelected(r.ErdiTargetSeat, r.Selected);
                if (DrainShiZx(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)))
                    return;
                SettleShiZxErdi(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.OwnerDiscardCount, theirCount);
                return;
            default:
                throw new InvalidOperationException("Shi Gao Xiang answered an incompatible discard branch.");
        }

        void DiscardShiZxSelected(int seat, int[] cardIds)
        {
            foreach (var cardId in cardIds)
            {
                var from = _cardZonesOperations._cardZones.GetLocation(cardId);
                _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(cardId), from, CardLocation.DiscardPile, new CardMoveReason(ShiGaoXiangComposition.ErdiDiscardReason), disclosure: CardMovementDisclosure.FaceUp);
            }
        }
    }

    internal IReadOnlyList<PromptChoice> ShiZxCurrentChoices(ProgramSkillFrame f)
    {
        var r = f.GaoXiang!;
        return (r.Operation, r.Stage) switch
        {
            ("chiyuan", ShiZxStage.PickReceiver) => ShiZxReceiverSeats(f.OwnerSeat).Select(seat => MakeShiZxSeatChoice(f, seat)).ToArray(),
            ("chiyuan", ShiZxStage.PickSlashTarget) => ShiZxSlashTargetSeats(f.OwnerSeat, r.ReceiverSeat).Select(seat => MakeShiZxSeatChoice(f, seat)).ToArray(),
            ("erdi", ShiZxStage.ErdiTarget) => ShiZxErdiTargetSeats(f.OwnerSeat).Select(seat => MakeShiZxSeatChoice(f, seat)).ToArray(),
            ("erdi", ShiZxStage.OwnerDiscarding or ShiZxStage.TheirDiscarding) => ShiZxDiscardChoices(f, r, r.Stage == ShiZxStage.OwnerDiscarding ? f.OwnerSeat : r.ErdiTargetSeat),
            _ => [],
        };
    }

    internal PromptChoice MakeShiZxSeatChoice(ProgramSkillFrame f, int seat) => new(new($"shi-gao-xiang.{(f.Id)}.seat-{(seat)}"), $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}", [], [seat], new Dictionary<string, string> { ["program-action"] = ShiGaoXiangComposition.ProgramAction, ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["branch"] = $"seat-{(seat)}", });
    internal IReadOnlyList<PromptChoice> ShiZxDiscardChoices(ProgramSkillFrame f, ShiZxReceipt r, int seat)
    {
        var choices = new List<PromptChoice>();
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(card => !r.Selected.Contains(card.Id)))
            choices.Add(MakeShiZxDiscardChoice(f, "pick", card.Id, r.Selected));
        foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(card => r.Selected.Contains(card.Id)))
            choices.Add(MakeShiZxDiscardChoice(f, "unpick", card.Id, r.Selected));
        choices.Add(MakeShiZxChoice(f, "confirm", $"确认弃置 {(r.Selected.Length)} 张手牌"));
        return choices;
    }

    internal void SettleShiZxErdi(ProgramSkillFrame f, int ownerCount, int theirCount)
    {
        var r = f.GaoXiang!;
        var opponent = _coreMatchParticipantsPlayersAccess._players[r.ErdiTargetSeat];
        if (ownerCount > theirCount)
        {
            ShiZxRecover(f, f.OwnerSeat, 1);
            if (opponent.IsAlive && _coreMatchOutcomeStatusWinnerAccess._winner == Winner.None)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = r with { Stage = ShiZxStage.ErdiSettling, DamageCount = 1 } });
                _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), r.ErdiTargetSeat, 1, explicitSourceSeat: f.OwnerSeat);
                return;
            }

            FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (ownerCount < theirCount)
            _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], theirCount, log: true);
        FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
    }

    internal void ShiZxRecover(ProgramSkillFrame f, int seat, int amount)
    {
        var holder = _coreMatchParticipantsPlayersAccess._players[seat];
        var requested = Math.Min(amount, holder.MaxHp - holder.Hp);
        if (requested <= 0 || !holder.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return;
        if (_coreRecoveryReplacementOperations.TryQueueRecoveryReplacement(f.Id, f.OwnerSeat, seat, requested, new(RecoveryAttemptProducer.Program, f.InstructionIndex)))
            return;
        var recovery = _coreNativeRecoveryFramesOperations.BeginRecovery(f.Id, f.OwnerSeat, seat, requested);
        holder.Hp += requested;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RecoveryAppliedEvent(f.OwnerSeat, seat, requested, holder.Hp));
        _coreRuntimeOperations.PopResolutionFrame(recovery, ResolutionFrameKind.Recovery);
    }

    internal void FinishShiZxOperation(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { GaoXiang = null });
        _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
    }

    internal bool DrainShiZx(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 68740)]
    internal bool ResumeShiZx(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { GaoXiang: { } r } f || f.Id != id)
            return false;
        switch (r.Operation, r.Stage)
        {
            case ("chiyuan", ShiZxStage.SlashSettling):
                SettleShiZxChiyuanSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            case ("chiyuan", ShiZxStage.RevengeSettling):
                FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            case ("erdi", ShiZxStage.ErdiSettling)when r.DamageCount > 0:
                FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
            default:
                if (DrainShiZx(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)))
                    return true;
                ContinueShiZxPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
                return true;
        }
    }

    internal void ContinueShiZxPrompt(ProgramSkillFrame f)
    {
        var r = f.GaoXiang!;
        switch (r.Operation, r.Stage)
        {
            case ("chiyuan", ShiZxStage.PickReceiver):
                PublishShiZxChoices(f, ShiZxStage.PickReceiver, ShiZxReceiverSeats(f.OwnerSeat).Select(seat => (seat, default(int? ))).ToArray());
                return;
            case ("chiyuan", ShiZxStage.PickSlashTarget):
                PublishShiZxChoices(f, ShiZxStage.PickSlashTarget, ShiZxSlashTargetSeats(f.OwnerSeat, r.ReceiverSeat).Select(seat => (seat, default(int? ))).ToArray());
                return;
            case ("erdi", ShiZxStage.ErdiTarget):
                PublishShiZxChoices(f, ShiZxStage.ErdiTarget, ShiZxErdiTargetSeats(f.OwnerSeat).Select(seat => (seat, default(int? ))).ToArray());
                return;
            case ("erdi", ShiZxStage.OwnerDiscarding or ShiZxStage.TheirDiscarding):
                PublishShiZxDiscard(f, r.Stage);
                return;
            default:
                FinishShiZxOperation(f);
                return;
        }
    }

    internal void SettleShiZxChiyuanSlash(ProgramSkillFrame f)
    {
        var r = f.GaoXiang!;
        var issued = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<ShiZxChiyuanSlashIssuedEvent>().Last(issue => issue.FrameId == f.Id);
        var fresh = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<DamageAppliedEvent>().Skip(r.DamageEventsBefore);
        var damaged = fresh.Any(e => e.SourceCardUseResolutionId == issued.UseId || !r.Revenge && e.SourceSeat == f.OwnerSeat && e.TargetSeat == issued.TargetSeat || r.Revenge && e.SourceSeat == issued.TargetSeat && e.TargetSeat == f.OwnerSeat);
        if (damaged)
        {
            ShiZxRecover(f, issued.ReceiverSeat, 1);
            FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        if (!_coreMatchParticipantsPlayersAccess._players[issued.TargetSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishShiZxOperation(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id));
            return;
        }

        IssueShiZxSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), issued.TargetSeat, revenge: true);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 68740)]
    internal bool ReturnShiZxMovement(ProgramSkillFrame f) => f.GaoXiang is not null && ResumeShiZx(f.Id);
    // ---- AI -------------------------------------------------------------------
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZxChiyuan)]
    internal PromptChoice ChooseShiZxChiyuanAi(PendingDecision decision, ProgramSkillFrame f) => decision.Choices[0];
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShiZxErdi)]
    internal PromptChoice ChooseShiZxErdiAi(PendingDecision decision, ProgramSkillFrame f)
    {
        var hasSelection = decision.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch")?.StartsWith("unpick-", StringComparison.Ordinal) == true);
        var pick = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch")?.StartsWith("pick-", StringComparison.Ordinal) == true);
        if (pick is not null && !hasSelection)
            return pick;
        var confirm = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("branch") == "confirm");
        if (confirm is not null)
            return confirm;
        return decision.Choices[0];
    }
}
