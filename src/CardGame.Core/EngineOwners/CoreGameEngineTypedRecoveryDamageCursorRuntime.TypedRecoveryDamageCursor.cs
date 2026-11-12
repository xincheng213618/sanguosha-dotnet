#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTypedRecoveryDamageCursorRuntime
{
    internal HealthContinuationRuntime HealthContinuations => _healthContinuationOperations.Get();

    internal TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor() => HealthContinuations.ProjectTypedCommittedSlashPaymentCursor();
    internal bool TypedCommittedSlashPaymentActionMatches(CardUseFrame use, CardActionContext action, int rootIndex) => HealthContinuations.TypedCommittedSlashPaymentActionMatches(use, action, rootIndex);
    internal TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor() => HealthContinuations.ProjectTypedResponseCompletionHealthCursor();
    internal TypedPaidObserverSuffixProjection ProjectTypedPaidObserverSuffix(int firstIndex, Func<IReadOnlyList<IGameEvent>> history, bool exactOrdinaryHpProducer = false) => HealthContinuations.ProjectTypedPaidObserverSuffix(firstIndex, history, exactOrdinaryHpProducer);
    internal static bool TypedPaidDyingEntryReturnMatches(DyingFrame dying, ProgramLifecycleTriggerWindowFrame entry) => RuntimeSkills.RuntimeRules_Core_TypedRecoveryDamageCursor.TypedPaidDyingEntryReturnMatches(dying, entry);
    internal bool TypedPaidDyingEntryObserverMatches(ProgramLifecycleTriggerWindowFrame entry, ProgramSkillFrame observer, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedPaidDyingEntryObserverMatches(entry, observer, history);
    internal bool TypedOrdinaryPaidHpProducerMatches(ProgramSkillFrame producer, SkillProgramEffect effect, HpChangedTriggerWindowFrame hp) => HealthContinuations.TypedOrdinaryPaidHpProducerMatches(producer, effect, hp);
    internal static bool IsTypedRecoveryTargetSupported(RecoverProgramInstruction instruction) => RuntimeSkills.RuntimeRules_Core_TypedRecoveryDamageCursor.IsTypedRecoveryTargetSupported(instruction);
    internal bool UnsupportedTypedRecoveryTargetIsMalformed(ProgramSkillFrame producer, RecoverProgramInstruction instruction, int actualTarget) => HealthContinuations.UnsupportedTypedRecoveryTargetIsMalformed(producer, instruction, actualTarget);
    internal bool TypedResponseCompletionRecoveryMatches(RecoveryReplacementFrame recovery) => HealthContinuations.TypedResponseCompletionRecoveryMatches(recovery);
    internal bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery) => HealthContinuations.TypedRecoveryApplicationBoundsMatch(recovery);
    internal static bool IsRecoveryCursorPrimitive(ProgramSkillInstruction? instruction) => RuntimeSkills.RuntimeRules_Core_TypedRecoveryDamageCursor.IsRecoveryCursorPrimitive(instruction);
    internal SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer) => HealthContinuations.TypedRecoveryPausedEffect(producer);
    internal bool DirectProgramRecoveryReturnMatches(ProgramSkillFrame producer, SkillProgramEffect effect, RecoverProgramInstruction instruction, RecoveryReplacementFrame recovery) => HealthContinuations.DirectProgramRecoveryReturnMatches(producer, effect, instruction, recovery);
    internal bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp) => HealthContinuations.TypedRecoveryHpReturnMatches(recovery, hp);
    internal bool TypedRecoveryChosenCandidateMatches(RecoveryReplacementFrame recovery) => HealthContinuations.TypedRecoveryChosenCandidateMatches(recovery);
    internal bool TypedRecoveryRewardReturnMatches(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement) => HealthContinuations.TypedRecoveryRewardReturnMatches(recovery, movement);
    internal bool TypedRecoveryRewardReturnMatches(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement, ProgramSkillFrame? origin) => HealthContinuations.TypedRecoveryRewardReturnMatches(recovery, movement, origin);
    internal bool TypedRecoveryRewardBatchMatchesReceipt(RecoveryReplacementFrame recovery, CardMovementBatchContext batch) => HealthContinuations.TypedRecoveryRewardBatchMatchesReceipt(recovery, batch);
    internal bool TypedRecoveryProgramDyingMatches(ProgramSkillFrame producer, DyingFrame dying, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryProgramDyingMatches(producer, dying, history);
    internal bool TypedRecoveryDyingResponseMatches(DyingFrame dying, ProgramSkillFrame response, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryDyingResponseMatches(dying, response, history);
    internal bool HasTypedRecoveryProgramDying() => HealthContinuations.HasTypedRecoveryProgramDying();
}
