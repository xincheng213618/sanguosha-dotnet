#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDirectSkillDamageRuntime
{
    private readonly ICoreDirectSkillDamageResolutionStackOperations _resolutionStackOperations;
    internal CoreGameEngineDirectSkillDamageRuntime(ICoreDirectSkillDamageResolutionStackOperations resolutionStackOperations)
    {
        this._resolutionStackOperations = resolutionStackOperations;
    }
}
