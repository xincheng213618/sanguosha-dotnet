#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOldHandDispositionObserversRuntime
{
    internal bool OldHandDispositionFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (root.OldHandDisposition is not { Stage: "movement" } r || root.PendingMovementContinuation is not { } pending || !_coreOldHandDispositionOperations.IsOldHandDispositionMovement(root, _coreOldHandDispositionOperations.OldHandPausedEffect(root), pending) || child is not CardsMovedTriggerWindowFrame moved || moved.Batch.ParentFrameId != root.Id || moved.ResumeProgramFrameId is not null || moved.Batch.AwaitingProgramFrameId != root.Id || moved.Batch.OriginOwnerSeat != root.OwnerSeat || moved.Batch.OriginSkillId != root.SkillId || moved.Batch.OriginSkillInstanceId != root.SkillInstanceId || moved.Batch.Movements.Count == 0 || moved.Batch.Movements.Any(m => !_coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) || m.Sequence <= r.Before || m.Sequence > r.After))
            return false;
        _coreOldHandDispositionOperations.AssertOldHandDisposition(root);
        return true;
    }

    internal ProgramSkillFrame? OldHandDispositionObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _coreOldHandDispositionObserversOperations.OldHandDispositionFirstChild(root, child), (index) => _coreObserverSubtreesOperations2.HasHandGainDebtObserverSuffix(index));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 70, group: 10)]
    internal bool IsOldHandDispositionProgramDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } dying && OldHandDispositionObserverRoot()is { } root && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == dying.FrameId) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == root.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1730)]
    internal bool HasOldHandDispositionDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && OldHandDispositionObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 510)]
    internal bool TryAdvanceOldHandDispositionSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && OldHandDispositionObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(false);
}
