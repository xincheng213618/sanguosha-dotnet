#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhuJunOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 27030)]
    internal void AssertZhuJunCuipo(ProgramSkillFrame f)
    {
        if (f.ZhuJunCuipoDraw is not { } r)
            return;
        var q = r.Qualification;
        if (f.InstructionIndex != 1 || r.InstructionIndex != f.InstructionIndex || f.SkillId != q.Source.SkillId || f.SkillInstanceId != q.Source.SkillInstanceId || f.OwnerSeat != q.Source.OwnerSeat || f.TriggerId != q.Source.BindingId || f.GameplayHash != q.GameplayHash || q.Damage || q.Ordinal != q.NameLength || q.NameLength != ZhuJunCardNameLength(q.EffectiveKind) || q.Turn != _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber || q.TurnOwner != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCommitted, CardUse: { } card } c || c.ParentFrameId != r.WindowId || c.OwnerSeat != f.OwnerSeat || card.ActorSeat != f.OwnerSeat || card.EffectiveKind != q.EffectiveKind || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(window.Candidates[window.CandidateIndex])) || window.Action.ActionId != card.CardActionId || window.Action.ActorSeat != f.OwnerSeat || q.ActionId is { } actionId && card.CardActionId != actionId || q.ActionId is null && (_coreCardUseLifecycleOperations.LifecycleCardUse(q.OriginFrameId)is not { } legacy || legacy.Action?.ActionId != card.CardActionId || !_coreActualTurnLegacyCompletionOperations.IsExactLegacyActualUseCompletion(legacy)) || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveCuipoActualTurnUse || f.PendingMovementContinuation is null || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.ActualCount is < 0 or > 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoQualifiedEvent>().Count(e => e == q) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<ZhuJunCuipoDrawIssuedEvent>().Count(e => e.FrameId == f.Id && e.WindowId == r.WindowId && e.OriginFrameId == q.OriginFrameId && e.ActionId == q.ActionId && e.Source == q.Source && e.Before == r.Before && e.After == r.After && e.ActualCount == r.ActualCount) != 1 || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhuJunDrawReason) != r.ActualCount || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(m => m.Sequence > r.Before && m.Sequence <= r.After).Any(m => !IsZhuJunDrawMovement(f, m)))
            throw new InvalidOperationException("Cuipo lost its exact committed actor, frozen ordinal or once-issued native draw interval.");
    }

    internal bool IsZhuJunDrawMovement(ProgramSkillFrame f, CardMovementRecord m) => f.ZhuJunCuipoDraw is { } r && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == ZhuJunDrawReason || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle);
    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 240)]
    internal bool IsZhuJunMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => f.ZhuJunCuipoDraw is not null && effect?.Op == SkillProgramEffectOp.ResolveCuipoActualTurnUse && p == f.PendingMovementContinuation && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    internal bool ZhuJunFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.ZhuJunCuipoDraw is not { } r || f.PendingMovementContinuation is null)
            return false;
        AssertZhuJunCuipo(f);
        if (child is CardsMovedTriggerWindowFrame moved)
            return global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => IsZhuJunDrawMovement(f, m));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        return child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange && state.ResumeProgramFrameId == f.Id && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == f.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
    }

    internal ProgramSkillFrame? ZhuJunObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterZhuJunOlOperations.ZhuJunFirstChild(f, c), (i) => _coreObserverSubtreesOperations2.HasObserverSuffix(i, (child) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 600)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 710)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 740)]
    internal bool HasZhuJunDamageObserver(long id) => ZhuJunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 560)]
    internal bool IsZhuJunDying() => IsZhuJunLegacyAlcoholPreRecoveryRide() || _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && ZhuJunObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 27030)]
    internal bool AdvanceZhuJunSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ZhuJunObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
