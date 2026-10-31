#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Collections;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanYuWumiaoRuntime
{
    internal bool MatchesWuYixianRoot(ProgramSkillFrame f) => f.Yixian is { } r && f.SkillId == "wumiao:yixian" && f.ActivationId == "all-equipment" && f.TriggerId is null && f.WindowContext is null && f.SelectedCardIds.Count == 0 && f.SelectedTargetSeats.Count == 0 && f.InstructionIndex == 1 && r.InstructionIndex == f.InstructionIndex && r.Source == WuYixianSource(f) && r.GameplayHash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op == SkillProgramEffectOp.ResolveYixianAllEquipmentObtainAndCompensate && r.ActualTurn >= 1 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(r.TurnOwner);
    internal bool HasPaidWuYixian(ProgramSkillFrame f)
    {
        if (!MatchesWuYixianRoot(f) || f.Yixian is not { Paid: true, Branch: { } branch } r)
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<YixianPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is [var paid] && paid == new YixianPaidEvent(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, branch, r.BatchId, r.Entities.Count, r.Recipients.Count, r.SequenceBefore, r.SequenceAfter);
    }

    internal bool CanContinueIssuedWuYixian(ProgramSkillFrame f)
    {
        if (!HasPaidWuYixian(f))
            return false;
        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 240)]
    internal bool IsWuYixianMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending) => f.Yixian is { Paid: true } r && effect?.Op == SkillProgramEffectOp.ResolveYixianAllEquipmentObtainAndCompensate && pending == f.PendingMovementContinuation && pending.BeforeCount == 0 && pending.CoverageResultBind is null && (r.Stage == YixianStage.EquipmentChildren ? pending.SubjectSeat == f.OwnerSeat : r.Stage is YixianStage.DrawChildren or YixianStage.RecoveryChildren && r.Cursor < r.Recipients.Count && pending.SubjectSeat == r.Recipients[r.Cursor].Seat);
    internal bool WuYixianPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.Yixian is not { Paid: true } r || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == YixianStage.EquipmentChildren)
        {
            if (m.Sequence <= r.SequenceBefore || m.Sequence > r.SequenceAfter)
                return false;
            return r.Entities.Any(e => m.Sequence == e.MovementSequence && m.CardId == e.CardId && m.From == e.From && m.To == e.To && m.Reason.Value == WuYixianObtainReason) || m.From.Zone == CardZoneKind.WoodenOxGrain && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard && r.Entities.Any(e => e.PrintedKind == CardKind.WoodenOx && e.From.Zone == CardZoneKind.Equipment && e.From.OwnerSeat == m.From.OwnerSeat);
        }

        return r.Stage == YixianStage.DrawChildren && r.DrawOutcome is { } o && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(o.RecipientSeat) && m.Reason.Value == WuYixianDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(o.RecipientSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    internal bool WuYixianLionRemoval(ProgramSkillFrame f, int seat) => f.Yixian is { Stage: YixianStage.EquipmentChildren } r && r.Entities.Any(e => e.PrintedKind == CardKind.SilverLion && e.From == CardLocation.Equipment(seat) && e.To != e.From);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 410)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 400)]
    internal bool IsWuYixianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Yixian is not { Paid: true } r || !HasPaidWuYixian(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement)
            return r.Stage == YixianStage.RecoveryChildren && r.Cursor < r.Recipients.Count && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && recovery.Attempt.Completion.InstructionIndex == f.InstructionIndex && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.Recipients[r.Cursor].Seat && recovery.Attempt.Amount == 1 || r.Stage == YixianStage.EquipmentChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.MoveReason == new CardMoveReason(WuYixianObtainReason) && WuYixianLionRemoval(f, recovery.Attempt.TargetSeat);
        var index = _resolutionStackOperations._resolutionStack.FindIndex(s => s.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(s => s.Id == recovery.Id);
        return index >= 0 && child > index + 1 && WuYixianFirstChild(f, _resolutionStackOperations._resolutionStack[index + 1]) && WuYixianObserverSuffix(index) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    internal bool WuYixianFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!HasPaidWuYixian(f) || f.Yixian is not { } r)
            return false;
        if (r.Stage == YixianStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsWuYixianRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && (r.Stage == YixianStage.EquipmentChildren && hp.Change.SourceSeat == hp.Change.TargetSeat && WuYixianLionRemoval(f, hp.Change.TargetSeat) || r.Stage == YixianStage.RecoveryChildren && hp.Change.SourceSeat == f.OwnerSeat && hp.Change.TargetSeat == r.Recipients[r.Cursor].Seat);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => WuYixianPaidMovement(f, m));
    }

    internal bool WuYixianObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying));
    internal ProgramSkillFrame? WuYixianObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterGuanYuWumiaoOperations.WuYixianFirstChild(f, child), (index) => _characterGuanYuWumiaoOperations.WuYixianObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1180)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1290)]
    internal bool HasWuYixianDamageObserver(long id) => WuYixianObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1320)]
    internal bool HasWuYixianBeforeDamageObserver(long id) => WuYixianObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28530)]
    internal bool AdvanceWuYixianChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuYixianObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
