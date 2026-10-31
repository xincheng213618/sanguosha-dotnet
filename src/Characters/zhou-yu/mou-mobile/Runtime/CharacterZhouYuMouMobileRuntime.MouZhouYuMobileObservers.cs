#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouMobileRuntime
{
    internal bool ValidMouZyMobile(ProgramSkillFrame f)
    {
        if (f.MouZhouYuMobile is not { } r || r.Source != MouZyMobileSource(f) || r.GameplayHash != f.GameplayHash || f.SkillId != MouZhouYuMobileComposition.Fanjian || f.ActivationId != "cover-and-declare" || f.TriggerId is not null || f.WindowContext is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || !f.SelectedTargetSeats.SequenceEqual([r.TargetSeat]) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TargetSeat) || r.TargetSeat == f.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Phase.ActorSeat != f.OwnerSeat || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || r.Turn.TurnNumber != r.Phase.TurnNumber || r.Turn.OwnerSeat != r.Phase.ActualTurnOwnerSeat || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileStartedEvent>().Count(e => e == new MouZhouYuMobileStartedEvent(f.Id, r.Source, r.GameplayHash, r.Turn, r.Phase, r.TargetSeat)) != 1)
            return false;
        var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileCoveredEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.PaymentSequence == 0)
            return r.Stage == MouZhouYuMobileStage.OwnerChoice && r.CardId == 0 && paid.Length == 0 && r.Answer is null && !r.Revealed && r.GiftSequence == 0 && !r.LossPaid;
        if (paid is not [var payment] || payment != new MouZhouYuMobileCoveredEvent(f.Id, r.CardId, r.PrintedKind, r.ActualSuit, r.DeclaredSuit, CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, r.PaymentSequence) || r.DeclaredSuit is not (Suit.Spade or Suit.Heart or Suit.Club or Suit.Diamond) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == r.PaymentSequence && m.CardId == r.CardId && m.CardKind == r.PrintedKind && m.From == payment.From && m.To == payment.To && m.Reason.Value == "skill-program.mou-mobile:fanjian.cover"))
            return false;
        var answers = _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileAnsweredEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Answer is null)
            return answers.Length == 0 && r.GiftSequence == 0 && (r.Stage is MouZhouYuMobileStage.CoverChildren or MouZhouYuMobileStage.TargetChoice && !r.Revealed || r.Stage == MouZhouYuMobileStage.CleanupChildren && r.Revealed);
        if (r.Answer is not ("same" or "different" or "turn-over") || answers is not [var answer] || answer != new MouZhouYuMobileAnsweredEvent(f.Id, r.TargetSeat, r.Answer))
            return false;
        if (r.GiftSequence > 0 && (!r.Revealed || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileGiftedEvent>().Count(e => e.FrameId == f.Id && e.CardId == r.CardId && e.TargetSeat == r.TargetSeat && e.Sequence == r.GiftSequence && e.From == CardLocation.Processing && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == r.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == "skill-program.mou-mobile:fanjian.gift")) != 1))
            return false;
        if (r.LossPaid && ((long)r.LossHpBefore - r.LossHpAfter != 1 || r.Stage != MouZhouYuMobileStage.LossChildren || _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileLossPaidEvent>().Count(e => e == new MouZhouYuMobileLossPaidEvent(f.Id, r.TargetSeat, r.LossHpBefore, r.LossHpAfter)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillHpLostEvent>().Count(e => e.FrameId == f.Id && e.SkillId == f.SkillId && e.TargetSeat == r.TargetSeat && e.Amount == 1 && e.RemainingHp == r.LossHpAfter) != 1))
            return false;
        return r.Stage is MouZhouYuMobileStage.ChoiceChildren or MouZhouYuMobileStage.GiftChildren or MouZhouYuMobileStage.LossChildren or MouZhouYuMobileStage.CleanupChildren;
    }

    internal void SealMouZyMobile(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouZhouYuMobile = f.MouZhouYuMobile!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
        {
            var fact = new MouZhouYuMobileMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMouZyMobile(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 31100)]
    internal bool IsMouZyMobileMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveMouZhouYuFanjian && ValidMouZyMobile(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && f.MouZhouYuMobile!.Stage is not (MouZhouYuMobileStage.OwnerChoice or MouZhouYuMobileStage.TargetChoice);
    internal bool MouZyMobileFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouZyMobile(f))
            return false;
        var r = f.MouZhouYuMobile!;
        if (child is DyingFrame d)
            return d.ParentFrameId == f.Id && d.ResumesProgramSkill && d.VictimSeat == r.TargetSeat && r.Stage == MouZhouYuMobileStage.LossChildren && r.LossPaid;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.TargetSeat == r.TargetSeat && hp.Change.Kind == HpChangeKind.Loss && r.LossPaid && hp.Change.Amount == 1 && hp.Change.HpBefore == r.LossHpBefore && hp.Change.HpAfter == r.LossHpAfter;
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => _coreProgramEventHistoryOperations.ProgramEventHistory<MouZhouYuMobileMovementSignedEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouZyMobileObserverSuffix(int index) => _resolutionStackOperations._resolutionStack[index + 1] is DyingFrame d && index + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(index + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterZhouYuMouMobileOperations.MouZyMobileFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouZyMobileObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhouYuMouMobileOperations.MouZyMobileFirstChild(f, child), (i) => _characterZhouYuMouMobileOperations.MouZyMobileObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 31100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 31100)]
    internal bool HasMouZyMobileDamageObserver(long id) => MouZyMobileObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 31100)]
    internal bool HasMouZyMobileBeforeDamageObserver(long id) => MouZyMobileObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 31100)]
    internal bool IsMouZyMobileDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.Id == d.ParentFrameId && MouZyMobileFirstChild(f, d)) || MouZyMobileObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 31100)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 31100)]
    internal bool IsMouZyMobileRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouZyMobile(f))
            return false;
        if (recovery.ParentFrameId == f.Id)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i + 1 && MouZyMobileFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouZyMobileObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 31100)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 31100)]
    internal bool KeepMouZyMobilePaid(ProgramSkillFrame f) => f.MouZhouYuMobile is { PaymentSequence: > 0 } && ValidMouZyMobile(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 31100)]
    internal bool ResumeMouZyMobile(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouZhouYuMobile: not null } f || f.Id != id)
            return false;
        ContinueMouZyMobile(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 31100)]
    internal bool ReturnMouZyMobileMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouZyMobile(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 31100)]
    internal bool AdvanceMouZyMobileChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouZyMobileObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
