#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionHealthEntries
{
    internal bool IsResponseCompletionHealthChild(ProgramCardTriggerWindowFrame window, ResolutionFrame child) => _resolve().IsResponseCompletionHealthChild(window, child);
    internal bool ReturnCardResponseCompletionHpChange(HpChangedTriggerWindowFrame child) => _resolve().ReturnCardResponseCompletionHpChange(child);
    internal bool ReturnCardResponseCompletionRecovery(RecoveryReplacementFrame child) => _resolve().ReturnCardResponseCompletionRecovery(child);
    internal bool TryDrainResponseCompletionHealth(ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainResponseCompletionHealth(supplied);
}
