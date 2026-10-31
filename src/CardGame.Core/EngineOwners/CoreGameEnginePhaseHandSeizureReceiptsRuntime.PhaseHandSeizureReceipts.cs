#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePhaseHandSeizureReceiptsRuntime
{
    internal bool ValidPhaseHandSeizureReceipt(ProgramSkillFrame frame)
    {
        if (frame.PhaseHandSeizure is not { } receipt || frame.PhaseHandDebtReturn is not null || frame.InstructionIndex != 1 || receipt.InstructionIndex != 1 || frame.TriggerId is not null || frame.WindowContext is not null || frame.SelectedCardIds is not [var cost] || cost != receipt.CostCardId || frame.SelectedTargetSeats is not [var target] || target != receipt.TargetSeat || receipt.ActualTurnNumber != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play && !_corePhaseHandSeizureOperations.PhaseHandSeizureTerminalState || receipt.PhaseInstanceId != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || receipt.CostFrom.OwnerSeat != frame.OwnerSeat || receipt.CostFrom.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || receipt.CostBefore < 0 || receipt.CostAfter <= receipt.CostBefore || ProgramInstructionResolver.Default.Resolve(frame, _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program!).Instructions is not [{ Op: SkillProgramEffectOp.DiscardTurnOverAndTakeHand, StateId: var continuation }] || continuation != receipt.ContinuationId)
            return false;
        var paid = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandSeizurePaidEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (paid is not [var fact] || fact != new PhaseHandSeizurePaidEvent(frame.Id, _corePhaseHandSeizureOperations.PhaseHandSeizureSource(frame), frame.GameplayHash, target, receipt.ActualTurnNumber, receipt.PhaseInstanceId, cost, receipt.CostFrom, receipt.CostBefore, receipt.CostAfter) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandSeizurePaidEvent>().Count(e => e.Source.OwnerSeat == frame.OwnerSeat && e.Source.SkillId == frame.SkillId && e.ActualTurnNumber == receipt.ActualTurnNumber && e.PhaseInstanceId == receipt.PhaseInstanceId) != 1)
            return false;
        var costs = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.CostBefore && m.Sequence <= receipt.CostAfter && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandSeizureReason(frame, "payment")).ToArray();
        if (costs is not [var move] || move.CardId != cost || move.From != receipt.CostFrom || !(move.To == CardLocation.DiscardPile || move.To == CardLocation.OutsideGame && (_coreAdvancedLifecycleOperations.GetAdvancedCard(cost).IsGeneralWeapon || _coreEphemeralCardMaterialsOperations.IsEphemeralCardDestroyedMaterial(move))))
            return false;
        var flipped = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandSeizureTurnedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        var issued = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandSeizureIssuedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (receipt.Stage == PhaseHandSeizureStage.PaymentChildren)
            return flipped.Length == 0 && issued.Length == 0 && receipt.TakenCardIds.Count == 0 && receipt.TakeBefore == 0 && receipt.TakeAfter == 0;
        if (flipped is not [var turn] || turn != new PhaseHandSeizureTurnedEvent(frame.Id, frame.OwnerSeat, receipt.WasFaceDown, !receipt.WasFaceDown))
            return false;
        if (receipt.Stage == PhaseHandSeizureStage.FlipChildren)
            return issued.Length == 0 && receipt.TakenCardIds.Count == 0 && receipt.TakeBefore == 0 && receipt.TakeAfter == 0;
        if (receipt.Stage != PhaseHandSeizureStage.TakeChildren || receipt.TakeBefore < receipt.CostAfter || receipt.TakeAfter < receipt.TakeBefore || receipt.TakenCardIds.Distinct().Count() != receipt.TakenCardIds.Count)
            return false;
        var same = target == frame.OwnerSeat;
        if (issued is not [var debt] || debt != new PhaseHandSeizureIssuedEvent(frame.Id, _corePhaseHandSeizureOperations.PhaseHandSeizureSource(frame), frame.GameplayHash, receipt.ContinuationId, target, receipt.ActualTurnNumber, receipt.PhaseInstanceId, same ? 0 : receipt.TakenCardIds.Count, same, receipt.TakeBefore, receipt.TakeAfter))
            return false;
        var taken = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.TakeBefore && m.Sequence <= receipt.TakeAfter && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandSeizureReason(frame, "take")).ToArray();
        if (same)
            return receipt.TakeAfter == receipt.TakeBefore && taken.Length == 0;
        return taken.Length == receipt.TakenCardIds.Count && receipt.TakenCardIds.All(id => taken.Count(m => m.CardId == id && m.From == CardLocation.Hand(target) && m.To == CardLocation.Hand(frame.OwnerSeat)) == 1);
    }

    internal bool ValidPhaseHandDebtReturnReceipt(ProgramSkillFrame frame)
    {
        if (frame.PhaseHandDebtReturn is not { } receipt || frame.PhaseHandSeizure is not null || frame.InstructionIndex != 1 || receipt.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.ReturnIssuedPhaseHandDebt || receipt.RequiredCount < 0 || receipt.FrozenHp < 0 || receipt.RequiredCount > receipt.FrozenHp || receipt.CardIds.Count != receipt.Locations.Count || receipt.CardIds.Distinct().Count() != receipt.CardIds.Count || receipt.Locations.Any(l => l.OwnerSeat != frame.OwnerSeat || l.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)))
            return false;
        var due = _corePhaseHandSeizureOperations.RequirePhaseHandDebtParent(frame);
        if (receipt.SeizureFrameId != due.FrameId || receipt.TargetSeat != due.TargetSeat)
            return false;
        var started = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandDebtReturnStartedEvent>().Where(e => e.FrameId == frame.Id).ToArray();
        if (started is not [var begin] || begin != new PhaseHandDebtReturnStartedEvent(frame.Id, due.FrameId, frame.OwnerSeat, due.TargetSeat, due.ActualTurnNumber, due.PhaseInstanceId, receipt.FrozenHp, receipt.RequiredCount) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandDebtReturnStartedEvent>().Count(e => e.SeizureFrameId == due.FrameId) != 1)
            return false;
        var settled = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PhaseHandDebtSettledEvent>().Where(e => e.SeizureFrameId == due.FrameId).ToArray();
        if (receipt.Stage == PhaseHandDebtReturnStage.Choosing)
        {
            if (receipt.RequiredCount <= 0 || receipt.CardIds.Count >= receipt.RequiredCount || receipt.SequenceBefore != 0 || receipt.SequenceAfter != 0)
                return false;
            if (_corePhaseHandSeizureOperations.PhaseHandSeizureTerminalState)
                return settled is [var closed] && closed == new PhaseHandDebtSettledEvent(due.FrameId, 0, frame.OwnerSeat, due.TargetSeat, "game-end", 0, 0, 0, 0) && !_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence > due.After && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandDebtReturnReason(frame) && m.From.OwnerSeat == frame.OwnerSeat);
            return settled.Length == 0 && receipt.CardIds.Select((id, index) => (id, index)).All(p => _cardZonesOperations._cardZones.GetLocation(p.id) == receipt.Locations[p.index] && _corePhaseHandSeizureOperations.PhaseHandDebtCards(frame).Any(c => c.Id == p.id));
        }

        if (receipt.Stage != PhaseHandDebtReturnStage.MovementChildren || receipt.CardIds.Count != receipt.RequiredCount || receipt.SequenceBefore < 0 || receipt.SequenceAfter < receipt.SequenceBefore || settled is not [var end])
            return false;
        var moving = receipt.CardIds.Select((id, index) => (id, from: receipt.Locations[index])).Where(p => p.from != CardLocation.Hand(receipt.TargetSeat)).ToArray();
        if (end != new PhaseHandDebtSettledEvent(due.FrameId, frame.Id, frame.OwnerSeat, due.TargetSeat, moving.Length == 0 ? "same-hand" : "returned", receipt.RequiredCount, moving.Length, receipt.SequenceBefore, receipt.SequenceAfter))
            return false;
        var moved = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= receipt.SequenceAfter && m.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePhaseHandSeizureRuntime.PhaseHandDebtReturnReason(frame)).ToArray();
        return moving.Length == 0 ? receipt.SequenceBefore == receipt.SequenceAfter && moved.Length == 0 : moved.Length == moving.Length && moving.All(p => moved.Count(m => m.CardId == p.id && m.From == p.from && m.To == CardLocation.Hand(receipt.TargetSeat)) == 1);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 10)]
    internal bool IsPhaseHandSeizureMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && (effect?.Op == SkillProgramEffectOp.DiscardTurnOverAndTakeHand && frame.PhaseHandSeizure?.Stage is PhaseHandSeizureStage.PaymentChildren or PhaseHandSeizureStage.TakeChildren && ValidPhaseHandSeizureReceipt(frame) || effect?.Op == SkillProgramEffectOp.ReturnIssuedPhaseHandDebt && frame.PhaseHandDebtReturn?.Stage == PhaseHandDebtReturnStage.MovementChildren && ValidPhaseHandDebtReturnReceipt(frame));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 700)]
    internal bool CanContinuePhaseHandSeizure(ProgramSkillFrame frame)
    {
        if (frame.PhaseHandSeizure is not null)
            return ValidPhaseHandSeizureReceipt(frame);
        if (frame.PhaseHandDebtReturn is not null)
            return ValidPhaseHandDebtReturnReceipt(frame);
        if (frame.WindowContext?.Window != SkillProgramTriggerWindow.PlayEnding || frame.TriggerId is null)
            return false;
        var candidate = new ProgramTriggerCandidate(frame.OwnerSeat, frame.SkillId, frame.TriggerId, frame.SkillInstanceId, frame.GameplayHash, 0, 0);
        if (!_corePhaseHandSeizureOperations.CanRunIssuedPhaseHandDebt(candidate, frame.WindowContext))
            return false;
        _ = _corePhaseHandSeizureOperations.RequirePhaseHandDebtParent(frame);
        return true;
    }

    internal void AssertPhaseHandSeizure(ProgramSkillFrame frame)
    {
        if (frame.PhaseHandSeizure is not null && !ValidPhaseHandSeizureReceipt(frame) || frame.PhaseHandDebtReturn is not null && !ValidPhaseHandDebtReturnReceipt(frame))
            throw new InvalidOperationException("A phase-hand paid receipt lost its exact source, original phase, entity movement or typed return.");
    }

    internal bool IsPhaseHandDebtChoiceLegal(PromptChoice choice) => _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame frame && frame.PhaseHandDebtReturn is { Stage: PhaseHandDebtReturnStage.Choosing } receipt && ValidPhaseHandDebtReturnReceipt(frame) && _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[receipt.TargetSeat].IsAlive && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], _corePhaseHandSeizureOperations.PhaseHandDebtChoices(frame).Where(c => c.Id == choice.Id).ToArray());
}
