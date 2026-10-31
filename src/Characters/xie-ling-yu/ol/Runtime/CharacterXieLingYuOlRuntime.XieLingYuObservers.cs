#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXieLingYuOlRuntime
{
    internal bool CanContinueIssuedXieLingYu(ProgramSkillFrame f)
    {
        if (f.XieLingYu is { Paid: true })
        {
            return true;
        }

        if (f.InstructionIndex is not (0 or 1) || f.TriggerId is null || f.WindowContext is not { } c)
            return false;
        return IsIssuedYuandiCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27581)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27581)]
    internal bool PermitIssuedXieLingYu(ProgramSkillFrame f) => CanContinueIssuedXieLingYu(f);
    internal bool IsXieLingYuPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.XieLingYu!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == XieLingYuStage.CostChildren)
            return m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == XinyouDiscardReason && r.Materials.Any(c => c.CardId == m.CardId && c.PrintedKind == m.CardKind && c.From == m.From) && _coreProgramEventHistoryOperations.ProgramEventHistory<XinyouMaterialDiscardedEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.MovementSequence == m.Sequence);
        if (r.Stage != XieLingYuStage.DrawChildren || r.Outcome is not { } o)
            return false;
        return r.Gains.Any(c => c.CardId == m.CardId && c.PrintedKind == m.CardKind && c.MovementSequence == m.Sequence && m.To == CardLocation.Hand(f.OwnerSeat)) || m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool XieLingYuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.XieLingYu is not { Paid: true } r)
            return false;
        if (r.Stage == XieLingYuStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsXieLingYuPaidMovement(f, m));
    }

    internal ProgramSkillFrame? XieLingYuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.XieLingYuFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 280)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 270)]
    internal bool IsXieLingYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.XieLingYu is not { Paid: true })
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var recoveryIndex = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return rootIndex >= 0 && recoveryIndex > rootIndex + 1 && XieLingYuFirstChild(f, _resolutionStackOperations._resolutionStack[rootIndex + 1]) && _coreObserverSubtreesOperations.HasObserverSuffix(rootIndex, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[recoveryIndex - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27581)]
    internal bool AdvanceXieLingYuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && XieLingYuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
