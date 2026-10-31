#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoYouZhiMouMobileRuntime
{
    internal bool ValidMouGyz(ProgramSkillFrame f)
    {
        if (f.MouGuoYouZhi is not { } r || r.Source != MouGyzSource(f) || r.GameplayHash != f.GameplayHash || f.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.ActorSeat) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected.Distinct().Count() != r.Selected.Count || r.Gifts.Select(g => g.CardId).Distinct().Count() != r.Gifts.Count || r.DistinctNames < 0 || r.DistinctNames > r.Gifts.Count || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiStartedEvent>().Count(e => e == new MouGuoYouZhiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActorSeat, r.ActualTurn, r.ActualPhase)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouGuoYouZhiBique)
        {
            if (f.SkillId != MouGuoYouZhiComposition.Bique || f.ActivationId != "inspect-and-give" || f.TriggerId is not null || f.SelectedTargetSeats is not [var target] || target != r.ActorSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiViewedEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == r.ActorSeat && e.OwnerSeat == f.OwnerSeat) != 1)
                return false;
        }
        else if (r.Op != SkillProgramEffectOp.ResolveMouGuoYouZhiZhongchun || f.SkillId != MouGuoYouZhiComposition.Zhongchun || f.TriggerId is not ("own-ending" or "other-ending") || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnEnding } c || c.SourceSeat != r.ActorSeat || !ExactMouGyzEndingParent(f))
            return false;
        if (r.Paid && (r.Op == SkillProgramEffectOp.ResolveMouGuoYouZhiBique ? _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiGiftAcceptedEvent>().Count(e => e.FrameId == f.Id && e.TargetSeat == r.ActorSeat && e.Count == r.Gifts.Count) != 1 : _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiObtainIssuedEvent>().Count(e => e == new MouGuoYouZhiObtainIssuedEvent(f.Id, r.ActorSeat, r.CardId)) != 1))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiMovementEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == e.CardId && m.CardKind == e.PrintedKind && m.From == e.From && m.To == e.To && m.Reason == e.Reason && m.Sequence == e.Sequence) == 1);
    }

    internal void SealMouGyz(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouGuoYouZhi!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGuoYouZhi = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouGuoYouZhiMovementEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMouGyz(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 32400)]
    internal bool MouGyzMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op is SkillProgramEffectOp.ResolveMouGuoYouZhiBique or SkillProgramEffectOp.ResolveMouGuoYouZhiZhongchun && ValidMouGyz(f) && p == f.PendingMovementContinuation && p.BeforeCount == 0 && p.CoverageResultBind is null && f.MouGuoYouZhi!.Stage is MouGuoYouZhiStage.GiftChildren or MouGuoYouZhiStage.ObtainChildren or MouGuoYouZhiStage.DiscardChildren or MouGuoYouZhiStage.DrawChildren;
    internal bool MouGyzFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGyz(f))
            return false;
        var r = f.MouGuoYouZhi!;
        if (child is CardUseFrame use)
            return MouGyzNativeRidesOn(use, f);
        if (child is CardDeclarationFrame declaration)
            return MatchesMouGyzDeclaration(f, declaration);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouGuoYouZhiStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoYouZhiMovementEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouGyzSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterGuoYouZhiMouMobileOperations.MouGyzFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || _characterGuoYouZhiMouMobileOperations.MouGyzDeclarationEdge(n) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouGyzObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterGuoYouZhiMouMobileOperations.MouGyzFirstChild(f, c), (i) => _characterGuoYouZhiMouMobileOperations.MouGyzSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 32400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 32400)]
    internal bool MouGyzDamage(long id) => MouGyzObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 32400)]
    internal bool MouGyzBeforeDamage(long id) => MouGyzObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 32400)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 32400)]
    internal bool MouGyzDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGyzObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 32400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 32400)]
    internal bool MouGyzRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child)
    {
        if (!ValidMouGyz(f))
            return false;
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, child, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var a = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var b = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == child.Id);
        return a >= 0 && b > a + 1 && MouGyzFirstChild(f, _resolutionStackOperations._resolutionStack[a + 1]) && MouGyzSuffix(a) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, _resolutionStackOperations._resolutionStack[b - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 32400)]
    internal bool KeepMouGyzPaid(ProgramSkillFrame f) => f.MouGuoYouZhi is { Paid: true } && ValidMouGyz(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 32400)]
    internal bool ResumeMouGyz(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGuoYouZhi: not null } f || f.Id != id)
            return false;
        ContinueMouGyz(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 32400)]
    internal bool ReturnMouGyzMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouGyz(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 32400)]
    internal bool AdvanceMouGyzChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGyzObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 32400)]
    internal void AssertMouGyz(ProgramSkillFrame f)
    {
        if (f.MouGuoYouZhi is not null && (!ValidMouGyz(f) || f.MouGuoYouZhi.Native is { } r && !ValidMouGyzNative(r, f, r.UseId > 0)))
            throw new InvalidOperationException("Guo You Zhi lost its real gift, original logical discard or exact native return.");
    }
}
