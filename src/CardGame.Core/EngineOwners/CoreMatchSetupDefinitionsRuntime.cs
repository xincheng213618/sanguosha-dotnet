#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchSetupDefinitionsRuntime
{
    private readonly ICoreMatchSetupDefinitionsCoreModeRulesOperations _coreModeRulesOperations;
    private readonly ICoreMatchSetupDefinitionsCorePrivateGeneralLibrariesOperations _corePrivateGeneralLibrariesOperations;
    private readonly ICoreMatchSetupDefinitionsCoreNativeDamageHandlesOperations _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    internal CoreMatchSetupDefinitionsRuntime(ICoreMatchSetupDefinitionsCoreModeRulesOperations coreModeRulesOperations, ICoreMatchSetupDefinitionsCorePrivateGeneralLibrariesOperations corePrivateGeneralLibrariesOperations, ICoreMatchSetupDefinitionsCoreNativeDamageHandlesOperations coreNativeDamageHandlesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess)
    {
        this._coreModeRulesOperations = coreModeRulesOperations;
        this._corePrivateGeneralLibrariesOperations = corePrivateGeneralLibrariesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
    }
}
