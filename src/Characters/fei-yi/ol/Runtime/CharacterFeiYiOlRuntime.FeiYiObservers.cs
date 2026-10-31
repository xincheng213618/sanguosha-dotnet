#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFeiYiOlRuntime
{
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 26600)]
    internal void AssertFeiYi(ProgramSkillFrame f)
    {
        if (f.FeiYi is not { } r)
            return;
        if (r.Source != FeiYiSource(f) || r.GameplayHash != f.GameplayHash || r.Instruction != f.InstructionIndex || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != r.Operation || !FeiYiComposition.IsOperation(r.Operation) || r.Turn != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber || r.TurnOwner != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || !Enum.IsDefined(r.Stage) || r.Before < 0 || r.After < r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || r.Selected.Distinct().Count() != r.Selected.Count || r.Selected.Any(id => !r.Candidates.Contains(id)) || _coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Operation == r.Operation && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner && e.PhaseInstance == r.PhaseInstance && e.Odd == r.Odd && e.WindowId == r.WindowId && e.BatchId == r.BatchId) != 1)
            throw new InvalidOperationException("Fei Yi lost its exact owning instruction, immutable start, actual turn or card draft.");
        if (r.Operation == SkillProgramEffectOp.YanruParityDrawDiscard)
        {
            if (f.TriggerId is not null || f.WindowContext is not null || r.WindowId is not null || r.BatchId is not null || r.RevealedCard is not null || r.PhaseInstance != _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId || f.OwnerSeat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat || !r.Paid && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || _coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiYanruPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.PhaseInstance == r.PhaseInstance && e.Turn == r.Turn && e.Odd == r.Odd) != (r.Paid ? 1 : 0))
                throw new InvalidOperationException("Yanru lost its independent actual-Play parity invoice.");
        }
        else
        {
            if (f.TriggerId is null || f.WindowContext is not { } context || context.ParentFrameId != r.WindowId || context.MovementBatch?.Id != r.BatchId || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().SingleOrDefault(w => w.Id == r.WindowId)is not { } window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex]) || window.Batch.FeiYiSingletons?.SingleOrDefault(t => t.OwnerSeat == f.OwnerSeat)is not { } transition || transition.CardId != r.RevealedCard || transition.Rank != r.RevealedRank || r.RevealedRank is not (>= 1 and <= 13) || r.Stage is not (FeiYiStage.DirectionChoice or FeiYiStage.HezhongDrawChildren) || _coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiHezhongPaidEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Turn == r.Turn && e.TurnOwner == r.TurnOwner && e.BatchId == r.BatchId && e.CardId == r.RevealedCard && e.Rank == r.RevealedRank && e.Greater == r.Greater) != (r.Paid ? 1 : 0))
                throw new InvalidOperationException("Hezhong lost its actual singleton batch, selected direction or one-time draw invoice.");
        }

        if (r.Stage is FeiYiStage.DrawChildren or FeiYiStage.DiscardChildren or FeiYiStage.HezhongDrawChildren)
        {
            var facts = _coreProgramEventHistoryOperations.ProgramEventHistory<FeiYiMovementIssuedEvent>().Where(e => e.FrameId == f.Id && e.Stage == r.Stage && e.Before == r.Before && e.After == r.After).ToArray();
            if (!r.Paid || facts.Length != 1 || facts[0].Count != _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value == (r.Stage == FeiYiStage.DiscardChildren ? FeiYiDiscardReason : FeiYiDrawReason)))
                throw new InvalidOperationException("Fei Yi lost its one issued native movement interval.");
        }
    }

    internal bool FeiYiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.FeiYi is not { Paid: true } r || root.PendingMovementContinuation is null)
            return false;
        AssertFeiYi(root);
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && (moved.ResumeProgramFrameId == root.Id || moved.Batch.AwaitingProgramFrameId == root.Id) && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => m.Sequence > r.Before && m.Sequence <= r.After && (r.Stage == FeiYiStage.DiscardChildren ? m.From == CardLocation.Hand(root.OwnerSeat) && m.To == CardLocation.DiscardPile && m.Reason.Value == FeiYiDiscardReason : m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(root.OwnerSeat) && m.Reason.Value == FeiYiDrawReason));
        if (child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills)
            return skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
        if (child is ProgramLifecycleTriggerWindowFrame state && state.Continuation == ProgramLifecycleContinuation.ResumeCharacterStateChange)
            return state.ResumeProgramFrameId == root.Id && state.CharacterStateContinuation == CharacterStateContinuation.Program && _coreProgramEventHistoryOperations.ProgramEventHistory<CharacterStateChangedEvent>().Any(e => e.Change.Id == state.Id && e.Change.ParentFrameId == root.Id && e.Change.TargetSeat == state.OwnerSeat && e.Change.Window == state.Window);
        return false;
    }

    internal ProgramSkillFrame? FeiYiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterFeiYiOlOperations.FeiYiFirstChild(root, child), (index) => _coreObserverSubtreesOperations2.HasObserverSuffix(index, (child) => _coreObserverSubtreesOperations2.IsMovementObserverEdge(child) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(child) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[child], _resolutionStackOperations2._resolutionStack()[child - 1]), (child, dying) => _coreObserverSubtreesOperations2.IsSuspendedOrPaidRescueObserverSuffix(child, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 470)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 580)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 610)]
    internal bool HasFeiYiDamageObserver(long id) => FeiYiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 440)]
    internal bool IsFeiYiDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && FeiYiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26600)]
    internal bool AdvanceFeiYiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FeiYiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
