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
    internal bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryApplicationBoundsMatch(recovery, history);
    internal bool TypedRecoveryChosenCandidateMatches(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryChosenCandidateMatches(recovery, history);
    internal bool TypedRecoveryDyingResponseMatches(DyingFrame dying, ProgramSkillFrame response, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryDyingResponseMatches(dying, response, history);
    internal bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryHpReturnMatches(recovery, hp, history);
    internal SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer) => _resolve().TypedRecoveryPausedEffect(producer);
    internal bool TypedRecoveryProgramDyingMatches(ProgramSkillFrame producer, DyingFrame dying, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryProgramDyingMatches(producer, dying, history);
    internal bool TypedRecoveryRewardBatchMatchesReceipt(RecoveryReplacementFrame recovery, CardMovementBatchContext batch) => _resolve().TypedRecoveryRewardBatchMatchesReceipt(recovery, batch);
    internal bool TypedRecoveryRewardReturnMatches(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement, IReadOnlyList<IGameEvent> history) => _resolve().TypedRecoveryRewardReturnMatches(recovery, movement, history);
    internal bool TypedRecoveryRewardReturnMatches2(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement, IReadOnlyList<IGameEvent> history, ProgramSkillFrame? origin) => _resolve().TypedRecoveryRewardReturnMatches(recovery, movement, history, origin);
    internal bool TypedResponseCompletionRecoveryMatches(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => _resolve().TypedResponseCompletionRecoveryMatches(recovery, history);
    internal bool UnsupportedTypedRecoveryTargetIsMalformed(ProgramSkillFrame producer, RecoverProgramInstruction instruction, int actualTarget) => _resolve().UnsupportedTypedRecoveryTargetIsMalformed(producer, instruction, actualTarget);
    internal HealthContinuationRuntime HealthContinuations { get => _resolve().HealthContinuations; }
}
