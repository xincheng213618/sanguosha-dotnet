#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineRoundGainedEquipmentObserversRuntime
{
    internal bool RoundGainedEquipmentDrawFirstChild(ProgramSkillFrame frame, ResolutionFrame child)
    {
        if (frame.RoundGainedEquipmentDrawReceipt is not { } receipt || !_coreRoundGainedEquipmentDrawsOperations.ValidRoundGainedEquipmentDraw(frame))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.Id == moved.Id && moved.Batch.ParentFrameId == frame.Id && moved.Batch.AwaitingProgramFrameId == frame.Id && moved.Batch.OriginOwnerSeat == frame.OwnerSeat && moved.Batch.OriginSkillId == frame.SkillId && moved.Batch.OriginSkillInstanceId == frame.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > receipt.MovementSequenceBefore && m.Sequence <= receipt.MovementSequenceAfter && m.Reason.Value == "skill-program.round-gained.equipment-draw" && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(frame.OwnerSeat));
        if (child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged)
            return skills.ResumeProgramFrameId == frame.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex <= skills.Candidates.Count;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == frame.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && state.CandidateIndex >= 0 && state.CandidateIndex <= state.Candidates.Count && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == frame.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal bool RoundGainedEquipmentDrawStructuralEdge(ResolutionFrame parent, ResolutionFrame child) => parent is ProgramSkillFrame root && RoundGainedEquipmentDrawFirstChild(root, child) || parent is ProgramCardTriggerWindowFrame window && child is ProgramSkillFrame { RoundGainedEquipmentDrawReceipt: not null } reward && reward.WindowContext?.ParentFrameId == window.Id && _coreRoundGainedEquipmentDrawsOperations.ValidRoundGainedEquipmentDraw(reward);
    internal ProgramSkillFrame? RoundGainedEquipmentDrawObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => this.RoundGainedEquipmentDrawFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 20)]
    internal bool IsRoundGainedEquipmentDrawDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && RoundGainedEquipmentDrawObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 130)]
    internal bool TryAdvanceRoundGainedEquipmentDrawSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || RoundGainedEquipmentDrawObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
