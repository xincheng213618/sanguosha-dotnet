#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineTypedRecoveryDamageCursorRuntime
{
    internal HealthContinuationRuntime HealthContinuations => _healthContinuationOperations.Get();

    internal TypedCommittedSlashPaymentCursorProjection ProjectTypedCommittedSlashPaymentCursor() => HealthContinuations.ProjectTypedCommittedSlashPaymentCursor();
    internal bool TypedCommittedSlashPaymentActionMatches(CardUseFrame use, CardActionContext action, int rootIndex) => HealthContinuations.TypedCommittedSlashPaymentActionMatches(use, action, rootIndex);
    internal TypedResponseCompletionHealthCursorProjection ProjectTypedResponseCompletionHealthCursor() => HealthContinuations.ProjectTypedResponseCompletionHealthCursor();
    internal TypedPaidObserverSuffixProjection ProjectTypedPaidObserverSuffix(int firstIndex, Func<IReadOnlyList<IGameEvent>> history, bool exactOrdinaryHpProducer = false) => HealthContinuations.ProjectTypedPaidObserverSuffix(firstIndex, history, exactOrdinaryHpProducer);
    internal static bool TypedPaidDyingEntryReturnMatches(DyingFrame dying, ProgramLifecycleTriggerWindowFrame entry) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TypedRecoveryDamageCursor.TypedPaidDyingEntryReturnMatches(dying, entry);
    internal bool TypedPaidDyingEntryObserverMatches(ProgramLifecycleTriggerWindowFrame entry, ProgramSkillFrame observer, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedPaidDyingEntryObserverMatches(entry, observer, history);
    internal bool TypedOrdinaryPaidHpProducerMatches(ProgramSkillFrame producer, SkillProgramEffect effect, HpChangedTriggerWindowFrame hp) => HealthContinuations.TypedOrdinaryPaidHpProducerMatches(producer, effect, hp);
    internal static bool IsTypedRecoveryTargetSupported(RecoverProgramInstruction instruction) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TypedRecoveryDamageCursor.IsTypedRecoveryTargetSupported(instruction);
    internal bool UnsupportedTypedRecoveryTargetIsMalformed(ProgramSkillFrame producer, RecoverProgramInstruction instruction, int actualTarget) => HealthContinuations.UnsupportedTypedRecoveryTargetIsMalformed(producer, instruction, actualTarget);
    internal bool TypedResponseCompletionRecoveryMatches(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedResponseCompletionRecoveryMatches(recovery, history);
    internal bool TypedRecoveryApplicationBoundsMatch(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryApplicationBoundsMatch(recovery, history);
    internal static bool IsRecoveryCursorPrimitive(ProgramSkillInstruction? instruction) => global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_TypedRecoveryDamageCursor.IsRecoveryCursorPrimitive(instruction);
    internal SkillProgramEffect? TypedRecoveryPausedEffect(ProgramSkillFrame producer) => HealthContinuations.TypedRecoveryPausedEffect(producer);
    internal bool DirectProgramRecoveryReturnMatches(ProgramSkillFrame producer, SkillProgramEffect effect, RecoverProgramInstruction instruction, RecoveryReplacementFrame recovery) => HealthContinuations.DirectProgramRecoveryReturnMatches(producer, effect, instruction, recovery);
    internal bool TypedRecoveryHpReturnMatches(RecoveryReplacementFrame recovery, HpChangedTriggerWindowFrame hp, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryHpReturnMatches(recovery, hp, history);
    internal bool TypedRecoveryChosenCandidateMatches(RecoveryReplacementFrame recovery, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryChosenCandidateMatches(recovery, history);
    internal bool TypedRecoveryRewardReturnMatches(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryRewardReturnMatches(recovery, movement, history);
    internal bool TypedRecoveryRewardReturnMatches(RecoveryReplacementFrame recovery, CardsMovedTriggerWindowFrame movement, IReadOnlyList<IGameEvent> history, ProgramSkillFrame? origin) => HealthContinuations.TypedRecoveryRewardReturnMatches(recovery, movement, history, origin);
    internal bool TypedRecoveryRewardBatchMatchesReceipt(RecoveryReplacementFrame recovery, CardMovementBatchContext batch) => HealthContinuations.TypedRecoveryRewardBatchMatchesReceipt(recovery, batch);
    internal bool TypedRecoveryProgramDyingMatches(ProgramSkillFrame producer, DyingFrame dying, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryProgramDyingMatches(producer, dying, history);
    internal bool TypedRecoveryDyingResponseMatches(DyingFrame dying, ProgramSkillFrame response, IReadOnlyList<IGameEvent> history) => HealthContinuations.TypedRecoveryDyingResponseMatches(dying, response, history);
    internal bool HasTypedRecoveryProgramDying() => HealthContinuations.HasTypedRecoveryProgramDying();
}
