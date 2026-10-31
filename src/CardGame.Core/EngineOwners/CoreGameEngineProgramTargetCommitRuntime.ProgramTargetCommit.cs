#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramTargetCommitRuntime
{
    SkillProgramStepOutcome IProgramTargetCommitHost.SelectRelativeZoneDemandTarget(SkillProgramEffect effect, ProgramSkillFrame frame) => BeginRelativeZoneDemand(effect, frame);
    SkillProgramStepOutcome IProgramTargetCommitHost.DrawOnFirstProgramTargetEncounter(SkillProgramEffect effect, ProgramSkillFrame frame) => DrawFirstProgramTargetReward(effect, frame);
    internal bool ProgramTargetSourceValid(ProgramSkillFrame frame) => _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Any(item => item.Id == frame.SkillId);
    internal SkillProgramStepOutcome BeginRelativeZoneDemand(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var plan = ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!);
        var paid = plan.GetInstruction(frame.InstructionIndex - 2).Effect;
        var captured = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
        if (frame.TriggerId is not null || frame.InstructionIndex != 3 || paid is not { Op: SkillProgramEffectOp.MoveBoundCards, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true } || paid.SourceBind != effect.SourceBind || captured.CardIds.Count != 1 || captured.SourceLocations.Single()is not { Zone: CardZoneKind.Hand or CardZoneKind.Equipment, OwnerSeat: { } costOwner } || costOwner != frame.OwnerSeat || !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().SkipWhile(item => item is not ProgramSkillStartedEvent started || started.FrameId != frame.Id).OfType<CardMovedEvent>().Any(move => move.CardId == captured.CardIds[0] && move.From == captured.SourceLocations[0] && move.To == CardLocation.DiscardPile && move.Reason.Value == $"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.MoveBoundCards)}"))
            throw new InvalidOperationException("A relative-zone declaration requires its real, completed own HE discard parent.");
        if (!ProgramTargetSourceValid(frame))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "目标声明的技能实例已失效。");
            return SkillProgramStepOutcome.AwaitChild;
        }

        var owner = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat];
        var candidates = _coreMatchParticipantsPlayersAccess._players.Where(target => target.IsAlive && target.Seat != owner.Seat).SelectMany(target => new[] { "hand", "equipment" }.Where(mode => mode == "hand" ? _corePhysicalOwnedCardQueriesOperations.GetHand(target).Count > _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count : _corePhysicalOwnedCardQueriesOperations.GetEquipment(target).Count > _corePhysicalOwnedCardQueriesOperations.GetEquipment(owner).Count).Select(mode => new ProgramRelativeZoneCandidate(target.Seat, mode))).ToArray();
        if (candidates.Length == 0)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        frame = frame with
        {
            RelativeZoneDemand = new(effect.ResultBind!, Array.AsReadOnly(candidates))
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        PublishRelativeZoneTarget(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> RelativeZoneChoices(ProgramSkillFrame frame) => frame.RelativeZoneDemand!.Candidates.Select(candidate => new PromptChoice(new($"relative-zone.{(frame.Id)}.{(candidate.Mode)}.{(candidate.TargetSeat)}"), candidate.Mode == "hand" ? $"令 {(_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat].Name)} 交给你一张手牌或装备牌" : $"令 {(_coreMatchParticipantsPlayersAccess._players[candidate.TargetSeat].Name)} 弃置一张装备牌", [], [candidate.TargetSeat], new Dictionary<string, string> { ["program-action"] = "relative-zone-target", ["frame-id"] = frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture), ["mode"] = candidate.Mode })).ToArray();
    internal void PublishRelativeZoneTarget(ProgramSkillFrame frame)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "弃牌已结算，选择当前牌数严格大于你的角色及效果。", [], [], frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            TargetSeat = frame.OwnerSeat,
            Choices = RelativeZoneChoices(frame),
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("relative-zone-target")]
    internal void ResolveRelativeZoneTarget(PromptChoice selected)
    {
        var frame = _resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame ?? throw new InvalidOperationException("Missing real relative-zone parent.");
        var draft = frame.RelativeZoneDemand ?? throw new InvalidOperationException("Missing relative-zone selection.");
        if (draft.Commit is not null || !CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([selected], RelativeZoneChoices(frame).Where(choice => choice.Id == selected.Id).ToArray()))
            throw new InvalidOperationException("The relative-zone declaration no longer matches its published parent.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (!ProgramTargetSourceValid(frame))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "目标声明的技能实例已失效。");
            return;
        }

        var target = selected.Targets.Single();
        var mode = selected.Parameters["mode"];
        if (!_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(frame, completed: false);
            return;
        }

        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramTargetCommittedEvent>().Select(item => item.Declaration).Where(item => item.OwnerSeat == frame.OwnerSeat && item.SourceProgramId == frame.SkillId && item.SourceActivationId == frame.ActivationId).ToArray();
        var commit = new ProgramTargetCommitContext(frame.Id, frame.OwnerSeat, frame.SkillId, frame.ActivationId, frame.SkillInstanceId, target, mode, history.Length + 1, !history.Any(item => item.TargetSeat == target));
        frame = frame with
        {
            SelectedTargetSeats = Array.AsReadOnly(new[] { target }),
            ChoiceBindings = Array.AsReadOnly(frame.ChoiceBindings.Append(new(draft.ResultBind, mode, frame.OwnerSeat)).ToArray()),
            RelativeZoneDemand = draft with
            {
                Commit = commit
            }
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(frame);
        // Record before reward eligibility: suppression and later loss/regrant do not erase an encounter.
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramTargetCommittedEvent(commit));
        var facts = _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]);
        var candidates = _coreProgramLifecycleOperations.CollectEligibleProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], SkillProgramTriggerWindow.ProgramTargetCommitted, facts);
        if (candidates.Count == 0)
        {
            _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
            return;
        }

        _coreRuntimeOperations.PushRuntimeFrame(new ProgramLifecycleTriggerWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, frame.OwnerSeat, SkillProgramTriggerWindow.ProgramTargetCommitted, candidates, ProgramLifecycleContinuation.ResumeParentProgram, facts) { ResumeProgramFrameId = frame.Id, ProgramTarget = commit });
        _coreRuntimeOperations.AdvanceRuntimeTop<ProgramLifecycleTriggerWindowFrame>();
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 1010)]
    internal bool TryResumeProgramTargetCommit(long id)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        if (frame.RelativeZoneDemand?.Commit is null)
            return false;
        if (!ProgramTargetSourceValid(frame))
        {
            _coreProgramLifecycleOperations.CancelProgramBindingAndCleanup(frame, "目标声明后技能实例或拥有者已失效。");
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { RelativeZoneDemand = null });
        return false;
    }

    internal bool IsProgramTargetContextValid(ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.ProgramTargetCommitted || context.ProgramTarget is not { } commit || context.OwnerSeat != commit.OwnerSeat || context.SourceSeat != commit.OwnerSeat || context.TargetSeat != commit.TargetSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramLifecycleTriggerWindowFrame>().SingleOrDefault(item => item.Id == context.ParentFrameId)is not { } window || window.Id != context.ParentFrameId || window.Window != context.Window || window.ProgramTarget != commit || window.Continuation != ProgramLifecycleContinuation.ResumeParentProgram || window.ResumeProgramFrameId != commit.ParentProgramFrameId || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(item => item.Id == commit.ParentProgramFrameId)is not { } parent || parent.SkillId != commit.SourceProgramId || parent.ActivationId != commit.SourceActivationId || parent.SkillInstanceId != commit.SourceSkillInstanceId || parent.OwnerSeat != commit.OwnerSeat || parent.RelativeZoneDemand?.Commit != commit || parent.InstructionIndex != 3 || !parent.SelectedTargetSeats.SequenceEqual([commit.TargetSeat]))
            return false;
        var declarations = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramTargetCommittedEvent>().Select(item => item.Declaration).Where(item => item.OwnerSeat == commit.OwnerSeat && item.SourceProgramId == commit.SourceProgramId && item.SourceActivationId == commit.SourceActivationId).ToArray();
        return declarations.Count(item => item == commit) == 1 && declarations.Count(item => item.DeclarationOrdinal < commit.DeclarationOrdinal) == commit.DeclarationOrdinal - 1 && commit.FirstEncounter == !declarations.Where(item => item.DeclarationOrdinal < commit.DeclarationOrdinal).Any(item => item.TargetSeat == commit.TargetSeat);
    }

    internal SkillProgramStepOutcome DrawFirstProgramTargetReward(SkillProgramEffect effect, ProgramSkillFrame frame)
    {
        var context = frame.WindowContext ?? throw new InvalidOperationException("First-target reward requires an actual declaration window.");
        if (!IsProgramTargetContextValid(context))
            throw new InvalidOperationException("First-target reward lost its exact real parent and declaration ordinal.");
        var commit = context.ProgramTarget!;
        if (!commit.FirstEncounter || commit.SourceProgramId != effect.SkillIds.Single() || commit.SourceActivationId != effect.StateId || !ProgramTargetSourceValid(frame))
            return SkillProgramStepOutcome.Continue;
        _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, effect.Amount, null, null, SkillProgramCardSetVisibility.Private, new("program.first-target.reward"));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }
}
