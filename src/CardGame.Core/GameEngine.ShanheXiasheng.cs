namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheXiashengHost(GameEngine engine) : IShanheXiashengHost
    { public SkillProgramStepOutcome ResolveXiasheng(ProgramSkillFrame frame, string stage) => engine.BeginShanheXiasheng(frame, stage); }
    [ProgramTargetWindow(ProgramTargetWindowStage.FinalizedInstantTrickRequirement, 5408)]
    private bool ShanheXiashengRequiresTargetWindow(ProgramTargetCardWindowQuery query) =>
        _contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveShanheXiasheng);

    private (int Red, int Black) ShanheXiashengColors(int seat)
    {
        var hand = GetHand(_players[seat]);
        return (hand.Count(card => card.Suit is Suit.Heart or Suit.Diamond), hand.Count(card => card.Suit is Suit.Spade or Suit.Club));
    }
    private CardActionContext? ShanheXiashengAction(int owner, ProgramSkillWindowContext context) =>
        context.CardUse is { } identity && identity.ActorSeat == owner &&
        _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(window => window.Id == context.ParentFrameId) is { } window &&
        window.Action.ActionId == identity.CardActionId && window.Action.ActorSeat == owner &&
        (window.Action.Type == CardActionType.Use || window.Action.Type == CardActionType.Response &&
            window.Action.EffectiveKind is CardKind.Dodge or CardKind.Nullification) ? window.Action : null;

    private IReadOnlyList<IReadOnlyList<int>> ShanheXiashengExtraTargets(CardUseFrame use) =>
        DesignatedExtraTargetCandidates(use).Where(targets => IsSlashCard(use.CardKind)
            ? CanBeRecipientCategorySlashTarget(use, _players[targets[0]])
            : use.CardKind != CardKind.Snatch || CanBeProgramCardUseRoleTarget(use, _players[targets[0]])).ToArray();

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 5408)]
    private bool CanOfferShanheXiasheng(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (!trigger.Effects.Any(effect => effect.Op == SkillProgramEffectOp.ResolveShanheXiasheng)) return true;
        if (candidate.SkillId != "shanhe:xiasheng-3362" || ShanheXiashengAction(candidate.OwnerSeat, context) is not { EffectiveIsRed: { } red } action) return false;
        var colors = ShanheXiashengColors(candidate.OwnerSeat);
        if (trigger.Effects[0].StateId == "draw") return !red && colors.Red > colors.Black;
        return red && colors.Black > colors.Red && TryGetDesignatedExtraTargetUse(candidate.OwnerSeat, context, out var use, out _) &&
            ShanheXiashengExtraTargets(use).Count > 0;
    }

    private SkillProgramStepOutcome BeginShanheXiasheng(ProgramSkillFrame supplied, string stage)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheXiasheng is not null || frame.WindowContext is not { } context ||
            ShanheXiashengAction(frame.OwnerSeat, context) is not { } action)
            throw new InvalidOperationException("Xiasheng requires its original current-use frame.");
        var colors = ShanheXiashengColors(frame.OwnerSeat);
        ReplaceRuntimeTop(frame = frame with { ShanheXiasheng = new(context.ParentFrameId, action.ActionId, stage, colors.Red, colors.Black) });
        AdvanceEventRulesAndQueueFact(new ShanheXiashengBranchIssuedEvent(frame.Id, frame.OwnerSeat, action.ActionId, stage, colors.Red, colors.Black));
        if (stage == "draw") return BeginOperationProgramDraw(frame, frame.OwnerSeat, 1, new("skill-program.shanhe-xiasheng.draw"));
        if (!TryGetDesignatedExtraTargetUse(frame.OwnerSeat, context, out var use, out _)) throw new InvalidOperationException("Xiasheng lost its actual designation parent.");
        if (use.CardKind == CardKind.DrawTwo && use.Action!.EffectiveDesignatedTargetSeats.Count == 0)
        {
            var targets = new[] { use.SourceSeat }; var normalized = CloneDesignatedExtraTargetAction(use.Action, targets);
            UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = normalized }, normalized);
            use = LifecycleCardUse(use.Id)!;
        }
        Dictionary<string, string> Parameters() => new() { ["program-action"] = "shanhe-xiasheng" };
        var choices = ShanheXiashengExtraTargets(use).Select(targets => new PromptChoice(new($"xiasheng.{frame.Id}.{string.Join(".", targets)}"),
            "夏晟：额外指定" + string.Join("、", targets.Select(seat => _players[seat].Name)), [], targets, Parameters()))
            .Append(new(new($"xiasheng.{frame.Id}.decline"), "不追加目标", [], [], Parameters())).ToArray();
        ReplaceRuntimeTop(frame = frame with { ShanheXiasheng = frame.ShanheXiasheng! with { Choices = choices, OriginalTargets = use.TargetSeats }, Step = ResolutionFrameStep.AwaitingResponse });
        AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetOfferedEvent(frame.Id, use.Id, action.ActionId, frame.InstructionIndex,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            frame.ShanheXiasheng.OriginalTargets, choices.SelectMany(choice => choice.Targets.Take(1)).Distinct().ToArray()));
        var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, "夏晟：可为此红色牌额外指定一个合法目标", [],
            choices.SelectMany(choice => choice.Targets).Distinct().ToArray(), frame.OwnerSeat)
        { PromptId = CreatePromptId(), Choices = frame.ShanheXiasheng.Choices, IsPrivate = false, SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description) };
        _status = _players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        AdvanceRulesAndPublishState(); return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("shanhe-xiasheng")]
    private void AnswerShanheXiasheng(PromptChoice selected)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheXiasheng: { Stage: "target" } receipt } frame ||
            _pendingDecision?.PlayerSeat != frame.OwnerSeat || frame.WindowContext?.ParentFrameId != receipt.ParentWindowId ||
            receipt.Choices.SingleOrDefault(choice => choice.Id == selected.Id) is not { } choice || !AssistedChoicesEqual([choice], [selected]) ||
            !TryGetDesignatedExtraTargetUse(frame.OwnerSeat, frame.WindowContext, out var use, out var window) || use.Action!.ActionId != receipt.ActionId)
            throw new InvalidOperationException("Xiasheng lost its exact original red use or issued legal target choice.");
        if (choice.Targets.Count > 0)
        {
            if (!ShanheXiashengExtraTargets(use).Any(targets => targets.SequenceEqual(choice.Targets))) throw new InvalidOperationException("Xiasheng's additional target became illegal.");
            var targets = use.TargetSeats.Concat(choice.Targets).ToArray(); var action = CloneDesignatedExtraTargetAction(use.Action!, targets);
            var simple = use.AdjustedSimpleContinuation;
            if (window.Continuation == ProgramCardContinuation.FinalizedSimpleCard)
                simple ??= window.SimpleContinuation ?? throw new InvalidOperationException("Xiasheng lost its native basic-card continuation.");
            UpdateProgramRoleCardUse(use with { TargetSeats = targets, Action = action, AdjustedSimpleContinuation = simple,
                TargetsAdjusted = true, Enhancements = use.Enhancements | CurrentCardEnhancement.ExtraTarget }, action);
            AdvanceEventRulesAndQueueFact(new ProgramCardUseTargetAddedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, use.Id, choice.Targets[0]));
        }
        var result = LifecycleCardUse(use.Id)!.TargetSeats;
        AdvanceEventRulesAndQueueFact(new DesignatedExtraTargetResolvedEvent(frame.Id, use.Id, receipt.ActionId, frame.InstructionIndex,
            new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash,
            receipt.OriginalTargets, choice.Targets, result));
        ClearPendingDecision(); ReplaceRuntimeTop(frame with { ShanheXiasheng = null }); AdvanceRuntimeProgram(frame.Id);
    }

    private bool IsShanheXiashengTargetFact(CardUseFrame use, DesignatedExtraTargetResolvedEvent fact, IReadOnlyList<int> originalTargets)
    {
        if (fact.Source.SkillId != "shanhe:xiasheng-3362" || use.Action is not { Type: CardActionType.Use } action ||
            fact.CardUseFrameId != use.Id || fact.ActionId != action.ActionId || fact.Source.OwnerSeat != action.ActorSeat ||
            fact.InstructionIndex != 1 || !fact.OriginalTargetSeats.SequenceEqual(originalTargets) ||
            fact.AddedTargetSeats.Count != (use.CardKind == CardKind.BorrowedSword ? 2 : 1) ||
            !fact.ResultTargetSeats.SequenceEqual(originalTargets.Concat(fact.AddedTargetSeats)) ||
            _contentRegistry.GetSkill(fact.Source.SkillId).Program is not { } program || program.GameplayHash != fact.GameplayHash ||
            program.Triggers.SingleOrDefault(trigger => trigger.Id == fact.Source.BindingId)?.Effects is not
                [{ Op: SkillProgramEffectOp.ResolveShanheXiasheng, StateId: "target" }]) return false;
        return ProgramEventHistory<ShanheXiashengBranchIssuedEvent>().Any(branch => branch.FrameId == fact.ProgramFrameId &&
                branch.ActionId == fact.ActionId && branch.OwnerSeat == action.ActorSeat && branch.Branch == "target" && branch.BlackCount > branch.RedCount) &&
            ProgramEventHistory<DesignatedExtraTargetResolvedEvent>().Count(item => item.ProgramFrameId == fact.ProgramFrameId) == 1 &&
            ProgramEventHistory<DesignatedExtraTargetOfferedEvent>().Count(item => item.ProgramFrameId == fact.ProgramFrameId &&
                item.CardUseFrameId == use.Id && item.ActionId == fact.ActionId && item.Source == fact.Source &&
                item.GameplayHash == fact.GameplayHash && item.OriginalTargetSeats.SequenceEqual(originalTargets) &&
                item.CandidateTargetSeats.Contains(fact.AddedTargetSeats[0])) == 1 &&
            ProgramEventHistory<ProgramCardUseTargetAddedEvent>().Count(item => item.FrameId == fact.ProgramFrameId &&
                item.CardUseFrameId == use.Id && item.OwnerSeat == action.ActorSeat && item.SkillId == fact.Source.SkillId &&
                item.TargetSeat == fact.AddedTargetSeats[0]) == 1;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveShanheXiasheng)]
    private void ReturnShanheXiashengDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (frame.ShanheXiasheng is not { Stage: "draw", DrawOutcome: null } receipt ||
            outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != 1 || frame.NativeDraw is not null)
            throw new InvalidOperationException("Xiasheng lost its once-issued black-card draw.");
        ReplaceRuntimeTop(frame = frame with { ShanheXiasheng = receipt with { Stage = "draw-children", DrawOutcome = outcome } });
        if (AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat) == SkillProgramStepOutcome.Continue) AdvanceRuntimeProgram(frame.Id);
    }
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 5408)]
    private bool IsShanheXiashengDrawMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement) =>
        effect?.Op == SkillProgramEffectOp.ResolveShanheXiasheng && frame.ShanheXiasheng is { Stage: "draw-children", DrawOutcome: not null } &&
        movement.SubjectSeat == frame.OwnerSeat && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Resume, 5408)]
    private bool ResumeShanheXiasheng(long frameId)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ShanheXiasheng: { Stage: "draw-children" } } frame || frame.Id != frameId) return false;
        ReplaceRuntimeTop(frame with { ShanheXiasheng = null, PendingMovementContinuation = null }); AdvanceRuntimeProgram(frame.Id); return true;
    }
    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheXiasheng)]
    private PromptChoice AiShanheXiasheng(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices
        .OrderByDescending(choice => choice.Targets.Count == 0 ? 0 : choice.Targets.Sum(seat =>
            OtherRevealedPublicTeamAlly(_players[frame.OwnerSeat], _players[seat]) ? -1 : 2)).First();
}
