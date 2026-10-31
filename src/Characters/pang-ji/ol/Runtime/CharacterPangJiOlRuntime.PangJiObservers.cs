#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPangJiOlRuntime
{

    internal bool CanContinueIssuedPangJi(ProgramSkillFrame f)
    {
        if (CanContinueIssuedDouyu(f))
            return true;
        if (f.PangJi is { Paid: true })
        {
            return true;
        }

        return f.PangJi is null && f.TriggerId == "armed-target-pindian" && f.WindowContext is { } c && IsIssuedBiguoCandidate(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex), c);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28130)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28130)]
    internal bool PermitIssuedPangJi(ProgramSkillFrame f) => CanContinueIssuedPangJi(f);
    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 190)]
    internal bool IsPangJiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => IsDouyuMovement(f, e, p) || f.PangJi is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is PangJiStage.CostChildren or PangJiStage.ObtainChildren or PangJiStage.PindianChildren;
    internal bool PangJiPaidMovement(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.PangJi!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == PangJiStage.CostChildren)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoCostPaidEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.PrintedKind == m.CardKind && e.From == m.From && e.To == m.To && e.Reason == m.Reason && e.Sequence == m.Sequence) || r.Cost is { CardKind: CardKind.WoodenOx, From.Zone: CardZoneKind.Equipment } && m.From == CardLocation.WoodenOxGrain(f.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.WoodenOxGrainDiscard;
        if (r.Stage == PangJiStage.ObtainChildren)
            return _coreProgramEventHistoryOperations.ProgramEventHistory<BiguoObtainEntityPaidEvent>().Any(e => e.FrameId == f.Id && e.CardId == m.CardId && e.PrintedKind == m.CardKind && e.From == m.From && e.To == m.To && e.Sequence == m.Sequence) && m.Reason.Value == BiguoObtainReason;
        return r.Stage == PangJiStage.PindianChildren && r.Result is { } result && (m.CardId == result.SourceCardId || m.CardId == result.OpponentCardId);
    }

    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 350)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 340)]
    internal bool IsPangJiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame child) => IsDouyuRecoveryParent(f, child) || f.PangJi is { Paid: true, Stage: PangJiStage.CostChildren, Cost: { CardKind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } } && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(child, f) && child.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && child.Attempt.SourceSeat == f.OwnerSeat && child.Attempt.TargetSeat == f.OwnerSeat && child.Attempt.Amount == 1 && child.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && child.Attempt.Completion.MoveReason?.Value == BiguoCostReason;
    internal bool IsBiguoPindianSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch)
    {
        if (f.PangJi is not { Paid: true, Stage: PangJiStage.PindianChildren, Result: { } result } r || r.Op != SkillProgramEffectOp.ResolveBiguoTargetedPindianAndCategoryBan || batch.ParentFrameId != r.PindianId || batch.AwaitingProgramFrameId is not null || batch.TurnNumber != r.ActualTurn || batch.OriginOwnerSeat != f.OwnerSeat || batch.OriginSkillId != f.SkillId || batch.OriginSkillInstanceId != f.SkillInstanceId || batch.Movements.Count is < 1 or > 2 || batch.Movements.Select(m => m.Sequence).Distinct().Count() != batch.Movements.Count || batch.Movements.Select(m => m.CardId).Distinct().Count() != batch.Movements.Count || f.PendingMovementContinuation is not { } pending || !IsPangJiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            return false;
        return batch.Movements.All(m => m.TurnNumber == r.ActualTurn && m.Sequence > r.Before && m.Sequence <= r.After && (m.CardId == result.SourceCardId || m.CardId == result.OpponentCardId) && _coreAdvancedLifecycleOperations.GetAdvancedCard(m.CardId).Kind == m.CardKind && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(actual => actual.Sequence == m.Sequence && actual == m) == 1 && (m.To == CardLocation.Processing && (m.Reason == CardMoveReasons.PindianReveal && (m.From == CardLocation.Hand(result.SourceSeat) || m.From == CardLocation.Hand(result.OpponentSeat) || m.From == CardLocation.DrawPile && _coreProgramEventHistoryOperations.ProgramEventHistory<PindianTopSourceChosenEvent>().Any(e => e.CardId == m.CardId && (e.OwnerSeat == result.SourceSeat || e.OwnerSeat == result.OpponentSeat))) || m.From == CardLocation.DrawPile && m.Reason.Value == "pindian.reserve-top" && _coreProgramEventHistoryOperations.ProgramEventHistory<PindianTopSourceChosenEvent>().Any(e => e.CardId == m.CardId && (e.OwnerSeat == result.SourceSeat || e.OwnerSeat == result.OpponentSeat))) || m.From == CardLocation.Processing && (m.To == CardLocation.DiscardPile && m.Reason == CardMoveReasons.PindianFinish || m.Reason.Value == "program.pindian.claim" && (m.To == CardLocation.Hand(result.SourceSeat) || m.To == CardLocation.Hand(result.OpponentSeat)))));
    }

    internal bool IsBiguoPindianSettlementReturn(ProgramSkillFrame f, CardsMovedTriggerWindowFrame moved) => moved.Id == moved.Batch.Id && moved.ResumeProgramFrameId == f.Id && !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.HasAlternateProgramMovementReturn(moved) && IsBiguoPindianSettlementBatch(f, moved.Batch);
    internal bool PangJiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.PangJi is not { Paid: true } r)
            return false;
        if (child is PindianFrame p)
            return r.Stage == PangJiStage.Pindian && p.ParentFrameId == f.Id && IsBiguoPindianFrame(p);
        if (child is RecoveryReplacementFrame recovery)
            return IsPangJiRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.Window == state.Window);
        return child is CardsMovedTriggerWindowFrame moved && (IsBiguoPindianSettlementReturn(f, moved) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.ParentFrameId == f.Id && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => PangJiPaidMovement(f, m)));
    }

    internal bool PangJiLifecycleEdge(int index) => _resolutionStackOperations._resolutionStack[index - 1] is ProgramLifecycleTriggerWindowFrame w && _resolutionStackOperations._resolutionStack[index] is ProgramSkillFrame f && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]) && f.WindowContext is { } c && c.ParentFrameId == w.Id && c.Window == w.Window;
    internal bool PangJiObserverSuffix(int index) => _coreObserverSubtreesOperations.HasObserverSuffix(index, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _characterPangJiOlOperations.PangJiLifecycleEdge(n) || _characterPangJiOlOperations.DouyuPindianChildRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame { Zenrun: not null } z && _characterSunHongOlOperations.ZenrunFirstChild(z, _resolutionStackOperations2._resolutionStack()[n]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d));
    internal ProgramSkillFrame? PangJiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterPangJiOlOperations.PangJiFirstChild(f, child), (n) => _characterPangJiOlOperations.PangJiObserverSuffix(n));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1120)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1230)]
    internal bool HasPangJiDamageObserver(long id) => HasDouyuDamageObserver(id) || PangJiObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1260)]
    internal bool HasPangJiBeforeDamageObserver(long id) => HasDouyuBeforeDamageObserver(id) || PangJiObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id && x is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 28130)]
    internal bool AdvancePangJiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && PangJiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 2300)]
    internal bool WindowSettlement_IsBiguoPindianSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsBiguoPindianSettlementBatch(frame, batch);
}
