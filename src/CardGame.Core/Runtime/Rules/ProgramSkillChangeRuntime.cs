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
internal sealed class ProgramSkillChangeRuntime(IProgramSkillChangeFacts facts, IProgramSkillChangeOperations operations, ISkillGrantRevisionObservations observations, IMatchParticipantStateAccess participants, IProgramTurnQueries turn) : IProgramSkillAcquisitions
{
    void IProgramSkillAcquisitions.Grant(ProgramSkillFrame frame, int recipientSeat, IReadOnlyList<string> skillIds) => Grant(frame, recipientSeat, skillIds);
    SkillGrant IProgramSkillAcquisitions.GrantOwnedSource(ProgramSkillFrame frame, string sourceId, string skillId) => GrantOwnedSource(frame, sourceId, skillId);
    internal ProgramSkillFrame Prepare(ProgramSkillFrame supplied) => facts.ActiveFrame(supplied.Id);

    internal int ChangeState(ProgramSkillFrame supplied, int seat, SkillStateSpec spec, IReadOnlyList<string> skills, int? endSeat = null)
    {
        var frame = Prepare(supplied);
        var origin = new SkillStateOrigin(frame.Id, frame.InstructionIndex - 1,
            new(frame.SkillId, BindingId(frame), frame.OwnerSeat, frame.SkillInstanceId));
        return IssueSkillState(origin, seat, spec, skills, endSeat);
    }

    // The issuer supplies an already accepted rule source. Native use and turn
    // observers share this mutation without manufacturing a live program frame.
    internal int IssueSkillState(SkillStateOrigin origin, int seat, SkillStateSpec spec, IReadOnlyList<string> skills, int? endSeat = null)
    {
        if (!facts.IsValidSeat(seat)) return 0;
        if (spec.Duration == SkillStateDuration.UntilTurnEndOf && (endSeat is null || !facts.IsValidSeat(endSeat.Value))) return 0;
        facts.RequireSkill(origin.Source.SkillId);
        foreach (var skill in skills) facts.RequireSkill(skill);
        observations.Capture(participants.Players);
        var grants = facts.Grants(seat).Where(g => g.IsEnabled && !g.IsModeRule && g.SkillState is not { Acquisition: false } &&
            (!spec.CharacterOnly || SkillStateQuery.IsCharacter(g)) && (!spec.PrintedOnly || SkillStateQuery.IsPrinted(g)) &&
            (!(spec.ExcludeLocked || spec.Scope == SkillStateScope.NonLocked) || !facts.SkillTags(g.SkillId).HasFlag(SkillTag.Locked)) &&
            (spec.Scope is SkillStateScope.All or SkillStateScope.NonLocked || skills.Contains(g.SkillId, StringComparer.Ordinal))).ToArray();
        var ids = grants.Select(g => g.SkillId).Distinct(StringComparer.Ordinal).ToArray();
        var source = $"skill-state:{origin.FrameId}:{origin.EffectIndex}:{seat}:{origin.Source.SkillInstanceId}";
        var scope = spec.Action == SkillStateAction.Transfer ? SkillStateScope.Named : spec.Scope;
        var owner = origin.Source.OwnerSeat;
        if (spec.Action == SkillStateAction.Lose && spec.Duration == SkillStateDuration.Permanent && !spec.ExpireOnDeath)
            foreach (var grant in grants) operations.SetGrantEnabled(seat, grant.GrantId, false);
        else
        {
            var restrictions = spec.Action == SkillStateAction.Lose
                ? grants.Select(g => (Name: (string?)g.SkillId, GrantId: (string?)g.GrantId))
                : (scope is SkillStateScope.Named or SkillStateScope.ChosenFromTarget or SkillStateScope.Expression
                    ? spec.Action == SkillStateAction.Suppress ? skills.Distinct(StringComparer.Ordinal).ToArray() : ids
                    : new string?[] { null })
                    .Select(name => (Name: name, GrantId: (string?)null));
            foreach (var (name, grantId) in restrictions)
            {
                var key = $"{source}:{grantId ?? name ?? "*"}";
                var state = new SkillStateLease(spec.Action == SkillStateAction.Transfer ? SkillStateAction.Suppress : spec.Action,
                    scope, spec.Duration, turn.TurnNumber, endSeat ?? turn.CurrentSeat, name,
                    spec.PrintedOnly, spec.ExcludeLocked, spec.ExpireOnDeath, CharacterOnly: spec.CharacterOnly,
                    AfterCurrentTurn: spec.AfterCurrentTurn) { Origin = origin, DeathSeat = seat, TargetGrantId = grantId, EndTiming = spec.EndTiming };
                operations.Grant(seat, new SkillGrant(key, name ?? origin.Source.SkillId, key, source, false) { SkillState = state });
            }
        }
        if (spec.Action == SkillStateAction.Transfer)
        {
            var previous = facts.OwnedGrants(owner);
            var acquired = ids.Where(id => !previous.Any(g => g.SkillId == id)).ToArray();
            foreach (var id in ids)
            {
                var key = $"{source}:acquire:{id}";
                var instance = previous.Where(g => g.SkillId == id).OrderBy(g => g.SkillInstanceId, StringComparer.Ordinal)
                    .FirstOrDefault()?.SkillInstanceId ?? $"skill-state-shared:{owner}:{id}";
                operations.Grant(owner, new SkillGrant(key, id, instance, source)
                {
                    SkillState = new(SkillStateAction.Transfer, SkillStateScope.Named, spec.Duration,
                        turn.TurnNumber, endSeat ?? turn.CurrentSeat, id, ExpireOnDeath: spec.ExpireOnDeath, Acquisition: true,
                        AfterCurrentTurn: spec.AfterCurrentTurn) { Origin = origin, DeathSeat = seat, EndTiming = spec.EndTiming }
                });
            }
            foreach (var id in acquired) operations.RegisterTaggedConversionSkill(owner, id);
            if (acquired.Length > 0)
                operations.AdvanceFact(new SkillsAcquiredEvent(owner, origin.Source.SkillId, Array.AsReadOnly(acquired)));
        }
        var changed = spec.Action == SkillStateAction.Suppress && scope == SkillStateScope.Named ? skills : ids;
        foreach (var id in changed.DefaultIfEmpty(origin.Source.SkillId))
            operations.AdvanceFact(new SkillStateChangedEvent(seat, owner, id, spec.Action, source) { Origin = origin, IssuedTurn = turn.TurnNumber });
        if (spec.Action == SkillStateAction.Lose)
        {
            operations.AdvanceFact(new CharacterSkillsLostEvent(seat, owner, Array.AsReadOnly(ids)));
            if (spec is { Scope: SkillStateScope.All, Duration: SkillStateDuration.Permanent, CharacterOnly: true })
                operations.Log("SkillTriggered", $"{facts.Name(seat)} 受到【{facts.SkillName(origin.Source.SkillId)}】影响，失去所有武将技能。", owner, seat);
        }
        return ids.Length;
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 25)]
    internal void ExpireSkillStates(IGameEvent fact) => RemoveExpiredStates(s =>
        s.EndTiming == SkillStateEndTiming.AfterTurnEnded && fact is TurnEndedEvent e && EndsAtTurn(s, e.TurnNumber, e.ActorSeat) ||
        s.ExpireOnDeath && fact is PlayerDiedEvent death && death.VictimSeat == (s.DeathSeat ?? s.EndSeat));

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 28001)]
    internal void ExpireSkillStatesBeforeTurnEnded(TurnEndedEvent ended) => RemoveExpiredStates(s =>
        s.EndTiming == SkillStateEndTiming.BeforeTurnEnded && EndsAtTurn(s, ended.TurnNumber, ended.ActorSeat));

    // Preserve the existing actual-turn cleanup boundary, before returned cards
    // can open movement children. Other expiry windows keep their own ordering.
    internal void ExpireCurrentTurnStates(int turnNumber, int turnSeat) => RemoveExpiredStates(s =>
        s.Duration == SkillStateDuration.CurrentTurn && EndsAtTurn(s, turnNumber, turnSeat));

    private static bool EndsAtTurn(SkillStateLease s, int turnNumber, int turnSeat) =>
        s.Duration != SkillStateDuration.Permanent && turnSeat == s.EndSeat &&
        (s.Duration == SkillStateDuration.CurrentTurn ? turnNumber == s.IssuedTurn :
            s.AfterCurrentTurn ? turnNumber > s.IssuedTurn : turnNumber >= s.IssuedTurn);

    private void RemoveExpiredStates(Func<SkillStateLease, bool> expired)
    {
        var ended = participants.Players.SelectMany(player => player.SkillGrants.Grants
            .Where(grant => grant.SkillState is { } state && expired(state))
            .Select(grant => (player.Seat, Grant: grant))).ToArray();
        foreach (var (seat, grant) in ended) operations.RemoveGrant(seat, grant.GrantId);
        foreach (var (seat, grant) in ended)
            operations.AdvanceFact(new SkillStateExpiredEvent(seat, grant.SourceId, grant.SkillState!.Origin));
    }

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
    string Name(int seat);
    string SkillName(string skillId);
    SkillTag SkillTags(string skillId);
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
    void InvalidateBindings(int seat);
    void RegisterTaggedConversionSkill(int seat, string skillId);
    void AdvanceFact(IGameEvent fact);
    IReadOnlyList<string> AcquireDyingOwnerReplacementSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds);
    ProgramReplacedPaidHpContinuation? CaptureDyingReplacementContinuation(ProgramSkillFrame frame, SkillProgramEffect effect);
    void IssueReplacedPaidHpContinuation(ProgramReplacedPaidHpContinuation continuation);
    void Finish(ProgramSkillFrame frame);
}
