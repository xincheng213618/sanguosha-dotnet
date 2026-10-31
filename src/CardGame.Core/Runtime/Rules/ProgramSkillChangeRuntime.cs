namespace CardGame.Core;
internal enum ProgramSkillSourceRemoval
{
    Enabled,
    All,
    NonModePreserveProjections
}

/// <summary>
/// Instruction-bound skill mutations. Rule-specific triggers, paid native
/// children and expiry qualification remain with their original callers.
/// </summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramSkillChangeRuntime(IProgramSkillChangeFacts facts, IProgramSkillChangeOperations operations, ISkillGrantRevisionObservations observations, IMatchParticipantStateAccess participants, IProgramTurnQueries turn, IActualDiscardRecoveryPhaseQueries phases, IProgramHealthChildQueries healthChildren) : IProgramSkillAcquisitions
{
    void IProgramSkillAcquisitions.Grant(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds) => Grant(frame, recipientSeat, skillIds);
    SkillGrant IProgramSkillAcquisitions.GrantOwnedSource(ProgramSkillFrame frame, string sourceId, string skillId) => GrantOwnedSource(frame, sourceId, skillId);
    internal ProgramSkillFrame Prepare(ProgramSkillFrame supplied) => facts.ActiveFrame(supplied.Id);

    private SkillStateBoundary? _processingBoundary;
    internal int ActualTurnNumber => turn.TurnNumber;
    internal IEnumerable<IGameEvent> AdvancingFacts => facts.AdvancingFacts;

    internal SkillGrant[] Candidates(int seat, SkillStateMatch match, string? sourceSkillId = null,
        IProgramExpressionWorld? bindings = null) => !facts.IsValidSeat(seat) ? [] : facts.Grants(seat)
            .Where(grant => grant.IsEnabled && SkillStateMatcher.Matches(match, grant, facts.Player(seat),
                facts.Definition(grant.SkillId), sourceSkillId, bindings)).ToArray();

    internal int ChangeState(ProgramSkillFrame supplied, int seat, SkillStateSpec spec, IProgramExpressionWorld? bindings = null)
    {
        var frame = Prepare(supplied);
        var origin = new SkillStateOrigin(frame.Id, frame.InstructionIndex - 1,
            new(frame.SkillId, BindingId(frame), frame.OwnerSeat, frame.SkillInstanceId));
        return IssueSkillState(origin, seat, spec, bindings);
    }

    // The issuer supplies an already accepted rule source. Native use and turn
    // observers share this mutation without manufacturing a live program frame.
    internal int IssueSkillState(SkillStateOrigin origin, int seat, SkillStateSpec spec, IProgramExpressionWorld? bindings = null)
    {
        if (!facts.IsValidSeat(seat)) return 0;
        facts.RequireSkill(origin.Source.SkillId);
        foreach (var skill in spec.Match.SkillIds ?? []) facts.RequireSkill(skill);
        origin = origin with { IssuedTurn = turn.TurnNumber };
        var lifetime = NormalizeLifetime(origin, origin.Source.OwnerSeat, seat, spec.Until);
        if (lifetime is null) return 0;
        var grants = Candidates(seat, spec.Match, origin.Source.SkillId, bindings);
        var ids = grants.Select(grant => grant.SkillId).Distinct(StringComparer.Ordinal).ToArray();
        var match = spec.Match.IsTransient ? spec.Match.Freeze(grants) : spec.Match;
        var owner = origin.Source.OwnerSeat;
        IReadOnlyList<SkillGrant> previous = spec.Action == SkillStateAction.Transfer ? facts.OwnedGrants(owner) : [];
        var source = $"skill-state:{origin.FrameId}:{origin.EffectIndex}:{seat}:{origin.Source.SkillInstanceId}";
        if (facts.Player(seat).SkillGrants.Restrictions.Any(restriction => restriction.Id == source) ||
            facts.Grants(owner).Any(grant => grant.SourceId == source))
            throw new InvalidOperationException("An active skill-state source cannot be issued a second time.");
        observations.Capture(participants.Players);
        if (spec.Action == SkillStateAction.Lose && spec.Until.Count == 0 && spec.While is null)
            foreach (var grant in grants) operations.SetGrantEnabled(seat, grant.GrantId, false);
        else
        {
            if (spec.Action == SkillStateAction.Lose) match = match.Freeze(grants);
            if (spec.Action == SkillStateAction.Transfer) match = new(ids, match.Filter, match.GrantIds);
            operations.SetRestriction(seat, new(source, source,
                spec.Action == SkillStateAction.Transfer ? SkillStateAction.Suppress : spec.Action,
                match, new(owner, seat, phases.Current()), lifetime, spec.While));
        }
        if (spec.Action == SkillStateAction.Transfer)
        {
            var acquired = ids.Where(id => !previous.Any(g => g.SkillId == id)).ToArray();
            foreach (var id in ids)
            {
                var key = $"{source}:acquire:{id}";
                var instance = previous.Where(g => g.SkillId == id).OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal)
                    .FirstOrDefault()?.SkillInstanceId ?? $"skill-state-shared:{owner}:{id}";
                operations.Grant(owner, new SkillGrant(key, id, instance, source) { Lifetime = lifetime });
            }
            foreach (var id in acquired) operations.RegisterTaggedConversionSkill(owner, id);
            if (acquired.Length > 0)
                operations.AdvanceFact(new SkillsAcquiredEvent(owner, origin.Source.SkillId, Array.AsReadOnly(acquired)));
        }
        var changed = spec.Action == SkillStateAction.Suppress && match.SkillIds is { } named ? named : ids;
        foreach (var id in changed.DefaultIfEmpty(origin.Source.SkillId))
            operations.AdvanceFact(new SkillStateChangedEvent(seat, owner, id, spec.Action, source) { Origin = origin, IssuedTurn = turn.TurnNumber });
        if (spec.Action == SkillStateAction.Lose)
        {
            operations.AdvanceFact(new CharacterSkillsLostEvent(seat, owner, Array.AsReadOnly(ids)));
            if (spec.Until.Count == 0 && spec.While is null && spec.Match.StructurallyEquals(SkillStateMatch.CharacterSkills))
                operations.Log("SkillTriggered", $"{facts.Name(seat)} 受到【{facts.SkillName(origin.Source.SkillId)}】影响，失去所有武将技能。", owner, seat);
        }
        return ids.Length;
    }

    internal SkillStateLifetime? CreateApplicationLifetime(int owner, int target,
        IReadOnlyList<SkillStateBoundarySpec> until, SkillStateConditionContext context) =>
        NormalizeLifetime(null, owner, target, until, context);

    private SkillStateLifetime? NormalizeLifetime(SkillStateOrigin? origin, int owner, int target,
        IReadOnlyList<SkillStateBoundarySpec> until, SkillStateConditionContext? context = null)
    {
        var ends = new List<SkillStateBoundaryEnd>();
        foreach (var boundary in until)
        {
            int? seat = boundary.Participant switch
            {
                SkillStateSubject.Owner => owner,
                SkillStateSubject.Target => target,
                SkillStateSubject.TurnOwner => turn.TurnOwnerSeat,
                _ => null
            };
            if (seat is { } participant && !facts.IsValidSeat(participant)) return null;
            var current = AdvancingBoundary(boundary.Event, seat, boundary.Phase) ?? _processingBoundary;
            var occurrence = current is { } active && active.Event == boundary.Event &&
                active.Seat == seat && active.Phase == boundary.Phase ? active.Occurrence : boundary.Event switch
                {
                    SkillStateBoundaryEvent.RoundAdvanced => facts.RoundNumber,
                    SkillStateBoundaryEvent.PhaseStarted or SkillStateBoundaryEvent.PhaseEnded => phases.Current()?.Token ?? 0,
                    SkillStateBoundaryEvent.DyingStarted or SkillStateBoundaryEvent.DyingEnded =>
                        context?.IssuedDyingFrameId ?? healthChildren?.ActiveDying?.Id ?? 0,
                    SkillStateBoundaryEvent.Death => seat is { } victim ? facts.DeathOccurrence(victim) : 0,
                    _ => context?.IssuedTurnNumber ?? turn.TurnNumber
                };
            ends.Add(new(boundary.Event, seat, boundary.Timing, occurrence, boundary.SkipIssued, boundary.Phase));
        }
        return new(origin, ends);
    }

    private SkillStateBoundary? AdvancingBoundary(SkillStateBoundaryEvent kind, int? seat, ActualDiscardRecoveryPhaseKind? phase)
    {
        foreach (var item in facts.AdvancingFacts)
        {
            SkillStateBoundary? boundary = item switch
            {
                ActualDiscardRecoveryPhaseStartedEvent fact => new(SkillStateBoundaryEvent.PhaseStarted, fact.Phase.ActorSeat, SkillStateBoundaryTiming.Before, fact.Phase.Token, fact.Phase.Kind),
                ActualPhaseEndedEvent fact => new(SkillStateBoundaryEvent.PhaseEnded, fact.Phase.ActorSeat, SkillStateBoundaryTiming.Before, fact.Phase.Token, fact.Phase.Kind),
                TurnStartedEvent fact => new(SkillStateBoundaryEvent.TurnStarted, fact.ActorSeat, SkillStateBoundaryTiming.Before, fact.TurnNumber),
                TurnEndedEvent fact => new(SkillStateBoundaryEvent.TurnEnded, fact.ActorSeat, SkillStateBoundaryTiming.Before, fact.TurnNumber),
                PlayerDiedEvent fact => new(SkillStateBoundaryEvent.Death, fact.VictimSeat, SkillStateBoundaryTiming.Before, facts.DeathOccurrence(fact.VictimSeat)),
                PlayerDyingEvent fact => new(SkillStateBoundaryEvent.DyingStarted, null, SkillStateBoundaryTiming.Before, fact.ResolutionId),
                DyingResolvedEvent fact => new(SkillStateBoundaryEvent.DyingEnded, null, SkillStateBoundaryTiming.Before, fact.ResolutionId),
                _ => null
            };
            if (boundary is not null && boundary.Event == kind && boundary.Seat == seat && boundary.Phase == phase) return boundary;
        }
        return null;
    }

    internal void ProcessSkillStateBoundary(SkillStateBoundary boundary)
    {
        var previous = _processingBoundary;
        _processingBoundary = boundary;
        try
        {
            ExpireSkillStates(lifetime => lifetime.Until.Any(end => end.Event == boundary.Event &&
                end.Seat == boundary.Seat && end.Timing == boundary.Timing && end.Phase == boundary.Phase &&
                (end.Event == SkillStateBoundaryEvent.DyingEnded && !end.SkipIssued
                    ? boundary.Occurrence == end.IssuedOccurrence
                    : end.SkipIssued ? boundary.Occurrence > end.IssuedOccurrence : boundary.Occurrence >= end.IssuedOccurrence)));
        }
        finally { _processingBoundary = previous; }
    }

    internal void RevokeSkillStateOrigin(SkillStateOrigin origin) => ExpireSkillStates(lifetime =>
        lifetime.Origin is { } issued && issued.FrameId == origin.FrameId && issued.EffectIndex == origin.EffectIndex &&
        issued.Source == origin.Source);

    private void ExpireSkillStates(Func<SkillStateLifetime, bool> expires)
    {
        var restrictions = participants.Players.SelectMany(player => player.SkillGrants.Restrictions
            .Where(restriction => expires(restriction.Lifetime)).Select(restriction => (player.Seat, Restriction: restriction))).ToArray();
        var grants = participants.Players.SelectMany(player => player.SkillGrants.Grants
            .Where(grant => grant.Lifetime is { } lifetime && expires(lifetime)).Select(grant => (player.Seat, Grant: grant))).ToArray();
        if (restrictions.Length == 0 && grants.Length == 0) return;
        observations.Capture(participants.Players);
        foreach (var (seat, restriction) in restrictions) operations.RemoveRestriction(seat, restriction.Id);
        foreach (var (seat, grant) in grants) operations.RemoveGrant(seat, grant.GrantId);
        var expired = restrictions.Select(item => new SkillStateExpiredEvent(item.Seat, item.Restriction.SourceId, item.Restriction.Lifetime.Origin))
            .Concat(grants.Select(item => new SkillStateExpiredEvent(item.Seat, item.Grant.SourceId, item.Grant.Lifetime!.Origin))).Distinct();
        foreach (var fact in expired) operations.AdvanceFact(fact);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 25)]
    internal void BeforeBoundaryEvent(IGameEvent fact)
    {
        switch (fact)
        {
            case ActualDiscardRecoveryPhaseStartedEvent started: PhaseBoundary(started.Phase, SkillStateBoundaryEvent.PhaseStarted, SkillStateBoundaryTiming.Before); break;
            case ActualPhaseEndedEvent ended: PhaseBoundary(ended.Phase, SkillStateBoundaryEvent.PhaseEnded, SkillStateBoundaryTiming.Before); break;
            case PlayerDiedEvent death: ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.Death, death.VictimSeat, SkillStateBoundaryTiming.Before, facts.DeathOccurrence(death.VictimSeat))); break;
        }
    }
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 25)]
    internal void AfterBoundaryEvent(IGameEvent fact)
    {
        switch (fact)
        {
            case PlayerDiedEvent death: ProcessSkillStateBoundary(new(SkillStateBoundaryEvent.Death, death.VictimSeat, SkillStateBoundaryTiming.After, facts.DeathOccurrence(death.VictimSeat))); break;
            case ActualDiscardRecoveryPhaseStartedEvent started: PhaseBoundary(started.Phase, SkillStateBoundaryEvent.PhaseStarted, SkillStateBoundaryTiming.After); break;
            case ActualPhaseEndedEvent ended: PhaseBoundary(ended.Phase, SkillStateBoundaryEvent.PhaseEnded, SkillStateBoundaryTiming.After); break;
        }
    }
    private void PhaseBoundary(ActualDiscardRecoveryPhaseKey phase, SkillStateBoundaryEvent kind, SkillStateBoundaryTiming timing) =>
        ProcessSkillStateBoundary(new(kind, phase.ActorSeat, timing, phase.Token, phase.Kind));

    internal bool TryUpgradeConversionTier(ProgramSkillFrame supplied, string state, int? expectedTier)
    {
        var frame = Prepare(supplied);
        var tier = facts.ConversionTier(frame.OwnerSeat, state);
        if (tier is < 0 or > 2)
            throw new InvalidOperationException("A conversion modification count is outside its defined versions.");
        if (tier == 2 || expectedTier is { } expected && tier != expected)
            return false;
        CommitConversionTier(frame.OwnerSeat, state, tier + 1);
        operations.Log("SkillTriggered", $"{facts.Name(frame.OwnerSeat)} 的转换能力修改至第 {tier + 1} 层。", frame.OwnerSeat);
        return true;
    }

    internal bool CommitConversionTier(int ownerSeat, string state, int tier)
    {
        if (tier is < 0 or > 2)
            throw new InvalidOperationException("A conversion modification count is outside its defined versions.");
        if (facts.ConversionTier(ownerSeat, state) == tier)
            return false;
        operations.WriteConversionTier(ownerSeat, state, tier);
        operations.AdvanceFact(new ConfiguredConversionTierChangedEvent(ownerSeat, state, tier));
        return true;
    }

    internal void SetBooleanState(ProgramSkillFrame supplied, string skillId, string instanceId, string stateId, bool value)
    {
        var frame = Prepare(supplied);
        operations.SetBooleanState(frame.OwnerSeat, skillId, instanceId, stateId, value);
    }

    internal void Grant(ProgramSkillFrame supplied, int recipientSeat, IReadOnlyList<string> skillIds)
    {
        var frame = Prepare(supplied);
        if (!facts.IsValidSeat(recipientSeat))
            throw new InvalidOperationException("A skill acquisition lost its actual recipient.");
        Acquire(frame, recipientSeat, skillIds, announceAwakening: true);
    }

    internal SkillGrant GrantOwnedSource(ProgramSkillFrame supplied, string sourceId, string skillId)
    {
        var frame = Prepare(supplied);
        var seat = frame.OwnerSeat;
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceId);
        if (!sourceId.StartsWith("acquired:", StringComparison.Ordinal) || !facts.Alive(seat) || frame.PendingMovementContinuation is not null || frame.SelectedCardPayment is not null && frame.SelectedCardPaymentResult is null)
            throw new InvalidOperationException("An issued skill source requires its settled original instruction and living owner.");
        facts.RequireSkill(skillId);
        var grantId = $"{sourceId}:{skillId}";
        if (facts.Grants(seat).SingleOrDefault(g => g.GrantId == grantId)is { } prior)
            return prior.SourceId == sourceId && prior.SkillId == skillId ? prior : throw new InvalidOperationException("An issued skill source conflicts with its original acquisition.");
        var instanceId = facts.ActiveGrants(seat).Where(g => g.SkillId == skillId).OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault()?.SkillInstanceId ?? grantId;
        observations.Capture(participants.Players);
        var acquired = !facts.EnabledSkillIds(seat).Contains(skillId, StringComparer.Ordinal);
        var grant = new SkillGrant(grantId, skillId, instanceId, sourceId);
        operations.Grant(seat, grant);
        if (acquired)
        {
            operations.RegisterTaggedConversionSkill(seat, skillId);
            operations.AdvanceFact(new SkillsAcquiredEvent(seat, frame.SkillId, Array.AsReadOnly(new[] { skillId })));
        }

        return grant;
    }

    internal IReadOnlyList<SkillGrant> RevokeOwnedSources(ProgramSkillFrame supplied, IReadOnlyList<string> grantIds)
    {
        var frame = Prepare(supplied);
        var ids = grantIds.ToHashSet(StringComparer.Ordinal);
        var removed = facts.Grants(frame.OwnerSeat).Where(g => ids.Contains(g.GrantId)).ToArray();
        if (removed.Any(g => g.IsModeRule || !g.SourceId.StartsWith("acquired:", StringComparison.Ordinal)))
            throw new InvalidOperationException("Source revocation may remove only explicitly issued acquisition grants.");
        observations.Capture(participants.Players);
        foreach (var grant in removed)
            operations.RemoveGrant(frame.OwnerSeat, grant.GrantId);
        return Array.AsReadOnly(removed);
    }

    internal void GrantIssuedTurnSkills(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, SkillGrantTurnExpiry expiry)
    {
        var sourceId = $"turn:{expiry.TurnNumber}:{expiry.TurnOwnerSeat}:native:{frame.OwnerSeat}:{frame.SkillId}:" + $"{frame.SkillInstanceId}:{BindingId(frame)}:frame:{frame.Id}:effect:{frame.InstructionIndex - 1}:recipient:{recipientSeat}";
        GrantParticipantSkills(frame, recipientSeat, skillIds, sourceId, expiry, null);
    }

    internal void GrantParticipantSkills(ProgramSkillFrame supplied, int recipientSeat, IReadOnlyList<string> skillIds, string sourceId, SkillGrantTurnExpiry? turnExpiry, SkillGrantNextActualTurnStartExpiry? nextStartExpiry)
    {
        var active = Prepare(supplied);
        if (!facts.IsValidSeat(recipientSeat) || active.PendingMovementContinuation is not null || active.SelectedCardPayment is not null && active.SelectedCardPaymentResult is null)
            throw new InvalidOperationException("A participant skill grant requires its exact settled instruction and recipient.");
        if (!facts.Alive(recipientSeat))
            return;
        var granted = skillIds.Distinct(StringComparer.Ordinal).ToArray();
        foreach (var skillId in granted)
            facts.RequireSkill(skillId);
        foreach (var skillId in granted)
        {
            var existing = facts.ActiveGrants(recipientSeat).Where(grant => grant.SkillId == skillId).OrderBy(grant => grant.SourceId.StartsWith("turn:", StringComparison.Ordinal) ? 1 : 0).ThenBy(grant => grant.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
            var instanceId = existing?.SkillInstanceId ?? $"turn-shared:{recipientSeat}:{skillId}";
            operations.Grant(recipientSeat, new SkillGrant($"{sourceId}:{skillId}", skillId, instanceId, sourceId, TurnExpiry: turnExpiry) { NextActualTurnStartExpiry = nextStartExpiry });
        }

        operations.AdvanceFact(new ProgramTurnSkillsGrantedEvent(active.Id, active.SkillId, BindingId(active), active.OwnerSeat, Array.AsReadOnly(granted)) { RecipientSeat = recipientSeat, GrantSourceId = sourceId, TurnExpiry = turnExpiry, NextActualTurnStartExpiry = nextStartExpiry });
    }

    internal IReadOnlyList<string> Acquire(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds, bool announceAwakening)
    {
        var acquired = recipientSeat == frame.OwnerSeat && facts.IsDyingOwnerReplacement(frame) ? operations.AcquireDyingOwnerReplacementSkills(frame, skillIds) : AcquireRuntimeSkills(recipientSeat, frame.SkillId, skillIds);
        if (announceAwakening && facts.IsAwakening(frame.SkillId))
            operations.AdvanceFact(new SkillAwakenedEvent(recipientSeat, frame.SkillId, facts.MaximumHp(recipientSeat), acquired));
        return acquired;
    }

    internal IReadOnlyList<string> AcquireRuntimeSkills(int seat, string sourceSkillId, IReadOnlyList<string> skillIds)
    {
        var acquired = new List<string>();
        foreach (var skillId in skillIds.Distinct(StringComparer.Ordinal))
        {
            facts.RequireSkill(skillId);
            if (!facts.EnabledSkillIds(seat).Contains(skillId, StringComparer.Ordinal))
                acquired.Add(skillId);
        }

        var sourceId = $"acquired:{sourceSkillId}";
        foreach (var skillId in skillIds.Distinct(StringComparer.Ordinal))
        {
            var grantId = $"{sourceId}:{skillId}";
            if (facts.Grants(seat).Any(grant => grant.GrantId == grantId))
                continue;
            operations.Grant(seat, new SkillGrant(grantId, skillId, grantId, sourceId));
        }

        if (acquired.Count == 0)
            return[];
        foreach (var skillId in acquired)
            operations.RegisterTaggedConversionSkill(seat, skillId);
        operations.AdvanceFact(new SkillsAcquiredEvent(seat, sourceSkillId, Array.AsReadOnly(acquired.ToArray())));
        return Array.AsReadOnly(acquired.ToArray());
    }

    internal void Replace(ProgramSkillFrame supplied, IReadOnlyList<string> lost, IReadOnlyList<string> acquired, ProgramSkillSourceRemoval removal, bool acquireFirst, bool announceAwakening)
    {
        var frame = Prepare(supplied);
        var seat = frame.OwnerSeat;
        if (acquireFirst)
            Acquire(frame, seat, acquired, announceAwakening);
        var grants = removal == ProgramSkillSourceRemoval.NonModePreserveProjections ? lost.SelectMany(id => facts.Grants(seat).Where(g => g.SkillId == id)).ToArray() : facts.Grants(seat).Where(g => lost.Contains(g.SkillId)).ToArray();
        foreach (var grant in grants)
        {
            if (removal == ProgramSkillSourceRemoval.Enabled && !grant.IsEnabled || removal == ProgramSkillSourceRemoval.NonModePreserveProjections && grant.IsModeRule)
                continue;
            if (removal == ProgramSkillSourceRemoval.NonModePreserveProjections && (grant.LordProjection is not null || grant.GeneralLibraryProjection is not null))
                operations.SetGrantEnabled(seat, grant.GrantId, false);
            else
                operations.RemoveGrant(seat, grant.GrantId);
        }

        if (!acquireFirst)
            Acquire(frame, seat, acquired, announceAwakening);
    }

    internal SkillProgramStepOutcome ReplaceOwnerSkills(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = Prepare(supplied);
        var paidContinuation = frame.WindowContext?.Window == SkillProgramTriggerWindow.DyingEntering ? operations.CaptureDyingReplacementContinuation(frame, effect) : null;
        Replace(frame, effect.SkillIds, [effect.SourceBind!], ProgramSkillSourceRemoval.Enabled, acquireFirst: true, announceAwakening: false);
        CommitFact(frame, new ProgramOwnerSkillsReplacedEvent(frame.Id, frame.SkillId, frame.OwnerSeat, effect.SkillIds.ToArray(), effect.SourceBind!));
        if (paidContinuation is not null)
            operations.IssueReplacedPaidHpContinuation(paidContinuation);
        operations.Finish(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal void CommitFact(ProgramSkillFrame frame, IGameEvent fact)
    {
        Prepare(frame);
        operations.AdvanceFact(fact);
    }

    internal SkillGrant CommitExplicitSource(ProgramSkillFrame supplied, SkillGrant grant, IReadOnlyList<string>? revokedGrantIds)
    {
        var frame = Prepare(supplied);
        var seat = frame.OwnerSeat;
        facts.RequireSkill(grant.SkillId);
        if (!facts.Alive(seat))
            throw new InvalidOperationException("A skill source needs its living original owner.");
        var revoked = (revokedGrantIds ?? []).Select(id => facts.Grants(seat).SingleOrDefault(g => g.GrantId == id)).OfType<SkillGrant>().ToArray();
        if (revoked.Any(g => g.IsModeRule))
            throw new InvalidOperationException("A native source replacement cannot remove a mode rule.");
        observations.Capture(participants.Players);
        var acquired = !facts.EnabledSkillIds(seat).Contains(grant.SkillId, StringComparer.Ordinal);
        foreach (var prior in revoked)
            operations.RemoveGrant(seat, prior.GrantId);
        if (facts.Grants(seat).SingleOrDefault(g => g.GrantId == grant.GrantId)is { } existing)
            return existing == grant ? existing : throw new InvalidOperationException("A native skill source changed its paid identity.");
        operations.Grant(seat, grant);
        operations.InvalidateBindings(seat);
        operations.RegisterTaggedConversionSkill(seat, grant.SkillId);
        if (acquired)
            operations.AdvanceFact(new SkillsAcquiredEvent(seat, frame.SkillId, Array.AsReadOnly(new[] { grant.SkillId })));
        return grant;
    }

    internal void RemoveExplicitSources(ProgramSkillFrame supplied, IReadOnlyList<string> grantIds)
    {
        var frame = Prepare(supplied);
        var ids = grantIds.ToHashSet(StringComparer.Ordinal);
        var grants = facts.Grants(frame.OwnerSeat).Where(g => ids.Contains(g.GrantId)).ToArray();
        if (grants.Any(g => g.IsModeRule))
            throw new InvalidOperationException("A native source removal cannot remove a mode rule.");
        observations.Capture(participants.Players);
        foreach (var grant in grants)
            operations.RemoveGrant(frame.OwnerSeat, grant.GrantId);
        operations.InvalidateBindings(frame.OwnerSeat);
    }

    private static string BindingId(ProgramSkillFrame frame) => RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(frame);
}

internal interface IProgramSkillAcquisitions : IProgramRuntimeOperations
{
    void Grant(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds);
    SkillGrant GrantOwnedSource(ProgramSkillFrame frame, string sourceId, string skillId);
}

internal interface IProgramSkillGrantQueries : IProgramSelectionQuery
{
    IReadOnlyList<SkillGrant> Grants(int seat);
    IReadOnlyList<SkillGrant> ActiveGrants(int seat);
    IReadOnlyList<SkillGrant> OwnedGrants(int seat);
}

internal interface IProgramSkillChangeFacts : IProgramSkillGrantQueries
{
    ProgramSkillFrame ActiveFrame(long id);
    bool IsValidSeat(int seat);
    bool Alive(int seat);
    CharacterState Player(int seat);
    ContentSkillDefinition Definition(string skillId);
    long DeathOccurrence(int seat);
    IEnumerable<IGameEvent> AdvancingFacts { get; }
    string Name(int seat);
    string SkillName(string skillId);
    int RoundNumber { get; }
    int MaximumHp(int seat);
    int ConversionTier(int seat, string state);
    void RequireSkill(string skillId);
    bool IsAwakening(string skillId);
    bool IsDyingOwnerReplacement(ProgramSkillFrame frame);
    IReadOnlyList<string> EnabledSkillIds(int seat);
}

internal interface IProgramSkillChangeOperations : IProgramRuntimeOperations
{
    void WriteConversionTier(int seat, string state, int tier);
    void Log(string kind, string message, int seat, int? target = null);
    void SetBooleanState(int seat, string skillId, string instanceId, string stateId, bool value);
    void Grant(int seat, SkillGrant grant);
    void RemoveGrant(int seat, string grantId);
    void SetGrantEnabled(int seat, string grantId, bool enabled);
    void SetRestriction(int seat, SkillStateRestriction restriction);
    void RemoveRestriction(int seat, string restrictionId);
    void InvalidateBindings(int seat);
    void RegisterTaggedConversionSkill(int seat, string skillId);
    void AdvanceFact(IGameEvent fact);
    IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds);
    ProgramReplacedPaidHpContinuation? CaptureDyingReplacementContinuation(ProgramSkillFrame frame, SkillProgramEffect effect);
    void IssueReplacedPaidHpContinuation(ProgramReplacedPaidHpContinuation continuation);
    void Finish(ProgramSkillFrame frame);
}
