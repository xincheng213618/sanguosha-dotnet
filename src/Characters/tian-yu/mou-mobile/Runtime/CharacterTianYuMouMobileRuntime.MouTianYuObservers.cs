#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianYuMouMobileRuntime
{
    internal bool ValidMouTianYu(ProgramSkillFrame f) => f.MouTianYu is { } r && f.InstructionIndex == 1 && r.Source == MouTianYuSource(f) && r.GameplayHash == f.GameplayHash && f.WindowContext?.ParentFrameId == r.WindowId && _coreProgramEventHistoryOperations.ProgramEventHistory<MouTianYuStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.WindowId == r.WindowId && e.UseId == r.UseId && e.ActionId == r.ActionId && e.TargetSeat == r.TargetSeat && e.TurnOrdinal == r.TurnOrdinal) == 1;
    internal bool MouTianYuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MouTianYu is not { Stage: MouTianYuStage.Children } r || !ValidMouTianYu(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MouTianYuMovementSignedEvent>().Count(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Reason == m.Reason && e.Sequence == m.Sequence) == 1) || child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouTianYuObserverEdge(int index) => _coreObserverSubtreesOperations.IsMovementObserverEdge(index) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(index) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(index) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[index - 1], _resolutionStackOperations._resolutionStack[index]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[index], _resolutionStackOperations._resolutionStack[index - 1]);
    internal bool MouTianYuObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _characterTianYuMouMobileOperations.MouTianYuObserverEdge(n), (n, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouTianYuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterTianYuMouMobileOperations.MouTianYuFirstChild(f, c), (n) => _characterTianYuMouMobileOperations.MouTianYuObserverSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30600)]
    internal bool IsMouTianYuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouTianYuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30600)]
    internal bool IsMouTianYuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var root = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        if (ValidMouTianYu(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return true;
        return root >= 0 && child > root + 1 && MouTianYuFirstChild(f, _resolutionStackOperations._resolutionStack[root + 1]) && MouTianYuObserverSuffix(root) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30600)]
    internal bool AdvanceMouTianYuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouTianYuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
