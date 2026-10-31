#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineSourceCurseObserversRuntime
{
    internal bool SourceCurseMovementMatches(SourceCurseProgramReceipt r, int cardId, CardLocation from, CardLocation to, string reason) => _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == cardId && m.From == from && m.To == to && m.Reason.Value == reason) == 1;
    internal bool ValidSourceCurseReceipt(ProgramSkillFrame f)
    {
        if (f.SourceCurseReceipt is not { } r || r.InstructionIndex != f.InstructionIndex || r.ActualTurn != _coreActualTurnProgressTurnNumberAccess._turnNumber || r.Before < 0 || r.After < r.Before || _coreSourceCursesOperations.SourceCursePausedEffect(f)is not { } effect)
            return false;
        if (r.Stage == SourceCurseStage.DepositChildren)
        {
            if (effect.Op != SkillProgramEffectOp.DepositSelectedSourceCurse || r.Deposit is not { } d || d.DepositFrameId != f.Id || d.Source != new CardConversionSource(f.SkillId, f.ActivationId, f.OwnerSeat, f.SkillInstanceId) || d.GameplayHash != f.GameplayHash || d.CreatedTurn != r.ActualTurn || !f.SelectedCardIds.SequenceEqual([d.CardId]) || !f.SelectedTargetSeats.SequenceEqual([d.TargetSeat]) || r.PaymentFrom is not { } from || from.OwnerSeat != f.OwnerSeat || from.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.Activations.Single(a => a.Id == f.ActivationId).SourceZones.Contains(from.Zone) || r.After <= r.Before || !SourceCurseMovementMatches(r, d.CardId, from, d.Location, "program.source-curse.deposit"))
                return false;
            return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseDepositedEvent>().Count(e => e.Deposit == d && e.From == from && _coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == e.MovementSequence && m.CardId == d.CardId && m.From == from && m.To == d.Location)) == 1;
        }

        if (r.Stage == SourceCurseStage.LossHpChildren)
        {
            if (effect.Op != SkillProgramEffectOp.LoseHpForLostSourceCurses || r.Deposit is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterTurnEnded } context || context.ParentFrameId != r.OriginalParentId || r.LossIndex < 0 || r.LossIndex > r.Losses.Count || r.Losses.Count == 0 || r.Losses.Select(l => l.Deposit.TargetSeat).Distinct().Count() != r.Losses.Count || r.Losses.Any(l => l.ActualTurn != r.ActualTurn || !_coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurseLosses.Contains(l) || l.Deposit.Source.OwnerSeat != f.OwnerSeat || l.Deposit.Source.SkillId != f.SkillId || l.Deposit.Source.SkillInstanceId != f.SkillInstanceId || l.Deposit.GameplayHash != f.GameplayHash) || _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().LastOrDefault(w => w.Id == r.OriginalParentId)is not { } parent || !_coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, f, context))
                return false;
            return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseLossRosterIssuedEvent>().Count(e => e.ProgramFrameId == f.Id && e.ActualTurnEndFrameId == r.OriginalParentId && e.ActualTurn == r.ActualTurn && e.Losses.SequenceEqual(r.Losses)) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseHpLossIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).All(e => e.ActualTurn == r.ActualTurn && e.RosterIndex >= 0 && e.RosterIndex < r.LossIndex && r.Losses[e.RosterIndex].Deposit.TargetSeat == e.TargetSeat) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseHpLossIssuedEvent>().Where(e => e.ProgramFrameId == f.Id).GroupBy(e => e.RosterIndex).All(g => g.Count() == 1);
        }

        if (effect.Op != SkillProgramEffectOp.DrawForSourceCurseUse || r.Deposit is not { } curse || curse.Source.OwnerSeat != f.OwnerSeat || curse.BenefitSkillId != f.SkillId || curse.BenefitSkillInstanceId != f.SkillInstanceId || curse.BenefitGameplayHash != f.GameplayHash || r.DrawCount is < 0 or > 1 || curse.ActualDrawCount is < 0 or > 2 || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, SourceCurseUse: { } original } c || c.ParentFrameId != r.OriginalParentId || original.ActionId != r.ActionId || !global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime.SameSourceCurse(original.Deposit, curse) || original.ActorSeat != curse.TargetSeat || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().LastOrDefault(w => w.Id == r.OriginalParentId)is not { } window || window.Action.ActionId != r.ActionId || window.Action.ActorSeat != original.ActorSeat || window.Action.Type != CardActionType.Use || window.Action.EffectiveIsRed != original.EffectiveIsRed || !global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime.MatchesSourceCurseCard(curse, window.Action, effect) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
            return false;
        var facts = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseDrawIssuedEvent>().Where(e => e.ProgramFrameId == f.Id && e.ParentFrameId == r.OriginalParentId && e.ActionId == r.ActionId).ToArray();
        if (facts is not [var issued] || issued.Deposit != curse || issued.ActualDrawCount != r.DrawCount || issued.After < issued.Before || _coreCardMovementJournalCardMovementsAccess._cardMovements.Count(m => m.Sequence > issued.Before && m.Sequence <= issued.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "program.source-curse.draw") != r.DrawCount)
            return false;
        if (r.Stage == SourceCurseStage.DrawChildren)
            return r.Before == issued.Before && r.After == issued.After;
        if (r.Stage != SourceCurseStage.ObtainChildren || r.DrawCount != 1 || curse.ActualDrawCount != 2 || r.After <= r.Before)
            return false;
        if (effect.SourceCurseDiscardAfterSecondDraw == true)
            return SourceCurseMovementMatches(r, curse.CardId, curse.Location, CardLocation.DiscardPile, "program.source-curse.remove") && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseRemovedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Deposit == curse && e.Before == r.Before && e.After == r.After) == 1;
        return SourceCurseMovementMatches(r, curse.CardId, curse.Location, CardLocation.Hand(f.OwnerSeat), "program.source-curse.obtain") && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseObtainedEvent>().Count(e => e.ProgramFrameId == f.Id && e.Deposit == curse && e.Before == r.Before && e.After == r.After) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.BeforeSelectionState, 360)]
    internal void AssertSourceCurseReceipt(ProgramSkillFrame f)
    {
        if (f.SourceCurseReceipt is not null && !ValidSourceCurseReceipt(f))
            throw new InvalidOperationException("A source curse lost its original physical deposit, paid segment or exact typed parent.");
    }

    internal void AssertSourceCurseState()
    {
        if (_coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Select(c => c.TargetSeat).Distinct().Count() != _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Count || _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Select(c => c.CardId).Distinct().Count() != _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Count || _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Select(c => c.DepositFrameId).Distinct().Count() != _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses.Count)
            throw new InvalidOperationException("Each target has at most one distinct physical source curse.");
        foreach (var d in _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurses)
            if (d.DepositFrameId <= 0 || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.Source.OwnerSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(d.TargetSeat) || d.Source.OwnerSeat == d.TargetSeat || d.ActualDrawCount is < 0 or > 2 || _cardZonesOperations._cardZones.CardsAt(d.Location)is not [var card] || card.Id != d.CardId || card.Kind != d.CardKind || card.Suit != d.PrintedSuit || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<SourceCurseDepositedEvent>().Count(e => global::CardGame.Core.EngineOwners.CoreGameEngineSourceCursesRuntime.SameSourceCurse(e.Deposit, d)) != 1)
                throw new InvalidOperationException("An issued curse lost its actual target-owned card or original source identity.");
        foreach (var loss in _coreGameEngineSourceCursesSourceCurseLossesSourceCursesAccess._sourceCurseLosses)
            if (!_coreCardMovementJournalCardMovementsAccess._cardMovements.Any(m => m.Sequence == loss.MovementSequence && m.TurnNumber == loss.ActualTurn && m.CardId == loss.Deposit.CardId && m.From == loss.Deposit.Location && m.To == loss.Destination && m.Reason.Value == loss.Reason))
                throw new InvalidOperationException("A curse loss must be backed by its exact real movement.");
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>())
            AssertSourceCurseReceipt(f);
    }

    internal bool SourceCurseFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.SourceCurseReceipt is not { } r || !ValidSourceCurseReceipt(root))
            return false;
        if (r.Stage == SourceCurseStage.LossHpChildren)
        {
            if (r.CurrentHpTarget is not { } target)
                return false;
            if (child is DyingFrame dying)
                return dying.ParentFrameId == root.Id && dying.Continuation == DyingContinuationKind.ProgramSkill && dying.VictimSeat == target && (_coreDyingCursorRuntimeOperations.ActiveDying?.FrameId == dying.Id || (_coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(dying) || _coreRecipientCategoryMarkContinuationsOperations.IsOriginalDyingSuspendedByRecipientCategoryMark(dying)) || _coreOwnedDeathBenefitReturnsOperations.IsOriginalDyingSuspendedByOwnedDeathBenefit(dying)) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramSkillHpLostEvent>().Any(e => e.FrameId == root.Id && e.SkillId == root.SkillId && e.TargetSeat == target && e.RemainingHp == 0);
            return child is HpChangedTriggerWindowFrame hp && hp.Change.ParentFrameId == root.Id && hp.ResumeFrameId == root.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.TargetSeat == target && hp.Change.Kind == HpChangeKind.Loss;
        }

        if (root.PendingMovementContinuation is null)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.AwaitingProgramFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage != SourceCurseStage.DepositChildren || r.Deposit?.CardKind != CardKind.SilverLion || r.PaymentFrom != CardLocation.Equipment(root.OwnerSeat))
            return false;
        if (child is HpChangedTriggerWindowFrame lion)
            return lion.Change.ParentFrameId == root.Id && lion.ResumeFrameId == root.Id && lion.Continuation == PostEventContinuation.AwaitedProgramMovement && lion.Change.Kind == HpChangeKind.Recovery && lion.Change.SourceSeat == root.OwnerSeat && lion.Change.TargetSeat == root.OwnerSeat && lion.Change.Amount == 1;
        return child is RecoveryReplacementFrame replacement && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(replacement, root) && replacement.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && replacement.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && replacement.Attempt.SourceSeat == root.OwnerSeat && replacement.Attempt.TargetSeat == root.OwnerSeat && replacement.Attempt.Amount == 1 && replacement.Attempt.Completion.MoveReason?.Value == "program.source-curse.deposit";
    }

    internal ProgramSkillFrame? SourceCurseObserverRoot()
    {
        for (var i = 0; i + 1 < _resolutionStackOperations._resolutionStack.Count; i++)
        {
            if (_resolutionStackOperations._resolutionStack[i] is not ProgramSkillFrame root || !SourceCurseFirstChild(root, _resolutionStackOperations._resolutionStack[i + 1]))
                continue;
            if (_resolutionStackOperations._resolutionStack[i + 1] is DyingFrame original && i + 2 < _resolutionStackOperations._resolutionStack.Count && ((_coreDyingSuitsAndHistoricalUseObserversOperations.IsOriginalDyingSuspendedByDyingSuits(original) || _coreRecipientCategoryMarkContinuationsOperations.IsOriginalDyingSuspendedByRecipientCategoryMark(original)) || _coreOwnedDeathBenefitReturnsOperations.IsOriginalDyingSuspendedByOwnedDeathBenefit(original) && _coreOwnedDeathBenefitObserversOperations.OwnedDeathBenefitObserverRoot()is not null || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(i + 1, original) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(i + 1, original) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(i + 1, original) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(i + 1, original) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(i + 1, original) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(i + 1, original)))))
                return root;
            var exact = true;
            for (var child = i + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
            {
                if (!(_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child) || _coreProgramChildResolutionOperations.ProgramChildResolutionEdge(child) || _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child])))
                {
                    exact = false;
                    break;
                }

                if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && child + 1 < _resolutionStackOperations._resolutionStack.Count && (_coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying) || (_coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(child, dying) || (_coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(child, dying) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(child, dying)))))
                    break;
            }

            if (exact)
                return root;
        }

        return null;
    }

    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 170, group: 10)]
    internal bool IsSourceCurseProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SourceCurseObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1970)]
    internal bool HasSourceCurseDamageObserver(long windowId) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == windowId && f is DamageTriggerWindowFrame) && SourceCurseObserverRoot()is not null;
}
