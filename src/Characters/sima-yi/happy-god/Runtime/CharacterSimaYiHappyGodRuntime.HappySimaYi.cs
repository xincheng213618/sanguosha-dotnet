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
    SkillProgramStepOutcome IHappySimaYiHost.Execute(ProgramSkillFrame f, SkillProgramEffectOp op, string? resultBind) => op == SkillProgramEffectOp.GainHappySimaRenjie ? GainHappySimaRen(f) : CaptureHappySimaFirstDraw(f, resultBind!);
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

    internal SkillProgramStepOutcome CaptureHappySimaFirstDraw(ProgramSkillFrame supplied, string resultBind)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.SkillId != HappySimaJilue || f.HappySimaYi is not null || HappySimaPaidRen(f)is not { } paid)
            throw new InvalidOperationException("Happy Jilue's reward requires its original accepted one-Ren payment.");
        var first = HappySimaFirstAcceptedFrame(f.OwnerSeat) == f.Id;
        if (first)
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HappySimaYi = new(HappySimaSource(f), f.GameplayHash,
                f.WindowContext?.ParentFrameId ?? 0, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber,
                f.InstructionIndex, paid.ResolutionId) });
        _coreProgramExpressionsOperations.CaptureProgramExpressionNumber(f.Id, resultBind, first ? 1 : 0);
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ChooseOption, SkillId = "happy:jilue")]
    internal PromptChoice ChooseHappySimaFirst(PendingDecision prompt) => prompt.Choices.First(choice => choice.Parameters.GetValueOrDefault("option-id") == "draw");
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 41840)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 41840)]
    internal bool RetainHappySimaFirst(ProgramSkillFrame f)
    {
        if (f.HappySimaYi is not { } accepted)
            return false;
        var drawIndex = accepted.Instruction + 2;
        if (f.NativeDraw?.InstructionIndex == drawIndex)
            return true;
        if (f.InstructionIndex < accepted.Instruction || f.InstructionIndex > drawIndex ||
            f.ChoiceBindings.Any(choice => choice.Name == HappySimaYiComposition.FirstChoice && choice.OptionId == "decline"))
            return false;
        return !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramDrawCompletedEvent>()
            .Any(draw => draw.FrameId == f.Id && draw.InstructionIndex == drawIndex);
    }
}
