#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineResponseCompletionHealthRuntime
{
    internal static bool IsResponseCompletionSilverLionAttempt(IReadOnlyList<CardMovementRecord> costs, RecoveryAttempt a) => RuntimeSkills.RuntimeRules_Core_ResponseCompletionHealth.IsResponseCompletionSilverLionAttempt(costs, a);
    internal static bool IsResponseCompletionSilverLionHealth(IReadOnlyList<CardMovementRecord> costs, HpChangeContext h) => RuntimeSkills.RuntimeRules_Core_ResponseCompletionHealth.IsResponseCompletionSilverLionHealth(costs, h);
    internal static bool SameResponseCompletionRecovery(RecoveryAttempt a, RecoveryAttempt b) => RuntimeSkills.RuntimeRules_Core_ResponseCompletionHealth.SameResponseCompletionRecovery(a, b);
    internal bool TryDrainResponseCompletionHealth(ProgramCardTriggerWindowFrame supplied) => _responseCompletionOperations.Get().TryDrainResponseCompletionHealth(supplied);
    internal bool IsResponseCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _responseCompletionOperations.Get().IsResponseCompletionHealthChild(window, child);
    internal bool ReturnCardResponseCompletionRecovery(RecoveryReplacementFrame child) => _responseCompletionOperations.Get().ReturnCardResponseCompletionRecovery(child);
    internal bool ReturnCardResponseCompletionHpChange(HpChangedTriggerWindowFrame child) => _responseCompletionOperations.Get().ReturnCardResponseCompletionHpChange(child);
}
