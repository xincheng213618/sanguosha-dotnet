#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineHpOrderedDistancePoliciesRuntime
{
    private readonly ICoreHpOrderedDistancePoliciesCoreProgramCardPoliciesOperations _coreProgramCardPoliciesOperations;
    internal CoreGameEngineHpOrderedDistancePoliciesRuntime(ICoreHpOrderedDistancePoliciesCoreProgramCardPoliciesOperations coreProgramCardPoliciesOperations)
    {
        this._coreProgramCardPoliciesOperations = coreProgramCardPoliciesOperations;
    }
}
