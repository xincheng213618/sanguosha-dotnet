#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCuiLieAnniversaryRuntime
{
    internal bool CuiLieOriginal(ProgramSkillFrame f, SkillProgramEffectOp op)
    {
        if (_contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != f.GameplayHash || f.SelectedCardIds.Count != 0)
            return false;
        if (op == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijue)
            return f.SkillId == AnniversaryCuiLieComposition.Zijue && f.ActivationId == "declare-number" && f.TriggerId is null && f.WindowContext is null && f.OwnerSeat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play && f.SelectedTargetSeats is [var seat] && _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && seat != f.OwnerSeat && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramSkillStartedEvent>().Count(e => e.FrameId == f.Id && e.OwnerSeat == f.OwnerSeat && e.SkillId == f.SkillId && e.ActivationId == f.ActivationId) == 1;
        if (f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat || f.ActivationId != f.TriggerId || f.SelectedTargetSeats.Count != 0)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (index < 1)
            return false;
        if (op == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw)
            return f.SkillId == AnniversaryCuiLieComposition.Zijue && f.TriggerId == "paid-pindian-draw" && w.Window == SkillProgramTriggerWindow.PindianCardsRevealed && _resolutionStackOperations._resolutionStack[index - 1] is PindianRevealWindowFrame p && p.Id == w.ParentFrameId && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, p.Candidates[p.CandidateIndex]) && p.Cards.Any(c => c.Seat == f.OwnerSeat);
        return op == SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi && f.SkillId == AnniversaryCuiLieComposition.Chibi && f.TriggerId == "far-target-contest" && CuiLieUse(w)is { } u && _resolutionStackOperations._resolutionStack[index - 1] is ProgramCardTriggerWindowFrame window && window.Id == w.ParentFrameId && window.ParentFrameId == u.Id && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex]));
    }

    internal bool ValidCuiLie(ProgramSkillFrame f)
    {
        if (f.AnniversaryCuiLie is not { } r || f.InstructionIndex != 1 || !CuiLieOriginal(f, r.Operation) || r.Source != CuiLieSource(f) || r.Hash != f.GameplayHash || r.ParentId != (f.WindowContext?.ParentFrameId ?? 0) || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber || r.Phase != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(r.Actor) || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Any(c => c <= 0) || r.Cards.Distinct().Count() != r.Cards.Count || r.Materials.Distinct().Count() != r.Materials.Count || r.Cursor < 0 || r.RewardLeases.Distinct().Count() != r.RewardLeases.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLieStartedEvent>().Count(e => e == new AnniversaryCuiLieStartedEvent(f.Id, r.Source, r.Hash, r.Operation, r.ParentId, r.Turn, r.Phase, r.Actor)) != 1)
            return false;
        if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijue)
        {
            if (f.SelectedTargetSeats is not [var actor] || actor != r.Actor || r.PindianId != 0 || r.Result is not null || r.UseId != 0 || r.ActionId != 0 || r.RewardLeases.Count != 0 || r.Materials.Count != 0 || r.Stage is AnniversaryCuiLieStage.Pindian or AnniversaryCuiLieStage.PindianChildren or AnniversaryCuiLieStage.BorrowedVictim or AnniversaryCuiLieStage.MutationChildren)
                return false;
            if (r.Stage == AnniversaryCuiLieStage.Number)
                return r.Number == 0 && r.Cards.Count == 0;
            if (r.Number is < 2 or > 4 || r.Cards.Count > r.Number || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLieNumberEvent>().Count(e => e == new AnniversaryCuiLieNumberEvent(f.Id, r.Actor, r.Number)) != 1)
                return false;
            if (r.Stage == AnniversaryCuiLieStage.GiveChildren && (r.Cards.Count != r.Number || r.Moves.Count != r.Number || r.Moves.Any(m => m.From.OwnerSeat != f.OwnerSeat || m.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !r.Cards.Contains(m.CardId) || m.Reason.Value != CuiLieGive || m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m)) || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLieGiftEvent>().Count(e => e == new AnniversaryCuiLieGiftEvent(f.Id, r.Number, r.Before, r.After)) != 1))
                return false;
            if (r.Stage is AnniversaryCuiLieStage.Drawing or AnniversaryCuiLieStage.DrawChildren && r.Cards.Count != 0)
                return false;
        }
        else if (r.Operation == SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw)
        {
            if (r.Actor != f.OwnerSeat || r.Number != 1 || r.RewardLeases.Count == 0 || r.Cursor >= r.RewardLeases.Count || r.Cards.Count != 0 || r.Materials.Count != 0 || r.PindianId != 0 || r.Result is not null || r.UseId != 0 || r.ActionId != 0 || r.Stage is not (AnniversaryCuiLieStage.Drawing or AnniversaryCuiLieStage.DrawChildren or AnniversaryCuiLieStage.Completed) || r.RewardLeases.Any(id => !CuiLieLeases(f.OwnerSeat).Any(l => l.FrameId == id && l.Source.SkillInstanceId == f.SkillInstanceId)))
                return false;
        }
        else
        {
            if (r.Operation != SkillProgramEffectOp.ResolveAnniversaryCuiLieChibi || CuiLieUse(f.WindowContext)is not { Action: { } a } u || u.Id != r.UseId || a.ActionId != r.ActionId || a.ActorSeat != r.Actor || r.PindianId <= f.Id || r.Number != 0 || r.Cards.Count != 0 || r.RewardLeases.Count != 0 || r.Draw is not null || r.Stage is AnniversaryCuiLieStage.Number or AnniversaryCuiLieStage.Payment or AnniversaryCuiLieStage.GiveChildren or AnniversaryCuiLieStage.RecoveryChildren or AnniversaryCuiLieStage.Drawing or AnniversaryCuiLieStage.DrawChildren || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLiePindianIssuedEvent>().Count(e => e == new AnniversaryCuiLiePindianIssuedEvent(f.Id, r.PindianId, r.Actor)) != 1)
                return false;
            if (r.Stage != AnniversaryCuiLieStage.Pindian && (r.Result is not { } result || result.SourceSeat != f.OwnerSeat || result.OpponentSeat != r.Actor || _resolutionStackOperations._resolutionStack.Any(x => x.Id == r.PindianId) || f.PindianResultBindings.SingleOrDefault(b => b.Name == CuiLieContest)is not { } binding || binding.SourceSeat != result.SourceSeat || binding.OpponentSeat != result.OpponentSeat || binding.SourceRank != result.SourceRank || binding.OpponentRank != result.OpponentRank || binding.SourceWon != result.SourceWon || _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId && e.Result == result) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLiePindianReturnedEvent>().Count(e => e == new AnniversaryCuiLiePindianReturnedEvent(f.Id, r.PindianId, result.SourceRank, result.OpponentRank, result.SourceWon)) != 1))
                return false;
            if (r.Stage == AnniversaryCuiLieStage.MutationChildren && (_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLieUseChangedEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var changed] || !changed.Cancelled || !changed.Materials.SequenceEqual(r.Materials) || !IsAnniversaryCuiLieTargetMutationFact(u, changed, changed.BeforeTargets) || r.Moves.Count != r.Materials.Count || r.Moves.Any(m => !r.Materials.Contains(m.CardId) || m.From != CardLocation.Processing || m.Reason.Value != CuiLieClaim || m.Sequence <= r.Before || m.Sequence > r.After || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))))
                return false;
        }

        if (r.Draw is { } d && (d.RecipientSeat != f.OwnerSeat || d.RequestedCount != r.Number || d.SequenceBefore != r.Before || d.SequenceAfter != r.After || !_coreProgramEventHistoryOperations.ProgramEventHistory<AnniversaryCuiLieDrawReturnedEvent>().Any(e => e == new AnniversaryCuiLieDrawReturnedEvent(f.Id, f.OwnerSeat, r.Number, d.ActualDrawIds.Count, r.Before, r.After))))
            return false;
        return true;
    }

    internal bool HasAnniversaryCuiLiePrompt() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is { Kind: DecisionKind.ProgramTrigger, IsPrivate: true } p && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { Step: ResolutionFrameStep.AwaitingResponse, AnniversaryCuiLie.Stage: AnniversaryCuiLieStage.Number or AnniversaryCuiLieStage.Payment or AnniversaryCuiLieStage.BorrowedVictim } f && p.PlayerSeat == CuiLieChooser(f) && p.TargetSeat == p.PlayerSeat && p.SourceSeat == f.OwnerSeat && ValidCuiLie(f) && global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(p.Choices, CuiLieChoices(f));
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 46900)]
    internal void AssertCuiLie(ProgramSkillFrame f)
    {
        if (f.AnniversaryCuiLie is not null && (!ValidCuiLie(f) || f.PendingMovementContinuation is { } p && !CuiLieMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p)))
            throw new InvalidOperationException("Cui Lie lost his original recipient, paid native contest, issued lease or real movement return.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 46900)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 46900)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 46900)]
    internal bool CuiLieMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.GiveChildren or AnniversaryCuiLieStage.RecoveryChildren or AnniversaryCuiLieStage.DrawChildren or AnniversaryCuiLieStage.PindianChildren or AnniversaryCuiLieStage.MutationChildren } r && effect?.Op == r.Operation && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidCuiLie(f);
    internal bool IsAnniversaryCuiLiePindianSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => f.AnniversaryCuiLie is { Stage: AnniversaryCuiLieStage.PindianChildren, Result: { } result } r && ValidCuiLie(f) && batch.ParentFrameId == r.PindianId && batch.AwaitingProgramFrameId is null && batch.TurnNumber == r.Turn && batch.OriginSkillId == f.SkillId && batch.OriginSkillInstanceId == f.SkillInstanceId && batch.OriginOwnerSeat == f.OwnerSeat && batch.Movements.Count is >= 1 and <= 2 && batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(x => x == m) == 1 && (m.CardId == result.SourceCardId || m.CardId == result.OpponentCardId) && (m.To == CardLocation.Processing && (m.Reason == CardMoveReasons.PindianReveal || m.Reason.Value == "pindian.reserve-top") || m.From == CardLocation.Processing && (m.Reason == CardMoveReasons.PindianFinish || m.Reason.Value == "program.pindian.claim")));
    internal bool CuiLieFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidCuiLie(f) || f.AnniversaryCuiLie is not { } r)
            return false;
        if (child is PindianFrame pair)
            return r.Stage == AnniversaryCuiLieStage.Pindian && pair.Id == r.PindianId && pair.ParentFrameId == f.Id && pair.SourceSeat == f.OwnerSeat && pair.OpponentSeat == r.Actor && pair.SkillId == f.SkillId && pair.ProgramResultBind == CuiLieContest;
        if (child is NativeDrawAttemptFrame draw)
            return r.Stage == AnniversaryCuiLieStage.Drawing && draw.Context.ParentFrameId == f.Id && draw.Context.RecipientSeat == f.OwnerSeat && draw.Context.RequestedCount == r.Number && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(draw, f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && CuiLieMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && (IsAnniversaryCuiLiePindianSettlementBatch(f, moved.Batch) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved)) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return r.Stage == AnniversaryCuiLieStage.RecoveryChildren && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool CuiLieObserverEdge(int i)
    {
        var parent = _resolutionStackOperations._resolutionStack[i - 1];
        var child = _resolutionStackOperations._resolutionStack[i];
        if (child is PindianRevealWindowFrame reveal && parent is PindianFrame pair)
            return _corePindianRevealWindowsOperations.ExactPairRevealParent(pair, reveal);
        if (child is ProgramSkillFrame p && parent is PindianRevealWindowFrame w)
            return w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && p.WindowContext?.ParentFrameId == w.Id && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(p, w.Candidates[w.CandidateIndex]);
        return parent is ProgramSkillFrame f && CuiLieFirstChild(f, child) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i);
    }

    internal bool CuiLieObserverSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _characterCuiLieAnniversaryOperations.CuiLieObserverEdge(n), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, d) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, d));
    internal ProgramSkillFrame? CuiLieObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterCuiLieAnniversaryOperations.CuiLieFirstChild(f, c), (i) => _characterCuiLieAnniversaryOperations.CuiLieObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 46900)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 46900)]
    internal bool CuiLieDamageObserver(long id) => CuiLieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 46900)]
    internal bool CuiLieBeforeDamage(long id) => CuiLieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 46900)]
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 46900)]
    internal bool CuiLieDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && CuiLieObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 46900)]
    internal bool CuiLieAdvanceChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && CuiLieObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 46900)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 46900)]
    internal bool CuiLieRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.AnniversaryCuiLie is null)
            return false;
        var parent = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        var child = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == recovery.Id);
        return parent >= 0 && child > parent && CuiLieFirstChild(f, _resolutionStackOperations._resolutionStack[parent + 1]) && (child == parent + 1 || CuiLieObserverSuffix(parent)) && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, _resolutionStackOperations._resolutionStack[child - 1]);
    }

    [ProgramPipelineHook(ProgramPipelinePhase.IsProgramMovementSettlement, 2500)]
    internal bool WindowSettlement_IsAnniversaryCuiLiePindianSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => IsAnniversaryCuiLiePindianSettlementBatch(frame, batch);
}
