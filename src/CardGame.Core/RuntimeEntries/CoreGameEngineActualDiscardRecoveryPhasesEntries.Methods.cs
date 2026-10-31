#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesEntries
{
    internal bool ActualDiscardEntityStillAvailable(DiscardRecoveryEntity entity) => _resolve().ActualDiscardEntityStillAvailable(entity);
    internal IReadOnlyList<DiscardRecoveryEntity> ActualDiscardRecoveryEntities(CardMovementBatchContext batch, int source) => _resolve().ActualDiscardRecoveryEntities(batch, source);
    internal bool ActualDiscardRecoveryPhaseUsed(int owner, string skill, string stateId, ActualDiscardRecoveryPhaseKey phase) => _resolve().ActualDiscardRecoveryPhaseUsed(owner, skill, stateId, phase);
    internal void ClearActualDiscardRecoveryPhase() => _resolve().ClearActualDiscardRecoveryPhase();
    internal ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => _resolve().CurrentActualDiscardRecoveryPhase();
    internal bool IsRecordedActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKey phase) => _resolve().IsRecordedActualDiscardRecoveryPhase(phase);
    internal int[] MatchingActualDiscardRecoveryIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate) => _resolve().MatchingActualDiscardRecoveryIndexes(batch, candidate);
    internal void RestoreActualDiscardRecoveryPhase(DrawPhaseObligationFrame parent) => _resolve().RestoreActualDiscardRecoveryPhase(parent);
    internal void RestoreActualDiscardRecoveryPhase2(ProgramPhaseSchedule schedule) => _resolve().RestoreActualDiscardRecoveryPhase(schedule);
    internal void StartActualDiscardRecoveryPhase(ActualDiscardRecoveryPhaseKind kind, int actor) => _resolve().StartActualDiscardRecoveryPhase(kind, actor);
}
