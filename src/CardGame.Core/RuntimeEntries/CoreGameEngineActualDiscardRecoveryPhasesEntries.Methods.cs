#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineActualDiscardRecoveryPhasesEntries
{
    internal bool ActualDiscardEntityStillAvailable(global::CardGame.Core.DiscardRecoveryEntity entity) => _resolve().ActualDiscardEntityStillAvailable(entity);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.DiscardRecoveryEntity> ActualDiscardRecoveryEntities(global::CardGame.Core.CardMovementBatchContext batch, int source) => _resolve().ActualDiscardRecoveryEntities(batch, source);
    internal bool ActualDiscardRecoveryPhaseUsed(int owner, string skill, string stateId, global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().ActualDiscardRecoveryPhaseUsed(owner, skill, stateId, phase);
    internal global::CardGame.Core.ActualDiscardRecoveryBatchQualifiedEvent? ActualDiscardRecoveryQualification(global::CardGame.Core.CardMovementBatchContext batch, int source) => _resolve().ActualDiscardRecoveryQualification(batch, source);
    internal void ClearActualDiscardRecoveryPhase() => _resolve().ClearActualDiscardRecoveryPhase();
    internal global::CardGame.Core.ActualDiscardRecoveryPhaseKey? CurrentActualDiscardRecoveryPhase() => _resolve().CurrentActualDiscardRecoveryPhase();
    internal bool IsRecordedActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().IsRecordedActualDiscardRecoveryPhase(phase);
    internal int[] MatchingActualDiscardRecoveryIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MatchingActualDiscardRecoveryIndexes(batch, candidate);
    internal void RestoreActualDiscardRecoveryPhase(global::CardGame.Core.DrawPhaseObligationFrame parent) => _resolve().RestoreActualDiscardRecoveryPhase(parent);
    internal void RestoreActualDiscardRecoveryPhase2(global::CardGame.Core.ProgramPhaseSchedule schedule) => _resolve().RestoreActualDiscardRecoveryPhase(schedule);
    internal void StartActualDiscardRecoveryPhase(global::CardGame.Core.ActualDiscardRecoveryPhaseKind kind, int actor) => _resolve().StartActualDiscardRecoveryPhase(kind, actor);
}
