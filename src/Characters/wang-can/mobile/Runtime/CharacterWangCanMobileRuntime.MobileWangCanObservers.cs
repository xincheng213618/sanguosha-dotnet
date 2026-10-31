#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanMobileRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.HealthChangeSubject, 81500)]
    internal bool MobileWangCanXiHealthSubject(int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) > 0;

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 81500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 81500)]
    internal bool KeepMobileWangCanXi(ProgramSkillFrame f)
    {
        if (f.MobileWangCanXi is not { Stage: not MobileWangCanXiStage.Mark })
            return false;
        return true;
    }

    internal bool MobileWangCanXiObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileWangCanXi is not { } r)
            return false;
        if (r.Stage == MobileWangCanXiStage.Health)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying)
            return r.Stage == MobileWangCanXiStage.HealthChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Subject && dying.ResumesProgramSkill && dying.KillerSeat is null;
        if (child is HpChangedTriggerWindowFrame hp)
            return CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && (r.Stage == MobileWangCanXiStage.HealthChildren && hp.Change.TargetSeat == r.Subject || r.Stage == MobileWangCanXiStage.GiftChildren && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage == MobileWangCanXiStage.GiftChildren && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileWangCanXiGift;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == MobileWangCanXiStage.GiftChildren && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileWangCanXiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => this.MobileWangCanXiObserverChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 81500)]
    internal bool MobileWangCanXiDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileWangCanXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 81500)]
    internal bool AdvanceMobileWangCanXiHealth() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileWangCanXiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
