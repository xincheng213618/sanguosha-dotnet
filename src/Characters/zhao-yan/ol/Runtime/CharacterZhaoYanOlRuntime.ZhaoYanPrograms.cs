#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoYanOlRuntime
{
    internal ProgramTongxieArmedEvent? ActiveTongxieArm(int ownerSeat)
    {
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Reverse())
        {
            switch (fact)
            {
                case ProgramTongxieArmedEvent armed when armed.OwnerSeat == ownerSeat:
                    return armed;
                case TurnStartedEvent turn when turn.ActorSeat == ownerSeat:
                    return null;
            }
        }

        return null;
    }

    internal bool TongxieLostHpThisTurn(int seat)
    {
        foreach (var fact in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Reverse())
        {
            switch (fact)
            {
                case ProgramSkillHpLostEvent lost when lost.TargetSeat == seat:
                    return true;
                case TurnStartedEvent:
                    return false;
            }
        }

        return false;
    }

    internal static bool IsTongxieMember(ProgramTongxieArmedEvent arm, int seat) => RuntimeSkills.RuntimeRules_zhao_yan_ol_ZhaoYanPrograms.IsTongxieMember(arm, seat);
    internal IEnumerable<int> TongxieFollowUpResponders(IReadOnlyList<int> members, int actorSeat, int targetSeat)
    {
        return members.Where(seat => seat != actorSeat && seat != targetSeat).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Where(seat => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Any(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind))).OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(actorSeat, seat)).ThenBy(seat => seat);
    }

    internal IEnumerable<int> TongxieEligiblePreventers(IReadOnlyList<int> members, int protectedSeat)
    {
        return members.Where(seat => seat != protectedSeat).Where(seat => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive).Where(seat => !TongxieLostHpThisTurn(seat)).OrderBy(seat => _coreTableOrderOperations.RelativeTableOrder(protectedSeat, seat)).ThenBy(seat => seat);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 150, SkillProgramEffectOp.TongxieArm)]
    internal bool CanRunTongxieArm(int ownerSeat)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        return _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != ownerSeat);
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 160, SkillProgramEffectOp.TongxieFollowUp)]
    internal bool CanRunTongxieFollowUp(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        if (context is not { MovementBatch: { } batch, MovementIndex: { } movementIndex } || FindTongxieChainUse(batch, movementIndex)is not { } use)
            return false;
        if (IsTongxieProducedUse(use.ActionId))
            return false;
        var arm = ActiveTongxieArm(ownerSeat);
        if (arm is null || !IsTongxieMember(arm, use.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[use.ActorSeat].IsAlive)
            return false;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[use.TargetSeat].IsAlive)
            return false;
        return TongxieFollowUpResponders(arm.MemberSeats, use.ActorSeat, use.TargetSeat).Any();
    }

    internal bool IsTongxieProducedUse(long actionId) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TongxieSettledSlashUseEvent>().Any(use => use.ActionId == actionId && use.ParentProgramFrameId is not null);
    internal void CaptureTongxieSettledSlashUse(long batchId, CardMovementRecord movement)
    {
        if (_contentRegistryOperations._contentRegistry?.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.TongxieFollowUp) != true || movement.Reason != CardMoveReasons.UseFinished || movement.From != CardLocation.Processing || movement.To != CardLocation.DiscardPile)
            return;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().LastOrDefault(frame => frame.Action is { Type: CardActionType.Use } action && action.PhysicalCards is [var cost] && cost.CardId == movement.CardId);
        if (use?.Action is not { } accepted || !CoreNativeCardClassificationsRuntime.IsSlashCard(accepted.EffectiveKind) || use.TargetSeats is not [var target] || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target))
            return;
        if (!_coreMatchParticipantsPlayersAccess._players.Any(owner => owner.IsAlive && ActiveTongxieArm(owner.Seat)is { } arm && IsTongxieMember(arm, accepted.ActorSeat)))
            return;
        long? parentId = null;
        if (use.CardAttack?.ProgramSkillCardUseFrameId is { } candidateId && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(frame => frame.Id == candidateId)is { ReexecuteParticipantInstruction: true, TongxieFollowUp: { } pending } parent && pending.UsedCardIds.LastOrDefault() == movement.CardId && pending.UsedBy.LastOrDefault() == accepted.ProviderSeat && accepted.PhysicalCards[0].From == CardLocation.Hand(accepted.ProviderSeat) && _contentRegistryOperations._contentRegistry.GetSkill(parent.SkillId).Program is { } program && program.GameplayHash == parent.GameplayHash && parent.InstructionIndex > 0 && ProgramInstructionResolver.Default.Resolve(parent, program).GetPausedInstruction(parent.InstructionIndex).Effect.Op == SkillProgramEffectOp.TongxieFollowUp)
            parentId = parent.Id;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new TongxieSettledSlashUseEvent(use.Id, accepted.ActionId, movement.CardId, accepted.ActorSeat, target, accepted.EffectiveKind, batchId, movement.Sequence, parentId));
    }

    internal TongxieChainUse? FindTongxieChainUse(CardMovementBatchContext batch, int movementIndex)
    {
        if (movementIndex < 0 || movementIndex >= batch.Movements.Count)
            return null;
        var movement = batch.Movements[movementIndex];
        var settled = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TongxieSettledSlashUseEvent>().SingleOrDefault(entry => entry.BatchId == batch.Id && entry.MovementSequence == movement.Sequence && entry.CardId == movement.CardId);
        return settled is null ? null : new TongxieChainUse(settled.ActorSeat, settled.TargetSeat, settled.CardId, settled.ActionId);
    }

    internal int[] MatchingTongxieSettledSlashIndexes(CardMovementBatchContext batch)
    {
        if (!_characterYanJunOlOperations.TracksSettledActionCards)
            return[];
        var settled = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TongxieSettledSlashUseEvent>().Where(entry => entry.BatchId == batch.Id).Select(entry => entry.MovementSequence).ToHashSet();
        return batch.Movements.Select((movement, index) => (movement, index)).Where(item => settled.Contains(item.movement.Sequence) && item.movement.To == CardLocation.DiscardPile && _cardZonesOperations._cardZones.GetLocation(item.movement.CardId) == CardLocation.DiscardPile).Select(item => item.index).Order().ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 170, SkillProgramEffectOp.TongxieGuard)]
    internal bool CanRunTongxieGuard(int ownerSeat, ProgramSkillWindowContext context)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        if (context.Window != SkillProgramTriggerWindow.BeforeDamageApplied || context.TargetSeat is not { } victim || context.Amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive)
            return false;
        var arm = ActiveTongxieArm(ownerSeat);
        if (arm is null || !IsTongxieMember(arm, victim))
            return false;
        return TongxieEligiblePreventers(arm.MemberSeats, victim).Any();
    }

    internal SkillProgramStepOutcome TongxieProgramArm(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } || active.OwnerSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            return SkillProgramStepOutcome.Continue;
        var others = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != owner.Seat).Select(player => player.Seat).Order().ToArray();
        if (others.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = new List<PromptChoice>
        {
            new(new ChoiceId($"tongxie-arm.frame-{(frame.Id)}.self"), "仅自己称为“同协”角色。", [], [owner.Seat], new Dictionary<string, string> { ["program-action"] = "tongxie-arm", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["seats"] = "" })
        };
        foreach (var seat in others)
            choices.Add(new PromptChoice(new ChoiceId($"tongxie-arm.frame-{(frame.Id)}.single-{(seat)}"), $"与 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 称为“同协”角色。", [], [seat], new Dictionary<string, string> { ["program-action"] = "tongxie-arm", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["seats"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        for (var first = 0; first < others.Length; first++)
            for (var second = first + 1; second < others.Length; second++)
                choices.Add(new PromptChoice(new ChoiceId($"tongxie-arm.frame-{(frame.Id)}.pair-{(others[first])}-{(others[second])}"), $"与 {(_coreMatchParticipantsPlayersAccess._players[others[first]].Name)}、{(_coreMatchParticipantsPlayersAccess._players[others[second]].Name)} 称为“同协”角色。", [], [others[first], others[second]], new Dictionary<string, string> { ["program-action"] = "tongxie-arm", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["seats"] = $"{(others[first])},{(others[second])}" }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择至多两名其他角色，与你一起称为“同协”角色（直到你的下回合开始）。", [], choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择同协角色", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("tongxie-arm")]
    internal void ResolveTongxieArmChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Tongxie arm choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.TongxieArm } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Tongxie arm choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Tongxie arm choice lost its chooser.");
        var chosen = ParseTongxieArmSeats(selected.Parameters.GetValueOrDefault("seats"));
        if (chosen.Length > 2 || chosen.Distinct().Count() != chosen.Length || chosen.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive || seat == owner.Seat))
            throw new InvalidOperationException("The Tongxie arm choice names an invalid member set.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var members = new[]
        {
            owner.Seat
        }.Concat(chosen).Order().ToArray();
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】：" + $"{(DescribeTongxieMembers(members))}称为“同协”角色（直到{(owner.Name)}的下回合开始）。", active.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTongxieArmedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, Array.AsReadOnly(members)));
        DrawForUniqueLeastTongxieHand(active, members);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal static int[] ParseTongxieArmSeats(string? seats) => RuntimeSkills.RuntimeRules_zhao_yan_ol_ZhaoYanPrograms.ParseTongxieArmSeats(seats);
    internal string DescribeTongxieMembers(IReadOnlyList<int> members) => string.Join("、", members.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name));
    internal void DrawForUniqueLeastTongxieHand(ProgramSkillFrame active, IReadOnlyList<int> members)
    {
        var counts = members.Select(seat => (Seat: seat, Count: _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Count)).ToArray();
        var minimum = counts.Min(entry => entry.Count);
        var least = counts.Where(entry => entry.Count == minimum).ToArray();
        var drawer = least.Length == 1 ? _coreMatchParticipantsPlayersAccess._players[least[0].Seat] : null;
        if (drawer is null)
        {
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】：没有手牌唯一最少的“同协”角色，无人摸牌。", active.OwnerSeat);
            return;
        }

        _corePhysicalDrawFacadesOperations.DrawCards(drawer, 1, true, new($"skill-program.{(active.SkillId)}.tongxie-draw"));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】：{(drawer.Name)} 手牌唯一最少，摸一张牌。", active.OwnerSeat, drawer.Seat);
    }

    internal SkillProgramStepOutcome TongxieProgramFollowUp(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (frame.TongxieFollowUp is { } pending)
            return ContinueTongxieFollowUp(active, pending);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.DiscardPileReceived, MovementBatch: { } batch, MovementIndex: { } movementIndex } || FindTongxieChainUse(batch, movementIndex)is not { } use || IsTongxieProducedUse(use.ActionId))
            return SkillProgramStepOutcome.Continue;
        // A second binding for the same batch card never reopens the chain.
        if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(other => other.Id != active.Id && other.TongxieFollowUp is { } running && running.TriggerActionId == use.ActionId))
            return SkillProgramStepOutcome.Continue;
        var arm = ActiveTongxieArm(active.OwnerSeat);
        if (arm is null || !IsTongxieMember(arm, use.ActorSeat) || !_coreMatchParticipantsPlayersAccess._players[use.ActorSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(use.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[use.TargetSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var responders = TongxieFollowUpResponders(arm.MemberSeats, use.ActorSeat, use.TargetSeat).ToArray();
        if (responders.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { TongxieFollowUp = new TongxieFollowUpState(use.ActorSeat, use.TargetSeat, use.CardId, responders) { TriggerActionId = use.ActionId } });
        return PresentTongxieFollowUpPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    internal SkillProgramStepOutcome ContinueTongxieFollowUp(ProgramSkillFrame active, TongxieFollowUpState pending)
    {
        if (pending.PendingResponders.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { TongxieFollowUp = null });
            var segments = new[]
            {
                DescribeTongxieActors("使用【杀】", pending.UsedBy),
                DescribeTongxieActors("放弃追杀", pending.DeclinedBy)
            }.Where(segment => segment.Length > 0).ToArray();
            _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】追杀结算完成" + (segments.Length > 0 ? $"：{(string.Join("，", segments))}。" : "。"), active.OwnerSeat, pending.TargetSeat);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTongxieFollowUpResolvedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, pending.ActorSeat, pending.TargetSeat, Array.AsReadOnly(pending.UsedBy.ToArray()), Array.AsReadOnly(pending.DeclinedBy.ToArray()), Array.AsReadOnly(pending.UsedCardIds.ToArray())));
            return SkillProgramStepOutcome.Continue;
        }

        return PresentTongxieFollowUpPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
    }

    internal string DescribeTongxieActors(string verb, IReadOnlyList<int> seats) => seats.Count == 0 ? "" : $"{(verb)}：{(string.Join("、", seats.Select(seat => _coreMatchParticipantsPlayersAccess._players[seat].Name)))}";
    internal SkillProgramStepOutcome PresentTongxieFollowUpPrompt(ProgramSkillFrame frame)
    {
        var pending = frame.TongxieFollowUp ?? throw new InvalidOperationException("The Tongxie follow-up lost its pending state.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var responderSeat = pending.PendingResponders[0];
        var responder = _coreMatchParticipantsPlayersAccess._players[responderSeat];
        var target = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat];
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var slashes = _corePhysicalOwnedCardQueriesOperations.GetHand(responder).Where(card => CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind)).OrderBy(card => card.Id).ToArray();
        if (slashes.Length == 0)
        {
            // The candidate list was frozen at window start; a lost slash skips
            // this responder without a prompt.
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { TongxieFollowUp = pending with { PendingResponders = pending.PendingResponders.Skip(1).ToArray() } });
            return ContinueTongxieFollowUp(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).TongxieFollowUp!);
        }

        var choices = slashes.Select(card => new PromptChoice(new ChoiceId($"tongxie-follow-up.frame-{(frame.Id)}.card-{(card.Id)}"), $"对 {(target.Name)} 使用【{(card.DisplayName)}】（无距离限制）。", [card.Id], [pending.TargetSeat], new Dictionary<string, string> { ["program-action"] = "tongxie-follow-up", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList();
        choices.Add(new PromptChoice(new ChoiceId($"tongxie-follow-up.frame-{(frame.Id)}.decline"), $"不对 {(target.Name)} 使用【杀】。", [], [], new Dictionary<string, string> { ["program-action"] = "tongxie-follow-up-decline", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, responderSeat, $"【{(presentation.Name)}】你可以对 {(target.Name)} 使用一张无距离限制的【杀】。", slashes.Select(card => card.Id).ToArray(), [pending.TargetSeat], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = responderSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 追杀", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = responder.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("tongxie-follow-up", "tongxie-follow-up-decline")]
    internal void ResolveTongxieFollowUpChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Tongxie follow-up choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.TongxieFollowUp } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Tongxie follow-up choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var pending = active.TongxieFollowUp ?? throw new InvalidOperationException("The Tongxie follow-up lost its pending state.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != pending.PendingResponders[0])
            throw new InvalidOperationException("The Tongxie follow-up responder changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var responderSeat = pending.PendingResponders[0];
        if (selected.Parameters.GetValueOrDefault("program-action") == "tongxie-follow-up-decline")
        {
            if (selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The decline branch must not name a card or target.");
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { TongxieFollowUp = pending with { PendingResponders = pending.PendingResponders.Skip(1).ToArray(), DeclinedBy = pending.DeclinedBy.Append(responderSeat).ToArray() } });
            if (ContinueTongxieFollowUp(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).TongxieFollowUp!) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        if (selected.Parameters.GetValueOrDefault("program-action") != "tongxie-follow-up" || !int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId) || selected.Cards.Count != 1 || selected.Cards[0] != cardId || selected.Targets.Count != 1 || selected.Targets[0] != pending.TargetSeat)
            throw new InvalidOperationException("The Tongxie follow-up answer is malformed.");
        var location = _cardZonesOperations._cardZones.GetLocation(cardId);
        var card = location == CardLocation.Hand(responderSeat) ? _cardZonesOperations._cardZones.CardsAt(location).SingleOrDefault(item => item.Id == cardId) : null;
        if (card is null || !CoreNativeCardClassificationsRuntime.IsSlashCard(card.Kind))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(active, "所选【杀】已离开使用者手牌，同协追杀结算已取消。");
            return;
        }

        var usedBy = pending.UsedBy.Append(responderSeat).ToArray();
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { // The reexecute flag re-enters this instruction after the slash
 // child completes; the used evidence stays on the frame.
        ReexecuteParticipantInstruction = true, TongxieFollowUp = pending with { PendingResponders = pending.PendingResponders.Skip(1).ToArray(), UsedBy = usedBy, UsedCardIds = pending.UsedCardIds.Append(cardId).ToArray() } });
        _coreNativeSlashUseOperations.ResolveSlashCore(_coreMatchParticipantsPlayersAccess._players[responderSeat], _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat], card, card.Kind, responderSeat, physicalCards: [card], countsTowardSlashLimit: false, programSkillCardUseFrameId: active.Id);
    }

    internal SkillProgramStepOutcome TongxieProgramGuard(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (frame.TongxieGuard is { } pending)
            return pending.AcceptedPreventer is not null ? FinalizeTongxieGuard(active, pending) : SkillProgramStepOutcome.Continue;
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.BeforeDamageApplied, TargetSeat: { } victim, Amount: { } amount } || amount <= 0 || !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var arm = ActiveTongxieArm(active.OwnerSeat);
        if (arm is null || !IsTongxieMember(arm, victim))
            return SkillProgramStepOutcome.Continue;
        var preventers = TongxieEligiblePreventers(arm.MemberSeats, victim).ToArray();
        if (preventers.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { TongxieGuard = new TongxieGuardState(victim, amount, preventers) });
        return PresentTongxieGuardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    internal SkillProgramStepOutcome PresentTongxieGuardPrompt(ProgramSkillFrame frame)
    {
        var pending = frame.TongxieGuard ?? throw new InvalidOperationException("The Tongxie guard lost its pending state.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var preventerSeat = pending.PendingPreventers[0];
        var preventer = _coreMatchParticipantsPlayersAccess._players[preventerSeat];
        if (!preventer.IsAlive || TongxieLostHpThisTurn(preventerSeat))
        {
            // The candidate list was frozen at window start; a dead or already
            // paid member skips without a prompt.
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { TongxieGuard = pending with { PendingPreventers = pending.PendingPreventers.Skip(1).ToArray() } });
            return ContinueTongxieGuard(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).TongxieGuard!);
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var protectedName = _coreMatchParticipantsPlayersAccess._players[pending.ProtectedSeat].Name;
        var choices = new List<PromptChoice>
        {
            new(new ChoiceId($"tongxie-guard.frame-{(frame.Id)}.prevent"), $"防止 {(protectedName)} 受到的 {(pending.PreventedAmount)} 点伤害，你失去1点体力。", [], [pending.ProtectedSeat], new Dictionary<string, string> { ["program-action"] = "tongxie-guard", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }),
            new(new ChoiceId($"tongxie-guard.frame-{(frame.Id)}.decline"), "不防止此伤害。", [], [], new Dictionary<string, string> { ["program-action"] = "tongxie-guard-decline", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })
        };
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, preventerSeat, $"【{(presentation.Name)}】你可以防止 {(protectedName)} 受到的 {(pending.PreventedAmount)} 点伤害并失去1点体力。", [], [pending.ProtectedSeat], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = preventerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 保护", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = preventer.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal SkillProgramStepOutcome ContinueTongxieGuard(ProgramSkillFrame active, TongxieGuardState pending)
    {
        if (pending.PendingPreventers.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { TongxieGuard = null });
            return SkillProgramStepOutcome.Continue;
        }

        return PresentTongxieGuardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id));
    }

    [ProgramAnswer("tongxie-guard", "tongxie-guard-decline")]
    internal void ResolveTongxieGuardChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Tongxie guard choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.TongxieGuard } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Tongxie guard choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var pending = active.TongxieGuard ?? throw new InvalidOperationException("The Tongxie guard lost its pending state.");
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != pending.PendingPreventers[0])
            throw new InvalidOperationException("The Tongxie guard responder changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var preventerSeat = pending.PendingPreventers[0];
        if (selected.Parameters.GetValueOrDefault("program-action") == "tongxie-guard-decline")
        {
            if (selected.Cards.Count != 0 || selected.Targets.Count != 0)
                throw new InvalidOperationException("The decline branch must not name a card or target.");
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { TongxieGuard = pending with { PendingPreventers = pending.PendingPreventers.Skip(1).ToArray() } });
            if (ContinueTongxieGuard(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id).TongxieGuard!) == SkillProgramStepOutcome.Continue)
                _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        if (selected.Parameters.GetValueOrDefault("program-action") != "tongxie-guard" || selected.Cards.Count != 0)
            throw new InvalidOperationException("The Tongxie guard answer is malformed.");
        // Prevent first, then the preventer's hp cost. A lethal cost starts a
        // dying child: the reexecute flag re-enters this instruction after the
        // child and the accepted state finalizes there.
        _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        var accepted = pending with
        {
            AcceptedPreventer = preventerSeat
        };
        if (_coreMatchParticipantsPlayersAccess._players[preventerSeat].Hp == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { ReexecuteParticipantInstruction = true, TongxieGuard = accepted });
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, preventerSeat, 1);
            return;
        }

        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, preventerSeat, 1);
        FinalizeTongxieGuard(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), accepted);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome FinalizeTongxieGuard(ProgramSkillFrame active, TongxieGuardState pending)
    {
        if (pending.AcceptedPreventer is not { } preventerSeat)
            throw new InvalidOperationException("The Tongxie guard finalize lost its preventer.");
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { TongxieGuard = null });
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】保护生效：" + $"{(_coreMatchParticipantsPlayersAccess._players[pending.ProtectedSeat].Name)} 受到的 {(pending.PreventedAmount)} 点伤害被防止，" + $"{(_coreMatchParticipantsPlayersAccess._players[preventerSeat].Name)} 失去1点体力。", active.OwnerSeat, pending.ProtectedSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTongxieGuardedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, pending.ProtectedSeat, preventerSeat, pending.PreventedAmount));
        return SkillProgramStepOutcome.Continue;
    }
    // 同协 arm: price every offered member set; each chosen member scores as a
    // support target and a guaranteed unique-least draw adds its card value.
    [ProgramAiChoice(SkillProgramEffectOp.TongxieArm)]
    internal PromptChoice SelectAiTongxieArmChoice(PendingDecision decision)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        return decision.Choices.Select(choice =>
        {
            var seats = ParseTongxieArmSeats(choice.Parameters.GetValueOrDefault("seats"));
            var members = new[]
            {
                decision.PlayerSeat
            }.Concat(seats);
            var support = seats.Sum(seat => brain.ScoreProgramTarget(view, seat, new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false)));
            return (Choice: choice, Score: support + TongxieArmDrawBonus(view, decision.PlayerSeat, seats));
        }).OrderByDescending(entry => entry.Score).ThenBy(entry => entry.Choice.Parameters.GetValueOrDefault("seats", ""), StringComparer.Ordinal).First().Choice;
    }

    internal double TongxieArmDrawBonus(GameSnapshot view, int ownerSeat, int[] chosen)
    {
        var counts = new[]
        {
            ownerSeat
        }.Concat(chosen).Select(seat => view.Players.Single(player => player.Seat == seat).HandCount).ToArray();
        var minimum = counts.Min();
        return counts.Count(count => count == minimum) == 1 ? 7d : 0d;
    }

    // 同协 follow-up: use the cheapest slash when the target is hostile enough
    // to be worth the card; the answer stays deterministic in card-id order.
    [ProgramAiChoice(SkillProgramEffectOp.TongxieFollowUp)]
    internal PromptChoice SelectAiTongxieFollowUpChoice(PendingDecision decision)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        var decline = decision.Choices.First(choice => choice.Parameters.GetValueOrDefault("program-action") == "tongxie-follow-up-decline");
        var use = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "tongxie-follow-up");
        if (use is null || use.Targets.Count == 0)
            return decline;
        var value = brain.ScoreProgramTarget(view, use.Targets[0], new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false));
        return value > 0 ? use : decline;
    }

    // 同协 guard: protect a friend while the preventer keeps hp above one; the
    // prevention value scales with the prevented amount.
    [ProgramAiChoice(SkillProgramEffectOp.TongxieGuard)]
    internal PromptChoice SelectAiTongxieGuardChoice(PendingDecision decision)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        var decline = decision.Choices.First(choice => choice.Parameters.GetValueOrDefault("program-action") == "tongxie-guard-decline");
        var prevent = decision.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") == "tongxie-guard");
        if (prevent is null || prevent.Targets.Count == 0)
            return decline;
        if (view.Players.Single(player => player.Seat == decision.PlayerSeat).Hp <= 1)
            return decline;
        var value = brain.ScoreProgramTarget(view, prevent.Targets[0], new SkillProgramAiHint(0, 0, 0, 0, 1, 0, false, false));
        return value > 0 ? prevent : decline;
    }

    SkillProgramStepOutcome IZhaoYanProgramHost.TongxieArm(ProgramSkillFrame frame, SkillProgramEffect effect) => TongxieProgramArm(frame, effect);
    SkillProgramStepOutcome IZhaoYanProgramHost.TongxieFollowUp(ProgramSkillFrame frame, SkillProgramEffect effect) => TongxieProgramFollowUp(frame, effect);
    SkillProgramStepOutcome IZhaoYanProgramHost.TongxieGuard(ProgramSkillFrame frame, SkillProgramEffect effect) => TongxieProgramGuard(frame, effect);
    [ProgramPipelineHook(ProgramPipelinePhase.SelectLegacyDiscardIndexes, 700)]
    internal int[]? SelectWindowIndexes_MatchingTongxieSettledSlashIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, SkillProgramTrigger trigger)
    {
        var features = ProgramInstructionResolver.Default.Features(trigger);
        if (features.HasOperation(SkillProgramEffectOp.TongxieFollowUp))
            return MatchingTongxieSettledSlashIndexes(batch);
        return null;
    }
}
