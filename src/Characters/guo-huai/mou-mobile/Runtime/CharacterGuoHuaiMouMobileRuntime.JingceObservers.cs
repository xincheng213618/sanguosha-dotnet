#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoHuaiMouMobileRuntime
{
    internal bool ValidMouGh(ProgramSkillFrame f)
    {
        if (f.MouGuoHuai is not { } r || r.Source != MouGhSource(f) || r.GameplayHash != f.GameplayHash || f.SkillId != MouGuoHuaiMobileComposition.Jingce || f.InstructionIndex != 1 || f.ActivationId != f.TriggerId || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.WindowContext?.ParentFrameId != r.ParentId || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Index < 0 || r.Index > r.Rewards.Count || r.Cards.Count > 3 || r.Cards.Distinct().Count() != r.Cards.Count || r.Predictions.Count > r.Cards.Count || r.Predictions.Select(p => p.CardId).Distinct().Count() != r.Predictions.Count || r.Predictions.Where((p, i) => p.Ordinal != i + 1 || !r.Cards.Contains(p.CardId)).Any() || _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiStartedEvent>().Count(e => e == new MouGuoHuaiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ParentId, r.ActualTurn)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart)
        {
            if (f.TriggerId != "start-jingce" || f.WindowContext.Window != SkillProgramTriggerWindow.TurnStartBeforeNormalFlow || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.ParentId)is not ProgramLifecycleTriggerWindowFrame { Continuation: ProgramLifecycleContinuation.MouGuoHuaiActualStart } parent || parent.OwnerSeat != f.OwnerSeat || r.Rewards.Any(id => !_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiPredictionDueEvent>().Any(e => e.PredictionId == id && e.ActualTurn == r.ActualTurn && e.OwnerSeat == f.OwnerSeat && e.NoneSuccess)))
                return false;
        }
        else if (r.Op == SkillProgramEffectOp.ResolveMouGuoHuaiJingcePrediction)
        {
            if (f.TriggerId != "predict-jingce" || f.WindowContext.Window != SkillProgramTriggerWindow.AfterTurnEnded || f.WindowContext.SourceSeat != f.OwnerSeat)
                return false;
        }
        else if (r.Op != SkillProgramEffectOp.ResolveMouGuoHuaiJingceReward || f.TriggerId != "verify-first-gain" || f.WindowContext.Window != SkillProgramTriggerWindow.CardsGained || r.Rewards is not [var id] || !_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiGainQualifiedEvent>().Any(e => e.PredictionId == id && e.BatchId == r.ParentId && e.Source == r.Source && e.GameplayHash == r.GameplayHash))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiMovementEvent>().Where(e => e.FrameId == f.Id).All(e => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == e.From && m.To == e.To && m.Reason == e.Reason) == 1) && (r.Stage is not (MouGuoHuaiStage.Drawing or MouGuoHuaiStage.DrawChildren) || r.PredictionId > 0 && r.Requested is >= 1 and <= 3 && r.Index < r.Rewards.Count && r.Rewards[r.Index] == r.PredictionId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiRewardIssuedEvent>().Count(e => e == new MouGuoHuaiRewardIssuedEvent(f.Id, r.PredictionId, r.Requested, f.OwnerSeat)) == 1);
    }

    internal void SealMouGh(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.MouGuoHuai!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouGuoHuai = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence).ToArray())
        {
            var fact = new MouGuoHuaiMovementEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiMovementEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainMouGh(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 32000)]
    internal bool IsMouGhMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op is SkillProgramEffectOp.ResolveMouGuoHuaiJingceStart or SkillProgramEffectOp.ResolveMouGuoHuaiJingcePrediction or SkillProgramEffectOp.ResolveMouGuoHuaiJingceReward && ValidMouGh(f) && f.MouGuoHuai!.Stage is MouGuoHuaiStage.StoreChildren or MouGuoHuaiStage.PlacingChildren or MouGuoHuaiStage.DrawChildren && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouGhFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidMouGh(f))
            return false;
        var r = f.MouGuoHuai!;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouGuoHuaiStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame recovery)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouGuoHuaiMovementEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouGhSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterGuoHuaiMouMobileOperations.MouGhFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? MouGhObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterGuoHuaiMouMobileOperations.MouGhFirstChild(f, child), (i) => _characterGuoHuaiMouMobileOperations.MouGhSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 32000)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 32000)]
    internal bool HasMouGhDamage(long id) => MouGhObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 32000)]
    internal bool HasMouGhBeforeDamage(long id) => MouGhObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 32000)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 32000)]
    internal bool IsMouGhDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGhObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 32000)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 32000)]
    internal bool MouGhRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMouGh(f))
            return false;
        if (_coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, recovery, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray()))
            return true;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && c > i + 1 && MouGhFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && MouGhSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 32000)]
    internal bool KeepMouGhPaid(ProgramSkillFrame f) => f.MouGuoHuai is { Paid: true } && ValidMouGh(f);
    [ProgramContinuation(ProgramContinuationKind.Resume, 32000)]
    internal bool ResumeMouGh(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouGuoHuai: not null } f || f.Id != id)
            return false;
        ContinueMouGh(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 32000)]
    internal bool ReturnMouGhMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeMouGh(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 32000)]
    internal bool AdvanceMouGhChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGhObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 32000)]
    internal void AssertMouGh(ProgramSkillFrame f)
    {
        if (f.MouGuoHuai is not null && !ValidMouGh(f))
            throw new InvalidOperationException("Jingce lost its actual prediction, original source or signed native return.");
    }

    internal IReadOnlyList<PlayerMarkerSnapshot>? AppendMouGuoHuaiJingce(int seat, IReadOnlyList<PlayerMarkerSnapshot>? ordinary)
    {
        var count = _corePublicPileSourcesOperations.PublicPileSources(seat).Where(s => s.SkillId == MouGuoHuaiMobileComposition.Jingce).Sum(s => _corePublicPileSourcesOperations.PublicPileCards(s).Count);
        return count > 0 ? Array.AsReadOnly((ordinary ?? []).Where(m => m.Kind != PlayerMarkerKind.MouGuoHuaiJingce).Append(new PlayerMarkerSnapshot(PlayerMarkerKind.MouGuoHuaiJingce, "精策", count)).OrderBy(m => m.Kind).ToArray()) : ordinary;
    }
}
