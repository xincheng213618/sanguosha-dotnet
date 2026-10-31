#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterFuRongOlRuntime
{
    internal FuRongPayment? FuRongCurrentCost(ProgramSkillFrame f) => f.FuRongPair is { } r ? r.Stage == FuRongPairStage.OwnerChildren ? r.OwnerPayment : r.Stage == FuRongPairStage.TargetChildren ? r.TargetPayment : null : null;
    internal bool FuRongFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.FuRongPair is not { } r || r.OwnerPayment is null)
            return false;
        if (r.Stage == FuRongPairStage.NativeUse && r.NativeReturn is null)
            return false;
        AssertFuRongPair(f);
        if (child is CardUseFrame use)
            return r.Stage == FuRongPairStage.NativeUse && r.NativeReturn is { } ret && ret.ProgramFrameId == f.Id && ret.CardUseFrameId == use.Id && use.FuRongReturn == ret && use.SourceSeat == f.OwnerSeat && use.CardId == ret.CardId && use.Action?.ActionId == ret.ActionId && use.PhysicalCardIds is [var physical] && physical == ret.CardId;
        if (child is CardsMovedTriggerWindowFrame moved)
        {
            if (moved.Batch.ParentFrameId != f.Id || moved.Batch.OriginOwnerSeat != f.OwnerSeat || moved.Batch.OriginSkillId != f.SkillId || moved.Batch.OriginSkillInstanceId != f.SkillInstanceId || moved.Batch.Movements.Count == 0 || moved.ResumeProgramFrameId != f.Id && moved.Batch.AwaitingProgramFrameId != f.Id)
                return false;
            if (FuRongCurrentCost(f)is { } paid)
                return moved.Batch.Movements is [var m] && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence == paid.Sequence && m.CardId == paid.CardId && m.From == paid.From && m.To == CardLocation.DiscardPile && m.Reason.Value == FuRongCostReason;
            if (r.Stage == FuRongPairStage.DrawChildren)
                return moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.DrawBefore && m.Sequence <= r.DrawAfter && (m.From == CardLocation.DrawPile && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == "skill-program.fu-rong.xiaosi.compensation" || m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile && m.Reason == CardMoveReasons.Reshuffle));
            return r.Stage == FuRongPairStage.NativeChildren && r.NativeReturn is { } native && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.CardId == native.CardId && m.From == CardLocation.Processing);
        }

        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } skills && skills.ResumeProgramFrameId == f.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? FuRongObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterFuRongOlOperations.FuRongFirstChild(root, child), (root) => _coreObserverSubtreesOperations.HasObserverSuffix(root, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, dying) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, dying)));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 410)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 520)]
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 550)]
    internal bool HasFuRongDamageObserver(long id) => FuRongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 380)]
    internal bool IsFuRongDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && FuRongObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26310)]
    internal bool AdvanceFuRongSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FuRongObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
}
