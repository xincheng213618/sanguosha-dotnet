#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesRuntime
{
    internal ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => _actualDiscardRecoveryPhaseOperations.Get().Current();
    internal void StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind kind, int actor) => _actualDiscardRecoveryPhaseOperations.Get().Start(kind, actor);
    internal bool IsRecordedActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKey phase) => _actualDiscardRecoveryPhaseOperations.Get().IsRecorded(phase);
    internal void ClearActualDiscardRecoveryPhase() => _actualDiscardRecoveryPhaseOperations.Get().Clear();
    internal void RestoreActualDiscardRecoveryPhase(ProgramPhaseSchedule schedule) => _actualDiscardRecoveryPhaseOperations.Get().Restore(schedule);
    internal void RestoreActualDiscardRecoveryPhase(DrawPhaseObligationFrame parent) => _actualDiscardRecoveryPhaseOperations.Get().Restore(parent);
    internal IReadOnlyList<DiscardRecoveryEntity> ActualDiscardRecoveryEntities(CardMovementBatchContext batch, int source) => _actualDiscardRecoveryPhaseOperations.Get().Entities(batch, source);
    internal bool ActualDiscardEntityStillAvailable(DiscardRecoveryEntity entity) => _actualDiscardRecoveryPhaseOperations.Get().StillAvailable(entity);
    internal ActualDiscardRecoveryBatchQualifiedEvent? ActualDiscardRecoveryQualification(CardMovementBatchContext batch, int source) => _actualDiscardRecoveryPhaseOperations.Get().Qualification(batch, source);
    internal bool ActualDiscardRecoveryPhaseUsed(int owner, string skill, string stateId, ActualDiscardRecoveryPhaseKey phase) => _actualDiscardRecoveryPhaseOperations.Get().PhaseUsed(owner, skill, stateId, phase);
    internal int[] MatchingActualDiscardRecoveryIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _actualDiscardRecoveryPhaseOperations.Get().MatchingIndexes(batch, candidate);
}
