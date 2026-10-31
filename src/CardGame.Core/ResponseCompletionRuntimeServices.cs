namespace CardGame.Core;

// Individual facts keep the response's original journal and frame searches lazy.
// No mutable player, stack, or movement collection crosses this boundary.
internal interface IResponseCompletionFacts : IProgramSelectionQuery
{
    Winner Winner { get; }
    int TurnNumber { get; }
    int CurrentSeat { get; }
    int FrameCount { get; }
    ResolutionFrame FrameAt(int index);
    int PendingMovementBatchCount { get; }
    CardMovementBatchContext PendingMovementBatchAt(int index);
    int PendingHpChangeCount { get; }
    HpChangeContext PendingHpChangeAt(int index);
    bool ContainsMovement(CardMovementRecord movement);
    CardMovementRecord? LastMovement(Func<CardMovementRecord, bool> predicate);
    bool HasTriggerWindow(SkillProgramTriggerWindow window);
    bool UsesTriggerCondition(SkillProgramTriggerConditionKind condition);
    bool IsValidPlayerSeat(int seat);
    bool IsAlive(int playerIndex);
    int PlayerSeat(int playerIndex);
    SkillProgramTriggerFacts CaptureProgramTriggerFacts(int playerIndex);
    IReadOnlyList<ProgramTriggerCandidate> CollectProgramTriggerCandidates(int playerIndex, SkillProgramTriggerWindow window);
    SkillProgramTrigger GetProgramTrigger(ProgramTriggerCandidate candidate);
    SkillProgram? Program(string skillId);
    SkillProgramViewAs? ViewAsRule(CardConversionSource source);
    IEnumerable<IGameEvent> CompleteProgramEventHistory();
    NullificationWindowFrame? ActiveNullificationWindow { get; }
    long? ActiveDuelOwnerId { get; }
    int DuelOpponentSeat(long ownerId);
    int DuelResponderSeat(long ownerId);
    long? ActiveGroupOwnerId { get; }
    int GroupSourceSeat(long ownerId);
    CardKind? GroupRequiredCardKind(long ownerId);
    long? GroupCurrentAttackOwnerId(long ownerId);
}

// These proofs belong to existing native payment and card-use mechanisms. The
// response runtime owns the once-paid cursor and chooses when to ask each proof.
internal interface IResponseCompletionNativeProtocols : IProgramRuntimeOperations
{
    bool HasJiyunResponseCompletion(CardActionContext action, ProgramCardContinuation continuation);
    bool IsNativePublicPileOriginalActionCost(CardActionContext action, CardActionCost cost);
    bool IsJueshengDuelUse(long useId);
    bool HasGuanduChariotResponsePayment(CardActionContext action, ProgramCardContinuation continuation);
    bool HasChenglieResponseObserver(CardActionContext action, ProgramCardContinuation continuation);
    bool HasBoundaryZhouCangResponseObserver(CardActionContext action, ProgramCardContinuation continuation);
    bool TryGetXieweiResponseCostMovement(CardActionContext action, int cardId, out CardMovementRecord movement);
    bool TryGetSiruiDestroyedResponseCost(CardActionContext action, int cardId, out CardMovementRecord movement);
    bool TryGetWushuangJinzhiResponseMovements(CardActionContext action, int cardId, out IReadOnlyList<CardMovementRecord> movements);
    CardMovementRecord? GuanduChariotResponsePayment(CardActionContext action, long parentFrameId,
        CardActionCost cost, ProgramCardContinuation continuation);
    CardMovementRecord? DestroyedEquipmentResponsePayment(CardActionContext action, long parentFrameId,
        CardActionCost cost, ProgramCardContinuation continuation);
    bool IsProgramAlternativeCost(CardActionContext action, int cardId);
    bool IsExchangedCardClaim(long actionId, int cardId);
    bool IsWuZglReturnedCopyCost(CardActionContext action, CardActionCost cost, CardMovementRecord finish);
    bool IsShanheStartingResourceResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish);
    bool IsHappyZuoCiDestroyedResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish);
    bool HasNullificationEffectOwner(NullificationWindowFrame window);
    long? NullificationParentActionId(NullificationWindowFrame window);
    void CapturePairedColorResponseLink(ProgramCardTriggerWindowFrame window);
    void CaptureMaMiDiResponseLink(ProgramCardTriggerWindowFrame window);
    bool RemovePendingHpChange(HpChangeContext change);
    void RemovePendingMovementBatch(CardMovementBatchContext batch);
    CardsMovedTriggerWindowFrame? CreateCardsMovedProgramWindow(CardMovementBatchContext batch, long? resumeProgramFrameId);
    void CaptureLeCaoZhiWenlanQualification(CardActionContext action, SkillProgramTriggerWindow window);
    IReadOnlyList<ProgramCardTriggerCandidate> CollectSharedCardActionCandidates(CardActionContext action,
        SkillProgramTriggerWindow window, IReadOnlyList<int> eventTargets, bool? cardUseCausedDamage);
    bool TryBeginCompletedResponseUsePrograms(long? attackOwnerFrameId, CardActionContext action, ProgramCardContinuation continuation);
    NullificationWindowFrame ClearTieredRoundCounterspellResponse(NullificationWindowFrame window);
    void ContinueNullificationWindow(NullificationWindowFrame window);
    void ContinueFinishedCardResponse(long attackOwnerFrameId, CardActionContext action, ProgramCardContinuation continuation);
}

internal sealed partial class ResponseCompletionRuntime
{
    private readonly IResponseCompletionFacts _facts;
    private readonly IResponseCompletionNativeProtocols _native;
    private readonly IProgramRuntimeFrames _frames;
    private readonly IProgramRuntimeEvents _events;

    internal ResponseCompletionRuntime(IResponseCompletionFacts facts, IResponseCompletionNativeProtocols native,
        IProgramRuntimeFrames frames, IProgramRuntimeEvents events)
    {
        _facts = facts;
        _native = native;
        _frames = frames;
        _events = events;
    }

    private IEnumerable<ResolutionFrame> FrameFacts()
    {
        for (var index = 0; index < _facts.FrameCount; index++)
            yield return _facts.FrameAt(index);
    }

    private IEnumerable<CardMovementBatchContext> PendingMovementBatches()
    {
        for (var index = 0; index < _facts.PendingMovementBatchCount; index++)
            yield return _facts.PendingMovementBatchAt(index);
    }

    private IEnumerable<HpChangeContext> PendingHealthChanges()
    {
        for (var index = 0; index < _facts.PendingHpChangeCount; index++)
            yield return _facts.PendingHpChangeAt(index);
    }
}
