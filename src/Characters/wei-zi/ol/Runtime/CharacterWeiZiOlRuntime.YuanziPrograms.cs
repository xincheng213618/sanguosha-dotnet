#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWeiZiOlRuntime
{
    internal SkillProgramStepOutcome YuanziProgramResolve(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow, SourceSeat: { } boundarySeat } || boundarySeat == active.OwnerSeat || boundarySeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !owner.IsAlive || !_coreMatchParticipantsPlayersAccess._players[boundarySeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(owner).OrderBy(card => card.Id).ToArray();
        if (hand.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        _corePhysicalMovementFacadesOperations.MoveCards(hand, CardLocation.Hand(active.OwnerSeat), CardLocation.Hand(boundarySeat), new($"skill-program.{(active.SkillId)}.yuanzi-give"), _characterHuangQuanMouOperations.NativeGiveCompletion(active.Id, active.OwnerSeat, boundarySeat, hand.Select(c => c.Id).ToArray(), _coreCardMovementJournalMovementSequenceAccess._movementSequence, null));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(owner.Name)} 发动【{(presentation.Name)}】，将所有手牌（{(hand.Length)}张）交给 {(_coreMatchParticipantsPlayersAccess._players[boundarySeat].Name)}。", active.OwnerSeat, boundarySeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYuanziInvokedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, boundarySeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome YuanziProgramDamageDraw(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId) || frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } sourceSeat, Amount: > 0 } || sourceSeat == active.OwnerSeat || sourceSeat != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) || !_coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive)
            return SkillProgramStepOutcome.Continue;
        var invocation = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramYuanziInvokedEvent>().LastOrDefault(fact => fact.OwnerSeat == active.OwnerSeat && fact.RecipientSeat == sourceSeat && fact.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber);
        if (invocation is null || _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[sourceSeat]).Count < _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { YuanziDrawPending = new YuanziDrawPendingState(sourceSeat) });
        var frameId = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】你可以摸两张牌。", [], [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = sourceSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 摸牌", $"{(_coreMatchParticipantsPlayersAccess._players[sourceSeat].Name)} 本回合造成伤害后手牌数不小于你，你可以摸两张牌。"),
            Choices = new List<PromptChoice>
            {
                new(new ChoiceId($"yuanzi-draw.frame-{(active.Id)}"), "摸两张牌。", [], [], new Dictionary<string, string> { ["program-action"] = "yuanzi-draw", ["frame-id"] = frameId }),
                new(new ChoiceId($"yuanzi-decline.frame-{(active.Id)}"), $"不发动【{(presentation.Name)}】。", [], [], new Dictionary<string, string> { ["program-action"] = "yuanzi-decline", ["frame-id"] = frameId })
            }.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal void ResolveYuanziDrawChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Yuanzi draw choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.YuanziDamageDraw } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Yuanzi draw choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } decision || decision.PlayerSeat != active.OwnerSeat)
            throw new InvalidOperationException("The Yuanzi draw chooser changed while suspended.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var pending = active.YuanziDrawPending ?? throw new InvalidOperationException("The Yuanzi draw choice lost its pending state.");
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id)with { YuanziDrawPending = null });
        if (selected.Parameters.GetValueOrDefault("program-action") != "yuanzi-draw")
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
            return;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        var drawn = _corePhysicalDrawFacadesOperations.DrawCards(owner, 2, true, new($"skill-program.{(active.SkillId)}.yuanzi-draw"));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 的【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】生效：摸两张牌。", active.OwnerSeat, pending.RecipientSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramYuanziDamageDrawEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, pending.RecipientSeat, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, Array.AsReadOnly(drawn.ToArray())));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow())
            _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }
}
