#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSimaYiHappyGodRuntime
{
    internal const string HappySimaRenjie = "happy:renjie", HappySimaJilue = "happy:jilue", HappySimaWansha = "happy:jilue-wansha";
    internal static CardConversionSource HappySimaSource(ProgramSkillFrame f) => new(f.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool AllowsHappySimaPeach(int actor) => actor == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || !_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[_coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat], HappySimaWansha);
    internal int HappySimaDiscardCount(int owner, ProgramSkillWindowContext? context)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardsMoved, MovementBatch: { MovementTiming: { Phase: TurnPhase.Discard } timing } batch } || timing.PhaseActorSeat != owner || batch.TurnNumber != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber)
            return 0;
        return batch.Movements.Where(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(m) == CardLocation.Hand(owner)).Select(m => m.CardId).Distinct().Count();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 41840)]
    internal bool AllowsHappySimaTrigger(ProgramTriggerLegalityContext g)
    {
        if (g.SkillId == HappySimaRenjie && g.Trigger.Id == "discard-ren")
            return HappySimaDiscardCount(g.OwnerSeat, g.WindowContext) > 0;
        if (g.SkillId == HappySimaJilue && g.Trigger.Id == "dying-wansha")
            return g.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && !_coreNationalSkillsOperations.HasRuntimeSkill(g.Owner, HappySimaWansha);
        return true;
    }

    [ProgramActivationLegality(ProgramActivationLegalityPhase.BeforeOwnerFacts, 41840, SkillProgramEffectOp.OfferHappySimaJilueFirstDraw)]
    internal bool AllowsHappySimaActivation(ProgramActivationLegalityContext g) => g.SkillId != HappySimaJilue || !_coreNationalSkillsOperations.HasRuntimeSkill(g.Owner, HappySimaWansha);
    SkillProgramStepOutcome IHappySimaYiHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op) => op == SkillProgramEffectOp.GainHappySimaRenjie ? GainHappySimaRen(f) : BeginHappySimaFirstDraw(f);
    internal SkillProgramStepOutcome GainHappySimaRen(ProgramSkillFrame f)
    {
        if (f.SkillId != HappySimaRenjie || f.InstructionIndex != 1 || f.WindowContext is not { } context)
            throw new InvalidOperationException("Happy Renjie requires its actual initialization, damage or frozen discard batch.");
        var amount = f.TriggerId switch
        {
            "initial-ren" when context.Window == SkillProgramTriggerWindow.GameStarting => 1,
            "damage-ren" when context.Window == SkillProgramTriggerWindow.AfterDamageApplied && context.TargetSeat == f.OwnerSeat => context.Amount,
            "discard-ren" => HappySimaDiscardCount(f.OwnerSeat, context),
            _ => 0
        };
        _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().ChangeAttributedMarker(f, new(ProgramParticipantRef.Owner), PlayerMarkerKind.Ren, amount);
        return SkillProgramStepOutcome.Continue;
    }

    internal long HappySimaFirstAcceptedFrame(int owner)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindLastIndex(history, e => e is TurnStartedEvent turn && turn.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && turn.ActorSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat);
        if (start < 0)
            return 0;
        foreach (var fact in history.Skip(start + 1))
        {
            if (fact is ProgramSkillStartedEvent active && active.SkillId == HappySimaJilue && active.OwnerSeat == owner)
                return active.FrameId;
            if (fact is ProgramBindingStartedEvent triggered && triggered.SkillId == HappySimaJilue && triggered.OwnerSeat == owner)
                return triggered.FrameId;
        }

        return 0;
    }

    internal PlayerMarkerChangedEvent? HappySimaPaidRen(ProgramSkillFrame f)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var start = Array.FindIndex(history, e => e is ProgramSkillStartedEvent a && a.FrameId == f.Id && a.SkillId == f.SkillId && a.OwnerSeat == f.OwnerSeat && a.ActivationId == f.ActivationId || e is ProgramBindingStartedEvent t && t.FrameId == f.Id && t.SkillId == f.SkillId && t.OwnerSeat == f.OwnerSeat && t.BindingId == f.TriggerId && t.SkillInstanceId == f.SkillInstanceId);
        return start < 0 ? null : history.Take(start).OfType<PlayerMarkerChangedEvent>().LastOrDefault(e => e.PlayerSeat == f.OwnerSeat && e.Marker == PlayerMarkerKind.Ren && e.Delta == -1 && e.Reason == $"skill-program.{(f.SkillId)}.{(CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f))}.cost");
    }

    internal SkillProgramStepOutcome BeginHappySimaFirstDraw(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != HappySimaJilue || f.HappySimaYi is not null || HappySimaPaidRen(f)is not { } paid)
            throw new InvalidOperationException("Happy Jilue's reward requires its original accepted one-Ren payment.");
        if (HappySimaFirstAcceptedFrame(f.OwnerSeat) != f.Id)
            return SkillProgramStepOutcome.Continue;
        var r = new HappySimaYiReceipt(HappySimaSource(f), f.GameplayHash, f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, f.InstructionIndex, paid.ResolutionId, HappySimaYiStage.Choice);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HappySimaYi = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySimaJilueFirstOfferedEvent(f.Id, r.Source, r.Hash, r.Parent, r.Turn, r.Instruction, r.Payment));
        PublishHappySimaFirstDraw(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> HappySimaFirstChoices(ProgramSkillFrame f) => new[]
    {
        ("draw", "本回合首次发动极略，摸一张牌"),
        ("decline", "不摸牌，继续原极略")
    }.Select(pair => new PromptChoice(new("happy-sima.first." + f.Id + "." + pair.Item1), pair.Item2, [], [], new Dictionary<string, string> { ["program-action"] = "happy-sima-first-draw", ["frame-id"] = f.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["mode"] = pair.Item1 })).ToArray();
    internal void PublishHappySimaFirstDraw(ProgramSkillFrame f)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        var choices = HappySimaFirstChoices(f);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "极略：本实际回合首次发动，可摸一张牌。", [], [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("happy-sima-first-draw")]
    internal void AnswerHappySimaFirstDraw(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappySimaYi: { Stage: HappySimaYiStage.Choice } } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !HappySimaFirstChoices(f).Any(c => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Happy Jilue requires the exact original first-use choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var take = choice.Parameters["mode"] == "draw";
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySimaJilueFirstChosenEvent(f.Id, take));
        if (!take)
        {
            CompleteHappySimaFirst(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect, HappySimaYi = f.HappySimaYi!with { Stage = HappySimaYiStage.Drawing } });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, 1, HappySimaDrawReason);
    }

    internal static CardMoveReason HappySimaDrawReason => new("skill.happy.jilue.first-draw");

    internal void CompleteHappySimaFirst(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.ResolvingEffect, HappySimaYi = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 41840)]
    internal bool ResumeHappySimaFirst(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { HappySimaYi: { } r } f || f.Id != id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null)
            return false;
        if (r.Stage == HappySimaYiStage.Choice)
        {
            PublishHappySimaFirstDraw(f);
            return true;
        }

        if (r.Stage == HappySimaYiStage.Drawing)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        CompleteHappySimaFirst(_coreProgramLifecycleOperations.GetActiveProgramFrame(id));
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 41840)]
    internal bool ReturnHappySimaFirstMovement(ProgramSkillFrame f) => f.HappySimaYi is not null && ResumeHappySimaFirst(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.OfferHappySimaJilueFirstDraw)]
    internal void ReturnHappySimaFirstDraw(ProgramSkillFrame f, NativeDrawOutcome outcome)
    {
        var r = f.HappySimaYi!;
        if (r.Stage != HappySimaYiStage.Drawing || r.Draw is not null || outcome.RecipientSeat != f.OwnerSeat || outcome.RequestedCount != 1 || outcome.SequenceAfter != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            throw new InvalidOperationException("Happy Jilue lost its once-requested one-card native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappySimaYi = r with { Stage = HappySimaYiStage.Children, Draw = outcome }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HappySimaJilueDrawReturnedEvent(f.Id, outcome.ActualDrawIds.Count, outcome.SequenceBefore, outcome.SequenceAfter, outcome.AttemptFrameId));
        ResumeHappySimaFirst(f.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.OfferHappySimaJilueFirstDraw)]
    internal PromptChoice ChooseHappySimaFirst(PendingDecision p) => p.Choices.First();
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41840)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41840)]
    internal bool RetainHappySimaFirst(ProgramSkillFrame f) => f.HappySimaYi is not null;
}
