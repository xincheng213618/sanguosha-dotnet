#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHanSongAnniversaryRuntime
{
    internal bool ExactHanSongParent(ProgramSkillFrame f)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        return f.SkillId == AnniversaryHanSongComposition.Shuaiyan && f.WindowContext is { } c && c.OwnerSeat == f.OwnerSeat && index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame p && p.Id == c.ParentFrameId && p.Window == c.Window && p.OwnerSeat == c.SourceSeat && c.TargetSeat == p.OwnerSeat && p.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && (p.Window == SkillProgramTriggerWindow.DrawPhaseEnded && p.Continuation == ProgramLifecycleContinuation.CompleteDrawPhaseEnded && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Draw || p.Window == SkillProgramTriggerWindow.DiscardPhaseEnded && p.Continuation == ProgramLifecycleContinuation.EndTurnAfterDiscardPhase && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Discard) && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]);
    }

    internal bool ValidHanSong(ProgramSkillFrame f)
    {
        if (f.AnniversaryHanSong is not { } r || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !ExactHanSongParent(f) || r.Source != HanSongSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || r.ParentId != f.WindowContext!.ParentFrameId || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.SubjectSeat != f.WindowContext.SourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.SubjectSeat) || r.DrawCount < 1 || r.DrawCount > _coreMatchParticipantsPlayersAccess._players.Count || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHanSongStartedEvent>().Count(e => e == new AnniversaryHanSongStartedEvent(f.Id, r.Source, r.GameplayHash, r.ParentId, r.ActualTurn, r.SubjectSeat, r.DrawCount)) != 1)
            return false;
        if (r.SubjectSeat != f.OwnerSeat && r.DrawCount != 1)
            return false;
        var issues = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHanSongBenefitIssuedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage is AnniversaryHanSongStage.BenefitChoice or AnniversaryHanSongStage.CardChoice)
            return r.SubjectSeat != f.OwnerSeat && r.Draw is null && r.DiscardId == 0 && r.Before == 0 && r.After == 0 && issues.Length == 0;
        if (issues is not [var issued] || issued.Count != (issued.Draw ? r.DrawCount : 1))
            return false;
        if (issued.Draw)
        {
            if (r.DiscardId != 0 || r.DiscardFrom is not null || r.DiscardSequence != 0)
                return false;
            if (r.Stage == AnniversaryHanSongStage.Drawing)
                return r.Draw is null;
            return r.Stage is AnniversaryHanSongStage.DrawChildren or AnniversaryHanSongStage.Complete && r.Draw is { } draw && draw.RecipientSeat == f.OwnerSeat && draw.RequestedCount == r.DrawCount && draw.SequenceBefore == r.Before && draw.SequenceAfter == r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHanSongDrawReturnedEvent>().Count(e => e == new AnniversaryHanSongDrawReturnedEvent(f.Id, draw.ActualDrawIds.Count, r.Before, r.After)) == 1;
        }

        return r.Stage is AnniversaryHanSongStage.DiscardChildren or AnniversaryHanSongStage.Complete && r.Draw is null && r.DiscardId > 0 && r.DiscardFrom is { } from && from.OwnerSeat == r.SubjectSeat && from.Zone is CardZoneKind.Hand or CardZoneKind.Equipment && r.DiscardSequence > r.Before && r.DiscardSequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == r.DiscardSequence && m.CardId == r.DiscardId && m.From == from && m.Reason.Value == HanSongDiscardReason && (m.To == CardLocation.DiscardPile || _coreProgramAtomicCardMovesOperations.IsRedirectedPaidDiscardMovement(m, from, new(HanSongDiscardReason)))) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryHanSongDiscardPaidEvent>().Count(e => e == new AnniversaryHanSongDiscardPaidEvent(f.Id, r.SubjectSeat, r.DiscardSequence)) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 45900)]
    internal void AssertHanSong(ProgramSkillFrame f)
    {
        if (f.AnniversaryHanSong is not { } r)
            return;
        if (!ValidHanSong(f))
            throw new InvalidOperationException("Han Song lost the original phase candidate, immutable benefit issue, native draw or real logical-discard receipt.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && r.Stage is AnniversaryHanSongStage.BenefitChoice or AnniversaryHanSongStage.CardChoice && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } p || p.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, HanSongChoices(f))))
            throw new InvalidOperationException("Han Song lost its exact published owner menu.");
    }

    internal bool HasAnniversaryHanSongPrompt() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger } p && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryHanSong: { Stage: AnniversaryHanSongStage.BenefitChoice or AnniversaryHanSongStage.CardChoice }, Step: ResolutionFrameStep.AwaitingResponse } f && p.PlayerSeat == f.OwnerSeat && ValidHanSong(f) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, HanSongChoices(f));
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 45900)]
    internal bool HanSongMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryHanSong is { Stage: AnniversaryHanSongStage.DrawChildren or AnniversaryHanSongStage.DiscardChildren } && e?.Op == SkillProgramEffectOp.ResolveAnniversaryHanSongShuaiyan && f.PendingMovementContinuation == p && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidHanSong(f);
}
