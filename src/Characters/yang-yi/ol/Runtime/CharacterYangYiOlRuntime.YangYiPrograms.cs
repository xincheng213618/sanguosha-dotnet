#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterYangYiOlRuntime
{
    internal const int MaximumJuanxiaTrickCount = 3;
    internal SkillProgramStepOutcome JuanxiaProgramDeclareTricks(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (active.SelectedTargetSeats is not [var target] || target == active.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { JuanxiaLaunch = new JuanxiaLaunchState(target, 0, [], JuanxiaStage.Choice, null) });
        return PresentJuanxiaTrickPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    internal bool IsJuanxiaTrickCandidate(int ownerSeat, int targetSeat, CardKind kind, IReadOnlyList<string> usedKinds)
    {
        if (usedKinds.Contains(kind.ToString(), StringComparer.Ordinal))
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!owner.IsAlive || !target.IsAlive || !IsJuanxiaSingleTargetTrick(kind))
            return false;
        if (_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(target, kind) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(ownerSeat, targetSeat, kind))
            return false;
        return kind switch
        {
            CardKind.Duel => true,
            CardKind.Dismantlement => _corePhysicalOwnedCardQueriesOperations.HasTargetCard(target),
            CardKind.Snatch => _corePhysicalOwnedCardQueriesOperations.HasTargetCard(target) && (_coreNativeRuleQueriesOperations.GetCombatDistance(ownerSeat, targetSeat) == 1 || _coreDirectedCardPolicyQueriesOperations.HasCardDistanceExemption(owner, target, CardKind.Snatch) || _coreProgramCardPoliciesOperations.HasCardPolicy(owner, SkillProgramCardPolicyKind.IgnoreUseDistance, CardKind.Snatch)),
            CardKind.FireAttack => _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0,
            _ => false
        };
    }

    internal static bool IsJuanxiaSingleTargetTrick(CardKind kind) => RuntimeSkills.RuntimeRules_yang_yi_ol_YangYiPrograms.IsJuanxiaSingleTargetTrick(kind);
    internal SkillProgramStepOutcome PresentJuanxiaTrickPrompt(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaLaunch ?? throw new InvalidOperationException("The Juanxia launch lost its frame state.");
        if (state.Stage != JuanxiaStage.Choice)
        {
            // A returned child re-presents this prompt; normalize the stage so
            // the next choice resolution recognizes its suspended chooser.
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { JuanxiaLaunch = state with { Stage = JuanxiaStage.Choice, PendingKind = null } });
            active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            state = active.JuanxiaLaunch!;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || !_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].IsAlive || state.UsedCount >= MaximumJuanxiaTrickCount)
        {
            FinishJuanxiaLaunch(active);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var candidates = CharacterYangYiOlDefinitions.JuanxiaTrickOrder.Where(kind => IsJuanxiaTrickCandidate(active.OwnerSeat, state.TargetSeat, kind, state.UsedKinds)).ToArray();
        if (candidates.Length == 0)
        {
            FinishJuanxiaLaunch(active);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(kind => new PromptChoice(new ChoiceId($"juanxia-trick.frame-{(frame.Id)}.{(kind)}"), $"视为对 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】。", [], [state.TargetSeat], new Dictionary<string, string> { ["program-action"] = "juanxia-trick", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["trick"] = kind.ToString() })).Append(new PromptChoice(new ChoiceId($"juanxia-trick.frame-{(frame.Id)}.stop"), "停止发动。", [], [], new Dictionary<string, string> { ["program-action"] = "juanxia-trick", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["trick"] = "stop" })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择视为对 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 使用的普通锦囊，或停止。", [], [state.TargetSeat], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择普通锦囊", $"至多 {(MaximumJuanxiaTrickCount)} 张牌名各不相同、仅指定唯一目标的普通锦囊牌依次视为使用。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void FinishJuanxiaLaunch(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaLaunch;
        if (state is null)
            return;
        if (state.UsedCount > 0 && _coreMatchParticipantsPlayersAccess._players[state.TargetSeat].IsAlive)
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJuanxiaDebtEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, state.TargetSeat, state.UsedCount));
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 狷狭：{(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 的下一个结束阶段开始时，其可以视为对 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 使用 {(state.UsedCount)} 张【杀】。", active.OwnerSeat, state.TargetSeat);
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { JuanxiaLaunch = null });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    [ProgramAnswer("juanxia-trick")]
    internal void ResolveJuanxiaTrickChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Juanxia trick choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.JuanxiaDeclareTricks } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Juanxia trick choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat || active.JuanxiaLaunch is not { } state || state.Stage != JuanxiaStage.Choice)
            throw new InvalidOperationException("The Juanxia trick chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var trickParameter = selected.Parameters.GetValueOrDefault("trick");
        if (trickParameter == "stop")
        {
            FinishJuanxiaLaunch(active);
            return;
        }

        if (!Enum.TryParse<CardKind>(trickParameter, out var kind) || !IsJuanxiaSingleTargetTrick(kind) || !IsJuanxiaTrickCandidate(active.OwnerSeat, state.TargetSeat, kind, state.UsedKinds))
            throw new InvalidOperationException("The selected Juanxia trick is no longer legal.");
        switch (kind)
        {
            case CardKind.Duel:
                BeginJuanxiaVirtualDuel(active, state with { UsedCount = state.UsedCount + 1, UsedKinds = AppendJuanxiaKind(state.UsedKinds, kind), Stage = JuanxiaStage.Child });
                return;
            case CardKind.Snatch or CardKind.Dismantlement:
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { JuanxiaLaunch = state with { PendingKind = kind.ToString(), Stage = JuanxiaStage.Card } });
                PresentJuanxiaTargetCardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), kind);
                return;
            case CardKind.FireAttack:
                BeginJuanxiaFireAttackReveal(active, state);
                return;
            default:
                throw new InvalidOperationException($"The Juanxia trick kind '{(kind)}' is unsupported.");
        }
    }

    internal static IReadOnlyList<string> AppendJuanxiaKind(IReadOnlyList<string> kinds, CardKind kind) => RuntimeSkills.RuntimeRules_yang_yi_ol_YangYiPrograms.AppendJuanxiaKind(kinds, kind);
    internal void BeginJuanxiaVirtualDuel(ProgramSkillFrame frame, JuanxiaLaunchState state)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A Juanxia Duel cannot overwrite a pending card resolution.");
        _coreRuntimeOperations.ReplaceRuntimeTop(active with { JuanxiaLaunch = state });
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(active.Id, active.OwnerSeat, state.TargetSeat, card: null, damageAmount: 1, playedCardKind: CardKind.Duel, programSkillFrameId: active.Id);
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _coreNativeCardContinuationCursorsOperations.ActiveDuel = _kernelCoreMainOperations.CreateDuelHandle(attack);
        _coreCardAttackStateOperations.UpdateCardAttackState(active.Id, state => state!with { ProgramUseInstructionIndex = active.InstructionIndex });
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，视为对 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 使用【决斗】。", active.OwnerSeat, state.TargetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJuanxiaTrickUsedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, state.TargetSeat, CardKind.Duel.ToString(), 0, false));
        _coreNativeDuelOperations.BeginDuelResponse(_coreNativeCardContinuationCursorsOperations.ActiveDuel);
    }

    internal bool IsJuanxiaDuelOrigin(ProgramSkillFrame frame, out int targetSeat)
    {
        targetSeat = -1;
        if (frame.JuanxiaLaunch is not { Stage: JuanxiaStage.Child, PendingKind: null } launch || frame.SelectedTargetSeats is not [var target] || target != launch.TargetSeat || launch.UsedCount != launch.UsedKinds.Count || launch.UsedCount is < 1 or > MaximumJuanxiaTrickCount || launch.UsedKinds.Count(kind => kind == "Duel") != 1 || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } context || frame.TriggerId is null || frame.ActivationId != frame.TriggerId || context.OwnerSeat != frame.OwnerSeat || context.SourceSeat != frame.OwnerSeat || context.TargetSeat != frame.OwnerSeat)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(item => item.Id == frame.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not TurnEndingBoundaryFrame ending || ending.Id != context.ParentFrameId || ending.OwnerSeat != frame.OwnerSeat || ending.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || ending.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || ending.ItemIndex < 0 || ending.ItemIndex >= ending.Items.Count || ending.Items[ending.ItemIndex] is not { Kind: TurnEndingBoundaryItemKind.Program, Candidate: { } candidate } || !CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(frame, candidate))
            return false;
        targetSeat = target;
        return true;
    }

    internal void PresentJuanxiaTargetCardPrompt(ProgramSkillFrame frame, CardKind kind)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaLaunch ?? throw new InvalidOperationException("The Juanxia card choice lost its frame state.");
        var target = _coreMatchParticipantsPlayersAccess._players[state.TargetSeat];
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var cardName = CardCatalog.Get(kind).DisplayName;
        var choices = new List<PromptChoice>();
        void Add(Card? card, string description, string parameter)
        {
            choices.Add(new PromptChoice(new ChoiceId($"juanxia-card.frame-{(frame.Id)}.{(kind)}.{(parameter)}"), description, card is null ? [] : [card.Id], [target.Seat], new Dictionary<string, string> { ["program-action"] = "juanxia-target-card", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["trick"] = kind.ToString(), ["card-id"] = card?.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) ?? string.Empty, ["card-ref"] = parameter }));
        }

        if (_corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > 0)
            Add(null, $"随机获得 {(target.Name)} 的一张手牌。", "hand");
        foreach (var equipment in _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).OrderBy(card => card.Id))
            Add(equipment, $"选择 {(target.Name)} 装备区的【{(equipment.DisplayName)}】。", $"card-{(equipment.Id)}");
        foreach (var judgment in _corePhysicalOwnedCardQueriesOperations.GetJudgment(target).OrderBy(card => card.Id))
            Add(judgment, $"选择 {(target.Name)} 判定区的【{(judgment.DisplayName)}】。", $"card-{(judgment.Id)}");
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择【{(cardName)}】的目标牌。", choices.SelectMany(choice => choice.Cards).Distinct().Order().ToArray(), [target.Seat], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择目标牌", $"视为对 {(target.Name)} 使用【{(cardName)}】。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("juanxia-target-card")]
    internal void ResolveJuanxiaTargetCardChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Juanxia card choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.JuanxiaDeclareTricks } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Juanxia card choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat || active.JuanxiaLaunch is not { } state || state.Stage != JuanxiaStage.Card || !Enum.TryParse<CardKind>(state.PendingKind, out var kind) || kind is not (CardKind.Snatch or CardKind.Dismantlement))
            throw new InvalidOperationException("The Juanxia card chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[state.TargetSeat];
        var cardReference = selected.Parameters.GetValueOrDefault("card-ref");
        Card card;
        CardLocation from;
        if (cardReference == "hand")
        {
            var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).ToArray();
            if (hand.Length == 0)
            {
                CancelJuanxiaTrick(active, state, kind, "狷狭：目标已没有手牌，这张牌的结算取消。");
                return;
            }

            card = hand[_randomOperations._random.Next(hand.Length)];
            from = CardLocation.Hand(target.Seat);
        }
        else
        {
            if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId))
                throw new InvalidOperationException("The Juanxia card choice lost its selected card.");
            card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(cardId)).Single(candidate => candidate.Id == cardId);
            from = _cardZonesOperations._cardZones.GetLocation(cardId);
            if (from.OwnerSeat != target.Seat || from.Zone is not (CardZoneKind.Equipment or CardZoneKind.Judgment))
            {
                CancelJuanxiaTrick(active, state, kind, "狷狭：目标牌已离开原区域，这张牌的结算取消。");
                return;
            }
        }

        var destination = kind == CardKind.Snatch ? CardLocation.Hand(owner.Seat) : CardLocation.DiscardPile;
        var logicalDiscard = kind == CardKind.Dismantlement ? _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(active.OwnerSeat, active.OwnerSeat, [card.Id], new($"skill-program.{(active.SkillId)}.dismantle"), active.Id) : null;
        _corePhysicalMovementFacadesOperations.MoveCard(card, from, destination, new($"skill-program.{(active.SkillId)}.{(kind switch {CardKind.Snatch => "snatch", _ => "dismantle"})}"));
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，视为对 {(target.Name)} 使用【{(CardCatalog.Get(kind).DisplayName)}】。", active.OwnerSeat, target.Seat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJuanxiaTrickUsedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, target.Seat, kind.ToString(), card.Id, false));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { JuanxiaLaunch = state with { UsedCount = state.UsedCount + 1, UsedKinds = AppendJuanxiaKind(state.UsedKinds, kind), Stage = JuanxiaStage.Child, PendingKind = null } });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal void BeginJuanxiaFireAttackReveal(ProgramSkillFrame frame, JuanxiaLaunchState state)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var target = _coreMatchParticipantsPlayersAccess._players[state.TargetSeat];
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).ToArray();
        if (hand.Length == 0)
        {
            CancelJuanxiaTrick(active, state, CardKind.FireAttack, "狷狭：目标已没有手牌，这张牌的结算取消。");
            return;
        }

        var revealed = hand[_randomOperations._random.Next(hand.Length)];
        _coreMatchLogOperations.AddLog("CardEffect", $"狷狭：{(target.Name)} 的手牌【{(revealed.DisplayName)}】被展示（视为使用【火攻】）。", active.OwnerSeat, target.Seat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { JuanxiaLaunch = state with { PendingKind = CardKind.FireAttack.ToString(), RevealedCardId = revealed.Id, Stage = JuanxiaStage.Fire } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(active.Id, [_coreSnapshotValueProjectionOperations.ToSnapshot(revealed)]));
        if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(active.Id, "juanxia-fire-shown-hand"))
            return;
        PresentJuanxiaFirePaymentPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), revealed);
    }

    internal void PresentJuanxiaFirePaymentPrompt(ProgramSkillFrame frame, Card revealed)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaLaunch ?? throw new InvalidOperationException("The Juanxia fire payment lost its frame state.");
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var revealedIsRed = CoreNativeCardClassificationsRuntime.IsRedSuit(revealed.Suit);
        var costs = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).Where(card => CoreNativeCardClassificationsRuntime.IsRedSuit(card.Suit) == revealedIsRed).OrderBy(card => card.Id).ToArray();
        var choices = costs.Select(card => new PromptChoice(new ChoiceId($"juanxia-fire.frame-{(frame.Id)}.card-{(card.Id)}"), $"弃置【{(card.DisplayName)}】，对 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 造成1点火焰伤害。", [card.Id], [state.TargetSeat], new Dictionary<string, string> { ["program-action"] = "juanxia-fire-payment", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).Append(new PromptChoice(new ChoiceId($"juanxia-fire.frame-{(frame.Id)}.decline"), "放弃伤害。", [], [], new Dictionary<string, string> { ["program-action"] = "juanxia-fire-payment", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = string.Empty })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】火攻：展示的是【{(revealed.DisplayName)}】，请选择弃置一张同颜色牌或放弃伤害。", costs.Select(card => card.Id).ToArray(), [state.TargetSeat], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 火攻", "弃置一张与展示牌颜色相同的手牌或装备牌，令其受到1点火焰伤害。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("juanxia-fire-payment")]
    internal void ResolveJuanxiaFirePaymentChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Juanxia fire payment lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.JuanxiaDeclareTricks } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Juanxia fire payment does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat || active.JuanxiaLaunch is not { } state || state.Stage != JuanxiaStage.Fire || state.RevealedCardId == 0)
            throw new InvalidOperationException("The Juanxia fire payment changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var revealedLocation = _cardZonesOperations._cardZones.GetLocation(state.RevealedCardId);
        var revealedStillShown = revealedLocation.OwnerSeat == state.TargetSeat && revealedLocation.Zone == CardZoneKind.Hand;
        var cardIdParameter = selected.Parameters.GetValueOrDefault("card-id");
        var paid = !string.IsNullOrEmpty(cardIdParameter) && revealedStillShown;
        if (paid)
        {
            if (!int.TryParse(cardIdParameter, System.Globalization.CultureInfo.InvariantCulture, out var costId))
                throw new InvalidOperationException("The Juanxia fire payment lost its selected cost.");
            var costFrom = _cardZonesOperations._cardZones.GetLocation(costId);
            var cost = _cardZonesOperations._cardZones.CardsAt(costFrom).SingleOrDefault(card => card.Id == costId);
            var revealedCard = _cardZonesOperations._cardZones.CardsAt(revealedLocation).SingleOrDefault(card => card.Id == state.RevealedCardId) ?? throw new InvalidOperationException("The Juanxia fire reveal left its shown hand card.");
            if (cost is null || costFrom.OwnerSeat != active.OwnerSeat || costFrom.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || CoreNativeCardClassificationsRuntime.IsRedSuit(cost.Suit) != CoreNativeCardClassificationsRuntime.IsRedSuit(revealedCard.Suit))
                throw new InvalidOperationException("The Juanxia fire payment is no longer a legal same-color card.");
            _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(active.OwnerSeat, active.OwnerSeat, cost, costFrom, CardMoveReasons.FireAttackDiscard);
        }

        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，视为对 {(_coreMatchParticipantsPlayersAccess._players[state.TargetSeat].Name)} 使用【火攻】{((paid ? "，造成1点火焰伤害。" : "，未造成伤害。"))}", active.OwnerSeat, state.TargetSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJuanxiaTrickUsedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, state.TargetSeat, CardKind.FireAttack.ToString(), state.RevealedCardId, paid));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { JuanxiaLaunch = state with { UsedCount = state.UsedCount + 1, UsedKinds = AppendJuanxiaKind(state.UsedKinds, CardKind.FireAttack), Stage = JuanxiaStage.Child, PendingKind = null, RevealedCardId = 0 } });
        if (!paid)
        {
            PresentJuanxiaTrickPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
            return;
        }

        _coreSkillProgramsOperations.BeginProgramSkillDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), state.TargetSeat, 1, nature: DamageNature.Fire);
    }

    internal void CancelJuanxiaTrick(ProgramSkillFrame frame, JuanxiaLaunchState state, CardKind kind, string reason)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreMatchLogOperations.AddLog("SkillEffect", reason, active.OwnerSeat, state.TargetSeat);
        PresentJuanxiaTrickPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
    }

    internal SkillProgramStepOutcome JuanxiaProgramRetaliation(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.WindowContext is not { SourceSeat: { } turnOwner } || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(turnOwner) || turnOwner == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[turnOwner].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat], active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var debts = OutstandingJuanxiaDebts(active.OwnerSeat, turnOwner).ToArray();
        if (debts.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var total = debts.Sum(debt => debt.Count);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { JuanxiaRetaliate = new JuanxiaRetaliationState(turnOwner, total, Array.AsReadOnly(debts.Select(debt => debt.LaunchFrameId).ToArray()), Array.AsReadOnly(debts.Select(debt => debt.Count).ToArray()), JuanxiaStage.Retaliate) });
        return PresentJuanxiaRetaliationPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    internal IReadOnlyList<(long LaunchFrameId, int Count)> OutstandingJuanxiaDebts(int ownerSeat, int debtorSeat)
    {
        var settled = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramJuanxiaRetaliationEvent>().Select(e => e.LaunchFrameId).ToHashSet();
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramJuanxiaDebtEvent>().Where(debt => debt.OwnerSeat == ownerSeat && debt.TargetSeat == debtorSeat && !settled.Contains(debt.FrameId)).Select(debt => (debt.FrameId, debt.TrickCount)).ToArray();
    }

    internal SkillProgramStepOutcome PresentJuanxiaRetaliationPrompt(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaRetaliate ?? throw new InvalidOperationException("The Juanxia retaliation lost its frame state.");
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var debtor = _coreMatchParticipantsPlayersAccess._players[state.DebtorSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !debtor.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
        {
            FinishJuanxiaRetaliation(active);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = new[]
        {
            new PromptChoice(new ChoiceId($"juanxia-retaliate.frame-{(frame.Id)}.use"), $"视为对 {(owner.Name)} 使用 {(state.Remaining)} 张【杀】。", [], [active.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "juanxia-retaliate", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["answer"] = "use" }),
            new PromptChoice(new ChoiceId($"juanxia-retaliate.frame-{(frame.Id)}.decline"), "不使用【杀】。", [], [], new Dictionary<string, string> { ["program-action"] = "juanxia-retaliate", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["answer"] = "decline" })
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, state.DebtorSeat, $"【{(presentation.Name)}】你可以视为对 {(owner.Name)} 使用 {(state.Remaining)} 张【杀】。", [], [active.OwnerSeat], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = state.DebtorSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 报复", $"{(owner.Name)} 曾对你发动狷狭，你可以视为对其使用等量张【杀】。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = debtor.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("juanxia-retaliate")]
    internal void ResolveJuanxiaRetaliateChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Juanxia retaliation choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.JuanxiaRetaliation } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Juanxia retaliation choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || active.JuanxiaRetaliate is not { } state || state.Stage != JuanxiaStage.Retaliate || decision.PlayerSeat != state.DebtorSeat)
            throw new InvalidOperationException("The Juanxia retaliation chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters.GetValueOrDefault("answer") != "use")
        {
            FinishJuanxiaRetaliation(active);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { JuanxiaRetaliate = state with { Stage = JuanxiaStage.Slash } });
        BeginJuanxiaRetaliationSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
    }

    internal bool CanUseJuanxiaRetaliationSlash(int actorSeat, int targetSeat) => _coreMatchParticipantsPlayersAccess._players[actorSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive && actorSeat != targetSeat && !_coreCardUseModulesOperations.HasTurnCardTargetRestriction(actorSeat, SkillProgramCardTargetRestriction.SelfOnly) && !_coreCardUseModulesOperations.IsCardUseForbidden(actorSeat, CardKind.Slash, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actorSeat, targetSeat, CardKind.Slash) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat], CardKind.Slash, Suit.None, null) && !_coreDeckEquipmentProgramsOperations.HasBeneficiarySuitShield(actorSeat, targetSeat, Suit.None) && !_coreNativeDamagePoliciesOperations.IsSlashProhibited(_coreMatchParticipantsPlayersAccess._players[targetSeat]) && _coreRuleQueriesOperations.CanSpendSlashUse(_coreMatchParticipantsPlayersAccess._players[actorSeat], _coreMatchParticipantsPlayersAccess._players[targetSeat], ignoresCount: true);
    internal void BeginJuanxiaRetaliationSlash(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaRetaliate ?? throw new InvalidOperationException("The Juanxia retaliation lost its frame state.");
        var debtorSeat = state.DebtorSeat;
        var ownerSeat = active.OwnerSeat;
        if (state.Remaining <= 0 || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[debtorSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive || !CanUseJuanxiaRetaliationSlash(debtorSeat, ownerSeat))
        {
            FinishJuanxiaRetaliation(active);
            return;
        }

        if (_coreActiveNativeAttackQueriesOperations.ActiveCardAttack is not null || _coreNativeCardContinuationCursorsOperations.ActiveDuel is not null)
            throw new InvalidOperationException("A Juanxia Slash cannot overwrite a pending card resolution.");
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { JuanxiaRetaliate = state with { Remaining = state.Remaining - 1 } });
        var debtor = _coreMatchParticipantsPlayersAccess._players[debtorSeat];
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var resolutionId = ++_coreResolutionIdentityBoundaryOperations._resolutionSequence;
        var action = _coreProgramFactionActionRewardOperations.CaptureFactionAction(new CardActionContext(++_coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence, _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault()?.Action?.ActionId, CardActionType.Use, debtorSeat, debtorSeat, null, null, null, CardKind.Slash, [ownerSeat], [], [], effectiveSuit: Suit.None, effectiveRank: 0));
        _coreRuntimeOperations.PushRuntimeFrame(new CardUseFrame(resolutionId, debtorSeat, 0, CardKind.Slash, [ownerSeat], PhysicalCardIds: []) { Action = action });
        if (_corePlayCardHistoryOperations.TracksPlayCardHistory)
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseAppearanceCapturedEvent(action));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUseDeclaredEvent(resolutionId, 0, CardKind.Slash, debtorSeat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TargetsConfirmedEvent(resolutionId, [ownerSeat]));
        var attack = _kernelCoreMainOperations.CreateCardAttackHandle(resolutionId, debtorSeat, ownerSeat, card: null, damageAmount: 1 + debtor.AlcoholDamageBonus, playedCardKind: CardKind.Slash, ignoresArmor: _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(debtor, owner, CardKind.Slash), programSkillCardUseFrameId: active.Id);
        _coreAlcoholSlashSuppressionsOperations.Value.CaptureProgramAlcoholConsumption(resolutionId, debtor);
        debtor.HasAlcoholEffect = false;
        _coreActiveNativeAttackQueriesOperations.ActiveCardAttack = attack;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(0, CardKind.Slash, debtorSeat, ownerSeat));
        _coreCardUseLifecycleOperations.TryMarkProgramUseCommitted(resolutionId);
        _coreMatchLogOperations.AddLog("CardUsed", $"{(debtor.Name)} 狷狭：视为对 {(owner.Name)} 使用【杀】。", debtorSeat, ownerSeat);
        if (!_coreCardActionsOperations.TryBeginProgramCardWindow(attack, action, SkillProgramTriggerWindow.CardUseCommitted, action.TargetSeats, ProgramCardContinuation.CommittedSlash))
            _coreNativeSlashUseOperations.BeginSlashTargetResolution(attack);
    }

    internal void FinishJuanxiaRetaliation(ProgramSkillFrame frame)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var state = active.JuanxiaRetaliate;
        if (state is null)
            return;
        var offeredTotal = state.LaunchCounts.Sum();
        var usedTotal = Math.Max(0, offeredTotal - Math.Max(0, state.Remaining));
        var remainingUsed = usedTotal;
        for (var index = 0; index < state.LaunchFrameIds.Count; index++)
        {
            var offered = state.LaunchCounts[index];
            var used = Math.Min(offered, Math.Max(0, remainingUsed));
            remainingUsed -= used;
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramJuanxiaRetaliationEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, state.DebtorSeat, state.LaunchFrameIds[index], offered, used));
        }

        if (offeredTotal > 0)
            _coreMatchLogOperations.AddLog("SkillEffect", $"狷狭报复结算：{(_coreMatchParticipantsPlayersAccess._players[state.DebtorSeat].Name)} 视为对 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 使用了 {(usedTotal)}/{(offeredTotal)} 张【杀】。", state.DebtorSeat, active.OwnerSeat);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { JuanxiaRetaliate = null });
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    // Program resume hook: a returned child (duel, program damage or a closed
    // cards-moved window) hands control back to the paused Juanxia instruction.
    [ProgramContinuation(ProgramContinuationKind.Resume, 570)]
    internal bool ResumeJuanxia(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.Id != frameId)
            return false;
        if (frame.JuanxiaRetaliate is { Stage: JuanxiaStage.Slash } retaliation)
        {
            if (retaliation.Remaining > 0)
                BeginJuanxiaRetaliationSlash(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId));
            else
                FinishJuanxiaRetaliation(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId));
            return true;
        }

        if (frame.JuanxiaLaunch is not { } launch)
            return false;
        switch (launch.Stage)
        {
            case JuanxiaStage.Child:
                PresentJuanxiaTrickPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId));
                return true;
            case JuanxiaStage.Card when Enum.TryParse<CardKind>(launch.PendingKind, out var kind) && kind is CardKind.Snatch or CardKind.Dismantlement:
                PresentJuanxiaTargetCardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId), kind);
                return true;
            case JuanxiaStage.Fire when launch.RevealedCardId > 0:
                PresentJuanxiaFirePaymentPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frameId), _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(launch.RevealedCardId)).Single(card => card.Id == launch.RevealedCardId));
                return true;
            default:
                return false;
        }
    }

    SkillProgramStepOutcome IYangYiProgramHost.JuanxiaDeclareTricks(ProgramSkillFrame frame, SkillProgramEffect effect) => JuanxiaProgramDeclareTricks(frame, effect);
    SkillProgramStepOutcome IYangYiProgramHost.JuanxiaRetaliation(ProgramSkillFrame frame, SkillProgramEffect effect) => JuanxiaProgramRetaliation(frame, effect);
    [PublicHandRevealReturn("juanxia-fire-shown-hand")]
    internal void ReturnJuanxiaPublicHand(ProgramSkillFrame frame)
    {
        if (frame.JuanxiaLaunch is not { Stage: JuanxiaStage.Fire, RevealedCardId: > 0 } state)
            throw new InvalidOperationException("Juanxia lost its original FireAttack Hand reveal.");
        if (_cardZonesOperations._cardZones.GetLocation(state.RevealedCardId) != CardLocation.Hand(state.TargetSeat))
        {
            CancelJuanxiaTrick(frame, state, CardKind.FireAttack, "狷狭：展示牌已离开手牌，取消此次火攻。");
            return;
        }

        PresentJuanxiaFirePaymentPrompt(frame, _coreAdvancedLifecycleOperations.GetAdvancedCard(state.RevealedCardId));
    }
}
