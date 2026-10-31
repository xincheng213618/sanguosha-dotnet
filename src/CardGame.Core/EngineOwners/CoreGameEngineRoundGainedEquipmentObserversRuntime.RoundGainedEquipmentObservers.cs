#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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
    internal ProgramSkillFrame? RoundGainedEquipmentDrawObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreRoundGainedEquipmentObserversOperations.RoundGainedEquipmentDrawFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 10, group: 70)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 20)]
    internal bool IsRoundGainedEquipmentDrawDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && RoundGainedEquipmentDrawObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    internal bool HasRoundGainedEquipmentDrawCardObserver(long useId) => RoundGainedEquipmentDrawObserverRoot()is { } root && root.RoundGainedEquipmentDrawReceipt!.Qualification.CardUseFrameId == useId;
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1660)]
    internal bool HasRoundGainedEquipmentDrawDamageObserver(long id) => HasRoundGainedEquipmentDrawObserver(id, false);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 240)]
    internal bool HasRoundGainedEquipmentDrawBeforeDamageObserver(long id) => HasRoundGainedEquipmentDrawObserver(id, true);
    internal bool HasRoundGainedEquipmentDrawObserver(long id, bool before)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && (before ? f is BeforeDamageProgramWindowFrame : f is DamageTriggerWindowFrame));
        if (index < 0 || RoundGainedEquipmentDrawObserverRoot()is not { } root)
            return false;
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
        if (index > rootIndex)
            return true;
        for (var child = index + 1; child <= rootIndex; child++)
            if (!RoundGainedEquipmentDrawStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreResponseCompletionOperations.ResponseCompletionStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossStructuralEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreOrderedPrintedSkillLossObserversOperations.OrderedPrintedSkillLossSkillsChangedEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]))
                return false;
        return index < rootIndex;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.BeforeResponseHealth, 130)]
    internal bool TryAdvanceRoundGainedEquipmentDrawSubtree()
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || RoundGainedEquipmentDrawObserverRoot()is null)
            return false;
        return _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
    }
}
