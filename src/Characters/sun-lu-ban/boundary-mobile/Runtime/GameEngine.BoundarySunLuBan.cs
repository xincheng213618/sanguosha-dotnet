namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string BoundaryZenhui = "boundary:zenhui-mobile", BoundaryZenhuiAction = "boundary-sun-lu-ban-choice";

    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 54100)]
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 54100)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 54100)]
    private bool BoundarySunLuBanTargetWindow(ProgramTargetCardWindowQuery query) =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui);

    private bool AllowsBoundarySunLuBanDrawTwoRoleTarget(CardUseFrame use) =>
        use is { CardKind: CardKind.DrawTwo, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.DrawTwo } action } &&
        use.Step != ResolutionFrameStep.Completed && use.SourceSeat == action.ActorSeat &&
        action.EffectiveDesignatedTargetSeats is [var self] && self == action.ActorSeat &&
        (action.EffectiveIsRed == false || action.EffectiveIsRed is null && action.EffectiveSuit is Suit.Spade or Suit.Club) &&
        _phase == TurnPhase.Play && _currentSeat == action.ActorSeat &&
        ReferenceEquals(use, _resolutionStack.SingleOrDefault(frame => frame.Id == use.Id)) &&
        EnabledSkillPrograms(_players[action.ActorSeat]).Any(program => program.Id == BoundaryZenhui) &&
        ProgramEventHistory<CardUseDeclaredEvent>().Any(declared => declared.ResolutionId == use.Id && declared.SourceSeat == action.ActorSeat &&
            declared.CardId == use.CardId && declared.CardKind == CardKind.DrawTwo);

    [ProgramCapability]
    private sealed class BoundarySunLuBanHost(GameEngine engine) : IBoundarySunLuBanHost
    { public SkillProgramStepOutcome Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => engine.BeginBoundarySunLuBanChoice(frame, effect); }

    private IReadOnlyList<PromptChoice> BoundarySunLuBanChoices(ProgramSkillFrame frame)
    {
        var use = GetProgramRoleCardUse(frame);
        if (frame.SkillId != BoundaryZenhui || frame.TriggerId != "own-choice-role" || frame.InstructionIndex != 2 ||
            frame.SelectedTargetSeats is not [var selected] || !GetProgramCardUseRoleTargets(frame).Contains(selected) ||
            use.SourceSeat != frame.OwnerSeat || _phase != TurnPhase.Play || _currentSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Mobile Zenhui requires its original sole-target Use, original owner and still-legal selected participant.");
        PromptChoice Choice(string option, string text) => new(new($"boundary-sun-lu-ban.frame-{frame.Id}.{option}"), text, [], [],
            new Dictionary<string, string> { ["program-action"] = BoundaryZenhuiAction, ["option-id"] = option });
        var choices = new List<PromptChoice>();
        if (GetHand(_players[selected]).Count + GetEquipment(_players[selected]).Count > 0)
            choices.Add(Choice("become-user", "获得其一张牌，然后令其代替你成为此牌使用者"));
        choices.Add(Choice("extra-target", "令其也成为此牌目标"));
        return Array.AsReadOnly(choices.ToArray());
    }

    private SkillProgramStepOutcome BeginBoundarySunLuBanChoice(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.ChoiceBindings.Any(binding => binding.Name == effect.ResultBind)) throw new InvalidOperationException("Mobile Zenhui cannot choose twice.");
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "谮毁：由你选择获得牌并更改使用者，或增加目标。", [], [], frame.OwnerSeat)
        { PromptId = CreatePromptId(), IsPrivate = true, TargetSeat = frame.OwnerSeat, Choices = BoundarySunLuBanChoices(frame), SkillPrompt = new(frame.SkillId, skill.Name, "谮毁 · 由你选择", skill.Description) };
        _status = _players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState(); return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer(BoundaryZenhuiAction)]
    private void AnswerBoundarySunLuBanChoice(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame frame || frame.SkillId != BoundaryZenhui ||
            _pendingDecision is not { IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat ||
            !AssistedChoicesEqual([selected], BoundarySunLuBanChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mobile Zenhui requires its exact published owner's choice on the original frame.");
        var use = GetProgramRoleCardUse(frame);
        var effect = GetPausedPrivateOfferEffect(frame)!;
        var option = selected.Parameters["option-id"];
        ClearPendingDecision();
        ReplaceRuntimeTop(frame with { ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, option, frame.OwnerSeat)).ToArray()) });
        AdvanceEventRulesAndQueueFact(new BoundarySunLuBanChoiceEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash,
            use.Id, use.Action!.ActionId, frame.SelectedTargetSeats.Single(), effect.ResultBind!, option));
        AdvanceRuntimeProgram(frame.Id);
    }

    private bool IsBoundarySunLuBanChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding) =>
        frame.SkillId == BoundaryZenhui && binding.ChooserSeat == frame.OwnerSeat && binding.OptionId is "become-user" or "extra-target" &&
        frame.WindowContext?.CardUse is { } context && frame.SelectedTargetSeats is [var target] &&
        ProgramEventHistory<BoundarySunLuBanChoiceEvent>().Count(fact => fact.FrameId == frame.Id && fact.OwnerSeat == frame.OwnerSeat &&
            fact.SkillInstanceId == frame.SkillInstanceId && fact.GameplayHash == frame.GameplayHash && fact.UseFrameId == context.ParentCardUseFrameId &&
            fact.ActionId == context.CardActionId && fact.SelectedSeat == target && fact.ResultBind == binding.Name && fact.OptionId == binding.OptionId) == 1 &&
        ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!).Instructions.Take(frame.InstructionIndex)
            .Count(effect => effect.Op == SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui && effect.ResultBind == binding.Name) == 1;

    [ProgramAiChoice(SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui)]
    private static PromptChoice BoundarySunLuBanChoiceAi(PendingDecision prompt) => prompt.Choices[0];

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 54100)]
    private void AssertBoundarySunLuBanChoice(ProgramSkillFrame frame)
    {
        if (frame.SkillId != BoundaryZenhui || frame.InstructionIndex != 2 || frame.ChoiceBindings.Any(binding => binding.Name == "own-role-choice")) return;
        if (!ReferenceEquals(frame, _resolutionStack.LastOrDefault()) || _pendingDecision is not { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } prompt ||
            prompt.PlayerSeat != frame.OwnerSeat || !AssistedChoicesEqual(prompt.Choices, BoundarySunLuBanChoices(frame)))
            throw new InvalidOperationException("Mobile Zenhui lost its exact owner-only branch chooser on the original card-use frame.");
    }
}
