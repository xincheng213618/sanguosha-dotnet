#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOffTurnUsedCardObserversRuntime
{
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 560)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 580)]
    internal bool CanContinueOffTurnUsedCardGift(ProgramSkillFrame frame)
    {
        if (frame.OffTurnUsedCardGift is not { Stage: OffTurnUsedCardGiftStage.GiftChildren })
            return false;
        return true;
    }
    internal bool OffTurnUsedCardGiftFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.OffTurnUsedCardGift is { Stage: OffTurnUsedCardGiftStage.GiftChildren } && OffTurnUsedCardGiftSkillsChangedEdge(root, child))
            return true;
        if (root.OffTurnUsedCardGift is not { Stage: OffTurnUsedCardGiftStage.GiftChildren } r || child is not CardsMovedTriggerWindowFrame movement || movement.Batch.Id != r.GiftBatchId || movement.Batch.ParentFrameId != root.Id || movement.Batch.AwaitingProgramFrameId is { } awaiting && awaiting != root.Id || movement.Batch.OriginSkillId != root.SkillId || movement.Batch.OriginSkillInstanceId != root.SkillInstanceId || movement.Batch.OriginOwnerSeat != root.OwnerSeat || r.PaidCardIds is null || r.RecipientSeat is null || !movement.Batch.Movements.Select(m => m.CardId).SequenceEqual(r.PaidCardIds) || movement.Batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After || m.From != CardLocation.DiscardPile || m.To != CardLocation.Hand(r.RecipientSeat.Value) || m.Reason.Value != $"skill-program.{(root.SkillId)}.off-turn-used-card-gift"))
            return false;
        return true;
    }

    internal bool OffTurnUsedCardGiftSkillsChangedEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (parent is ProgramSkillFrame program && child is ProgramLifecycleTriggerWindowFrame window)
            return window.Window == SkillProgramTriggerWindow.SkillsChanged && window.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && window.ResumeProgramFrameId == program.Id && _coreTableSeatQueriesOperations.IsValidPlayerSeat(window.OwnerSeat) && window.Candidates.Count > 0 && window.CandidateIndex >= 0 && window.CandidateIndex <= window.Candidates.Count && window.Candidates.All(c => c.OwnerSeat == window.OwnerSeat && _coreProgramLifecycleOperations.GetProgramTrigger2(c).Window == SkillProgramTriggerWindow.SkillsChanged);
        return parent is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex < skills.Candidates.Count && child is ProgramSkillFrame observer && observer.WindowContext is { Window: SkillProgramTriggerWindow.SkillsChanged } context && context.ParentFrameId == skills.Id && context.OwnerSeat == observer.OwnerSeat && context.SourceSeat == skills.OwnerSeat && context.TargetSeat == skills.OwnerSeat && CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(observer, skills.Candidates[skills.CandidateIndex]) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == observer.Id && e.OwnerSeat == observer.OwnerSeat && e.SkillId == observer.SkillId && e.BindingId == observer.TriggerId && e.SkillInstanceId == observer.SkillInstanceId && e.Window == context.Window) == 1;
    }

    internal bool OffTurnUsedCardGiftStructuralEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (parent is ProgramSkillFrame root && root.OffTurnUsedCardGift is not null)
        {
            return OffTurnUsedCardGiftFirstChild(root, child);
        }

        if (child is not ProgramSkillFrame observer || observer.OffTurnUsedCardGift is not { } r || parent is not CardsMovedTriggerWindowFrame window || window.Id != r.WindowFrameId || window.Batch.Id != r.AnchorBatchId)
            return false;
        return true;
    }

    internal ProgramSkillFrame? OffTurnUsedCardGiftObserverRoot(long id)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id);
        if (index < 0 || _resolutionStackOperations._resolutionStack[index] is not ProgramSkillFrame root || root.OffTurnUsedCardGift is not { } r)
            return null;
        if (index == _resolutionStackOperations._resolutionStack.Count - 1)
            return root;
        if (!OffTurnUsedCardGiftFirstChild(root, _resolutionStackOperations._resolutionStack[index + 1]))
            return null;
        for (var child = index + 2; child < _resolutionStackOperations._resolutionStack.Count; child++)
        {
            if (!OffTurnUsedCardGiftSkillsChangedEdge(_resolutionStackOperations._resolutionStack[child - 1], _resolutionStackOperations._resolutionStack[child]) && !_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(child))
                return null;
            if (_resolutionStackOperations._resolutionStack[child] is DyingFrame dying && (_coreParticipantHandDamageRidesOperations.IsPaidHandRepaymentRescueRide(child, dying) || _coreParticipantHandProgramRescueRidesOperations.IsPaidHandRepaymentProgramAlcoholRide(child, dying) || _coreUnrespondableCounterspellsOperations.PolicyCounterspellVirtualAlcoholRide(child, dying) || _coreHalfHandPhaseDebtDamageObserversOperations.PaidObserverDamageVirtualAlcoholRide(child, dying)))
                break;
        }

        return root;
    }
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 580)]
    internal bool IsOffTurnUsedCardGiftProgramDying() => _coreDyingCursorRuntimeOperations.Value.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OffTurnUsedCardGift is not null && OffTurnUsedCardGiftObserverRoot(f.Id)is not null && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id));
}
