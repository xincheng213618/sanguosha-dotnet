#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuKangWumiaoRuntime
{
    internal void RequireWuLuKangDixian(ProgramSkillFrame f)
    {
        if (f.WuLuKangDixian is not { } r || !HasWuLuKangDixianOriginalPayment(f) || r.Source != WuLuKangDixianSource(f) || r.GameplayHash != f.GameplayHash || r.Phase.ActorSeat != f.OwnerSeat || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || r.SequenceBefore < 0 || r.SequenceBefore > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || !Enum.IsDefined(r.Stage) || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianStartedEvent>().Count(e => e == new WuLuKangDixianStartedEvent(f.Id, r.Source, r.GameplayHash, r.Phase, r.SequenceBefore)) != 1)
            throw new InvalidOperationException("Dixian lost its exact limited payment, source or original actual Play.");
        var chosen = _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianRankChosenEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == WuLuKangDixianStage.Choosing)
        {
            if (r.Rank != 0 || chosen.Length != 0 || r.Materials.Count != 0 || r.DrawOutcome is not null || r.MaterialIndex != 0 || r.ActualGained != 0)
                throw new InvalidOperationException("An unaccepted Dixian rank cannot inspect or change its future stock predicate.");
            return;
        }

        if (r.Rank is < 1 or > 13 || chosen is not [var c] || c.Rank != r.Rank || c.AllAtLeast != r.AllAtLeast || c.StockCount != r.StockCount || c.MaterialCount != r.Materials.Count || r.StockCount < 0 || r.MaterialIndex < 0 || r.MaterialIndex > r.Materials.Count || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || r.Materials.Any(m => m.PrintedRank != 13 || _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Rank != 13 || _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind != m.PrintedKind || m.From.Zone is not (CardZoneKind.Equipment or CardZoneKind.Judgment or CardZoneKind.DrawPile or CardZoneKind.DiscardPile)) || !_coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianMaterialFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Material).SequenceEqual(r.Materials))
            throw new InvalidOperationException("Dixian changed its frozen printed-rank decision or genuine field/stock K roster.");
        var moved = _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianEntityMovedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        var skipped = _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianEntitySkippedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (moved.Length + skipped.Length != r.MaterialIndex || moved.Select(e => e.Index).Concat(skipped.Select(e => e.Index)).Order().Where((value, i) => value != i).Any() || moved.Any(e => e.Index >= r.Materials.Count || r.Materials[e.Index].CardId != e.CardId || r.Materials[e.Index].From != e.From || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.Sequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == WuLuKangDixianObtainReason)) || r.ActualGained != moved.Count(e => e.To == CardLocation.Hand(f.OwnerSeat)))
            throw new InvalidOperationException("Dixian's acquired entities must match their real, once-only native destinations.");
        if (r.AllAtLeast && (r.Materials.Count != 0 || r.Stage is not (WuLuKangDixianStage.Drawing or WuLuKangDixianStage.DrawChildren)) || !r.AllAtLeast && (r.DrawOutcome is not null || r.Stage is not (WuLuKangDixianStage.Collecting or WuLuKangDixianStage.MovementChildren)))
            throw new InvalidOperationException("Dixian cannot switch the accepted success/failure branch.");
        if (r.DrawOutcome is { } o && (o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.Rank || f.NativeDraw is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianDrawReturnedEvent>().Count(e => e == new WuLuKangDixianDrawReturnedEvent(f.Id, r.Rank, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId)) != 1) || r.Stage == WuLuKangDixianStage.DrawChildren && r.DrawOutcome is null)
            throw new InvalidOperationException("Dixian must retain its exact requested native draw and actual replacement result.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29701)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29701)]
    internal bool ContinueAcceptedWuLuKangDixian(ProgramSkillFrame f)
    {
        if (f.WuLuKangDixian is not { Rank: > 0 })
            return false;
        RequireWuLuKangDixian(f);
        return true;
    }
    internal bool WuLuKangDixianMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.WuLuKangDixian is not { } r || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == WuLuKangDixianStage.DrawChildren && r.DrawOutcome is { } o)
            return m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == WuLuKangDixianDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
        if (r.Stage != WuLuKangDixianStage.MovementChildren)
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.Sequence == m.Sequence && e.CardId == m.CardId && e.From == m.From && e.To == m.To))
            return true;
        // An actual Wooden Ox departure may spill its real grain in this same
        // batch. It is a child movement, never another frozen K acquisition.
        return m.From.Zone == CardZoneKind.WoodenOxGrain && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && _coreProgramEventHistoryOperations.ProgramEventHistory<WuLuKangDixianEntityMovedEvent>().Any(e => e.FrameId == f.Id && e.From == CardLocation.Equipment(m.From.OwnerSeat!.Value) && _coreAdvancedLifecycleOperations.GetAdvancedCard(e.CardId).Kind == CardKind.WoodenOx && e.Sequence < m.Sequence);
    }

    internal bool WuLuKangDixianFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.WuLuKangDixian is not { Rank: > 0 } r || !HasWuLuKangDixianOriginalPayment(f))
            return false;
        RequireWuLuKangDixian(f);
        if (r.Stage == WuLuKangDixianStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => WuLuKangDixianMovementMatches(f, m));
    }

    internal bool WuLuKangDixianObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (this.WuLuKangDixianFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || p.Zenrun is not null && _characterSunHongOlOperations.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? WuLuKangDixianObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.WuLuKangDixianFirstChild(f, child), (i) => this.WuLuKangDixianObserverSuffix(i));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 500)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 490)]
    internal bool IsWuLuKangDixianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.WuLuKangDixian is null || recovery.ParentFrameId == f.Id)
            return false;
        var p = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return p >= 0 && c > p + 1 && WuLuKangDixianFirstChild(f, _resolutionStackOperations._resolutionStack[p + 1]) && WuLuKangDixianObserverSuffix(p) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29701)]
    internal bool AdvanceWuLuKangDixianChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuLuKangDixianObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
