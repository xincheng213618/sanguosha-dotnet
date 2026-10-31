#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTangJiOlRuntime
{
    internal bool IsKanggeMarkedSeat(int seat, int ownerSeat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Kangge, ownerSeat)) > 0;
    internal int? GetKanggeMarkedSeat(int ownerSeat) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.MarkerSourceCounts.GetValueOrDefault((PlayerMarkerKind.Kangge, ownerSeat)) > 0).Select(player => (int? )player.Seat).FirstOrDefault();
    internal SkillProgramStepOutcome KanggeProgramChooseTarget(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || GetKanggeMarkedSeat(owner.Seat)is not null)
            return SkillProgramStepOutcome.Continue;
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).Select(player => player.Seat).ToArray();
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(seat => new PromptChoice(new ChoiceId($"kangge-choose.frame-{(frame.Id)}.seat-{(seat)}"), $"选择 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)}。", [], [seat], new Dictionary<string, string> { ["program-action"] = "kangge-choose", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["chosen"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择一名其他角色作为“抗歌”对象。", [], candidates, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择“抗歌”对象", "本局中：其于其回合外获得手牌时你摸等量的牌（每回合至多三张）；每轮限一次，其进入濒死状态时你可以令其回复体力至1点；其死亡时你弃置所有牌并失去1点体力。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("kangge-choose")]
    internal void ResolveKanggeChooseChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Kangge choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.KanggeChooseTarget } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Kangge choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat || !int.TryParse(selected.Parameters.GetValueOrDefault("chosen"), System.Globalization.CultureInfo.InvariantCulture, out var chosen) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(chosen) || chosen == active.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[chosen].IsAlive)
            throw new InvalidOperationException("The Kangge choice lost its chooser or candidate.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreParticipantAndReserveProgramsOperations.SetAttributedNatureMarker(active.OwnerSeat, chosen, PlayerMarkerKind.Kangge, 1, active.Id);
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】：选择 {(_coreMatchParticipantsPlayersAccess._players[chosen].Name)} 作为“抗歌”对象。", active.OwnerSeat, chosen);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramKanggeChosenEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, chosen));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome KanggeProgramGainDraw(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardsGained, TargetSeat: { } gainer } || !IsKanggeMarkedSeat(gainer, owner.Seat) || gainer == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return SkillProgramStepOutcome.Continue;
        var drawnThisTurn = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramKanggeGainedDrawEvent>().Where(item => item.OwnerSeat == owner.Seat && item.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber).Sum(item => item.DrawnCount);
        var drawn = Math.Min(1, Math.Max(0, ProgramKanggeTurnDrawCap - drawnThisTurn));
        if (drawn == 0)
            return SkillProgramStepOutcome.Continue;
        _coreProgramLifecycleOperations.DrawProgramCards(active.Id, owner.Seat, drawn, null, null, SkillProgramCardSetVisibility.Private, new($"skill-program.{(active.SkillId)}.kangge-gain-draw"));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】:{(_coreMatchParticipantsPlayersAccess._players[gainer].Name)} 于回合外获得手牌，{(owner.Name)} 摸 {(drawn)} 张牌。", active.OwnerSeat, gainer);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramKanggeGainedDrawEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, gainer, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, drawn));
        return SkillProgramStepOutcome.Continue;
    }

    internal bool KanggeHealUsedThisRound(int ownerSeat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramKanggeHealedEvent>().Any(item => item.OwnerSeat == ownerSeat && item.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber);
    internal SkillProgramStepOutcome KanggeProgramHealVictim(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.DyingEntering, TargetSeat: { } victim } || victim == owner.Seat || !IsKanggeMarkedSeat(victim, owner.Seat) || _coreMatchParticipantsPlayersAccess._players[victim].Hp > 0 || KanggeHealUsedThisRound(owner.Seat))
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = new[]
        {
            new PromptChoice(new ChoiceId($"kangge-heal.frame-{(frame.Id)}.heal"), $"令 {(_coreMatchParticipantsPlayersAccess._players[victim].Name)} 回复体力至1点。", [], [victim], new Dictionary<string, string> { ["program-action"] = "kangge-heal", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }),
            new PromptChoice(new ChoiceId($"kangge-heal.frame-{(frame.Id)}.decline"), "不发动【抗歌】。", [], [], new Dictionary<string, string> { ["program-action"] = "kangge-heal", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["decline"] = "true" })
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】是否令 {(_coreMatchParticipantsPlayersAccess._players[victim].Name)} 回复体力至1点？（每轮限一次）", [], [victim], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 濒死回复", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("kangge-heal")]
    internal void ResolveKanggeHealChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Kangge heal choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.KanggeHealVictim } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Kangge heal choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Kangge heal choice lost its chooser.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters.GetValueOrDefault("decline") == "true")
        {
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 未发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】的濒死回复。", active.OwnerSeat);
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        if (frame.WindowContext is not { TargetSeat: { } victim } || !IsKanggeMarkedSeat(victim, owner.Seat) || owner.Seat == victim || KanggeHealUsedThisRound(owner.Seat) || _coreMatchParticipantsPlayersAccess._players[victim].Hp > 0)
        {
            // The victim left the dying window between prompt and answer; there
            // is nothing left to rescue, so the trigger settles without effect.
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        _coreBoundRankBonusProgramsOperations.RecoverOtherDyingVictim(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), 1);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame resumed || resumed.Id != frame.Id)
            return;
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】：令 {(_coreMatchParticipantsPlayersAccess._players[victim].Name)} 回复体力至1点。", active.OwnerSeat, victim);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramKanggeHealedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, victim, _coreActualRoundProgressRoundNumberAccess._roundNumber));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome KanggeProgramDeathPrice(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.CharacterDied, TargetSeat: { } victim } || !IsKanggeMarkedSeat(victim, owner.Seat))
            return SkillProgramStepOutcome.Continue;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).ToArray();
        var equipment = _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).ToArray();
        var judgment = _cardZonesOperations._cardZones.CardsAt(CardLocation.Judgment(owner.Seat)).ToArray();
        var reason = new CardMoveReason($"skill-program.{(active.SkillId)}.kangge-death-price");
        var logicalDiscard = _coreNativeLogicalDiscardsOperations.CaptureNativeLogicalDiscardInvocation(active.OwnerSeat, owner.Seat, hand.Concat(equipment).Concat(judgment).Select(card => card.Id).ToArray(), reason, active.Id);
        foreach (var card in hand)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(owner.Seat), CardLocation.DiscardPile, reason);
        foreach (var card in equipment)
            _corePhysicalMovementFacadesOperations.MoveCard(card, _cardZonesOperations._cardZones.GetLocation(card.Id), CardLocation.DiscardPile, reason);
        foreach (var card in judgment)
            _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Judgment(owner.Seat), CardLocation.DiscardPile, reason);
        _coreNativeLogicalDiscardsOperations.CompleteNativeLogicalDiscardInvocation(logicalDiscard);
        var discarded = hand.Length + equipment.Length + judgment.Length;
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】:{(_coreMatchParticipantsPlayersAccess._players[victim].Name)} 死亡，{(owner.Name)} 弃置所有牌（{(discarded)} 张）并失去1点体力。", active.OwnerSeat, victim);
        var before = owner.Hp;
        owner.Hp = Math.Max(0, before - 1);
        _coreHpChangeProgramsOperations.RecordHpChange(active.Id, null, owner.Seat, before, owner.Hp, HpChangeKind.Loss);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramSkillHpLostEvent(active.Id, active.SkillId, owner.Seat, Math.Min(before, 1), owner.Hp));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramKanggeDeathPriceEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), owner.Seat, victim, discarded));
        if (owner.Hp == 0)
        {
            _coreSkillProgramsOperations.BeginProgramSkillDying(active.Id, owner);
            return SkillProgramStepOutcome.AwaitChild;
        }

        return SkillProgramStepOutcome.Continue;
    }

    [ProgramAiChoice(SkillProgramEffectOp.KanggeChooseTarget)]
    internal PromptChoice SelectAiKanggeChooseChoice(PendingDecision decision) => decision.Choices.OrderByDescending(choice => choice.Targets.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Hp).FirstOrDefault()).ThenBy(choice => choice.Targets.FirstOrDefault()).ThenBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 270, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveKanggeMovement(SkillProgramTrigger trigger, ProgramSkillWindowContext context, CharacterState owner) => trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.KanggeGainDraw) ? context.TargetSeat is { } gainerSeat && _coreTableSeatQueriesOperations.IsValidPlayerSeat(gainerSeat) && gainerSeat != owner.Seat && _coreMatchParticipantsPlayersAccess._players[gainerSeat].IsAlive && gainerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && IsKanggeMarkedSeat(gainerSeat, owner.Seat) : null;
}
