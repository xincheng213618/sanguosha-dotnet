namespace CardGame.Core;

internal sealed record ProgramDamageCardMaterial(bool Applied, IReadOnlyList<int> CardIds);

internal interface IProgramDamageCardMaterialQueries : IProgramSelectionQuery
{
    ProgramDamageCardMaterial? Read(long attackFrameId);
}

internal interface IProgramDamageCardClaims : IProgramRuntimeOperations
{
    void Claim(long frameId, int ownerSeat, CardMoveReason reason);
}

/// <summary>Claims actual damage material through the existing atomic movement and original frame receipts.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramDamageCardClaimRuntime(IProgramRuntimeFrames frames, ICardZoneQueries zones,
    IProgramBoundCardMovementOperations movement, IProgramRuntimeEvents events,
    IProgramDamageCardClaimNativeRules nativeRules) : IProgramDamageCardMaterialQueries, IProgramDamageCardClaims
{
    public ProgramDamageCardMaterial? Read(long attackFrameId)
    {
        var owner = frames.Find(attackFrameId);
        if (owner is ProgramSkillFrame { AttackAttempt: { } program })
            return new(program.DamageWasApplied, Array.Empty<int>());
        var attack = owner switch
        {
            ProgramSkillFrame { CardAttack.Active: true } frame => frame.CardAttack,
            CardUseFrame { CardAttack.Active: true } frame => frame.CardAttack,
            JudgmentFrame { CardAttack.Active: true } frame => frame.CardAttack,
            _ => null
        };
        return attack is null ? null : new(attack.DamageWasApplied,
            attack.DamageWasApplied ? Array.AsReadOnly(attack.PhysicalCardIds.Select(id => PhysicalCard(id).Id).ToArray()) : Array.Empty<int>());
    }

    private Card PhysicalCard(int id) => zones.CardsAt(zones.GetLocation(id)).Single(card => card.Id == id);

    // The generic claim reads physical costs independently of the applied flag.
    // Applied is only a qualification of the public paid-material proof above.
    private Card[] RawPhysicalMaterial(ProgramDamageCardOwner owner) => !owner.IsNativeCard ? [] : frames.Find(owner.FrameId) switch
    {
        ProgramSkillFrame { CardAttack: { } attack } => attack.PhysicalCardIds.Select(PhysicalCard).ToArray(),
        CardUseFrame { CardAttack: { } attack } => attack.PhysicalCardIds.Select(PhysicalCard).ToArray(),
        JudgmentFrame { CardAttack: { } attack } => attack.PhysicalCardIds.Select(PhysicalCard).ToArray(),
        _ => throw new InvalidOperationException("The damage-card claim lost its original attack material.")
    };

    internal Card[] GetClaimable(ProgramSkillFrame frame)
    {
        if (frame.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied,
                ParentFrameId: var windowId, DamageFrameId: var damageId } ||
            nativeRules.CurrentDamageWindow is not { } window || window.Id != windowId || window.ParentFrameId != damageId)
            throw new InvalidOperationException("The program damage-card claim lost its damage window.");
        return RawPhysicalMaterial(nativeRules.SelectAttackOwner(window))
            .Where(card => zones.GetLocation(card.Id) == CardLocation.Processing).ToArray();
    }

    public void Claim(long frameId, int ownerSeat, CardMoveReason reason)
    {
        var frame = frames.ActiveProgram(frameId);
        var cards = GetClaimable(frame);
        if (cards.Length == 0) return;
        var damage = nativeRules.CurrentDamageWindow!;
        var before = movement.Sequence;
        movement.MoveCards(cards, CardLocation.Processing, CardLocation.Hand(ownerSeat), reason);
        if (nativeRules.ActiveGroupOwnerId is { } groupOwnerId) RecordGroupClaims(groupOwnerId, damage, cards);
        events.AdvanceFact(new ProgramDamageCardsClaimedEvent(frame.Id, frame.SkillId, frame.TriggerId!, ownerSeat,
            Array.AsReadOnly(cards.Select(card => card.Id).ToArray())));
        nativeRules.CaptureExtraEffectClaims(frame, damage, cards, before, reason);
    }

    private CardResolutionContinuations Continuations(long ownerId) => frames.Find(ownerId) switch
    {
        CardUseFrame use => use.Continuations,
        ProgramSkillFrame program => program.Continuations,
        JudgmentFrame judgment => judgment.Continuations,
        _ => throw new InvalidOperationException("The damage-card claim lost its native group owner.")
    };

    private GroupCardState Group(long ownerId) => Continuations(ownerId).GroupCard ??
        throw new InvalidOperationException("The damage-card claim lost its group continuation.");

    private void RecordGroupClaims(long groupOwnerId, DamageTriggerWindowFrame damage, IReadOnlyList<Card> cards)
    {
        // Preserve the live group's validation and final write even for another
        // attack. The shared rule decides whether its ledger accepts each card.
        var claimed = Group(groupOwnerId).DamageClaimedPhysicalCardIds.ToHashSet();
        foreach (var card in cards)
            RuntimeSkills.RuntimeRules_Core_CommonRules.RecordClaimedGroupPhysicalCard(claimed,
                groupOwnerId, Group(groupOwnerId).PhysicalCardIds.Select(PhysicalCard).ToArray(),
                nativeRules.SelectAttackOwner(damage).FrameId, card.Id);
        var owner = frames.Find(groupOwnerId) ?? throw new InvalidOperationException("The damage-card claim lost its native group owner.");
        var continuations = Continuations(groupOwnerId);
        var group = Group(groupOwnerId);
        var next = continuations with { GroupCard = group with { DamageClaimedPhysicalCardIds = Array.AsReadOnly(claimed.ToArray()) } };
        frames.Replace(groupOwnerId, owner switch
        {
            CardUseFrame use => use with { Continuations = next },
            ProgramSkillFrame program => program with { Continuations = next },
            JudgmentFrame judgment => judgment with { Continuations = next },
            _ => throw new InvalidOperationException("The damage-card claim cannot replace its group owner.")
        });
    }
}
