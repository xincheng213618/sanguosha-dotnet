#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal bool ZhaoZhongWindowMatches(ProgramSkillFrame f)
    {
        if (f.ZhaoZhong is not { } r)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain)
            return f.TriggerId is null && f.WindowContext is null && f.ActivationId == "give-recover-chain" && f.SelectedTargetSeats.SequenceEqual([r.TargetSeat]) && r.Phase is { Kind: ActualDiscardRecoveryPhaseKind.Play } p && _coreActualDiscardRecoveryPhasesOperations.IsRecordedActualDiscardRecoveryPhase(p) && p.ActorSeat == f.OwnerSeat;
        if (f.WindowContext is not { } c || c.ParentFrameId != r.WindowId || c.OwnerSeat != f.OwnerSeat)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId!, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (r.Op == SkillProgramEffectOp.ResolveBiriFirstNonDrawGain)
            return c.Window == SkillProgramTriggerWindow.CardsGained && c.MovementBatch is { } batch && batch.Id == r.WindowId && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is CardsMovedTriggerWindowFrame w && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] == candidate && _coreProgramEventHistoryOperations.ProgramEventHistory<BiriPotentialHolderEvent>().Count(e => e.BatchId == r.WindowId && e.Source == r.Source && e.GameplayHash == r.GameplayHash) == 1;
        if (_resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == r.WindowId)is not ProgramLifecycleTriggerWindowFrame window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || c.Window != window.Window)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolvePengbiFirstActualTurn)
            return window.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && window.OwnerSeat == f.OwnerSeat && r.TurnOwner == f.OwnerSeat && FirstPengbiActualTurn(f.OwnerSeat) == r.ActualTurn;
        return r.Op == SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice && window.Window == SkillProgramTriggerWindow.CharacterEnteredChain && DiciCandidateLease(candidate)is { } lease && lease.LeaseId == r.LeaseId && lease.TargetSeat == r.TargetSeat && window.OwnerSeat == r.TargetSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<DiciLeaseDueEvent>().Count(e => e.LeaseId == r.LeaseId && e.ChangeId == r.WindowId && e.TargetSeat == r.TargetSeat) == 1;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28110)]
    internal void AssertZhaoZhong(ProgramSkillFrame f)
    {
        if (f.ZhaoZhong is not { } r)
            return;
        if (r.Source != ZhaoZhongSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.InstructionIndex is < 0 or > 1 || f.InstructionIndex == 1 && _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || !ZhaoZhongComposition.Handles(r.Op) || !Enum.IsDefined(r.Stage) || !ZhaoZhongWindowMatches(f) || r.Neighbors is not System.Collections.IList { IsReadOnly: true } || r.Neighbors.Distinct().Count() != r.Neighbors.Count || r.Neighbors.Any(s => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(s) || s == r.TargetSeat) || r.Cursor < 0 || r.Cursor > r.Neighbors.Count || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhaoZhongStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Op == r.Op && e.WindowId == r.WindowId && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner) != 1)
            throw new InvalidOperationException("Zhao Zhong lost his immutable source, owning native window, paid stage or frozen queue.");
        if (r.Op == SkillProgramEffectOp.ResolvePengbiFirstActualTurn)
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<PengbiCommittedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Paid ? paid is not [var receipt] || receipt.Source != r.Source || receipt.GameplayHash != r.GameplayHash || receipt.SkillId != r.ChosenSkill || _coreProgramEventHistoryOperations.ProgramEventHistory<PengbiCommittedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat) != 1 : paid.Length != 0)
                throw new InvalidOperationException("Pengbi changed or repeated the first actual-turn skill choice.");
            foreach (var grant in _coreProgramEventHistoryOperations.ProgramEventHistory<PengbiSkillGrantedEvent>().Where(e => e.FrameId == f.Id))
                if (grant.SkillId is not ("ol:yintian" or "ol:biri") || grant.GrantId != $"zhao-zhong.pengbi:{(f.Id)}:{(grant.RecipientSeat)}:{(grant.SkillId)}" || grant.SkillInstanceId != grant.GrantId || grant.SourceId != $"zhao-zhong.pengbi:{(f.Id)}:{(grant.RecipientSeat)}")
                    throw new InvalidOperationException("Pengbi lost the exact permanent independent grant source.");
        }

        if (r.Op == SkillProgramEffectOp.ResolveDiciGiftRecoveryAndNeighborChain)
        {
            var paid = _coreProgramEventHistoryOperations.ProgramEventHistory<DiciAcceptedEvent>().Where(e => e.FrameId == f.Id).ToArray();
            if (r.Paid ? paid is not [var invoice] || invoice.Source != r.Source || invoice.GameplayHash != r.GameplayHash || invoice.TargetSeat != r.TargetSeat || invoice.Phase != r.Phase || _coreProgramEventHistoryOperations.ProgramEventHistory<DiciAcceptedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.Phase == r.Phase) != 1 : paid.Length != 0)
                throw new InvalidOperationException("Dici repeated its actual Play payment or replaced its original donor.");
            if (r.Neighbors.Count > 0 && (_coreProgramEventHistoryOperations.ProgramEventHistory<DiciNeighborsFrozenEvent>().Count(e => e.FrameId == f.Id && e.Count == r.Neighbors.Count) != 1 || !_coreProgramEventHistoryOperations.ProgramEventHistory<DiciNeighborFrozenEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).Select(e => e.Seat).SequenceEqual(r.Neighbors)))
                throw new InvalidOperationException("Dici changed its living-seat topology after freezing the neighbor queue.");
        }

        if (r.Op == SkillProgramEffectOp.ResolveIssuedDiciNextChainChoice && (!r.Paid || _coreProgramEventHistoryOperations.ProgramEventHistory<DiciLeaseStartedEvent>().Count(e => e.LeaseId == r.LeaseId && e.ChangeId == r.WindowId && e.ProgramFrameId == f.Id) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<DiciLeaseStartedEvent>().Count(e => e.LeaseId == r.LeaseId) != 1))
            throw new InvalidOperationException("Dici repeated or replaced its once-issued next-chain entitlement.");
        if (r.Op == SkillProgramEffectOp.ResolveBiriFirstNonDrawGain && (!r.Paid || _coreProgramEventHistoryOperations.ProgramEventHistory<BiriDrawIssuedEvent>().Count(e => e.FrameId == f.Id && e.BatchId == r.WindowId && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BiriDrawIssuedEvent>().Count(e => e.Source.OwnerSeat == f.OwnerSeat && e.ActualTurn == r.ActualTurn && e.TurnOwner == r.TurnOwner) != 1))
            throw new InvalidOperationException("Biri changed or repeated the first global non-draw gain's native draw.");
        if (r.Stage is ZhaoZhongStage.GiftChildren or ZhaoZhongStage.NextGiftChildren)
        {
            if (r.CardId is not { } id || r.CardKind is not { } kind || r.From != CardLocation.Hand(r.TargetSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<DiciGiftEntityEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var gift] || gift.CardId != id || gift.Kind != kind || gift.From != r.From || gift.To != CardLocation.Hand(f.OwnerSeat) || gift.Sequence <= r.Before || gift.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == gift.Sequence && m.CardId == gift.CardId && m.From == gift.From && m.To == gift.To && m.Reason.Value == (r.Stage == ZhaoZhongStage.NextGiftChildren ? DiciNextGiveReason : DiciGiveReason)))
                throw new InvalidOperationException("Dici lost its one actual Hand gift and immutable physical payment.");
        }

        if (f.PendingMovementContinuation is { } pending && !IsZhaoZhongMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Zhao Zhong lost his typed movement return.");
    }

    internal bool CanContinueIssuedZhaoZhong(ProgramSkillFrame f)
    {
        if (f.ZhaoZhong is { Paid: true })
        {
            AssertZhaoZhong(f);
            return true;
        }

        if (f.ZhaoZhong is not null || f.WindowContext is not { } c || f.TriggerId is null)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        if (IsBiriTrigger(_coreProgramLifecycleOperations.GetProgramTrigger(f)))
            return IsIssuedBiriCandidate(candidate, c);
        return f.TriggerId == "issued-next-chain" && IsIssuedDiciChainCandidate(candidate, c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28110)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28110)]
    internal bool PermitIssuedZhaoZhong(ProgramSkillFrame f) => CanContinueIssuedZhaoZhong(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 170)]
    internal bool IsZhaoZhongMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.ZhaoZhong is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is ZhaoZhongStage.GiftChildren or ZhaoZhongStage.RecoveryChildren or ZhaoZhongStage.NextGiftChildren or ZhaoZhongStage.DrawChildren;
    internal bool ZhaoZhongPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.ZhaoZhong!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (_coreProgramEventHistoryOperations.ProgramEventHistory<DiciGiftEntityEvent>().Any(e => e.FrameId == f.Id && e.LeaseId == r.LeaseId && e.CardId == m.CardId && e.Kind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence))
            return true;
        return r.DrawOutcome is { } o && m.Sequence > o.SequenceBefore && m.Sequence <= o.SequenceAfter && (o.ActualDrawIds.Contains(m.CardId) && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BiriDrawReason || o.ReplacementGainedIds.Contains(m.CardId) && m.To == CardLocation.Hand(f.OwnerSeat) || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 330)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 320)]
    internal bool IsZhaoZhongRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => f.ZhaoZhong is { Paid: true, Stage: ZhaoZhongStage.RecoveryChildren } r && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f) && child.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && child.Attempt.SourceSeat == f.OwnerSeat && child.Attempt.TargetSeat == r.TargetSeat && child.Attempt.Amount == 1 && child.Attempt.Completion.Producer == RecoveryAttemptProducer.Program && child.Attempt.Completion.InstructionIndex == f.InstructionIndex;
    internal bool ZhaoZhongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhaoZhong is not { Paid: true } r)
            return false;
        AssertZhaoZhong(f);
        if (r.Stage == ZhaoZhongStage.Drawing && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is RecoveryReplacementFrame recovery)
            return IsZhaoZhongRecoveryParent(f, recovery);
        if (child is DamageFrame damage)
            return r.Stage == ZhaoZhongStage.NextDamageChildren && damage.ParentFrameId == f.Id && f.AttackAttempt is { } a && damage.SourceSeat == f.OwnerSeat && damage.TargetSeat == a.TargetSeat && damage.Amount == a.DamageAmount;
        if (child is BeforeDamageProgramWindowFrame before)
            return r.Stage == ZhaoZhongStage.NextDamageChildren && (before.ContinuationAttackResolutionId ?? before.ParentFrameId) == f.Id && f.AttackAttempt is not null;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => ZhaoZhongPaidMovement(f, m));
    }

    internal bool ZhaoZhongLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame w && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } c && c.ParentFrameId == w.Id && c.Window == w.Window;
    internal bool ZhaoZhongObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterZhaoZhongOlOperations.ZhaoZhongLifecycleEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: not null } z && _characterSunHongOlOperations.ZenrunFirstChild(z, _resolutionStackOperations2._resolutionStack()[n]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations2.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? ZhaoZhongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterZhaoZhongOlOperations.ZhaoZhongFirstChild(f, child), (i) => _characterZhaoZhongOlOperations.ZhaoZhongObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1100)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1210)]
    internal bool HasZhaoZhongDamageObserver(long id) => ZhaoZhongObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1240)]
    internal bool HasZhaoZhongBeforeDamageObserver(long id) => ZhaoZhongObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1070)]
    internal bool IsZhaoZhongDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && ZhaoZhongObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28110)]
    internal bool AdvanceZhaoZhongChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhaoZhongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
