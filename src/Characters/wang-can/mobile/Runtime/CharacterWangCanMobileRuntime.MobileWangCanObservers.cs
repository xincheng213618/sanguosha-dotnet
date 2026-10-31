#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWangCanMobileRuntime
{
    [ProgramPipelineHook(ProgramPipelinePhase.HealthChangeSubject, 81500)]
    internal bool MobileWangCanXiHealthSubject(int target) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(target) && _coreMatchParticipantsPlayersAccess._players[target].Markers.GetValueOrDefault(PlayerMarkerKind.MobileWangCanXi) > 0;
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 81500)]
    internal void AssertMobileWangCanXi(ProgramSkillFrame f)
    {
        if (f.MobileWangCanXi is not { } r)
            return;
        if (f.SkillId != MobileWangCanShanxi || r.Source != MobileWangCanSource(f) || r.Hash != f.GameplayHash || f.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || r.Cards.Count > 2 || r.Cards.Distinct().Count() != r.Cards.Count || r.Before > r.After || r.After > _coreCardMovementJournalMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWangCanXiIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var issued] || issued != new MobileWangCanXiIssuedEvent(f.Id, r.Source, r.Hash, r.ChangeId, r.Subject))
            throw new InvalidOperationException("Shanxi lost its exact original source, issuing receipt or native payment cursor.");
        if (r.Stage == MobileWangCanXiStage.Mark ? r.ChangeId != 0 || r.Subject != -1 || f.WindowContext?.Window != SkillProgramTriggerWindow.PlayPhaseStarting : !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Subject) || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterHpRecovered, HpChange: { } change } || change.Id != r.ChangeId || change.TargetSeat != r.Subject || !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileWangCanXiQualifiedEvent>().Any(e => e.ChangeId == r.ChangeId && e.Source == r.Source && e.Hash == r.Hash && e.TargetSeat == r.Subject))
            throw new InvalidOperationException("Shanxi lost its actual starting boundary or frozen marked recovery.");
        if (r.Stage == MobileWangCanXiStage.GiftChildren && (r.Cards.Count != 2 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileWangCanXiPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || !paid.Gift || paid.TargetSeat != r.Subject || paid.Before != r.Before || paid.After != r.After || paid.ActualCount != r.ActualCount))
            throw new InvalidOperationException("Shanxi lost its two-card actual native gift return.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 81500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 81500)]
    internal bool KeepMobileWangCanXi(ProgramSkillFrame f)
    {
        if (f.MobileWangCanXi is not { Stage: not MobileWangCanXiStage.Mark })
            return false;
        AssertMobileWangCanXi(f);
        return true;
    }

    internal bool MobileWangCanXiObserverChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.MobileWangCanXi is not { } r)
            return false;
        AssertMobileWangCanXi(f);
        if (r.Stage == MobileWangCanXiStage.Health)
            return _characterDaQiaoXiaoQiaoHappyGodOperations.HappyQiaoHealthChoiceRidesOn(child, f);
        if (child is DyingFrame dying)
            return r.Stage == MobileWangCanXiStage.HealthChildren && dying.ParentFrameId == f.Id && dying.VictimSeat == r.Subject && dying.ResumesProgramSkill && dying.KillerSeat is null;
        if (child is HpChangedTriggerWindowFrame hp)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramHpReturnMatches(f, hp) && (r.Stage == MobileWangCanXiStage.HealthChildren && hp.Change.TargetSeat == r.Subject || r.Stage == MobileWangCanXiStage.GiftChildren && hp.Change.Kind == HpChangeKind.Recovery);
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage == MobileWangCanXiStage.GiftChildren && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && recovery.Attempt.Completion.MoveReason?.Value == MobileWangCanXiGift;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == MobileWangCanXiStage.GiftChildren && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After);
        return child is ProgramLifecycleTriggerWindowFrame lifecycle && lifecycle.ResumeProgramFrameId == f.Id && (lifecycle.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange || lifecycle is { Window: SkillProgramTriggerWindow.SkillsChanged, Continuation: ProgramLifecycleContinuation.ResumeParentProgram });
    }

    internal ProgramSkillFrame? MobileWangCanXiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterWangCanMobileOperations.MobileWangCanXiObserverChild(f, child), (index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 81500)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 81500)]
    internal bool MobileWangCanXiDyingProof() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && MobileWangCanXiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 81500)]
    internal bool AdvanceMobileWangCanXiHealth() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MobileWangCanXiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
