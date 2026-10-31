#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineDrawPhaseDrawHandLimitRuntime
{
    private readonly ICoreDrawPhaseDrawHandLimitCoreAlternatingBenefitsOperations _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess _coreActualTurnProgressTurnNumberTurnProgressionAccess;
    internal CoreGameEngineDrawPhaseDrawHandLimitRuntime(ICoreDrawPhaseDrawHandLimitCoreAlternatingBenefitsOperations coreAlternatingBenefitsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreActualTurnProgressTurnNumberTurnProgressionAccess coreActualTurnProgressTurnNumberTurnProgressionAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreActualTurnProgressTurnNumberTurnProgressionAccess = coreActualTurnProgressTurnNumberTurnProgressionAccess;
    }
}
