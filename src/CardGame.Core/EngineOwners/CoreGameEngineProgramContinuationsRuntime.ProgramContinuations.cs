#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Reflection;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramContinuationsRuntime
{
    internal bool TryBeginProgramChildWindows(long frameId, PostEventContinuation continuation) => _programChildWindowOperations.Get().Begin(frameId, continuation);
}
