#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineExtraEffectsAndRankGuessObserversRuntime
{
    internal bool ExtraEffectsAndRankGuessFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.ExtraEffectsAndRankGuess is not { } r)
            return false;
        _coreExtraEffectsAndRankGuessingOperations.RequireExtraEffectsAndRankGuess(root);
        if (root.PendingMovementContinuation is { } movement && _coreExtraEffectsAndRankGuessingOperations.IsExtraEffectsAndRankGuessMovement(root, _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(root), movement) && child is CardsMovedTriggerWindowFrame moved)
            return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.MovementBefore && m.Sequence <= r.MovementAfter && m.Reason.Value is "skill-program.rank-guess.reveal" or "skill-program.rank-guess.gain" or "skill-program.extra-effect.cost");
        return child is ProgramLifecycleTriggerWindowFrame skills && skills.Window == SkillProgramTriggerWindow.SkillsChanged && skills.ResumeProgramFrameId == root.Id && skills.Continuation == ProgramLifecycleContinuation.ResumeParentProgram;
    }

    internal ProgramSkillFrame? ExtraEffectsAndRankGuessObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreExtraEffectsAndRankGuessObserversOperations.ExtraEffectsAndRankGuessFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1600)]
    internal bool HasExtraEffectsAndRankGuessDamageObserver(long id) => ExtraEffectsAndRankGuessObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 90)]
    internal bool TryAdvanceExtraEffectsAndRankGuessSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && ExtraEffectsAndRankGuessObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
