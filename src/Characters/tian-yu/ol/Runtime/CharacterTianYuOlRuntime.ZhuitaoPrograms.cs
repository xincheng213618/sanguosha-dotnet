#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuOlRuntime
{
    internal SkillProgramStepOutcome ZhuitaoProgramMarkTarget(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreProgramLifecycleOperations.CanInvokeProgramSkill(active))
            return SkillProgramStepOutcome.Continue;
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && player.Seat != active.OwnerSeat && ActiveDirectedDistanceGrants(active.OwnerSeat, player.Seat).Count == 0).Select(player => player.Seat).Order().ToArray();
        if (candidates.Length == 0)
            return SkillProgramStepOutcome.Continue;
        var presentation = _contentRegistryOperations._contentRegistry!.GetSkill(active.SkillId);
        var choices = candidates.Select(seat => new PromptChoice(new ChoiceId($"zhuitao-mark.frame-{(active.Id)}.seat-{(seat)}"), $"令你与 {(_coreMatchParticipantsPlayersAccess._players[seat].Name)} 的距离-1。", [], [seat], new Dictionary<string, string> { ["program-action"] = "zhuitao-mark", ["frame-id"] = active.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["target-seat"] = seat.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray();
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new PendingDecision(DecisionKind.ProgramTrigger, active.OwnerSeat, $"【{(presentation.Name)}】请选择一名未以此法减少距离的其他角色，你与你的目标距离-1。", [], candidates, active.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = false,
            TargetSeat = active.OwnerSeat,
            SkillPrompt = new SkillPromptPresentation(active.SkillId, presentation.Name, $"{(presentation.Name)} · 选择目标", presentation.Description),
            Choices = choices.AsReadOnly()
        };
        _coreMatchOutcomeStatusWinnerAccess._status = owner.IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("zhuitao-mark")]
    internal void ResolveZhuitaoMarkChoice(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("The Zhuitao choice lost its program frame.");
        var paused = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry!.GetSkill(frame.SkillId).Program!).GetPausedInstruction(frame.InstructionIndex);
        if (paused.Effect is not { Op: SkillProgramEffectOp.ZhuitaoMarkTarget } || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            throw new InvalidOperationException("The Zhuitao choice does not match its suspended instruction.");
        if (!int.TryParse(selected.Parameters.GetValueOrDefault("target-seat"), System.Globalization.CultureInfo.InvariantCulture, out var targetSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(targetSeat) || targetSeat == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[targetSeat].IsAlive || ActiveDirectedDistanceGrants(frame.OwnerSeat, targetSeat).Count > 0)
            throw new InvalidOperationException("The Zhuitao target is no longer legal.");
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDirectedDistanceGrantedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.SkillInstanceId, active.OwnerSeat, targetSeat, -1));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(_coreMatchParticipantsPlayersAccess._players[active.OwnerSeat].Name)} 发动【{(_contentRegistryOperations._contentRegistry.GetSkill(active.SkillId).Name)}】，" + $"令自己与 {(_coreMatchParticipantsPlayersAccess._players[targetSeat].Name)} 的距离-1。", active.OwnerSeat, targetSeat);
        _coreRuntimeOperations.AdvanceRuntimeProgram(active.Id);
    }

    internal SkillProgramStepOutcome ZhuitaoProgramRevoke(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || active.WindowContext?.TargetSeat is not { } damagedSeat || damagedSeat == active.OwnerSeat)
            return SkillProgramStepOutcome.Continue;
        var grants = ActiveDirectedDistanceGrants(active.OwnerSeat, damagedSeat);
        if (grants.Count == 0)
            return SkillProgramStepOutcome.Continue;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramDirectedDistanceRevokedEvent(active.Id, active.SkillId, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(active), active.SkillInstanceId, active.OwnerSeat, damagedSeat));
        _coreMatchLogOperations.AddLog("SkillEffect", $"{(owner.Name)} 对 {(_coreMatchParticipantsPlayersAccess._players[damagedSeat].Name)} 造成伤害，失去以此法对其减少的距离。", active.OwnerSeat, damagedSeat);
        return SkillProgramStepOutcome.Continue;
    }

    // 追讨 mark: price every candidate through the shared target scoring; the
    // hint charges one point of prospective harm so a hostile target wins.
    [ProgramAiChoice(SkillProgramEffectOp.ZhuitaoMarkTarget)]
    internal PromptChoice SelectAiZhuitaoMarkChoice(PendingDecision decision)
    {
        var view = _corePlayerViewProjectionOperations.CreateSnapshot(decision.PlayerSeat);
        var brain = _coreAiBrainsAiBrainsAccess._aiBrains[decision.PlayerSeat];
        return decision.Choices.Select(choice =>
        {
            var targetSeat = int.Parse(choice.Parameters.GetValueOrDefault("target-seat", "-1"), System.Globalization.CultureInfo.InvariantCulture);
            var hint = new SkillProgramAiHint(0, 0, 0, 0, 0, 1, false, false);
            return (Choice: choice, Score: brain.ScoreProgramTarget(view, targetSeat, hint));
        }).OrderByDescending(entry => entry.Score).ThenBy(entry => entry.Choice.Id.Value, StringComparer.Ordinal).First().Choice;
    }
}
