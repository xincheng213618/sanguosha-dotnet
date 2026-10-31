#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuXieMouRuntime
{
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 34800)]
    internal bool MouLiuXieMovementReturn(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => effect?.Op == SkillProgramEffectOp.ResolveMouLiuXieZhanban && MouLiuXie.Valid(f) && f.MouLiuXie is { Stage: MouLiuXieStage.DiscardChildren or MouLiuXieStage.DrawChildren } && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouLiuXieFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!MouLiuXie.Valid(f) || f.MouLiuXie is not { } r)
            return false;
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage == MouLiuXieStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved && f.PendingMovementContinuation is { } pending && MouLiuXieMovementReturn(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (r.Stage == MouLiuXieStage.Damage && f.AttackAttempt is { } attack && f.AttackReturn is not null && attack.SourceSeat == f.OwnerSeat && attack.TargetSeat == r.Payer && !attack.SourceLess && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLiuXieDamageIssuedEvent>().Count(e => e.FrameId == f.Id && e.SourceSeat == f.OwnerSeat && e.TargetSeat == r.Payer) == 1)
            return _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id));
    }

    internal bool MouLiuXieNativeEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && MouLiuXieFirstNativeChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal bool MouLiuXieNativeSuffix(int n) => _resolutionStackOperations._resolutionStack[n + 1] is DyingFrame d && n + 2 < _resolutionStackOperations._resolutionStack.Count && _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n + 1, d) || _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _characterLiuXieMouOperations.MouLiuXieNativeEdge(i), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? MouLiuXieNativeRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLiuXieMouOperations.MouLiuXieFirstNativeChild(f, c), (n) => _characterLiuXieMouOperations.MouLiuXieNativeSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 34800)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 34800)]
    internal bool MouLiuXieDamageWindow(long id) => MouLiuXieNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 34800)]
    internal bool MouLiuXieBeforeDamage(long id) => MouLiuXieNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 34800)]
    internal bool MouLiuXieDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MouLiuXieNativeRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 34800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 34800)]
    internal bool MouLiuXieRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (MouLiuXieFirstNativeChild(f, recovery))
            return true;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouLiuXieFirstNativeChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouLiuXieNativeSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 34800)]
    internal bool MouLiuXieAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouLiuXieNativeRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
