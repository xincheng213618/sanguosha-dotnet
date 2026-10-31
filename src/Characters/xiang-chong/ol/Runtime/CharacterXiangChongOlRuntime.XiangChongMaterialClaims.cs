#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXiangChongOlRuntime
{
    internal void RecordXiangChongEquipmentMaterialClaim(ProgramSkillFrame frame, XiangChongEquipmentOrigin origin)
    {
        if (origin.From != CardLocation.Processing)
            return;
        var f = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == frame.Id);
        if (!MatchesXiangChongEquipment(f, origin) || f.XiangChong is not { Lost: { } lost, MaterialClaim: null } r || f.WindowContext?.MovementBatch is not { ParentFrameId: { } originalId } batch || originalId == origin.CardUseFrameId || _coreCardUseLifecycleOperations.LifecycleCardUse(originalId)is not { Action: { Type: CardActionType.Use } action } || !action.PhysicalCards.Any(c => c.CardId == origin.CardId && c.CardKind == origin.CardKind && c.From == lost.From) || _cardZonesOperations._cardZones.GetLocation(origin.CardId) != CardLocation.Processing || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == origin.CardId)?.Sequence != r.LossSequence || !batch.Movements.Any(m => m.Sequence == r.LossSequence && m.CardId == origin.CardId && m.CardKind == origin.CardKind && m.From == lost.From && m.To == CardLocation.Processing) || _coreCardUseLifecycleOperations.LifecycleCardUse(origin.CardUseFrameId)is not { XiangChongGuyingEquipment: { } issued } || issued != origin)
            throw new InvalidOperationException("Guying's nested equipment must claim its exact original live use material before any movement.");
        var claim = new XiangChongEquipmentMaterialClaim(originalId, action.ActionId, batch.Id, r.LossSequence, lost.From, r.Source, r.GameplayHash, origin);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { XiangChong = r with { MaterialClaim = claim } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongEquipmentMaterialClaimIssuedEvent(claim));
    }

    internal bool ValidXiangEquipmentMaterialInvoice(XiangChongEquipmentMaterialClaim c) => c.Equipment.From == CardLocation.Processing && c.OriginalUseId != c.Equipment.CardUseFrameId && c.LossSequence > 0 && c.LossBatchId > 0 && c.Source.SkillId == "ol:guying" && c.Source.OwnerSeat == c.Equipment.ActorSeat && _contentRegistryOperations._contentRegistry.GetSkill(c.Source.SkillId).Program?.GameplayHash == c.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentMaterialClaimIssuedEvent>().Count(e => e.Claim == c) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongGuyingInvokedEvent>().Count(e => e.FrameId == c.Equipment.ProgramFrameId && e.OwnerSeat == c.Equipment.ActorSeat && e.Source == c.Source && e.BatchId == c.LossBatchId && e.LossSequence == c.LossSequence && e.CardId == c.Equipment.CardId) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentIssuedEvent>().Count(e => e.Origin == c.Equipment) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == c.LossSequence && m.CardId == c.Equipment.CardId && m.CardKind == c.Equipment.CardKind && m.From == c.OriginalFrom && m.To == CardLocation.Processing);
    internal bool IsXiangChongEquipmentMaterialClaim(long useId, int cardId, CardActionContext action)
    {
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentMaterialClaimIssuedEvent>())
        {
            var c = e.Claim;
            var o = c.Equipment;
            if (c.OriginalUseId != useId || c.OriginalActionId != action.ActionId || o.CardId != cardId || !ValidXiangEquipmentMaterialInvoice(c) || !action.PhysicalCards.Any(p => p.CardId == cardId && p.CardKind == o.CardKind && p.From == c.OriginalFrom))
                continue;
            if (_coreCardUseLifecycleOperations.LifecycleCardUse(o.CardUseFrameId)is { XiangChongGuyingEquipment: { } native, Action: { Type: CardActionType.Use } a } inner && native == o && a.PhysicalCards is [var cost] && cost.CardId == cardId && cost.CardKind == o.CardKind && cost.From == CardLocation.Processing && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(inner, o.ActorSeat, o.ActorSeat) && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == o.ProgramFrameId && f.XiangChong?.MaterialClaim == c) && XiangEquipmentMaterialLiveLocation(c, inner))
                return true;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentMaterialEnteredEvent>().Any(x => x.Claim == c && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(candidate => candidate.CardId == cardId && candidate.Sequence > c.LossSequence)is { } m && m.Sequence == x.Sequence && m.CardKind == o.CardKind && m.From == CardLocation.Processing && m.To == CardLocation.Equipment(x.EquippedSeat) && m.Reason == CardMoveReasons.EquipmentEnter && _coreProgramEventHistoryOperations.ProgramEventHistory<EquipmentChangedEvent>().Any(ch => ch.ResolutionId == o.CardUseFrameId && ch.PlayerSeat == x.EquippedSeat && ch.CardId == cardId && ch.CardKind == o.CardKind)) && _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentReturnedEvent>().Count(x => x.Origin == o && x.Issued) == 1)
                return true;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentResultEvent>().Any(x => x.Result.Origin == o && x.Result.EntrySequence is null && x.Result.Termination is { } terminal && ValidXiangEquipmentResult(x.Result) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == cardId && m.Sequence > c.LossSequence)?.Sequence == terminal.Sequence) && _coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentReturnedEvent>().Count(x => x.Origin == o && x.Issued) == 1)
                return true;
        }

        return false;
    }

    internal bool XiangEquipmentMaterialLiveLocation(XiangChongEquipmentMaterialClaim claim, CardUseFrame inner)
    {
        var o = claim.Equipment;
        var first = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == o.CardId && m.Sequence > claim.LossSequence);
        if (first is null)
            return _cardZonesOperations._cardZones.GetLocation(o.CardId) == CardLocation.Processing;
        if (first.From != CardLocation.Processing)
            return false;
        if (first.To == CardLocation.Equipment(inner.SourceSeat) && first.Reason == CardMoveReasons.EquipmentEnter)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<EquipmentChangedEvent>().Any(e => e.ResolutionId == inner.Id && e.PlayerSeat == inner.SourceSeat && e.CardId == o.CardId && e.CardKind == o.CardKind);
        return first.To == CardLocation.DiscardPile && (first.Reason.Value == "area.abolished.equipment" && _coreMatchParticipantsPlayersAccess._players[inner.SourceSeat].EquipmentSlotCapacity(_coreEquipmentContentOperations.EquipmentRules.Get(o.CardKind).Slot) == 0 || first.Reason == CardMoveReasons.UseFinished && !_coreMatchParticipantsPlayersAccess._players[inner.SourceSeat].IsAlive);
    }

    internal void CaptureXiangChongEquipmentMaterialEntry(ProgramSkillFrame f)
    {
        if (f.XiangChong?.MaterialClaim is not { } c)
            return;
        if (!ValidXiangEquipmentMaterialInvoice(c))
            throw new InvalidOperationException("Guying's completed nested equipment lost its original material invoice.");
        var o = c.Equipment;
        var moved = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.FirstOrDefault(m => m.CardId == o.CardId && m.Sequence > c.LossSequence);
        if (moved is not { To: { Zone: CardZoneKind.Equipment, OwnerSeat: { } seat } } || moved.From != CardLocation.Processing || moved.Reason != CardMoveReasons.EquipmentEnter || !_coreProgramEventHistoryOperations.ProgramEventHistory<EquipmentChangedEvent>().Any(e => e.ResolutionId == o.CardUseFrameId && e.PlayerSeat == seat && e.CardId == o.CardId && e.CardKind == o.CardKind))
            return;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<XiangChongEquipmentMaterialEnteredEvent>().Any(e => e.Claim == c))
            throw new InvalidOperationException("Guying cannot complete the same original equipment claim twice.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiangChongEquipmentMaterialEnteredEvent(c, moved.Sequence, seat));
    }
}
