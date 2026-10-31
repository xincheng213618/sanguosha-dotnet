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
        var skills = SkillStateCandidates(frame, effect, seat);
        if (spec.Scope == SkillStateScope.ChosenFromTarget && skills.Length > 0)
        {
            _stateRuntime.ReplaceRuntimeTop(frame with { SkillStateChoice = new(seat, Array.AsReadOnly(skills)) });
            _statePrompt._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "选择一个技能。", [], [], frame.OwnerSeat)
            {
                PromptId = _statePromptBoundary.CreatePromptId(),
                IsPrivate = true,
                Choices = Array.AsReadOnly(skills.Select(id => new PromptChoice(new($"skill-state:{frame.Id}:{id}"),
                    _contentRegistryOperations._contentRegistry.GetSkill(id).Name, [], [seat],
                    new Dictionary<string, string> { ["program-action"] = "skill-state", ["skill"] = id })).ToArray())
            };
            _stateOutcome._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
            return SkillProgramStepOutcome.AwaitChoice;
        }
        ApplyProgramSkillState(frame, seat, spec, skills, effect.ResultBind);
        return SkillProgramStepOutcome.Continue;
    }

    private string[] SkillStateCandidates(ProgramSkillFrame frame, SkillProgramEffect effect, int seat)
    {
        var spec = effect.SkillState!;
        var player = _coreMatchParticipantsPlayersAccess._players[seat];
        if (spec is { Action: SkillStateAction.Suppress, Scope: SkillStateScope.Named })
            return effect.SkillIds.ToArray();
        // Selection is limited to information already available to the chooser.
        var visible = spec.Scope == SkillStateScope.ChosenFromTarget
            ? _stateViews.Value.CreateSnapshot(frame.OwnerSeat).Players[seat] : null;
        var visibleSkills = visible is null ? null : (visible.Skills ?? []).Concat(visible.SecondarySkills ?? [])
            .Select(skill => skill.ContentId).ToHashSet(StringComparer.Ordinal);
        return player.SkillGrants.Grants.Where(g => g.IsEnabled && !g.IsModeRule &&
            (!spec.CharacterOnly || SkillStateQuery.IsCharacter(g)) &&
            g.SkillState is not { Acquisition: false } && (!spec.PrintedOnly || SkillStateQuery.IsPrinted(g)))
            .Where(g => (!spec.ExcludeLocked && spec.Scope != SkillStateScope.NonLocked ||
                !_contentRegistryOperations._contentRegistry.GetSkill(g.SkillId).Tags.HasFlag(SkillTag.Locked)) &&
                (spec.Scope != SkillStateScope.Named || effect.SkillIds.Contains(g.SkillId)) &&
                (spec.Filter is null || ProgramExpressions.EvaluateBool(spec.Filter,
                    new SkillFilterWorld(_stateWorld.CreateProgramExpressionWorld(frame), player, g,
                        _contentRegistryOperations._contentRegistry.GetSkill(g.SkillId))) == true))
            .Where(g => visibleSkills is null || visibleSkills.Contains(g.SkillId))
            .Select(g => g.SkillId).Distinct(StringComparer.Ordinal).ToArray();
    }

    internal void ApplyProgramSkillState(ProgramSkillFrame frame, int seat, SkillStateSpec spec, IReadOnlyList<string> skills, string? resultBind = null)
    {
        var end = spec.UntilTurnEndOf is { } reference ? ResolveSkillStateParticipant(frame, reference) : (int?)null;
        var count = ProgramSkillChanges.ChangeState(frame, seat, spec, skills, end);
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
        if (!pending.Skills.Contains(skill) || !SkillStateCandidates(frame, effect, pending.Seat).Contains(skill))
            throw new InvalidOperationException("The selected skill is no longer available to this chooser.");
        _statePromptBoundary.ClearPendingDecision();
        _stateRuntime.ReplaceRuntimeTop(frame = frame with { SkillStateChoice = null });
        ApplyProgramSkillState(frame, pending.Seat, effect.SkillState!, [skill], effect.ResultBind);
        _stateRuntime.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramAiChoice(SkillProgramEffectOp.SkillState)]
    internal PromptChoice SkillStateAi(PendingDecision decision, ProgramSkillFrame frame) => decision.Choices[0];
}

internal sealed class SkillFilterWorld(IProgramExpressionWorld world, CharacterState player, SkillGrant grant,
    ContentSkillDefinition skill) : IProgramExpressionWorld
{
    public int? Participant(string name) => world.Participant(name);
    public IReadOnlyList<int>? ParticipantSet(string name) => world.ParticipantSet(name);
    public int? Stat(string stat, int seat) => world.Stat(stat, seat);
    public bool? Flag(string flag, int seat) => world.Flag(flag, seat);
    public int? BindingStat(string stat, string name) => world.BindingStat(stat, name);
    public int? Number(string name) => name switch
    {
        "skill.locked" => skill.Tags.HasFlag(SkillTag.Locked) ? 1 : 0,
        "skill.limited" => skill.Tags.HasFlag(SkillTag.Limited) ? 1 : 0,
        "skill.awakening" => skill.Tags.HasFlag(SkillTag.Awakening) ? 1 : 0,
        "skill.lord" => skill.Tags.HasFlag(SkillTag.Lord) ? 1 : 0,
        "skill.printed" => SkillStateQuery.IsPrinted(grant) ? 1 : 0,
        "skill.ordinal" => Array.FindIndex((grant.SourceId == CharacterState.SecondarySkillSource ? player.SecondaryGeneral?.Skills ?? [] : player.General.Skills)
            .ToArray(), s => s.ContentId == grant.SkillId) + 1,
        _ => world.Number(name)
    };
}
