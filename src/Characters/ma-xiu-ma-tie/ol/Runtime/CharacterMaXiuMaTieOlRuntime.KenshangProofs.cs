#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaXiuMaTieOlRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27110)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27110)]
    internal bool ContinuePaidKenshang(ProgramSkillFrame f)
    {
        if (f.Kenshang is not null)
        {
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is { } id && f.ActivationId == id && f.WindowContext is { } c && CanRunIssuedKenshangCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 830)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 770)]
    internal bool IsKenshangRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => false;
    internal bool KenshangFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Kenshang is not { Stage: KenshangProgramStage.DrawChildren } r)
            return false;
        if (child is ProgramLifecycleTriggerWindowFrame s && s.ResumeProgramFrameId == f.Id)
            return s.Window == SkillProgramTriggerWindow.SkillsChanged && s.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return f.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame m && CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, m) && m.Batch.OriginOwnerSeat == f.OwnerSeat && m.Batch.OriginSkillId == f.SkillId && m.Batch.OriginSkillInstanceId == f.SkillInstanceId && m.Batch.Movements.All(x => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(x) && x.Sequence > r.Before && x.Sequence <= r.After);
    }

    internal ProgramSkillFrame? KenshangObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => this.KenshangFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27110)]
    internal bool AdvanceKenshangSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && KenshangObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
