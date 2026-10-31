#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterQiaoRuiAnniversaryRuntime
{
    internal bool QiaoRuiOriginal(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || !_coreOriginalHandEntityObserversOperations.OriginalHandBindingStarted(f, w.Window))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != w.ParentFrameId)
            return false;
        if (op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw)
            return f.SkillId == AnniversaryQiaoRuiComposition.Aishou && f.TriggerId == "ending-marked-draw" && w.Window == SkillProgramTriggerWindow.TurnEnding && w.SourceSeat == f.OwnerSeat && w.TargetSeat == f.OwnerSeat && _resolutionStackOperations._resolutionStack[index - 1] is TurnEndingBoundaryFrame ending && ending.OwnerSeat == f.OwnerSeat && ending.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && ending.ItemIndex >= 0 && ending.ItemIndex < ending.Items.Count && ending.Items[ending.ItemIndex].Candidate is { } candidate && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, candidate);
        if (op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouPrepare)
            return f.SkillId == AnniversaryQiaoRuiComposition.Aishou && f.TriggerId == "prepare-marked-discard" && _coreOriginalHandEntityObserversOperations.ExactOriginalHandLifecycleParent(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow);
        if (op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouLastLoss)
            return f.SkillId == AnniversaryQiaoRuiComposition.Aishou && f.TriggerId == "outside-last-mark-loss" && w.Window == SkillProgramTriggerWindow.CardsMoved && w.MovementBatch is { } b && _resolutionStackOperations._resolutionStack[index - 1] is CardsMovedTriggerWindowFrame moved && moved.Batch.Id == b.Id && moved.Batch.Movements.SequenceEqual(b.Movements) && moved.CandidateIndex >= 0 && moved.CandidateIndex < moved.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, moved.Candidates[moved.CandidateIndex]);
        return op == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiSaowei && f.SkillId == AnniversaryQiaoRuiComposition.Saowei && f.TriggerId == "completed-other-slash" && QiaoRuiOriginalSlash(w)is { Action: { } a } original && a.ActorSeat != f.OwnerSeat && _resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame card && card.ParentFrameId == original.Id && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(card.Candidates[card.CandidateIndex]));
    }

    internal bool ValidQiaoRui(ProgramSkillFrame f)
    {
        if (f.AnniversaryQiaoRui is not { } r || f.InstructionIndex != 1 || !QiaoRuiOriginal(f, r.Operation) || r.Source != QiaoRuiSource(f) || r.Hash != f.GameplayHash || r.ParentId != f.WindowContext!.ParentFrameId || r.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Requested < 0 || r.Cards.Any(id => id <= 0) || r.Cards.Distinct().Count() != r.Cards.Count || r.NativeBatches.Distinct().Count() != r.NativeBatches.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiStartedEvent>().Count(e => e == new AnniversaryQiaoRuiStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.ParentId, r.Turn)) != 1)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouDraw)
        {
            if (r.Cards.Count != 0 || r.Plan is not null || r.UseId != 0 || r.ActionId != 0 || r.Damaged || r.Returned || r.NativeBatches.Count != 0 || r.Stage is not (AnniversaryQiaoRuiStage.Drawing or AnniversaryQiaoRuiStage.DrawChildren or AnniversaryQiaoRuiStage.Completed))
                return false;
            if (r.Draw is { } d && (d.RecipientSeat != f.OwnerSeat || d.RequestedCount != r.Requested || d.SequenceBefore != r.Before || d.SequenceAfter != r.After || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiDrawReturnedEvent>().Count(e => e == new AnniversaryQiaoRuiDrawReturnedEvent(f.Id, r.Requested, d.ActualDrawIds.Count, r.Before, r.After)) != 1))
                return false;
        }
        else if (r.Operation is SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouPrepare or SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouLastLoss)
        {
            if (r.Draw is not null || r.Requested != 0 || r.Plan is not null || r.UseId != 0 || r.ActionId != 0 || r.Returned || r.Damaged || r.NativeBatches.Count != 0 || r.Stage is not (AnniversaryQiaoRuiStage.DiscardChildren or AnniversaryQiaoRuiStage.HealthChildren or AnniversaryQiaoRuiStage.Completed))
                return false;
            if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryQiaoRuiAishouLastLoss && (r.Cards.Count != 0 || r.Stage == AnniversaryQiaoRuiStage.DiscardChildren))
                return false;
            if (r.Cards.Count > 0)
            {
                var originalMarks = QiaoRuiMarkedHand(f.OwnerSeat, r.Before).ToHashSet();
                if (r.Cards.Any(id => !originalMarks.Contains(id)))
                    return false;
            }
        }
        else
        {
            if (r.Operation != SkillProgramEffectOp.ResolveAnniversaryQiaoRuiSaowei || r.Draw is not null || r.Requested != 0 || r.Cards.Count != 0 || r.Stage is AnniversaryQiaoRuiStage.Drawing or AnniversaryQiaoRuiStage.DrawChildren or AnniversaryQiaoRuiStage.DiscardChildren or AnniversaryQiaoRuiStage.HealthChildren)
                return false;
            if (r.Stage == AnniversaryQiaoRuiStage.SlashChoice && (r.Plan is not null || r.UseId != 0 || r.ActionId != 0 || r.Returned || r.Damaged || r.NativeBatches.Count != 0))
                return false;
            if (r.Plan is { } p && (p.CardId <= 0 || p.From != CardLocation.Hand(f.OwnerSeat) || p.Targets.Count == 0 || p.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t) || t == f.OwnerSeat) || p.Targets.Distinct().Count() != p.Targets.Count || QiaoRuiOriginalSlash(f.WindowContext)is not { Action: { } original } || p.Targets.Any(t => !original.TargetSeats.Contains(t))))
                return false;
            if (r.Stage is AnniversaryQiaoRuiStage.Issuing or AnniversaryQiaoRuiStage.NativeSlash or AnniversaryQiaoRuiStage.NativeTail or AnniversaryQiaoRuiStage.ClaimChildren && r.Plan is null)
                return false;
            if (r.UseId != 0 && (r.UseId <= f.Id || r.ActionId <= 0 || r.Plan is not { } plan || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiUseIssuedEvent>().Count(e => e.Origin == new AnniversaryQiaoRuiUseOrigin(f.Id, r.Source, r.Hash, plan.CardId, plan.From, r.UseId, r.ActionId)) != 1))
                return false;
            if (r.Returned && _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiUseReturnedEvent>().Count(e => e == new AnniversaryQiaoRuiUseReturnedEvent(f.Id, r.UseId, r.ActionId)) != 1)
                return false;
            if (r.Damaged != _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryQiaoRuiDamageEvent>().Any(e => e.FrameId == f.Id && e.UseId == r.UseId && e.Amount > 0))
                return false;
        }

        return true;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 49400)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 49400)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 49400)]
    internal bool QiaoRuiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.AnniversaryQiaoRui is { Stage: AnniversaryQiaoRuiStage.DrawChildren or AnniversaryQiaoRuiStage.DiscardChildren or AnniversaryQiaoRuiStage.HealthChildren or AnniversaryQiaoRuiStage.NativeTail or AnniversaryQiaoRuiStage.ClaimChildren } r && effect?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidQiaoRui(f);
    internal bool QiaoRuiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidQiaoRui(f) || f.AnniversaryQiaoRui is not { } r)
            return false;
        if (child is CardUseFrame use)
            return r.Stage == AnniversaryQiaoRuiStage.NativeSlash && IsQiaoRuiNativeUse(use);
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryQiaoRuiStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == r.Requested && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && QiaoRuiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && (global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || IsAnniversaryQiaoRuiSettlementBatch(f, moved.Batch)) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal ProgramSkillFrame? QiaoRuiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterQiaoRuiAnniversaryOperations.QiaoRuiFirstChild(f, c), (i) => _characterQiaoRuiAnniversaryOperations.QiaoRuiObserverSuffix(i));
    internal bool QiaoRuiNativeEdge(int child)
    {
        if (_resolutionStackOperations._resolutionStack[child - 1] is CardUseFrame { AnniversaryQiaoRuiUse: not null, Step: ResolutionFrameStep.AwaitingResponse, CardAttack: { Active: true } attack } responding && IsQiaoRuiNativeUse(responding) && _resolutionStackOperations._resolutionStack[child] is ResponseWindowFrame { Step: ResolutionFrameStep.AwaitingResponse, RequiredCardKind: CardKind.Dodge } response)
            return response.ParentFrameId == responding.Id && response.SourceSeat == attack.SourceSeat && response.ResponderSeat == attack.TargetSeat && response.IncomingCard == (attack.EffectiveCardKind ?? responding.CardKind) && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(response.IncomingCard);
        if (_resolutionStackOperations._resolutionStack[child - 1] is CardUseFrame { AnniversaryQiaoRuiUse: not null } use && IsQiaoRuiNativeUse(use) && _resolutionStackOperations._resolutionStack[child] is ProgramCardTriggerWindowFrame window)
            return window.ParentFrameId == use.Id && window.Action.ActionId == use.Action!.ActionId;
        if (_resolutionStackOperations._resolutionStack[child - 1] is ProgramCardTriggerWindowFrame card && _coreCardUseLifecycleOperations.LifecycleCardUse(card.ParentFrameId)is { AnniversaryQiaoRuiUse: not null } native && IsQiaoRuiNativeUse(native) && _resolutionStackOperations._resolutionStack[child] is ProgramSkillFrame f && card.CandidateIndex >= 0 && card.CandidateIndex < card.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(card.Candidates[card.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(card, card.Candidates[card.CandidateIndex]);
        return false;
    }

    internal bool QiaoRuiObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _characterQiaoRuiAnniversaryOperations.QiaoRuiNativeEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterQiaoRuiAnniversaryOperations.QiaoRuiFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, d) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, d));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 49400)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 49400)]
    internal bool QiaoRuiDamageObserver(long id) => QiaoRuiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 49400)]
    internal bool QiaoRuiBeforeDamage(long id) => QiaoRuiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 49400)]
    internal bool QiaoRuiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && QiaoRuiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 49400)]
    internal bool AdvanceQiaoRuiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && QiaoRuiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 49400)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 49400)]
    internal bool QiaoRuiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryQiaoRui is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && QiaoRuiFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || QiaoRuiObserverSuffix(parent)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }
}
