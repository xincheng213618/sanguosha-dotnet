#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineTypedRecoveryDamageCursorEntries
{
    internal bool DirectProgramRecoveryReturnMatches(ProgramSkillFrame producer, SkillProgramEffect effect, RecoverProgramInstruction instruction, RecoveryReplacementFrame recovery) => _resolve().DirectProgramRecoveryReturnMatches(producer, effect, instruction, recovery);
    internal bool HasTypedRecoveryProgramDying() => _resolve().HasTypedRecoveryProgramDying();
    internal TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor() => _resolve().ProjectTypedCommittedSlashPaymentCursor();
    internal TypedPaidObserverSuffixProjection ProjectTypedPaidObserverSuffix(int firstIndex, Func<IReadOnlyList<IGameEvent>> history, bool exactOrdinaryHpProducer = false) => _resolve().ProjectTypedPaidObserverSuffix(firstIndex, history, exactOrdinaryHpProducer);
    internal TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor() => _resolve().ProjectTypedResponseCompletionHealthCursor();
    internal bool TypedCommittedSlashPaymentActionMatches(CardUseFrame use, CardActionContext action, int rootIndex) => _resolve().TypedCommittedSlashPaymentActionMatches(use, action, rootIndex);
    internal bool TypedOrdinaryPaidHpProducerMatches(ProgramSkillFrame producer, SkillProgramEffect effect, HpChangedTriggerWindowFrame hp) => _resolve().TypedOrdinaryPaidHpProducerMatches(producer, effect, hp);
    internal bool TypedPaidDyingEntryObserverMatches(ProgramLifecycleTriggerWindowFrame entry, ProgramSkillFrame observer, IReadOnlyList<IGameEvent> history) => _resolve().TypedPaidDyingEntryObserverMatches(entry, observer, history);
    internal bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery) => _resolve().TypedRecoveryApplicationBoundsMatch(recovery);
    internal bool TypedRecoveryChosenCandidateMatches(RecoveryReplacementFrame recovery) => _resolve().TypedRecoveryChosenCandidateMatches(recovery);
    internal bool TypedRecoveryDyingResponseMatches(DyingFrame dying, ProgramSkillFrame response, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryDyingResponseMatches(dying, response, history);
    internal bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp) => _resolve().TypedRecoveryHpReturnMatches(recovery, hp);
    internal SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer) => _resolve().TypedRecoveryPausedEffect(producer);
    internal bool TypedRecoveryProgramDyingMatches(ProgramSkillFrame producer, DyingFrame dying, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryProgramDyingMatches(producer, dying, history);
    internal bool TypedRecoveryRewardBatchMatchesReceipt(RecoveryReplacementFrame recovery, CardMovementBatchContext batch) => _resolve().TypedRecoveryRewardBatchMatchesReceipt(recovery, batch);
    internal bool TypedRecoveryRewardReturnMatches(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement) => _resolve().TypedRecoveryRewardReturnMatches(recovery, movement);
    internal bool TypedRecoveryRewardReturnMatches2(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement, ProgramSkillFrame? origin) => _resolve().TypedRecoveryRewardReturnMatches(recovery, movement, origin);
    internal bool TypedResponseCompletionRecoveryMatches(RecoveryReplacementFrame recovery) => _resolve().TypedResponseCompletionRecoveryMatches(recovery);
    internal bool UnsupportedTypedRecoveryTargetIsMalformed(ProgramSkillFrame producer, RecoverProgramInstruction instruction, int actualTarget) => _resolve().UnsupportedTypedRecoveryTargetIsMalformed(producer, instruction, actualTarget);
    internal HealthContinuationRuntime HealthContinuations { get => _resolve().HealthContinuations; }
}
