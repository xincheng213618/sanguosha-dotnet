#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShenwuActivitiesRuntime
{
    private readonly ICoreShenwuActivitiesModeRuntimeOperations _modeRuntimeOperations;
    internal CoreGameEngineShenwuActivitiesRuntime(ICoreShenwuActivitiesModeRuntimeOperations modeRuntimeOperations)
    {
        this._modeRuntimeOperations = modeRuntimeOperations;
    }
}
