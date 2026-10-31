#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHalfHandPhaseDebtProgramsRuntime
{
    internal long HalfHandDebtSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count == 0 ? 0 : _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements[^1].Sequence;

    internal CardConversionSource HalfHandDebtSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HalfHandDebtSourceCurrent(ProgramSkillFrame f) => _coreMatchOutcomeWinnerAccess._winner == Winner.None && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId && p.GameplayHash == f.GameplayHash);
    internal bool HalfHandDebtLifecycleMatches(ProgramSkillFrame f, SkillProgramTriggerWindow window)
    {
        if (f.WindowContext is not { } context || context.Window != window || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _resolutionStackOperations._resolutionStack.FirstOrDefault(frame => frame.Id == context.ParentFrameId)is not ProgramLifecycleTriggerWindowFrame parent || parent.OwnerSeat != f.OwnerSeat || parent.Window != window || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        return _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber > 0;
    }

    internal HalfHandDrawIssuedEvent? CurrentHalfHandExtraDraw(int owner, string skill, string instance, string hash, string state)
    {
        if (owner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return null;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HalfHandDrawIssuedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance && e.GameplayHash == hash && e.StateId == state && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwnerSeat == owner);
    }

    internal HalfHandTargetSupportIssuedEvent? CurrentHalfHandSupport(int owner, string skill, string instance, string hash, string state)
    {
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var issued = history.OfType<HalfHandTargetSupportIssuedEvent>().LastOrDefault(e => e.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance && e.GameplayHash == hash && e.StateId == state && e.ActualDeliveredCount > 0);
        return issued is not null && !history.OfType<TurnStartedEvent>().Any(e => e.ActorSeat == owner && e.TurnNumber > issued.ActualTurnNumber) && _coreMatchParticipantsPlayersAccess._players[owner].IsAlive && _coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], skill, instance) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[owner]).Any(p => p.Id == skill && p.GameplayHash == hash) ? issued : null;
    }

    internal PhaseHandExchangeDebtIssuedEvent? CurrentPhaseHandDebt(int owner, string skill, string instance, string hash, string state)
    {
        if (_coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || owner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat)
            return null;
        var history = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray();
        var issued = history.OfType<PhaseHandExchangeDebtIssuedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner && e.Source.SkillId == skill && e.Source.SkillInstanceId == instance && e.GameplayHash == hash && e.StateId == state && e.ActualTurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && e.TurnOwnerSeat == owner && e.PhaseInstanceId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        return issued is not null && !history.OfType<PhaseHandExchangeDebtPaymentStartedEvent>().Any(e => e.ExchangeProgramFrameId == issued.ProgramFrameId) ? issued : null;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 750)]
    internal bool CanRunHalfHandPhaseDebt(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.GiveHalfHandAndIssueTargetSupport)is { } gift)
        {
            var draw = CurrentHalfHandExtraDraw(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.GameplayHash, gift.StateId!);
            return context.Window == SkillProgramTriggerWindow.AfterNormalDraw && draw is not null && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]).Count > 5 && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HalfHandTargetSupportIssuedEvent>().Any(e => e.DrawProgramFrameId == draw.ProgramFrameId);
        }

        if (trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.SelectFrozenHandExchangeDebtPayment)is { } payment)
            return context.Window == SkillProgramTriggerWindow.PlayEnding && CurrentPhaseHandDebt(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.GameplayHash, payment.StateId!)is not null;
        if (trigger.Effects.FirstOrDefault(e => e.Op == SkillProgramEffectOp.OfferHalfHandRecipientSupport)is { } aid)
            return context.Window == SkillProgramTriggerWindow.OtherActualUseTargeted && CurrentHalfHandSupport(candidate.OwnerSeat, candidate.SkillId, candidate.SkillInstanceId, candidate.GameplayHash, aid.StateId!)is { } support && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[support.RecipientSeat]).Count > 0;
        return true;
    }

    internal SkillProgramStepOutcome DrawExtraHalfHandSupport(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.HalfHandDraw is not null || frame.InstructionIndex != 1 || !HalfHandDebtLifecycleMatches(frame, SkillProgramTriggerWindow.DrawPhaseStarting))
            throw new InvalidOperationException("Half-hand extra draw lost its real actual draw candidate.");
        var before = HalfHandDebtSequence;
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        _coreProgramLifecycleOperations.DrawProgramCards(frame.Id, frame.OwnerSeat, effect.Amount, null, null, SkillProgramCardSetVisibility.Private, reason);
        var after = HalfHandDebtSequence;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > before && m.Sequence <= after && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat) && m.Reason == reason);
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var receipt = new ProgramHalfHandDrawReceipt(0, frame.WindowContext!.ParentFrameId, HalfHandDebtSource(frame), frame.GameplayHash, effect.StateId!, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, effect.Amount, actual, before, after);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HalfHandDraw = receipt });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HalfHandDrawIssuedEvent(frame.Id, receipt.DrawWindowFrameId, receipt.Source, receipt.GameplayHash, receipt.StateId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, effect.Amount, actual, before, after));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    internal SkillProgramStepOutcome GiveHalfHandSupport(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (!HalfHandDebtLifecycleMatches(frame, SkillProgramTriggerWindow.AfterNormalDraw) || frame.InstructionIndex != 3 || frame.HalfHandGift is not null || frame.SelectedTargetSeats is not [var recipient] || recipient == frame.OwnerSeat || frame.CardSetBindings.SingleOrDefault(b => b.Name == effect.SourceBind)is not { } binding || binding.CardIds.Count == 0 || binding.SourceLocations.Count != binding.CardIds.Count || binding.SourceLocations.Any(l => l != CardLocation.Hand(frame.OwnerSeat)) || CurrentHalfHandExtraDraw(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, effect.StateId!)is not { } drawn)
            throw new InvalidOperationException("Half-hand support lost its exact real draw and selected owned gift.");
        if (!HalfHandDebtSourceCurrent(frame) || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
            return SkillProgramStepOutcome.Continue;
        if (_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<HalfHandTargetSupportIssuedEvent>().Any(e => e.DrawProgramFrameId == drawn.ProgramFrameId))
            throw new InvalidOperationException("One original draw cannot issue a second half-hand recipient.");
        var handCount = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Count;
        if (handCount <= 5 || binding.CardIds.Count != handCount / 2 || binding.CardIds.Any(id => _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.Hand(frame.OwnerSeat)))
            throw new InvalidOperationException("The original half-hand selection no longer owns its full real Hand payment.");
        var reason = new CardMoveReason($"skill-program.{(frame.SkillId)}.{(effect.Op)}");
        var payment = new ProgramHalfHandGiftPayment(2, drawn.ProgramFrameId, HalfHandDebtSource(frame), frame.GameplayHash, effect.StateId!, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, frame.OwnerSeat, recipient, handCount, binding.CardIds, HalfHandDebtSequence, HalfHandDebtSequence);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { HalfHandGift = payment, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(binding.CardIds, CardLocation.Hand(recipient), reason, _characterHuangQuanMouOperations.NativeGiveCompletionRecords(frame.Id, frame.OwnerSeat, recipient, binding.CardIds, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            if (records.Count != payment.CardIds.Count || records.Any(m => m.From != CardLocation.Hand(frame.OwnerSeat) || m.To != CardLocation.Hand(recipient)))
                throw new InvalidOperationException("Only a full actual half-hand payment may issue its recipient qualification.");
            var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
            var after = records.Max(m => m.Sequence);
            _coreRuntimeOperations.ReplaceRuntimeTop(current with { HalfHandGift = payment with { SequenceAfter = after } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HalfHandTargetSupportIssuedEvent(frame.Id, drawn.ProgramFrameId, payment.Source, frame.GameplayHash, effect.StateId!, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, frame.OwnerSeat, recipient, records.Count, payment.SequenceBefore, after));
        }));
        if (!_coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frame.Id))
            _coreRuntimeOperations.ReturnRuntimeProgramMovement(frame.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal bool IsPayableDeferredHandPair(int owner, int first, int second) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(first) && _coreTableSeatQueriesOperations.IsValidPlayerSeat(second) && first != second && first != owner && second != owner && _coreMatchParticipantsPlayersAccess._players[first].IsAlive && _coreMatchParticipantsPlayersAccess._players[second].IsAlive && Math.Abs(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[first]).Count - _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[second]).Count) <= _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Count + _corePhysicalOwnedCardQueriesOperations.GetEquipment2(owner).Count;
    internal bool HasPayableDeferredHandPair(int owner) => _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != owner).Any(a => _coreMatchParticipantsPlayersAccess._players.Any(b => IsPayableDeferredHandPair(owner, a.Seat, b.Seat)));
    internal SkillProgramStepOutcome ExchangeAndArmPhaseHandDebt(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.PhaseHandExchange is not null || frame.WindowContext is not null || frame.InstructionIndex != 1 || frame.SelectedCardIds.Count != 0 || frame.SelectedTargetSeats is not [var a, var b] || !HalfHandDebtSourceCurrent(frame) || frame.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat || _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase != TurnPhase.Play || !IsPayableDeferredHandPair(frame.OwnerSeat, a, b))
            throw new InvalidOperationException("Deferred hand exchange requires one original payable living pair in the actual Play phase.");
        var first = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[a]).Select(c => c.Id).Order().ToArray();
        var second = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[b]).Select(c => c.Id).Order().ToArray();
        var receipt = new ProgramPhaseHandExchangeReceipt(0, HalfHandDebtSource(frame), frame.GameplayHash, effect.StateId!, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, a, b, Math.Abs(first.Length - second.Length), first, second, HalfHandDebtSequence, HalfHandDebtSequence, false);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandExchange = receipt });
        return _coreProgramHandExchangesOperations.ExchangeProgramSelectedTargetHands(frame);
    }

    internal void FinalizeDeferredHandExchange(long frameId)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        if (frame.PhaseHandExchange is not { Issued: false } receipt)
            return;
        var after = HalfHandDebtSequence;
        var reason = $"skill-program.{(frame.SkillId)}.exchange";
        bool Paid(int id, int from, int to) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= after && m.CardId == id && m.From == CardLocation.Hand(from) && m.To == CardLocation.Processing && m.Reason.Value == reason) == 1 && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > receipt.SequenceBefore && m.Sequence <= after && m.CardId == id && m.From == CardLocation.Processing && m.To == CardLocation.Hand(to) && m.Reason.Value == reason) == 1;
        if (receipt.FirstCardIds.Any(id => !Paid(id, receipt.FirstSeat, receipt.SecondSeat)) || receipt.SecondCardIds.Any(id => !Paid(id, receipt.SecondSeat, receipt.FirstSeat)))
            throw new InvalidOperationException("The exchange debt cannot issue before all original hand entities transfer.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { PhaseHandExchange = receipt with { SequenceAfter = after, Issued = true } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandExchangeDebtIssuedEvent(frame.Id, receipt.Source, receipt.GameplayHash, receipt.StateId, receipt.ActualTurnNumber, receipt.TurnOwnerSeat, receipt.PhaseInstanceId, receipt.FirstSeat, receipt.SecondSeat, receipt.FrozenDifference, receipt.FirstCardIds.Count, receipt.SecondCardIds.Count, receipt.SequenceBefore, after));
    }

    internal bool MatchesDeferredHandDebtCost(ProgramSkillFrame frame, string bind, Card card, CardLocation location) => frame.PhaseHandDebtPayment is not { } paid || paid.ResultBind != bind || location.OwnerSeat == frame.OwnerSeat && location.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, card, location, OwnedCardMoveIntent.Discard) && !(location.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, card));
    internal SkillProgramStepOutcome SelectPhaseHandDebtPayment(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        if (frame.PhaseHandDebtPayment is not null || frame.InstructionIndex != 1 || !HalfHandDebtLifecycleMatches(frame, SkillProgramTriggerWindow.PlayEnding))
            throw new InvalidOperationException("Phase hand debt lost its exact current PlayEnding candidate.");
        var due = CurrentPhaseHandDebt(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, frame.GameplayHash, effect.StateId!);
        if (due is null)
        {
            _coreProgramLifecycleOperations.SetProgramCardSet(frame.Id, effect.ResultBind!, [], SkillProgramCardSetVisibility.Private, []);
            return SkillProgramStepOutcome.Continue;
        }

        var available = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Select(c => (Card: c, Location: CardLocation.Hand(frame.OwnerSeat))).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment2(frame.OwnerSeat).Select(c => (Card: c, Location: CardLocation.Equipment(frame.OwnerSeat)))).Count(item => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(frame.OwnerSeat, item.Card, item.Location, OwnedCardMoveIntent.Discard) && !(item.Location.Zone == CardZoneKind.Equipment && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(frame.OwnerSeat, frame.SkillId, frame.SkillInstanceId, item.Card)));
        var count = Math.Min(due.FrozenDifference, available);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { PhaseHandDebtPayment = new(0, effect.StateId!, effect.ResultBind!, due.ProgramFrameId, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, due.FrozenDifference, count, HalfHandDebtSequence) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PhaseHandExchangeDebtPaymentStartedEvent(frame.Id, due.ProgramFrameId, HalfHandDebtSource(frame), frame.GameplayHash, effect.StateId!, _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber, frame.OwnerSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, due.FrozenDifference, count));
        return _coreProgramOwnedCardsOperations.SelectProgramOwnedCards(frame, frame.OwnerSeat, count, null, [CardZoneKind.Hand, CardZoneKind.Equipment], effect.ResultBind!, 0, 0, [], []);
    }

    internal SkillProgramEffect PhaseHandDebtOwnedSelectionEffect(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (effect.Op != SkillProgramEffectOp.SelectFrozenHandExchangeDebtPayment)
            return effect;
        var paid = frame.PhaseHandDebtPayment ?? throw new InvalidOperationException("Missing frozen phase debt selection.");
        if (paid.InstructionIndex != frame.InstructionIndex - 1 || paid.StateId != effect.StateId || paid.ResultBind != effect.ResultBind)
            throw new InvalidOperationException("The phase debt selection outlived its original frozen instruction.");
        return new(SkillProgramEffectOp.SelectOwnedCards, SkillProgramEffectTarget.Owner, paid.RequiredPaymentCount, effect.Condition, zones: [CardZoneKind.Hand, CardZoneKind.Equipment], resultBind: effect.ResultBind);
    }

    SkillProgramStepOutcome global::CardGame.Core.IHalfHandPhaseDebtProgramHost.DrawExtraAndArmHalfHandSupport(ProgramSkillFrame f, SkillProgramEffect e) => DrawExtraHalfHandSupport(f, e);
    SkillProgramStepOutcome global::CardGame.Core.IHalfHandPhaseDebtProgramHost.GiveHalfHandAndIssueTargetSupport(ProgramSkillFrame f, SkillProgramEffect e) => GiveHalfHandSupport(f, e);
    SkillProgramStepOutcome global::CardGame.Core.IHalfHandPhaseDebtProgramHost.ExchangeHandsAndArmPhaseDebt(ProgramSkillFrame f, SkillProgramEffect e) => ExchangeAndArmPhaseHandDebt(f, e);
    SkillProgramStepOutcome global::CardGame.Core.IHalfHandPhaseDebtProgramHost.SelectFrozenHandExchangeDebtPayment(ProgramSkillFrame f, SkillProgramEffect e) => SelectPhaseHandDebtPayment(f, e);
    SkillProgramStepOutcome global::CardGame.Core.IHalfHandPhaseDebtProgramHost.OfferHalfHandRecipientSupport(ProgramSkillFrame f, SkillProgramEffect e) => _coreHalfHandTargetSupportOperations.OfferHalfHandSupport(f, e);
}
