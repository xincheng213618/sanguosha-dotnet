#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDyingEntryProgramsEntries
{
    internal void CompleteDyingEntryProgramWindow(ProgramLifecycleTriggerWindowFrame frame) => _resolve().CompleteDyingEntryProgramWindow(frame);
    internal bool TryBeginDyingEntryProgramWindow(DyingFrame dying) => _resolve().TryBeginDyingEntryProgramWindow(dying);
}
