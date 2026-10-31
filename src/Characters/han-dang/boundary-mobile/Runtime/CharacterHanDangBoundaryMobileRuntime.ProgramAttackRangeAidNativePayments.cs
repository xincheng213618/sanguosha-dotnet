#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanDangBoundaryMobileRuntime
{
    internal CardMoveReason ProgramAttackRangeAidReason(ProgramSkillFrame frame) => new($"skill-program.{(frame.SkillId)}.{(SkillProgramEffectOp.RequestAttackRangeAid)}");
    internal Card[] ProgramAttackRangeAidWeapons(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[seat])).Where(card => !card.IsGeneralWeapon && _coreEquipmentContentOperations.EquipmentRules.IsEquipment(card.Kind) && _coreFieldEquipmentOperations.FieldEquipmentSlot(card) == EquipmentSlot.Weapon && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Discard)).OrderBy(card => card.Id).ToArray();
    internal bool ValidProgramAttackRangeAid(ProgramSkillFrame frame)
    {
        if (frame.AttackRangeAid is not { } aid || frame.InstructionIndex < 1 || aid.InstructionIndex != frame.InstructionIndex || frame.SelectedTargetSeats is not [var target] || target != aid.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)?.Op != SkillProgramEffectOp.RequestAttackRangeAid || _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash != frame.GameplayHash || aid.ResponderSeats is not System.Collections.IList { IsReadOnly: true } || aid.ResponderSeats.Count == 0 || aid.ResponderSeats.Distinct().Count() != aid.ResponderSeats.Count || aid.ResponderSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) || seat == target) || aid.ResponderIndex < 0 || aid.ResponderIndex >= aid.ResponderSeats.Count)
            return false;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var startIndex = Array.FindLastIndex(history, entry => entry is ProgramAttackRangeAidStartedEvent start && start.FrameId == frame.Id);
        if (startIndex < 0 || history[startIndex] is not ProgramAttackRangeAidStartedEvent started || started.SkillId != frame.SkillId || started.BindingId != global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame) || started.OwnerSeat != frame.OwnerSeat || started.TargetSeat != target || !started.ResponderSeats.SequenceEqual(aid.ResponderSeats))
            return false;
        var prior = history.Skip(startIndex + 1).OfType<ProgramAttackRangeAidChoiceResolvedEvent>().Where(entry => entry.FrameId == frame.Id).ToArray();
        var skipped = history.Skip(startIndex + 1).OfType<ProgramAttackRangeAidResponderSkippedEvent>().Where(entry => entry.FrameId == frame.Id).ToArray();
        if (prior.Length + skipped.Length != aid.ResponderIndex + (aid.Payment is { Stage: ProgramAttackRangeAidPaymentStage.MovementChildren } ? 1 : 0) || prior.Any(entry => entry.SkillId != frame.SkillId || entry.BindingId != global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame) || entry.OwnerSeat != frame.OwnerSeat || entry.TargetSeat != target) || prior.Select(entry => entry.ResponderSeat).Concat(skipped.Select(entry => entry.ResponderSeat)).Distinct().Count() != prior.Length + skipped.Length || skipped.Any(entry => entry.ResponderIndex < 0 || entry.ResponderIndex >= aid.ResponderIndex || aid.ResponderSeats[entry.ResponderIndex] != entry.ResponderSeat) || !aid.ResponderSeats.Take(aid.ResponderIndex + (aid.Payment is { Stage: ProgramAttackRangeAidPaymentStage.MovementChildren } ? 1 : 0)).ToHashSet().SetEquals(prior.Select(entry => entry.ResponderSeat).Concat(skipped.Select(entry => entry.ResponderSeat))))
            return false;
        if (aid.Payment is not { } payment)
            return true;
        if (payment.ResponderIndex != aid.ResponderIndex || payment.ResponderSeat != aid.ResponderSeats[aid.ResponderIndex] || !Enum.IsDefined(payment.Stage) || payment.Before < 0 || payment.After < payment.Before || payment.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence)
            return false;
        if (payment.CardId is { } cardId)
        {
            if (payment.Stage != ProgramAttackRangeAidPaymentStage.MovementChildren || payment.Draw is not null || payment.PrintedKind is not { } kind || !_coreEquipmentContentOperations.EquipmentRules.IsEquipment(kind) || _coreEquipmentContentOperations.EquipmentRules.Get(kind).Slot != EquipmentSlot.Weapon || payment.From is not { Zone: CardZoneKind.Hand or CardZoneKind.Equipment } from || from.OwnerSeat != payment.ResponderSeat || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.CardId == cardId && move.CardKind == kind && move.Sequence > payment.Before && move.Sequence <= payment.After && move.From == from && move.To == CardLocation.Processing && move.Reason == ProgramAttackRangeAidReason(frame)) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(move => move.CardId == cardId && move.CardKind == kind && move.Sequence > payment.Before && move.Sequence <= payment.After && move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason == ProgramAttackRangeAidReason(frame)) != 1 || prior.Last().DiscardedWeaponCardId != cardId || prior.Last().DrawnCardIds.Count != 0)
                return false;
        }
        else
        {
            if (payment.PrintedKind is not null || payment.From is not null)
                return false;
            if (payment.Stage == ProgramAttackRangeAidPaymentStage.Drawing)
                return payment.Draw is null && frame.PendingMovementContinuation is null;
            if (payment.Draw is not { } draw || draw.RecipientSeat != target || draw.RequestedCount != 1 || draw.SequenceBefore != payment.Before || draw.SequenceAfter != payment.After || prior.Last().DiscardedWeaponCardId is not null || !prior.Last().DrawnCardIds.SequenceEqual(draw.ActualDrawIds))
                return false;
        }

        return frame.PendingMovementContinuation is null || frame.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null, SubjectSeat: var subject } && subject == frame.OwnerSeat;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.RequestAttackRangeAid)]
    internal void ReturnProgramAttackRangeAidNativeDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (!ValidProgramAttackRangeAid(frame) || frame.AttackRangeAid is not { Payment: { Stage: ProgramAttackRangeAidPaymentStage.Drawing, CardId: null } payment } aid || outcome.RecipientSeat != aid.TargetSeat || outcome.RequestedCount != 1)
            throw new InvalidOperationException("Attack-range aid requires its original responder's one typed native draw.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { AttackRangeAid = aid with { Payment = payment with { Stage = ProgramAttackRangeAidPaymentStage.MovementChildren, Draw = outcome, Before = outcome.SequenceBefore, After = outcome.SequenceAfter } }, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramAttackRangeAidChoiceResolvedEvent(frame.Id, frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, payment.ResponderSeat, aid.TargetSeat, null, Array.AsReadOnly(outcome.ActualDrawIds.ToArray())));
        ContinueProgramAttackRangeAidPayment(frame.Id);
    }

    internal void ContinueProgramAttackRangeAidPayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AttackRangeAid: { Payment: { } payment } aid } frame || frame.Id != id)
            return;
        if (!ValidProgramAttackRangeAid(frame))
            throw new InvalidOperationException("Attack-range aid lost its original paid responder, native entities or typed return.");
        if (payment.Stage == ProgramAttackRangeAidPaymentStage.Drawing)
        {
            if (frame.NativeDraw is not null)
                return;
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(frame, aid.TargetSeat, 1, ProgramAttackRangeAidReason(frame));
            return;
        }

        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return;
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PendingMovementContinuation = null, AttackRangeAid = frame.AttackRangeAid!with { Payment = null, ResponderIndex = aid.ResponderIndex + 1 } });
        if (!_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { AttackRangeAid = null });
            _coreSkillProgramsOperations.FinishProgramSkill(frame, true);
            return;
        }

        _coreProgramAttackRangeAidOperations.PublishProgramAttackRangeAid(frame, ProgramAttackRangeAidReason(frame));
    }
    [ProgramContinuation(ProgramContinuationKind.Resume, 56900)]
    internal bool ResumeProgramAttackRangeAidPayment(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { AttackRangeAid.Payment: not null } frame || frame.Id != id)
            return false;
        ContinueProgramAttackRangeAidPayment(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 56900)]
    internal bool ReturnProgramAttackRangeAidMovement(ProgramSkillFrame frame) => frame.AttackRangeAid?.Payment is not null && ResumeProgramAttackRangeAidPayment(frame.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 56900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 56900)]
    internal bool KeepPaidProgramAttackRangeAid(ProgramSkillFrame frame) => frame.AttackRangeAid?.Payment is not null && ValidProgramAttackRangeAid(frame);
}
