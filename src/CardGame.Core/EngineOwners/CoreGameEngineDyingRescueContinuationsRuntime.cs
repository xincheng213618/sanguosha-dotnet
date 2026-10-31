#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDyingRescueContinuationsRuntime
{
    private readonly ICoreDyingRescueContinuationsCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    internal CoreGameEngineDyingRescueContinuationsRuntime(ICoreDyingRescueContinuationsCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
    }
}
