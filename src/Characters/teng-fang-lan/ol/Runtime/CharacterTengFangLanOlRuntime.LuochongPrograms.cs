#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTengFangLanOlRuntime
{
    internal static string LuochongOptionDisplayName(string optionId) => RuntimeSkills.RuntimeRules_teng_fang_lan_ol_Luochong_Programs.LuochongOptionDisplayName(optionId);
    internal static bool IsLuochongOptionId(string optionId) => CharacterTengFangLanOlDefinitions.LuochongOptionIds.Contains(optionId);
    internal (IReadOnlySet<string> Options, IReadOnlySet<int> Targets) LuochongRoundLedger(int ownerSeat) => (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramLuochongResolvedEvent>().Where(fact => fact.OwnerSeat == ownerSeat && fact.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber).Select(fact => fact.OptionId).ToHashSet(StringComparer.Ordinal), _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramLuochongResolvedEvent>().Where(fact => fact.OwnerSeat == ownerSeat && fact.RoundNumber == _coreActualRoundProgressRoundNumberAccess._roundNumber).Select(fact => fact.TargetSeat).ToHashSet());
    internal IReadOnlyList<(Card Card, CardLocation Location)> LuochongDiscardPool(int targetSeat)
    {
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        return _coreTurnHandCategoryRestrictionsOperations.GetSelfDiscardableOwnedHand(target).Select(card => (card, CardLocation.Hand(targetSeat))).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Select(card => (card, CardLocation.Equipment(targetSeat)))).OrderBy(entry => entry.card.Id).ToArray();
    }

    internal bool IsLuochongOptionAvailableFor(int ownerSeat, string optionId, int targetSeat, IReadOnlySet<string> usedOptions, IReadOnlySet<int> usedTargets, IReadOnlySet<string> removedOptions)
    {
        if (removedOptions.Contains(optionId) || usedOptions.Contains(optionId) || usedTargets.Contains(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive)
            return false;
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        return optionId switch
        {
            // Recovery on an unwounded target is a no-op and never offered.
            "recover" => target.Hp < target.MaxHp,
            "loseHp" => true,
            "discardTwo" => LuochongDiscardPool(targetSeat).Count > 0,
            "drawTwo" => true,
            _ => false
        };
    }

    internal IReadOnlyList<(string OptionId, int TargetSeat)> CollectLuochongAvailablePairs(int ownerSeat, out (IReadOnlySet<string> Options, IReadOnlySet<int> Targets) ledger)
    {
        var(options, targets) = LuochongRoundLedger(ownerSeat);
        ledger = (options, targets);
        var removed = AichenRemovedOptions(ownerSeat);
        return CharacterTengFangLanOlDefinitions.LuochongOptionIds.Where(optionId => !removed.Contains(optionId) && !options.Contains(optionId)).SelectMany(optionId => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).Select(player => player.Seat).Where(targetSeat => !targets.Contains(targetSeat)).Where(targetSeat => IsLuochongOptionAvailableFor(ownerSeat, optionId, targetSeat, options, targets, removed)).Select(targetSeat => (optionId, targetSeat))).OrderBy(entry => Array.IndexOf(CharacterTengFangLanOlDefinitions.LuochongOptionIds, entry.optionId)).ThenBy(entry => entry.targetSeat).ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 140, SkillProgramEffectOp.LuochongResolve)]
    internal bool CanRunLuochongResolve(int ownerSeat)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        return CollectLuochongAvailablePairs(ownerSeat, out _).Count > 0;
    }

    internal SkillProgramStepOutcome LuochongProgramResolve(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        if (frame.LuochongPending is { } pending)
            return FinalizeLuochongResolution(active, pending);
        if (!owner.IsAlive)
            return SkillProgramStepOutcome.Continue;
        var pairs = CollectLuochongAvailablePairs(active.OwnerSeat, out _);
        if (pairs.Count == 0)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = pairs.Select(pair => new PromptChoice(new ChoiceId($"luochong-pick.frame-{(frame.Id)}.{(pair.OptionId)}-{(pair.TargetSeat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[pair.TargetSeat].Name)} {(LuochongOptionDisplayName(pair.OptionId))}。", [], [pair.TargetSeat], new Dictionary<string, string> { ["program-action"] = "luochong-pick", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = pair.OptionId, ["target-seat"] = pair.TargetSeat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToList<PromptChoice>();
        choices.Add(new PromptChoice(new ChoiceId($"luochong-decline.frame-{(frame.Id)}"), "不发动【落宠】。", [], [], new Dictionary<string, string> { ["program-action"] = "luochong-decline", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】你可以选择一项，令一名角色执行。每轮每项每名角色限一次。", [], choices.Where(choice => choice.Targets.Count > 0).SelectMany(choice => choice.Targets).ToArray(), active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择选项与目标", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("luochong-pick", "luochong-decline")]
    internal void ResolveLuochongPickChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Luochong choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.LuochongResolve } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Luochong choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters.GetValueOrDefault("program-action") == "luochong-decline")
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        if (!int.TryParse(selected.Parameters.GetValueOrDefault("target-seat"), System.Globalization.CultureInfo.InvariantCulture, out var targetSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || selected.Parameters.GetValueOrDefault("option")is not { } option || !IsLuochongOptionId(option))
            throw new InvalidOperationException("The Luochong choice lost its option or target.");
        if (!CanRunLuochongResolve(active.OwnerSeat))
            throw new InvalidOperationException("The Luochong choice has no remaining available pair.");
        var pending = new LuochongPendingState(option, targetSeat, 0);
        switch (option)
        {
            case "recover":
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(active.Id, active.OwnerSeat, targetSeat, 1, null, null);
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，令 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 回复1点体力。", active.OwnerSeat, targetSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(LuochongResolvedEvent(active, pending));
                _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
                return;
            case "loseHp" when _coreMatchParticipantsPlayersAccess._players[targetSeat].Hp == 1:
                // Keep the paid instruction committed while the real dying child
                // is live. Its existing repeat flag re-enters this operation only
                // after the child returns, where the receipt finalizes once.
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { ReexecuteParticipantInstruction = true, LuochongPending = pending });
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，令 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 失去1点体力。", active.OwnerSeat, targetSeat);
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, targetSeat, 1);
                return;
            case "loseHp":
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(active.Id, active.SkillId, targetSeat, 1);
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，令 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 失去1点体力。", active.OwnerSeat, targetSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(LuochongResolvedEvent(active, pending));
                _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
                return;
            case "discardTwo":
            {
                var remaining = Math.Min(2, LuochongDiscardPool(targetSeat).Count);
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { LuochongPending = pending with { RemainingDiscards = remaining } });
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，令 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 弃置牌。", active.OwnerSeat, targetSeat);
                PresentLuochongDiscardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
                return;
            }

            default:
                _corePhysicalDrawFacadesOperations.DrawCards(_coreMatchParticipantsPlayersAccess._players[targetSeat], 2, true, new($"skill-program.{(active.SkillId)}.luochong-draw"));
                _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，令 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 摸两张牌。", active.OwnerSeat, targetSeat);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(LuochongResolvedEvent(active, pending));
                if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
                    _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
                return;
        }
    }

    internal ProgramLuochongResolvedEvent LuochongResolvedEvent(ProgramSkillFrame active, LuochongPendingState pending) => new(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, _coreActualRoundProgressRoundNumberAccess._roundNumber, pending.OptionId, pending.TargetSeat, pending.OptionId == "recover" ? 1 : 0, pending.OptionId == "loseHp" ? 1 : 0, pending.DiscardedCardIds, pending.OptionId == "drawTwo" ? 2 : 0);
    internal SkillProgramStepOutcome FinalizeLuochongResolution(ProgramSkillFrame frame, LuochongPendingState pending)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { LuochongPending = null, ReexecuteParticipantInstruction = false });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(LuochongResolvedEvent(active, pending));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }
    internal void PresentLuochongDiscardPrompt(ProgramSkillFrame frame)
    {
        var pending = frame.LuochongPending ?? throw new InvalidOperationException("The Luochong discard lost its pending state.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var pool = LuochongDiscardPool(pending.TargetSeat);
        if (pending.RemainingDiscards <= 0 || pool.Count == 0)
        {
            CompleteLuochongDiscardStage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = pool.Select(entry => new PromptChoice(new ChoiceId($"luochong-discard.frame-{(frame.Id)}.card-{(entry.Card.Id)}"), $"弃置【{(entry.Card.DisplayName)}】。", [entry.Card.Id], [], new Dictionary<string, string> { ["program-action"] = "luochong-discard", ["card-id"] = entry.Card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, pending.TargetSeat, $"【{(presentation.Name)}】请弃置 {(pending.RemainingDiscards)} 张牌。", pool.Select(entry => entry.Card.Id).ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = pending.TargetSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 弃牌", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 令你弃置 {(pending.RemainingDiscards)} 张牌。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[pending.TargetSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("luochong-discard")]
    internal void ResolveLuochongDiscardChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Luochong discard choice lost its program frame.");
        var pending = frame.LuochongPending ?? throw new InvalidOperationException("The Luochong discard lost its pending state.");
        if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId) || LuochongDiscardPool(pending.TargetSeat).SingleOrDefault(entry => entry.Card.Id == cardId).Card is null)
            throw new InvalidOperationException("The Luochong discard card is no longer discardable.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var entry = LuochongDiscardPool(pending.TargetSeat).Single(item => item.Card.Id == cardId);
        active = _coreNativeLogicalDiscardsOperations.AppendFrameNativeLogicalDiscard(active, pending.TargetSeat, [cardId], new($"skill-program.{(active.SkillId)}.luochong-discard"));
        _corePhysicalMovementFacadesOperations.MoveCard(entry.Card, entry.Location, CardLocation.DiscardPile, new($"skill-program.{(active.SkillId)}.luochong-discard"));
        _coreMatchLogOperations.AddLog("CardDiscarded", $"{(_coreMatchParticipantsPlayersAccess._players[pending.TargetSeat].Name)} 因【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】弃置【{(entry.Card.DisplayName)}】。", pending.TargetSeat, active.OwnerSeat);
        var discarded = pending.DiscardedCardIds.Append(cardId).ToArray();
        if (pending.RemainingDiscards > 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { LuochongPending = pending with { RemainingDiscards = pending.RemainingDiscards - 1, DiscardedCardIds = discarded } });
            PresentLuochongDiscardPrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { LuochongPending = pending with { RemainingDiscards = 0, DiscardedCardIds = discarded } });
        CompleteLuochongDiscardStage(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    internal void CompleteLuochongDiscardStage(ProgramSkillFrame frame)
    {
        var pending = frame.LuochongPending ?? throw new InvalidOperationException("The Luochong discard stage lost its pending state.");
        var active = _coreNativeLogicalDiscardsOperations.CompleteFrameNativeLogicalDiscard(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { LuochongPending = null });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(LuochongResolvedEvent(active, pending));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    // 落宠 pick: price every offered pair through the shared target scoring; the
    // decline choice only survives when every pair scores negatively.
    [ProgramAiChoice(SkillProgramEffectOp.LuochongResolve)]
    internal PromptChoice SelectAiLuochongResolveChoice(PendingDecision decision)
    {
        if (decision.Choices[0].Parameters.GetValueOrDefault("program-action") == "luochong-discard")
            return decision.Choices.OrderBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        return decision.Choices.Select(choice =>
        {
            if (choice.Parameters.GetValueOrDefault("program-action") == "luochong-decline")
                return (Choice: choice, Score: double.NegativeInfinity);
            var option = choice.Parameters.GetValueOrDefault("option") ?? "";
            var targetSeat = int.Parse(choice.Parameters.GetValueOrDefault("target-seat", "-1"), System.Globalization.CultureInfo.InvariantCulture);
            var hint = option switch
            {
                "drawTwo" => new SkillProgramAiHint(0, 0, 0, 2, 0, 0, false, false),
                "recover" => new SkillProgramAiHint(0, 0, 0, 0, 1, 0, false, false),
                "loseHp" => new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false),
                "discardTwo" => new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false)
                {
                    TargetValueAdjustment = -10d
                },
                _ => new SkillProgramAiHint(0, 0, 0, 0, 0, 0, false, false)};
            return (Choice: choice, Score: brain.ScoreProgramTarget(view, targetSeat, hint));
        }).OrderByDescending(entry => entry.Score).ThenBy(entry => entry.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
    }
}
