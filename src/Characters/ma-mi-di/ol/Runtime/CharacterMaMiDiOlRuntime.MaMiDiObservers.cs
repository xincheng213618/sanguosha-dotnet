#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaMiDiOlRuntime
{
    internal bool IsMaMiDiStepMovement(MaMiDiDiscardStep step, CardMovementRecord m) => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > step.Before && m.Sequence <= step.After && (m.CardId == step.CardId && m.CardKind == step.Kind && m.From == step.From && m.To == step.To && m.Reason.Value == MaMiDiDiscardReason || step.Kind == CardKind.WoodenOx && step.From == CardLocation.Equipment(step.TargetSeat) && step.WoodenOxGrain.Contains(m.CardId) && m.From == CardLocation.WoodenOxGrain(step.TargetSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard);
    internal bool MaMiDiLion(ProgramSkillFrame f) => f.MaMiDi is { Stage: MaMiDiStage.TargetChildren, Steps.Count: > 0 } r && r.Steps[^1] is { Kind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment };
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 800)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 740)]
    internal bool IsMaMiDiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => MaMiDiLion(f) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.SourceSeat == f.MaMiDi!.Steps[^1].TargetSeat && recovery.Attempt.TargetSeat == recovery.Attempt.SourceSeat && recovery.Attempt.Amount == 1 && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MaMiDiDiscardReason;
    internal bool MaMiDiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MaMiDi is not { } r)
            return false;
        if (child is DeathFrame death)
            return r.Op == SkillProgramEffectOp.PayBingjieTurnMaximumHp && r.Stage == MaMiDiStage.MaximumChildren && r.MaximumBefore == 1 && r.MaximumAfter == 0 && r.HpAfter <= 0 && death.ParentFrameId == f.Id && death.VictimSeat == f.OwnerSeat && death.KillerSeat is null && death.ReturnKind == DeathReturnKind.ProgramSkill && _coreProgramEventHistoryOperations.ProgramEventHistory<MaMiDiMaximumIssuedEvent>().Count(e => e.FrameId == f.Id && e.Op == r.Op && e.MaximumBefore == 1 && e.MaximumAfter == 0) == 1;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && (r.Stage == MaMiDiStage.MaximumChildren ? hp.Continuation == PostEventContinuation.Program && hp.Change.Kind == HpChangeKind.MaximumHp && hp.Change.SourceSeat is null && hp.Change.TargetSeat == f.OwnerSeat && hp.Change.Amount == 1 && hp.Change.HpAfter == r.HpAfter : hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == r.Steps[^1].TargetSeat && hp.Change.TargetSeat == hp.Change.SourceSeat && MaMiDiLion(f));
        if (r.Stage != MaMiDiStage.TargetChildren || f.PendingMovementContinuation is null || r.Steps.Count == 0)
            return false;
        if (child is RecoveryReplacementFrame recovery)
            return IsMaMiDiRecoveryParent(f, recovery);
        return child is CardsMovedTriggerWindowFrame moved && (moved.ResumeProgramFrameId == f.Id || moved.Batch.AwaitingProgramFrameId == f.Id) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsMaMiDiStepMovement(r.Steps[^1], m));
    }

    internal ProgramSkillFrame? MaMiDiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MaMiDiFirstChild(f, child), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27070)]
    internal bool AdvanceMaMiDiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MaMiDiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
