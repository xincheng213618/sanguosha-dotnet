#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuYongOlRuntime
{

    internal bool CanContinueIssuedOrdinaryGuYong(ProgramSkillFrame f)
    {
        if (f.OrdinaryBingyi is not { Paid: true })
            return false;
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27598)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27598)]
    internal bool PermitPaidOrdinaryBingyi(ProgramSkillFrame f) => CanContinueIssuedOrdinaryGuYong(f);
    internal bool IsOrdinaryBingyiDrawMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.OrdinaryBingyi is not { Paid: true, Stage: OrdinaryBingyiStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= o.SequenceBefore || m.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(o.RecipientSeat) && m.Reason.Value == OrdinaryBingyiDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(o.RecipientSeat);
    }

    internal bool OrdinaryBingyiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.OrdinaryBingyi is not { Paid: true } r)
            return false;
        if (r.Stage == OrdinaryBingyiStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsOrdinaryBingyiDrawMovement(f, m));
    }

    internal bool OrdinaryBingyiObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? OrdinaryBingyiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.OrdinaryBingyiFirstChild(f, child), (i) => this.OrdinaryBingyiObserverSuffix(i));
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 310)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 300)]
    internal bool IsOrdinaryGuYongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.OrdinaryBingyi is not { Paid: true } || recovery.ParentFrameId == f.Id)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id);
        return i >= 0 && j > i + 1 && OrdinaryBingyiFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && OrdinaryBingyiObserverSuffix(i) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27598)]
    internal bool AdvanceOrdinaryBingyiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && OrdinaryBingyiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
