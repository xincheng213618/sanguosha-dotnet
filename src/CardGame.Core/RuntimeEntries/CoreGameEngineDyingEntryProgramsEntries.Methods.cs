#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingEntryProgramsEntries
{
    internal void CompleteDyingEntryProgramWindow(global::CardGame.Core.ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteDyingEntryProgramWindow(frame);
    internal bool TryBeginDyingEntryProgramWindow(global::CardGame.Core.DyingFrame dying) => _resolve().TryBeginDyingEntryProgramWindow(dying);
}
