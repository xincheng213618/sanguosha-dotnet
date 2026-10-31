#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoTongZhaoGuangAnniversaryRuntime
{
    internal bool AnniversaryZtgOriginal(ProgramSkillFrame f)
    {
        if (f.SkillId is not (AnniversaryZhaoTongZhaoGuangComposition.Longyuan or AnniversaryZhaoTongZhaoGuangComposition.Qingren) || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.ActivationId != f.TriggerId || f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.SkillInstanceId == f.SkillInstanceId && e.BindingId == f.TriggerId && e.Window == c.Window) != 1)
            return false;
        if (f.SkillId == AnniversaryZhaoTongZhaoGuangComposition.Longyuan)
            return f.TriggerId == "three-yizan-awakening" && c.Window == SkillProgramTriggerWindow.AfterTurnEnded && _resolutionStackOperations._resolutionStack.OfType<DeferredTurnEndFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is { } parent && _coreAfterTurnEndedProgramsOperations.MatchesAfterTurnEndedChild(parent, f, c);
        return f.TriggerId == "own-ending-yizan-count" && c.Window == SkillProgramTriggerWindow.TurnEnding && c.SourceSeat == f.OwnerSeat && c.TargetSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack.OfType<TurnEndingBoundaryFrame>().SingleOrDefault(p => p.Id == c.ParentFrameId)is { } ending && ending.OwnerSeat == f.OwnerSeat && ending.TurnNumber == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && candidate.OccurrenceIndex == c.OccurrenceIndex;
    }

    internal bool ValidAnniversaryQingren(ProgramSkillFrame f)
    {
        if (f.AnniversaryQingren is not { } r || !AnniversaryZtgOriginal(f) || f.SkillId != AnniversaryZhaoTongZhaoGuangComposition.Qingren || r.Source != AnniversaryZtgSource(f) || r.Hash != f.GameplayHash || r.ParentId != f.WindowContext!.ParentFrameId || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || f.InstructionIndex is < 1 or > 2 || f.InstructionIndex == 2 && r.Stage != AnniversaryQingrenStage.Completed || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.CutoffActionId < 0 || r.CutoffActionId > _coreGameEngineCardActionsCardActionSequenceAccess._cardActionSequence || r.Count != AnniversaryYizanCount(f.OwnerSeat, r.Turn, r.CutoffActionId) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQingrenIssuedEvent>().Count(e => e == new AnniversaryQingrenIssuedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn, r.CutoffActionId, r.Count)) != 1)
            return false;
        if (r.Draw is not { } d)
            return r.Count == 0 ? r.Stage == AnniversaryQingrenStage.Completed : r.Stage == AnniversaryQingrenStage.Drawing;
        return r.Count > 0 && r.Stage is AnniversaryQingrenStage.DrawChildren or AnniversaryQingrenStage.Completed && d.RecipientSeat == f.OwnerSeat && d.RequestedCount == r.Count && d.SequenceBefore >= 0 && d.SequenceAfter >= d.SequenceBefore && d.SequenceAfter <= _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence && d.ActualDrawIds is System.Collections.IList { IsReadOnly: true } && d.ActualDrawIds.Distinct().Count() == d.ActualDrawIds.Count && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQingrenDrawReturnedEvent>().Count(e => e == new AnniversaryQingrenDrawReturnedEvent(f.Id, r.Count, d.ActualDrawIds.Count, d.SequenceBefore, d.SequenceAfter)) == 1 && d.ActualDrawIds.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > d.SequenceBefore && m.Sequence <= d.SequenceAfter && m.CardId == id && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == AnniversaryQingrenDrawReason) == 1);
    }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 44600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 44600)]
    internal bool KeepPaidAnniversaryQingren(ProgramSkillFrame f) => f.AnniversaryQingren is { Stage: AnniversaryQingrenStage.Drawing or AnniversaryQingrenStage.DrawChildren } && ValidAnniversaryQingren(f);
    internal bool AnniversaryQingrenFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryQingren is not { } r || !ValidAnniversaryQingren(f))
            return false;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryQingrenStage.Drawing && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f) && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == r.Count;
        if (child is CardsMovedTriggerWindowFrame moved)
            return r.Stage == AnniversaryQingrenStage.DrawChildren && r.Draw is { } outcome && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > outcome.SequenceBefore && m.Sequence <= outcome.SequenceAfter);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal bool AnniversaryQingrenObserverSuffix(int root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying));
    internal ProgramSkillFrame? AnniversaryQingrenObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhaoTongZhaoGuangAnniversaryOperations.AnniversaryQingrenFirstChild(f, child), (i) => _characterZhaoTongZhaoGuangAnniversaryOperations.AnniversaryQingrenObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 44600)]
    internal bool AnniversaryQingrenDyingObserver() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && AnniversaryQingrenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 44600)]
    internal bool AdvanceAnniversaryQingrenChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && AnniversaryQingrenObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
