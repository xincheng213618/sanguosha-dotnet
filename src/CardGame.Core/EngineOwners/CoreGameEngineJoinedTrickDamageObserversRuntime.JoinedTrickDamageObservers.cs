#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineJoinedTrickDamageObserversRuntime
{
    internal bool JoinedTrickDamageRewardFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.JoinedTrickDamageRewardReceipt is not { } receipt || !_coreJoinedTrickDamageRewardsOperations.Value.ValidJoinedTrickDamageReward(frame))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == frame.Id && moved.Batch.AwaitingProgramFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > receipt.MovementSequenceBefore && m.Sequence <= receipt.MovementSequenceAfter && m.Reason.Value == "skill-program.joined-trick.damage-reward" && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat));
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == frame.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == frame.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal bool JoinedTrickDamageRewardStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is ProgramSkillFrame root && JoinedTrickDamageRewardFirstChild(root, child) || parent is ProgramCardTriggerWindowFrame window && child is ProgramSkillFrame { JoinedTrickDamageRewardReceipt: not null } reward && reward.WindowContext?.ParentFrameId == window.Id && _coreJoinedTrickDamageRewardsOperations.Value.ValidJoinedTrickDamageReward(reward);
    internal ProgramSkillFrame? JoinedTrickDamageRewardObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.JoinedTrickDamageRewardFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 30)]
    internal bool IsJoinedTrickDamageRewardDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && JoinedTrickDamageRewardObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 120)]
    internal bool TryAdvanceJoinedTrickDamageRewardSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || JoinedTrickDamageRewardObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
