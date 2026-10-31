#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuaTuoBoundaryMobileRuntime
{
    internal const string BoundaryHuaTuoQingnang = "boundary:qingnang-mobile", BoundaryHuaTuoDiscardReason = "skill-program.boundary:qingnang-mobile.DiscardSelected";
    internal SkillGrantPhaseExpiry? BoundaryHuaTuoCurrentPlay(int owner) => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat == owner && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0 && _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId > 0 ? new(_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, owner, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId) : null;
    internal bool ValidBoundaryHuaTuoPayment(BoundaryHuaTuoQingnangPaidEvent e)
    {
        var r = e.Payment;
        return r.Source.SkillId == BoundaryHuaTuoQingnang && r.Source.BindingId == "discard-and-heal" && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Source.OwnerSeat) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Target) && r.Phase.TurnNumber > 0 && r.Phase.PhaseActorSeat == r.Source.OwnerSeat && r.Phase.PhaseInstanceId > 0 && r.MovementSequence > r.Before && r.Hash == _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash && r.Red == (r.Suit is Suit.Heart or Suit.Diamond) && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(start => start.FrameId == e.FrameId && start.OwnerSeat == r.Source.OwnerSeat && start.SkillId == r.Source.SkillId && start.ActivationId == r.Source.BindingId) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == r.MovementSequence && m.CardId == r.CardId && m.CardKind == r.Kind && m.From == CardLocation.Hand(r.Source.OwnerSeat) && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId), m.From, CardLocation.DiscardPile, new(BoundaryHuaTuoDiscardReason)) && m.Reason.Value == BoundaryHuaTuoDiscardReason) == 1;
    }

    internal IEnumerable<BoundaryHuaTuoQingnangPaidEvent> BoundaryHuaTuoPayments(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryHuaTuoQingnangPaidEvent>().Where(e => e.Payment.Source.OwnerSeat == owner && ValidBoundaryHuaTuoPayment(e));
    int global::CardGame.Core.IBoundaryHuaTuoSelection.PhaseLimit(int owner) => BoundaryHuaTuoCurrentPlay(owner)is { } phase ? 1 + BoundaryHuaTuoPayments(owner).Count(e => e.Payment.Phase == phase && e.Payment.Red) : 0;
    bool global::CardGame.Core.IBoundaryHuaTuoSelection.MaySelect(int owner, int target) => !BoundaryHuaTuoPayments(owner).Any(e => e.Payment.Phase.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.Payment.Target == target);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 60910)]
    internal void CaptureBoundaryHuaTuoStart(ProgramSkillStartedEvent started)
    {
        if (started.SkillId != BoundaryHuaTuoQingnang)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(started.FrameId);
        if (f.TriggerId is not null || f.ActivationId != "discard-and-heal" || f.InstructionIndex != 0 || f.BoundaryHuaTuoQingnang is not null || f.SelectedCardIds is not [var cardId] || f.SelectedTargetSeats is not [var target] || BoundaryHuaTuoCurrentPlay(f.OwnerSeat)is not { } phase || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(f.OwnerSeat))
            throw new InvalidOperationException("Qingnang must freeze its real owner hand and actual Play before payment.");
        var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(cardId);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { BoundaryHuaTuoQingnang = new(new(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId), f.GameplayHash, phase, target, cardId, card.Kind, suit, suit is Suit.Heart or Suit.Diamond, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence) });
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 60911)]
    internal void CaptureBoundaryHuaTuoPayment(CardMovedEvent moved)
    {
        if (moved.Reason.Value != BoundaryHuaTuoDiscardReason || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryHuaTuoQingnang: { MovementSequence: 0 } r } f || moved.CardId != r.CardId || moved.From != CardLocation.Hand(f.OwnerSeat) || f.InstructionIndex != 1 || f.SelectedCardPayment is not { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: false } payment || !payment.CardIds.SequenceEqual([r.CardId]))
            return;
        var movement = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.CardId == moved.CardId && m.From == moved.From && m.To == moved.To && m.Reason == moved.Reason);
        var paid = r with
        {
            MovementSequence = movement.Sequence
        };
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { BoundaryHuaTuoQingnang = paid });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryHuaTuoQingnangPaidEvent(f.Id, paid));
    }

    internal bool ValidBoundaryHuaTuoFrame(ProgramSkillFrame f) => f.BoundaryHuaTuoQingnang is { } r && f.SkillId == BoundaryHuaTuoQingnang && f.ActivationId == "discard-and-heal" && f.TriggerId is null && r.Source == new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) && r.Hash == f.GameplayHash && r.Hash == _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash && f.SelectedCardIds.SequenceEqual([r.CardId]) && f.SelectedTargetSeats.SequenceEqual([r.Target]) && r.Phase.PhaseActorSeat == f.OwnerSeat && (r.MovementSequence == 0 ? _cardZonesOperations._cardZones.GetLocation(r.CardId) == CardLocation.Hand(f.OwnerSeat) : _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryHuaTuoQingnangPaidEvent>().Count(e => e.FrameId == f.Id && e.Payment == r && ValidBoundaryHuaTuoPayment(e)) == 1);
    [ProgramContinuation(ProgramContinuationKind.Movement, 60910)]
    internal bool ReturnPaidBoundaryHuaTuoDiscard(ProgramSkillFrame f)
    {
        if (f.BoundaryHuaTuoQingnang is null || f.SelectedCardPaymentResult is not null)
            return false;
        if (f.BoundaryHuaTuoQingnang is not { MovementSequence: > 0 } r || !ValidBoundaryHuaTuoFrame(f) || f.InstructionIndex != 1 || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != f.Id || f.PendingMovementContinuation is not null || f.SelectedCardPayment is not { InstructionIndex: 1, Operation: SkillProgramEffectOp.DiscardSelected, MovementCommitted: true, ActiveChildFrameId: null } payment || !payment.CardIds.SequenceEqual([r.CardId]) || !payment.CardIds.SequenceEqual(f.SelectedCardIds))
            throw new InvalidOperationException("Qingnang cannot return before its exact original paid hand discard and movement children complete.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { SelectedCardPaymentResult = new(payment.InstructionIndex, payment.Operation, payment.LastCompletedChildFrameId, true) });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 60910)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 60910)]
    internal bool KeepPaidBoundaryHuaTuo(ProgramSkillFrame f) => f.BoundaryHuaTuoQingnang is { MovementSequence: > 0 } && ValidBoundaryHuaTuoFrame(f);
}
