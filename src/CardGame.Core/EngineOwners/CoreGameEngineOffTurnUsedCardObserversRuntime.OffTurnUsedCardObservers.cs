#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

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

    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 170)]
    internal bool IsOffTurnUsedCardGiftMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => effect?.Op == SkillProgramEffectOp.GiveOffTurnUsedCards && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null && CanContinueOffTurnUsedCardGift(frame);
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
        return parent is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram && skills.CandidateIndex >= 0 && skills.CandidateIndex < skills.Candidates.Count && child is ProgramSkillFrame observer && observer.WindowContext is { Window: SkillProgramTriggerWindow.SkillsChanged } context && context.ParentFrameId == skills.Id && context.OwnerSeat == observer.OwnerSeat && context.SourceSeat == skills.OwnerSeat && context.TargetSeat == skills.OwnerSeat && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(observer, skills.Candidates[skills.CandidateIndex]) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ProgramBindingStartedEvent>().Count(e => e.FrameId == observer.Id && e.OwnerSeat == observer.OwnerSeat && e.SkillId == observer.SkillId && e.BindingId == observer.TriggerId && e.SkillInstanceId == observer.SkillInstanceId && e.Window == context.Window) == 1;
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
    internal bool IsOffTurnUsedCardGiftProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Any(f => f.OffTurnUsedCardGift is not null && OffTurnUsedCardGiftObserverRoot(f.Id)is not null && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id));
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1790)]
    internal bool HasOffTurnUsedCardGiftDamageObserver(long windowId) => OffTurnUsedCardGiftDamageObserver(windowId, false);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 140)]
    internal bool HasOffTurnUsedCardGiftBeforeDamageObserver(long windowId) => OffTurnUsedCardGiftDamageObserver(windowId, true);
    internal bool OffTurnUsedCardGiftDamageObserver(long windowId, bool before)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == windowId);
        if (index < 0)
            return false;
        if (before ? _resolutionStackOperations._resolutionStack[index] is not BeforeDamageProgramWindowFrame : _resolutionStackOperations._resolutionStack[index] is not DamageTriggerWindowFrame)
            return false;
        if (_resolutionStackOperations._resolutionStack.Take(index).OfType<ProgramSkillFrame>().Any(f => f.OffTurnUsedCardGift is not null && OffTurnUsedCardGiftObserverRoot(f.Id)is not null))
            return true;
        ProgramTriggerCandidate? candidate = _resolutionStackOperations._resolutionStack[index] switch
        {
            BeforeDamageProgramWindowFrame b when b.CandidateIndex >= 0 && b.CandidateIndex < b.Candidates.Count => b.Candidates[b.CandidateIndex].Candidate,
            DamageTriggerWindowFrame d when d.CandidateIndex >= 0 && d.CandidateIndex < d.Candidates.Count => d.Candidates[d.CandidateIndex].ToProgramCandidate(),
            _ => null
        };
        if (candidate is null || index + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramSkillFrame producer || producer.WindowContext?.ParentFrameId != windowId || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(producer, candidate))
            return false;
        foreach (var root in _resolutionStackOperations._resolutionStack.Skip(index + 2).OfType<ProgramSkillFrame>().Where(f => f.OffTurnUsedCardGift is not null))
        {
            if (OffTurnUsedCardGiftObserverRoot(root.Id)is null)
                continue;
            var end = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
            var okay = true;
            for (var edge = index + 2; edge <= end; edge++)
                if (!_coreHalfHandPhaseDebtDamageObserversOperations.HalfHandPaidDamageObserverEdge(edge))
                {
                    okay = false;
                    break;
                }

            if (okay)
                return true;
        }

        return false;
    }
}
