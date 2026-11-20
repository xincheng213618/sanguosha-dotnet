namespace CardGame.Core;

/// <summary>Owns movement journal issuance and the ordered native receipt, observer and output boundaries.</summary>
[ProgramRuntimeModule]
internal sealed class MovementRecordRuntime(IMovementRecordFacts facts, IMovementRecordMetadata metadata,
    IMovementRecordJournal journal, IMovementRecordMaterials materials, INativeMaterialClaims nativeMaterialClaims,
    IMovementRecordObservers observers,
    IMovementRecordOrigins origins, IMovementRecordNotifications notifications, IProgramRuntimeEvents events,
    ICardMovementEventSuppression suppression, ICardMovementBatchLifecycle batches,
    RuntimeEntries.DeferredRuntime<GeneratedEquipmentRuntime> equipmentLifetime, ICardMovementTimingQueries timing)
{
    internal CardMovementRecord Record(
        Card card,
        CardLocation from,
        CardLocation to,
        CardMoveReason reason, Action<CardMovementRecord>? beforeFact = null, CardKind? adjacentDiscardIdentity = null,
        CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified,
        HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = null,
        HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = null,
        NativeLogicalDiscardMaterial? nativeDiscard = null)
    {
        var movement = new CardMovementRecord(
            journal.NextSequence(),
            facts.TurnNumber,
            card.Id,
            card.Kind,
            from,
            to,
            reason) { BatchId = batches.ActiveBatchId ?? 0, MovementTiming = timing.Capture(), PrintedSuit = card.Suit, Disclosure = facts.PublishesDisclosure ? disclosure : CardMovementDisclosure.Unspecified,
                ActualPlaySlashLoss = metadata.CaptureActualPlaySlashLoss(card, from, to),
                RedOwnedLoss = metadata.CaptureRedOwnedLoss(card, from, to, facts.Sequence),
                HappyMedicineHandLoss = metadata.CaptureHappyHuaTuoHandLoss(card, from, to, facts.Sequence),
                AnniversaryGuoZhaoLoss = metadata.CaptureAnniversaryGuoZhaoLoss(card, from, to, facts.Sequence),
                AnniversaryZhangHeng = metadata.CaptureAnniversaryZhangHengMovement(card, from, to),
                HappyWeiYanGainDestination = happyWeiYanGainDestination,
                HappyZuoCiPhantomDestination = happyZuoCiPhantomDestination,
                NativeDiscard = nativeDiscard is { } material && material.CardId == card.Id && material.From == from &&
                    to == CardLocation.DiscardPile && batches.ActiveBatchId is > 0 ? material : null };
        journal.Append(movement);
        nativeMaterialClaims.CaptureDelayedMaterialDeparture(movement);
        equipmentLifetime.Value.CaptureNativeMaterialDeparture(movement);
        materials.ValidateHappyWeiYanGainDestinationRecord(movement);
        materials.ValidateHappyZuoCiPhantomMovement(movement);
        materials.RecordDiouCarrierDestruction(movement);
        materials.RecordShenDaCarrierDestruction(movement);
        materials.RecordWushuangShopEquipmentDestruction(movement);
        materials.RecordShanheStartingResourceDestruction(movement);
        materials.RecordWuZglCopyMovement(movement);
        observers.CaptureBianFuRenForeignDiscardProducer(movement);
        observers.CaptureDuFuRenOwnedOrigin(movement);
        observers.ObserveOriginalHandMovement(movement);
        observers.ObserveSourceCurseMovement(movement);
        origins.RecordLishiDiscardRedirection(movement);
        beforeFact?.Invoke(movement);
        equipmentLifetime.Value.DestroyOnEntry(movement);
        origins.CaptureRequestedDeckBasicPaid(card, from, to);
        origins.CaptureDiscardedEntityOrigin(card, movement);
        origins.CaptureAdjacentDiscardOrigin(card, movement, adjacentDiscardIdentity);
        origins.CollectFullDiscardPhaseSuit(card, movement);
        if (facts.Started)
        {
            if (reason != facts.SimultaneousTopReservationReason && reason.Value != facts.PrivatePindianReserveReason)
                notifications.Enqueue(movement);
            if (!suppression.HasSuppressedCardMovedEvents)
                events.AdvanceFact(new CardMovedEvent(card.Id, card.Kind, from, to, reason));
        }
        return movement;
    }
}
