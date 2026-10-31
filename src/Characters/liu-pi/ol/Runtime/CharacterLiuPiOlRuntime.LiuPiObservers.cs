#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuPiOlRuntime
{
    internal bool LiuPiFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.LiuPiExchange is not { } r)
            return false;
        if (child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId == root.Id && moved.Batch.ParentFrameId == root.Id && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.SequenceBefore && m.Sequence <= r.SequenceAfter && (m.Reason.Value == LiuPiMovementReason(r.Stage) || r.Stage == LiuPiExchangeStage.RevealChildren && m.Reason == CardMoveReasons.Reshuffle && m.From == CardLocation.DiscardPile && m.To == CardLocation.DrawPile));
        return child is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.SkillsChanged } changed && changed.ResumeProgramFrameId == root.Id && changed.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? LiuPiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _characterLiuPiOlOperations.LiuPiFirstChild(root, child), (index) => _coreObserverSubtreesOperations.HasMovementObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 350)]
    [ProgramObserverProof(ProgramObserverProofPhase.NestedDamage, 460)]
    internal bool HasLiuPiDamageObserver(long id) => LiuPiObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame or BeforeDamageProgramWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 26120)]
    internal bool AdvanceLiuPiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && LiuPiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
