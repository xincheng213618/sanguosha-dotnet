namespace CardGame.Core;

/// <summary>Drives the lifecycle candidate cursor and dispatches its typed native returns.</summary>
[ProgramRuntimeModule]
internal sealed class ProgramLifecycleWindowRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramWindowCursors cursors, IProgramBindingActions bindings,
    IProgramBindingFacts bindingFacts, IProgramLifecycleFacts facts, IProgramLifecycleContexts contexts,
    IProgramLifecycleReturns returns, IActualPhaseEndOperations phaseEnds)
{
    private readonly IProgramRuntimeFrames _frames = frames;
    private readonly IProgramRuntimeEvents _events = events;
    private readonly IProgramBindingActions _bindings = bindings;
    private readonly IProgramBindingFacts _bindingFacts = bindingFacts;

    [NativeFrameRoute(typeof(ProgramLifecycleTriggerWindowFrame), 310)]
    internal void ContinueWindow() => ProgramTriggerWindowDriver.Run(new WindowHost(this));

    internal bool TryReadPending(out ProgramTriggerWindowEntry entry)
    {
        if (_frames.Top is ProgramLifecycleTriggerWindowFrame frame)
        {
            var candidate = frame.Candidates[frame.CandidateIndex];
            entry = new(candidate, CreateContext(frame, candidate, pending: true));
            return true;
        }
        entry = default;
        return false;
    }

    internal bool TrySkip(ProgramTriggerCandidate candidate)
    {
        if (_frames.Top is not ProgramLifecycleTriggerWindowFrame frame) return false;
        // Lifecycle skips historically consume the current cursor, rather than
        // comparing the supplied candidate. Preserve that parent contract.
        AdvanceCandidate(frame, activated: false, completed: false);
        _frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
        return true;
    }

    internal bool TryResolveGroup(ProgramTriggerCandidate first, ProgramSkillWindowContext context,
        PromptChoice selected, string? action)
    {
        if (_frames.Top is not ProgramLifecycleTriggerWindowFrame frame ||
            _bindingFacts.Trigger(first).ChoiceGroup is not { } group) return false;
        ProgramTriggerWindowDriver.ResolveChoiceGroup(new WindowHost(this), frame, first, context,
            group, selected, action, prompts.Clear);
        return true;
    }

    internal void AdvanceCandidate(ProgramLifecycleTriggerWindowFrame frame, bool activated, bool completed)
    {
        var candidate = frame.Candidates[frame.CandidateIndex];
        _events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId,
            candidate.SkillInstanceId, candidate.OwnerSeat, frame.Window, activated, completed));
        cursors.AdvanceProgramLifecycleCursor(frame);
    }

    internal ProgramSkillWindowContext CreateContext(ProgramLifecycleTriggerWindowFrame frame,
        ProgramTriggerCandidate candidate, bool pending)
    {
        if (pending && frame.Window == SkillProgramTriggerWindow.IdentityGeneralAppeared)
            return contexts.IdentityAppearance(frame, candidate);
        var triggerFacts = frame.ParticipantFacts?.GetValueOrDefault(candidate.OwnerSeat) ?? frame.Facts;
        if (!pending &&
            (frame.Window == SkillProgramTriggerWindow.DrawPhaseStarting && facts.HasAttributedEventOperations ||
             frame.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && facts.HasAttributedEventOperations &&
             !LiushiEffectContract.Clears(_bindingFacts.Trigger(candidate))))
            triggerFacts = triggerFacts with { EventTargetMarkerCounts = facts.MarkerCounts(frame.OwnerSeat) };
        var context = new ProgramSkillWindowContext(frame.Window, frame.Id, candidate.OwnerSeat,
            SourceSeat: !pending && frame.ResumeDyingFrameId is not null ? facts.DyingKillerSeat : frame.OwnerSeat,
            TargetSeat: frame.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: triggerFacts);
        if (pending) context = context with
        {
            ProgramTarget = frame.ProgramTarget,
            BeforeTurnPreludeProof = frame.BeforeTurnPreludeProof,
            SourceSeat = frame.ProgramTarget?.OwnerSeat ?? frame.OwnerSeat,
            TargetSeat = frame.ProgramTarget?.TargetSeat ?? frame.OwnerSeat
        };
        if (contexts.IsIssuedDiciChainTrigger(_bindingFacts.Trigger(candidate)))
            context = contexts.IssuedDiciChain(frame, candidate, context);
        if (!pending && frame.Window == SkillProgramTriggerWindow.IdentityGeneralAppeared)
            context = contexts.IdentityAppearance(frame, candidate);
        if (!pending) context = context with { BeforeTurnPreludeProof = frame.BeforeTurnPreludeProof };
        context = contexts.QingzhongEnding(frame, candidate, context);
        context = contexts.WeiluEnding(frame, candidate, context);
        context = contexts.XuRongDying(context);
        context = contexts.ZhuijiEnding(frame, candidate, context);
        if (!pending)
        {
            context = contexts.PhaseNamePrediction(candidate, context);
            if (frame.ProgramTarget is { } commit)
                context = context with { SourceSeat = commit.OwnerSeat, TargetSeat = commit.TargetSeat, ProgramTarget = commit };
        }
        return contexts.NativeDraw(frame, context with { ActualPhaseEnd = frame.ActualPhaseEnd });
    }

    internal void CompleteWindow(ProgramLifecycleTriggerWindowFrame frame)
    {
        _frames.Pop(frame.Id, ResolutionFrameKind.ProgramLifecycleTriggerWindow);
        if (frame.ResumeDrawPhaseObligationFrameId is not null)
        { returns.DrawPhaseObligation(frame); return; }
        if (returns.PipelineCompleted(frame)) return;
        switch (frame.Continuation)
        {
            case ProgramLifecycleContinuation.ResumeHellAppearance:
                if (!facts.IsExactHellAppearanceWindow(frame))
                    throw new InvalidOperationException("Hell arrival lost its exact appearance receipt.");
                _events.AdvanceFact(new HellAppearanceCompletedEvent(frame.HellAppearanceId!.Value));
                break;
            case ProgramLifecycleContinuation.ResumeNativeDrawAttempt:
                returns.NativeDrawAttempt(frame); break;
            case ProgramLifecycleContinuation.CompleteRoundEndingBoundary:
                returns.RoundEnding(frame); break;
            case ProgramLifecycleContinuation.ResumeActualPhaseEnd:
                phaseEnds.ReturnActualPhaseEndWindow(frame); break;
            case ProgramLifecycleContinuation.ResumeYanPuDyingExit:
                returns.YanPuDyingExit(frame); break;
            case ProgramLifecycleContinuation.ResumeAfterDeferredPrivateOffers:
                returns.AfterForeignActualContests(frame.OwnerSeat); break;
            case ProgramLifecycleContinuation.ResumeDyingEntry:
                returns.DyingEntry(frame); break;
            case ProgramLifecycleContinuation.ResumeCardDeclaration:
                if (_frames.Top is not CardDeclarationFrame declaration || declaration.Id != frame.ResumeProgramFrameId ||
                    declaration.Stage != CardDeclarationStage.Granting)
                    throw new InvalidOperationException("A declaration grant lost its exact parent.");
                _frames.Advance(declaration.Id); break;
            case ProgramLifecycleContinuation.ResumeParentProgram:
                _frames.AdvanceProgram(frame.ResumeProgramFrameId!.Value); break;
            case ProgramLifecycleContinuation.CompleteGameStarting:
                break;
            case ProgramLifecycleContinuation.MouGuoHuaiActualStart:
                returns.AfterHengye(frame.OwnerSeat); break;
            case ProgramLifecycleContinuation.NormalTurnStart:
                if (returns.ZhouQunPaidPreparation(frame)) break;
                if (returns.DuFuRenPaidPreparation(frame)) break;
                if (returns.ChengGongYingPaidPreparation(frame)) break;
                returns.NormalTurnStart(frame.OwnerSeat); break;
            case ProgramLifecycleContinuation.ResumeIdentityAppearance:
                returns.IdentityAppearance(frame); break;
            case ProgramLifecycleContinuation.BeiMiHuPreludeRound:
            case ProgramLifecycleContinuation.BeiMiHuPreludeOwn:
                returns.BeiMiHuPrelude(frame); break;
            case ProgramLifecycleContinuation.RoundProgramsTurnStart:
                returns.AfterRoundPrograms(frame.OwnerSeat); break;
            case ProgramLifecycleContinuation.CompleteDrawPhase:
                returns.DrawPhase(frame, frame.ActualPhaseSubstitution is
                    { SkippedPhase: SkillProgramTurnPhase.Draw } substitution && substitution.ParentFrameId == frame.Id);
                break;
            case ProgramLifecycleContinuation.CompletePlayPhase:
                returns.PlayPhase(); break;
            case ProgramLifecycleContinuation.CompletePhaseHandDebtForcedEnd:
            case ProgramLifecycleContinuation.CompleteWeiluForcedPlayEnd:
                returns.EndTurn(); break;
            case ProgramLifecycleContinuation.CompleteJudgmentPhaseStarting:
                returns.JudgmentStarting(frame); break;
            case ProgramLifecycleContinuation.ResumeCharacterStateChange:
                returns.CharacterStateChange(frame); break;
            case ProgramLifecycleContinuation.CompleteDrawPhaseEnded:
                returns.DrawPhaseEnded(frame.OwnerSeat, frame.DrawPhaseEndedDelayedEffects ??
                    throw new InvalidOperationException("Draw end lost its frozen phase continuation."));
                break;
            case ProgramLifecycleContinuation.CompleteAfterNormalDraw:
                returns.AfterNormalDraw(frame.OwnerSeat); break;
            case ProgramLifecycleContinuation.CompleteDiscardPhase:
                returns.DiscardPhase(frame.OwnerSeat); break;
            case ProgramLifecycleContinuation.EndTurnAfterDiscardPhase:
                returns.EndTurn(); break;
            default:
                throw new InvalidOperationException("Unsupported lifecycle continuation.");
        }
    }

    private sealed class WindowHost(ProgramLifecycleWindowRuntime runtime) : IProgramTriggerWindowHost<ProgramLifecycleTriggerWindowFrame>
    {
        public bool TryGetFrame(out ProgramLifecycleTriggerWindowFrame frame)
        {
            if (runtime._frames.Top is ProgramLifecycleTriggerWindowFrame current) { frame = current; return true; }
            frame = null!; return false;
        }
        public ProgramTriggerWindowPreparation<ProgramLifecycleTriggerWindowFrame> Prepare(ProgramLifecycleTriggerWindowFrame frame) => new(ProgramTriggerWindowFlow.Ready, frame);
        public bool IsComplete(ProgramLifecycleTriggerWindowFrame frame) => frame.CandidateIndex >= frame.Candidates.Count;
        public ProgramTriggerWindowFlow Close(ProgramLifecycleTriggerWindowFrame frame)
        { runtime.CompleteWindow(frame); return ProgramTriggerWindowFlow.Yield; }
        public ProgramTriggerWindowEntry Read(ProgramLifecycleTriggerWindowFrame frame)
        {
            var candidate = frame.Candidates[frame.CandidateIndex];
            return new(candidate, runtime.CreateContext(frame, candidate, pending: false));
        }
        public bool CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => runtime._bindingFacts.CanRun(candidate, context);
        public SkillProgramTrigger ResolveTrigger(ProgramTriggerCandidate candidate) => runtime._bindingFacts.Trigger(candidate);
        public void Skip(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => runtime.AdvanceCandidate(frame, false, false);
        public IReadOnlyList<ProgramTriggerCandidate> ChoiceGroup(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate first, string group) =>
            runtime._bindings.ChoiceGroup(frame.Candidates.Skip(frame.CandidateIndex), first, group);
        public void BeforePrompt(ProgramLifecycleTriggerWindowFrame frame) { }
        public void Expose(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => runtime._bindings.Expose(candidate, context);
        public void ExposeGroup(IReadOnlyList<ProgramTriggerCandidate> members, ProgramTriggerCandidate first, ProgramSkillWindowContext context, string group) => runtime._bindings.ExposeGroup(members, first, context, group);
        public void Begin(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => runtime._bindings.Begin(candidate, context);
        public int CandidateIndex(ProgramLifecycleTriggerWindowFrame frame) => frame.CandidateIndex;
        public void RecordGroupSkip(ProgramLifecycleTriggerWindowFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
            runtime._events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId,
                candidate.SkillInstanceId, candidate.OwnerSeat, context.Window, Activated: false, Completed: false));
        public void MovePastGroup(ProgramLifecycleTriggerWindowFrame frame, int nextIndex) => runtime._frames.ReplaceTop(frame with { CandidateIndex = nextIndex });
        public void Continue() => runtime._frames.AdvanceTop<ProgramLifecycleTriggerWindowFrame>();
    }
}
