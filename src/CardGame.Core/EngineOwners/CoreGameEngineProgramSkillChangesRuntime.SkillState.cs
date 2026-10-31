namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineProgramSkillChangesRuntime : ISkillStateProgramHost
{
    SkillProgramStepOutcome ISkillStateProgramHost.ChangeSkillState(ProgramSkillFrame frame, SkillProgramEffect effect, int targetSeat)
    {
        var seat = effect.TargetReference is { } reference ? ResolveSkillStateParticipant(frame, reference) : targetSeat;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat))
        {
            if (effect.ResultBind is { } bind) _stateExpressions.CaptureProgramExpressionNumber(frame.Id, bind, 0);
            return SkillProgramStepOutcome.Continue;
        }
        var spec = effect.SkillState!;
        var grants = SkillStateCandidates(frame, spec, seat);
        var options = grants.GroupBy(grant => grant.SkillId, StringComparer.Ordinal)
            .Select(group => new ProgramSkillStateOption(group.Key, Array.AsReadOnly(group.Select(grant => grant.GrantId).ToArray())))
            .ToArray();
        if (spec.ChooseOne && options.Length > 0)
        {
            _stateRuntime.ReplaceRuntimeTop(frame with { SkillStateChoice = new(seat, Array.AsReadOnly(options)) });
            _statePrompt._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "选择一个技能。", [], [], frame.OwnerSeat)
            {
                PromptId = _statePromptBoundary.CreatePromptId(),
                IsPrivate = true,
                Choices = Array.AsReadOnly(options.Select(option => new PromptChoice(new($"skill-state:{frame.Id}:{option.SkillId}"),
                    _contentRegistryOperations._contentRegistry.GetSkill(option.SkillId).Name, [], [seat],
                    new Dictionary<string, string> { ["program-action"] = "skill-state", ["skill"] = option.SkillId })).ToArray())
            };
            _stateOutcome._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return SkillProgramStepOutcome.AwaitChoice;
        }
        ApplyProgramSkillState(frame, seat, spec, effect.ResultBind);
        return SkillProgramStepOutcome.Continue;
    }

    private SkillGrant[] SkillStateCandidates(ProgramSkillFrame frame, SkillStateSpec spec, int seat)
    {
        // Selection is limited to information already available to the chooser.
        var visible = spec.ChooseOne
            ? _stateViews.Value.CreateSnapshot(frame.OwnerSeat).Players[seat] : null;
        var visibleSkills = visible is null ? null : (visible.Skills ?? []).Concat(visible.SecondarySkills ?? [])
            .Select(skill => skill.ContentId).ToHashSet(StringComparer.Ordinal);
        return ProgramSkillChanges.Candidates(seat, spec.Match, frame.SkillId, _stateWorld.CreateProgramExpressionWorld(frame))
            .Where(g => visibleSkills is null || visibleSkills.Contains(g.SkillId))
            .ToArray();
    }

    internal void ApplyProgramSkillState(ProgramSkillFrame frame, int seat, SkillStateSpec spec, string? resultBind = null)
    {
        var count = ProgramSkillChanges.ChangeState(frame, seat, spec, _stateWorld.CreateProgramExpressionWorld(frame));
        if (resultBind is not null) _stateExpressions.CaptureProgramExpressionNumber(frame.Id, resultBind, count);
    }

    private int ResolveSkillStateParticipant(ProgramSkillFrame frame, ProgramParticipantReference reference) =>
        reference.Kind == ProgramParticipantRef.EventSource ? frame.WindowContext?.SourceSeat ?? -1 :
            _stateParticipants.ResolveProgramParticipant(frame, reference);

    [ProgramAnswer("skill-state")]
    internal void AnswerSkillState(PromptChoice choice)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(_stateFrames._resolutionStack.OfType<ProgramSkillFrame>().Last().Id);
        var pending = frame.SkillStateChoice!;
        var effect = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        var skill = choice.Parameters["skill"];
        var option = pending.Options.SingleOrDefault(option => option.SkillId == skill);
        var selected = SkillStateCandidates(frame, effect.SkillState!, pending.Seat)
            .Where(grant => grant.SkillId == skill && option?.GrantIds.Contains(grant.GrantId, StringComparer.Ordinal) == true).ToArray();
        if (option is null || selected.Length == 0)
            throw new InvalidOperationException("The selected skill is no longer available to this chooser.");
        _statePromptBoundary.ClearPendingDecision();
        _stateRuntime.ReplaceRuntimeTop(frame = frame with { SkillStateChoice = null });
        var spec = effect.SkillState!;
        ApplyProgramSkillState(frame, pending.Seat,
            new(spec.Action, spec.Match.Freeze(selected), spec.Until, spec.While), effect.ResultBind);
        _stateRuntime.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.SkillState)]
    internal PromptChoice SkillStateAi(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices[0];
}
