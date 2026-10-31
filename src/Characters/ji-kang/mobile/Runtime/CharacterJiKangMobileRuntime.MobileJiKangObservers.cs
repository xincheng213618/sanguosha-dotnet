#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJiKangMobileRuntime
{

    internal bool MobileJiKangPaidInstruction(ProgramSkillFrame f)
    {
        if (f.MobileJiKang is not { } r)
            return false;
        if (f.SkillId == "mobile:juexiang")
            return f.WindowContext?.Window == SkillProgramTriggerWindow.OwnerDied;
        return r.Targets.Count > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileJiKangProgressEvent>().Any(e => e.FrameId == f.Id && e.Step == "discard" && e.Subject == f.OwnerSeat && e.Value == r.Targets.Count && e.After > e.Before);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 80900)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 80900)]
    internal bool KeepMobileJiKangIssued(ProgramSkillFrame f) => MobileJiKangPaidInstruction(f);
    internal bool MobileJiKangObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileJiKang is not { } r)
            return false;
        if (r.Stage is MobileJiKangStage.Health or MobileJiKangStage.KillerHealth)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying)
            return r.Stage is MobileJiKangStage.HealthChildren or MobileJiKangStage.KillerHealthChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Subject && dying.ResumesProgramSkill && dying.KillerSeat is null;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && (r.Stage is MobileJiKangStage.HealthChildren or MobileJiKangStage.KillerHealthChildren && hp.Change.TargetSeat == r.Subject || r.Stage is MobileJiKangStage.CostChildren or MobileJiKangStage.KillerDiscardChildren or MobileJiKangStage.ClubChildren && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery)
            return _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.Program && (r.Stage == MobileJiKangStage.HealthChildren && r.Comparison < 0 && recovery.Attempt.SourceSeat == f.OwnerSeat && recovery.Attempt.TargetSeat == r.Subject && recovery.Attempt.Amount == 1 || r.Stage is MobileJiKangStage.CostChildren or MobileJiKangStage.KillerDiscardChildren or MobileJiKangStage.ClubChildren && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileJiKangDiscard);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage is MobileJiKangStage.CostChildren or MobileJiKangStage.KillerDiscardChildren or MobileJiKangStage.ClubChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileJiKangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MobileJiKangObserverChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 80900)]
    internal bool MobileJiKangDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileJiKangObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 80900)]
    internal bool AdvanceMobileJiKangHealth() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileJiKangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
