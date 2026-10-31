#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangfuSongWumiaoRuntime
{
    internal bool ValidWuHf(ProgramSkillFrame f) => f.WuHuangFuSong is { } r && r.Source == WuHfSource(f) && r.GameplayHash == f.GameplayHash && f.InstructionIndex == 1 && r.WindowId == f.WindowContext?.ParentFrameId && WuHuangFuSongComposition.Handles(r.Operation) && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == r.Operation && r.Before >= 0 && r.After >= r.Before && r.After <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && _coreProgramEventHistoryOperations.ProgramEventHistory<WuHuangFuSongStartedEvent>().Count(e => e == new WuHuangFuSongStartedEvent(f.Id, r.Source, r.GameplayHash, r.Operation, r.Turn, r.WindowId, r.UseId)) == 1 && (!r.Paid || r.Operation == SkillProgramEffectOp.SettleWuHuangFuSongJiangxian || _coreProgramEventHistoryOperations.ProgramEventHistory<WuHuangFuSongMovedEvent>().Count(e => e.FrameId == f.Id && e.CardId == r.CardId && e.From == r.From && e.To == r.To && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To)) == 1);
    internal void SealWuHf(long id, long before)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        var r = f.WuHuangFuSong!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WuHuangFuSong = r with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        foreach (var m in _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > before && m.Sequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
        {
            var fact = new WuHuangFuSongMovementSignedEvent(id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence);
            if (!_coreProgramEventHistoryOperations.ProgramEventHistory<WuHuangFuSongMovementSignedEvent>().Contains(fact))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(fact);
        }
    }

    internal bool DrainWuHf(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 381)]
    internal bool IsWuHfMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect is not null && WuHuangFuSongComposition.Handles(effect.Op) && ValidWuHf(f) && pending == f.PendingMovementContinuation && pending.SubjectSeat == f.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    internal bool WuHfFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidWuHf(f))
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return f.WuHuangFuSong!.Stage == WuHuangFuSongStage.Drawing && f.NativeDraw is not null;
        if (child is RecoveryReplacementFrame replacement)
            return replacement.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, replacement, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && hp.Change.ParentFrameId == f.Id && _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReceiptMatches(f, hp, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.Count > 0 && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreProgramEventHistoryOperations.ProgramEventHistory<WuHuangFuSongMovementSignedEvent>().Contains(new(f.Id, m.CardId, m.CardKind, m.From, m.To, m.Reason, m.Sequence)));
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool WuHfObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterHuangfuSongWumiaoOperations.WuHfFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? WuHfObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterHuangfuSongWumiaoOperations.WuHfFirstChild(f, c), (i) => _characterHuangfuSongWumiaoOperations.WuHfObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1361)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1471)]
    internal bool HasWuHfDamageObserver(long id) => WuHfObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1491)]
    internal bool HasWuHfBeforeDamageObserver(long id) => WuHfObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 930)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 870)]
    internal bool IsWuHfRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame r)
    {
        if (f.WuHuangFuSong is null || !ValidWuHf(f))
            return false;
        if (r.ParentFrameId == f.Id)
            return _coreProgramRecoveryReceiptsOperations.ProgramRecoveryReplacementReceiptMatches(f, r, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().ToArray());
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == r.Id);
        return i >= 0 && child > i + 1 && WuHfFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && WuHfObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(r, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30000)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30000)]
    internal bool ContinuePaidWuHf(ProgramSkillFrame f) => f.WuHuangFuSong is { Paid: true } && ValidWuHf(f) || f.WuHuangFuSong is { Operation: SkillProgramEffectOp.SettleWuHuangFuSongJiangxian } && ValidWuHf(f) || f.WuHuangFuSong is null && f.InstructionIndex is 0 or 1 && f.WindowContext is { } c && f.TriggerId == "lose-after-turn" && IsIssuedWuHfEnding(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    [ProgramContinuation(ProgramContinuationKind.Resume, 30000)]
    internal bool ResumeWuHf(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WuHuangFuSong: not null } f || f.Id != id)
            return false;
        ContinueWuHf(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 30000)]
    internal bool ReturnWuHfMovement(ProgramSkillFrame f) => f.PendingMovementContinuation is not null && ResumeWuHf(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30000)]
    internal bool AdvanceWuHfChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuHfObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
