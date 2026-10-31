#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramContinuationsEntries
{
    internal bool TryBeginProgramChildWindows(long frameId, global::CardGame.Core.PostEventContinuation continuation) => _resolve().TryBeginProgramChildWindows(frameId, continuation);
}
