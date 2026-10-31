#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangWenAnniversaryRuntime
{
    internal bool ValidZhangWenSibian(ProgramSkillFrame f)
    {
        if (f.ZhangWenSibian is not { } r || f.SkillId != AnniversaryZhangWenComposition.Sibian || f.TriggerId != AnniversaryZhangWenComposition.Replacement || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != ZhangWenSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || f.WindowContext is not { Window: SkillProgramTriggerWindow.DrawPhaseStarting } context || context.ParentFrameId != r.ParentId || _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == r.ParentId)is not ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.DrawPhaseStarting } parent || parent.OwnerSeat != f.OwnerSeat || parent.CandidateIndex < 0 || parent.CandidateIndex >= parent.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex]) || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSibianStartedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var started] || started != new ZhangWenSibianStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn) || r.Entities.Count > 4 || r.Entities.Select(e => e.CardId).Distinct().Count() != r.Entities.Count || r.Extrema.Distinct().Count() != r.Extrema.Count)
            return false;
        var revealed = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSibianEntityEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
        if (revealed.Length != r.Entities.Count || revealed.Where((e, i) => e.Index != i || e.Entity != r.Entities[i] || e.Extreme != r.Extrema.Contains(e.Entity.CardId)).Any() || r.Entities.Any(e => e.RevealSequence <= 0 || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == e.RevealSequence && m.CardId == e.CardId && m.CardKind == e.Kind && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing)))
            return false;
        var expected = r.Entities.Count == 0 ? [] : r.Entities.Where(e => e.Rank == r.Entities.Min(x => x.Rank) || e.Rank == r.Entities.Max(x => x.Rank)).Select(e => e.CardId).ToArray();
        if (!expected.SequenceEqual(r.Extrema))
            return false;
        var recordedSteps = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSibianStepEvent>().Where(e => e.FrameId == f.Id).Select(e => e.Step).ToArray();
        if (!recordedSteps.SequenceEqual(r.Steps) || r.Steps.Count is < 1 or > 3 || r.Steps.Select(s => s.Stage).Distinct().Count() != r.Steps.Count || r.Steps[0].Stage != ZhangWenSibianStage.RevealChildren || r.Steps.Where((s, i) => s.Before < 0 || s.After < s.Before || s.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || i > 0 && s.Before < r.Steps[i - 1].After).Any())
            return false;
        foreach (var step in r.Steps)
        {
            var moves = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > step.Before && m.Sequence <= step.After).ToArray();
            if (step.Stage == ZhangWenSibianStage.RevealChildren)
            {
                if (r.Entities.Any(e => e.RevealSequence <= step.Before || e.RevealSequence > step.After))
                    return false;
            }
            else if (step.Stage == ZhangWenSibianStage.GainChildren)
            {
                if (moves.Where(m => m.Reason == ZhangWenGain).Any(m => !r.Extrema.Contains(m.CardId) || m.From != CardLocation.Processing || m.To != CardLocation.Hand(f.OwnerSeat) && m.To != CardLocation.OutsideGame && !_coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, CardLocation.Hand(f.OwnerSeat))))
                    return false;
            }
            else if (step.Stage == ZhangWenSibianStage.FinishChildren)
            {
                var target = r.Recipient is { } seat && seat >= 0 ? CardLocation.Hand(seat) : CardLocation.DiscardPile;
                if (moves.Where(m => m.Reason == ZhangWenGift || m.Reason == ZhangWenCleanup).Any(m => !r.Entities.Any(e => e.CardId == m.CardId) || m.From != CardLocation.Processing || m.To != target && m.To != CardLocation.OutsideGame && !_coreHappyWeiYanGainDestinationsOperations.IsHappyWeiYanRedirectedGain(m, target)))
                    return false;
            }
            else
                return false;
        }

        var choices = _coreProgramEventHistoryOperations.ProgramEventHistory<ZhangWenSibianRecipientEvent>().Where(e => e.FrameId == f.Id).ToArray();
        return r.Recipient is null ? choices.Length == 0 : choices is [var choice] && choice.Recipient == r.Recipient && (choice.Recipient == -1 || _coreTableSeatQueriesOperations.IsValidPlayerSeat(choice.Recipient));
    }

    internal void RequireZhangWenSibian(ProgramSkillFrame f)
    {
        if (!ValidZhangWenSibian(f))
            throw new InvalidOperationException("Sibian lost its original draw parent, public top-four entities, extrema or once-paid movement return.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 45100)]
    internal bool ZhangWenMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ZhangWenSibian is { Stage: ZhangWenSibianStage.RevealChildren or ZhangWenSibianStage.GainChildren or ZhangWenSibianStage.FinishChildren } && ValidZhangWenSibian(f) && e?.Op == SkillProgramEffectOp.ResolveAnniversaryZhangWenSibian && f.PendingMovementContinuation == p && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool ZhangWenFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhangWenSibian is not { } r || f.PendingMovementContinuation is null || !ValidZhangWenSibian(f) || r.Steps.Count == 0)
            return false;
        var step = r.Steps[^1];
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > step.Before && m.Sequence <= step.After);
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? ZhangWenObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterZhangWenAnniversaryOperations.ZhangWenFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 45100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 45100)]
    internal bool ZhangWenDamageObserver(long id) => ZhangWenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 45100)]
    internal bool ZhangWenBeforeDamageObserver(long id) => ZhangWenObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 45100)]
    internal bool AdvanceZhangWenSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangWenObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
