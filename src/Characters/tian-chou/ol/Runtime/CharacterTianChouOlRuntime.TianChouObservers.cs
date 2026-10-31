#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterTianChouOlRuntime
{
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 820)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 760)]
    internal bool IsTianChouRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.TianChouShandao is { Stage: TianChouShandaoStage.TopChildren, Tops.Count: > 0 } r && ValidTianChou(f) && r.Tops[^1] is { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } top && recovery.ParentFrameId == f.Id && recovery.Return == new RecoveryReplacementReturn(PostEventContinuation.Program, f.Id) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == TianChouTopReason && recovery.Attempt.SourceSeat == top.OwnerSeat && recovery.Attempt.TargetSeat == top.OwnerSeat && recovery.Attempt.Amount == 1;
    internal bool TianChouFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.TianChouShandao is not { } r || !ValidTianChou(f))
            return false;
        if (child is CardUseFrame use)
            return r.Stage == TianChouShandaoStage.NativeUse && use.TianChouShandao is { } origin && MatchesTianChouShandao(f, origin) && use.Id == origin.CardUseFrameId && use.CardId == 0 && use.PhysicalCardIds is { Count: 0 } && use.CardKind == origin.CardKind && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, origin.ActorSeat, origin.ActorSeat);
        if (child is RecoveryReplacementFrame recovery)
            return IsTianChouRecoveryParent(f, recovery);
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == TianChouShandaoStage.TopChildren && r.Tops.Count > 0 && moved.ResumeProgramFrameId == f.Id && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsTianChouTopMovement(r.Tops[^1], m));
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage == TianChouShandaoStage.TopChildren && r.Tops.Count > 0 && r.Tops[^1] is { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } top && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.Program && hp.Change.ParentFrameId == f.Id && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == top.OwnerSeat && hp.Change.TargetSeat == top.OwnerSeat;
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? TianChouObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterTianChouOlOperations.TianChouFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 650)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 760)]
    internal bool HasTianChouDamageObserver(long id) => TianChouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 790)]
    internal bool HasTianChouBeforeDamageObserver(long id) => HasTianChouDamageObserver(id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 610)]
    internal bool IsTianChouDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && TianChouObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27090)]
    internal bool AdvanceTianChouSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && TianChouObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
