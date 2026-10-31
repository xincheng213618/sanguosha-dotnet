#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuOlMagicRuntime
{
    internal bool ValidMoLuBu(ProgramSkillFrame f) => f.MoLuBu is { } r && f.InstructionIndex == 1 && r.Source.SkillId == f.SkillId && r.GameplayHash == f.GameplayHash && (r.Operation == SkillProgramEffectOp.ResolveMoLuBuKuangmoDeath ? _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuKuangmoEnteredEvent>().Any(e => e.Source == r.Source && e.GameplayHash == r.GameplayHash) : r.Source == MoLuBuSource(f)) && _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Operation == r.Operation && e.WindowId == r.WindowId && e.OriginalOwnerSeat == r.OriginalOwnerSeat && e.ActualTurn == r.ActualTurn && e.Reassign == r.Reassign) == 1;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 30800)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 30800)]
    internal bool ContinueIssuedMoLuBu(ProgramSkillFrame f) => f.MoLuBu is not null && ValidMoLuBu(f) || f.WindowContext is { } c && IsIssuedMoLuBuDeathCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId ?? "", f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    internal bool MoLuBuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MoLuBu is not { } r || !ValidMoLuBu(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f))
            return true;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp);
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<MoLuBuMovementSignedEvent>().Count(e => e.FrameId == f.Id && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Reason == m.Reason && e.Sequence == m.Sequence) == 1);
    }

    internal bool MoLuBuObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _resolutionStackOperations2._resolutionStack()[i - 1] is ProgramSkillFrame f && _characterLuBuOlMagicOperations.MoLuBuFirstChild(f, _resolutionStackOperations2._resolutionStack()[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MoLuBuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLuBuOlMagicOperations.MoLuBuFirstChild(f, c), (i) => _characterLuBuOlMagicOperations.MoLuBuObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30800)]
    internal bool MoLuBuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MoLuBuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 30800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 30800)]
    internal bool MoLuBuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (!ValidMoLuBu(f))
            return false;
        if (_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MoLuBuFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MoLuBuObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 30800)]
    internal bool AdvanceMoLuBuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MoLuBuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
