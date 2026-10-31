#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardUsePaymentsRuntime
{
    internal static bool OwnsGenericSlashPaymentTail(CardUseFrame use) => RuntimeSkills.RuntimeRules_Core_CardUsePayments.OwnsGenericSlashPaymentTail(use);
    internal void PaySlashUseMaterials(IReadOnlyList<Card> cards, int providerSeat, CardLocation? sourceLocation)
    {
        if (_coreProgramCardUsePaymentsOperations.CardUsePayments.TryPayMaterials(cards, providerSeat, sourceLocation, CardMoveReasons.Use))
            return;
        if (cards.Count == 1)
        {
            _corePhysicalMovementFacadesOperations.MoveCard(cards[0], sourceLocation ?? _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(_coreMatchParticipantsPlayersAccess._players[providerSeat], cards[0]), CardLocation.Processing, CardMoveReasons.Use, disclosure: CardMovementDisclosure.FaceUp);
            return;
        }

        // One accepted action pays its physical materials together, including HE
        // conversions. Each material still uses the ordinary alternative-cost rule.
        foreach (var card in cards)
            if (_cardZonesOperations._cardZones.GetLocation(card.Id) != (sourceLocation ?? _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(_coreMatchParticipantsPlayersAccess._players[providerSeat], card)))
                throw new InvalidOperationException("A card-use payment lost its original physical source.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(cards.Select(card => card.Id).ToArray(), CardLocation.Processing, CardMoveReasons.Use, destinationForCard: (card, from) => _coreAlternativeViewAsCostsOperations.NormalizeProgramViewAsCostDestination(card, from, CardLocation.Processing, CardMoveReasons.Use), disclosure: CardMovementDisclosure.FaceUp);
    }

    internal void CaptureSlashUsePaymentBatch(CardMovementBatchContext batch)
    {
        _coreProgramCardUsePaymentsOperations.CardUsePayments.CaptureMovement(batch);
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || batch.ParentFrameId != use.Id || use.Action is not { Type: CardActionType.Use } action || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || !CardUsePaymentMaterialsMatch(use, batch))
            return;
        if (use.PaymentMovementReceipt is not null)
            throw new InvalidOperationException("One card-use payment cannot issue its physical materials twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { PaymentMovementReceipt = new(use.Id, action.ActionId, batch.Movements[0].Sequence - 1, batch.Movements[^1].Sequence) { Batch = batch } });
    }

    internal bool IsCardUsePaymentBatch(CardUseFrame use, CardMovementBatchContext batch) => _coreProgramCardUsePaymentsOperations.CardUsePayments.MatchesBatch(use, batch) || CardUsePaymentBatchMatchesReceipt(use, batch) && CardUsePaymentMaterialsMatch(use, batch);
    internal bool IsPaidDestroyedGeneralWeaponUseMaterial(long useId, int cardId) => cardId > 0 && _cardZonesOperations._cardZones.GetLocation(cardId) == CardLocation.OutsideGame && _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId).IsGeneralWeapon && _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { Type: CardActionType.Use } action, PaymentMovementReceipt: { Batch: { } batch } receipt } use && receipt.OwnerFrameId == useId && receipt.ActionId == action.ActionId && CardUsePaymentBatchMatchesReceipt(use, batch) && CardUsePaymentMaterialsMatch(use, batch) && action.PhysicalCards.SingleOrDefault(c => c.CardId == cardId)is { } material && material.From == CardLocation.Equipment(action.ProviderSeat) && batch.Movements.SingleOrDefault(m => m.CardId == cardId)is { } movement && movement.From == material.From && movement.CardKind == material.CardKind && movement.To == CardLocation.OutsideGame && movement.Reason == CardMoveReasons.Use && movement.Sequence > receipt.SequenceBefore && movement.Sequence <= receipt.SequenceAfter && _coreCardMovementJournalCardMovementsAccess._cardMovements.SingleOrDefault(m => m.Sequence == movement.Sequence) == movement;
    internal static bool CardUsePaymentBatchMatchesReceipt(CardUseFrame use, CardMovementBatchContext batch, CardUsePaymentMovementReceipt? issuedReceipt = null) => RuntimeSkills.RuntimeRules_Core_CardUsePayments.CardUsePaymentBatchMatchesReceipt(use, batch, issuedReceipt);
    internal bool CardUsePaymentMaterialsMatch(CardUseFrame use, CardMovementBatchContext batch)
    {
        if (!OwnsGenericSlashPaymentTail(use) || use.Action is not { Type: CardActionType.Use } action || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || use.CardKind != action.EffectiveKind || batch.ParentFrameId != use.Id || batch.ParentBatchId is not null || batch.AwaitingProgramFrameId is not null || batch.NativeDrawInvocation is not null || batch.NativeBuZhiDiscardInvocation is not null || batch.Movements.Count == 0 || batch.Movements.Count != action.PhysicalCards.Count || batch.Movements.Select(move => move.CardId).Distinct().Count() != batch.Movements.Count)
            return false;
        foreach (var move in batch.Movements)
        {
            var cost = action.PhysicalCards.SingleOrDefault(cost => cost.CardId == move.CardId);
            if (cost is null || cost.From != move.From || cost.CardKind != move.CardKind || move.Reason != CardMoveReasons.Use || move.TurnNumber != batch.TurnNumber || move.Sequence <= 0 || move.Sequence > _coreCardMovementJournalCardMovementsAccess._cardMovements.Count || _coreCardMovementJournalCardMovementsAccess._cardMovements[move.Sequence - 1] != move)
                return false;
            var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(move.CardId)).Single(card => card.Id == move.CardId);
            var destination = card.IsGeneralWeapon && cost.From.Zone == CardZoneKind.Equipment || CoreGameEngineGuanduEquipmentRuntime.IsGuanduThunderChariotDeparture(card, cost.From, CardLocation.Processing) ? CardLocation.OutsideGame : _coreWushuangJinzhiOperations.IsWushuangJinzhiPaidMaterial(action, move.CardId) ? CardLocation.Processing : _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(action, move.CardId) ? CardLocation.DrawPile : CardLocation.Processing;
            destination = _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(card, cost.From, destination);
            if (move.To != destination || _characterDiaoChanLeOperations.IsDiouCarrier(card.Id) && destination == CardLocation.OutsideGame && !_coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(move))
                return false;
        }

        return true;
    }

    internal bool IsPaidCardUseMovementReturn(CardUseFrame use, CardsMovedTriggerWindowFrame movement) => use.RecoveryPaidContinuation is { } continuation && _coreProgramCardUsePaymentsOperations.HasPaidCardUseMovementTail(use, continuation) && continuation.SourceSeat == use.SourceSeat && use.Step == ResolutionFrameStep.ResolvingEffect && movement.Id == movement.Batch.Id && movement.ResumePaidCardUseFrameId == use.Id && movement.ResumeProgramFrameId is null && movement.ResumeDeclarationFrameId is null && movement.ResumeRecoveryReplacementFrameId is null && movement.ResumeDrawFundedDistinctBasicFrameId is null && movement.ResumeNativeCardEffectDrawFrameId is null && movement.ResumeGuanduCardEffectFrameId is null && movement.ResumeWumiaoFloodFrameId is null && movement.ResumeDragonBoatCardEffectFrameId is null && movement.ResumeHuxinMirrorPlacementFrameId is null && movement.ResumeEquipmentRecastFrameId is null && movement.ResumeColorFireAttackFrameId is null && movement.ResumeCounterspellPaymentFrameId is null && movement.ResumeHistoricalEndingUseFrameId is null && movement.ResumeGuoSiUseFrameId is null && movement.ResumeDenglouUseFrameId is null && movement.ResumeShanheNullifiedClaimWindowId is null && movement.ResumeRoundPileAlcoholUseFrameId is null && movement.ResumeDrawPhaseObligationFrameId is null && movement.ResumeManualDiscardFrameId is null && movement.ResumeFactionRequestCostFrameId is null && movement.DeferredTurnEndReturn is null && movement.ResumeCardSupplyCompletionFrameId is null && movement.ResumeResponseCompletionFrameId is null && IsCardUsePaymentBatch(use, movement.Batch);
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.ProcessingAllowance, 10)]
    internal bool HasSharedCardUsePaymentProcessing(IReadOnlyList<Card> processing) => _resolutionStackOperations._resolutionStack.Count == 2 && _resolutionStackOperations._resolutionStack[0] is CardUseFrame { RecoveryPaidContinuation: { } continuation } use && _resolutionStackOperations._resolutionStack[1] is CardsMovedTriggerWindowFrame movement && _coreProgramCardUsePaymentsOperations.CardUsePayments.AllowsTail(ProgramCardUsePaymentStage.SharedProcessingTail, use, continuation) && IsPaidCardUseMovementReturn(use, movement) && processing.All(card => use.PaymentMovementReceipt!.Batch.Movements.Any(cost => cost.CardId == card.Id && cost.To == CardLocation.Processing));
    [ProgramPipelineHook(ProgramPipelinePhase.CompleteMovementBatch, 700)]
    internal CardMovementBatchContext CompleteMovementBatchCaptureSlashUsePaymentBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed)
    {
        CaptureSlashUsePaymentBatch(completed);
        return completed;
    }
}
