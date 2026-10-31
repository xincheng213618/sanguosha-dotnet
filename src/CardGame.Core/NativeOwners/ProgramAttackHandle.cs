#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ProgramAttackHandle(Func<RuntimeEntries.CoreGameEngineProgramAttackAttemptEntries> getCoreGameEngineProgramAttackAttemptRuntime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineRecipientScopedDamageRuntime>> getCoreGameEngineRecipientScopedDamageRuntime, long ownerFrameId) : GameEngine.IDamageAttempt
{
    private AttackAttemptState State => getCoreGameEngineProgramAttackAttemptRuntime().GetProgramAttackState(ownerFrameId);

    private void Update(Func<AttackAttemptState, AttackAttemptState> update) => getCoreGameEngineProgramAttackAttemptRuntime().UpdateProgramAttackState(ownerFrameId, update);
    public long ResolutionId => ownerFrameId;
    public int SourceSeat => State.SourceSeat;
    public int CardUserSeat => State.SourceSeat;
    public int TargetSeat => State.TargetSeat;
    public Card? Card => null;
    public IReadOnlyList<Card> PhysicalCards => [];
    public CardKind? EffectiveCardKind => null;
    public CardConversionSource? ConversionSource => null;
    public int DamageAmount => State.DamageAmount;
    public bool DamageAmountFinalized => State.DamageAmountFinalized;
    public bool IgnoresArmor => State.IgnoresArmor;
    public bool IsDelayedJudgmentDamage => false;
    public bool IsProgramJudgmentDamage => State.ProgramJudgmentWindowId is not null;
    public bool IsSourceLess => State.SourceLess;
    public DamageNature? DamageNatureOverride => State.Nature;
    public bool IsChainPropagation => State.IsChainPropagation;
    public bool DamageWasApplied => State.DamageWasApplied;
    public bool BeforeDamageProgramsResolved => State.BeforeDamageProgramsResolved;
    public bool DamageRedirected => State.DamageRedirected;
    public int SourceToTargetDistanceAtDamage => State.SourceToTargetDistanceAtDamage;

    public bool PendingRedBladeDamageBonus
    {
        get => false;
        set
        {
            if (value)
                throw new InvalidOperationException("Cardless damage cannot use Red Blood Blade.");
        }
    }

    public void SetIgnoresArmor(bool value) => Update(state => state with { IgnoresArmor = value });
    public void FinalizeDamageAmount(int bonus, int? maximum = null) => Update(state =>
    {
        if (state.DamageAmountFinalized)
            throw new InvalidOperationException("The attack damage amount is already final.");
        var amount = Math.Max(0, checked(state.DamageAmount + bonus));
        return state with
        {
            DamageAmount = maximum is { } cap ? Math.Min(amount, cap) : amount,
            DamageAmountFinalized = true
        };
    });
    public void ReduceFinalizedDamageAmount(int amount) => Update(state =>
    {
        if (!state.DamageAmountFinalized || amount <= 0 || amount > state.DamageAmount)
            throw new InvalidOperationException("Damage reduction requires a positive part of the frozen amount.");
        return state with
        {
            DamageAmount = state.DamageAmount - amount
        };
    });
    public void IncreaseFinalizedDamageAmount(int amount) => Update(state =>
    {
        if (!state.DamageAmountFinalized || state.DamageWasApplied || amount <= 0)
            throw new InvalidOperationException("Damage increase requires a positive amount before frozen damage is applied.");
        return state with
        {
            DamageAmount = checked(state.DamageAmount + amount)
        };
    });
    public void MarkBeforeDamageProgramsResolved() => Update(state => state with { BeforeDamageProgramsResolved = true });
    public void MarkDamageApplied() => Update(state => state with { DamageWasApplied = true });
    public void CaptureDamageDistance(int distance) => Update(state => state with { SourceToTargetDistanceAtDamage = distance });
    public void MarkDyingResolvedForDamage(long damageFrameId) => Update(state => state with { ResolvedDyingDamageFrameId = damageFrameId });
    public bool HasResolvedDyingForDamage(long damageFrameId) => State.ResolvedDyingDamageFrameId == damageFrameId;
    public void RedirectFinalizedDamageTarget(int targetSeat, GameEngine.ProgramDamageTransferFollowup followup) => Update(state =>
    {
        if (!state.DamageAmountFinalized || state.DamageRedirected || state.TargetSeat == targetSeat)
            throw new InvalidOperationException("Damage redirection requires a new recipient and a finalized amount.");
        return state with
        {
            TargetSeat = targetSeat,
            DamageRedirected = true,
            TransferSkillId = followup.SkillId,
            TransferOwnerSeat = followup.OwnerSeat,
            TransferTargetSeat = followup.TargetSeat,
            TransferDrawLostHp = followup.DrawLostHp
        };
    });
    public bool TryConsumeProgramDamageTransferFollowup(out GameEngine.ProgramDamageTransferFollowup followup)
    {
        var state = State;
        if (state.TransferSkillId is null || state.TransferOwnerSeat is null || state.TransferTargetSeat is null)
        {
            followup = null !;
            return false;
        }

        followup = new(state.TransferSkillId, state.TransferOwnerSeat.Value, state.TransferTargetSeat.Value, state.TransferDrawLostHp);
        Update(current => current with { TransferSkillId = null, TransferOwnerSeat = null, TransferTargetSeat = null, TransferDrawLostHp = false });
        return true;
    }

    public void SetChainedTargets(IReadOnlyList<int> targetSeats) => Update(state =>
    {
        if (state.ChainedTargetSeats is { Count: > 0 } || state.ChainedTargetIndex != 0)
            throw new InvalidOperationException("The elemental chain targets have already been captured.");
        return state with
        {
            ChainedTargetSeats = Array.AsReadOnly(targetSeats.ToArray())
        };
    });
    public bool TryAdvanceChainedTarget(Func<int, bool> isAlive, bool resetDamageForTargetModifiers, out int fromSeat)
    {
        var state = State;
        fromSeat = state.TargetSeat;
        var targets = state.ChainedTargetSeats ?? [];
        var cursor = state.ChainedTargetIndex;
        while (cursor < targets.Count)
        {
            var next = targets[cursor++];
            if (!isAlive(next))
                continue;
            Update(current => current with { TargetSeat = next, ChainedTargetIndex = cursor, IsChainPropagation = true, DamageAmountFinalized = resetDamageForTargetModifiers ? false : current.DamageAmountFinalized });
            getCoreGameEngineRecipientScopedDamageRuntime().Value.RestoreRecipientScopedDamageBase(this, fromSeat);
            return true;
        }

        Update(current => current with { ChainedTargetIndex = cursor });
        return false;
    }
}
