namespace CardGame.Core;

internal interface IAtomicCardMovementFacts : IProgramSelectionQuery
{
    bool TracksAdjacentDiscardStorage { get; }
    bool IsCarrier(int cardId);
    bool IsPurchasedEquipment(int cardId);
    bool IsStartingResource(int cardId);
}

internal interface IAtomicCardMovementPolicies : IProgramRuntimeOperations
{
    bool ClaimDeclarationPayment(Card card, CardLocation from, CardLocation to);
    HappyZuoCiPhantomDestinationPlan? PlanPhantomDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    CardLocation NormalizeViewAsCostDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    CardLocation NormalizeEphemeralDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    CardLocation NormalizeCopiedDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    CardLocation NormalizeDiscardDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    bool IsGeneratedDeparture(Card card, CardLocation from, CardLocation to);
    HappyWeiYanGainDestinationPlan? PlanGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    CardLocation NormalizeGainDestination(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    bool SettleRetainedGain(HappyWeiYanGainDestinationPlan plan);
    CardLocation NormalizePhantomDestination(Card card, CardLocation from, CardLocation to);
    bool CanPlanDiscardTransfers(CardLocation to);
    HellShiyouTransferPlan PrepareDiscardTransfers(IReadOnlyList<CardTransfer> transfers, CardMoveReason reason);
    void RecordInterceptedMovement(HellShiyouTransferPlan plan, CardMovementRecord movement, Action<CardMovementRecord>? beforeFact);
    CardKind? FreezeAdjacentIdentity(Card card, CardLocation from, CardLocation to, CardMoveReason reason);
    void DestroyReturnSoulIfDiscarded(Card card, CardLocation to);
}

internal interface IAtomicCardMovementZones : IProgramRuntimeOperations
{
    IReadOnlyList<Card> CardsAt(CardLocation location);
    CardLocation GetLocation(int cardId);
    void Move(int cardId, CardLocation from, CardLocation to);
    IReadOnlyList<Card> MoveMany(IEnumerable<int> cardIds, CardLocation from, CardLocation to);
    IReadOnlyList<Card> MoveBatch(IEnumerable<CardTransfer> transfers);
    IReadOnlyList<Card> MoveAll(CardLocation from, CardLocation to);
}

internal interface IAtomicCardMovementBatches : IProgramRuntimeOperations
{
    CardMovementBatchBuilder Begin(IEnumerable<CardLocation> sources, IEnumerable<CardLocation> destinations);
    void Complete(CardMovementBatchBuilder batch, IReadOnlyList<CardMovementRecord> movements, bool committed);
}

internal interface IAtomicCardMovementEffects : IProgramRuntimeOperations
{
    CardMovementRecord Record(Card card, CardLocation from, CardLocation to, CardMoveReason reason,
        Action<CardMovementRecord>? beforeFact = null, CardKind? adjacentDiscardIdentity = null,
        CardMovementDisclosure disclosure = CardMovementDisclosure.Unspecified,
        HappyWeiYanGainDestinationPlan? happyWeiYanGainDestination = null,
        HappyZuoCiPhantomDestinationPlan? happyZuoCiPhantomDestination = null);
    void ResolveEquipmentSkillGrant(Card card, CardLocation from, CardLocation to);
    void ClearJudgmentEffectiveKind(Card card, CardLocation from, CardLocation to);
    void ResolveArmorRemoval(Card card, CardLocation from, CardMoveReason reason);
    void ResolveStorageEquipmentMove(Card card, CardLocation from, CardLocation to);
    void CollectDiscardPhaseHandDiscard(Card card, CardLocation from, CardLocation to);
}
