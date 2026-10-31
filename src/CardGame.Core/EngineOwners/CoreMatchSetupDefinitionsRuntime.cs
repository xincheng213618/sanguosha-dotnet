#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
[global::CardGame.Core.ProgramRuntimeModule]
internal sealed partial class CoreMatchSetupDefinitionsRuntime
{
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries _coreModeRulesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries _corePrivateGeneralLibrariesOperations;
    private readonly global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries _coreNativeDamageHandlesOperations;
    private readonly global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess _coreMatchDefinitionModeDefinitionOptionsAccess;
    internal CoreMatchSetupDefinitionsRuntime(global::CardGame.Core.RuntimeEntries.CoreGameEngineModeRulesEntries coreModeRulesOperations, global::CardGame.Core.RuntimeEntries.CoreGameEnginePrivateGeneralLibrariesEntries corePrivateGeneralLibrariesOperations, global::CardGame.Core.RuntimeEntries.CoreNativeDamageHandlesEntries coreNativeDamageHandlesOperations, global::CardGame.Core.EngineStates.Contracts.ICoreMatchDefinitionModeDefinitionOptionsAccess coreMatchDefinitionModeDefinitionOptionsAccess)
    {
        this._coreModeRulesOperations = coreModeRulesOperations;
        this._corePrivateGeneralLibrariesOperations = corePrivateGeneralLibrariesOperations;
        this._coreNativeDamageHandlesOperations = coreNativeDamageHandlesOperations;
        this._coreMatchDefinitionModeDefinitionOptionsAccess = coreMatchDefinitionModeDefinitionOptionsAccess;
    }
}
