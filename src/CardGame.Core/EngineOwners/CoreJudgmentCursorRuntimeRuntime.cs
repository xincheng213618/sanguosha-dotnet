#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreJudgmentCursorRuntimeRuntime
{
    private readonly ICoreJudgmentCursorRuntimeResolutionStackOperations _resolutionStackOperations;
    internal CoreJudgmentCursorRuntimeRuntime(ICoreJudgmentCursorRuntimeResolutionStackOperations resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
