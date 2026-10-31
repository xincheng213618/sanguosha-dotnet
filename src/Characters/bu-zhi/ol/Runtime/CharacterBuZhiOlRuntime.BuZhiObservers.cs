#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterBuZhiOlRuntime
{
    internal bool BuZhiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BuZhi is not { } r || !ValidBuZhi(f))
            return false;
        if (r.Stage == "movement" && f.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        return r.Stage == "damage-issued" && r.DamageIssued && f.AttackAttempt is { } attack && f.AttackReturn is not null && attack.SourceSeat == f.OwnerSeat && attack.TargetSeat == r.Target && !attack.SourceLess && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? BuZhiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => this.BuZhiFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 130)]
    internal bool TryAdvanceBuZhiSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BuZhiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 70)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 70)]
    internal bool CanContinueBuZhi(ProgramSkillFrame f) => ValidBuZhi(f) && (f.BuZhi is { Stage: "movement" or "damage-issued" } || f.BuZhi is { Stage: "branch" or "discard" or "damage" } && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && BuZhiPaidEquipmentEntitlement(f));
}
