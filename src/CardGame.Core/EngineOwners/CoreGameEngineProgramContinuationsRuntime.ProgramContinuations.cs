#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramContinuationsRuntime
{
    internal bool TryBeginProgramChildWindows(long frameId, PostEventContinuation continuation) => _programChildWindowOperations.Get().Begin(frameId, continuation);
}
