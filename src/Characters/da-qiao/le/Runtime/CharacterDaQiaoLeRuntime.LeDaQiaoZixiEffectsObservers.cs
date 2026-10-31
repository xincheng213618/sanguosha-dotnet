#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDaQiaoLeRuntime
{
    internal bool ZixiEffectsFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZixiEffects is not { } r || !ValidZixiEffectsStarted(f))
            return false;
        if (r.Stage == ZixiEffectsStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsZixiEffectsRecoveryParent(f, recovery);
        if (child is DamageFrame damage)
            return r.Stage == ZixiEffectsStage.DamageChildren && f.AttackAttempt is { } attack && damage.ParentFrameId == f.Id && damage.SourceSeat == f.OwnerSeat && damage.TargetSeat == attack.TargetSeat && damage.Amount == attack.DamageAmount;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == ZixiEffectsStage.DamageChildren && f.AttackAttempt is { } attempt && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && before.SourceSeat == f.OwnerSeat && before.TargetSeat == attempt.TargetSeat;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == ZixiEffectsStage.ObtainChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.SourceSeat == r.TargetSeat && hp.Change.TargetSeat == r.TargetSeat && hp.Change.Amount == 1 && r.Entities.Any(e => e.PrintedKind == CardKind.SilverLion && e.From == CardLocation.Equipment(r.TargetSeat));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is not null && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && (moved.Batch.Movements.All(m => ZixiEffectsPrimaryMovement(f, m)) || ZixiEffectsOxSpill(f, moved.Batch));
    }

    internal bool ZixiEffectsLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame window && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) && f.WindowContext is { } context && context.ParentFrameId == window.Id && context.Window == window.Window;
    internal ProgramSkillFrame? ZixiEffectsObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.ZixiEffectsFirstChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (next) => _coreObserverSubtreesOperations.IsMovementObserverEdge(next) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(next) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[next - 1], _resolutionStackOperations2._resolutionStack()[next]) || this.ZixiEffectsLifecycleEdge(next) || _resolutionStackOperations2._resolutionStack()[next - 1] is ProgramSkillFrame { Zenrun: not null } zenrun && _characterSunHongOlOperations.ZenrunFirstChild(zenrun, _resolutionStackOperations2._resolutionStack()[next]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[next], _resolutionStackOperations2._resolutionStack()[next - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(next), (next, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(next, dying)));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28720)]
    internal bool AdvanceZixiEffectsChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZixiEffectsObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
