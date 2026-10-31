#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchPresentationRuntime
{
    private readonly ICoreMatchPresentationCoreMatchSetupDefinitionsOperations _coreMatchSetupDefinitionsOperations;
    private readonly ICoreMatchPresentationCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess _coreMatchDefinitionModeDefinitionAccess;
    internal CoreMatchPresentationRuntime(ICoreMatchPresentationCoreMatchSetupDefinitionsOperations coreMatchSetupDefinitionsOperations, ICoreMatchPresentationCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionAccess coreMatchDefinitionModeDefinitionAccess)
    {
        this._coreMatchSetupDefinitionsOperations = coreMatchSetupDefinitionsOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreMatchDefinitionModeDefinitionAccess = coreMatchDefinitionModeDefinitionAccess;
    }
}
