#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateOfferReturnsRuntime
{
    internal bool ValidPrivateOfferReceipt(ProgramSkillFrame f)
    {
        if (f.PrivateOffer is not { } r || r.InstructionIndex != f.InstructionIndex || !_corePrivateCardOffersOperations.ValidPrivateOfferDeposit(r.Location, out var original) || original != r.CardId || r.Location.OwnerSeat != f.OwnerSeat || r.Location.PrivateTurnHold is not { DeferredOffer: { } o } hold || hold.SkillId != f.SkillId || hold.SkillInstanceId != f.SkillInstanceId || o.GameplayHash != f.GameplayHash || GetPausedPrivateOfferEffect(f)is not { } effect)
            return false;
        if (r.Stage == PrivateOfferStage.DepositChildren)
            return effect.Op == SkillProgramEffectOp.DepositBoundPrivateCardOffer && f.Id == o.DepositFrameId && global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f) == o.DepositBindingId && f.SelectedTargetSeats.SequenceEqual([o.TargetSeat]) && r.SequenceAfter > r.SequenceBefore && r.PaymentCardId is null && r.PaymentFrom is null;
        if (effect.Op != SkillProgramEffectOp.ResolveDeferredPrivateCardOffer || _corePrivateCardOffersOperations.DeferredPrivateOfferParent(f) != r.Location || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrivateCardOfferViewedEvent>().Count(e => e.FrameId == f.Id && e.DepositFrameId == hold.HoldId && e.TargetSeat == o.TargetSeat && e.ActualTurn == _coreActualTurnProgressTurnNumberAccess._turnNumber) != 1)
            return false;
        if (r.Stage == PrivateOfferStage.ChoosingExchange)
            return r.PaymentCardId is null && r.PaymentFrom is null && r.SequenceBefore == 0 && r.SequenceAfter == 0 && _cardZonesOperations._cardZones.GetLocation(r.CardId) == r.Location;
        if (r.PaymentCardId is { } paid)
        {
            if (r.PaymentFrom != CardLocation.Hand(o.TargetSeat) || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(paid).Kind) != global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId).Kind) || !PrivateOfferSegmentMatches(f, PrivateOfferStage.ExchangeChildren, paid, r.PaymentFrom, CardLocation.Hand(f.OwnerSeat), "exchange"))
                return false;
        }
        else if (r.Stage is PrivateOfferStage.ExchangeChildren or PrivateOfferStage.ObtainChildren)
            return false;
        return r.Stage switch
        {
            PrivateOfferStage.ExchangeChildren => true,
            PrivateOfferStage.ObtainChildren => PrivateOfferSegmentMatches(f, r.Stage, r.CardId, r.Location, CardLocation.Hand(o.TargetSeat), "obtain"),
            PrivateOfferStage.RemovalChildren or PrivateOfferStage.HpLoss => r.PaymentCardId is null && PrivateOfferSegmentMatches(f, PrivateOfferStage.RemovalChildren, r.CardId, r.Location, CardLocation.DiscardPile, "remove"),
            _ => false
        };
    }

    internal SkillProgramEffect? GetPausedPrivateOfferEffect(ProgramSkillFrame f) => f.InstructionIndex < 1 ? null : ProgramInstructionResolver.Default.Resolve(f, _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!).GetPausedInstruction(f.InstructionIndex).Effect;
    internal bool PrivateOfferSegmentMatches(ProgramSkillFrame f, PrivateOfferStage stage, int id, CardLocation? from, CardLocation to, string reason)
    {
        var events = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PrivateCardOfferSegmentIssuedEvent>().Where(e => e.FrameId == f.Id && e.Stage == stage).ToArray();
        if (events is not [var fact] || fact.DepositFrameId != f.PrivateOffer!.Location.PrivateTurnHold!.HoldId || fact.Count != 1 || fact.After <= fact.Before)
            return false;
        var moves = _coreCardMovementJournalCardMovementsAccess._cardMovements.Where(m => m.Sequence > fact.Before && m.Sequence <= fact.After).ToArray();
        return moves is [var actual] && actual.CardId == id && actual.From == from && actual.To == to && actual.Reason.Value == global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime.PrivateOfferReason(f, reason);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 330)]
    internal bool ResumePrivateOffer(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { PrivateOffer: { } r } f || f.Id != id)
            return false;
        if (!ValidPrivateOfferReceipt(f))
            throw new InvalidOperationException("A private offer lost its exact original deposit, due cursor or paid segment.");
        if (r.Stage == PrivateOfferStage.ChoosingExchange)
            return true;
        if (f.PendingMovementContinuation is not null)
        {
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnPrivateOfferMovement(f);
            return true;
        }

        if (r.Stage == PrivateOfferStage.HpLoss)
        {
            if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.Program))
                return true;
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrivateOffer = null });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(id), true);
            return true;
        }

        throw new InvalidOperationException("A private offer resumed without its precise movement/HP return.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 280)]
    internal bool IsPrivateOfferMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.PrivateOffer is { Stage: not PrivateOfferStage.ChoosingExchange and not PrivateOfferStage.HpLoss } r && effect == GetPausedPrivateOfferEffect(f) && ValidPrivateOfferReceipt(f) && pending.BeforeCount == 0 && pending.CoverageResultBind is null && pending.SubjectSeat == (r.Stage == PrivateOfferStage.DepositChildren ? f.OwnerSeat : r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat);
    [ProgramContinuation(ProgramContinuationKind.Movement, 320)]
    internal bool ReturnPrivateOfferMovement(ProgramSkillFrame f)
    {
        if (f.PrivateOffer is not { } r)
            return false;
        if (f.PendingMovementContinuation is not { } pending || !IsPrivateOfferMovement(f, GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("A private offer cannot clear a different program movement return.");
        if (_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (r.Stage == PrivateOfferStage.DepositChildren || r.Stage == PrivateOfferStage.ObtainChildren)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrivateOffer = null });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), true);
            return true;
        }

        var target = r.Location.PrivateTurnHold!.DeferredOffer!.TargetSeat;
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { PrivateOffer = null });
            _coreSkillProgramsOperations.FinishProgramSkill(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id), false);
            return true;
        }

        if (r.Stage == PrivateOfferStage.ExchangeChildren)
        {
            if (_cardZonesOperations._cardZones.GetLocation(r.CardId) != r.Location)
                throw new InvalidOperationException("A paid private exchange lost its deposited reward entity.");
            var before = _corePrivateCardOffersOperations.PrivateOfferSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = r with { Stage = PrivateOfferStage.ObtainChildren, SequenceBefore = before, SequenceAfter = before }, PendingMovementContinuation = new(target, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(r.Location).Single(c => c.Id == r.CardId), r.Location, CardLocation.Hand(target), new(global::CardGame.Core.EngineOwners.CoreGameEnginePrivateCardOffersRuntime.PrivateOfferReason(f, "obtain")));
            _corePrivateCardOffersOperations.CommitPrivateOfferSegment(f.Id);
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(f))
                ReturnPrivateOfferMovement(f);
            return true;
        }

        if (r.Stage != PrivateOfferStage.RemovalChildren)
            throw new InvalidOperationException("Unexpected private offer payment stage.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PrivateOffer = r with { Stage = PrivateOfferStage.HpLoss } });
        if (_kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, target, 1) != SkillProgramStepOutcome.AwaitChild)
            ResumePrivateOffer(f.Id);
        return true;
    }

    internal void AssertPrivateOfferFrames()
    {
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
            if (f.PrivateOffer is not null && !ValidPrivateOfferReceipt(f))
                throw new InvalidOperationException("Invalid deferred private offer receipt.");
        foreach (var location in _cardZonesOperations._cardZones.PrivateTurnHoldLocations.Where(l => l.PrivateTurnHold?.DeferredOffer is not null && _cardZonesOperations._cardZones.CardsAt(l).Count != 0))
            if (!_corePrivateCardOffersOperations.ValidPrivateOfferDeposit(location, out var id) || _cardZonesOperations._cardZones.CardsAt(location)is not [var card] || card.Id != id)
                throw new InvalidOperationException("An issued private deposit lost its one real original entity.");
    }
}
