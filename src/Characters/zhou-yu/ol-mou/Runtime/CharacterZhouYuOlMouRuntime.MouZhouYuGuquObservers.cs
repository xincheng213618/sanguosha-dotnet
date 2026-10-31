#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuOlMouRuntime
{
    internal bool MouGuquRootMatches(ProgramSkillFrame f)
    {
        if (f.Guqu is not { } r || f.SkillId != MouGuquSkill || f.ActivationId != MouGuquBinding || f.TriggerId != MouGuquBinding || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Claim.Source != MouGuquFrameSource(f) || r.Claim.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveMouZhouYuGuqu || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || context.Window != SkillProgramTriggerWindow.CardUseCompleted || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramCardTriggerWindowFrame w || w.ParentFrameId != r.Claim.ParentFrameId || !ValidMouGuquClaim(r.Claim, w.Action) || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)?.GuquClaims?.Contains(r.Claim) != true || !_coreDesignatedExtraTargetsOperations.DesignatedExtraTargetCandidateMatches(new(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, context.OccurrenceIndex), context, w))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1].Id == r.WindowId && _coreProgramEventHistoryOperations.ProgramEventHistory<GuquStartedEvent>().Count(e => e == new GuquStartedEvent(f.Id, r.Claim, r.WindowId)) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<GuquStartedEvent>().Count(e => e.Claim.Source == r.Claim.Source && e.Claim.ActionId == r.Claim.ActionId) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 29021)]
    internal void AssertMouGuqu(ProgramSkillFrame f)
    {
        if (f.Guqu is not { } r)
            return;
        if (!MouGuquRootMatches(f) || !Enum.IsDefined(r.Stage))
            throw new InvalidOperationException("Guqu lost its exact original-source marked materials, native completed Use or paid draw entitlement.");
        var returned = _coreProgramEventHistoryOperations.ProgramEventHistory<GuquDrawReturnedEvent>().Where(e => e.FrameId == f.Id).ToArray();
        if (r.Stage == GuquStage.Drawing)
        {
            if (r.DrawOutcome is not null || returned.Length != 0 || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Guqu cannot reuse a returned native draw as a fresh request.");
            return;
        }

        if (r.DrawOutcome is not { } o || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != 2 || o.SequenceBefore < r.Claim.MovementSequence || o.SequenceAfter < o.SequenceBefore || o.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || f.NativeDraw is not null || f.PendingMovementContinuation is not { } p || !IsMouGuquMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) || returned is not [var fact] || fact != new GuquDrawReturnedEvent(f.Id, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId))
            throw new InvalidOperationException("Guqu lost its once-returned actual/replacement native draw invoice or unfinished children.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 29021)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 29021)]
    internal bool PermitPaidMouGuqu(ProgramSkillFrame f)
    {
        if (f.Guqu is not null)
        {
            AssertMouGuqu(f);
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId == MouGuquBinding && f.WindowContext is { } c && IsIssuedMouGuquCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0, c.OccurrenceIndex), c);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 320)]
    internal bool IsMouGuquMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.Guqu is { Stage: GuquStage.DrawChildren, DrawOutcome: not null } && effect?.Op == SkillProgramEffectOp.ResolveMouZhouYuGuqu && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool MouGuquDrawMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        if (f.Guqu is not { Stage: GuquStage.DrawChildren, DrawOutcome: { } o } || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= o.SequenceBefore || m.Sequence > o.SequenceAfter)
            return false;
        return o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == MouGuquDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
    }

    internal bool MouGuquFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Guqu is not { } r || !MouGuquRootMatches(f))
            return false;
        if (r.Stage == GuquStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => MouGuquDrawMovement(f, m));
    }

    internal bool MouGuquObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame p && (_characterZhouYuOlMouOperations.MouGuquFirstChild(p, _resolutionStackOperations2._resolutionStack()[n]) || p.Zenrun is not null && _characterSunHongOlOperations2.ZenrunFirstChild(p, _resolutionStackOperations2._resolutionStack()[n])) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, dying));
    internal ProgramSkillFrame? MouGuquObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhouYuOlMouOperations.MouGuquFirstChild(f, child), (index) => _characterZhouYuOlMouOperations.MouGuquObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1300)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1410)]
    internal bool HasMouGuquDamageObserver(long id) => MouGuquObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1440)]
    internal bool HasMouGuquBeforeDamageObserver(long id) => MouGuquObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1270)]
    internal bool IsMouGuquDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && MouGuquObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 470)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 460)]
    internal bool IsMouGuquRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.Guqu is null || recovery.ParentFrameId == f.Id)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent + 1 && MouGuquFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && MouGuquObserverSuffix(parent) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 29021)]
    internal bool AdvanceMouGuquChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && MouGuquObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
