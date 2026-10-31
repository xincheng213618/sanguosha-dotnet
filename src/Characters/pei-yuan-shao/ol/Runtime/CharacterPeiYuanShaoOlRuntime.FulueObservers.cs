#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterPeiYuanShaoOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27120)]
    internal void AssertFulue(ProgramSkillFrame f)
    {
        if (f.Fulue is not { } r)
            return;
        if (r.Source.OwnerSeat != f.OwnerSeat || r.Source.SkillId != f.SkillId || r.Source.SkillInstanceId != f.SkillInstanceId || r.GameplayHash != f.GameplayHash || r.ActualTurn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || r.UserSeat == r.TargetSeat || !Enum.IsDefined(r.Stage) || f.WindowContext?.CardUse is not { } card || card.ParentCardUseFrameId != r.UseId || card.CardActionId != r.ActionId || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } action } use || action.ActionId != r.ActionId || f.SelectedCardIds.Count != 0)
            throw new InvalidOperationException("Fulue lost its original native Use, source, actual turn or immutable participants.");
        if (r.Stage == FulueStage.CounterRequest)
        {
            if (!HasIssuedFulueRequest(f) || f.PendingMovementContinuation is not null)
                throw new InvalidOperationException("Fulue's counter request changed its exact completed zero-damage parent.");
            return;
        }

        if (f.InstructionIndex != 1 || f.SelectedTargetSeats.Count != 0 || f.WindowContext.Window != SkillProgramTriggerWindow.CardUseTargetsFinalized || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != (r.Counter ? SkillProgramEffectOp.ResolveFulueCounterOption : SkillProgramEffectOp.OfferFulueSingleTargetOption) || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == f.WindowContext.ParentFrameId)is not { } window || window.ParentFrameId != r.UseId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])))
            throw new InvalidOperationException("Fulue requires its exact current native target-finalization candidate.");
        if (r.Option is { } option)
        {
            if (!FulueUseProof(use) || use.FulueEntries!.Count(e => e.ProgramFrameId == f.Id && e.Option == option && e.UserSeat == r.UserSeat && e.TargetSeat == r.TargetSeat && e.Counter == r.Counter) != 1)
                throw new InvalidOperationException("Fulue lost its once-executed original option invoice.");
        }
        else if (r.Stage != FulueStage.Option || r.Counter)
            throw new InvalidOperationException("Fulue cannot obtain before selecting its actual option.");
        if (r.Stage == FulueStage.MovementChildren)
        {
            if (r.Option != FulueOption.Obtain || r.Obtained is not { } cost || cost.From.OwnerSeat != r.TargetSeat || cost.From.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment or CardZoneKind.Judgment) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.MoveSequence is not { } sequence || f.PendingMovementContinuation is not { BeforeCount: 0, CoverageResultBind: null } pending || pending.SubjectSeat != f.OwnerSeat || _coreProgramEventHistoryOperations.ProgramEventHistory<FulueObtainPaidEvent>().Where(e => e.ProgramFrameId == f.Id).ToArray()is not [var paid] || paid.UseId != r.UseId || paid.ActionId != r.ActionId || paid.RecipientSeat != r.UserSeat || paid.Material != cost || paid.Before != r.Before || paid.After != r.After || paid.MoveSequence != sequence || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence == sequence && m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == cost.CardId && m.CardKind == cost.CardKind && m.From == cost.From && m.To == _coreEphemeralCardMaterialsOperations.NormalizeEphemeralCardDestination(_coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId), cost.From, (cost.From.Zone == CardZoneKind.Equipment && _coreAdvancedLifecycleOperations.GetAdvancedCard(cost.CardId).IsGeneralWeapon ? CardLocation.OutsideGame : CardLocation.Hand(r.UserSeat))) && m.Reason.Value == FulueObtainReason))
                throw new InvalidOperationException("Fulue's genuine obtain lost its original entity, actual once-paid movement or owning child return.");
        }
        else if (f.PendingMovementContinuation is not null || r.Obtained is not null || r.MoveSequence is not null || r.Before != 0 || r.After != 0)
            throw new InvalidOperationException("Fulue cannot retain a movement cursor before payment.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27120)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 27120)]
    internal bool ContinueIssuedFulue(ProgramSkillFrame f)
    {
        if (f.Fulue is not null)
        {
            AssertFulue(f);
            return true;
        }

        return f.InstructionIndex is 0 or 1 && f.TriggerId is { } id && f.ActivationId == id && f.WindowContext is { } context && CanRunIssuedFulueCandidate(new(f.OwnerSeat, f.SkillId, id, f.SkillInstanceId, f.GameplayHash, 0), context);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 570)]
    internal bool IsFulueMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation pending)
    {
        if (f.Fulue?.Stage != FulueStage.MovementChildren || effect is null || !PeiYuanShaoComposition.Handles(effect.Op) || pending != f.PendingMovementContinuation)
            return false;
        AssertFulue(f);
        return true;
    }

    internal bool FulueLionProof(ProgramSkillFrame f, int seat, CardMoveReason? reason) => f.Fulue is { Stage: FulueStage.MovementChildren, Obtained: { CardKind: CardKind.SilverLion, From.Zone: CardZoneKind.Equipment } cost } r && cost.From.OwnerSeat == seat && reason?.Value == FulueObtainReason && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == cost.CardId && m.From == cost.From && m.Reason == reason);
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.RecoveryCursor, 840)]
    [ProgramRecoveryParent(ProgramRecoveryParentPhase.DamageCursor, 780)]
    internal bool IsFulueRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => f.Fulue?.Stage == FulueStage.MovementChildren && f.PendingMovementContinuation is not null && _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(recovery, f) && recovery.Return.Continuation == PostEventContinuation.AwaitedProgramMovement && recovery.Attempt.Amount == 1 && recovery.Attempt.SourceSeat == recovery.Attempt.TargetSeat && recovery.Attempt.Completion.Producer == RecoveryAttemptProducer.SilverLion && FulueLionProof(f, recovery.Attempt.TargetSeat, recovery.Attempt.Completion.MoveReason);
    internal bool FulueFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.Fulue is not { Stage: FulueStage.MovementChildren } r)
            return false;
        AssertFulue(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        if (child is RecoveryReplacementFrame recovery)
            return IsFulueRecoveryParent(f, recovery);
        if (child is HpChangedTriggerWindowFrame hp)
            return hp.Change.ParentFrameId == f.Id && hp.ResumeFrameId == f.Id && hp.Continuation == PostEventContinuation.AwaitedProgramMovement && hp.Change.Kind == HpChangeKind.Recovery && hp.Change.Amount == 1 && hp.Change.SourceSeat == hp.Change.TargetSeat && FulueLionProof(f, hp.Change.TargetSeat, new(FulueObtainReason));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? FulueObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterPeiYuanShaoOlOperations.FulueFirstChild(f, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 680)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 790)]
    internal bool HasFulueDamageObserver(long id) => FulueObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is DamageTriggerWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 820)]
    internal bool HasFulueBeforeDamageObserver(long id) => FulueObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id && f is BeforeDamageProgramWindowFrame) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 640)]
    internal bool IsFulueDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && FulueObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27120)]
    internal bool AdvanceFulueSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FulueObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
