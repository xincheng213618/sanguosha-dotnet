#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePrivateWhirlObserversRuntime
{
    internal bool WhirlFirstChild(ProgramSkillFrame root, ResolutionFrame child)
    {
        if (!_corePrivateWhirlOperations.CanContinuePrivateWhirl(root) || root.PrivateWhirl is not { } r || child is not CardsMovedTriggerWindowFrame moved)
            return false;
        return moved.Batch.ParentFrameId == root.Id && moved.Batch.AwaitingProgramFrameId == root.Id && moved.ResumeProgramFrameId is null && moved.Batch.OriginOwnerSeat == root.OwnerSeat && moved.Batch.OriginSkillId == root.SkillId && moved.Batch.OriginSkillInstanceId == root.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After && m.Reason.Value.StartsWith("skill-program.private-whirl.", StringComparison.Ordinal));
    }

    internal ProgramSkillFrame? PrivateWhirlObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((root, child) => _corePrivateWhirlObserversOperations.WhirlFirstChild(root, child), (index) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(index));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 120)]
    internal bool TryAdvancePrivateWhirlSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && PrivateWhirlObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(advanceRecoveryReplacement: true);
}
