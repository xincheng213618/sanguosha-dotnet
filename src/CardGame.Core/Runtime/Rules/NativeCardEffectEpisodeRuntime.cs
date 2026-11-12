namespace CardGame.Core;

[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeCardEffectEpisodeRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IMatchParticipantStateAccess participants, INativeCardEffectEpisodeExecution native) : INativeCardEffectEpisodes
{
    public NativeCardEffectEpisode Issue(long useId, NativeCardEffectEpisodeIssue issue,
        ProgramTrickContinuation? trick = null, ProgramSimpleCardContinuation? simple = null)
    {
        var use = Use(useId);
        var spec = issue.Spec;
        if (issue.IssuerFrameId <= 0 || issue.InstructionIndex < 0 || issue.IssuanceSlot < 0 || issue.Source is null ||
            string.IsNullOrWhiteSpace(issue.GameplayHash) || !ValidSeat(issue.Source.OwnerSeat) || spec.Count <= 0 ||
            !Enum.IsDefined(spec.Scope) || !Enum.IsDefined(spec.Replay) || !Enum.IsDefined(spec.TargetAdmission) ||
            spec.Scope == NativeCardEffectEpisodeScope.FrozenTarget && (spec.TargetSeat is not { } target || !ValidSeat(target)) ||
            spec.Scope is NativeCardEffectEpisodeScope.ExecutionOccurrence or NativeCardEffectEpisodeScope.ExecutionPair && spec.TargetIndex is not >= 0 ||
            spec.Scope == NativeCardEffectEpisodeScope.ExecutionPair && spec.TargetIndex % 2 != 0 ||
            spec.Scope == NativeCardEffectEpisodeScope.WholeUse && (spec.TargetSeat is not null || spec.TargetIndex is not null))
            throw new InvalidOperationException("An effect episode requires an exact issued source, count, scope and original native use.");
        var state = use.EffectEpisodes ?? new(new(use.Action?.ActionId, trick, simple));
        if (state.Original.ActionId != use.Action?.ActionId ||
            trick is not null && state.Original.Trick is { } originalTrick && originalTrick != trick ||
            simple is not null && state.Original.Simple is { } originalSimple && !SameSimple(originalSimple, simple))
            throw new InvalidOperationException("Effect episodes cannot replace their original native continuation.");
        var previous = state.Episodes.SingleOrDefault(episode => episode.Issue.IssuerFrameId == issue.IssuerFrameId &&
            episode.Issue.InstructionIndex == issue.InstructionIndex && episode.Issue.IssuanceSlot == issue.IssuanceSlot);
        if (previous is not null)
        {
            if (previous.Issue != issue) throw new InvalidOperationException("An effect episode key cannot change its issued meaning.");
            return previous;
        }
        var issued = new NativeCardEffectEpisode(frames.NextId(), issue);
        var original = state.Original;
        if (original.Trick is null && trick is not null) original = original with { Trick = trick };
        if (original.Simple is null && simple is not null) original = original with { Simple = simple };
        state = state with { Original = original, Episodes = [.. state.Episodes, issued] };
        frames.Replace(useId, use with { EffectEpisodes = state });
        events.AdvanceFact(new NativeCardEffectEpisodeIssuedEvent(useId, state.Original.ActionId, issued));
        return issued;
    }

    public NativeCardEffectEpisode IssueCurrent(ProgramSkillFrame supplied, ProgramCardEffectEpisode declaration)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted or SkillProgramTriggerWindow.CardUseTargetsFinalized,
                CardUse: { } identity } context ||
            frames.Find(context.ParentFrameId) is not ProgramCardTriggerWindowFrame window ||
            frames.Find(identity.ParentCardUseFrameId) is not CardUseFrame { Action: { Type: CardActionType.Use } action } use ||
            window.ParentFrameId != use.Id || window.Action.ActionId != action.ActionId || action.ActionId != identity.CardActionId ||
            window.CompletedResponseReturn is not null)
            throw new InvalidOperationException("A declared effect episode requires its original native use window.");
        var spec = declaration.Spec;
        if (declaration.TargetRef is { } reference)
        {
            var target = reference.Kind switch
            {
                ProgramParticipantRef.Owner => frame.OwnerSeat,
                ProgramParticipantRef.SelectedTarget when frame.SelectedTargetSeats.Count == 1 => frame.SelectedTargetSeats[0],
                ProgramParticipantRef.EventTarget when context.TargetSeat is { } seat => seat,
                ProgramParticipantRef.EventSource => context.SourceSeat ?? identity.ActorSeat,
                _ => throw new InvalidOperationException("A target-local effect episode lost its declared participant.")
            };
            spec = spec with { TargetSeat = target };
        }
        return Issue(use.Id, new(frame.Id, frame.InstructionIndex - 1, 0,
            new(frame.SkillId, frame.TriggerId ?? frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, spec),
            window.TrickContinuation, window.SimpleContinuation);
    }

    public NativeCardEffectEpisode? Get(long useId, long episodeId) =>
        (frames.Find(useId) as CardUseFrame)?.EffectEpisodes?.Episodes.SingleOrDefault(episode => episode.Id == episodeId);
    public bool IsCompleted(long useId, long episodeId) => Get(useId, episodeId) is { } episode && episode.Completed == episode.Issue.Spec.Count;
    public bool IsSettled(long useId, long episodeId) => Get(useId, episodeId) is { } episode && Settled(episode);
    public int Ordinal(long useId) => (frames.Find(useId) as CardUseFrame)?.EffectEpisodes?.StartedOrdinal ?? 0;
    public NativeCardEffectEpisodeCursor? Current(long useId) => (frames.Find(useId) as CardUseFrame)?.EffectEpisodes?.Current;
    public bool IsActive(long useId) => Current(useId) is not null;
    public bool IsTargetScoped(long useId) => Current(useId) is { } cursor &&
        Get(useId, cursor.EpisodeId) is { Issue.Spec.Scope: not NativeCardEffectEpisodeScope.WholeUse };
    public bool CanContinueAfterActorDeath(long useId)
    {
        if (frames.Find(useId) is not CardUseFrame { EffectEpisodes: { } state }) return false;
        if (state.Current is { } current)
            return state.Episodes.SingleOrDefault(episode => episode.Id == current.EpisodeId)?.Issue.Spec.ContinueAfterActorDeath == true;
        // This right was issued to the original use, so settling or cancelling
        // its extra copies does not erase that independent native permission.
        return state.Episodes.Any(episode => episode.Issue.Spec.ContinueOriginalAfterActorDeath);
    }
    public bool HasPending(long useId) => frames.Find(useId) is CardUseFrame { EffectEpisodes: { } state } use &&
        !native.IsMatchCompleted && !native.IsUseIneffective(use) &&
        state.Episodes.Any(episode => !episode.Cancelled && episode.Started < episode.Issue.Spec.Count);
    public NativeCardEffectOriginalState? Original(long useId) => (frames.Find(useId) as CardUseFrame)?.EffectEpisodes?.Original;

    public void CaptureAttack(long useId, int targetIndex, CardAttackState attack, FangtianHalberdState? fangtian = null)
    {
        if (frames.Find(useId) is not CardUseFrame { EffectEpisodes: { StartedOrdinal: 0 } state } use ||
            state.Original.Attacks.Any(seed => seed.TargetIndex == targetIndex)) return;
        if (targetIndex < 0 || targetIndex >= use.TargetSeats.Count || use.TargetSeats[targetIndex] != attack.TargetSeat)
            throw new InvalidOperationException("An effect seed requires the original native target occurrence.");
        frames.Replace(useId, use with { EffectEpisodes = state with { Original = state.Original with
            { Attacks = [.. state.Original.Attacks, new(targetIndex, attack)], Fangtian = fangtian ?? state.Original.Fangtian } } });
    }
    public void CaptureGroupDamageSource(long useId, int sourceSeat)
    {
        if (frames.Find(useId) is not CardUseFrame { EffectEpisodes: { StartedOrdinal: 0 } state } use) return;
        if (!ValidSeat(sourceSeat)) throw new InvalidOperationException("A native group damage source requires an actual participant.");
        frames.Replace(useId, use with { EffectEpisodes = state with { Original = state.Original with { GroupDamageSourceSeat = sourceSeat } } });
    }

    public bool TryBegin(long useId, bool afterAttack = false)
    {
        if (frames.Find(useId) is not CardUseFrame { EffectEpisodes: { } initial } use || frames.Top?.Id != useId || HasNativeChildren(use)) return false;
        var cancelled = native.IsMatchCompleted || native.IsUseIneffective(use);
        if (!cancelled && (initial.Current is null || Get(useId, initial.Current.EpisodeId)?.Issue.Spec.Scope == NativeCardEffectEpisodeScope.WholeUse) &&
            native.HasPendingTargets(use)) return false;
        if (initial.Current is not null)
        {
            CompleteCurrent(useId, executed: !cancelled);
            if (frames.Top?.Id != useId) return true;
        }
        while (frames.Find(useId) is CardUseFrame { EffectEpisodes: { } state } current)
        {
            if (native.IsMatchCompleted || native.IsUseIneffective(current))
            {
                CancelRemaining(current, state);
                return frames.Top?.Id != useId;
            }
            var next = state.Episodes.Where(episode => !Settled(episode))
                .OrderBy(episode => episode.Issue.Spec.Priority).ThenBy(episode => episode.Id).FirstOrDefault();
            if (next is null) return false;
            var cursor = ResolveCursor(current, state, next);
            var started = next with { Started = next.Started + 1 };
            var local = started.Issue.Spec.Scope != NativeCardEffectEpisodeScope.WholeUse;
            frames.Replace(useId, current with { EffectEpisodes = state with { Episodes = Replace(state.Episodes, started),
                Current = cursor, StartedOrdinal = cursor.Ordinal },
                TargetIndex = cursor.TargetIndex < 0 ? current.TargetSeats.Count : cursor.TargetIndex,
                Step = ResolutionFrameStep.ResolvingEffect,
                SequentialTrick = local ? null : current.SequentialTrick,
                AdjustedSimpleContinuation = local ? null : current.AdjustedSimpleContinuation,
                FireAttackSelection = null, ColorFireAttack = null, UnexpectedAssaultReveal = null, VirtualBasicEffectApplied = false });
            events.AdvanceFact(new NativeCardEffectEpisodeStartedEvent(useId, state.Original.ActionId, started, cursor));
            if (frames.Top?.Id != useId) return true;
            current = Use(useId);
            var spec = started.Issue.Spec;
            if (spec.Scope != NativeCardEffectEpisodeScope.WholeUse && cursor.TargetSeats.Count == 0 ||
                cursor.TargetSeats.Count > 0 && cursor.TargetSeats.All(seat => !ValidSeat(seat) || !participants.At(seat).IsAlive) ||
                spec.Scope != NativeCardEffectEpisodeScope.WholeUse && cursor.TargetSeats.Count > 0 && !participants.At(cursor.TargetSeats[0]).IsAlive ||
                !spec.ContinueAfterActorDeath && !participants.At(current.SourceSeat).IsAlive ||
                spec.Replay == NativeCardEffectEpisodeReplay.ConsumePhysicalPlacement ||
                spec.TargetAdmission == NativeCardEffectEpisodeTargetAdmission.RequireCurrentTargetsLegal && !native.TargetsRemainLegal(current))
            {
                CompleteCurrent(useId, executed: false);
                if (frames.Top?.Id != useId) return true;
                continue;
            }
            if (native.Begin(current, current.EffectEpisodes!.Original, started, cursor, afterAttack)) return true;
            CompleteCurrent(useId, executed: false);
            if (frames.Top?.Id != useId) return true;
        }
        return false;
    }

    public void CompleteCurrent(long useId, bool executed = true)
    {
        var use = Use(useId);
        if (use.EffectEpisodes is not { Current: { } cursor } state) return;
        if (frames.Top?.Id != useId || HasNativeChildren(use))
            throw new InvalidOperationException("An effect episode cannot complete while its actual native child is pending.");
        var episode = state.Episodes.Single(episode => episode.Id == cursor.EpisodeId);
        if (episode.Started != cursor.CopyIndex || episode.Completed != cursor.CopyIndex - 1)
            throw new InvalidOperationException("An effect completion requires its original active cursor.");
        var completed = episode with { Completed = episode.Completed + 1 };
        var local = episode.Issue.Spec.Scope != NativeCardEffectEpisodeScope.WholeUse;
        frames.Replace(useId, use with { EffectEpisodes = state with { Current = null, Episodes = Replace(state.Episodes, completed) },
            TargetIndex = local ? use.TargetSeats.Count : use.TargetIndex,
            SequentialTrick = local ? null : use.SequentialTrick,
            AdjustedSimpleContinuation = local ? null : use.AdjustedSimpleContinuation });
        events.AdvanceFact(new NativeCardEffectEpisodeCompletedEvent(useId, state.Original.ActionId, completed, cursor, executed));
    }

    private NativeCardEffectEpisodeCursor ResolveCursor(CardUseFrame use, NativeCardEffectEpisodesState state, NativeCardEffectEpisode episode)
    {
        var spec = episode.Issue.Spec;
        if (spec.Scope == NativeCardEffectEpisodeScope.WholeUse)
            return new(episode.Id, episode.Started + 1, checked(state.StartedOrdinal + 1), 0, use.TargetSeats);
        var index = spec.Scope switch
        {
            NativeCardEffectEpisodeScope.FrozenTarget => Array.IndexOf(use.TargetSeats.ToArray(), spec.TargetSeat!.Value),
            _ => spec.TargetIndex!.Value
        };
        var pair = spec.Scope == NativeCardEffectEpisodeScope.ExecutionPair ||
            spec.Scope == NativeCardEffectEpisodeScope.FrozenTarget && state.Original.Trick?.ActionKind == LegalActionKind.BorrowedSword;
        if (index < 0 || index >= use.TargetSeats.Count || pair && (index % 2 != 0 || index + 1 >= use.TargetSeats.Count))
            return new(episode.Id, episode.Started + 1, checked(state.StartedOrdinal + 1), -1, []);
        IReadOnlyList<int> targets = spec.Scope == NativeCardEffectEpisodeScope.WholeUse ? use.TargetSeats :
            pair ? [use.TargetSeats[index], use.TargetSeats[index + 1]] : [use.TargetSeats[index]];
        return new(episode.Id, episode.Started + 1, checked(state.StartedOrdinal + 1), index, targets);
    }
    private CardUseFrame Use(long id) => frames.Find(id) as CardUseFrame ?? throw new InvalidOperationException("An effect episode lost its original native use.");
    private void CancelRemaining(CardUseFrame use, NativeCardEffectEpisodesState state)
    {
        var cancelled = state.Episodes.Where(episode => !Settled(episode)).Select(episode => episode with { Cancelled = true }).ToArray();
        if (cancelled.Length == 0) return;
        frames.Replace(use.Id, use with { EffectEpisodes = state with
            { Episodes = state.Episodes.Select(episode => Settled(episode) ? episode : episode with { Cancelled = true }).ToArray() } });
        foreach (var episode in cancelled)
            events.AdvanceFact(new NativeCardEffectEpisodeCancelledEvent(use.Id, state.Original.ActionId, episode));
    }
    private static bool Settled(NativeCardEffectEpisode episode) => episode.Cancelled || episode.Completed == episode.Issue.Spec.Count;
    private static bool HasNativeChildren(CardUseFrame use) => use.NativeDraw is not null || use.NativeCardEffectDraw is not null ||
        use.WumiaoFlood is not null || use.GuanduCardEffect is not null || use.DragonBoatCardEffect is not null;
    private bool ValidSeat(int seat) => seat >= 0 && seat < participants.Players.Count;
    private static IReadOnlyList<NativeCardEffectEpisode> Replace(IReadOnlyList<NativeCardEffectEpisode> episodes, NativeCardEffectEpisode value) =>
        Array.AsReadOnly(episodes.Select(episode => episode.Id == value.Id ? value : episode).ToArray());
    private static bool SameSimple(ProgramSimpleCardContinuation left, ProgramSimpleCardContinuation right) =>
        left.CardId == right.CardId && left.Effect == right.Effect && left.RecoveryAmount == right.RecoveryAmount &&
        (left.RecoveryPolicySources ?? []).SequenceEqual(right.RecoveryPolicySources ?? []);
}
