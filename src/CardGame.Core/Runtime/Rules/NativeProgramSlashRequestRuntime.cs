namespace CardGame.Core;

// The original instruction owns its offer and accepted request. Material/payment
// producers retain their own typed children; none of those children requalifies this offer.
[ProgramRuntimeModule, ProgramCapability]
internal sealed class NativeProgramSlashRequestRuntime(IProgramRuntimeFrames frames,
    IMatchParticipantStateAccess participants, IProgramSlashRequestFacts facts, IAttackRangeQueries ranges) : INativeProgramSlashRequests
{
    public ProgramSlashRequestIdentity Issue(ProgramSkillFrame supplied, int actorSeat,
        ProgramSlashRequestSpec spec, int issuanceSlot = 0)
    {
        ArgumentNullException.ThrowIfNull(spec);
        var frame = frames.ActiveProgram(supplied.Id);
        if (frame.InstructionIndex < 1 || issuanceSlot < 0 || !Valid(actorSeat) || !Valid(frame.OwnerSeat) ||
            spec.CandidateSeats.Any(seat => !Valid(seat) || seat == actorSeat) ||
            spec.RangeOwnerSeat is { } rangeOwner && !Valid(rangeOwner))
            throw new InvalidOperationException("A native Slash request requires its original paused instruction and participant scope.");
        var identity = new ProgramSlashRequestIdentity(frame.Id, frame.InstructionIndex, issuanceSlot, actorSeat,
            new(frame.SkillId, frame.TriggerId ?? frame.ActivationId, frame.OwnerSeat, frame.SkillInstanceId),
            frame.GameplayHash, spec);
        if (frame.SlashRequest is { } previous)
        {
            if (previous.Identity.InstructionIndex == identity.InstructionIndex && previous.Identity.IssuanceSlot == issuanceSlot)
            {
                if (!SameIdentity(previous.Identity, identity))
                    throw new InvalidOperationException("An issued Slash request cannot change on reentry.");
                return previous.Identity;
            }
            if (previous.Identity.InstructionIndex > identity.InstructionIndex ||
                previous.Identity.InstructionIndex == identity.InstructionIndex && previous.Identity.IssuanceSlot > issuanceSlot ||
                previous.Identity.InstructionIndex == identity.InstructionIndex && previous.Acceptance is null && !previous.Closed)
                throw new InvalidOperationException("Only the next original request ordinal can replace a settled Slash request.");
        }
        frames.Replace(frame.Id, frame with { SlashRequest = new(identity) });
        return identity;
    }

    public ProgramSlashRequestIdentity? GetIdentity(ProgramSkillFrame supplied)
    {
        if (frames.Find(supplied.Id) is not ProgramSkillFrame { SlashRequest: { } state } frame ||
            state.Identity.FrameId != frame.Id || state.Identity.InstructionIndex != frame.InstructionIndex ||
            state.Identity.Source != new CardConversionSource(frame.SkillId, frame.TriggerId ?? frame.ActivationId,
                frame.OwnerSeat, frame.SkillInstanceId) || state.Identity.GameplayHash != frame.GameplayHash)
            return null;
        return state.Identity;
    }

    public ProgramSlashRequestIdentity? GetOffer(ProgramSkillFrame supplied, int? actorSeat = null)
    {
        if (GetIdentity(supplied) is not { } identity ||
            frames.Find(supplied.Id) is not ProgramSkillFrame { SlashRequest: { Acceptance: null, Closed: false } } frame ||
            actorSeat is { } actor && actor != identity.ActorSeat || !Valid(identity.ActorSeat) ||
            !participants.At(identity.ActorSeat).IsAlive ||
            !identity.Spec.RetainOwnerDeath && !participants.At(frame.OwnerSeat).IsAlive ||
            !identity.Spec.RetainSourceLoss && !facts.CanInvokeProgram(frame))
            return null;
        return identity;
    }

    public bool OwnsIssuedIdentity(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity, int actorSeat) =>
        GetIdentity(frame) is { } actual && actual.ActorSeat == actorSeat && SameIdentity(actual, identity);

    public IReadOnlyList<int> GetNearestLivingSeats(int actorSeat)
    {
        if (!Valid(actorSeat)) return [];
        var minimum = int.MaxValue;
        var nearest = new List<int>();
        foreach (var player in participants.Players)
        {
            if (!player.IsAlive || player.Seat == actorSeat) continue;
            var distance = facts.GetCombatDistance(actorSeat, player.Seat);
            if (distance < minimum) { minimum = distance; nearest.Clear(); }
            if (distance == minimum) nearest.Add(player.Seat);
        }
        nearest.Sort();
        return Array.AsReadOnly(nearest.ToArray());
    }

    public IReadOnlyList<int> GetTargets(ProgramSkillFrame frame, int actorSeat)
    {
        if (GetIdentity(frame) is { } issued && issued.ActorSeat == actorSeat &&
            frames.Find(frame.Id) is ProgramSkillFrame { SlashRequest.Acceptance: { } accepted } &&
            SameIdentity(issued, accepted.Identity))
            return Array.AsReadOnly(new[] { accepted.TargetSeat });
        if (GetOffer(frame, actorSeat) is not { } identity) return [];
        return Targets(identity, (ProgramSkillFrame)frames.Find(frame.Id)!);
    }

    private IReadOnlyList<int> Targets(ProgramSlashRequestIdentity identity, ProgramSkillFrame frame)
    {
        var spec = identity.Spec;
        var actorSeat = identity.ActorSeat;
        IReadOnlyList<int> targets;
        if (spec.TargetSource == ProgramSlashRequestTargetSource.FrozenCandidates)
            targets = Array.AsReadOnly(spec.CandidateSeats.Where(seat => participants.At(seat).IsAlive).ToArray());
        else if (spec.TargetSource == ProgramSlashRequestTargetSource.AllOtherLiving)
            targets = Array.AsReadOnly(participants.Players.Where(player => player.IsAlive && player.Seat != actorSeat)
                .Select(player => player.Seat).ToArray());
        else
        {
            var nearest = GetNearestLivingSeats(actorSeat);
            targets = spec.NearestStrictSnapshot && !nearest.SequenceEqual(spec.CandidateSeats) ? [] : nearest;
        }
        if (spec.RangeOwnerSeat is { } rangeOwner)
            targets = Array.AsReadOnly(targets.Where(target => participants.At(rangeOwner).IsAlive && target != rangeOwner &&
                ranges.IsWithinAttackRange(rangeOwner, target)).ToArray());
        return frame.SlashRequest?.SelectedTargetSeat is { } selected
            ? targets.Contains(selected) ? new[] { selected } : [] : targets;
    }

    public IReadOnlyList<int> GetIssuedTargets(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity, int actorSeat)
    {
        if (!OwnsIssuedIdentity(frame, identity, actorSeat)) return [];
        var actual = (ProgramSkillFrame)frames.Find(frame.Id)!;
        return actual.SlashRequest!.Acceptance is { } accepted
            ? Array.AsReadOnly(new[] { accepted.TargetSeat }) : Targets(identity, actual);
    }

    public bool OwnsOffer(ProgramSkillFrame frame, int actorSeat, int? targetSeat = null) =>
        GetOffer(frame, actorSeat) is { } identity && (targetSeat is null ||
            Targets(identity, (ProgramSkillFrame)frames.Find(frame.Id)!).Contains(targetSeat.Value));

    public void SelectTarget(ProgramSkillFrame supplied, int actorSeat, int targetSeat)
    {
        var frame = FindProgram(supplied.Id);
        if (!OwnsOffer(frame, actorSeat, targetSeat))
            throw new InvalidOperationException("A Slash request target must be selected from its original live offer.");
        frames.Replace(frame.Id, frame with { SlashRequest = frame.SlashRequest! with { SelectedTargetSeat = targetSeat } });
    }

    public ProgramSlashRequestAcceptance Accept(ProgramSkillFrame supplied, int actorSeat, int targetSeat)
    {
        var frame = FindProgram(supplied.Id);
        if (GetIdentity(frame) is not { } identity || identity.ActorSeat != actorSeat)
            throw new InvalidOperationException("An accepted Slash request lost its original actor and instruction.");
        if (frame.SlashRequest!.Acceptance is not null) return Commit(frame, identity, targetSeat);
        if (!OwnsOffer(frame, actorSeat, targetSeat))
            throw new InvalidOperationException("A Slash request can only accept a target from its actual live offer.");
        return Commit(frame, identity, targetSeat);
    }

    public ProgramSlashRequestAcceptance AcceptIssued(ProgramSkillFrame supplied, ProgramAssistedSlashAuthorization authorization,
        int actorSeat, int targetSeat)
    {
        var frame = FindProgram(supplied.Id);
        if (authorization.RequestIdentity is not { } identity || !OwnsIssuedIdentity(frame, identity, actorSeat) ||
            authorization.InstructionIndex != frame.InstructionIndex || authorization.TargetSeat != targetSeat || !Valid(targetSeat) ||
            frame.SlashRequest!.SelectedTargetSeat is { } selected && selected != targetSeat)
            throw new InvalidOperationException("An issued native Slash response lost its original request authorization.");
        return Commit(frame, identity, targetSeat);
    }

    public ProgramSlashRequestAcceptance AcceptIssued(ProgramSkillFrame supplied, ProgramSlashRequestIdentity identity,
        int actorSeat, int targetSeat)
    {
        var frame = FindProgram(supplied.Id);
        if (!OwnsIssuedIdentity(frame, identity, actorSeat) || !Valid(targetSeat) ||
            frame.SlashRequest!.Acceptance is null && !GetIssuedTargets(frame, identity, actorSeat).Contains(targetSeat))
            throw new InvalidOperationException("A paid Slash request continuation lost its original target scope.");
        return Commit(frame, identity, targetSeat);
    }

    private ProgramSlashRequestAcceptance Commit(ProgramSkillFrame frame, ProgramSlashRequestIdentity identity, int targetSeat)
    {
        var state = frame.SlashRequest!;
        if (state.Acceptance is { } previous)
        {
            if (previous.TargetSeat != targetSeat || !SameIdentity(previous.Identity, identity))
                throw new InvalidOperationException("An accepted Slash request cannot change its committed target.");
            return previous;
        }
        if (state.Closed)
            throw new InvalidOperationException("A closed unpaid Slash request cannot accept a new payment attempt.");
        var ordinal = checked(state.LastAttemptOrdinal + 1);
        var accepted = new ProgramSlashRequestAcceptance(identity, targetSeat, ordinal);
        frames.Replace(frame.Id, frame with { SlashRequest = state with
            { Acceptance = accepted, LastAttemptOrdinal = ordinal } });
        return accepted;
    }

    public bool OwnsAcceptance(ProgramSkillFrame frame, ProgramSlashRequestAcceptance acceptance,
        int actorSeat, int? targetSeat = null) =>
        GetIdentity(frame) is { } identity && frames.Find(frame.Id) is ProgramSkillFrame { SlashRequest.Acceptance: { } actual } &&
        identity.ActorSeat == actorSeat && (targetSeat is null || actual.TargetSeat == targetSeat) &&
        actual.TargetSeat == acceptance.TargetSeat && actual.AttemptOrdinal == acceptance.AttemptOrdinal &&
        SameIdentity(identity, actual.Identity) && SameIdentity(identity, acceptance.Identity);

    public bool CanExecuteAcceptedUse(ProgramSkillFrame supplied, ProgramSlashRequestAcceptance acceptance)
    {
        if (frames.Find(supplied.Id) is not ProgramSkillFrame frame ||
            !OwnsAcceptance(frame, acceptance, acceptance.Identity.ActorSeat, acceptance.TargetSeat)) return false;
        var state = frame.SlashRequest!;
        if (state.Failure is { } failed && failed.Acceptance == acceptance) return false;
        if (state.PreparingNativeUse || state.ExecutionFrameId is not null) return true;
        var spec = acceptance.Identity.Spec;
        return (spec.RetainAcceptedOwnerDeath || participants.At(frame.OwnerSeat).IsAlive) &&
            (spec.RetainAcceptedSourceLoss || facts.CanInvokeProgram(frame));
    }

    public bool ReleaseAcceptance(ProgramSkillFrame supplied, ProgramSlashRequestAcceptance acceptance)
    {
        if (frames.Find(supplied.Id) is not ProgramSkillFrame frame ||
            !OwnsAcceptance(frame, acceptance, acceptance.Identity.ActorSeat, acceptance.TargetSeat)) return false;
        var state = frame.SlashRequest!;
        if (state.PreparingNativeUse || state.ExecutionFrameId is not null)
            throw new InvalidOperationException("A submitted native Slash protocol cannot release its accepted payment attempt.");
        if (state.Failure is { Consumed: false })
            throw new InvalidOperationException("A failed Slash payment must return to its owning instruction before it can be retried.");
        frames.Replace(frame.Id, frame with { SlashRequest = state with
            { Acceptance = null, Failure = null, LastAttemptOrdinal = Math.Max(state.LastAttemptOrdinal, acceptance.AttemptOrdinal) } });
        return true;
    }

    public ProgramSlashRequestFailure FailAcceptedUse(ProgramSkillFrame supplied, ProgramSlashRequestAcceptance acceptance,
        long actualProducerFrameId)
    {
        var frame = FindProgram(supplied.Id);
        if (actualProducerFrameId <= 0 || !OwnsAcceptance(frame, acceptance, acceptance.Identity.ActorSeat, acceptance.TargetSeat))
            throw new InvalidOperationException("A no-use return requires its actual producer and original accepted payment attempt.");
        var state = frame.SlashRequest!;
        if (state.Failure is { } previous)
        {
            if (previous.Acceptance != acceptance || previous.ProducerFrameId != actualProducerFrameId)
                throw new InvalidOperationException("A failed Slash payment cannot change its actual producer or attempt.");
            return previous;
        }
        if (state.PreparingNativeUse || state.ExecutionFrameId is not null)
            throw new InvalidOperationException("An issued native Slash cannot return as an unexecuted payment.");
        var failure = new ProgramSlashRequestFailure(acceptance, actualProducerFrameId);
        frames.Replace(frame.Id, frame with { SlashRequest = state with { Failure = failure } });
        return failure;
    }

    public ProgramSlashRequestFailure FailIssuedUse(ProgramSkillFrame supplied, ProgramSlashRequestIdentity identity,
        int actorSeat, int targetSeat, long actualProducerFrameId)
    {
        var frame = FindProgram(supplied.Id);
        if (!OwnsIssuedIdentity(frame, identity, actorSeat) || actualProducerFrameId <= 0 || !Valid(targetSeat) || targetSeat == actorSeat)
            throw new InvalidOperationException("A paid no-use return lost its original issued request and producer.");
        var state = frame.SlashRequest!;
        if (state.Acceptance is { } accepted)
        {
            if (accepted.TargetSeat != targetSeat)
                throw new InvalidOperationException("A paid no-use return cannot substitute its original accepted target.");
            return FailAcceptedUse(frame, accepted, actualProducerFrameId);
        }
        if (state.PreparingNativeUse || state.ExecutionFrameId is not null ||
            state.SelectedTargetSeat is { } selected && selected != targetSeat ||
            (identity.Spec.TargetSource == ProgramSlashRequestTargetSource.FrozenCandidates || identity.Spec.NearestStrictSnapshot) &&
                !identity.Spec.CandidateSeats.Contains(targetSeat))
            throw new InvalidOperationException("A paid no-use return cannot replace its original issued target scope.");
        // A genuine paid producer may discover that the target died after payment.
        // Capture only its failed return, atomically; never manufacture a usable acceptance first.
        var ordinal = checked(state.LastAttemptOrdinal + 1);
        var acceptance = new ProgramSlashRequestAcceptance(identity, targetSeat, ordinal);
        var failure = new ProgramSlashRequestFailure(acceptance, actualProducerFrameId);
        frames.Replace(frame.Id, frame with { SlashRequest = state with
            { Acceptance = acceptance, LastAttemptOrdinal = ordinal, Failure = failure } });
        return failure;
    }

    public ProgramSlashRequestFailure? TakeFailure(ProgramSkillFrame supplied)
    {
        if (frames.Find(supplied.Id) is not ProgramSkillFrame { SlashRequest.Failure: { Consumed: false } failure } frame ||
            !OwnsAcceptance(frame, failure.Acceptance, failure.Acceptance.Identity.ActorSeat, failure.Acceptance.TargetSeat)) return null;
        frames.Replace(frame.Id, frame with { SlashRequest = frame.SlashRequest! with { Failure = failure with { Consumed = true } } });
        return failure;
    }

    public void Close(ProgramSkillFrame supplied)
    {
        var frame = FindProgram(supplied.Id);
        if (GetIdentity(frame) is null) return;
        frames.Replace(frame.Id, frame with { SlashRequest = frame.SlashRequest! with { Closed = true } });
    }

    private bool Valid(int seat) => seat >= 0 && seat < participants.Players.Count;
    private ProgramSkillFrame FindProgram(long id) => frames.Find(id) as ProgramSkillFrame ??
        throw new InvalidOperationException("A native Slash request lost its explicit original program frame.");
    internal static bool SameIdentity(ProgramSlashRequestIdentity a, ProgramSlashRequestIdentity b) =>
        a.FrameId == b.FrameId && a.InstructionIndex == b.InstructionIndex && a.IssuanceSlot == b.IssuanceSlot &&
        a.ActorSeat == b.ActorSeat && a.Source == b.Source && a.GameplayHash == b.GameplayHash && SameSpec(a.Spec, b.Spec);
    private static bool SameSpec(ProgramSlashRequestSpec a, ProgramSlashRequestSpec b) => a == b;
}
