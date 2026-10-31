#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineResponseCompletionHealthEntries
{
    internal bool IsResponseCompletionHealthChild(global::CardGame.Core.ProgramCardTriggerWindowFrame window, global::CardGame.Core.ResolutionFrame child) => _resolve().IsResponseCompletionHealthChild(window, child);
    internal bool ReturnCardResponseCompletionHpChange(global::CardGame.Core.HpChangedTriggerWindowFrame child) => _resolve().ReturnCardResponseCompletionHpChange(child);
    internal bool ReturnCardResponseCompletionRecovery(global::CardGame.Core.RecoveryReplacementFrame child) => _resolve().ReturnCardResponseCompletionRecovery(child);
    internal bool TryDrainResponseCompletionHealth(global::CardGame.Core.ProgramCardTriggerWindowFrame supplied) => _resolve().TryDrainResponseCompletionHealth(supplied);
}
