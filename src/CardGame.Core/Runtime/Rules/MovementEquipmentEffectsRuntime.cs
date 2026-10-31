namespace CardGame.Core;

internal interface IMovementEquipmentFacts : IProgramSelectionQuery
{
    bool UsesStorageEquipment { get; }
    bool UsesRecoveryArmor { get; }
    CardMovementRecord? LastMovement(int cardId, CardLocation from, CardLocation to);
    bool WasBoneAt(int cardId, CardLocation location, int sequence);
    int Count(CardLocation location);
}

// A transient port preserves the original participant across native recovery
// callbacks. It never enters a frame or replaces the player's state owner.
internal interface IMovementArmorOwner : IProgramRuntimeOperations
{
    bool Alive { get; }
    bool HasHpValue { get; }
    bool Wounded { get; }
    bool ArmorIneffective { get; }
    bool EquipmentDisabled { get; }
    int Hp { get; }
    string Name { get; }
    void RecoverOne();
}

internal interface IMovementEquipmentOperations : IProgramRuntimeOperations
{
    IMovementArmorOwner ArmorOwner(int seat);
    bool QueueArmorRecovery(long parentFrameId, int seat, CardMoveReason reason);
    long BeginArmorRecovery(long parentFrameId, int seat);
    void MoveAll(CardLocation from, CardLocation to, CardMoveReason reason);
    void RemoveJudgmentIdentity(int cardId);
    void Log(string kind, string message, int actor);
}

/// <summary>Resolves equipment departure and judgment identity after physical movement.</summary>
[ProgramRuntimeModule]
internal sealed class MovementEquipmentEffectsRuntime(IMovementEquipmentFacts facts,
    IMovementEquipmentOperations operations, IProgramRuntimeFrames frames, IProgramRuntimeEvents events)
{
    internal void ResolveStorageMove(Card card, CardLocation from, CardLocation to)
    {
        if (!facts.UsesStorageEquipment || card.Kind != CardKind.WoodenOx ||
            from is not { Zone: CardZoneKind.Equipment, OwnerSeat: { } ownerSeat })
            return;
        var departure = facts.LastMovement(card.Id, from, to);
        if (departure is not null && facts.WasBoneAt(card.Id, from, departure.Sequence - 1)) return;
        var grainFrom = CardLocation.WoodenOxGrain(ownerSeat);
        if (facts.Count(grainFrom) == 0) return;
        if (to is { Zone: CardZoneKind.Equipment, OwnerSeat: { } targetSeat })
            operations.MoveAll(grainFrom, CardLocation.WoodenOxGrain(targetSeat), CardMoveReasons.WoodenOxTransfer);
        else
            operations.MoveAll(grainFrom, CardLocation.DiscardPile, CardMoveReasons.WoodenOxGrainDiscard);
    }

    internal void ResolveArmorRemoval(Card card, CardLocation from, CardMoveReason reason)
    {
        if (!facts.UsesRecoveryArmor || card.Kind != CardKind.SilverLion ||
            from is not { Zone: CardZoneKind.Equipment, OwnerSeat: { } ownerSeat })
            return;
        var owner = operations.ArmorOwner(ownerSeat);
        if (!owner.Alive || !owner.HasHpValue || !owner.Wounded || owner.ArmorIneffective || owner.EquipmentDisabled)
            return;
        var parentFrameId = frames.Top?.Id ?? 0;
        if (operations.QueueArmorRecovery(parentFrameId, ownerSeat, reason)) return;
        var recoveryFrameId = operations.BeginArmorRecovery(parentFrameId, ownerSeat);
        try
        {
            owner.RecoverOne();
            events.AdvanceFact(new RecoveryAppliedEvent(ownerSeat, ownerSeat, 1, owner.Hp));
            events.AdvanceFact(new SilverLionRemovedRecoveryEvent(recoveryFrameId, ownerSeat, reason, 1, owner.Hp));
            operations.Log("EquipmentEffect", $"{owner.Name} 失去【白银狮子】，回复 1 点体力。", ownerSeat);
        }
        finally
        {
            frames.Pop(recoveryFrameId, ResolutionFrameKind.Recovery);
        }
    }

    internal void ClearJudgmentIdentity(Card card, CardLocation from, CardLocation to)
    {
        if (from.Zone == CardZoneKind.Judgment && to.Zone != CardZoneKind.Judgment)
            operations.RemoveJudgmentIdentity(card.Id);
    }
}
