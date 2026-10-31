#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunShangxiangSpOlRuntime
{
    internal bool ValidSpSunShangxiangLiangzhu(ProgramSkillFrame f)
    {
        if (f.SpSunShangxiangLiangzhu is not { } r || r.Source != SpSunShangxiangSource(f) || r.GameplayHash != f.GameplayHash || f.SkillId != SpSunShangxiangOlComposition.Liangzhu || f.TriggerId != "actual-own-play-recovery" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || f.WindowContext is not { } w || w.ParentFrameId != r.ChangeId || w.TargetSeat != r.RecoveredSeat || !Enum.IsDefined(r.Stage) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<SpSunShangxiangLiangzhuStartedEvent>().Count(e => e == new SpSunShangxiangLiangzhuStartedEvent(f.Id, r.Source, r.GameplayHash, r.ChangeId, r.RecoveredSeat)) != 1 || !CanRunSpSunShangxiangLiangzhu(new(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority), w))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not HpChangedTriggerWindowFrame parent || parent.Id != r.ChangeId || parent.Change != w.HpChange || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]))
            return false;
        if (r.Stage == SpSunShangxiangLiangzhuStage.Choosing)
            return r.Branch is null && r.Requested == 0 && r.Draw is null;
        if (r.Branch is not ("self" or "help") || r.Requested != (r.Branch == "help" ? 2 : 1) || r.RecipientSeat != (r.Branch == "help" ? r.RecoveredSeat : f.OwnerSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<SpSunShangxiangLiangzhuChoiceEvent>().Count(e => e == new SpSunShangxiangLiangzhuChoiceEvent(f.Id, r.Branch, r.RecipientSeat, r.Requested)) != 1)
            return false;
        if (r.Stage == SpSunShangxiangLiangzhuStage.Drawing)
            return r.Draw is null;
        return r.Draw is { } draw && draw.RecipientSeat == r.RecipientSeat && draw.RequestedCount == r.Requested && _coreProgramEventHistoryOperations.ProgramEventHistory<SpSunShangxiangLiangzhuDrawEvent>().Count(e => e == new SpSunShangxiangLiangzhuDrawEvent(f.Id, draw.RecipientSeat, draw.RequestedCount, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.AttemptFrameId)) == 1;
    }
    internal bool IsSpSunShangxiangLiangzhuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => e?.Op == SkillProgramEffectOp.ResolveSpSunShangxiangLiangzhu && f.SpSunShangxiangLiangzhu is { Stage: SpSunShangxiangLiangzhuStage.Children, Draw: not null } && ValidSpSunShangxiangLiangzhu(f) && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool SpSunShangxiangLiangzhuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidSpSunShangxiangLiangzhu(f) || f.SpSunShangxiangLiangzhu is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == SpSunShangxiangLiangzhuStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && r.Draw is { } draw && f.PendingMovementContinuation is { } p && IsSpSunShangxiangLiangzhuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > draw.SequenceBefore && m.Sequence <= draw.SequenceAfter);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool SpSunShangxiangLiangzhuObserverEdge(int n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[n - 1], _resolutionStackOperations._resolutionStack[n]) || _resolutionStackOperations._resolutionStack[n - 1] is ProgramSkillFrame f && SpSunShangxiangLiangzhuFirstChild(f, _resolutionStackOperations._resolutionStack[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[n], _resolutionStackOperations._resolutionStack[n - 1]);
    internal bool SpSunShangxiangLiangzhuSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame dying && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, dying) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => this.SpSunShangxiangLiangzhuObserverEdge(i), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? SpSunShangxiangLiangzhuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.SpSunShangxiangLiangzhuFirstChild(f, c), (n) => this.SpSunShangxiangLiangzhuSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 32300)]
    internal bool IsSpSunShangxiangLiangzhuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && SpSunShangxiangLiangzhuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 32300)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 32300)]
    internal bool IsSpSunShangxiangLiangzhuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var c = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return n >= 0 && c > n + 1 && SpSunShangxiangLiangzhuFirstChild(f, _resolutionStackOperations._resolutionStack[n + 1]) && SpSunShangxiangLiangzhuSuffix(n) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[c - 1]);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 32300)]
    internal bool ResumeSpSunShangxiangLiangzhu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SpSunShangxiangLiangzhu: not null } f || f.Id != id)
            return false;
        ContinueSpSunShangxiangLiangzhu(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 32300)]
    internal bool ReturnSpSunShangxiangLiangzhuMovement(ProgramSkillFrame f) => f.SpSunShangxiangLiangzhu is not null && f.PendingMovementContinuation is not null && ResumeSpSunShangxiangLiangzhu(f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 32300)]
    internal bool AdvanceSpSunShangxiangLiangzhuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && SpSunShangxiangLiangzhuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 32300)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 32300)]
    internal bool ContinuePaidSpSunShangxiangLiangzhu(ProgramSkillFrame f) => f.SpSunShangxiangLiangzhu is { Branch: not null } && ValidSpSunShangxiangLiangzhu(f);
}
