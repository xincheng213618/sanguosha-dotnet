#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuXunWumiaoRuntime
{
    internal bool WuLuXunZhangcaiRootMatches(ProgramSkillFrame f)
    {
        if (f.WuLuXunZhangcai is not { } r || f.SkillId != "wumiao:zhangcai" || f.ActivationId != f.TriggerId || f.TriggerId != r.Qualification.Source.BindingId || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Qualification.Source != WuLuXunActionSource(f) || r.Qualification.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveWuLuXunZhangcai || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || context.Window != r.Qualification.Window || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame window || window.ParentFrameId != r.NativeParentId || !WuLuXunZhangcaiWindowMatches(window, r.Qualification) || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, window))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var q = r.Qualification;
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == r.WindowId && q.RemainingEights >= 0 && q.DrawCount == Math.Max(1, q.RemainingEights) && q.MovementSequence >= 0 && q.MovementSequence <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && (q.EffectiveRank == 8 || q.RuxianFrameId is { } active && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunRuxianActivatedEvent>().Any(e => e.FrameId == active && e.Source.OwnerSeat == f.OwnerSeat && e.ActualTurn <= q.ActualTurn) && !_coreProgramEventHistoryOperations.ProgramEventHistory<TurnStartedEvent>().Any(e => e.ActorSeat == f.OwnerSeat && e.TurnNumber <= q.ActualTurn && e.TurnNumber > _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunRuxianActivatedEvent>().Single(a => a.FrameId == active).ActualTurn)) && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiQualifiedEvent>().Count(e => e.Qualification == q) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiStartedEvent>().Count(e => e == WuLuXunZhangcaiStart(f.Id, r)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuXunZhangcaiStartedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Source.SkillInstanceId == f.SkillInstanceId && e.ActionId == q.ActionId) == 1;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29002)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29002)]
    internal bool PermitPaidWuLuXunZhangcai(ProgramSkillFrame f)
    {
        if (f.WuLuXunZhangcai is null)
            return false;
        return true;
    }
    internal bool WuLuXunZhangcaiDrawMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.WuLuXunZhangcai is not { Stage: WuLuXunZhangcaiStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= o.SequenceBefore || m.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == WuLuXunZhangcaiDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool WuLuXunZhangcaiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuLuXunZhangcai is not { } r || !WuLuXunZhangcaiRootMatches(f))
            return false;
        if (r.Stage == WuLuXunZhangcaiStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => WuLuXunZhangcaiDrawMovement(f, m));
    }

    internal bool WuLuXunZhangcaiObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (this.WuLuXunZhangcaiFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || p.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? WuLuXunZhangcaiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.WuLuXunZhangcaiFirstChild(f, child), (index) => this.WuLuXunZhangcaiObserverSuffix(index));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 460)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 450)]
    internal bool IsWuLuXunZhangcaiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.WuLuXunZhangcai is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && WuLuXunZhangcaiFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && WuLuXunZhangcaiObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29002)]
    internal bool AdvanceWuLuXunZhangcaiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuLuXunZhangcaiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
