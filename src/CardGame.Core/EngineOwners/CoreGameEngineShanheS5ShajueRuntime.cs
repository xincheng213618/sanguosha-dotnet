#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineShanheS5ShajueRuntime
{
    private readonly ICoreShanheS5ShajueResolutionStackOperations _resolutionStackOperations;
    internal CoreGameEngineShanheS5ShajueRuntime(ICoreShanheS5ShajueResolutionStackOperations resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
