#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreGameEngineBossActivitiesRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries _coreAlternatingBenefitsOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess _coreMatchDefinitionOptionsPlayerCountAccess;
    internal CoreGameEngineBossActivitiesRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineAlternatingBenefitsEntries coreAlternatingBenefitsOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionOptionsPlayerCountAccess coreMatchDefinitionOptionsPlayerCountAccess)
    {
        this._coreAlternatingBenefitsOperations = coreAlternatingBenefitsOperations;
        this._coreMatchDefinitionOptionsPlayerCountAccess = coreMatchDefinitionOptionsPlayerCountAccess;
    }
}
