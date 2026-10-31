#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuHongOlRuntime
{
    internal static string YujueSlotDisplayName(EquipmentSlot slot) => RuntimeSkills.RuntimeRules_liu_hong_ol_Yujue_Programs.YujueSlotDisplayName(slot);
    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 40, SkillProgramEffectOp.YujueResolve)]
    internal bool CanRunYujueResolve(int ownerSeat)
    {
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) || !_coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive)
            return false;
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        if (!CharacterLiuHongOlDefinitions.YujueSlotOrder.Any(slot => owner.EquipmentSlotCapacity(slot) > 0))
            return false;
        return _coreMatchParticipantsPlayersAccess._players.Any(player => player.IsAlive && player.Seat != ownerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > 0);
    }

    internal SkillProgramStepOutcome YujueProgramResolve(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (active.YujuePending is { } pending)
        {
            if (pending.Stage == YujuePendingStage.GiftPaid)
            {
                CompleteYujueInvocation(active, pending, given: true, pending.TargetSeat, pending.GivenCardId);
                return SkillProgramStepOutcome.AwaitChild;
            }

            if (pending.TargetSeat is { } target)
                return PresentYujueGivePrompt(active, pending with { TargetSeat = target });
            PresentYujueTargetPrompt(active, pending);
            return SkillProgramStepOutcome.AwaitChoice;
        }

        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active) || !CanRunYujueResolve(active.OwnerSeat))
            return SkillProgramStepOutcome.Continue;
        return PresentYujueSlotPrompt(active);
    }

    internal SkillProgramStepOutcome PresentYujueSlotPrompt(ProgramSkillFrame active)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var slots = CharacterLiuHongOlDefinitions.YujueSlotOrder.Where(slot => owner.EquipmentSlotCapacity(slot) > 0).ToArray();
        var choices = slots.Select(slot => new PromptChoice(new ChoiceId($"yujue-slot.frame-{(active.Id)}.{(slot)}"), $"废除你的{(YujueSlotDisplayName(slot))}。", [], [], new Dictionary<string, string> { ["program-action"] = "yujue-slot", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["slot"] = slot.ToString() })).ToList<PromptChoice>();
        choices.Add(new PromptChoice(new ChoiceId($"yujue-decline.frame-{(active.Id)}"), "不发动【鬻爵】。", [], [], new Dictionary<string, string> { ["program-action"] = "yujue-decline", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) }));
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】你可以废除一个装备栏，然后令一名有手牌的其他角色交给你一张手牌，其获得“执笏”直到你的下回合开始。", [], [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择要废除的装备栏", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    // Prompt router: every 鬻爵 prompt rides one op, so the shared program
    // dispatch hands each accepted choice back through this single entry.
    [ProgramAnswer("yujue-slot", "yujue-decline", "yujue-target", "yujue-give")]
    internal void ResolveYujueProgramChoice(PromptChoice selected)
    {
        switch (selected.Parameters.GetValueOrDefault("program-action"))
        {
            case "yujue-slot" or "yujue-decline":
                ResolveYujueSlotChoice(selected);
                return;
            case "yujue-target":
                ResolveYujueTargetChoice(selected);
                return;
            case "yujue-give":
                ResolveYujueGiveChoice(selected);
                return;
            default:
                throw new InvalidOperationException("The Yujue choice lost its action parameter.");
        }
    }

    internal void ResolveYujueSlotChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Yujue choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.YujueResolve } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Yujue choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (selected.Parameters.GetValueOrDefault("program-action") == "yujue-decline")
        {
            var activation = _contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Program!.Activations.Single(entry => entry.Id == active.ActivationId);
            var key = (active.OwnerSeat, active.SkillId, activation.UsageGroup);
            if (_coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses.GetValueOrDefault(key) <= 0 || _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses.GetValueOrDefault(key) <= 0)
                throw new InvalidOperationException("An unpaid Yujue cancellation lost its exact activation usage.");
            _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programUses[key]--;
            _coreGameEngineSkillProgramsProgramPhaseUsesProgramUsesAccess._programPhaseUses[key]--;
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        if (!Enum.TryParse<EquipmentSlot>(selected.Parameters.GetValueOrDefault("slot"), out var slot) || !CharacterLiuHongOlDefinitions.YujueSlotOrder.Contains(slot) || _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].EquipmentSlotCapacity(slot) <= 0)
            throw new InvalidOperationException("The Yujue slot is no longer available.");
        if (!CanRunYujueResolve(active.OwnerSeat))
            throw new InvalidOperationException("The Yujue invocation lost its prerequisites.");
        var source = new CardConversionSource(active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, active.SkillInstanceId);
        var pending = new YujuePendingState(slot.ToString(), null, source, active.GameplayHash, active.InstructionIndex);
        AbolishYujueSlot(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), slot);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYujueSlotPaidEvent(active.Id, source, active.GameplayHash, active.InstructionIndex, slot.ToString()));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { YujuePending = pending, ReexecuteParticipantInstruction = true });
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal void AbolishYujueSlot(ProgramSkillFrame frame, EquipmentSlot slot)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (frame.TriggerId is not null || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != owner.Seat || !owner.IsAlive || owner.EquipmentSlotCapacity(slot) <= 0)
            throw new InvalidOperationException("The Yujue slot payment lost its own Play activation.");
        _coreAdvancedLifecycleOperations.SetEquipmentSlotCapacity(owner, slot, 0);
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】，废除自己的{(YujueSlotDisplayName(slot))}。", active.OwnerSeat);
    }

    internal void PresentYujueTargetPrompt(ProgramSkillFrame frame, YujuePendingState pending)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count > 0).Select(player => player.Seat).OrderBy(seat => seat).ToArray();
        if (candidates.Length == 0)
        {
            // The recipient pool collapsed (only possible through a foreign
            // trigger inside the cards-moved window); the invocation ends and
            // the abolished slot stays spent.
            CompleteYujueInvocation(active, pending, given: false, targetSeat: null, givenCardId: null);
            return;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(seat => new PromptChoice(new ChoiceId($"yujue-target.frame-{(active.Id)}.seat-{(seat)}"), $"令 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 交给你一张手牌。", [], [seat], new Dictionary<string, string> { ["program-action"] = "yujue-target", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target-seat"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择一名有手牌的其他角色。", [], candidates, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择交牌角色", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal void ResolveYujueTargetChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Yujue target choice lost its program frame.");
        var pending = frame.YujuePending ?? throw new InvalidOperationException("The Yujue target choice lost its pending state.");
        if (!int.TryParse(selected.Parameters.GetValueOrDefault("target-seat"), System.Globalization.CultureInfo.InvariantCulture, out var targetSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[targetSeat]).Count == 0)
            throw new InvalidOperationException("The Yujue recipient is no longer legal.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var advanced = pending with
        {
            TargetSeat = targetSeat
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { YujuePending = advanced });
        PresentYujueGivePrompt(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), advanced);
    }

    internal SkillProgramStepOutcome PresentYujueGivePrompt(ProgramSkillFrame frame, YujuePendingState pending)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var targetSeat = pending.TargetSeat ?? throw new InvalidOperationException("The Yujue give prompt lost its recipient.");
        var target = _coreMatchParticipantsPlayersAccess._players[targetSeat];
        if (!target.IsAlive || _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count == 0)
        {
            CompleteYujueInvocation(active, pending, given: false, targetSeat, null);
            return SkillProgramStepOutcome.AwaitChild;
        }

        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(target).OrderBy(card => card.Id).ToArray();
        var choices = hand.Select(card => new PromptChoice(new ChoiceId($"yujue-give.frame-{(active.Id)}.card-{(card.Id)}"), $"交给 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)}【{(card.DisplayName)}】。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "yujue-give", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["card-id"] = card.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, targetSeat, $"【{(presentation.Name)}】请交给 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 一张手牌。", hand.Select(card => card.Id).ToArray(), [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = targetSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 交牌", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 令你交给其一张手牌，你获得“执笏”直到其下回合开始。"),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = target.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveYujueGiveChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Yujue give choice lost its program frame.");
        var pending = frame.YujuePending ?? throw new InvalidOperationException("The Yujue give choice lost its pending state.");
        var targetSeat = pending.TargetSeat ?? throw new InvalidOperationException("The Yujue give choice lost its recipient.");
        if (!int.TryParse(selected.Parameters.GetValueOrDefault("card-id"), System.Globalization.CultureInfo.InvariantCulture, out var cardId) || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(targetSeat))
            throw new InvalidOperationException("The Yujue given card is no longer in the recipient hand.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(targetSeat)).Single(card => card.Id == cardId), CardLocation.Hand(targetSeat), CardLocation.Hand(active.OwnerSeat), new($"skill-program.{(active.SkillId)}.yujue-give"), _characterHuangQuanMouOperations.NativeGiveCompletion(frame.Id, targetSeat, active.OwnerSeat, [cardId], _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, null));
        _coreMatchLogOperations.AddLog("CardGained", $"{(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 交给 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 一张手牌。", targetSeat, active.OwnerSeat);
        var movementSequence = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last().Sequence;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYujueGiftPaidEvent(active.Id, pending.Source, active.GameplayHash, active.InstructionIndex, targetSeat, cardId, movementSequence));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { YujuePending = pending with { Stage = YujuePendingStage.GiftPaid, GivenCardId = cardId, GiftMovementSequence = movementSequence }, ReexecuteParticipantInstruction = true });
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal void CompleteYujueInvocation(ProgramSkillFrame frame, YujuePendingState pending, bool given, int? targetSeat, int? givenCardId)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYujueInvokedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, pending.AbolishedSlot, targetSeat ?? -1, givenCardId ?? -1));
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { YujuePending = null, ReexecuteParticipantInstruction = false });
        if (given && targetSeat is { } seat)
        {
            _coreCharacterHostOperations.AcquireRuntimeSkills(_coreMatchParticipantsPlayersAccess._players[seat], $"zhihu:{(active.OwnerSeat)}:{(active.Id)}", ["ol:zhihu"]);
            _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 获得“执笏”，直到 {(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的下回合开始。", active.OwnerSeat, seat);
        }

        _coreMatchLogOperations.AddLog("SkillTriggered", given ? $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】结算完成：废除{(YujueSlotDisplayName(Enum.Parse<EquipmentSlot>(pending.AbolishedSlot)))}，获得一张手牌。" : $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 的【{(_contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId).Name)}】结算完成：废除{(YujueSlotDisplayName(Enum.Parse<EquipmentSlot>(pending.AbolishedSlot)))}。", active.OwnerSeat, targetSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal PromptChoice SelectAiYujueSlotChoice(PendingDecision decision)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[decision.PlayerSeat];
        var ranked = CharacterLiuHongOlDefinitions.YujueAiAbolishPreference.Where(slot => owner.EquipmentSlotCapacity(slot) > 0).Select((slot, index) => (Slot: slot, Index: index, Equipped: _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Any(card => _coreEquipmentContentOperations.EquipmentRules.Get(card.Kind).Slot == slot))).OrderBy(entry => entry.Equipped ? 1 : 0).ThenBy(entry => entry.Index).First();
        return decision.Choices.Single(choice => choice.Parameters.GetValueOrDefault("slot") == ranked.Slot.ToString());
    }

    internal PromptChoice SelectAiYujueTargetChoice(PendingDecision decision)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        return decision.Choices.Select(choice =>
        {
            var targetSeat = int.Parse(choice.Parameters.GetValueOrDefault("target-seat", "-1"), System.Globalization.CultureInfo.InvariantCulture);
            var hint = new SkillProgramAiHint(0, 0, 0, 2, 0, 0, false, false);
            return (Choice: choice, Score: brain.ScoreProgramTarget(view, targetSeat, hint));
        }).OrderByDescending(entry => entry.Score).ThenBy(entry => entry.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
    }

    internal PromptChoice SelectAiYujueGiveChoice(PendingDecision decision) => decision.Choices.OrderBy(choice => choice.Id.Value, StringComparer.Ordinal).First();
    [ProgramAiChoice(SkillProgramEffectOp.YujueResolve)]
    internal PromptChoice SelectAiYujueChoice(PendingDecision decision)
    {
        var first = decision.Choices[0].Parameters.GetValueOrDefault("program-action");
        if (first == "yujue-give")
            return SelectAiYujueGiveChoice(decision);
        if (first == "yujue-target")
            return SelectAiYujueTargetChoice(decision);
        if (first == "yujue-slot")
            return SelectAiYujueSlotChoice(decision);
        // Decline only survives when every slot choice is unwanted: the AI
        // always spends an empty slot first, so the decline is a fallback.
        return decision.Choices.First(choice => choice.Parameters.GetValueOrDefault("program-action") == "yujue-decline");
    }
}
