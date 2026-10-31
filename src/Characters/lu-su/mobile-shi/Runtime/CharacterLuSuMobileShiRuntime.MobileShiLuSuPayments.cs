#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMobileShiRuntime
{
    internal bool ShiLsUseMaterialsMatch(CardUseFrame use, CardMovementBatchContext b)
    {
        if (use.Action is not { Type: CardActionType.Use } a || CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || use.CardKind != a.EffectiveKind || use.SourceSeat != a.ActorSeat || b.ParentFrameId != use.Id || b.ParentBatchId is not null || b.AwaitingProgramFrameId is not null || b.NativeDrawInvocation is not null || b.NativeBuZhiDiscardInvocation is not null || b.TurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || b.Movements.Count == 0 || b.Movements.Count != a.PhysicalCards.Count || b.Movements.Select(m => m.CardId).Distinct().Count() != b.Movements.Count)
            return false;
        var reason = _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) ? CardMoveReasons.EquipmentUse : CardMoveReasons.Use;
        foreach (var m in b.Movements)
        {
            var cost = a.PhysicalCards.SingleOrDefault(c => c.CardId == m.CardId);
            if (cost is null || cost.From != m.From || cost.CardKind != m.CardKind || m.Reason != reason || m.Sequence <= 0 || m.Sequence > _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[m.Sequence - 1] != m)
                return false;
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId);
            var destination = card.IsGeneralWeapon && cost.From.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : _coreAlternativeViewAsCostsOperations.IsProgramAlternativeCost(a, m.CardId) ? CardLocation.DrawPile : CardLocation.Processing;
            destination = _coreAlternativeViewAsCostsOperations.NormalizeProgramViewAsCostDestination(card, cost.From, destination, reason);
            if (m.To != destination)
                return false;
        }

        return b.Movements.Any(m => m.From.Zone == CardZoneKind.Hand && m.From.OwnerSeat is { } provider && provider != a.ActorSeat && b.SourceCounts.Any(c => c.Location == m.From && c.CountBefore > 0 && c.CountAfter == 0) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsMaterialAuthorizedEvent>().Any(e => e.UseId == use.Id && e.ActionId == a.ActionId && e.Actor == a.ActorSeat && e.CardId == m.CardId && e.From == m.From && e.Before < m.Sequence));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.CaptureMovement, 48900)]
    internal void CaptureShiLsUsePayment(CardMovementBatchContext b)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not CardUseFrame use || !ShiLsUseMaterialsMatch(use, b) || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !LiveShiLsGrants().Any(g => ShiLsRefillGrant(g.Source.OwnerSeat, g.Source.SkillInstanceId, b)?.GrantId == g.GrantId))
            return;
        if (use.ShiLuSuPayment is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsUsePaymentCapturedEvent>().Any(e => e.UseId == use.Id))
            throw new InvalidOperationException("One Haoshi-observed native use cannot pay its original physical materials twice.");
        _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ShiLuSuPayment = new(use.Id, use.Action!.ActionId, b.Movements[0].Sequence - 1, b.Movements[^1].Sequence) { Batch = b } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShiLsUsePaymentCapturedEvent(use.Id, use.Action!.ActionId, b.Id));
    }

    [ProgramCardUsePayment(ProgramCardUsePaymentStage.MatchBatch, 48900)]
    internal bool IsShiLsUsePaymentBatch(CardUseFrame use, CardMovementBatchContext b) => use.ShiLuSuPayment is { } paid && ShiLsUseMaterialsMatch(use, b) && CoreGameEngineCardUsePaymentsRuntime.CardUsePaymentBatchMatchesReceipt(use, b, paid) && _coreProgramEventHistoryOperations.ProgramEventHistory<ShiLsUsePaymentCapturedEvent>().Count(e => e.UseId == use.Id && e.ActionId == use.Action!.ActionId && e.BatchId == b.Id) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.TrailingTail, 48900)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.HpObserverTail, 48900)]
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.SharedProcessingTail, 48900)]
    internal bool HasShiLsUsePaymentTail(CardUseFrame use, RecoveryPaidCardUseContinuation continuation)
    {
        if (continuation.SourceSeat != use.SourceSeat || use.ShiLuSuPayment is not { } paid || !IsShiLsUsePaymentBatch(use, paid.Batch))
            return false;
        return continuation.Kind switch
        {
            RecoveryPaidCardUseKind.Trick => continuation.CardId == use.CardId && continuation.ActionKind is not null && continuation.TargetSeats is { } targets && targets.SequenceEqual(use.TargetSeats) && (continuation.PlayedCardKind ?? use.CardKind) == use.CardKind,
            RecoveryPaidCardUseKind.Simple => continuation.CardId == use.CardId && continuation.Simple?.CardId == use.CardId,
            RecoveryPaidCardUseKind.EquipmentUse => continuation.CardId == use.CardId && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(use.CardKind) && continuation.EquipmentSlot == _coreEquipmentContentOperations.EquipmentRules.Get(use.CardKind).Slot,
            _ => false
        };
    }

    [NativeUseValidation(48900)]
    internal void ValidateShiLsUsePayment(CardUseFrame use)
    {
        if (use.ShiLuSuPayment is { } paid && !IsShiLsUsePaymentBatch(use, paid.Batch))
            throw new InvalidOperationException("Haoshi lost the exact original native Use payment invoice.");
    }
}
