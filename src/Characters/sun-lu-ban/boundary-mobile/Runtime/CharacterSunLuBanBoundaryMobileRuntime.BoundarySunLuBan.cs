#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunLuBanBoundaryMobileRuntime
{
    internal const string BoundaryZenhui = "boundary:zenhui-mobile", BoundaryZenhuiAction = "boundary-sun-lu-ban-choice";
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 54100)]
    [ProgramTargetWindow(ProgramTargetWindowStage.ImplicitSelfDrawTwo, 54100)]
    [ProgramTargetWindow(ProgramTargetWindowStage.NormalizeImplicitDrawTwo, 54100)]
    internal bool BoundarySunLuBanTargetWindow(ProgramTargetCardWindowQuery query) => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui);
    internal bool AllowsBoundarySunLuBanDrawTwoRoleTarget(CardUseFrame use) => use is { CardKind: CardKind.DrawTwo, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.DrawTwo } action } && use.Step != ResolutionFrameStep.Completed && use.SourceSeat == action.ActorSeat && action.EffectiveDesignatedTargetSeats is [var self] && self == action.ActorSeat && (action.EffectiveIsRed == false || action.EffectiveIsRed is null && action.EffectiveSuit is Suit.Spade or Suit.Club) && _coreActualTurnProgressCurrentSeatPhaseAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat == action.ActorSeat && ReferenceEquals(use, _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == use.Id)) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat]).Any(program => program.Id == BoundaryZenhui) && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<CardUseDeclaredEvent>().Any(declared => declared.ResolutionId == use.Id && declared.SourceSeat == action.ActorSeat && declared.CardId == use.CardId && declared.CardKind == CardKind.DrawTwo);
    SkillProgramStepOutcome IBoundarySunLuBanHost.Choose(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginBoundarySunLuBanChoice(frame, effect);
    internal IReadOnlyList<PromptChoice> BoundarySunLuBanChoices(ProgramSkillFrame frame)
    {
        var use = _coreProgramCardUseRolesOperations.GetProgramRoleCardUse(frame);
        if (frame.SkillId != BoundaryZenhui || frame.TriggerId != "own-choice-role" || frame.InstructionIndex != 2 || frame.SelectedTargetSeats is not [var selected] || !_coreProgramCardUseRolesOperations.GetProgramCardUseRoleTargets(frame).Contains(selected) || use.SourceSeat != frame.OwnerSeat || _coreActualTurnProgressCurrentSeatPhaseAccess._phase != TurnPhase.Play || _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat != frame.OwnerSeat)
            throw new InvalidOperationException("Mobile Zenhui requires its original sole-target Use, original owner and still-legal selected participant.");
        PromptChoice Choice(string option, string text) => new(new($"boundary-sun-lu-ban.frame-{(frame.Id)}.{(option)}"), text, [], [], new Dictionary<string, string> { ["program-action"] = BoundaryZenhuiAction, ["option-id"] = option });
        var choices = new List<PromptChoice>();
        if (_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[selected]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[selected]).Count > 0)
            choices.Add(Choice("become-user", "获得其一张牌，然后令其代替你成为此牌使用者"));
        choices.Add(Choice("extra-target", "令其也成为此牌目标"));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal SkillProgramStepOutcome BeginBoundarySunLuBanChoice(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.ChoiceBindings.Any(binding => binding.Name == effect.ResultBind))
            throw new InvalidOperationException("Mobile Zenhui cannot choose twice.");
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "谮毁：由你选择获得牌并更改使用者，或增加目标。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = frame.OwnerSeat,
            Choices = BoundarySunLuBanChoices(frame),
            SkillPrompt = new(frame.SkillId, skill.Name, "谮毁 · 由你选择", skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer(BoundaryZenhuiAction)]
    internal void AnswerBoundarySunLuBanChoice(PromptChoice selected)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.SkillId != BoundaryZenhui || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: true } prompt || prompt.PlayerSeat != frame.OwnerSeat || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], BoundarySunLuBanChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("Mobile Zenhui requires its exact published owner's choice on the original frame.");
        var use = _coreProgramCardUseRolesOperations.GetProgramRoleCardUse(frame);
        var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)!;
        var option = selected.Parameters["option-id"];
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, option, frame.OwnerSeat)).ToArray()) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundarySunLuBanChoiceEvent(frame.Id, frame.OwnerSeat, frame.SkillInstanceId, frame.GameplayHash, use.Id, use.Action!.ActionId, frame.SelectedTargetSeats.Single(), effect.ResultBind!, option));
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramAiChoice(SkillProgramEffectOp.ChooseBoundarySunLuBanZenhui)]
    internal static PromptChoice BoundarySunLuBanChoiceAi(PendingDecision prompt) => prompt.Choices[0];
}
