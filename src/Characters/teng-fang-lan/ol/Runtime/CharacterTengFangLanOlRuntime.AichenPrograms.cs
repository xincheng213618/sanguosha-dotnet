#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTengFangLanOlRuntime
{
    internal HashSet<string> AichenRemovedOptions(int ownerSeat) => _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramAichenRemovedEvent>().Where(fact => fact.OwnerSeat == ownerSeat).Select(fact => fact.RemovedOptionId).ToHashSet(StringComparer.Ordinal);
    internal SkillProgramStepOutcome AichenProgramRemoveOption(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var removed = AichenRemovedOptions(active.OwnerSeat);
        var remaining = CharacterTengFangLanOlDefinitions.LuochongOptionIds.Where(optionId => !removed.Contains(optionId)).ToArray();
        if (remaining.Length <= 1)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = remaining.Select(optionId => new PromptChoice(new ChoiceId($"aichen-remove.frame-{(frame.Id)}.{(optionId)}"), $"移除“落宠”的选项：{(LuochongOptionDisplayName(optionId))}。", [], [], new Dictionary<string, string> { ["program-action"] = "aichen-remove", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["option"] = optionId })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】“落宠”选项数大于1，请选择移除其中一项。", [], [], active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 移除选项", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("aichen-remove")]
    internal void ResolveAichenRemoveChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Aichen choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.AichenRemoveOption } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Aichen choice does not match its suspended instruction.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (selected.Parameters.GetValueOrDefault("option")is not { } option || !IsLuochongOptionId(option) || AichenRemovedOptions(active.OwnerSeat).Contains(option))
            throw new InvalidOperationException("The Aichen removal option is no longer available.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var remainingCount = CharacterTengFangLanOlDefinitions.LuochongOptionIds.Length - AichenRemovedOptions(active.OwnerSeat).Count - 1;
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，移除“落宠”的选项：{(LuochongOptionDisplayName(option))}。", active.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAichenRemovedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.OwnerSeat, option, remainingCount));
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }
}
