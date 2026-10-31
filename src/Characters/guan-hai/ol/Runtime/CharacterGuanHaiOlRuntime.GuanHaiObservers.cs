#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanHaiOlRuntime
{
    internal bool CanContinueIssuedGuanHai(ProgramSkillFrame f)
    {
        if (f.GuanHai is { Paid: true })
        {
            AssertGuanHai(f);
            return true;
        }

        if (f.InstructionIndex is not (0 or 1) || f.TriggerId is null || f.WindowContext is not { } c)
            return false;
        var candidate = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, _coreProgramLifecycleOperations.GetProgramTrigger(f).Priority, c.OccurrenceIndex);
        return IsIssuedXieweiCandidate(candidate, c) || IsIssuedGuanHaiYouqueCandidate(candidate, c);
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27578)]
    internal void AssertGuanHai(ProgramSkillFrame f)
    {
        if (f.GuanHai is not { } r)
            return;
        if (f.InstructionIndex != 1 || r.Source != GuanHaiSource(f) || r.GameplayHash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.GameplayHash || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Op || r.ActualTurn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Materials is not System.Collections.IList { IsReadOnly: true } || r.Materials.Select(m => m.CardId).Distinct().Count() != r.Materials.Count || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreProgramEventHistoryOperations.ProgramEventHistory<GuanHaiStartedEvent>().Count(e => e == new GuanHaiStartedEvent(f.Id, r.Source, r.GameplayHash, r.Op, r.ActualTurn, r.TurnOwner, r.ParentId)) != 1)
            throw new InvalidOperationException("Guan Hai lost his original owning instruction, immutable costs, native turn or issued source.");
        if (r.Op == SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse)
        {
            if (r.Stage != GuanHaiStage.XieweiChildren || !r.Paid || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats.Count != 0 || f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted } c || c.ParentFrameId != r.ParentId || _resolutionStackOperations._resolutionStack.SingleOrDefault(w => w.Id == r.ParentId)is not ProgramCardTriggerWindowFrame w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex])is not { } candidate || candidate.OwnerSeat != f.OwnerSeat || candidate.SkillId != f.SkillId || candidate.BindingId != f.TriggerId || candidate.SkillInstanceId != f.SkillInstanceId || IssuedXieweiPayment(w.Action)is not { } p || p.Id != r.PaymentId || p.UseId != r.NativeUseId || p.ActionId != r.ActionId || !p.Materials.SequenceEqual(r.Materials) || _coreProgramEventHistoryOperations.ProgramEventHistory<XieweiCommittedEvent>().Count(e => e == new XieweiCommittedEvent(f.Id, p.Id, p.UseId, p.ActionId)) != 1)
                throw new InvalidOperationException("Xiewei lost its exact paid native commitment parent.");
        }
        else if (r.Op == SkillProgramEffectOp.ResolveYouqueMinimumBaitPindian)
        {
            if (f.TriggerId is not null || f.WindowContext is not null || f.ActivationId != "minimum-bait-pindian" || f.SelectedCardIds.Count != 0 || f.SelectedTargetSeats is not [var target] || target != r.OpponentSeat || r.Phase is not { Kind: ActualDiscardRecoveryPhaseKind.Play } phase || phase.ActorSeat != f.OwnerSeat || phase.TurnNumber != r.ActualTurn || phase.ActualTurnOwnerSeat != r.TurnOwner || r.Paid != (r.PindianId > 0) || !r.Paid && r.Stage != GuanHaiStage.ChooseBait)
                throw new InvalidOperationException("Youque lost its actual Play and original other hand-bearing participant.");
            if (r.Paid && (r.Pile is null || _coreProgramEventHistoryOperations.ProgramEventHistory<YouquePindianIssuedEvent>().Count(e => e == new YouquePindianIssuedEvent(f.Id, r.PindianId, r.Source, r.GameplayHash, r.Pile, r.BaitCardId, r.BaitRank, r.OpponentSeat, phase, _coreProgramEventHistoryOperations.ProgramEventHistory<YouquePindianIssuedEvent>().Single(e => e.FrameId == f.Id).Before)) != 1))
                throw new InvalidOperationException("Youque replaced the original selected minimum bait.");
            if (r.Result is { } result)
            {
                if (_coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Count(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId && e.Result == result) != 1 || result.SourceSeat != f.OwnerSeat || (result.SourceCardId != r.BaitCardId && !_characterPangJiOlOperations.IsDouyuHistoricalPairAssignment(r.PindianId, result, r.BaitCardId) && !_characterLuKangWumiaoOperations.IsWuLuKangHistoricalPairAssignment(r.PindianId, result, r.BaitCardId)) || result.OpponentSeat != r.OpponentSeat || result.SourceRank == result.OpponentRank || r.WinnerSeat != (result.SourceWon ? result.SourceSeat : result.OpponentSeat) || r.LoserSeat != (result.SourceWon ? result.OpponentSeat : result.SourceSeat))
                    throw new InvalidOperationException("Youque changed its real native Pindian winner, loser or rank.");
                var frozen = _coreProgramEventHistoryOperations.ProgramEventHistory<YouqueDiscardMaterialEvent>().Where(e => e.FrameId == f.Id).OrderBy(e => e.Index).ToArray();
                if (!frozen.Select(e => new GuanHaiMaterial(e.CardId, e.Kind, e.From)).SequenceEqual(r.Materials) || frozen.Where((e, i) => e.Index != i || e.LoserSeat != r.LoserSeat || e.From != CardLocation.Hand(r.LoserSeat)).Any())
                    throw new InvalidOperationException("Youque cannot split or replace the real loser's whole higher-rank Hand invoice.");
                foreach (var material in r.Materials)
                    if (_coreProgramEventHistoryOperations.ProgramEventHistory<YouqueDiscardPaidEvent>().Where(e => e.FrameId == f.Id && e.CardId == material.CardId).ToArray()is not [var paid] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == material.CardId && m.CardKind == material.Kind && m.From == material.From && m.To == CardLocation.DiscardPile && m.Reason.Value == YouqueDiscardReason))
                        throw new InvalidOperationException("Youque's whole Hand discard has an unpaid entity.");
            }

            if (r.NativeUseId > 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<YouqueSlashIssuedEvent>().Count(e => e.Origin.ProgramFrameId == f.Id && e.Origin.UseId == r.NativeUseId && e.Origin.ActionId == r.ActionId && e.Origin.Source == r.Source && e.Origin.Phase == phase && e.Origin.ActorSeat == r.LoserSeat && e.Origin.TargetSeat == r.WinnerSeat) != 1)
                throw new InvalidOperationException("Youque lost the exact once-issued native loser-to-winner Slash.");
            if (r.NativeReturned && (r.Stage is not (GuanHaiStage.NativeSlash or GuanHaiStage.NativeChildren) || _coreCardUseLifecycleOperations.LifecycleCardUse(r.NativeUseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == r.NativeUseId) != 1))
                throw new InvalidOperationException("Youque returned before its whole native Slash finished.");
        }
        else if (r.Op != SkillProgramEffectOp.ResolveYouqueIssuedSlashDamageBenefit || !r.Paid || r.Stage is not (GuanHaiStage.RewardDraw or GuanHaiStage.RewardChildren) || f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } dc || dc.DamageFrameId != r.DamageFrameId || dc.Amount <= 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<YouqueBenefitIssuedEvent>().Count(e => e == new YouqueBenefitIssuedEvent(f.Id, r.ParentId, r.NativeUseId, r.DamageFrameId, f.OwnerSeat)) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<YouqueSlashIssuedEvent>().Count(e => e.Origin.ProgramFrameId == r.ParentId && e.Origin.UseId == r.NativeUseId && e.Origin.ActionId == r.ActionId && e.Origin.Phase == r.Phase && e.Origin.Source.OwnerSeat == f.OwnerSeat) != 1)
            throw new InvalidOperationException("Youque reward requires its original actual positive-damage native use, never a synthetic damage event.");
        if (f.PendingMovementContinuation is { } pending && !IsGuanHaiMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), pending))
            throw new InvalidOperationException("Guan Hai has an unrelated native movement return.");
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 520)]
    internal bool IsGuanHaiMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.GuanHai is { Paid: true } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && r.Stage is GuanHaiStage.XieweiChildren or GuanHaiStage.Pindian or GuanHaiStage.PindianChildren or GuanHaiStage.DiscardChildren or GuanHaiStage.NativeChildren or GuanHaiStage.RewardChildren;
    internal bool GuanHaiMovementMatches(ProgramSkillFrame f, CardMovementRecord m)
    {
        var r = f.GuanHai!;
        if (!_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m))
            return false;
        if (r.Stage == GuanHaiStage.XieweiChildren)
            return _coreCardUseLifecycleOperations.LifecycleCardUse(r.NativeUseId)?.XieweiPayments?.Any(p => p.Id == r.PaymentId && ValidXieweiPayment(p) && p.Movements.Contains(m)) == true;
        if (r.Stage == GuanHaiStage.Pindian && _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().SingleOrDefault(e => e.FrameId == r.PindianId && e.SkillId == f.SkillId)is { Result: { } result })
            return new[]
            {
                result.SourceCardId,
                result.OpponentCardId
            }.Contains(m.CardId) && (m.Reason == CardMoveReasons.PindianReveal && m.To == CardLocation.Processing && (m.From == (m.CardId == result.SourceCardId ? r.Pile!.Location : CardLocation.Hand(result.OpponentSeat)) || m.From == CardLocation.DrawPile) || m.Reason == CardMoveReasons.PindianFinish && m.From == CardLocation.Processing && m.To == CardLocation.DiscardPile || m.Reason.Value == "program.pindian.claim" && m.From == CardLocation.Processing && m.To.Zone == CardZoneKind.Hand || m.Reason.Value == "pindian.reserve-top" && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing);
        if (m.Sequence <= r.Before || m.Sequence > r.After)
            return false;
        if (r.Stage == GuanHaiStage.DiscardChildren)
            return r.Materials.Any(c => c.CardId == m.CardId && c.Kind == m.CardKind && c.From == m.From) && m.To == CardLocation.DiscardPile && m.Reason.Value == YouqueDiscardReason;
        if (r.Stage == GuanHaiStage.RewardChildren)
            return m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == YouqueDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle;
        return false; // The zero-entity native Slash has no material settlement.
    }

    internal bool GuanHaiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.GuanHai is not { Paid: true } r)
            return false;
        AssertGuanHai(f);
        if (child is PindianFrame p)
            return r.Stage == GuanHaiStage.Pindian && p.ParentFrameId == f.Id && p.Id == r.PindianId && IsGuanHaiPindianSource(p, _corePindianOwnedPileSourcesOperations.HasPaidOwnedPilePindianSource(p));
        if (child is CardUseFrame use)
            return r.Stage == GuanHaiStage.NativeSlash && use.YouqueSlash?.ProgramFrameId == f.Id && IsIssuedYouqueUse(use);
        if (r.Stage == GuanHaiStage.RewardDraw && f.NativeDraw is not null)
            return global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f);
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state)
            return state.ResumeProgramFrameId == f.Id && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange;
        if (child is HpChangedTriggerWindowFrame hp)
            return r.Stage is GuanHaiStage.XieweiChildren or GuanHaiStage.DiscardChildren or GuanHaiStage.RewardChildren && hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is not CardsMovedTriggerWindowFrame moved || moved.Batch.Movements.Count == 0 || !moved.Batch.Movements.All(m => GuanHaiMovementMatches(f, m)))
            return false;
        return r.Stage == GuanHaiStage.XieweiChildren ? moved.Batch.ParentFrameId == r.NativeUseId && (moved.Batch.AwaitingProgramFrameId == f.Id || moved.ResumeProgramFrameId == f.Id) : global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && (r.Stage == GuanHaiStage.Pindian ? moved.Batch.ParentFrameId == r.PindianId : moved.Batch.ParentFrameId == f.Id);
    }

    internal ProgramSkillFrame? GuanHaiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, child) => _characterGuanHaiOlOperations.GuanHaiFirstChild(f, child), (i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1040)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 1150)]
    internal bool HasGuanHaiDamageObserver(long id) => GuanHaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 1180)]
    internal bool HasGuanHaiBeforeDamageObserver(long id) => GuanHaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 1010)]
    internal bool IsGuanHaiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && GuanHaiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 270)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 260)]
    internal bool IsGuanHaiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery)
    {
        if (f.GuanHai is not { Paid: true } || !_coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f))
            return false;
        return _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == recovery.Id) > _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id) && GuanHaiObserverRoot()?.Id == f.Id;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27578)]
    internal bool AdvanceGuanHaiChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && GuanHaiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
