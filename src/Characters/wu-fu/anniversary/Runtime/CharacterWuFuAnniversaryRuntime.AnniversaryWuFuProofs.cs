#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterWuFuAnniversaryRuntime
{
    internal bool ValidWuFu(ProgramSkillFrame f)
    {
        if (f.AnniversaryWuFu is not { } r || f.SkillId != AnniversaryWuFuComposition.Zhonge || f.ActivationId != "adjust-and-commit" || f.TriggerId is not null || f.InstructionIndex != 1 || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || r.Source != WuFuSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveAnniversaryWuFuZhonge || r.Phase.Kind != ActualDiscardRecoveryPhaseKind.Play || r.Phase.ActorSeat != f.OwnerSeat || !_coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(r.Phase) || !Enum.IsDefined(r.Stage) || r.Count < 0 || r.Cursor < 0 || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Distinct().Count() != r.Cards.Count || r.Cards.Any(id => id <= 0) || r.Cards.Count > r.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuStartedEvent>().Count(e => e == new AnniversaryWuFuStartedEvent(f.Id, r.Source, r.Hash, r.Phase)) != 1)
            return false;
        if (r.Votes.Count > 2 || r.Votes.Where((v, i) => v.Seat != (i == 0 ? f.OwnerSeat : r.Peer)).Any())
            return false;
        if (r.Stage is AnniversaryWuFuStage.AdjustDiscard or AnniversaryWuFuStage.AdjustDraw or AnniversaryWuFuStage.AdjustChildren or AnniversaryWuFuStage.Target)
        {
            if (r.Peer != -1 || r.Votes.Count != 0 || r.Cursor != 0)
                return false;
        }
        else if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Peer) || r.Peer == f.OwnerSeat)
            return false;
        if (r.Stage == AnniversaryWuFuStage.Commit)
            return r.Votes.Count < 2 && f.PendingMovementContinuation is null;
        if (r.Stage is AnniversaryWuFuStage.Item or AnniversaryWuFuStage.Drawing or AnniversaryWuFuStage.Children or AnniversaryWuFuStage.Issuing or AnniversaryWuFuStage.NativeUse or AnniversaryWuFuStage.NativeChildren)
        {
            if (r.Votes.Count != 2 || r.Cursor > WuFuTasks(f).Length)
                return false;
            var reveal = _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuVoteRevealedEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
            if (reveal.Length != 2 || reveal.Where((e, i) => e.Index != i || e.Seat != r.Votes[i].Seat || e.Draw != r.Votes[i].Draw).Any())
                return false;
            if (r.Stage != AnniversaryWuFuStage.Item && r.Cursor >= WuFuTasks(f).Length)
                return false;
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuItemReturnedEvent>().Count(e => e.FrameId == f.Id) != r.Cursor)
                return false;
        }

        if (r.Stage is AnniversaryWuFuStage.AdjustChildren or AnniversaryWuFuStage.Children or AnniversaryWuFuStage.NativeChildren)
        {
            if (f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } p || p.SubjectSeat != f.OwnerSeat)
                return false;
            if (r.Stage == AnniversaryWuFuStage.NativeChildren)
                return r.Returned && r.UseId > 0 && _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuSlashReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.UseId == r.UseId && e.ActionId == r.ActionId) == 1;
            if (r.Stage == AnniversaryWuFuStage.AdjustChildren)
            {
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuAdjustedEvent>().Count(e => e.FrameId == f.Id && e.Requested == r.Count && e.Before == r.Before && e.After == r.After && e.Draw == (r.Draw is not null)) != 1)
                    return false;
                if (r.Draw is { } adjust)
                    return adjust.RecipientSeat == f.OwnerSeat && adjust.RequestedCount == r.Count && adjust.SequenceBefore == r.Before && adjust.SequenceAfter == r.After;
                return r.Cards.Count == r.Count && r.Cards.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == id && m.From == CardLocation.Hand(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == WuFuDiscardReason && m.Sequence > r.Before && m.Sequence <= r.After) == 1);
            }

            return r.Draw is { } d && WuFuTask(f).Draw && d.RecipientSeat == WuFuTask(f).Actor && d.RequestedCount == 1 && d.SequenceBefore == r.Before && d.SequenceAfter == r.After && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryWuFuDrawReturnedEvent>().Count(e => e.FrameId == f.Id && e.Cursor == r.Cursor && e.Recipient == d.RecipientSeat && e.Requested == 1 && e.Actual == d.ActualDrawIds.Count && e.Replacement == d.ReplacementGainedIds.Count && e.AttemptId == d.AttemptFrameId) == 1;
        }

        return f.PendingMovementContinuation is null;
    }

    internal bool HasAnniversaryWuFuPrompt() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { AnniversaryWuFu.Stage: AnniversaryWuFuStage.Target or AnniversaryWuFuStage.Commit or AnniversaryWuFuStage.AdjustDiscard, Step: ResolutionFrameStep.AwaitingResponse } f && p.PlayerSeat == WuFuChooser(f) && p.TargetSeat == WuFuChooser(f) && ValidWuFu(f) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, WuFuChoices(f));
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 46600)]
    internal void AssertWuFu(ProgramSkillFrame f)
    {
        if (f.AnniversaryWuFu is null)
            return;
        if (!ValidWuFu(f))
            throw new InvalidOperationException("Zhonge lost its original actual phase, native payment or simultaneous commitments.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && _corePendingDecisionPolicyPipelineOperations._pendingDecision is not null && f.AnniversaryWuFu.Stage is AnniversaryWuFuStage.Target or AnniversaryWuFuStage.Commit or AnniversaryWuFuStage.AdjustDiscard && !HasAnniversaryWuFuPrompt())
            throw new InvalidOperationException("Zhonge's original private participant choice changed.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 46600)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 46600)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 46600)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 46600)]
    internal bool WuFuMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryWuFu is { Stage: AnniversaryWuFuStage.AdjustChildren or AnniversaryWuFuStage.Children or AnniversaryWuFuStage.NativeChildren } && e?.Op == SkillProgramEffectOp.ResolveAnniversaryWuFuZhonge && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidWuFu(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 46600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 46600)]
    internal bool RetainWuFu(ProgramSkillFrame f) => f.AnniversaryWuFu is not null && ValidWuFu(f);
    internal bool WuFuFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidWuFu(f) || f.AnniversaryWuFu is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryWuFuStage.NativeUse && IsWuFuUse(use);
        if (global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return r.Stage is AnniversaryWuFuStage.AdjustDraw or AnniversaryWuFuStage.Drawing && f.NativeDraw is not null;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && WuFuMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && (r.Stage == AnniversaryWuFuStage.NativeChildren ? IsAnniversaryWuFuSettlementBatch(f, moved.Batch) : moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool WuFuNativeEdge(int i)
    {
        if (_resolutionStackOperations._resolutionStack[i - 1] is CardUseFrame { AnniversaryWuFuSlash: not null } use && IsWuFuUse(use) && _resolutionStackOperations._resolutionStack[i] is ProgramCardTriggerWindowFrame w)
            return w.ParentFrameId == use.Id && w.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[i - 1] is ProgramCardTriggerWindowFrame parent && _coreCardUseLifecycleOperations.LifecycleCardUse(parent.ParentFrameId)is { AnniversaryWuFuSlash: not null } native && IsWuFuUse(native) && _resolutionStackOperations._resolutionStack[i] is ProgramSkillFrame f && parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count)
        {
            var c = parent.Candidates[parent.CandidateIndex];
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(c)) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext(parent, c);
        }

        return false;
    }

    internal bool WuFuObserverEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || WuFuNativeEdge(i) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && WuFuFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal ProgramSkillFrame? WuFuObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterWuFuAnniversaryOperations.WuFuFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (n) => _characterWuFuAnniversaryOperations.WuFuObserverEdge(n), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 46600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 46600)]
    internal bool WuFuDamageObserver(long id) => WuFuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 46600)]
    internal bool WuFuBeforeDamageObserver(long id) => WuFuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 46600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 46600)]
    internal bool WuFuDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && WuFuObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46600)]
    internal bool AdvanceWuFuChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && WuFuObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
