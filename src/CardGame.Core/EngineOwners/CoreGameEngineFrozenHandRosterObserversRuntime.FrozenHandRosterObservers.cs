#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineFrozenHandRosterObserversRuntime
{
    internal bool FrozenHandRosterFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.FrozenHandRoster is not { } r || !_coreFrozenHandRosterOperations.ValidFrozenHandRoster(f))
            return false;
        if (r.Stage == "movement" && f.PendingMovementContinuation is not null && child is CardsMovedTriggerWindowFrame moved)
            return moved.ResumeProgramFrameId is null && moved.Batch.ParentFrameId == f.Id && moved.Batch.AwaitingProgramFrameId == f.Id && moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
        return r.Stage == "damage-issued" && r.DamageIssued && f.AttackAttempt is { } attack && f.AttackReturn is not null && attack.SourceSeat == f.OwnerSeat && attack.TargetSeat == r.DamageTarget && !attack.SourceLess && _coreDyingSuitsAndHistoricalUseObserversOperations.DyingSuitsStructuralEdge(f, child);
    }

    internal ProgramSkillFrame? FrozenHandRosterObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((r, c) => _coreFrozenHandRosterObserversOperations.FrozenHandRosterFirstChild(r, c), (i) => _coreSameNameHandDiscardOrDamageObserversOperations.SameNameHandObserverSuffix(i));
    [ProgramDyingProof(ProgramDyingProofPhase.Invariant, 80, group: 10)]
    internal bool IsFrozenHandRosterDying() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && FrozenHandRosterObserverRoot()is { } r && _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == r.Id);
    [ProgramObserverProof(ProgramObserverProofPhase.PaidDamage, 1740)]
    internal bool HasFrozenHandRosterDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is DamageTriggerWindowFrame) && FrozenHandRosterObserverRoot()is not null;
    [ProgramObserverProof(ProgramObserverProofPhase.BeforeDamage, 260)]
    internal bool HasFrozenHandRosterBeforeDamageObserver(long id) => _resolutionStackOperations._resolutionStack.Any(f => f.Id == id && f is BeforeDamageProgramWindowFrame) && FrozenHandRosterObserverRoot()is not null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 520)]
    internal bool TryAdvanceFrozenHandRosterSubtree() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && FrozenHandRosterObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 530)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 550)]
    internal bool CanContinueFrozenHandRoster(ProgramSkillFrame f) => f.FrozenHandRoster is { Stage: "movement" or "damage-issued" } && _coreFrozenHandRosterOperations.ValidFrozenHandRoster(f);
}
