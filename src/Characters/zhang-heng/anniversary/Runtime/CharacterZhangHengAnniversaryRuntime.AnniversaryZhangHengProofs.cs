#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangHengAnniversaryRuntime
{
    internal bool ZhangHengOriginal(ProgramSkillFrame f)
    {
        if (f.AnniversaryZhangHeng is not { } r || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting } w || w.ParentFrameId != r.ParentId || w.OwnerSeat != f.OwnerSeat || w.SourceSeat != f.OwnerSeat || w.TargetSeat != f.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == r.ParentId)is not PlayPhaseStartingBoundaryFrame parent || parent.OwnerSeat != f.OwnerSeat || parent.ItemIndex < 0 || parent.ItemIndex >= parent.Items.Count || parent.Items[parent.ItemIndex].Candidate is not { } candidate)
            return false;
        return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate) && w.OccurrenceIndex == candidate.OccurrenceIndex;
    }

    internal bool ValidZhangHeng(ProgramSkillFrame f)
    {
        if (f.AnniversaryZhangHeng is not { } r || f.SkillId != AnniversaryZhangHengComposition.Dangzai || f.TriggerId != "take-delayed-subset" || f.ActivationId != f.TriggerId || f.InstructionIndex != 1 || r.Source != ZhangHengSource(f) || r.Hash != f.GameplayHash || r.Turn != _coreActualTurnProgressTurnNumberAccess._turnNumber || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || !ZhangHengOriginal(f) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveAnniversaryZhangHengDangzai || r.Stage is not ("donor" or "cards" or "children") || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Distinct().Count() != r.Cards.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHengStartedEvent>().Count(e => e == new AnniversaryZhangHengStartedEvent(f.Id, r.Source, r.Hash, r.ParentId, r.Turn)) != 1)
            return false;
        if (r.Stage == "donor")
            return r.Donor == -1 && r.Cards.Count == 0 && f.PendingMovementContinuation is null;
        if (!_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Donor) || r.Donor == f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHengDonorEvent>().Count(e => e == new AnniversaryZhangHengDonorEvent(f.Id, r.Donor)) != 1)
            return false;
        if (r.Stage == "cards")
            return f.PendingMovementContinuation is null && r.Cards.All(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Judgment(r.Donor));
        return r.Cards.Count > 0 && f.PendingMovementContinuation is { BeforeCount: 0, CoverageResultBind: null } p && p.SubjectSeat == f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryZhangHengMovedEvent>().Count(e => e == new AnniversaryZhangHengMovedEvent(f.Id, r.Donor, r.Cards.Count, r.Before, r.After)) == 1 && r.Cards.All(id => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == CardLocation.Judgment(r.Donor) && m.To == CardLocation.Judgment(f.OwnerSeat) && m.Reason == ZhangHengMoveReason) == 1);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 49600)]
    internal void AssertZhangHeng(ProgramSkillFrame f)
    {
        if (f.AnniversaryZhangHeng is not null && !ValidZhangHeng(f))
            throw new InvalidOperationException("Dangzai lost its original Play boundary, legal physical subset or once-paid native transfer.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 49600)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 49600)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 49600)]
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 49600)]
    internal bool ZhangHengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.AnniversaryZhangHeng is { Stage: "children" } && e?.Op == SkillProgramEffectOp.ResolveAnniversaryZhangHengDangzai && p == f.PendingMovementContinuation && ValidZhangHeng(f);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 49600)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 49600)]
    internal bool RetainZhangHeng(ProgramSkillFrame f) => f.AnniversaryZhangHeng is { Stage: "children" } && ValidZhangHeng(f);
    internal bool ZhangHengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.AnniversaryZhangHeng is not { Stage: "children" } r || !ValidZhangHeng(f))
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && ZhangHengMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && r.Cards.Contains(m.CardId) && m.Reason == ZhangHengMoveReason);
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool ZhangHengObserverEdge(int i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations._resolutionStack[i - 1], _resolutionStackOperations._resolutionStack[i]) || _resolutionStackOperations._resolutionStack[i - 1] is ProgramSkillFrame f && ZhangHengFirstChild(f, _resolutionStackOperations._resolutionStack[i]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations._resolutionStack[i], _resolutionStackOperations._resolutionStack[i - 1]);
    internal ProgramSkillFrame? ZhangHengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhangHengAnniversaryOperations.ZhangHengFirstChild(f, c), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterZhangHengAnniversaryOperations.ZhangHengObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 49600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 49600)]
    internal bool ZhangHengDamageObserver(long id) => ZhangHengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 49600)]
    internal bool ZhangHengBeforeDamageObserver(long id) => ZhangHengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 49600)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49600)]
    internal bool ZhangHengDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ZhangHengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 49600)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 49600)]
    internal bool ZhangHengRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryZhangHeng is null)
            return false;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var j = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return i >= 0 && j > i && ZhangHengFirstChild(f, _resolutionStackOperations._resolutionStack[i + 1]) && (j == i + 1 || _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[j - 1]);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49600)]
    internal bool AdvanceZhangHengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhangHengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
