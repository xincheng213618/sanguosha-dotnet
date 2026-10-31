#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class CardAttackHandle : GameEngine.IDamageAttempt
{
    private readonly Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime;
    private readonly Func<RuntimeEntries.CharacterWuJingOlEntries> getCharacterWuJingOlRuntime;
    private readonly Func<RuntimeEntries.CoreGameEngineRecipientScopedDamageEntries> getCoreGameEngineRecipientScopedDamageRuntime;
    private CardAttackState State => getCoreGameEngineCardAttackStateRuntime().GetCardAttackState(ResolutionId);

    private void Update(Func<CardAttackState, CardAttackState> update) => getCoreGameEngineCardAttackStateRuntime().UpdateCardAttackState(ResolutionId, state => update(state ?? throw new InvalidOperationException("An attack handle lost its owned state.")));
    internal CardAttackHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CharacterWuJingOlEntries> getCharacterWuJingOlRuntime, Func<RuntimeEntries.CoreGameEngineRecipientScopedDamageEntries> getCoreGameEngineRecipientScopedDamageRuntime, long resolutionId, int sourceSeat, int targetSeat, Card? card, int damageAmount = 1, CardKind? playedCardKind = null, bool ignoresArmor = false, bool isDelayedJudgmentDamage = false, int? delayedJudgmentSeat = null, DamageNature? damageNatureOverride = null, IReadOnlyList<Card>? physicalCards = null, long? programJudgmentFrameId = null, long? programSkillFrameId = null, CardConversionSource? conversionSource = null, long? programSkillCardUseFrameId = null)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCharacterWuJingOlRuntime = getCharacterWuJingOlRuntime;
        this.getCoreGameEngineRecipientScopedDamageRuntime = getCoreGameEngineRecipientScopedDamageRuntime;
        ResolutionId = resolutionId;
        card = getCharacterWuJingOlRuntime().WuJingLiubingIssuedAppearance(resolutionId, card);
        getCoreGameEngineCardAttackStateRuntime().InitializeCardAttackState(resolutionId, new CardAttackState { Active = false, SourceSeat = sourceSeat, CardUserSeat = sourceSeat, TargetSeat = targetSeat, DamageAmount = damageAmount, IgnoresArmor = ignoresArmor, EffectiveCardKind = playedCardKind ?? card?.Kind, IsDelayedJudgmentDamage = isDelayedJudgmentDamage, IsSourceLess = isDelayedJudgmentDamage && (playedCardKind ?? card?.Kind) == CardKind.Lightning, ProgramJudgmentFrameId = programJudgmentFrameId, ProgramSkillFrameId = programSkillFrameId, ProgramSkillCardUseFrameId = programSkillCardUseFrameId, DelayedJudgmentSeat = delayedJudgmentSeat, DamageNatureOverride = damageNatureOverride, ConversionSource = conversionSource, CardId = card?.Id, AppearanceKind = card?.Kind, AppearanceSuit = card?.Suit, AppearanceRank = card?.Rank, PhysicalCardIds = Array.AsReadOnly((physicalCards ?? (card is null ? [] : [card])).Select(item => item.Id).ToArray()), });
    }

    internal CardAttackHandle(Func<RuntimeEntries.CoreGameEngineCardAttackStateEntries> getCoreGameEngineCardAttackStateRuntime, Func<RuntimeEntries.CharacterWuJingOlEntries> getCharacterWuJingOlRuntime, Func<RuntimeEntries.CoreGameEngineRecipientScopedDamageEntries> getCoreGameEngineRecipientScopedDamageRuntime, long ownerId)
    {
        this.getCoreGameEngineCardAttackStateRuntime = getCoreGameEngineCardAttackStateRuntime;
        this.getCharacterWuJingOlRuntime = getCharacterWuJingOlRuntime;
        this.getCoreGameEngineRecipientScopedDamageRuntime = getCoreGameEngineRecipientScopedDamageRuntime;
        ResolutionId = ownerId;
    }

    public long ResolutionId { get; }
    public int SourceSeat { get => State.SourceSeat; private set => Update(state => state with { SourceSeat = value }); }
    public int CardUserSeat { get => State.CardUserSeat; private set => Update(state => state with { CardUserSeat = value }); }
    public int TargetSeat { get => State.TargetSeat; private set => Update(state => state with { TargetSeat = value }); }
    public Card? Card => State.CardId is { } id ? getCoreGameEngineCardAttackStateRuntime().GetAttackCard(id)with
    {
        Kind = State.AppearanceKind!.Value,
        Suit = State.AppearanceSuit!.Value,
        Rank = State.AppearanceRank!.Value
    }

    : null;
    public IReadOnlyList<Card> PhysicalCards => State.PhysicalCardIds.Select(getCoreGameEngineCardAttackStateRuntime().GetAttackCard).ToArray();
    public CardConversionSource? ConversionSource { get => State.ConversionSource; }
    public bool IsTwoCardVirtualSlash => PhysicalCards.Count == 2 && EffectiveCardKind == CardKind.Slash;
    public int DamageAmount { get => State.DamageAmount; private set => Update(state => state with { DamageAmount = value }); }
    public bool DamageAmountFinalized { get => State.DamageAmountFinalized; private set => Update(state => state with { DamageAmountFinalized = value }); }
    public CardKind? EffectiveCardKind { get => State.EffectiveCardKind; }
    public bool IgnoresArmor { get => State.IgnoresArmor; private set => Update(state => state with { IgnoresArmor = value }); }
    public bool IsDelayedJudgmentDamage { get => State.IsDelayedJudgmentDamage; }
    public bool IsSourceLess { get => State.IsSourceLess; private set => Update(state => state with { IsSourceLess = value }); }
    public long? ProgramJudgmentFrameId { get => State.ProgramJudgmentFrameId; }
    public bool IsProgramJudgmentDamage => ProgramJudgmentFrameId is not null;
    public long? ProgramSkillFrameId { get => State.ProgramSkillFrameId; }
    public bool IsProgramSkillDamage => ProgramSkillFrameId is not null;
    public long? ProgramSkillCardUseFrameId { get => State.ProgramSkillCardUseFrameId; }
    public int? DelayedJudgmentSeat { get => State.DelayedJudgmentSeat; }
    public DamageNature? DamageNatureOverride { get => State.DamageNatureOverride; }
    public IReadOnlyList<int> ChainedTargetSeats { get => State.ChainedTargetSeats; private set => Update(state => state with { ChainedTargetSeats = value }); }
    public int ChainedTargetIndex { get => State.ChainedTargetIndex; private set => Update(state => state with { ChainedTargetIndex = value }); }
    public bool IsChainPropagation { get => State.IsChainPropagation; private set => Update(state => state with { IsChainPropagation = value }); }
    public bool FactionDefenseAttempted { get => State.FactionDefenseAttempted; private set => Update(state => state with { FactionDefenseAttempted = value }); }
    public bool FactionSlashAttempted { get => State.FactionSlashAttempted; private set => Update(state => state with { FactionSlashAttempted = value }); }
    public bool CixiongDoubleSwordsResolved { get => State.CixiongDoubleSwordsResolved; private set => Update(state => state with { CixiongDoubleSwordsResolved = value }); }
    public bool IceSwordAttempted { get => State.IceSwordAttempted; private set => Update(state => state with { IceSwordAttempted = value }); }
    public bool QilinBowAttempted { get => State.QilinBowAttempted; private set => Update(state => state with { QilinBowAttempted = value }); }
    public bool DamageWasApplied { get => State.DamageWasApplied; private set => Update(state => state with { DamageWasApplied = value }); }
    public int SourceToTargetDistanceAtDamage { get => State.SourceToTargetDistanceAtDamage; private set => Update(state => state with { SourceToTargetDistanceAtDamage = value }); }
    private long? ResolvedDyingDamageFrameId { get => State.ResolvedDyingDamageFrameId; set => Update(state => state with { ResolvedDyingDamageFrameId = value }); }
    public bool CardUseCausedDamage { get => getCoreGameEngineCardAttackStateRuntime().GetCardUseCausedDamage(ResolutionId); private set => getCoreGameEngineCardAttackStateRuntime().SetCardUseCausedDamage(ResolutionId, value); }
    public bool PendingRedBladeDamageBonus { get => State.PendingRedBladeDamageBonus; set => Update(state => state with { PendingRedBladeDamageBonus = value }); }
    public bool DamageRedirected { get => State.DamageRedirected; private set => Update(state => state with { DamageRedirected = value }); }
    public bool BeforeDamageProgramsResolved { get => State.BeforeDamageProgramsResolved; private set => Update(state => state with { BeforeDamageProgramsResolved = value }); }
    private GameEngine.ProgramDamageTransferFollowup? DamageTransferFollowup { get => State.DamageTransferFollowup is { } receipt ? new(receipt.SkillId, receipt.OwnerSeat, receipt.TargetSeat, receipt.DrawLostHp) : null; set => Update(state => state with { DamageTransferFollowup = value is { } receipt ? new(receipt.SkillId, receipt.OwnerSeat, receipt.TargetSeat, receipt.DrawLostHp) : null }); }
    public bool ProhibitsDodge { get => State.ProhibitsDodge; private set => Update(state => state with { ProhibitsDodge = value }); }
    public bool ProhibitsTargetHandResponses { get => State.ProhibitsTargetHandResponses; private set => Update(state => state with { ProhibitsTargetHandResponses = value }); }
    public IReadOnlyList<string> ResponseProhibitingSkillNames { get => State.ResponseProhibitingSkillNames; private set => Update(state => state with { ResponseProhibitingSkillNames = value }); }

    public void ProhibitDodgeBy(string skillName)
    {
        if (string.IsNullOrWhiteSpace(skillName))
            throw new ArgumentException("A response prohibition requires a skill name.", nameof(skillName));
        ProhibitsDodge = true;
        if (!ResponseProhibitingSkillNames.Contains(skillName, StringComparer.Ordinal))
            ResponseProhibitingSkillNames = Array.AsReadOnly(ResponseProhibitingSkillNames.Append(skillName).ToArray());
    }

    public void ProhibitTargetHandResponses() => ProhibitsTargetHandResponses = true;
    public int RequiredDodgeResponses { get => State.RequiredDodgeResponses; private set => Update(state => state with { RequiredDodgeResponses = value }); }
    public int SuccessfulDodgeResponses { get => State.SuccessfulDodgeResponses; private set => Update(state => state with { SuccessfulDodgeResponses = value }); }

    public void SetDamageParticipants(int sourceSeat, int targetSeat)
    {
        if (DamageAmountFinalized || IsChainPropagation)
        {
            throw new InvalidOperationException("Damage participants cannot change after damage begins.");
        }

        SourceSeat = sourceSeat;
        TargetSeat = targetSeat;
    }

    public void SetIgnoresArmor(bool value) => IgnoresArmor = value;
    public void ReplaceCardUser(int actorSeat)
    {
        if (DamageAmountFinalized || DamageWasApplied || IsChainPropagation)
            throw new InvalidOperationException("The card user cannot change after damage begins.");
        SourceSeat = actorSeat;
        CardUserSeat = actorSeat;
    }

    public void MarkBeforeDamageProgramsResolved() => BeforeDamageProgramsResolved = true;
    public void ReduceFinalizedDamageAmount(int amount)
    {
        if (!DamageAmountFinalized || amount <= 0 || amount > DamageAmount)
            throw new InvalidOperationException("Damage reduction requires a positive part of the frozen amount.");
        DamageAmount -= amount;
    }

    public void MarkDamageApplied() => DamageWasApplied = true;
    public void IncreaseFinalizedDamageAmount(int amount)
    {
        if (!DamageAmountFinalized || DamageWasApplied || amount <= 0)
            throw new InvalidOperationException("Damage increase requires a positive amount before frozen damage is applied.");
        DamageAmount = checked(DamageAmount + amount);
    }

    public void CaptureDamageDistance(int distance) => SourceToTargetDistanceAtDamage = distance;
    public void MarkDyingResolvedForDamage(long damageFrameId) => ResolvedDyingDamageFrameId = damageFrameId;
    public bool HasResolvedDyingForDamage(long damageFrameId) => ResolvedDyingDamageFrameId == damageFrameId;
    public void SetCardUseCausedDamage(bool value) => CardUseCausedDamage = value;
    public void RedirectFinalizedDamageTarget(int targetSeat, GameEngine.ProgramDamageTransferFollowup followup)
    {
        if (!DamageAmountFinalized || DamageRedirected || TargetSeat == targetSeat)
            throw new InvalidOperationException("Damage redirection requires a new recipient and a finalized amount.");
        TargetSeat = targetSeat;
        DamageRedirected = true;
        DamageTransferFollowup = followup;
    }

    public bool TryConsumeProgramDamageTransferFollowup(out GameEngine.ProgramDamageTransferFollowup followup)
    {
        followup = DamageTransferFollowup!;
        DamageTransferFollowup = null;
        return followup is not null;
    }

    public void FinalizeDamageAmount(int bonus, int? maximum = null)
    {
        if (DamageAmountFinalized)
        {
            throw new InvalidOperationException("The attack damage amount is already final.");
        }

        DamageAmount = Math.Max(0, checked(DamageAmount + bonus));
        if (maximum is { } cap)
        {
            DamageAmount = Math.Min(DamageAmount, cap);
        }

        DamageAmountFinalized = true;
    }

    public void MarkFactionDefenseAttempted()
    {
        if (FactionDefenseAttempted)
        {
            throw new InvalidOperationException("FactionDefense has already been requested for this response window.");
        }

        FactionDefenseAttempted = true;
    }

    public void MarkFactionSlashAttempted()
    {
        if (FactionSlashAttempted)
        {
            throw new InvalidOperationException("FactionSlash has already been requested for this response window.");
        }

        FactionSlashAttempted = true;
    }

    public void SetRequiredDodgeResponses(int count)
    {
        if (count < 1 || SuccessfulDodgeResponses != 0)
        {
            throw new InvalidOperationException("The required Dodge count cannot change after responses begin.");
        }

        RequiredDodgeResponses = count;
    }

    public bool RegisterDodgeResponse()
    {
        if (SuccessfulDodgeResponses >= RequiredDodgeResponses)
        {
            throw new InvalidOperationException("All required Dodge responses have already resolved.");
        }

        SuccessfulDodgeResponses++;
        FactionDefenseAttempted = false;
        return SuccessfulDodgeResponses >= RequiredDodgeResponses;
    }

    public void MarkCixiongDoubleSwordsResolved() => CixiongDoubleSwordsResolved = true;
    public void MarkIceSwordAttempted()
    {
        if (IceSwordAttempted)
        {
            throw new InvalidOperationException("Ice Sword has already resolved for this damage event.");
        }

        IceSwordAttempted = true;
    }

    public void MarkQilinBowAttempted()
    {
        if (QilinBowAttempted)
        {
            throw new InvalidOperationException("Qilin Bow has already resolved for this damage event.");
        }

        QilinBowAttempted = true;
    }

    public void SetChainedTargets(IReadOnlyList<int> targetSeats)
    {
        if (ChainedTargetSeats.Count != 0 || ChainedTargetIndex != 0)
        {
            throw new InvalidOperationException("The elemental chain targets have already been captured.");
        }

        ChainedTargetSeats = Array.AsReadOnly(targetSeats.ToArray());
    }

    public bool TryAdvanceChainedTarget(Func<int, bool> isAlive, bool resetDamageForTargetModifiers, out int fromSeat)
    {
        fromSeat = TargetSeat;
        while (ChainedTargetIndex < ChainedTargetSeats.Count)
        {
            var nextSeat = ChainedTargetSeats[ChainedTargetIndex++];
            if (!isAlive(nextSeat))
            {
                continue;
            }

            TargetSeat = nextSeat;
            IsChainPropagation = true;
            if (resetDamageForTargetModifiers)
            {
                DamageAmountFinalized = false;
            }

            getCoreGameEngineRecipientScopedDamageRuntime().RestoreRecipientScopedDamageBase(this, fromSeat);
            return true;
        }

        return false;
    }
}
