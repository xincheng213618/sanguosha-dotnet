#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuFengBoundaryRuntime
{
    internal bool ValidBoundaryLiuFeng(ProgramSkillFrame f)
    {
        if (f.BoundaryLiuFeng is not { } r || (f.InstructionIndex != 1 && !(f.InstructionIndex == 0 && r.Need is not null)) || r.Source != BoundaryLiuFengSource(f) || r.Hash != f.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash != r.Hash || r.Turn != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseStartedTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Cards.Count > 2 || r.Cards.Distinct().Count() != r.Cards.Count || r.From.Count != r.Cards.Count || r.Targets.Distinct().Count() != r.Targets.Count || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengStartedEvent>().Count(e => e == new BoundaryLiuFengStartedEvent(f.Id, r.Source, r.Hash, r.Op, r.ParentId, r.Turn, r.TurnOwner)) != 1)
            return false;
        if (r.Op == SkillProgramEffectOp.ResolveBoundaryLiuFengXiansi)
        {
            if (f.SkillId != BoundaryLiuFengComposition.Xiansi || f.TriggerId != "collect-up-to-two" || !_coreOriginalHandEntityObserversOperations.OriginalHandBindingStarted(f, SkillProgramTriggerWindow.TurnStartBeforeNormalFlow) || r.ParentId != f.WindowContext?.ParentFrameId || r.CostSequence != 0 || r.UseId != 0)
                return false;
        }
        else if (r.Op != SkillProgramEffectOp.ResolveBoundaryLiuFengQinling || f.SkillId != BoundaryLiuFengComposition.Qinling || f.ActivationId != BoundaryLiuFengComposition.Activation || f.TriggerId is not null || r.Targets.Count > 1)
            return false;
        if (r.Need is { } need)
        {
            if (r.OriginalDecision is not { } original || need.ActorSeat != f.OwnerSeat || need.OwnerFrameId != r.ParentId || r.Source.OwnerSeat != need.ActorSeat || original.PlayerSeat != need.ActorSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengRequestStartedEvent>().Count(e => e == new BoundaryLiuFengRequestStartedEvent(f.Id, need.OwnerFrameId, need.RequestFrameId, need.ActorSeat, need.InstructionIndex, need.RequestOp, need.Intent, original.PromptId, original.Revision, BoundaryLiuFengDecisionHash(original))) != 1)
                return false;
            if (r.CostSequence == 0 && r.UseId == 0 && r.Stage != BoundaryLiuFengStage.Completed && !BoundaryLiuFengNeedCurrent(r))
                return false;
            if (r.UseId == 0 && r.CostSequence > 0 && r.Stage != BoundaryLiuFengStage.Completed && !_resolutionStackOperations._resolutionStack.Any(x => x.Id == need.OwnerFrameId))
                return false;
        }

        if (r.CostSequence > 0 && (r.Cards.Count != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengCostPaidEvent>().Where(e => e.FrameId == f.Id).ToArray()is not [var paid] || paid.Source != r.Source || paid.Hash != r.Hash || paid.CardId != r.Cards[0] || paid.Sequence != r.CostSequence || paid.Turn != r.Turn || paid.TurnOwner != r.TurnOwner || paid.From.OwnerSeat != f.OwnerSeat || paid.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment) || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.From == paid.From && m.Reason.Value == "skill-program.boundary:qinling.cost")))
            return false;
        if (r.UseId > 0 && (_coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengUseIssuedEvent>().Where(e => e.Origin.FrameId == f.Id).ToArray()is not [var issue] || issue.Origin.UseId != r.UseId || issue.Origin.ActionId != r.ActionId || issue.Origin.Source != r.Source || issue.Origin.Hash != r.Hash || r.CostSequence <= 0))
            return false;
        if (r.Stage == BoundaryLiuFengStage.RewardDraw && (!r.Returned || !r.Damaged || !r.ConsequencePaid || r.RewardDraw is not null || r.Op != SkillProgramEffectOp.ResolveBoundaryLiuFengQinling))
            return false;
        if (r.RewardDraw is { } draw && (!r.Returned || !r.Damaged || !r.ConsequencePaid || r.Stage is not (BoundaryLiuFengStage.NativeTail or BoundaryLiuFengStage.Completed) || draw.RecipientSeat != f.OwnerSeat || draw.RequestedCount != 1 || draw.SequenceBefore != r.Before || draw.SequenceAfter != r.After || f.NativeDraw is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengRewardDrawReturnedEvent>().Count(e => e == new BoundaryLiuFengRewardDrawReturnedEvent(f.Id, r.UseId, draw.AttemptFrameId, draw.ActualDrawIds.Count, draw.ReplacementGainedIds.Count, draw.SequenceBefore, draw.SequenceAfter)) != 1))
            return false;
        return !r.Returned || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryLiuFengUseReturnedEvent>().Count(e => e.FrameId == f.Id && e.UseId == r.UseId && e.ActionId == r.ActionId && e.Damaged == r.Damaged) == 1;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 50200)]
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 50200)]
    [ProgramMovementProof(ProgramMovementProofPhase.PrivatePayments, 50200)]
    internal bool BoundaryLiuFengMovement(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.BoundaryLiuFeng is { Stage: BoundaryLiuFengStage.MovementChildren or BoundaryLiuFengStage.NativeTail } r && e?.Op == r.Op && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null && ValidBoundaryLiuFeng(f);
    internal bool BoundaryLiuFengFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (!ValidBoundaryLiuFeng(f) || f.BoundaryLiuFeng is not { } r)
            return false;
        if (r.Stage == BoundaryLiuFengStage.RewardDraw && f.NativeDraw is not null && global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(child, f))
            return true;
        if (child is CardUseFrame use)
            return r.Stage == BoundaryLiuFengStage.NativeUse && use.BoundaryLiuFeng is { } o && o.FrameId == f.Id && o.UseId == r.UseId && o.ActionId == r.ActionId && IsBoundaryLiuFengUse(use);
        if (child is CardsMovedTriggerWindowFrame moved)
            return f.PendingMovementContinuation is { } p && BoundaryLiuFengMovement(f, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f), p) && global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m));
        if (child is RecoveryReplacementFrame recovery)
            return recovery.ParentFrameId == f.Id && recovery.Return.ResumeFrameId == f.Id && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement;
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.ResumeFrameId == f.Id && hp.Change.ParentFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement;
        return child is ProgramLifecycleTriggerWindowFrame state && state.ResumeProgramFrameId == f.Id && (state.Window == SkillProgramTriggerWindow.SkillsChanged && state.Continuation == ProgramLifecycleContinuation.ResumeParentProgram || state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.CharacterStateContinuation == CharacterStateContinuation.Program);
    }

    internal bool BoundaryLiuFengNativeEdge(int child)
    {
        var parent = _resolutionStackOperations._resolutionStack[child - 1];
        var current = _resolutionStackOperations._resolutionStack[child];
        if (parent is CardUseFrame { BoundaryLiuFeng: not null } use && IsBoundaryLiuFengUse(use))
        {
            if (current is ResponseWindowFrame { RequiredCardKind: CardKind.Dodge } response && use.CardAttack is { Active: true } attack)
                return response.ParentFrameId == use.Id && response.SourceSeat == attack.SourceSeat && response.ResponderSeat == attack.TargetSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(response.IncomingCard);
            if (current is ProgramCardTriggerWindowFrame card)
                return card.ParentFrameId == use.Id && card.Action.ActionId == use.Action!.ActionId;
        }

        if (parent is ProgramCardTriggerWindowFrame window && _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is { BoundaryLiuFeng: not null } native && IsBoundaryLiuFengUse(native) && current is ProgramSkillFrame f && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count)
            return global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) && f.WindowContext == _coreCardActionsOperations.CreateCardActionProgramContext2(window, window.Candidates[window.CandidateIndex]);
        return false;
    }

    internal bool BoundaryLiuFengSuffix(int i) => _coreObserverSubtreesOperations.HasObserverSuffix(i, (n) => _coreObserverSubtreesOperations.IsMovementObserverEdge(n) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(n) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(n) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[n - 1], _resolutionStackOperations2._resolutionStack()[n]) || _coreDamageObserverCursorQueriesOperations.DamageFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.DamageObserverRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _characterLiuFengBoundaryOperations.BoundaryLiuFengNativeEdge(n) || _resolutionStackOperations2._resolutionStack()[n - 1] is ProgramSkillFrame f && _characterLiuFengBoundaryOperations.BoundaryLiuFengFirstChild(f, _resolutionStackOperations2._resolutionStack()[n]) || global::CardGame.Core.EngineOwners.CoreGameEngineNativeDrawAttemptsRuntime.NativeDrawAttemptRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[n], _resolutionStackOperations2._resolutionStack()[n - 1]), (n, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(n, d) || _coreTieredRoundZeroReturnsOperations.TieredRoundZeroDyingRescueRide(n, d) || _coreDrawFundedDistinctBasicReturnsOperations.DrawFundedDistinctBasicDyingRescueRide(n, d) || _characterHuanShuJunOlOperations.HuanpeiDyingRescueRide(n, d));
    internal ProgramSkillFrame? BoundaryLiuFengObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterLiuFengBoundaryOperations.BoundaryLiuFengFirstChild(f, c), (i) => _characterLiuFengBoundaryOperations.BoundaryLiuFengSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 50200)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 50200)]
    internal bool BoundaryLiuFengDamageObserver(long id) => BoundaryLiuFengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 50200)]
    internal bool BoundaryLiuFengBeforeDamage(long id) => BoundaryLiuFengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 50200)]
    internal bool BoundaryLiuFengDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && BoundaryLiuFengObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 50200)]
    internal bool AdvanceBoundaryLiuFengChildren() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryLiuFengObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
