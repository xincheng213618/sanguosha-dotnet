namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramPipelineHook(ProgramPipelinePhase.HealthChangeSubject, 81500)]
    private bool MobileWangCanXiHealthSubject(int target) => IsValidPlayerSeat(target) && _players[target].Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) > 0;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 81500)]
    private void AssertMobileWangCanXi(ProgramSkillFrame f)
    {
        if (f.MobileWangCanXi is not { } r) return;
        if (f.SkillId != MobileWangCanShanxi || r.Source != MobileWangCanSource(f) || r.Hash != f.GameplayHash || f.InstructionIndex != 1 ||
            !Enum.IsDefined(r.Stage) || r.Cards.Count > 2 || r.Cards.Distinct().Count() != r.Cards.Count || r.Before > r.After || r.After > _movementSequence ||
            ProgramEventHistory<MobileWangCanXiIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var issued] ||
            issued != new MobileWangCanXiIssuedEvent(f.Id, r.Source, r.Hash, r.ChangeId, r.Subject))
            throw new InvalidOperationException("Shanxi lost its exact original source, issuing receipt or native payment cursor.");
        if (r.Stage == MobileWangCanXiStage.Mark ? r.ChangeId != 0 || r.Subject != -1 || f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting :
            !IsValidPlayerSeat(r.Subject) || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { } change } ||
            change.Id != r.ChangeId || change.TargetSeat != r.Subject || !ProgramEventHistory<MobileWangCanXiQualifiedEvent>().Any(e =>
                e.ChangeId == r.ChangeId && e.Source == r.Source && e.Hash == r.Hash && e.TargetSeat == r.Subject))
            throw new InvalidOperationException("Shanxi lost its actual starting boundary or frozen marked recovery.");
        if (r.Stage == MobileWangCanXiStage.GiftChildren && (r.Cards.Count != 2 || ProgramEventHistory<MobileWangCanXiPaidEvent>().Where(e => e.FrameId == f.Id).ToArray() is not [var paid] ||
            !paid.Gift || paid.TargetSeat != r.Subject || paid.Before != r.Before || paid.After != r.After || paid.ActualCount != r.ActualCount))
            throw new InvalidOperationException("Shanxi lost its two-card actual native gift return.");
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 81500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 81500)]
    private bool KeepMobileWangCanXi(ProgramSkillFrame f)
    { if (f.MobileWangCanXi is not { Stage: not MobileWangCanXiStage.Mark }) return false; AssertMobileWangCanXi(f); return true; }

    private bool MobileWangCanXiObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileWangCanXi is not { } r) return false; AssertMobileWangCanXi(f);
        if (r.Stage == MobileWangCanXiStage.Health) return HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying) return r.Stage == MobileWangCanXiStage.HealthChildren && dying.ParentFrameId == f.Id &&
            dying.VictimSeat == r.Subject && dying.ResumesProgramSkill && dying.KillerSeat is null;
        if (child is HpChangedTriggerWindowFrame hp) return ProgramHpReturnMatches(f, hp) &&
            (r.Stage == MobileWangCanXiStage.HealthChildren && hp.Change.TargetSeat == r.Subject || r.Stage == MobileWangCanXiStage.GiftChildren && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery) return r.Stage == MobileWangCanXiStage.GiftChildren && RecoveryReplacementFrameRidesOn(recovery, f) &&
            recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileWangCanXiGift;
        if (child is CardsMovedTriggerWindowFrame moved) return r.Stage == MobileWangCanXiStage.GiftChildren && ProgramMovementReturnMatches(f, moved) &&
            moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId &&
            moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id &&
            (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }
    private ProgramSkillFrame? MobileWangCanXiObserverRoot() => FindProgramObserverRoot(static (e, f, child) => e.MobileWangCanXiObserverChild(f, child),
        static (e, index) => e.HasObserverSuffix(index, static (g, i) => g.IsMovementObserverEdge(i) || g.IsSameNameHandObserverEdge(i) || g.IsYanPuDyingExitEdge(i) ||
            g.TypedDamagePostEventEdge(g._resolutionStack[i - 1], g._resolutionStack[i]) || g.RecoveryReplacementFrameRidesOn(g._resolutionStack[i], g._resolutionStack[i - 1]),
            static (g, i, dying) => g.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 81500)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 81500)]
    private bool MobileWangCanXiDyingProof() => ActiveDying is { } dying && MobileWangCanXiObserverRoot() is { } root &&
        _resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 81500)]
    private bool AdvanceMobileWangCanXiHealth() => _pendingDecision is null && MobileWangCanXiObserverRoot() is not null && TryAdvanceObserverSubtree(true);
}
