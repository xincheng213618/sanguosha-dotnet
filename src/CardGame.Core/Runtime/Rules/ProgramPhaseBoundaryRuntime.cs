namespace CardGame.Core;

/// <summary>Owns phase-boundary admission, ordered prompts and exact typed child returns.</summary>
[ProgramRuntimeModule]
internal sealed partial class ProgramPhaseBoundaryRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramWindowCursors cursors, IMatchParticipantStateAccess participants,
    ICardZoneQueries zones, IProgramContentQueries content, IProgramTurnQueries turn,
    IProgramBindingActions bindings, IProgramBindingFacts bindingFacts, IProgramWindowCandidateQueries triggers,
    IProgramPhaseBoundaryFacts facts, IProgramPhaseBoundaryProtocols native)
{
    private IProgramRuntimeFrames Frames => frames;
    private IProgramRuntimeEvents Events => events;
    private IProgramBindingActions Bindings => bindings;
    private IProgramBindingFacts BindingFacts => bindingFacts;
    private IProgramPhaseBoundaryProtocols Native => native;
    private IProgramContentQueries Content => content;
    private IProgramRuntimePrompts Prompts => prompts;
    private IMatchParticipantStateAccess Participants => participants;

    internal bool TryBeginTurnEndingBoundary(CharacterState owner)
    {
        if ((!owner.IsAlive && !facts.HasWenheDeadOwnerEnding(owner.Seat) && !facts.HasAnniversaryTianjieDeadOwnerEnding(owner.Seat)) ||
            owner.Seat != turn.CurrentSeat || prompts.Current is not null || frames.Top is not null)
            return false;

        var ownerFacts = triggers.Capture(owner);
        var items = participants.Players.Where(player => player.IsAlive &&
                triggers.Shard(player).GetInstanceTriggers(SkillProgramTriggerWindow.TurnEnding).Count != 0)
            .SelectMany(player =>
            {
                var playerFacts = player.Seat == owner.Seat ? ownerFacts : triggers.Capture(player);
                if (triggers.Shard(player).GetUniqueTriggers(SkillProgramTriggerWindow.TurnEnding).Any(binding =>
                    ProgramInstructionResolver.Default.Features(binding.Trigger).UsesValue(SkillProgramTriggerValueKind.EventTargetHandCount)))
                    playerFacts = playerFacts with { EventTargetHandCount = zones.CardsAt(CardLocation.Hand(owner.Seat)).Count };
                return triggers.Eligible(player, SkillProgramTriggerWindow.TurnEnding, playerFacts)
                    .Where(candidate => owner.IsAlive || facts.IsWenheDeadOwnerEndingCandidate(candidate, owner.Seat) || facts.IsAnniversaryTianjieDeadOwnerEndingCandidate(candidate, owner.Seat))
                    .Where(candidate => !bindingFacts.Trigger(candidate).Effects.Any(effect => effect.Op is SkillProgramEffectOp.ResolveXingzuoIssuedEndingExchange or SkillProgramEffectOp.ResolveQushiIssuedHolderEnding) && MatchesTurnEndingScope(candidate, owner.Seat))
                    .Select(candidate => new TurnEndingBoundaryItem(
                        TurnEndingBoundaryItemKind.Program,
                        candidate.Priority,
                        $"program:{candidate.SkillId}:{candidate.BindingId}:{candidate.SkillInstanceId}",
                        candidate, bindingFacts.Trigger(candidate).TurnOwnerScope == SkillProgramTurnOwnerScope.OwnOrPreviousLiving
                            ? playerFacts with { FrozenPreviousLivingSeat = facts.PreviousLivingSeatFor(player.Seat) }
                            : player.Seat == owner.Seat && !ProgramInstructionResolver.Default.Features(bindingFacts.Trigger(candidate))
                                .HasOperation(SkillProgramEffectOp.OfferCurrentTurnHandExchange) ? null : playerFacts));
            }).ToList();
        native.AppendEarnedActualEndingItems(items);
        native.AppendPrepDiscardEndingItems(items);
        native.AppendGiftRetentionEndingItems(items);
        native.AppendFixedDistanceOneEndingItems(items);
        native.AppendYanPuEndingItems(items);
        native.AppendWenYangEndingItems(items);
        native.AppendLiuBaCoinEndingItems(items);
        native.AppendRuanYuXingzuoEndingItems(items);
        native.AppendQushiEndingItems(items);
        native.AppendLianjuEndingItems(items);
        native.AppendPipelineEndingItems(items);
        var ordered = items
            .OrderBy(item => facts.RelativeTableOrder(owner.Seat, item.Candidate?.OwnerSeat ?? item.RetentionOwnerSeat!.Value))
            .ThenByDescending(item => item.Priority)
            .ThenBy(item => item.StableIdentity, StringComparer.Ordinal)
            .ToArray();
        if (ordered.Length == 0) return false;

        frames.Push(new TurnEndingBoundaryFrame(frames.NextId(), owner.Seat, turn.TurnNumber,
            Array.AsReadOnly(ordered), ownerFacts));
        frames.AdvanceTop<TurnEndingBoundaryFrame>();
        return true;
    }

    private bool MatchesTurnEndingScope(ProgramTriggerCandidate candidate, int actualOwner)
    {
        if (facts.IsWenheDeadOwnerEndingCandidate(candidate, actualOwner)) return true;
        var scope = bindingFacts.Trigger(candidate).TurnOwnerScope;
        if (scope == SkillProgramTurnOwnerScope.AnyLiving) return participants.At(actualOwner).IsAlive;
        if (scope == SkillProgramTurnOwnerScope.OwnOrPreviousLiving)
            return candidate.OwnerSeat == actualOwner || facts.PreviousLivingSeatFor(candidate.OwnerSeat) == actualOwner;
        return scope == (candidate.OwnerSeat == actualOwner ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving);
    }

    internal ProgramSkillWindowContext CreateTurnEndingProgramContext(TurnEndingBoundaryFrame frame,
        ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? candidateFacts = null) =>
        facts.QushiEndingContext(frame, candidate, new(SkillProgramTriggerWindow.TurnEnding, frame.Id,
            candidate.OwnerSeat, SourceSeat: frame.OwnerSeat, TargetSeat: frame.OwnerSeat,
            OccurrenceIndex: candidate.OccurrenceIndex, Facts: candidateFacts ?? frame.Facts)
        {
            EarnedBenefit = frame.Items[frame.ItemIndex].EarnedBenefit,
            PrepDiscardPromise = frame.Items[frame.ItemIndex].PrepDiscardPromise,
            FixedDistanceDebt = frame.Items[frame.ItemIndex].FixedDistanceDebt
        });

    [NativeFrameRoute(typeof(TurnEndingBoundaryFrame), 450)]
    internal void ContinueTurnEndingBoundary()
    {
        while (frames.Top is TurnEndingBoundaryFrame frame)
        {
            // Child victory can leave the owner alive. Retire the boundary before
            // another observer is admitted, exactly as for a lethal child.
            if (facts.Winner != Winner.None)
            {
                frames.Pop(frame.Id, ResolutionFrameKind.TurnEndingBoundary);
                if (facts.Status != EngineStatus.Completed) native.CompleteGame();
                else native.PublishState();
                return;
            }
            if (frame.ItemIndex >= frame.Items.Count)
            {
                frames.Pop(frame.Id, ResolutionFrameKind.TurnEndingBoundary);
                native.FinalizeEndTurn(participants.At(frame.OwnerSeat));
                return;
            }
            var item = frame.Items[frame.ItemIndex];
            switch (item.Kind)
            {
                case TurnEndingBoundaryItemKind.GiftRetention:
                    if (native.ConsumeGiftRetentionEndingItem(frame, item)) return;
                    continue;
                case TurnEndingBoundaryItemKind.Program:
                {
                    var candidate = item.Candidate ??
                        throw new InvalidOperationException("A program turn-ending item lost its candidate.");
                    var context = CreateTurnEndingProgramContext(frame, candidate, item.Facts);
                    if (!bindingFacts.CanRun(candidate, context))
                    {
                        native.ConsumeSkippedEarnedActualEnding(frame);
                        native.ConsumeSkippedPrepDiscardEnding(frame);
                        native.ConsumeSkippedFixedDistanceDebt(frame);
                        AdvanceTurnEndingBoundaryCandidate(frame, candidate, activated: false, completed: false);
                        continue;
                    }
                    var trigger = content.Skill(candidate.SkillId).Program!.Triggers.Single(current => current.Id == candidate.BindingId);
                    if (trigger.Optional)
                    {
                        if (TryExposeTurnEndingOrder(frame, candidate)) return;
                        frames.ReplaceTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
                        bindings.Expose(candidate, context);
                        return;
                    }
                    bindings.Begin(candidate, context);
                    return;
                }
                default:
                    throw new InvalidOperationException("Unsupported turn-ending boundary item.");
            }
        }
    }

    internal void AdvanceTurnEndingBoundaryCandidate(TurnEndingBoundaryFrame frame,
        ProgramTriggerCandidate candidate, bool activated, bool completed)
    {
        events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId,
            candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.TurnEnding, activated, completed));
        cursors.AdvanceTurnEndingBoundaryCursor(frame);
    }

    internal bool TryBeginPlayPhaseStartingBoundary(CharacterState owner)
    {
        if (!owner.IsAlive || owner.Seat != turn.CurrentSeat || prompts.Current is not null || frames.Top is not null)
            return false;
        if (!participants.Players.Any(player => player.IsAlive &&
                (triggers.Shard(player).GetInstanceTriggers(SkillProgramTriggerWindow.PlayPhaseStarting).Count != 0 ||
                 triggers.IncludeIssuedReturns(player, SkillProgramTriggerWindow.PlayPhaseStarting, Array.Empty<ProgramTriggerCandidate>()).Count != 0)))
            return false;

        var ownerFacts = triggers.Capture(owner);
        var items = participants.Players.Where(player => player.IsAlive)
            .SelectMany(player =>
            {
                var playerFacts = player.Seat == owner.Seat ? ownerFacts : triggers.Capture(player);
                return triggers.Eligible(player, SkillProgramTriggerWindow.PlayPhaseStarting, playerFacts)
                    .Where(candidate => bindingFacts.Trigger(candidate).TurnOwnerScope ==
                        (player.Seat == owner.Seat ? SkillProgramTurnOwnerScope.Own : SkillProgramTurnOwnerScope.OtherLiving))
                    .Select(candidate => new TurnEndingBoundaryItem(TurnEndingBoundaryItemKind.Program,
                        candidate.Priority, $"program:{candidate.SkillId}:{candidate.BindingId}:{candidate.SkillInstanceId}",
                        candidate, bindingFacts.Trigger(candidate).Effects.Any(effect => effect.Op == SkillProgramEffectOp.UseBlackCardAtWoundedPlayStart)
                            ? playerFacts with { EventTargetHp = owner.Hp, EventTargetMaxHp = owner.MaxHp, EventTargetHpUndefined = !owner.HasHpValue, EventTargetRuleWounded = facts.UsesTreatedWounded ? facts.IsRuleWounded(owner) : null }
                            : player.Seat == owner.Seat ? null : playerFacts));
            }).ToList();
        var ordered = items.OrderBy(item => facts.RelativeTableOrder(owner.Seat, item.Candidate!.OwnerSeat))
            .ThenByDescending(item => item.Priority).ThenBy(item => item.StableIdentity, StringComparer.Ordinal).ToArray();
        if (ordered.Length == 0) return false;

        frames.Push(new PlayPhaseStartingBoundaryFrame(frames.NextId(), owner.Seat, Array.AsReadOnly(ordered), ownerFacts)
        { QingzhongStartingPhase = facts.CaptureQingzhongStartingPhase(owner), WeiluStartingPhase = facts.CaptureWeiluStartingPhase(owner) });
        frames.AdvanceTop<PlayPhaseStartingBoundaryFrame>();
        return true;
    }

    internal ProgramSkillWindowContext CreatePlayPhaseStartingProgramContext(PlayPhaseStartingBoundaryFrame frame,
        ProgramTriggerCandidate candidate, SkillProgramTriggerFacts? candidateFacts = null) =>
        new(SkillProgramTriggerWindow.PlayPhaseStarting, frame.Id, candidate.OwnerSeat,
            SourceSeat: frame.OwnerSeat, TargetSeat: frame.OwnerSeat,
            OccurrenceIndex: candidate.OccurrenceIndex, Facts: candidateFacts ?? frame.Facts)
        { QingzhongPhase = frame.QingzhongStartingPhase, WeiluPhase = frame.WeiluStartingPhase };

    [NativeFrameRoute(typeof(PlayPhaseStartingBoundaryFrame), 460)]
    internal void ContinuePlayPhaseStartingBoundary() => ProgramTriggerWindowDriver.Run(new PlayStartingWindowHost(this));

    internal void AdvancePlayPhaseStartingCandidate(PlayPhaseStartingBoundaryFrame frame,
        ProgramTriggerCandidate candidate, bool activated, bool completed)
    {
        events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId,
            candidate.SkillInstanceId, candidate.OwnerSeat, SkillProgramTriggerWindow.PlayPhaseStarting, activated, completed));
        cursors.AdvancePlayPhaseStartingCursor(frame);
    }

    internal IReadOnlyList<ProgramTriggerCandidate> GetPlayPhaseStartingChoiceGroup(PlayPhaseStartingBoundaryFrame frame,
        ProgramTriggerCandidate first, string choiceGroup) => bindings.ChoiceGroup(
            frame.Items.Skip(frame.ItemIndex).Select(item => item.Candidate)
                .Where(candidate => candidate is not null).Select(candidate => candidate!), first, choiceGroup);

    internal bool TryReadPlayStartingPending(out ProgramTriggerWindowEntry entry)
    {
        if (frames.Top is PlayPhaseStartingBoundaryFrame frame)
        {
            ProgramTriggerCandidate Candidate() => frame.Items[frame.ItemIndex].Candidate ??
                throw new InvalidOperationException("The play-phase-starting program item lost its candidate.");
            entry = new(Candidate(), CreatePlayPhaseStartingProgramContext(frame, Candidate(), frame.Items[frame.ItemIndex].Facts));
            return true;
        }
        entry = default;
        return false;
    }

    internal bool TrySkipPlayStartingCandidate(ProgramTriggerCandidate candidate)
    {
        if (frames.Top is not PlayPhaseStartingBoundaryFrame frame || frame.Items[frame.ItemIndex].Candidate != candidate)
            return false;
        AdvancePlayPhaseStartingCandidate(frame, candidate, activated: false, completed: false);
        frames.AdvanceTop<PlayPhaseStartingBoundaryFrame>();
        return true;
    }

    internal bool TryResolvePlayStartingGroup(ProgramTriggerCandidate first, ProgramSkillWindowContext context,
        PromptChoice selected, string? action)
    {
        if (frames.Top is not PlayPhaseStartingBoundaryFrame frame || bindingFacts.Trigger(first).ChoiceGroup is not { } group)
            return false;
        ProgramTriggerWindowDriver.ResolveChoiceGroup(new PlayStartingWindowHost(this), frame, first, context,
            group, selected, action, prompts.Clear);
        return true;
    }

    [ProgramWindowReturn(SkillProgramTriggerWindow.TurnEnding)]
    internal void ReturnTurnEndingProgramWindow(ProgramSkillFrame frame, bool completed)
    {
        var context = frame.WindowContext!;
        if (frames.Top is not TurnEndingBoundaryFrame turnEnding || turnEnding.Id != context.ParentFrameId ||
            turnEnding.Items[turnEnding.ItemIndex].Candidate is not { } expected ||
            expected != new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId!,
                frame.SkillInstanceId, frame.GameplayHash, turnEnding.Items[turnEnding.ItemIndex].Priority, context.OccurrenceIndex))
            throw new InvalidOperationException("The turn-ending program lost its parent item.");
        cursors.AdvanceTurnEndingBoundaryCursor(turnEnding);
        frames.AdvanceTop<TurnEndingBoundaryFrame>();
    }

    [ProgramWindowReturn(SkillProgramTriggerWindow.PlayPhaseStarting)]
    internal void ReturnPlayPhaseStartingProgramWindow(ProgramSkillFrame frame, bool completed)
    {
        var context = frame.WindowContext!;
        if (frames.Top is not PlayPhaseStartingBoundaryFrame playStarting || playStarting.Id != context.ParentFrameId)
            throw new InvalidOperationException("The play-phase-starting program lost its parent item.");
        if (context.ResumeCandidateIndex is { } resumeItemIndex)
        {
            if (resumeItemIndex > playStarting.Items.Count)
                throw new InvalidOperationException("The play-phase-starting resume cursor left the boundary.");
            frames.ReplaceTop(playStarting with { ItemIndex = resumeItemIndex });
        }
        else
        {
            if (playStarting.ItemIndex >= playStarting.Items.Count ||
                playStarting.Items[playStarting.ItemIndex].Candidate is not { } expected ||
                expected != new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId!,
                    frame.SkillInstanceId, frame.GameplayHash, playStarting.Items[playStarting.ItemIndex].Priority, context.OccurrenceIndex))
                throw new InvalidOperationException("The play-phase-starting program lost its parent item.");
            cursors.AdvancePlayPhaseStartingCursor(playStarting);
        }
        frames.AdvanceTop<PlayPhaseStartingBoundaryFrame>();
    }

    private sealed class PlayStartingWindowHost(ProgramPhaseBoundaryRuntime runtime) : IProgramTriggerWindowHost<PlayPhaseStartingBoundaryFrame>
    {
        public bool TryGetFrame(out PlayPhaseStartingBoundaryFrame frame)
        {
            if (runtime.Frames.Top is PlayPhaseStartingBoundaryFrame current) { frame = current; return true; }
            frame = null!; return false;
        }
        public ProgramTriggerWindowPreparation<PlayPhaseStartingBoundaryFrame> Prepare(PlayPhaseStartingBoundaryFrame frame) => new(ProgramTriggerWindowFlow.Ready, frame);
        public bool IsComplete(PlayPhaseStartingBoundaryFrame frame) => frame.ItemIndex >= frame.Items.Count;
        public ProgramTriggerWindowFlow Close(PlayPhaseStartingBoundaryFrame frame)
        {
            runtime.Frames.Pop(frame.Id, ResolutionFrameKind.PlayPhaseStartingBoundary);
            runtime.Native.PublishState();
            return ProgramTriggerWindowFlow.Yield;
        }
        public ProgramTriggerWindowEntry Read(PlayPhaseStartingBoundaryFrame frame)
        {
            var item = frame.Items[frame.ItemIndex];
            if (item.Kind != TurnEndingBoundaryItemKind.Program) throw new InvalidOperationException("Unsupported play-phase-starting boundary item.");
            var candidate = item.Candidate ?? throw new InvalidOperationException("A program play-phase-starting item lost its candidate.");
            return new(candidate, runtime.CreatePlayPhaseStartingProgramContext(frame, candidate, item.Facts));
        }
        public bool CanRun(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => runtime.BindingFacts.CanRun(candidate, context);
        public SkillProgramTrigger ResolveTrigger(ProgramTriggerCandidate candidate) => runtime.BindingFacts.Trigger(candidate);
        public void Skip(PlayPhaseStartingBoundaryFrame frame, ProgramTriggerCandidate candidate) => runtime.AdvancePlayPhaseStartingCandidate(frame, candidate, false, false);
        public IReadOnlyList<ProgramTriggerCandidate> ChoiceGroup(PlayPhaseStartingBoundaryFrame frame, ProgramTriggerCandidate first, string group) => runtime.GetPlayPhaseStartingChoiceGroup(frame, first, group);
        public void BeforePrompt(PlayPhaseStartingBoundaryFrame frame) => runtime.Frames.ReplaceTop(frame with { Step = ResolutionFrameStep.AwaitingResponse });
        public void Expose(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => runtime.Bindings.Expose(candidate, context);
        public void ExposeGroup(IReadOnlyList<ProgramTriggerCandidate> members, ProgramTriggerCandidate first, ProgramSkillWindowContext context, string group) => runtime.Bindings.ExposeGroup(members, first, context, group);
        public void Begin(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => runtime.Bindings.Begin(candidate, context);
        public int CandidateIndex(PlayPhaseStartingBoundaryFrame frame) => frame.ItemIndex;
        public void RecordGroupSkip(PlayPhaseStartingBoundaryFrame frame, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) =>
            runtime.Events.AdvanceFact(new ProgramBindingResolvedEvent(frame.Id, candidate.SkillId, candidate.BindingId,
                candidate.SkillInstanceId, candidate.OwnerSeat, context.Window, Activated: false, Completed: false));
        public void MovePastGroup(PlayPhaseStartingBoundaryFrame frame, int nextIndex) => runtime.Frames.ReplaceTop(frame with { ItemIndex = nextIndex });
        public void Continue() => runtime.Frames.AdvanceTop<PlayPhaseStartingBoundaryFrame>();
    }
}
